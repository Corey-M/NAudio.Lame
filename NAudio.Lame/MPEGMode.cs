using System;
using System.Collections.Generic;
using System.Text;

namespace NAudio.Lame
{
	/// <summary>MPEG channel mode</summary>
	public enum MPEGMode : uint
	{
		/// <summary>Stereo</summary>
		Stereo = 0,

		/// <summary>Joint Stereo</summary>
		JointStereo = 1,

		/// <summary>Dual Channel Stereo, like Stereo only... different?</summary>
		// LAME does not support this
		//DualChannel = 2,

		/// <summary>Mono</summary>
		Mono = 3,

		/// <summary>Undefined</summary>
		NotSet = 4
	}
}