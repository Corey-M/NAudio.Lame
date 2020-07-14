using LameDLLWrap;
using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;

namespace NAudio.Lame
{
	public static class LameDLL
	{
		/// <summary>Attempt to load the appropriate Lame native DLL for the current architecture.</summary>
		/// <param name="rootPaths">Optional array of directories to search for the DLL.</param>
		public static bool LoadNativeDLL(params string[] rootPaths) => LameDLLImpl.Native.LoadNativeDLL(rootPaths);

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

		/// <summary>Utility method for Windows OS detection.</summary>
		internal static bool IsWindowsOS => Environment.OSVersion.Platform == PlatformID.Win32NT;


		internal static class Native
		{
			[DllImport("kernel32", SetLastError = true, CharSet = CharSet.Ansi)]
			static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)] string lpFilename);

			private static IntPtr _hLameDll = IntPtr.Zero;

			private static bool TryLoadLameDLL(FileInfo file)
			{
				if (file == null || !file.Exists)
					return false;
				var handle = LoadLibrary(file.FullName);
				if (handle == IntPtr.Zero)
					return false;
				_hLameDll = handle;
				return true;
			}

			internal static bool LoadNativeDLL(params string[] rootPaths)
			{
				if (_hLameDll != IntPtr.Zero)
					return true;
				if (!LameDLLImpl.IsWindowsOS)
					return false;

				var paths = rootPaths
					.Concat(new[]
					{
						AppDomain.CurrentDomain.BaseDirectory,
						Path.GetDirectoryName(typeof(LameDLL).Assembly.Location)
					}).ToArray();

				var dllname = $"libmp3lame.{(Environment.Is64BitProcess ? "64" : "32")}.dll";

				foreach (var path in paths)
				{
					var file = new DirectoryInfo(path).GetFiles(dllname, SearchOption.AllDirectories).FirstOrDefault();
					if (TryLoadLameDLL(file))
						return true;
				}

				return false;
			}
		}
	}
}
