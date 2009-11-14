using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Scripting.Hosting;

namespace Sspe
{
    /// <summary>
    /// Script information
    /// </summary>
    internal class ScriptInfo
    {
        /// <summary>
        /// The path to the script file.
        /// </summary>
        public string ScriptPath { get; set; }

        /// <summary>
        /// The compiled code.
        /// </summary>
        public CompiledCode CompiledCode { get; set; }

        /// <summary>
        /// The scope assigned to the script.
        /// </summary>
        public ScriptScope ScriptScope { get; set; }

        /// <summary>
        /// The list of topics associated with the script.
        /// </summary>
        public List<string> Topics { get; set; }

        public ScriptInfo()
        {
            Topics = new List<string>();
        }
    }
}
