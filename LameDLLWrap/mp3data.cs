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
#region Attributions
//
// Contents of the LibMp3Lame.NativeMethods class and associated enumerations 
// are directly based on the lame.h available at:
//                https://sourceforge.net/p/lame/svn/6430/tree/trunk/lame/include/lame.h
//
// Source lines and comments included where useful/possible.
//
#endregion
using System.Runtime.InteropServices;

#if X64
using size_t = System.UInt64;
#else
#endif

namespace LameDLLWrap
{
	[StructLayout(LayoutKind.Sequential)]
	public struct mp3data
	{
		/// <summary>1 if header was parsed and following data was computed</summary>
		[MarshalAs(UnmanagedType.Bool)]
		public bool header_parsed;
		/// <summary>number of channels</summary>
		public int stereo;
		/// <summary>sample rate</summary>
		public int samplerate;
		/// <summary>bitrate</summary>
		public int bitrate;
		/// <summary>mp3 frame type</summary>
		public int mode;
		/// <summary>mp3 frame type</summary>
		public int mode_ext;
		/// <summary>number of samples per MP3 frame</summary>
		public int framesize;

		// This data is only computed if mpglib detects a Xing VBR header

		/// <summary>number of samples in MP3 file</summary>
		public ulong nsamp;
		/// <summary>total number of frames in MP3 file</summary>
		public int totalframes;

		// This data is not currently computed by mpglib

		/// <summary>frames decoded counter</summary>
		public int framenum;
	}
}
