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
using System.IO;
using System.Runtime.InteropServices;
using NAudio.Wave;
using LameDLLWrap;
using System.Collections.Generic;

namespace NAudio.Lame
{
	/// <summary>LAME encoding presets</summary>
	public enum LAMEPreset : int
	{
		/*values from 8 to 320 should be reserved for abr bitrates*/
		/*for abr I'd suggest to directly use the targeted bitrate as a value*/

		/// <summary>8-kbit ABR</summary>
		ABR_8 = 8,
		/// <summary>16-kbit ABR</summary>
		ABR_16 = 16,
		/// <summary>32-kbit ABR</summary>
		ABR_32 = 32,
		/// <summary>48-kbit ABR</summary>
		ABR_48 = 48,
		/// <summary>64-kbit ABR</summary>
		ABR_64 = 64,
		/// <summary>96-kbit ABR</summary>
		ABR_96 = 96,
		/// <summary>128-kbit ABR</summary>
		ABR_128 = 128,
		/// <summary>160-kbit ABR</summary>
		ABR_160 = 160,
		/// <summary>256-kbit ABR</summary>
		ABR_256 = 256,
		/// <summary>320-kbit ABR</summary>
		ABR_320 = 320,

		/*Vx to match Lame and VBR_xx to match FhG*/
		/// <summary>VBR Quality 9</summary>
		V9 = 410,
		/// <summary>FhG: VBR Q10</summary>
		VBR_10 = 410,
		/// <summary>VBR Quality 8</summary>
		V8 = 420,
		/// <summary>FhG: VBR Q20</summary>
		VBR_20 = 420,
		/// <summary>VBR Quality 7</summary>
		V7 = 430,
		/// <summary>FhG: VBR Q30</summary>
		VBR_30 = 430,
		/// <summary>VBR Quality 6</summary>
		V6 = 440,
		/// <summary>FhG: VBR Q40</summary>
		VBR_40 = 440,
		/// <summary>VBR Quality 5</summary>
		V5 = 450,
		/// <summary>FhG: VBR Q50</summary>
		VBR_50 = 450,
		/// <summary>VBR Quality 4</summary>
		V4 = 460,
		/// <summary>FhG: VBR Q60</summary>
		VBR_60 = 460,
		/// <summary>VBR Quality 3</summary>
		V3 = 470,
		/// <summary>FhG: VBR Q70</summary>
		VBR_70 = 470,
		/// <summary>VBR Quality 2</summary>
		V2 = 480,
		/// <summary>FhG: VBR Q80</summary>
		VBR_80 = 480,
		/// <summary>VBR Quality 1</summary>
		V1 = 490,
		/// <summary>FhG: VBR Q90</summary>
		VBR_90 = 490,
		/// <summary>VBR Quality 0</summary>
		V0 = 500,
		/// <summary>FhG: VBR Q100</summary>
		VBR_100 = 500,

		/*still there for compatibility*/
		/// <summary>R3Mix quality - </summary>
		R3MIX = 1000,
		/// <summary>Standard Quality</summary>
		STANDARD = 1001,
		/// <summary>Extreme Quality</summary>
		EXTREME = 1002,
		/// <summary>Insane Quality</summary>
		INSANE = 1003,
		/// <summary>Fast Standard Quality</summary>
		STANDARD_FAST = 1004,
		/// <summary>Fast Extreme Quality</summary>
		EXTREME_FAST = 1005,
		/// <summary>Medium Quality</summary>
		MEDIUM = 1006,
		/// <summary>Fast Medium Quality</summary>
		MEDIUM_FAST = 1007
	}

	/// <summary>Delegate for receiving output messages</summary>
	/// <param name="text">Text to output</param>
	public delegate void ReportFunction(string text);

	/// <summary>MP3 encoding class, uses libmp3lame DLL to encode.</summary>
	public class LameMP3FileWriter : Stream
	{
		static LameMP3FileWriter()
		{
			Loader.Init();
		}


		// Ensure that the Loader is initialized correctly
		//static bool init_loader = Loader.Initialized;

