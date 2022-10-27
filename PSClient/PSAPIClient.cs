using Microsoft.Extensions.Options;
using PSClient.Options;
using PSDTO.Process;
using PSInterfaces;
using System.Net.Http.Json;
using System.Text.Json;

namespace PSClient
{
    public class PSAPIClient : IPSAPIClient
    {
        private readonly HttpClient _httpClient;

        private readonly PSAPIClientOptions _options;

        public PSAPIClient(HttpClient httpClient, IOptions<PSAPIClientOptions> options)
        {
            _httpClient = httpClient;
            _options = options.Value;
        }

        public async Task<ProcessDTO> GetProcessAsync(long processId)
        {
            var result = await _httpClient.GetFromJsonAsync<ProcessDTO>($"{_options.BaseUrl}/PS/GetProcess?id={processId}");
            return result;
        }

        public async Task<ProcessStatusDTO> FinishProcessTaskAsync(FinishProcessTaskDTO finishProcessTask)
        {
            ProcessStatusDTO? result;
            using (var response = await _httpClient.PostAsJsonAsync($"{_options.BaseUrl}/PS/FinishProcessTask", finishProcessTask))
            {
                if (!response.IsSuccessStatusCode)
                {
                    // TODO handle failure
                }
                using var contentStream = await response.Content.ReadAsStreamAsync();
                // TODO fix nullable / non nullable types
                result = await JsonSerializer.DeserializeAsync<ProcessStatusDTO>(contentStream);
            }
            if (result is null)
            {
                throw new Exception("");
            }
            return result;
        }
    }
}