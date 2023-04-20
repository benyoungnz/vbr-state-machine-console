using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace vbr_state_machine_console.Models.AlertDestination
{
    internal class GenericCompare
    {
        public string VBRServer { get; set; }
        public string Parent { get; set; } 
        public string Property { get; set; }
        public string Actual { get; set; } 
        public string Expected { get; set; }
        public bool AlertRequired { get; set; }
        public string Description { get; set; }
    }
}
