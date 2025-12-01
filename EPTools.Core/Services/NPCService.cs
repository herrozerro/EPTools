using EPTools.Core.Models.EPDataModels;

namespace EPTools.Core.Services;

public class NpcService(EpDataService dataService)
{
	private readonly EpDataService _epData = dataService;


	public Npc GenerateNpc()
	{
		//Generation Steps
		//1. Choose Archtype
		//2. Choose Morph
		//3. Calculate Derived Stats
		//4. Choose Skill Ranges
		//5. Generate Skills (Archtype Skills allowed to hit upper end of range)
		//6. Choose Gear
		//7. Choose Ware
		//8. Calculate Attacks
		throw new NotImplementedException();
	}
}

public class Npc
{
	public string? Name { get; set; }
	public int Initiative { get; set; }
	public int WoundThreshold { get; set; }
	public int Durability { get; set; }
	public int DamageRating { get; set; }
	public int ThreatPool { get; set; }
	public List<string> Movement { get; set; } = [];
	public List<KeyValuePair<string, int>> Skills { get; set; }= [];
	public List<CharacterAptitude> Aptitudes { get; set; }= [];
	public List<string> Ware { get; set; }= [];
	public List<string> Traits { get; set; }= [];
	public string? Notes { get; set; }
}

public class CharacterAptitude
{
	public string? Name { get; set; }
	public string? Abbreviation { get; set; }
	private int Score { get; init; }
	public int CheckRating => Score * 3;

	public static List<CharacterAptitude> GetAptitudes()
	{
		return GetAptitudes(0, 0, 0, 0, 0, 0);
	}

	private static List<CharacterAptitude> GetAptitudes(int cog, int intu, int refl, int sav, int som, int wil)
	{
		var ls = new List<CharacterAptitude>();

		ls.Add(new CharacterAptitude { Name = "Cognition", Abbreviation = "COG", Score = cog });
		ls.Add(new CharacterAptitude { Name = "Intuition", Abbreviation = "INT", Score = intu });
		ls.Add(new CharacterAptitude { Name = "Reflexes", Abbreviation = "REF", Score = refl });
		ls.Add(new CharacterAptitude { Name = "Savvy", Abbreviation = "SAV", Score = sav });
		ls.Add(new CharacterAptitude { Name = "Somatics", Abbreviation = "SOM", Score = som });
		ls.Add(new CharacterAptitude { Name = "Willpower", Abbreviation = "WIL", Score = wil });

		return ls;
	}

	public static List<CharacterAptitude> GetAptitudeTemplates(AptitudeTemplate template)
	{
		return GetAptitudes(template.Aptitudes.Cognition,
			template.Aptitudes.Intuition,
			template.Aptitudes.Reflexes,
			template.Aptitudes.Savvy,
			template.Aptitudes.Somatics,
			template.Aptitudes.Willpower);
	}
}