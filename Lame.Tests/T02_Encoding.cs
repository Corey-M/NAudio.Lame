using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NAudio.Lame;
using NAudio.Wave;

namespace Lame.Tests
{
    [TestClass]
    public class T02_Encoding
    {
        private const string SourceFilename = @"Test.wav";

        [TestMethod]
        public void TC01_EncodeStream()
        {
            Assert.IsTrue(File.Exists(SourceFilename));

            TimeSpan source_time = TimeSpan.Zero;

            using (var mp3data = new MemoryStream())
            {
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
                    // encoding did not supply an ID3 tag, ensure none was present in file
                    Assert.IsNull(encoded.Id3v1Tag);
                    Assert.IsNull(encoded.Id3v2Tag);

                    // the STANDARD lame preset produces a Xing header.  Check it.
                    Assert.IsNotNull(encoded.XingHeader);
                    Assert.AreEqual(mp3data.Length, encoded.XingHeader.Bytes);

                    // confirm that length is a multiple of the block size
                    int blkSize = (encoded.XingHeader.Mp3Frame.SampleCount * encoded.WaveFormat.BitsPerSample * encoded.WaveFormat.Channels) / 8;
                    int calcLength = blkSize * encoded.XingHeader.Frames;
                    Assert.AreEqual(calcLength, encoded.Length);
                }
            }
        }
    }
}
