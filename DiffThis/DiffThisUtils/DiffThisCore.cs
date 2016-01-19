using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DiffThisUtils
{
    public class DiffThisCore
    {
        private List<IDiffThisServer> diffServers;
        public const string PluginPath = "plugins";

        public bool DisplayDiff
        {
            get;
            set;
        }

        public DiffThisCore()
        {
            DisplayDiff = true;
            LoadDiffServers();
        }

        public string Diff(string target)
        {
            ValidateFile(target, "target");

            IDiffThisServer server = GetDiffServer(target);
            return Diff(server, server.GetSource(target), target);
        }

        public string Diff(string source, string target)
        {
            ValidateFile(target, "target");
            IDiffThisServer server = GetDiffServer(target);

            return Diff(server, source, target);
        }

        public string Diff(IDiffThisServer server, string source, string target)
        {
            ValidateFile(target, "target");
            ValidateFile(source, "source");

            string diff = server.GetDiff(source, target);
            if(string.IsNullOrWhiteSpace(diff) || !File.Exists(diff))
            {
                IDiffThisClient client;
                if(string.IsNullOrWhiteSpace(diff))
                {
                    client = server.GetDiffThisClient(source);
                    diff = Path.GetTempFileName();
                }
                else
                {
                    client = server.GetDiffThisClient(source, diff);
                }

                diff = client.GenerateDiff(source,target,diff);

                ValidateFile(diff, "diff");

                if(DisplayDiff)
                {
                    ShowDiff(server, client, source, target, diff);
                }

            }
            else
            {
                diff = Diff(server, source, target, diff);
            }

            return diff;
        }

        public string Diff(string source, string target, string diff)
        {
            ValidateFile(target, "target");

            IDiffThisServer server = GetDiffServer(target);

            return Diff(server, source, target, diff);
        }

        public string Diff(IDiffThisServer server, string source, string target, string diff)
        {
            ValidateFile(target, "target");
            ValidateFile(source, "source");

            IDiffThisClient client;
            if (string.IsNullOrWhiteSpace(diff))
            {
                client = server.GetDiffThisClient(source);
                diff = Path.GetTempFileName();
            }
            else
            {
                client = server.GetDiffThisClient(source, diff);
            }

            diff = client.GenerateDiff(source, target, diff);
            ValidateFile(diff, "diff");

            if(DisplayDiff)
            {
                ShowDiff(server, client, source, target, diff);
            }

            return diff;
        }

        private void ShowDiff(IDiffThisServer server, IDiffThisClient client, string source, string target, string diff)
        {
            IDiffThisDisplay display = server.GetDiffThisDisplay(source, diff);

            display.StartDiff(server, client, source, target, diff);
        }

        public void ApplyDiff(string source, string diff, string target)
        {
            ValidateFile(diff, "diff");
            ValidateFile(source, "source");

            IDiffThisServer server = GetDiffServer(target);
            IDiffThisClient client = server.GetDiffThisClient(Path.GetExtension(source), Path.GetExtension(diff));

            client.ApplyDiff(source, diff, target);

            try
            {
                ValidateFile(target, "target");
            }
            catch 
            {
                throw new InvalidOperationException("Unable to verify that diff file has been applied");
            }
        }

        private void LoadDiffServers()
        {
            diffServers = new List<IDiffThisServer>();
            List<IDiffThisDisplay> displays = new List<IDiffThisDisplay>();
            List<IDiffThisClient> clients = new List<IDiffThisClient>();

            string pluginsDir = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase), PluginPath);

            if(!Directory.Exists(pluginsDir))
            {
                Directory.CreateDirectory(pluginsDir);
            }
            
            foreach(string directory in Directory.EnumerateDirectories(pluginsDir))
            {
                foreach(string dll in Directory.EnumerateFiles(directory, "*.plugin"))
                {
                    // Add Signing security to verify dll

                    Assembly plugin = Assembly.LoadFrom(dll);
                    foreach(Type type in plugin.GetTypes())
                    {
                        if(type is IDiffThisServer)
                        {
                            diffServers.Add(Activator.CreateInstance(type) as IDiffThisServer);
                        }
                        else if(type is IDiffThisDisplay)
                        {
                            displays.Add(Activator.CreateInstance(type) as IDiffThisDisplay);
                        }
                        else if(type is IDiffThisClient)
                        {
                            clients.Add(Activator.CreateInstance(type) as IDiffThisClient);
                        }
                    }
                }
            }

            foreach(IDiffThisServer server in diffServers)
            {
                server.LoadServer(clients, displays);
            }
        }

        private IDiffThisServer GetDiffServer(string target)
        {
            IDiffThisServer diffServer = diffServers.Where(server => server.GetServerPriority(target) >= 0).OrderBy(server => server.GetServerPriority(target)).FirstOrDefault();
            
            if(diffServer == null)
            {
                throw new ArgumentException("No diff server found that supports the target file type", "target");
            }

            return diffServer;
        }

        private void ValidateFile(string file, string paramName)
        {
            if (String.IsNullOrWhiteSpace(file))
            {
                throw new ArgumentNullException(paramName, String.Format("{0} cannot be empty", paramName));
            }

            if (!File.Exists(file))
            {
                throw new FileNotFoundException(paramName, String.Format("{0} file was not found at: {1}", paramName, file));
            }
        }
    }
}
