#region MIT license
// 
// MIT license
//
// Copyright (c) 2013 Corey Murtagh
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
// 
#endregion
using NAudio.Lame;
using NAudio.Wave;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            TranscodingTest();
            ID3Test();
        }

        static void TranscodingTest()
        {
            Codec.WaveToMP3("test.wav", "test1.mp3");
            Codec.MP3ToWave("test1.mp3", "test.wav");
        }

        static void ID3Test()
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

            tag.SetUDT(new[]
            {
                "udf1=First UDF added",
                "udf2=Second UDF",
                "unicode1=Unicode currency symbols: ₠ ₡ ₢ ₣ ₤ ₥ ₦ ₧ ₨ ₩ ₪ ₫"
            });

            Codec.WaveToMP3("test.wav", "test_id3.mp3", tag);
        }
    }

    public static class Codec
    {
        /// <summary>Convert WAV file to MP3 using libmp3lame library</summary>
        /// <param name="waveFileName">Filename of WAV file to convert</param>
        /// <param name="mp3FileName">Filename to save to</param>
        /// <param name="bitRate">MP3 bitrate in kbps, defaults to 128kbps</param>
        /// <remarks>Uses NAudio to read, can read any file compatible with <see cref="NAudio.Wave.AudioFileReader"/></remarks>
        public static void WaveToMP3(string waveFileName, string mp3FileName, int bitRate = 128)
        {
            using (var reader = new AudioFileReader(waveFileName))
            using (var writer = new LameMP3FileWriter(mp3FileName, reader.WaveFormat, bitRate))
                reader.CopyTo(writer);
        }

        /// <summary>Convert WAV file to MP3 using libmp3lame library, setting ID3 tag</summary>
        /// <param name="waveFileName">Filename of WAV file to convert</param>
        /// <param name="mp3FileName">Filename to save to</param>
        /// <param name="tag">ID3 tag data to insert into file</param>
        /// <param name="bitRate">MP3 bitrate in kbps, defaults to 128kbps</param>
        /// <remarks>Uses NAudio to read, can read any file compatible with <see cref="NAudio.Wave.AudioFileReader"/></remarks>
        public static void WaveToMP3(string waveFileName, string mp3FileName, ID3TagData tag, int bitRate = 128)
        {
            byte[] tagBytes = null;

            using (var reader = new AudioFileReader(waveFileName))
            using (var writer = new LameMP3FileWriter(mp3FileName, reader.WaveFormat, 128, tag))
            {
                reader.CopyTo(writer);
                tagBytes = writer.GetID3v2TagBytes();
            }

            if (tagBytes != null)
            {
                var dectag = ID3Decoder.Decode(tagBytes);
            }
        }

        /// <summary>Convert an MP3 file on disk to a WAV file in the same audio format</summary>
        /// <param name="mp3FileName">Filename of MP3 to convert</param>
        /// <param name="waveFileName">Filename to save to</param>
        /// <remarks>All operations use standard NAudio components</remarks>
        public static void MP3ToWave(string mp3FileName, string waveFileName)
        {
            using (var reader = new Mp3FileReader(mp3FileName))
            using (var writer = new WaveFileWriter(waveFileName, reader.WaveFormat))
                reader.CopyTo(writer);
        }
    }
}
