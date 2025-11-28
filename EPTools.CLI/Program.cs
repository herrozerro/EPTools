// See https://aka.ms/new-console-template for more information

using System.Text.Json;
using System.Text.Json.Nodes;
using EPTools.Core.Models.LifePathGen;
using EPTools.Core.Services;

Console.WriteLine("Hello, World!");

//modifyText();

FileFetchService fileFetchService = new FileFetchService();
EpDataService epDataService = new EpDataService(fileFetchService);
EgoService egoService = new EgoService(epDataService);
LifepathService lifepathService = new LifepathService(epDataService, egoService);

await fileFetchService.GetTFromEpFileAsync<List<LifePathNode>>("LifePathTableSynthmorphs");

var e = await lifepathService.GenerateEgo();

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
    string filePath = "data/EP-Data/morphs.json";
    string jsonData = File.ReadAllText(filePath);
    JsonArray jsonArray = JsonNode.Parse(jsonData).AsArray();

    foreach (JsonObject obj in jsonArray)
    {
        if (obj.ContainsKey("morph_traits"))
        {
            JsonArray morphTraits = obj["morph_traits"].AsArray();
            JsonArray formattedTraits = new JsonArray();

            foreach (JsonNode trait in morphTraits)
            {
                JsonObject formattedTrait = new JsonObject
                {
                    ["level"] = trait.ToString().Contains("Level 2") ? 2 : trait.ToString().Contains("Level 3") ? 3 : 1, 
                    ["name"] = trait.ToString().Split('(')[0].Trim()
                };
                formattedTraits.Add(formattedTrait);
            }

            obj["morph_traits"] = formattedTraits;
        }
    }

    string modifiedJsonData = jsonArray.ToJsonString(new JsonSerializerOptions { WriteIndented = true });
    File.WriteAllText(filePath, modifiedJsonData);
}