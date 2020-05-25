using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;

namespace NAudio.Lame
{
    /// <summary>
    /// Decoder for ID3v2 tags
    /// </summary>
    public static class ID3Decoder
    {
        /// <summary>
        /// Read an ID3v2 Tag from the current position in a stream.
        /// </summary>
        /// <param name="stream"><see cref="Stream"/> positioned at start of ID3v2 Tag.</param>
        /// <returns><see cref="ID3TagData"/> with tag content.</returns>
        public static ID3TagData Decode(Stream stream)
        {
            byte[] header = new byte[10];
            int rc = stream.Read(header, 0, 10);
            if (rc != 10 || !ValidateTagHeader(header))
                throw new InvalidDataException("Bad ID3 Tag Header");

            // decode size field and confirm range
            int size = DecodeHeaderSize(header, 6);
            if (size < 10 || size >= (1 << 28))
                throw new InvalidDataException($"ID3 header size '{size:#,0}' out of range.");

            // Load entire tag into buffer and parse
            var buffer = new byte[10 + size];
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            rc = stream.Read(buffer, 0, buffer.Length);
#pragma warning restore IDE0059 // Unnecessary assignment of a value
            return InternalDecode(buffer, 0, size, header[5]);
        }

        /// <summary>
        /// Read an ID3v2 Tag from the supplied array.
        /// </summary>
        /// <param name="buffer">Array containing complete ID3v2 Tag.</param>
        /// <returns><see cref="ID3TagData"/> with tag content.</returns>
        public static ID3TagData Decode(byte[] buffer)
        {
            // Check header
            if (!ValidateTagHeader(buffer))
                throw new InvalidDataException("Bad ID3 Tag Header");

            // decode size field and confirm range
            int size = DecodeHeaderSize(buffer, 6);
            if (size < 10 || size > (buffer.Length - 10))
                throw new InvalidDataException($"ID3 header size '{size:#,0}' out of range.");

            // Decode tag content
            return InternalDecode(buffer, 10, size, buffer[5]);
        }

        /// <summary>
        /// Decode frames from ID3 tag
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="offset"></param>
        /// <param name="size"></param>
        /// <param name="flags"></param>
        /// <returns></returns>
        private static ID3TagData InternalDecode(byte[] buffer, int offset, int size, byte flags)
        {
            // copy tag body data into array and remove unsynchronization padding if present
            byte[] bytes = new byte[size];
            Array.Copy(buffer, offset, bytes, 0, size);
            if ((flags & 0x80) != 0)
                bytes = UnsyncBytes(bytes);

            var res = new ID3TagData();
            int pos = 0;

            // skip extended header if present
            if ((flags & 0x40) != 0)
            {
                var ehSize = DecodeBEInt32(bytes, pos);
                pos += ehSize + 4;
            }

            // load all frames from the tag buffer
            for (var frame = ID3FrameData.ReadFrame(bytes, pos, out int frameSize); frameSize > 0 && frame != null; frame = ID3FrameData.ReadFrame(bytes, pos, out frameSize))
            {
                switch (frame.FrameID)
                {
                    case "TIT2":
                        res.Title = frame.ParseString();
                        break;
                    case "TPE1":
                        res.Artist = frame.ParseString();
                        break;
                    case "TALB":
                        res.Album = frame.ParseString();
                        break;
                    case "TYER":
                        res.Year = frame.ParseString();
                        break;
                    case "COMM":
                        res.Comment = frame.ParseCommentText();
                        break;
                    case "TCON":
                        res.Genre = frame.ParseString();
                        break;
                    case "TRCK":
                        res.Track = frame.ParseString();
                        break;
                    case "TIT3":
                        res.Subtitle = frame.ParseString();
                        break;
                    case "TPE2":
                        res.AlbumArtist = frame.ParseString();
                        break;
                    case "TXXX":
                    {
                        var udt = frame.ParseUserDefinedText();
                        res.UserDefinedText[udt.Key] = udt.Value;
                        break;
                    }
                    case "APIC":
                    {
                        var pic = frame.ParseAPIC();
                        res.AlbumArt = pic?.ImageBytes;
                        break;
                    }
                    default:
                        break;
                }

                pos += frameSize;
            }

            return res;
        }

        /// <summary>
        /// Check ID3v2 tag header is correctly formed
        /// </summary>
        /// <param name="buffer">Array containing ID3v2 header</param>
        /// <returns>True if checks pass, else false</returns>
        private static bool ValidateTagHeader(byte[] buffer)
            => buffer?.Length >= 4 && buffer[0] == 'I' && buffer[1] == 'D' && buffer[2] == '3' && buffer[3] == 3 && buffer[4] == 0;

