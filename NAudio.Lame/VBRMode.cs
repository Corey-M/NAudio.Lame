namespace NAudio.Lame
{
	/// <summary>Variable Bit Rate Mode</summary>
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
