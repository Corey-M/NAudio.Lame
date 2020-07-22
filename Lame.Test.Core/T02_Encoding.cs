using Microsoft.VisualStudio.TestTools.UnitTesting;
using NAudio.Lame;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;

namespace Lame.Test
{
	[TestClass]
	public class T02_Encoding
	{
		private const string SourceFilename = @"Test.wav";

		[TestMethod]
		public void TC01_EncodeStream()
		{
			Assert.IsTrue(File.Exists(SourceFilename));

			using (var mp3data = new MemoryStream())
			{
				TimeSpan source_time = TimeSpan.MinValue;

				// Convert source wave to MP3
				using (var source = new AudioFileReader(SourceFilename))
				using (var mp3writer = new LameMP3FileWriter(mp3data, source.WaveFormat, LAMEPreset.STANDARD))
				{
					source_time = source.TotalTime;
					source.CopyTo(mp3writer);
					Assert.AreEqual(source.Length, source.Position);
				}

				// Open MP3 file and test content
				mp3data.Position = 0;
				using (var encoded = new Mp3FileReader(mp3data))
				{
					var fmt = encoded.WaveFormat;

					// encoding did not supply an ID3 tag, ensure none was present in file
					Assert.IsNull(encoded.Id3v1Tag);
					Assert.IsNull(encoded.Id3v2Tag);

					// the STANDARD lame preset produces a Xing header.  Check it.
					Assert.IsNotNull(encoded.XingHeader);
					Assert.AreEqual(mp3data.Length, encoded.XingHeader.Bytes);

					// confirm that length is a multiple of the block size
					int blkSize = (encoded.XingHeader.Mp3Frame.SampleCount * fmt.BitsPerSample * fmt.Channels) / 8;
					int calcLength = blkSize * encoded.XingHeader.Frames;
					Assert.AreEqual(calcLength, encoded.Length);

					// Check encoded time is less than 1/2 frame different
					var encodedTime = encoded.TotalTime;
					var frameTime = ((double)encoded.XingHeader.Mp3Frame.SampleCount / fmt.SampleRate);
					var diff = Math.Abs(encodedTime.TotalSeconds - source_time.TotalSeconds);
					Assert.IsTrue(diff < frameTime * 1.5d);
				}
			}
		}

		[TestMethod]
		public void TC02_Formats()
		{
			bool TestFormat(WaveFormat format)
			{
				using (var mp3data = new MemoryStream())
				{
					using (var writer = new LameMP3FileWriter(mp3data, format, LAMEPreset.STANDARD))
					{
						var bfr = new byte[format.AverageBytesPerSecond];
						writer.Write(bfr, 0, bfr.Length);
					}

					mp3data.Position = 0;
					using (var reader = new Mp3FileReader(mp3data))
					{
						var encodedFormat = format;
						return
							(encodedFormat.SampleRate == format.SampleRate) &&
							(encodedFormat.Channels == format.Channels);
					}
				}
			}

			Assert.IsTrue(TestFormat(WaveFormat.CreateIeeeFloatWaveFormat(44100, 2)));
			Assert.IsTrue(TestFormat(WaveFormat.CreateIeeeFloatWaveFormat(44100, 1)));
			Assert.IsTrue(TestFormat(WaveFormat.CreateIeeeFloatWaveFormat(22050, 2)));
			Assert.IsTrue(TestFormat(WaveFormat.CreateIeeeFloatWaveFormat(22050, 1)));
			Assert.IsTrue(TestFormat(WaveFormat.CreateIeeeFloatWaveFormat(16000, 2)));
			Assert.IsTrue(TestFormat(WaveFormat.CreateIeeeFloatWaveFormat(16000, 1)));
			Assert.IsTrue(TestFormat(WaveFormat.CreateIeeeFloatWaveFormat(11028, 2)));
			Assert.IsTrue(TestFormat(WaveFormat.CreateIeeeFloatWaveFormat(11025, 1)));
			Assert.IsTrue(TestFormat(WaveFormat.CreateIeeeFloatWaveFormat(8000, 2)));
			Assert.IsTrue(TestFormat(WaveFormat.CreateIeeeFloatWaveFormat(8000, 1)));
		}

		/// <summary>Ensure that VBR presets result in different encoded size.</summary>
		[TestMethod]
		public void TC03_VBRPreset()
		{
			// Presets to test, should result in different sizes for all cases
			var presets = new[] { LAMEPreset.STANDARD, LAMEPreset.EXTREME, LAMEPreset.ABR_160, LAMEPreset.V1, LAMEPreset.V4, LAMEPreset.V6, LAMEPreset.V9 };
			var results = new Dictionary<LAMEPreset, long>();

			WaveFormat sourceFormat;
			using (var srcms = new MemoryStream())
			{
				using (var source = new AudioFileReader(SourceFilename))
				{
					sourceFormat = source.WaveFormat;
					source.CopyTo(srcms);
				}

				foreach (var preset in presets)
				{
					var config = new LameConfig { Preset = preset };
					if (preset == LAMEPreset.STANDARD)
						config.BitRate = 128;

					using (var mp3data = new MemoryStream())
					{
						using (var mp3writer = new LameMP3FileWriter(mp3data, sourceFormat, preset))
						{
							srcms.Position = 0;
							srcms.CopyTo(mp3writer);
						}
						results[preset] = mp3data.Length;
					}
				}
			}
			// Compare encoded sizes for all combinations of presets
			for (int i = 0; i < presets.Length - 2; i++)
			{
				var left = results[presets[i]];
				for (int j = i + 1; j < presets.Length; j++)
				{
					var right = results[presets[j]];

					Assert.AreNotEqual(left, right, $"{presets[i]} size matched {presets[j]}");
				}
			}
		}
	}
}
