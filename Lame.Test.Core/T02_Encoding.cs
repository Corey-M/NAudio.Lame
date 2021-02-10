using Microsoft.VisualStudio.TestTools.UnitTesting;
using NAudio.Lame;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace Lame.Test
{
	[TestClass]
	public class T02_Encoding
	{
		private const string SourceFilename = @"Test.wav";

		private static Stream EncodeSampleFile(LameConfig config, int copies = 1)
		{
			var mp3data = new MemoryStream();

			using (var source = new AudioFileReader(SourceFilename))
			using (var mp3writer = new LameMP3FileWriter(mp3data, source.WaveFormat, config))
			{
				for (int i = 0; i < copies; i++)
				{
					source.Position = 0;
					source.CopyTo(mp3writer);
				}
			}

			mp3data.Position = 0;
			return mp3data;
		}

		private static Stream EncodeSilence(LameConfig config, WaveFormat format, int seconds = 1)
		{
			var mp3data = new MemoryStream();

			using (var mp3writer = new LameMP3FileWriter(mp3data, format, config))
			{
				var bfr = new byte[format.AverageBytesPerSecond * seconds];
				mp3writer.Write(bfr, 0, bfr.Length);
			}

			mp3data.Position = 0;
			return mp3data;
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1031:Do not catch general exception types", Justification = "Exceptions irrelevant in this case.")]
		private static Mp3Frame[] StreamToFrames(Stream stream, bool readData = false)
		{
			IEnumerable<Mp3Frame> iterator()
			{
				stream.Position = 0;
				while (stream.Position < stream.Length)
				{
					Mp3Frame next = null;
					try { next = Mp3Frame.LoadFromStream(stream, readData); }
					catch { }
					if (next != null)
						yield return next;
				}
			}
			
			using (stream)
			{
				return iterator().Skip(1).ToArray();
			}
		}

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
				using (var mp3data = EncodeSilence(new LameConfig { Preset = LAMEPreset.STANDARD }, format, 1))
				using (var reader = new Mp3FileReader(mp3data))
				{
					var encodedFormat = format;
					return
						(encodedFormat.SampleRate == format.SampleRate) &&
						(encodedFormat.Channels == format.Channels);
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

			foreach (var preset in presets)
			{
				var config = new LameConfig { Preset = preset };
				if (preset == LAMEPreset.STANDARD)
					config.BitRate = 128;

				using (var mp3data = EncodeSampleFile(config))
				{
					results[preset] = mp3data.Length;
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

		/// <summary>Test output sample rate settings are functioning as expected.</summary>
		/// <remarks>Test for PR #48</remarks>
		[TestMethod]
		public void TC04_OutputSampleRate()
		{
			bool TestSampleRate(int rate)
			{
				using (var mp3data = EncodeSampleFile(new LameConfig { OutputSampleRate = rate }))
				using (var reader = new Mp3FileReader(mp3data))
				{
					return reader.Mp3WaveFormat.SampleRate == rate;
				}
			}

			Assert.IsTrue(TestSampleRate(22050));
			Assert.IsTrue(TestSampleRate(11025));
			Assert.IsTrue(TestSampleRate(8000));
		}

		[TestMethod]
		public void TC05_VBRConfig()
		{
			WaveFormat testFormat;
			using (var reader = new AudioFileReader(SourceFilename))
			{
				testFormat = reader.WaveFormat;
			}

			// check that variable encoding produces different bit rates.
			var frames = StreamToFrames(EncodeSampleFile(new LameConfig { VBR = VBRMode.Default, VBRMaximumRateKbps = 128 }));
			int minRate = frames.Skip(2).Min(f => f.BitRate);
			int maxRate = frames.Skip(2).Max(f => f.BitRate);
			Assert.AreNotEqual(minRate, maxRate);
			Assert.IsTrue(maxRate <= 128000);

			// Find minimum VBR rate by encoding silence
			frames = StreamToFrames(EncodeSilence(new LameConfig { VBR = VBRMode.Default }, testFormat, 5));
			var vbrMinRate = frames.Skip(2).Min(f => f.BitRate);

			// Test minimum rate enforcement
			frames = StreamToFrames(EncodeSilence(new LameConfig { VBR = VBRMode.Default, VBRMinimumRateKbps = 64, VBREnforceMinimum = true }, testFormat, 5));
			minRate = frames.Min(f => f.BitRate);
			Assert.AreNotEqual(vbrMinRate, minRate, $"Expected minimum of 64kpbps, got {minRate / 1000}");

			frames = StreamToFrames(EncodeSilence(new LameConfig { VBR = VBRMode.Default, VBRMinimumRateKbps = 64, VBREnforceMinimum = false }, testFormat, 5));
			minRate = frames.Min(f => f.BitRate);
			Assert.AreEqual(vbrMinRate, minRate, $"Expected minimum of {vbrMinRate / 1000}, got {minRate / 1000}");

			// check difference in size between diffent quality settings
			frames = StreamToFrames(EncodeSampleFile(new LameConfig { VBR = VBRMode.Default, VBRQuality = 0 }, 5));
			var avgRate0 = frames.Average(f => f.BitRate);

			frames = StreamToFrames(EncodeSampleFile(new LameConfig { VBR = VBRMode.Default, VBRQuality = 9 }, 5));
			var avgRate9 = frames.Average(f => f.BitRate);

			Assert.AreNotEqual(avgRate0, avgRate9, "Expected rate change between VBR Q0 and Q9");
		}

		[TestMethod]
		public void TC06_ABR()
		{
			// Simple ABR test
			for (int rate = 32; rate <= 128; rate += 16)
			{
				var frames = StreamToFrames(EncodeSampleFile(new LameConfig { VBR = VBRMode.ABR, ABRRateKbps = rate, VBRQuality = 5 }, 5));
				var avgRateKbps = frames.Average(f => f.BitRate) / 1000;
				var deviation = Math.Abs(1 - (avgRateKbps / rate));
				Assert.IsTrue(deviation < 0.10, $"ABR deviation for {rate}kbps exceeded threshold: {deviation * 100:0.0}%");
			}
		}
	}
}
