using LameDLLWrap;
using System;

namespace NAudio.Lame
{
	public static class LameDLL
	{
		static LameDLL()
		{
			Loader.Init();
		}

		/// <summary>Lame Version</summary>
		public static string LameVersion => LameDLLImpl.LameVersion;
		/// <summary>Lame Short Version</summary>
		public static string LameShortVersion => LameDLLImpl.LameShortVersion;
		/// <summary>Lame Very Short Version</summary>
		public static string LameVeryShortVersion => LameDLLImpl.LameVeryShortVersion;
		/// <summary>Lame Psychoacoustic Version</summary>
		public static string LamePsychoacousticVersion => LameDLLImpl.LamePsychoacousticVersion;
		/// <summary>Lame URL</summary>
		public static string LameURL => LameDLLImpl.LameURL;
		/// <summary>Lame library bit width - 32 or 64 bit</summary>
		public static string LameOSBitness => LameDLLImpl.LameOSBitness;

		public static LAMEVersion GetLameVersion() => LameDLLImpl.GetLameVersion();
	}

	internal static class LameDLLImpl
	{
		static LameDLLImpl()
		{
			Loader.Init();
		}

		/// <summary>Lame Version</summary>
		internal static string LameVersion => LibMp3Lame.LameVersion;
		/// <summary>Lame Short Version</summary>
		internal static string LameShortVersion => LibMp3Lame.LameShortVersion;
		/// <summary>Lame Very Short Version</summary>
		internal static string LameVeryShortVersion => LibMp3Lame.LameVeryShortVersion;
		/// <summary>Lame Psychoacoustic Version</summary>
		internal static string LamePsychoacousticVersion => LibMp3Lame.LamePsychoacousticVersion;
		/// <summary>Lame URL</summary>
		internal static string LameURL => LibMp3Lame.LameURL;
		/// <summary>Lame library bit width - 32 or 64 bit</summary>
		internal static string LameOSBitness => LibMp3Lame.LameOSBitness;

		/// <summary>Get LAME version information</summary>
		/// <returns>LAME version structure</returns>
		internal static LAMEVersion GetLameVersion()
			=> new LAMEVersion(LibMp3Lame.GetLameVersion());
	}
}
