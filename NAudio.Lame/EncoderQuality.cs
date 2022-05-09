using System;
using System.Collections.Generic;
using System.Text;

namespace NAudio.Lame
{
	public enum EncoderQuality : int
	{
		/// <summary>
		/// near-best quality, not too slow
		/// </summary>
		High = 2,
		/// <summary>
		/// good quality, fast
		/// </summary>
		Standard = 5,
		/// <summary>
		/// ok quality, really fast
		/// </summary>
		Fast = 7
	}
}
