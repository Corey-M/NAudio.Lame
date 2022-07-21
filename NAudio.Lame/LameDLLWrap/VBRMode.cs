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

#if X64
using size_t = System.UInt64;
#else
#endif

namespace LameDLLWrap
{
	/// <summary>Variable BitRate Mode</summary>
	public enum VBRMode : uint
	{
		/// <summary>No VBR (Constant Bitrate)</summary>
		Off = 0,
		/// <summary>MT Algorithm (Mark Taylor).  Now same as MTRH</summary>
		MT,
		/// <summary>RH Algorithm (Roger Hegemann)</summary>
		RH,
		/// <summary>ABR - Average Bitrate</summary>
		ABR,
		/// <summary>MTRH Algorithm (Mark Taylor &amp; Roger Hegemann)(</summary>
		MTRH,
		/// <summary>Default algorithm: MTRH</summary>
		Default = MTRH
	}
}
