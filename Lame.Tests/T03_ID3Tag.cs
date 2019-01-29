using System;
using System.IO;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NAudio.Lame;
using NAudio.Wave;

namespace Lame.Tests
{
    [TestClass]
    public class T03_ID3Tag
    {
        private const string SourceFilename = @"Test.wav";
        private const string UnicodeTest = @"UnicodeTest=Some unicode characters: Ω≈ç√∫˜µ≤≥÷";

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

        private ID3TagData GetWrittenTag(ID3TagData tag)
        {
            using (var ms = new MemoryStream())
            using (var reader = new AudioFileReader(SourceFilename))
            using (var writer = new LameMP3FileWriter(ms, reader.WaveFormat, LAMEPreset.STANDARD, tag))
            {
                reader.CopyTo(writer);
                writer.Flush();

                return ID3Decoder.Decode(writer.GetID3v2TagBytes());
            }
        }

        [TestMethod]
        public void TC01_CreateTag()
        {
            var srcTag = MakeDefaultTag();
            var tag = GetWrittenTag(srcTag);
            Assert.IsNotNull(tag);

            // confirm elements are the same
            Assert.AreEqual(srcTag.Title, tag.Title);
            Assert.AreEqual(srcTag.Artist, tag.Artist);
            Assert.AreEqual(srcTag.Album, tag.Album);
            Assert.AreEqual(srcTag.Year, tag.Year);
            Assert.AreEqual(srcTag.Comment, tag.Comment);
            Assert.AreEqual(srcTag.Genre, tag.Genre);
            Assert.AreEqual(srcTag.Subtitle, tag.Subtitle);
            Assert.AreEqual(srcTag.AlbumArtist, tag.AlbumArtist);
            Assert.AreEqual(srcTag.UserDefinedText.Count, tag.UserDefinedText.Count);

            foreach (var key in srcTag.UserDefinedText.Keys)
            {
                Assert.IsTrue(tag.UserDefinedText.ContainsKey(key));
                Assert.AreEqual(srcTag.UserDefinedText[key], tag.UserDefinedText[key], $"UDT[{key}] mismatch.");
            }
        }

        [TestMethod]
        public void TC02_UnicodeUDT()
        {
            var srcTag = MakeDefaultTag();
            srcTag.UserDefinedText["unicode"] = UnicodeTest;
            var tag = GetWrittenTag(srcTag);
            Assert.IsNotNull(tag);

            foreach (var key in srcTag.UserDefinedText.Keys)
            {
                Assert.IsTrue(tag.UserDefinedText.ContainsKey(key));
                Assert.AreEqual(srcTag.UserDefinedText[key], tag.UserDefinedText[key], $"UDT[{key}] mismatch.");
            }
        }

        [TestMethod]
        public void TC03_UnicodeComments()
        {
            var srcTag = MakeDefaultTag();
            srcTag.Comment = @"Comment, now available in Unicode 🎤💧";
            var tag = GetWrittenTag(srcTag);
            Assert.IsNotNull(tag);

            Assert.AreEqual(srcTag.Comment, tag.Comment);
        }
    }
}
