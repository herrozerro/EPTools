// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using System.Text.Json.Nodes;
using EPTools.Core.Models.Ego;
using EPTools.Core.Models.LifePathGen;
using EPTools.Core.Services;

Console.WriteLine("Hello, World!");

//modifyText();

var fileFetchService = new FileFetchService();
var epDataService = new EpDataService(fileFetchService);
var egoService = new EgoService(epDataService);
var lifepathService = new LifepathService(epDataService, egoService);

await fileFetchService.GetTFromEpFileAsync<List<LifePathNode>>("LifePathTableSynthmorphs");

Ego e;

var gear = await epDataService.GetAllGear();

while (true)
{
    e = await lifepathService.GenerateEgo();
    await epDataService.GetGearWare();
    Console.WriteLine(e.Name);
}

System.Text.Json.JsonSerializer.Serialize(e);

e.ToString();

void modifyText()
{
    var filePath = "data/EP-Data/morphs.json";
    var jsonData = File.ReadAllText(filePath);
    var jsonArray = JsonNode.Parse(jsonData).AsArray();

    foreach (JsonObject obj in jsonArray)
    {
        if (obj.ContainsKey("morph_traits"))
        {
            var morphTraits = obj["morph_traits"].AsArray();
            var formattedTraits = new JsonArray();

            foreach (var trait in morphTraits)
            {
                var formattedTrait = new JsonObject
                {
                    ["level"] = trait.ToString().Contains("Level 2") ? 2 : trait.ToString().Contains("Level 3") ? 3 : 1, 
                    ["name"] = trait.ToString().Split('(')[0].Trim()
                };
                formattedTraits.Add(formattedTrait);
            }

            obj["morph_traits"] = formattedTraits;
        }
    }

    var modifiedJsonData = jsonArray.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
    File.WriteAllText(filePath, modifiedJsonData);
}