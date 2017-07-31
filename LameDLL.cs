using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LameDLLWrap;

namespace NAudio.Lame
{
	/// <summary>LAME DLL version information</summary>
	public class LAMEVersion
	{
		/* generic LAME version */

		/// <summary>LAME library major version</summary>
		public int Major { get; private set; }
		/// <summary>LAME library minor version</summary>
		public int Minor { get; private set; }
		/// <summary>LAME library 'Alpha' version flag</summary>
		public bool Alpha { get; private set; }
		/// <summary>LAME library 'Beta' version flag</summary>
		public bool Beta { get; private set; }

		/// <summary>Psychoacoustic code major version</summary>
		public int PsychoAcoustic_Major { get; private set; }
		/// <summary>Psychoacoustic code minor version</summary>
		public int PsychoAcoustic_Minor { get; private set; }
		/// <summary>Psychoacoustic code 'Alpha' version flag</summary>
		public bool PsychoAcoustic_Alpha { get; private set; }
		/// <summary>Psychoacoustic code 'Beta' version flag</summary>
		public bool PsychoAcoustic_Beta { get; private set; }

		/// <summary>Compile-time features string</summary>
		public string Features { get; private set; }

		/// <summary>Constructor, library-local, converts <see cref="LameDLLWrap.LAMEVersion"/></summary>
		/// <param name="source"></param>
		internal LAMEVersion(LameDLLWrap.LAMEVersion source)
		{
			Major = source.major;
			Minor = source.minor;
			Alpha = source.alpha;
			Beta = source.beta;

			PsychoAcoustic_Major = source.psy_major;
			PsychoAcoustic_Minor = source.psy_minor;
			PsychoAcoustic_Alpha = source.psy_alpha;
			PsychoAcoustic_Beta = source.psy_beta;

			Features = source.features;
		}

		// Prevent default construction
		private LAMEVersion() { }
	}

	/// <summary>Static class providing access to context-free LAME entry points</summary>
	public static class LameDLL
	{
		static LameDLL()
		{
			Loader.Init();
		}

		#region DLL version data
		/// <summary>Lame Version</summary>
		public static string LameVersion { get { return LibMp3Lame.LameVersion; } }
		/// <summary>Lame Short Version</summary>
		public static string LameShortVersion { get { return LibMp3Lame.LameShortVersion; } }
		/// <summary>Lame Very Short Version</summary>
		public static string LameVeryShortVersion { get { return LibMp3Lame.LameVeryShortVersion; } }
		/// <summary>Lame Psychoacoustic Version</summary>
		public static string LamePsychoacousticVersion { get { return LibMp3Lame.LamePsychoacousticVersion; } }
		/// <summary>Lame URL</summary>
		public static string LameURL { get { return LibMp3Lame.LameURL; } }
		/// <summary>Lame library bit width - 32 or 64 bit</summary>
		public static string LameOSBitness { get { return LibMp3Lame.LameOSBitness; } }

		/// <summary>Get LAME version information</summary>
		/// <returns>LAME version structure</returns>
		public static NAudio.Lame.LAMEVersion GetLameVersion()
		{
			return new NAudio.Lame.LAMEVersion(LibMp3Lame.GetLameVersion());
		}
		#endregion
	}
}
