#NAudio.Lame

Wrapper for `libmp3lame.dll` to add MP3 encoding support to NAudio.

### Experimental Branch

In order to achieve correct handling of both 32- and 64-bit environments - both as specific targets and when compiled with the `AnyCPU` platfom target - without silly duplication of code.  I have split the DLL interface into a new project which is compiled against both x86 and x64 CPU targets.  The resultant assemblies (DLL files) are then added as resources to the `NAudio.Lame` assembly and loaded on demand.

Also extended to support the ID3 methods that are compiled into the lame DLLs.  A new `ID3TagData` class is used to initialize the tag with relevant information.

### Usage

I've included both 32-bit and 64-bit versions of `libmp3lame.dll` (named `libmp3lame.32.dll` and `libmp3lame.64.dll` respectively), both of which will be copied to the output folder.  If you are compiling for a specific CPU target - `x86` or `x64` - then you can remove the unused one from your project.  If compiling for `AnyCPU` then leave them both in.

The `LameMP3FileWriter` class implements a `Stream` that encodes data written to it, writing the encoded MP3 data to either a file or a stream you provide.


#### Example Code:

Here is a very simple codec class to convert a WAV file to and from MP3:

	using System.IO;
	using NAudio.Wave;
	using NAudio.Lame;

	public static class Codec
	{
		// Convert WAV to MP3 using libmp3lame library
		public static void WaveToMP3(string waveFileName, string mp3FileName, int bitRate = 128)
		{
			using (var reader = new AudioFileReader(waveFileName))
			using (var writer = new LameMP3FileWriter(mp3FileName, reader.WaveFormat, bitRate))
				reader.CopyTo(writer);
		}

		// Convert MP3 file to WAV using NAudio classes only
		public static void MP3ToWave(string mp3FileName, string waveFileName)
		{
			using (var reader = new Mp3FileReader(mp3FileName))
			using (var writer = new WaveFileWriter(waveFileName, reader.WaveFormat))
				reader.CopyTo(writer);
		}
	}


### ID3 tag support

The LameMP3FileWriter class now accepts an ID3TagData parameter, allowing you to supply some information 
that will be set as the ID3 tag on the MP3 file.

The `ID3TagData` class is pretty simple right now, with only basic information support.  There are a lot of other bits of information that can potentially be stored in the ID3 tag, with all sorts of interesting ways of encoding the data.  I'll have to play with it some more.

And yes, you *can* add a cover image, in JPG, PNG or GIF format.  It should support up to 128K file size, but seems to have issues with files that size.  The test program shows an example of a smaller file that does work.

#### Usage

	using NAudio.Wave;
	using NAudio.Lame;
	using System;
	
	class Program
	{
		static void Main(string[] args)
		{
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

### To Do List:

- ~~Create a nuget package~~
- Add support for decoding via libmp3lame
- Add [`IMp3FrameDecompressor`][1] implementation for pluggable MP3 decoding

[1]: http://naudio.codeplex.com/SourceControl/latest#NAudio/FileFormats/Mp3/IMp3FrameDecompressor.cs
