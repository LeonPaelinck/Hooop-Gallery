using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Project3H04.Client.Shared {
    public class PublicClient {
        public HttpClient Client { get; }

        public PublicClient(HttpClient httpClient) {
            Client = httpClient;
        }
    }
}
