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

using System;
using System.Collections.Generic;
using System.Linq;

namespace NAudio.Lame
{
    /// <summary>ID3 tag content</summary>
    public class ID3TagData
	{
		// Standard values:
		/// <summary>Track title (TIT2)</summary>
		public string Title;
		/// <summary>Artist (TPE1)</summary>
		public string Artist;
		/// <summary>Album (TALB)</summary>
		public string Album;
		/// <summary>Year (TYER)</summary>
		public string Year;
		/// <summary>Comment (COMM)</summary>
		public string Comment;
		/// <summary>Genre (TCON)</summary>
		public string Genre;
		/// <summary>Track number (TRCK)</summary>
		public string Track;

		// Experimental:
		/// <summary>Subtitle (TIT3)</summary>
		public string Subtitle;
        /// <summary>AlbumArtist (TPE2)</summary>
        public string AlbumArtist;

        /// <summary>User defined text frames (TXXX) - Multiples are allowed as long as their description is unique (Format : "description=text")</summary>
        /// <remarks>
        /// Obsolete.  Please use <see cref="UserDefinedText"/> property instead.
        /// 
        /// Implemented as accessor to <see cref="UserDefinedText"/> Dictionary.
        /// 
        /// If multiple tags with the same description are supplied only the last one is used.
        /// </remarks>
        [Obsolete("Use the UserDefinedText property instead.", false)]
        public string[] UserDefinedTags
        {
            get => UserDefinedText.Select(kv => $"{kv.Key}={kv.Value}").ToArray();
            set => SetUDT(value);
        }

        /// <summary>User defined text frames (TXXX)</summary>
        /// <remarks>Stored in ID3v2 tag as one TXXX frame per item.</remarks>
        public Dictionary<string, string> UserDefinedText { get; } = new Dictionary<string, string>();

        /// <summary>Album art - PNG, JPG or GIF file content</summary>
        public byte[] AlbumArt;

        /// <summary>
        /// Clear <see cref="UserDefinedText"/> and insret values from collection of "description=text" strings.
        /// </summary>
        /// <param name="data">Collection to load.</param>
        public void SetUDT(IEnumerable<string> data)
        {
            UserDefinedText.Clear();
            foreach (var item in data)
            {
                string key = item.Split('=').First();
                int valuePos = key.Length + 1;
                string val = valuePos > item.Length ? string.Empty : item.Substring(valuePos);
                UserDefinedText[key] = val;
            }
        }
    }
}
