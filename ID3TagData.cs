using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public string[] UserDefinedTags;

        /// <summary>Album art - PNG, JPG or GIF file content</summary>
        public byte[] AlbumArt;
	}
}
