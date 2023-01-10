using Microsoft.AspNetCore.Components;
using PSDTO.Process;

namespace PSManager.Components
{
    public partial class ProcessListComponent
    {
        [Inject]
        private HttpClient HttpClient { get; set; }

        private ProcessListDTO[]? processes;

        protected override async Task OnInitializedAsync()
        {
            processes = await HttpClient.GetFromJsonAsync<ProcessListDTO[]>("/PS/GetProcessList");
        }

        protected async Task<ProcessDTO> GetProcessDetailsAsync(long processId)
        {
            var process = await HttpClient.GetFromJsonAsync<ProcessDTO>($"/PS/GetProcess?id={processId}");
            return process;
        }
    }
}