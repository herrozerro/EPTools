using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPTools.Blazor.Models
{
    public class Aptitude_Template
    {
        public string name { get; set; }
        public string description { get; set; }
        public Aptitude_Template_Aptitude aptitudes { get; set; }
        public string resource { get; set; }
        public string reference { get; set; }
        public string id { get; set; }
    }

    public class Aptitude_Template_Aptitude
    {
        public int cognition { get; set; }
        public int intuition { get; set; }
        public int reflexes { get; set; }
        public int savvy { get; set; }
        public int somatics { get; set; }
        public int willpower { get; set; }
    }
}