        /// <summary>
        /// Decode a 28-bit integer stored in the low 7 bits of 4 bytes at the offset, most-significant bits first (big-endian).
        /// </summary>
        /// <param name="buffer">Array containing value to decode.</param>
        /// <param name="offset">Offset in array of the 4 bytes containing the value.</param>
        /// <returns>Decoded value.</returns>
        private static int DecodeHeaderSize(byte[] buffer, int offset)
            => (int)(
                ((uint)buffer[offset] << 21) |
                ((uint)buffer[offset + 1] << 14) |
                ((uint)buffer[offset + 2] << 7) |
                buffer[offset + 3]
            );

        /// <summary>
        /// Read 16-bit integer from <paramref name="buffer"/> as 2 big-endian bytes at <paramref name="offset"/>.
        /// </summary>
        /// <param name="buffer">Byte array containing value.</param>
        /// <param name="offset">Offset in byte array to start of value.</param>
        /// <returns>16-bit integer value.</returns>
        private static short DecodeBEInt16(byte[] buffer, int offset)
            => (short)((buffer[offset] << 8) | buffer[offset + 1]);

        /// <summary>
        /// Read 32-bit integer from <paramref name="buffer"/> as 4 big-endian bytes at <paramref name="offset"/>.
        /// </summary>
        /// <param name="buffer">Byte array containing value.</param>
        /// <param name="offset">Offset in byte array to start of value.</param>
        /// <returns>32-bit integer value.</returns>
        private static int DecodeBEInt32(byte[] buffer, int offset)
            => ((buffer[offset] << 24) | (buffer[offset + 1] << 16) | (buffer[offset + 2] << 8) | (buffer[offset + 3]));

        /// <summary>
        /// Remove NUL bytes inserted by 'unsynchronisation' of data buffer.
        /// </summary>
        /// <param name="buffer">Buffer with 'unsynchronized' data.</param>
        /// <returns>New array with insertions removed.</returns>
        private static byte[] UnsyncBytes(IEnumerable<byte> buffer)
        {
            IEnumerable<byte> ProcessBuffer()
            {
                byte prev = 0;
                foreach (var b in buffer)
                {
                    if (b != 0 || prev != 0xFF)
                        yield return b;
                    prev = b;
                }
            }
            return ProcessBuffer().ToArray();
        }

        /// <summary>
        /// Represents an ID3 frame read from the tag.
        /// </summary>
        private class ID3FrameData
        {
            /// <summary>
            /// Four-character Frame ID.
            /// </summary>
            public readonly string FrameID;

            /// <summary>
            /// Size of the frame in bytes, not including the header.  Should equal the size of the Data buffer.
            /// </summary>
            public readonly int Size;

            /// <summary>
            /// Frame header flags.
            /// </summary>
            public readonly short Flags;

            /// <summary>
            /// Frame content as bytes.
            /// </summary>
            public readonly byte[] Data;

            // private constructor
            private ID3FrameData(string frameID, int size, short flags, byte[] data)
            {
                FrameID = frameID;
                Size = size;
                Flags = flags;
                Data = data;
            }

            /// <summary>
            /// Read an ID3v2 content frame from the supplied buffer.
            /// </summary>
            /// <param name="buffer">Array containing content frame data.</param>
            /// <param name="offset">Offset of start of content frame data.</param>
            /// <param name="size">Output: total bytes consumed by frame, including header, or -1 if no frame available.</param>
            /// <returns><see cref="ID3FrameData"/> with frame, or null if no frame available.</returns>
            public static ID3FrameData ReadFrame(byte[] buffer, int offset, out int size)
            {
                size = -1;
                if ((buffer.Length - offset) <= 10)
                    return null;

                // Extract header data
                string frameID = Encoding.ASCII.GetString(buffer, offset, 4);
                int frameLength = DecodeBEInt32(buffer, offset + 4);
                short frameFlags = DecodeBEInt16(buffer, offset + 8);

                // copy frame content to byte array
                byte[] content = new byte[frameLength];
                Array.Copy(buffer, offset + 10, content, 0, frameLength);

                // Decompress if necessary
                if ((frameFlags & 0x80) != 0)
                {
                    using (var ms = new MemoryStream())
                    using (var dec = new DeflateStream(new MemoryStream(content), CompressionMode.Decompress))
                    {
                        dec.CopyTo(ms);
                        content = ms.ToArray();
                    }
                }

                // return frame
                size = 10 + frameLength;
                return new ID3FrameData(frameID, frameLength, frameFlags, content);
            }