		/// <summary>Union class for fast buffer conversion</summary>
		/// <remarks>
		/// <para>
		/// Because of the way arrays work in .NET, all of the arrays will have the same
		/// length value.  To prevent unaware code from trying to read/write from out of
		/// bounds, allocation is done at the grain of the Least Common Multiple of the
		/// sizes of the contained types.  In this case the LCM is 8 bytes - the size of
		/// a double or a long - which simplifies allocation.
		/// </para><para>
		/// This means that when you ask for an array of 500 bytes you will actually get 
		/// an array of 63 doubles - 504 bytes total.  Any code that uses the length of 
		/// the array will see only 63 bytes, shorts, etc.
		/// </para><para>
		/// CodeAnalysis does not like this class, with good reason.  It should never be
		/// exposed beyond the scope of the MP3FileWriter.
		/// </para>
		/// </remarks>
		// uncomment to suppress CodeAnalysis warnings for the ArrayUnion class:
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Portability", "CA1900:ValueTypeFieldsShouldBePortable", Justification = "This design breaks portability, but is never exposed outside the class.  Tested on 32-bit and 64-bit.")]
		[StructLayout(LayoutKind.Explicit)]
		private class ArrayUnion
		{
			/// <summary>Size array in bytes</summary>
			[FieldOffset(0)]
			public readonly int nBytes;

			[FieldOffset(16)]
			public readonly byte[] bytes;

			[FieldOffset(16)]
			public readonly short[] shorts;

			[FieldOffset(16)]
			public readonly int[] ints;

			[FieldOffset(16)]
			public readonly long[] longs;

			[FieldOffset(16)]
			public readonly float[] floats;

			[FieldOffset(16)]
			public readonly double[] doubles;

			// True sizes of the various array types, calculated from number of bytes

			public int nShorts { get { return nBytes / 2; } }
			public int nInts { get { return nBytes / 4; } }
			public int nLongs { get { return nBytes / 8; } }
			public int nFloats { get { return nBytes / 4; } }
			public int nDoubles { get { return nBytes / 8; } }

			/// <summary>Initialize array to hold the requested number of bytes</summary>
			/// <param name="reqBytes">Minimum byte count of array</param>
			/// <remarks>
			/// Since all arrays will have the same apparent count, allocation
			/// is done on the array with the largest data type.  This helps
			/// to prevent out-of-bounds reads and writes by methods that do
			/// not know about the union.
			/// </remarks>
			public ArrayUnion(int reqBytes)
			{
				// Calculate smallest number of doubles required to store the 
				// requested byte count
				int reqDoubles = (reqBytes + 7) / 8;

				this.doubles = new double[reqDoubles];
				this.nBytes = reqDoubles * 8;
			}
		};

		#region Properties
		// LAME library context 
		private LibMp3Lame _lame;

		// Format of input wave data
		private readonly WaveFormat inputFormat;
		
		// Output stream to write encoded data to
		private Stream outStream;

		// Flag to control whether we should dispose of output stream 
		private bool disposeOutput = false;
		#endregion

		#region Structors
		/// <summary>Create MP3FileWriter to write to a file on disk</summary>
		/// <param name="outFileName">Name of file to create</param>
		/// <param name="format">Input WaveFormat</param>
		/// <param name="quality">LAME quality preset</param>
		/// <param name="id3">Optional ID3 data block</param>
		public LameMP3FileWriter(string outFileName, WaveFormat format, NAudio.Lame.LAMEPreset quality, ID3TagData id3 = null)
			: this(File.Create(outFileName), format, quality, id3)
		{
			this.disposeOutput = true;
		}
		
