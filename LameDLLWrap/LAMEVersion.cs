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
using System;
using System.Runtime.InteropServices;

#if X64
using size_t = System.UInt64;
#else
#endif

namespace LameDLLWrap
{
	/// <summary>LAME DLL version information</summary>
	[StructLayout(LayoutKind.Sequential)]
	public class LAMEVersion
	{
		/* generic LAME version */
		/// <summary>LAME library major version</summary>
		public int major;
		/// <summary>LAME library minor version</summary>
		public int minor;
		/// <summary>LAME library 'Alpha' version flag</summary>
		[MarshalAs(UnmanagedType.Bool)]
		public bool alpha;
		/// <summary>LAME library 'Beta' version flag</summary>
		[MarshalAs(UnmanagedType.Bool)]
		public bool beta;

		/// <summary>Psychoacoustic code major version</summary>
		public int psy_major;
		/// <summary>Psychoacoustic code minor version</summary>
		public int psy_minor;
		/// <summary>Psychoacoustic code 'Alpha' version flag</summary>
		[MarshalAs(UnmanagedType.Bool)]
		public bool psy_alpha;
		/// <summary>Psychoacoustic code 'Beta' version flag</summary>
		[MarshalAs(UnmanagedType.Bool)]
		public bool psy_beta;

#pragma warning disable IDE1006 // Naming Styles

		/* compile time features */
		// const char *features;    /* Don't make assumptions about the contents! */
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Style", "IDE0044:Add readonly modifier", Justification = "Marshalled value")]
		private IntPtr features_ptr = IntPtr.Zero;

		/// <summary>Compile-time features string</summary>
		public string features
		{
			get
			{
				if (features_ptr != IntPtr.Zero)
					return Marshal.PtrToStringAnsi(features_ptr);
				return null;
			}
		}
	}
}
