using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NAudio.Lame;

namespace Lame.Tests
{
    /// <summary>
    /// Tests targetting the resource loader and initializer.
    /// </summary>
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
                Assert.IsNotNull(LameDLL.GetLameVersion());

                // Confirm loading successful
                Assert.IsTrue(HaveAssembly("LameDLLWrap"));
            }
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
