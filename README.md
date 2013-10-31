#NAudio.Lame

Wrapper for libmp3lame.dll to add MP3 encoding support to NAudio.


### Usage

I've included both versions of libmp3lame.dll.  Copy either `libmp3lame.dll.32bit` or `libmp3lame.dll.64bit` to your application and remove the bit-width suffix.

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
			using (var reader = new WaveFileReader(waveFileName))
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


### To Do List:

- [ ] Create a nuget package
- [ ] Add support for decoding via libmp3lame
- [ ] Add [`IMp3FrameDecompressor`][1] implementation for pluggable MP3 decoding

[1]: http://naudio.codeplex.com/SourceControl/latest#NAudio/FileFormats/Mp3/IMp3FrameDecompressor.cs
