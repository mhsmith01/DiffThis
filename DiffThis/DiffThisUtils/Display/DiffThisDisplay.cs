using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffThisUtils.Display
{
    public abstract class DiffThisDisplay : IDiffThisDisplay
    {
        private FileSystemWatcher sourceFileSystemWatcher;
        private FileSystemWatcher targetFileSystemWatcher;

        protected IDiffThisServer server;
        protected IDiffThisClient client;

        protected string sourceFile;
        protected string diffFile;
        protected string targetFile;
        protected string originalDiffFile;

        public DiffThisDisplay()
        {

        }

        public virtual void StartDiff(IDiffThisServer server, IDiffThisClient client, string sourceFile, string targetFile, string diffFile)
        {
            this.server = server;
            this.client = client;
            this.sourceFile = sourceFile;
            this.targetFile = targetFile;
            this.diffFile = diffFile;
            this.originalDiffFile = diffFile;

            sourceFileSystemWatcher = new FileSystemWatcher(Path.GetDirectoryName(sourceFile), Path.GetFileName(sourceFile));
            targetFileSystemWatcher = new FileSystemWatcher(Path.GetDirectoryName(targetFile), Path.GetFileName(targetFile));

            sourceFileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;
            targetFileSystemWatcher.NotifyFilter = NotifyFilters.LastWrite;

            sourceFileSystemWatcher.EnableRaisingEvents = true;
            targetFileSystemWatcher.EnableRaisingEvents = true;

            sourceFileSystemWatcher.Changed += new FileSystemEventHandler(UpdateSourceFileLocal);
            targetFileSystemWatcher.Changed += new FileSystemEventHandler(UpdateTargetFileLocal);
        }

        private void UpdateSourceFileLocal(object source, FileSystemEventArgs e)
        {
            this.UpdateDiffFileLocal();
            this.UpdateSourceFile(sourceFile);
            this.UpdateDiffFile(diffFile);
        }

        private void UpdateTargetFileLocal(object source, FileSystemEventArgs e)
        {
            this.UpdateDiffFileLocal();
            this.UpdateTargetFile(targetFile);
            this.UpdateDiffFile(diffFile);
        }

        private void UpdateDiffFileLocal()
        {
            if (this.originalDiffFile.Equals(diffFile))
            {
                this.diffFile = Path.GetTempFileName() + "." + Path.GetExtension(this.originalDiffFile);
            }

            this.client.GenerateDiff(this.sourceFile, this.targetFile, this.diffFile);
        }

        protected virtual void UpdateSourceFile(string sourceFile)
        {

        }

        protected virtual void UpdateTargetFile(string targetFile)
        {

        }

        protected virtual void UpdateDiffFile(string diffFile)
        {

        }


        public abstract string[] GetSupportedExtensions();

        public abstract string[] GetDiffExtensions();

        public abstract string GetName();
    }
}
