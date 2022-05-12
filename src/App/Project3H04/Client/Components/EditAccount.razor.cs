using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Append.Blazor.Sidepanel;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Project3H04.Client.Services;
using Project3H04.Shared.DTO;
using Project3H04.Shared.Gebruiker;
using Project3H04.Shared.Kunstenaars;

namespace Project3H04.Client.Components
{
    public partial class EditAccount
    {
        [Parameter] public Gebruiker_DTO Model { get; set; }
        [Parameter] public EventCallback OnRedirect { get; set; }
        //public Kunstenaar_DTO kunstenaar;
        //private Gebruiker_DTO model = new();
        //public Klant_DTO klant { get; set; }
        [Inject] public NavigationManager NavigationManager { get; set; }
        [Inject] public HttpClient httpClient { get; set; }
        [Inject] public ISidepanelService Sidepanel { get; set; }
        [Inject] public StorageService StorageService { get; set; }

        private IBrowserFile newImage;

        /*protected override async Task OnParametersSetAsync()
        {
            Console.WriteLine(GebruikerId + ":gebruikersid");
            Console.WriteLine(gebruiker.Email + " " + "is Geb");
            //Console.WriteLine((gebruiker is Kunstenaar_DTO) + "dees is kunst");

            //Gebruiker_DTO geb;
            //dees api call werkt nie blijkbaar => gwn API call fixen eeuy
            //geb = await httpClient.GetFromJsonAsync<Gebruiker_DTO>($"api/Gebruiker/{GebruikerId}");
            //gebruiker = kunstenaar;

            //model = new Gebruiker_DTO
            //{
            //    GebruikerId = geb.GebruikerId,
            //    Gebruikersnaam = geb.Gebruikersnaam,
            //    GeboorteDatum = geb.GeboorteDatum,
            //    Email = geb.Email,
            //    Fotopad = geb.Fotopad
            //};
            //geen api call nodig eig, gwn de gebruikerDTO als param gegeven
            await Task.Delay(100);

        }*/

        private void OnInputFileChange(InputFileChangeEventArgs e)
        {
            newImage = e.File;
            Model.Fotopad = newImage.Name;
        }


        private async Task EditGebruikerAsync()
        {
            var request = new GebruikerRequest.Edit();

            if (Model.Fotopad == null || Model.Fotopad.Equals(""))
            {
                Model.Fotopad = "/images/anonymous.JPG";
            } else if(newImage is not null)
            {
                //stuff om te uploaden
                request.newImage = true;
                request.newImageName = newImage.Name;
            }
            request.Model = Model;
            var response = await httpClient.PutAsJsonAsync("api/Gebruiker", request);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadFromJsonAsync<GebruikerResponse.Edit>();

            await StorageService.UploadImageAsync(content.Sas, newImage);
            //na edit terug naar account page om geg te zien   
            await OnRedirect.InvokeAsync();
           //StateHasChanged();
        }

    }
}
