using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffThisUtils
{
    public interface IDiffThisDisplay
    {
        void StartDiff(IDiffThisServer server, IDiffThisClient client, string sourceFile, string targetFile, string diffFile);

        /// <summary>
        /// Get all supported extensions
        /// </summary>
        /// <returns></returns>
        string[] GetSupportedExtensions();

        /// <summary>
        /// Get diff supported extensions
        /// </summary>
        /// <returns></returns>
        string[] GetDiffExtensions();

        /// <summary>
        /// Gets the name of the diff display
        /// </summary>
        /// <returns></returns>
        string GetName();
    }
}
