using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;

namespace EPTools.Blazor.Models
{
    public class GearDrug : Gear
    {
        public string type { get; set; }
        public string application { get; set; }
        public string duration { get; set; }
        public string addiction { get; set; }
    }
}
