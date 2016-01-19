using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffThisUtils
{
    public interface IDiffThisServer
    {
        void LoadServer(List<IDiffThisClient> clients, List<IDiffThisDisplay> displays);
        
        ServerPriority GetServerPriority(string target);

        string GetSource(string target);

        string GetDiff(string source, string target);

        IDiffThisClient GetDiffThisClient(string source);

        IDiffThisClient GetDiffThisClient(string source, string diff);

        IDiffThisDisplay GetDiffThisDisplay(string source, string diff);

        string GetName();
    }
}
