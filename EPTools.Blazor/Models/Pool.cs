using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPTools.Blazor.Models
{
    public class Pool
    {
        public string name { get; set; }
        public List<string> effects { get; set; }
        public List<string> checks { get; set; }
        public string resource { get; set; }
        public string reference { get; set; }
        public string id { get; set; }
    }
}
