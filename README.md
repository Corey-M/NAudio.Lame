# NAudio.Lame

## Description

Wrapper for `libmp3lame.dll` to add MP3 encoding support to NAudio on Windows.

**IMPORTANT:** Because this wraps Windows native DLLs *it will not work on any operating system.*  It may 
function with Windows emulation layers but I have never tested this.

Includes both 32-bit and 64-bit versions of Windows native `libmp3lame.dll` (named `libmp3lame.32.dll` and `libmp3lame.64.dll` respectively), both of which will be copied to the output folder on build.
If you are compiling for a specific CPU target - `x86` or `x64` - then you only need to distribute the appropriate version.

The `LameDLLWrap` project is the interface to both 32-bit and 64-bit version of the native DLLs, and is compiled for both targets.
Both versions are compiled into resources in `NAudio.Lame.dll`.
At runtime the version for the current process bit width is loaded from resources, which then references the appropriate native library.

Please note that native library loading will fail if for any reason the application's binary path is not in the current search path.
This will happen for example in ASP.NET projects.

## Usage

The `LameMP3FileWriter` class implements a `Stream` that encodes data written to it, writing the encoded MP3 data to either a file or a stream you provide.

Note that on .NET Core you must initialize the resource assembly loader before using any other feature of the library:

    Loader.Init();

### Sample Code

Here is a very simple codec class to convert a WAV file to and from MP3:

    using System.IO;
    using NAudio.Wave;
    using NAudio.Lame;

    public static class Codec
    {
        // Convert WAV to MP3 using libmp3lame library
        public static void WaveToMP3(string waveFileName, string mp3FileName, int bitRate = 128)
        {
            // Initialize resource assembly loader
            Loader.Init();

            using (var reader = new AudioFileReader(waveFileName))
            using (var writer = new LameMP3FileWriter(mp3FileName, reader.WaveFormat, bitRate))
                reader.CopyTo(writer);
        }

        // Convert MP3 file to WAV using NAudio classes only
        public static void MP3ToWave(string mp3FileName, string waveFileName)
        {
            // Initialize resource assembly loader
            Loader.Init();

            using (var reader = new Mp3FileReader(mp3FileName))
            using (var writer = new WaveFileWriter(waveFileName, reader.WaveFormat))
                reader.CopyTo(writer);
        }
    }

## ID3 tag support

The LameMP3FileWriter class now accepts an ID3TagData parameter, allowing you to supply some information that will be set as the ID3 tag on the MP3 file.

The `ID3TagData` class is pretty simple right now, with only basic information support.
There are a lot of other bits of information that can potentially be stored in the ID3 tag, with all sorts of interesting ways of encoding the data.
I'll have to play with it some more.

And yes, you *can* add a cover image, in JPG, PNG or GIF format.
LAME can't directly support ID3v2 tags greater than 32KB in size due to internal buffer size constraints, it does allow you to write your own ID3 tags.
The solution to the limit is to write the ID3 tags directly if they are too large for LAME to handle.
Probably a good idea to keep the size reasonable.

### Sample Code

    using NAudio.Wave;
    using NAudio.Lame;
    using System;

    class Program
    {
        static void Main(string[] args)
        {
            // Initialize resource assembly loader
            Loader.Init();

            ID3TagData tag = new ID3TagData 
            {
                Title = "A Test File",
                Artist = "Microsoft",
                Album = "Windows 7",
                Year = "2009",
                Comment = "Test only.",
                Genre = LameMP3FileWriter.Genres[1],
                Subtitle = "From the Calligraphy theme"
            };

            using (var reader = new AudioFileReader(@"test.wav"))
            using (var writer = new LameMP3FileWriter(@"test.mp3", reader.WaveFormat, 128, tag))
            {
                reader.CopyTo(writer);
            }
        }
    }

## Progress Events

As of v1.0.4 there is now an event (`MP3FileWriter.OnProgress`)that you can use to get progress information during the encoding process.  After each call to the LAME encoder the number of bytes in and out are sent to whatever event handler you've attached.  At the end of the process when the encoder is closing it will send the final numbers along with a flag to indicate that the encoding is complete.

