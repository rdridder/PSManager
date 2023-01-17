using Microsoft.AspNetCore.Components;
using Microsoft.Identity.Web;
using PSDTO.Process;
using System.Net.Http.Headers;

namespace PSManager.Components
{
    public partial class ProcessListComponent
    {
        [Inject]
        private HttpClient HttpClient { get; set; }

        [Inject]
        private ITokenAcquisition TokenAcquisition { get; set; }

        private ProcessListDTO[]? processes;

        protected override async Task OnInitializedAsync()
        {
            await PrepareAuthenticatedClient(HttpClient);
            processes = await HttpClient.GetFromJsonAsync<ProcessListDTO[]>("/PS/GetProcessList");
        }

        protected async Task<ProcessDTO> GetProcessDetailsAsync(long processId)
        {
            var process = await HttpClient.GetFromJsonAsync<ProcessDTO>($"/PS/GetProcess?id={processId}");
            return process;
        }

        private async Task PrepareAuthenticatedClient(HttpClient httpClient)
        {
            //You would specify the scopes (delegated permissions) here for which you desire an Access token of this API from Azure AD.
            //Note that these scopes can be different from what you provided in startup.cs.
            //The scopes provided here can be different or more from the ones provided in Startup.cs. Note that if they are different,
            //then the user might be prompted to consent again.
            var accessToken = await TokenAcquisition.GetAccessTokenForUserAsync(new List<string>());
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

    }
}