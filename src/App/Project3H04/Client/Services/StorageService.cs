using Microsoft.AspNetCore.Components.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Project3H04.Client.Services {
    public class StorageService {
        private readonly HttpClient httpClient;
        private const long maxFileSize = 1024 * 1024 * 10; // 10MB

        public StorageService(HttpClient httpClient) {
            this.httpClient = httpClient;
        }

        public async Task UploadImageAsync(Uri sas, IBrowserFile file) {
            if(sas is null || file is null)
                return;

            var content = new StreamContent(file.OpenReadStream(maxFileSize));
            content.Headers.Add("x-ms-blob-type", "BlockBlob");

            var response = await httpClient.PutAsync(sas, content);
            response.EnsureSuccessStatusCode();
        }

        public async Task UploadImagesAsync(IList<Uri> sases, IList<IBrowserFile> files) {
            Queue<IBrowserFile> fileQueue = new(files);
            Queue<Uri> sasQueue = new(sases);

            while(fileQueue.Count() != 0) {
                await UploadImageAsync(sasQueue.Dequeue(), fileQueue.Dequeue());
            }
        }
    }
}
