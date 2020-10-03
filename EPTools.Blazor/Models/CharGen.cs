using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace EPTools.Blazor.Models
{
    public class CharGen
    {
        public string step_name { get; set; }
        public CharGenGuidance guidance { get; set; }
        public string resource { get; set; }
        public string reference { get; set; }
        public string id { get; set; }
    }

    public class CharGenGuidance
    {
        public string heading { get; set; }
        public string text { get; set; }
    }
}
