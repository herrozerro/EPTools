﻿using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels
{
    public record Interest(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("description")] string Description,
        [property: JsonPropertyName("skills")] List<InterestSkill> Skills,
        [property: JsonPropertyName("resource")] string Resource,
        [property: JsonPropertyName("reference")] string Reference,
        [property: JsonPropertyName("id")] string Id
        );

    public record InterestSkill(
        [property: JsonPropertyName("name")] string Name,
        [property: JsonPropertyName("rating")] int Rating,
        [property: JsonPropertyName("options")] List<string> Options
        );
}