Since blocks are encoded very frequently I've added a very simple rate limiter that will skip progress notifications if one has happened within a certain time, except for the final progress event which is always raised.  By default the progress time limit is 100ms, so you should get no more than 10 progress updates per second.  You can raise or lower this by changing the `MP3FileWriter.MinProgressTime` property.  Timing is approximate as it uses `DateTime` to store the last progress timestamp.  Resolution may vary, but expect 15ms to be a fairly common minimum.  Setting `MinProgressTime` to 0 will disable the delay and send you updates for every encoder call.

### Sample Code

    using NAudio.Wave;
    using NAudio.Lame;
    using System;

    class Program
    {
        // For calculation of progress percentage, total bytes to be input
        static long input_length = 0;

        static void Main(string[] args)
        {
            // Initialize resource assembly loader
            Loader.Init();

            using (var reader = new NAudio.Wave.AudioFileReader(@"C:\Temp\TestWave.wav"))
            using (var writer = new NAudio.Lame.LameMP3FileWriter(@"C:\Temp\Encoded.mp3", reader.WaveFormat, NAudio.Lame.LAMEPreset.V3))
            {
                writer.MinProgressTime = 250;
                input_length = reader.Length;
                writer.OnProgress += writer_OnProgress;
                reader.CopyTo(writer);
            }
        }

        static void writer_OnProgress(object writer, long inputBytes, long outputBytes, bool finished)
        {
            string msg = string.Format("Progress: {0:0.0}%, Output: {1:#,0} bytes, Ratio: 1:{2:0.0}",
                (inputBytes * 100.0) / input_length,
                outputBytes,
                ((double)inputBytes) / Math.Max(1, outputBytes));

            Console.Write("\r{0," + (Console.BufferWidth - 1).ToString() + "}\r{1}", "", msg);
            if (finished)
                Console.WriteLine();
        }
    }

## Relase Notes

### Version 1.1.0

Rebuilt as .NET Standard 2.0 to attempt to make this fully compatible with .NET Core on Windows.

Made resource assembly loader public to allow manual initialization on .NET Core.

### Version 1.0.9

Since working on the Unicode stuff I decided I should add some tests.
While working on the initial tests themselves I found a couple of bugs and some ideas for new features.

Released to NuGet 29-Jan-2019.

New Features:

* Added Unicode comment support.
* Started changing `bool` returns in DLL wrapper to capture actual return value to improve availability of error information.
* Deprecated `ID3TagData.UserDefinedTags` and replaced with `Dictionary<>`-backed `ID3TagData.UserDefinedText` property instead.
* Added unit testing for basic functionality.
* Use Debug or Relase version of the `LameDLLWrap` libraries during build, making debugging and testing simpler than just using Release build everywhere.

Bugs Squashed:

* Fixed a UCS-2 decode error in the Unicode support.

### Version 1.0.8

In response to PR #23 I got to looking through the LAME source a lot more.
Took a little more work than I'd like, but Unicode is now partially supported.

While I was at it I fixed a couple of other things.

Released to NuGet 27-Jan-2019.

New Features:

* Added support for Unicode in User-Defined Text frames.
* Added a quick-n-nasty decoder for ID3v2 tags.

Bugs Squashed:

* Changed the way the NuGet package deploys native DLLs, removing powershell script reliance.
Should resolve #25 and #27 and allow the package to work outside of Visual Studio.

## To Do List (indefinitely postponed):

- ~~Create a nuget package~~
- Add support for decoding via libmp3lame
- Add [`IMp3FrameDecompressor`][1] implementation for pluggable MP3 decoding

[1]: http://naudio.codeplex.com/SourceControl/latest#NAudio/FileFormats/Mp3/IMp3FrameDecompressor.cs
[2]: https://sourceforge.net/p/lame/svn/6430/tree/trunk/lame/libmp3lame/id3tag.c#l617