using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffThisUtils.Server
{
    public class ServerConfig
    {
        // Extension selection
        public ExtensionConfig ServerExtensions;
        public List<ExtensionConfig> ClientExtensions;
        public List<ExtensionConfig> DisplayExtensions;
        
        // File selection
        public List<FileMapping> FileMapping;

        public ServerConfig()
        {
            // TODO: Complete member initialization
        }

        public ServerConfig(ServerConfig secondary)
        {
        }

        public static ServerConfig Load(string filename)
        {
            return null;
        }
    }

    public class ExtensionConfig
    {
        public string Name;
        public string[] Extensions;
    }

    public class FileMapping
    {
        string SourcePath;
        string TargetPath;
        bool Exclude;
    }
}
