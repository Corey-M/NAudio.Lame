using Microsoft.VisualStudio.TestTools.UnitTesting;
using NAudio.Lame;
using NAudio.Wave;
using System.IO;
using System.Text;

namespace Lame.Test
{
    [TestClass]
    public class T03_ID3Tag
    {
        [TestMethod]
        public void TC01_CreateTag()
        {
            CheckTagRoundTrip(MakeDefaultTag());
        }

        [TestMethod]
        public void TC02_UnicodeUDT()
        {
            var srcTag = MakeDefaultTag();
            srcTag.UserDefinedText["unicode"] = "Ω≈ç√∫˜µ≤≥÷";
            CheckTagRoundTrip(srcTag);
        }

        [TestMethod]
        public void TC03_UnicodeComments()
        {
            var srcTag = MakeDefaultTag();
            srcTag.Comment = @"Comment, now available in Unicode 🎤💧";
            CheckTagRoundTrip(srcTag);
        }

        [TestMethod]
        public void TC04_APICSize()
        {
            var srcTag = MakeDefaultTag();
            srcTag.AlbumArt = File.ReadAllBytes(@"Record-150.png");
            CheckTagRoundTrip(srcTag);

            srcTag.AlbumArt = File.ReadAllBytes(@"Record-300.png");
            CheckTagRoundTrip(srcTag);

            srcTag.AlbumArt = File.ReadAllBytes(@"Record-600.png");
            CheckTagRoundTrip(srcTag);
        }

        // Issue #42: NullReeferenceException when setting the ID3Tag.  Exception on failure.
        [TestMethod]
        public void Test_Issue42()
        {
            var waveFormat = new WaveFormat();
            var tag = new ID3TagData { Album = "Album" };

            using (var ms = new MemoryStream())
            using (var writer = new LameMP3FileWriter(ms, waveFormat, LAMEPreset.STANDARD, tag))
            {
                byte[] empty = new byte[8192];
                writer.Write(empty, 0, 8192);
                writer.Flush();
            }
        }

        private static ID3TagData MakeDefaultTag()
        {
            var res = new ID3TagData
            {
                Title = "Title",
                Artist = "Artist",
                Album = "Album",
                Year = "1999",
                Comment = "Comment, standard ASCII",
                Genre = @"Other",
                Track = "7",
                Subtitle = "Subtitle",
                AlbumArtist = "AlbumArtist",
            };
            res.SetUDT(new string[]
            {
                @"UDF01=Some simple ASCII text",
                @"Empty=",
            });
            return res;
        }

        // Create an in-memory MP3 file with the supplied ID3v2 tag, then read the tag back from the MP3 file
        private static ID3TagData GetTagAsWritten(ID3TagData tag)
        {
            var waveFormat = new WaveFormat();

            using (var ms = new MemoryStream())
            {
                using (var writer = new LameMP3FileWriter(ms, waveFormat, LAMEPreset.STANDARD, tag))
                {
                    byte[] empty = new byte[8192];
                    writer.Write(empty, 0, 8192);
                    writer.Flush();
                }
                ms.Position = 0;
                return ID3Decoder.Decode(ReadID3v2Tag(ms));
            }
        }

        // Get ID3v2 tag from stream
        private static byte[] ReadID3v2Tag(Stream stream)
        {
            long start = stream.Position;
            byte[] header = new byte[10];

            if (stream.Read(header, 0, 10) != 10)
                return null;

            if (Encoding.ASCII.GetString(header, 0, 3) != "ID3" || header[3] != 3 || header[4] != 0)
                return null;

            int size = 10 + (int)(((uint)header[6] << 21) | ((uint)header[7] << 14) | ((uint)header[8] << 7) | header[9]);
            if (size > stream.Length)
                return null;

            stream.Position = start;
            byte[] result = new byte[size];
            int rc = stream.Read(result, 0, size);
            if (rc != size)
                return null;
            return result;
        }

        // Write tag to file, read it back, then ensure that the two match.
        private static void CheckTagRoundTrip(ID3TagData tag)
        {
            // round-trip the tag
            var newTag = GetTagAsWritten(tag);
            Assert.IsNotNull(newTag);

            // confirm the the various elements are the same
            CompareTags(tag, newTag);
        }

        // Compare the properties of two tags, throw assertion exception on any apparent differences
        private static void CompareTags(ID3TagData left, ID3TagData right)
        {
            Assert.IsNotNull(right);

            // confirm elements are the same
            Assert.AreEqual(left.Title, right.Title, "Title mismatch");
            Assert.AreEqual(left.Artist, right.Artist, "Artist mismatch");
            Assert.AreEqual(left.Album, right.Album, "Album mismatch");
            Assert.AreEqual(left.Year, right.Year, "Year mismatch");
            Assert.AreEqual(left.Comment, right.Comment, "Comment mismatch");
            Assert.AreEqual(left.Genre, right.Genre, "Genre mismatch");
            Assert.AreEqual(left.Subtitle, right.Subtitle, "Subtitle mismatch");
            Assert.AreEqual(left.AlbumArtist, right.AlbumArtist, "AlbumArtist mismatch");
            Assert.AreEqual(left.UserDefinedText.Count, right.UserDefinedText.Count, "UserDefinedText count mismatch");

            foreach (var key in left.UserDefinedText.Keys)
            {
                Assert.IsTrue(right.UserDefinedText.ContainsKey(key));
                Assert.AreEqual(left.UserDefinedText[key], right.UserDefinedText[key], $"UDT[{key}] mismatch.");
            }

            Assert.AreEqual(left.AlbumArt?.Length ?? -1, right.AlbumArt?.Length ?? -1);
            if (left.AlbumArt != null)
            {
                Assert.IsTrue(System.Linq.Enumerable.SequenceEqual(left.AlbumArt, right.AlbumArt));
            }
        }
    }
}
