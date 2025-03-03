using EPTools.Core.Interfaces;
using EPTools.Core.Models.Templates;

namespace EPTools.Core.Services
{
    public class StatBlockTemplateService(IFetchService fetchService)
    {
        private IFetchService FetchService { get; set; } = fetchService;

        public async Task<List<StatBlockTemplate>> GetStatBlockTemplates()
        {
            return await FetchService.GetTFromFileAsync<List<StatBlockTemplate>>("StatBlockTemplates");
        }
    }
}
