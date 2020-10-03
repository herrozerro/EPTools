using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPTools.Blazor.Models
{
    public class Interest
    {
        public string name { get; set; }
        public string description { get; set; }
        public List<InterestSkill> skills { get; set; }
        public string resource { get; set; }
        public string reference { get; set; }
        public string id { get; set; }
    }

    public class InterestSkill
    {
        public string name { get; set; }
        public int rating { get; set; }
        public List<string> options { get; set; }
    }
}
