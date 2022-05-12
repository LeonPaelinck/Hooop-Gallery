using Project3H04.Client.Shared;
using Project3H04.Shared.Kunstenaars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Project3H04.Client.Services
{
    public class KunstenaarService
    {
        private readonly HttpClient authorisedClient;
        private readonly PublicClient publicClient; //Deze http PublicClient gebruiken voor Anonymous--> 
        private const string endpoint = "api/Kunstenaar";

        public KunstenaarService(HttpClient httpClient, PublicClient publicClient)
        {
            this.authorisedClient = httpClient;
            this.publicClient = publicClient;
        }

        public async Task<KunstenaarResponse.Index> GetIndexAsync(KunstenaarRequest.Index request)
        {
            var response = await publicClient.Client.GetFromJsonAsync<KunstenaarResponse.Index>($"api/Kunstenaar?term={request.Term}&take={request.Take}&recentArtists={request.RecentArtists}");
            return response;
        }

        public async Task<KunstenaarResponse.Detail> GetDetailAsync(KunstenaarRequest.Detail request)
        {
            var response = await publicClient.Client.GetFromJsonAsync<KunstenaarResponse.Detail>($"api/Kunstenaar/{request.Id}");
            return response;
        }
    }
}
