using System;
using System.Reflection;

namespace NAudio.Lame
{
	/// <summary>
	/// Resource Assembly Loader
	/// </summary>
	internal static class ResourceAssemblyLoader
	{
		#region State
		internal static bool Initialized = false;
		internal static string LoadedName;
		#endregion

		/// <summary>
		/// Initialize resource assembly loader
		/// </summary>
		public static void Init()
		{
			lock (typeof(ResourceAssemblyLoader))
			{
				if (!Initialized)
				{
					AppDomain.CurrentDomain.AssemblyResolve += LoadLameWrapper;
					Initialized = true;
				}
			}
		}

		private static Assembly LoadLameWrapper(object sender, ResolveEventArgs args)
		{
			var asmName = new AssemblyName(args.Name).Name + ".dll";
			var srcAssembly = typeof(ResourceAssemblyLoader).Assembly;

			// search resources for requested assembly
			byte[] src = null;
			foreach (string resName in srcAssembly.GetManifestResourceNames())
			{
				int p1 = resName.IndexOf(Environment.Is64BitProcess ? "x64" : "x86");
				int p2 = resName.IndexOf(asmName);

				if (p1 < 0 || p2 < 0)
					continue;

				LoadedName = resName;

				using (var strm = srcAssembly.GetManifestResourceStream(resName))
				{
					src = new byte[strm.Length];
					strm.Read(src, 0, (int)strm.Length);
					break;
				}
			}

			if (src == null)
				return null;

			// Attempt to load native DLL from default locations
			LameDLL.LoadNativeDLL();

			return Assembly.Load(src);
		}
	}
}
