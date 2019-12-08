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
	//	using NAudio.Lame;

	/// <summary>LAME encoding presets</summary>
	internal enum LAMEPreset : int
	{
		//values from 8 to 320 should be reserved for abr bitrates
		//for abr I'd suggest to directly use the targeted bitrate as a value

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

		//Vx to match Lame and VBR_xx to match FhG
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

		// still there for compatibility
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
}