		/// <summary>Create MP3FileWriter to write to supplied stream</summary>
		/// <param name="outStream">Stream to write encoded data to</param>
		/// <param name="format">Input WaveFormat</param>
		/// <param name="quality">LAME quality preset</param>
		/// <param name="id3">Optional ID3 data block</param>
		public LameMP3FileWriter(Stream outStream, WaveFormat format, NAudio.Lame.LAMEPreset quality, ID3TagData id3 = null)
			: base()
		{
			Loader.Init();
			//if (!Loader.Initialized)
			//	Loader.Initialized = false;

			// sanity check
			if (outStream == null)
				throw new ArgumentNullException("outStream");
			if (format == null)
				throw new ArgumentNullException("format");

			// check for unsupported wave formats
			if (format.Channels != 1 && format.Channels != 2)
				throw new ArgumentException(string.Format("Unsupported number of channels {0}", format.Channels), "format");
			if (format.Encoding != WaveFormatEncoding.Pcm && format.Encoding != WaveFormatEncoding.IeeeFloat)
				throw new ArgumentException(string.Format("Unsupported encoding format {0}", format.Encoding.ToString()), "format");
			if (format.Encoding == WaveFormatEncoding.Pcm && format.BitsPerSample != 16)
				throw new ArgumentException(string.Format("Unsupported PCM sample size {0}", format.BitsPerSample), "format");
			if (format.Encoding == WaveFormatEncoding.IeeeFloat && format.BitsPerSample != 32)
				throw new ArgumentException(string.Format("Unsupported Float sample size {0}", format.BitsPerSample), "format");
			if (format.SampleRate < 8000 || format.SampleRate > 48000)
				throw new ArgumentException(string.Format("Unsupported Sample Rate {0}", format.SampleRate), "format");

			// select encoder function that matches data format
			if (format.Encoding == WaveFormatEncoding.Pcm)
			{
				if (format.Channels == 1)
					_encode = encode_pcm_16_mono;
				else
					_encode = encode_pcm_16_stereo;
			}
			else
			{
				if (format.Channels == 1)
					_encode = encode_float_mono;
				else
					_encode = encode_float_stereo;
			}

			// Set base properties
			this.inputFormat = format;
			this.outStream = outStream;
			this.disposeOutput = false;

			// Allocate buffers based on sample rate
			this.inBuffer = new ArrayUnion(format.AverageBytesPerSecond);
			this.outBuffer = new byte[format.SampleRate * 5 / 4 + 7200];

			// Initialize lame library
			this._lame = new LibMp3Lame();

			this._lame.InputSampleRate = format.SampleRate;
			this._lame.NumChannels = format.Channels;

			this._lame.SetPreset((int)quality);

			if (id3 != null)
				ApplyID3Tag(id3);

			this._lame.InitParams();
		}


		/// <summary>Create MP3FileWriter to write to a file on disk</summary>
		/// <param name="outFileName">Name of file to create</param>
		/// <param name="format">Input WaveFormat</param>
		/// <param name="bitRate">Output bit rate in kbps</param>
		/// <param name="id3">Optional ID3 data block</param>
		public LameMP3FileWriter(string outFileName, WaveFormat format, int bitRate, ID3TagData id3 = null)
			: this(File.Create(outFileName), format, bitRate, id3)
		{
			this.disposeOutput = true;
		}

