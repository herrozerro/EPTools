using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPTools.Blazor.Models
{
    public class Skill
    {
        public string name { get; set; }
        public string aptitude { get; set; }
        public bool active { get; set; }
        public bool combat { get; set; }
        public bool physical { get; set; }
        public bool technical { get; set; }
        public bool social { get; set; }
        public bool know { get; set; }
        public bool field { get; set; }
        public bool mental { get; set; }
        public bool psi { get; set; }
        public bool vehicle { get; set; }
        public string description { get; set; }
        public List<string> vssample_fields { get; set; }
        public List<string> specializations { get; set; }
        public string resource { get; set; }
        public string reference { get; set; }
        public string id { get; set; }
    }
}
