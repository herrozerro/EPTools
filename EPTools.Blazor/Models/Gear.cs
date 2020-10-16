namespace EPTools.Blazor.Models
{
    public abstract class Gear
    {
        public string category { get; set; }
        public string subcategory { get; set; }
        public string name { get; set; }
        public string complexity { get; set; }
        public string description { get; set; }
        public string id { get; set; }
        public string reference { get; set; }
        public string resource { get; set; }
        public string summary { get; set; }
    }
}