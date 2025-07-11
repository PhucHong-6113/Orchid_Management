using OrchidManagement.Client.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace OrchidManagement.Client.Services.Orchid
{
    public class OrchidService
    {
        private readonly HttpClient _httpClient;

        public OrchidService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<OrchidDto>> GetFeaturedOrchidsAsync()
        {
            try
            {
                // Get first 6 orchids for featured section
                var orchids = await _httpClient.GetFromJsonAsync<IEnumerable<OrchidDto>>("api/orchids");
                return orchids?.Take(6) ?? new List<OrchidDto>();
            }
            catch
            {
                return new List<OrchidDto>();
            }
        }
    }
}