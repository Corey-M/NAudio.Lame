using Microsoft.VisualStudio.TestTools.UnitTesting;
using NAudio.Lame;
using System;
using System.Linq;
using System.Reflection;
namespace Lame.Test
{
	[TestClass]
	public class T01_Loader
	{
        /// <summary>
        /// Confirm that the resource DLL is not loaded until needed.
        /// </summary>
        [TestMethod]
        public void TC01_LateLoading()
        {
            if (HaveAssembly("LameDLLWrap"))
            {
                Assert.Inconclusive("The LameDLLWrap assembly was loaded before the Loader test was run.");
            }
            else
            {
                // Provoke loader to extract resource DLL and load assembly
                Assert.IsNotNull(LameDLL.LameVersion);
                Assert.IsNotNull(LameDLL.GetLameVersion());

                // Confirm loading successful
                Assert.IsTrue(HaveAssembly("LameDLLWrap"));
            }
        }

        [TestMethod]
        public void TC02_DLLVersion()
        {
            // GetLameVersion does not return the build number.
            var strVer = LameDLL.LameVersion;
            var ver = LameDLL.GetLameVersion();

            // If build number is 0 then it is omitted.  Add a dummy ".0" at the end
            var verParts = (strVer + ".0").Split('.');
            Assert.IsTrue(verParts.Length >= 3, $"Invalid version string \"{strVer}\"");
            Assert.IsTrue(int.TryParse(verParts[2], out int verBuild), $"Failed to parse build number from \"{strVer}\"");

            // check version is 3.99.5 or higher
            bool versionCheck = ver.Major == 3 && (ver.Minor > 99 || (ver.Minor == 99 && verBuild >= 5));
            Assert.IsTrue(versionCheck, $"Expected LAME dll version >= 3.99.5, found {strVer}");
        }

        /// <summary>
        /// Check if any loaded assembly has the specified name.
        /// </summary>
        /// <param name="name">Name to match in loaded assemblies.</param>
        /// <returns>True if found, else false.</returns>
        private bool HaveAssembly(string name)
        {
            var q =
                from a in AppDomain.CurrentDomain.GetAssemblies()
                let n = a.GetName()
                where string.Compare(name, n.Name, true) == 0
                select n.Name;
            return q.Any();
        }
    }
}
