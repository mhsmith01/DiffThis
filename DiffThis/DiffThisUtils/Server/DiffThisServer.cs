using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DiffThisUtils.Server
{
    public class DiffThisServer : IDiffThisServer
    {
        ServerConfig rootServerConfig;
        string assemblyRoot;
        public const string RootConfigFileName = "DiffThisServer.config";
        public const string SubConfigFileName = "diff.srv";

        public DiffThisServer()
        {
            assemblyRoot = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);
        }

        public void LoadServer(List<IDiffThisClient> clients, List<IDiffThisDisplay> displays)
        {
            LoadRootServerConfig();

            
        }

        public ServerPriority GetServerPriority(string target)
        {
            throw new NotImplementedException();
        }

        public string GetSource(string target)
        {
            throw new NotImplementedException();
        }

        public string GetDiff(string source, string target)
        {
            throw new NotImplementedException();
        }

        public IDiffThisClient GetDiffThisClient(string source)
        {
            throw new NotImplementedException();
        }

        public IDiffThisClient GetDiffThisClient(string source, string diff)
        {
            throw new NotImplementedException();
        }

        public IDiffThisDisplay GetDiffThisDisplay(string source, string diff)
        {
            throw new NotImplementedException();
        }

        public virtual string GetName()
        {
            return "DefaultServer";
        }

        protected virtual void LoadRootServerConfig()
        {
            string rootFile = Path.Combine(assemblyRoot, RootConfigFileName);

            if (File.Exists(rootFile))
            {
                rootServerConfig = ServerConfig.Load(rootFile);
            }
            else
            {
                rootServerConfig = new ServerConfig();
            }

            foreach(string directory in Directory.EnumerateDirectories(Path.Combine(assemblyRoot, DiffThisCore.PluginPath)))
            {
                string subFile = Path.Combine(directory, SubConfigFileName);
                if(File.Exists(subFile))
                {
                    rootServerConfig = MergeServeConfig(ServerConfig.Load(subFile), rootServerConfig);
                }
            }
        }

        protected virtual ServerConfig MergeServeConfig(ServerConfig primary, ServerConfig secondary)
        {
            ServerConfig merge = new ServerConfig(secondary);


            return merge;
        }

        protected virtual ServerConfig GetRelativeServerConfig(string targetPath)
        {
            string targetDir = Path.GetDirectoryName(targetPath);
            if(Directory.Exists(targetDir))
            {
                Stack<ServerConfig> serverConfigStack = new Stack<ServerConfig>();

                string currentDir = targetDir;
                while (currentDir != null)
                {
                    string subConfigFile = Path.Combine(currentDir, SubConfigFileName);
                    if (File.Exists(subConfigFile))
                    {
                        serverConfigStack.Push(ServerConfig.Load(subConfigFile));
                    }

                    currentDir = Directory.GetParent(currentDir).FullName;
                }

                ServerConfig resultConfig = rootServerConfig;

                while(serverConfigStack.Count > 0)
                {
                    resultConfig = MergeServeConfig(serverConfigStack.Pop(), resultConfig);
                }

                return resultConfig;
            }
            else
            {
                throw new DirectoryNotFoundException(string.Format("Unable for find directory: {0}", targetDir));
            }
        }
    }
}
