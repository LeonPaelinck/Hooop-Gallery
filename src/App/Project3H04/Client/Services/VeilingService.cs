using Project3H04.Client.Shared;
using Project3H04.Shared.DTO;
using Project3H04.Shared.Veilingen;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Project3H04.Client.Services {
    public class VeilingService {
        private readonly HttpClient authorisedClient;
        private readonly PublicClient publicClient; //Deze http PublicClient gebruiken voor Anonymous--> 
        private const string endpoint = "api/Veiling";

        public VeilingService(HttpClient authorisedClient, PublicClient publicClient) {
            this.authorisedClient = authorisedClient;
            this.publicClient = publicClient;
        }

        public async Task<Veiling_DTO> GetDetailAsync(int kunstwerkId) {
            var response = await authorisedClient.GetFromJsonAsync<Veiling_DTO>($"api/Veiling/GetByKunstwerkId/{kunstwerkId}");
            return response;
        }

        public async Task<bool> AddBodToVeilingAsync(int veilingId, Bod_DTO bod) {
            var response = await authorisedClient.PutAsJsonAsync<Bod_DTO>($"api/Veiling/AddBodToVeiling/{veilingId}", bod);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<bool>();
        }

        
        public async Task CreateVeilingAsync(Veiling_DTO veiling)
        {
            var response = await authorisedClient.PostAsJsonAsync($"api/Veiling/Create/", veiling);
            response.EnsureSuccessStatusCode();
        }


        public async Task EditVeilingAsync(Veiling_DTO veiling)
        {
            var response = await authorisedClient.PutAsJsonAsync($"api/Veiling/Edit/", veiling);
            response.EnsureSuccessStatusCode();
        }
    }
}
