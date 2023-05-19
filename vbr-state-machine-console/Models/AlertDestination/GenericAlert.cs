using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace vbr_state_machine_console.Models.AlertDestination
{
    internal class GenericAlert
    {
        public string Subject { get; set; }
        public string Message { get; set; } 
        public string Property { get; set; }
        public string VBRServer { get; set; }
        public string Priority { get; set; } 
        public string Parent { get; set; } 

    }
}