		/// <summary>Create MP3FileWriter to write to supplied stream</summary>
		/// <param name="outStream">Stream to write encoded data to</param>
		/// <param name="format">Input WaveFormat</param>
		/// <param name="bitRate">Output bit rate in kbps</param>
		/// <param name="id3">Optional ID3 data block</param>
		public LameMP3FileWriter(Stream outStream, WaveFormat format, int bitRate, ID3TagData id3 = null)
			: base()
		{
			Loader.Init();
			//if (!Loader.Initialized)
			//	Loader.Initialized = false;

			// sanity check
			if (outStream == null)
				throw new ArgumentNullException("outStream");
			if (format == null)
				throw new ArgumentNullException("format");

			// check for unsupported wave formats
			if (format.Channels != 1 && format.Channels != 2)
				throw new ArgumentException(string.Format("Unsupported number of channels {0}", format.Channels), "format");
			if (format.Encoding != WaveFormatEncoding.Pcm && format.Encoding != WaveFormatEncoding.IeeeFloat)
				throw new ArgumentException(string.Format("Unsupported encoding format {0}", format.Encoding.ToString()), "format");
			if (format.Encoding == WaveFormatEncoding.Pcm && format.BitsPerSample != 16)
				throw new ArgumentException(string.Format("Unsupported PCM sample size {0}", format.BitsPerSample), "format");
			if (format.Encoding == WaveFormatEncoding.IeeeFloat && format.BitsPerSample != 32)
				throw new ArgumentException(string.Format("Unsupported Float sample size {0}", format.BitsPerSample), "format");
			if (format.SampleRate < 8000 || format.SampleRate > 48000)
				throw new ArgumentException(string.Format("Unsupported Sample Rate {0}", format.SampleRate), "format");

			// select encoder function that matches data format
			if (format.Encoding == WaveFormatEncoding.Pcm)
			{
				if (format.Channels == 1)
					_encode = encode_pcm_16_mono;
				else
					_encode = encode_pcm_16_stereo;
			}
			else
			{
				if (format.Channels == 1)
					_encode = encode_float_mono;
				else
					_encode = encode_float_stereo;
			}

			// Set base properties
			this.inputFormat = format;
			this.outStream = outStream;
			this.disposeOutput = false;

			// Allocate buffers based on sample rate
			this.inBuffer = new ArrayUnion(format.AverageBytesPerSecond);
			this.outBuffer = new byte[format.SampleRate * 5 / 4 + 7200];

			// Initialize lame library
			this._lame = new LibMp3Lame();

			this._lame.InputSampleRate = format.SampleRate;
			this._lame.NumChannels = format.Channels;

			this._lame.BitRate = bitRate;

			if (id3 != null)
				ApplyID3Tag(id3);

			this._lame.InitParams();
		}


		// Close LAME instance and output stream on dispose
		/// <summary>Dispose of object</summary>
		/// <param name="final">True if called from destructor, false otherwise</param>
		protected override void Dispose(bool final)
		{
			if (_lame != null && outStream != null)
				Flush();

			if (_lame != null)
			{
				_lame.Dispose();
				_lame = null;
			}

			if (outStream != null && disposeOutput)
			{
				outStream.Dispose();
				outStream = null;
			}

			base.Dispose(final);
		}
		#endregion

		/// <summary>Get internal LAME library instance</summary>
		/// <returns>LAME library instance</returns>
		public LibMp3Lame GetLameInstance()
		{
			return _lame;
		}

		#region Internal encoder operations
		// Input buffer
		private ArrayUnion inBuffer = null;

		/// <summary>Current write position in input buffer</summary>
		private int inPosition;

		/// <summary>Output buffer, size determined by call to Lame.beInitStream</summary>
		protected byte[] outBuffer;

		long InputByteCount = 0;
		long OutputByteCount = 0;

		// encoder write functions, one for each supported input wave format
		
		private int encode_pcm_16_mono()
		{
			return _lame.Write(inBuffer.shorts, inPosition / 2, outBuffer, outBuffer.Length, true);
		}

		private int encode_pcm_16_stereo()
		{
			return _lame.Write(inBuffer.shorts, inPosition / 2, outBuffer, outBuffer.Length, false);
		}

		private int encode_float_mono()
		{
			return _lame.Write(inBuffer.floats, inPosition / 4, outBuffer, outBuffer.Length, true);
		}

		private int encode_float_stereo()
		{
			return _lame.Write(inBuffer.floats, inPosition / 4, outBuffer, outBuffer.Length, false);
		}

		// Selected encoding write function
		delegate int delEncode();
		delEncode _encode = null;

		// Pass data to encoder
		private void Encode()
		{
			// check if encoder closed
			if (outStream == null || _lame == null)
				throw new InvalidOperationException("Output Stream closed.");

			// If no data to encode, do nothing
			if (inPosition < inputFormat.Channels * 2)
				return;

			// send to encoder
			int rc = _encode();

			if (rc > 0)
			{
				outStream.Write(outBuffer, 0, rc);
				OutputByteCount += rc;
			}

			InputByteCount += inPosition;
			inPosition = 0;
		}
		#endregion

