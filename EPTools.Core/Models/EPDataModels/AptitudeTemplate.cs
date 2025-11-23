using System.Text.Json.Serialization;

namespace EPTools.Core.Models.EPDataModels
{
    public record AptitudeTemplate(
        string Name,
        string Description,
        AptitudeTemplateAptitudes Aptitudes,
        string Resource,
        string Reference,
        string Id);
    
    public record AptitudeTemplateAptitudes(
        int Cognition,
        int Intuition,
        int Reflexes,
        int Savvy,
        int Somatics,
        int Willpower);
}
