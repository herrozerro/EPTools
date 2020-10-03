using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPTools.Blazor.Models
{
    public class GearPack
    {
        public string name { get; set; }
        public string type { get; set; }
        public List<string> gear { get; set; }
        public List<GearPackOption> options { get; set; }
        public string resource { get; set; }
        public string reference { get; set; }
        public string id { get; set; }
    }

    public class GearPackOption
    {
        public string name { get; set; }
        public List<string> gear { get; set; }
    }
}
