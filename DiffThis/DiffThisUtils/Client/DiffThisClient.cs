using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffThisUtils.Display
{
    public class DiffThisClient : IDiffThisClient
    {
        public string GenerateDiff(string sourceFile, string targetFile, string diffFile)
        {
            throw new NotImplementedException();
        }

        public void ApplyDiff(string sourceFile, string diffFile, string targetFile)
        {
            throw new NotImplementedException();
        }

        public string[] GetSupportedExtensions()
        {
            return new string[0];
        }

        public string[] GetDiffExtensions()
        {
            return new string[]{"diff"};
        }

        public string GetName()
        {
            return "Notepad";
        }
    }
}
