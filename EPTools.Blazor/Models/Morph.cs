using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPTools.Blazor.Models
{
    public class Morph
    {
        public string name { get; set; }
        public string book { get; set; }
        public string type { get; set; }
        public int cost { get; set; }
        public int availability { get; set; }
        public int wound_threshold { get; set; }
        public int durability { get; set; }
        public int death_rating { get; set; }
        public MorphPools pools { get; set; }
        public List<MorphMovementRates> movement_rate { get; set; }
        public List<string> ware { get; set; }
        public List<MorphTrait> morph_traits { get; set; }
        public List<string> common_extras { get; set; }
        public List<string> notes { get; set; }
        public List<string> common_shape_adjustments { get; set; }
        public string image { get; set; }
        public string description { get; set; }
        public string resource { get; set; }
        public string reference { get; set; }
        public string id { get; set; }
    }

    public class MorphPools
    {
        public int insight { get; set; }
        public int moxie { get; set; }
        public int vigor { get; set; }
        public int flex { get; set; }
    }

    public class MorphMovementRates
    {
        public string movement_type { get; set; }
        public int @base { get; set; }
        public int full { get; set; }
    }

    public class MorphTrait
    {
        public string name { get; set; }
        public int level { get; set; }
    }
}
