namespace EPTools.Core.Models.Ego
{
    public class Morph
    {
        public string Name { get; set; } = string.Empty;
        public string MorphType { get; set; } = string.Empty;
        public string MorphSex { get; set; } = string.Empty;
        public string Size { get; set; } = string.Empty;
        public int Vigor { get; set; }
        public int Insight { get; set; }
        public int Moxie { get; set; }
        public int MorphFlex { get; set; }
        public bool ActiveMorph { get; set; }
        

        public List<Trait> Traits { get; set; } = new List<Trait>();
        public List<Ware> Wares { get; set; } = new List<Ware>();
    }
}
