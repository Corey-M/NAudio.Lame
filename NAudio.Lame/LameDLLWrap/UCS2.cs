using System.IO;
using System.Text;

namespace LameDLLWrap
{
    /// <summary>
    /// Utility class for encoding/decoding UCS-2 strings with optional BOM and terminator
    /// </summary>
    public static class UCS2
    {
        static readonly byte[] _terminator = new byte[] { 0, 0 };

        /// <summary>
        /// Get UCS-2 byte sequence for the supplied string.
        /// </summary>
        /// <param name="text">String to encode.</param>
        /// <param name="addBOM">If true prepend Byte Order Marker.</param>
        /// <param name="addTerminator">If true append NUL terminator bytes.</param>
        /// <returns>Encoded byte array.</returns>
        public static byte[] GetBytes(string text, bool addBOM = true, bool addTerminator = true)
        {
            var enc = Encoding.Unicode;

            using (var ms = new MemoryStream())
            using (var wri = new BinaryWriter(ms))
            {
                if (addBOM)
                    wri.Write(enc.GetPreamble());
                wri.Write(enc.GetBytes(text));
                if (addTerminator)
                    wri.Write(_terminator);
                wri.Flush();
                return ms.ToArray();
            }
        }

        private static Encoding UCS = null;

        /// <summary>
        /// Get String from supplied UCS-2 byte sequence with optional BOM and nul terminator bytes.
        /// </summary>
        /// <param name="bytes">Buffer to read UCS-2 byte sequence from.</param>
        /// <param name="offset">Offset in buffer to start of UCS-2 bytes.</param>
        /// <param name="length">Length of UCS-2 bytes in buffer.</param>
        /// <returns>Decoded string.</returns>
        public static string GetString(byte[] bytes, int offset = 0, int length = int.MaxValue)
        {
            Encoding enc = Encoding.Unicode;
            if (length > (bytes.Length - offset))
                length = bytes.Length - offset;

            if (length >= 2 && bytes[length - 2] == 0 && bytes[length - 1] == 0)
                length += 2;

            if (length >= 2)
            {
                if (bytes[offset] == 0xFF && bytes[offset + 1] == 0xFE)
                {
                    // Content is a little-endian Unicode string.  Use standard Unicode encoder.
                    offset += 2;
                    length -= 2;
                }
                else if (bytes[offset] == 0xFE && bytes[offset + 1] == 0xFF)
                {
                    // This is big-endian Unicode.  Use CP1201 to decode.
                    if (UCS == null)
                        UCS = Encoding.GetEncoding(1201);
                    enc = UCS;
                    offset += 2;
                    length -= 2;
                }
            }

            return enc.GetString(bytes, offset, length);
        }
    }
}
