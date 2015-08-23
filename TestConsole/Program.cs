using NAudio.Lame;
using NAudio.Wave;
using System;

namespace TestConsole
{
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
				Subtitle = "From the Calligraphy theme",
				AlbumArt = System.IO.File.ReadAllBytes(@"disco.png")
			};

			using (var reader = new AudioFileReader(@"test.wav"))
			using (var writer = new LameMP3FileWriter(@"test.mp3", reader.WaveFormat, 128, tag))
			{
				reader.CopyTo(writer);
			}
		}
	}
}