		#region Stream implementation
		/// <summary>Write-only stream.  Always false.</summary>
		public override bool CanRead { get { return false; } }
		/// <summary>Non-seekable stream.  Always false.</summary>
		public override bool CanSeek { get { return false; } }
		/// <summary>True when encoder can accept more data</summary>
		public override bool CanWrite { get { return outStream != null && _lame != null; } }

		/// <summary>Dummy Position.  Always 0.</summary>
		public override long Position
		{
			get { return 0; }
			set { throw new NotImplementedException(); }
		}

		/// <summary>Dummy Length.  Always 0.</summary>
		public override long Length
		{
			get { return 0; }
		}

		/// <summary>Add data to output buffer, sending to encoder when buffer full</summary>
		/// <param name="buffer">Source buffer</param>
		/// <param name="offset">Offset of data in buffer</param>
		/// <param name="count">Length of data</param>
		public override void Write(byte[] buffer, int offset, int count)
		{
			while (count > 0)
			{
				int blockSize = Math.Min(inBuffer.nBytes - inPosition, count);
				Buffer.BlockCopy(buffer, offset, inBuffer.bytes, inPosition, blockSize);
				
				inPosition += blockSize;
				count -= blockSize;
				offset += blockSize;

				if (inPosition >= inBuffer.nBytes)
					Encode();
			}
		}

		/// <summary>Finalise compression, add final output to output stream and close encoder</summary>
		public override void Flush()
		{
			// write remaining data
			if (inPosition > 0)
				Encode();

			// finalize compression
			int rc = _lame.Flush(outBuffer, outBuffer.Length);
			if (rc > 0)
				outStream.Write(outBuffer, 0, rc);

			// Cannot continue after flush, so clear output stream
			if (disposeOutput)
				outStream.Dispose();
			outStream = null;
		}

		/// <summary>Reading not supported.  Throws NotImplementedException.</summary>
		/// <param name="buffer"></param>
		/// <param name="offset"></param>
		/// <param name="count"></param>
		/// <returns></returns>
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		/// <summary>Setting length not supported.  Throws NotImplementedException.</summary>
		/// <param name="value">Length value</param>
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		/// <summary>Seeking not supported.  Throws NotImplementedException.</summary>
		/// <param name="offset">Seek offset</param>
		/// <param name="origin">Seek origin</param>
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}
		#endregion

		#region ID3 support
		private void ApplyID3Tag(ID3TagData tag)
		{
			if (tag == null)
				return;

			if (!string.IsNullOrEmpty(tag.Title))
				_lame.ID3SetTitle(tag.Title);
			if (!string.IsNullOrEmpty(tag.Artist))
				_lame.ID3SetArtist(tag.Artist);
			if (!string.IsNullOrEmpty(tag.Album))
				_lame.ID3SetAlbum(tag.Album);
			if (!string.IsNullOrEmpty(tag.Year))
				_lame.ID3SetYear(tag.Year);
			if (!string.IsNullOrEmpty(tag.Comment))
				_lame.ID3SetComment(tag.Comment);
			if (!string.IsNullOrEmpty(tag.Genre))
				_lame.ID3SetGenre(tag.Genre);
			if (!string.IsNullOrEmpty(tag.Track))
				_lame.ID3SetTrack(tag.Track);

			if (!string.IsNullOrEmpty(tag.Subtitle))
				_lame.ID3SetFieldValue(string.Format("TIT3={0}", tag.Subtitle));

			if (tag.AlbumArt != null && tag.AlbumArt.Length > 0 && tag.AlbumArt.Length < 131072)
				_lame.ID3SetAlbumArt(tag.AlbumArt);
		}

		private static Dictionary<int, string> _genres;
		/// <summary>Dictionary of Genres supported by LAME's ID3 tag support</summary>
		public static Dictionary<int, string> Genres
		{
			get
			{
				if (_genres == null)
					_genres = LibMp3Lame.ID3GenreList();
				return _genres;
			}
		}
		#endregion
	}
}
