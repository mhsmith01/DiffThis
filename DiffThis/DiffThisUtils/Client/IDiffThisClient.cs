using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffThisUtils
{
    /// <summary>
    /// Interface for creating a diffthis client
    /// </summary>
    public interface IDiffThisClient
    {
        /// <summary>
        /// From the source and target file, generate a diff file
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="targetFile"></param>
        /// <param name="diffFile"></param>
        string GenerateDiff(string sourceFile, string targetFile, string diffFile);

        /// <summary>
        /// From the source using the diff file, generate the resulting target file
        /// </summary>
        /// <param name="sourceFile"></param>
        /// <param name="diffFile"></param>
        /// <param name="targetFile"></param>
        void ApplyDiff(string sourceFile, string diffFile, string targetFile);

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
        /// Gets the name of the diffthis client
        /// </summary>
        /// <returns></returns>
        string GetName();
    }
}
