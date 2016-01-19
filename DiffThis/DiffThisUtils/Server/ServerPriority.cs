using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiffThisUtils
{
    public enum ServerPriority
    {
        /// <summary>
        ///  Do not use server
        /// </summary>
        None,

        /// <summary>
        /// Only use server if no other options available
        /// </summary>
        Low,

        /// <summary>
        /// Use server normaly
        /// </summary>
        Normal,

        /// <summary>
        /// Use server before any other option
        /// </summary>
        High
    }
}
