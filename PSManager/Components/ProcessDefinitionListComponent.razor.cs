using Microsoft.AspNetCore.Components;
using PSDTO.ProcessDefinition;

namespace PSManager.Components
{
    public partial class ProcessDefinitionListComponent
    {
        [Inject]
        private HttpClient HttpClient { get; set; }

        private ProcessDefinitionDTO[]? processes;

        protected override async Task OnInitializedAsync()
        {
            processes = await HttpClient.GetFromJsonAsync<ProcessDefinitionDTO[]>("/PS/GetProcessDefinitions");
        }
    }
}