            /// <summary>
            /// Read an ASCII string from an array, NUL-terminated or optionally end of buffer.
            /// </summary>
            /// <param name="buffer">Array containing ASCII string.</param>
            /// <param name="offset">Start of string in array.</param>
            /// <param name="requireTerminator">If true then fail if no terminator found.</param>
            /// <returns>String from buffer, string.Empty if 0-length, null on failure.</returns>
            private static string GetASCIIString(byte[] buffer, ref int offset, bool requireTerminator)
            {
                int start = offset;
                int position = offset;

                for (; position < buffer.Length && buffer[position] != 0; position++) ;
                if (requireTerminator && position >= buffer.Length)
                    return null;
                int length = position - start;
                offset = position + 1;
                return length < 1 ? string.Empty : Encoding.ASCII.GetString(buffer, start, length);
            }

            /// <summary>
            /// Read a Unicode string from an array, NUL-terminated or optionally end of buffer.
            /// </summary>
            /// <param name="buffer">Array containing ASCII string.</param>
            /// <param name="offset">Start of string in array.</param>
            /// <param name="requireTerminator">If true then fail if no terminator found.</param>
            /// <returns>String from buffer, string.Empty if 0-length, null on failure.</returns>
            private static string GetUnicodeString(byte[] buffer, ref int offset, bool requireTerminator = true)
            {
                int start = offset;
                int position = offset;

                for (; position < buffer.Length - 1 && (buffer[position] != 0 || buffer[position + 1] != 0); position += 2) ;
                if (requireTerminator && position >= buffer.Length)
                    return null;

                int length = position - start;
                offset = position + 2;
                string res = LameDLLWrap.UCS2.GetString(buffer, start, length);
                return res;
            }

            delegate string delGetString(byte[] buffer, ref int offset, bool requireTeminator);

            private delGetString GetGetString()
            {
                byte encoding = Data[0];
                if (encoding == 0)
                    return GetASCIIString;
                if (encoding == 1)
                    return GetUnicodeString;
                throw new InvalidDataException($"Invalid string encoding: {encoding}");
            }

            /// <summary>
            /// Parse the frame content as a string.
            /// </summary>
            /// <returns>String content, string.Empty if 0-length.</returns>
            /// <exception cref="InvalidDataException">Invalid string encoding.</exception>
            public string ParseString()
            {
                int position = 1;
                return GetGetString()(Data, ref position, false);
            }

            /// <summary>
            /// Parse the frame content as a Comment (COMM) frame, return comment text only.
            /// </summary>
            /// <returns>Comment text only.  Language and short description omitted.</returns>
            public string ParseCommentText()
            {
                var getstr = GetGetString();
                int position = 1;

                string language = Encoding.ASCII.GetString(Data, position, 3);
                position += 3;
                string shortdesc = getstr(Data, ref position, true);
                string comment = getstr(Data, ref position, false);

                return comment;
            }

            /// <summary>
            /// Parse the frame content as a User-Defined Text Information (TXXX) frame.
            /// </summary>
            /// <returns><see cref="KeyValuePair{TKey, TValue}"/> with content, or exception on error.</returns>
            public KeyValuePair<string, string> ParseUserDefinedText()
            {
                byte encoding = Data[0];
                delGetString getstring;
                if (encoding == 0)
                    getstring = GetASCIIString;
                else if (encoding == 1)
                    getstring = GetUnicodeString;
                else
                    throw new InvalidDataException($"Unknown string encoding: {encoding}");

                int position = 1;
                string description = getstring(Data, ref position, true);
                string value = getstring(Data, ref position, false);
                return new KeyValuePair<string, string>(description, value);
            }

            /// <summary>
            /// Parse the frame content as an attached picture (APIC) frame.
            /// </summary>
            /// <returns><see cref="APICData"/> object </returns>
            public APICData ParseAPIC()
            {
                if (FrameID != "APIC")
                    return null;
                var getstr = GetGetString();

                // get attributes
                int position = 1;
                string mime = getstr(Data, ref position, true);
                byte type = Data[position++];
                string description = getstr(Data, ref position, true);

                // get image content
                int datalength = Data.Length - position;
                byte[] imgdata = new byte[datalength];
                Array.Copy(Data, position, imgdata, 0, datalength);

                return new APICData
                {
                    MIMEType = mime,
                    ImageType = type,
                    Description = description,
                    ImageBytes = imgdata,
                };
            }

            /// <summary>
            /// Data for an Attached Picture (APIC) frame.
            /// </summary>
            public class APICData
            {
                /// <summary>
                /// MIME type of contained image 
                /// </summary>
                public string MIMEType;

                /// <summary>
                /// Type of image.  Refer to http://id3.org/id3v2.3.0#Attached_picture for list of values.
                /// </summary>
                public byte ImageType;

                /// <summary>
                /// Picture description.
                /// </summary>
                public string Description;

                /// <summary>
                /// Picture file content.
                /// </summary>
                public byte[] ImageBytes;
            }
        }
    }
}
