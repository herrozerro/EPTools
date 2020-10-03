using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPTools.Blazor.Models
{
    public class GearArmor : Gear
    {
        public string waretype { get; set; }
        public int energy { get; set; }
        public int kenetic { get; set; }
        public bool stackable { get; set; }
    }
}
