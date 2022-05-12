using Domain;
using Project3H04.Server.Data;
using Project3H04.Shared.DTO;
using Project3H04.Shared.Fotos;
using Project3H04.Shared.Gebruiker;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Project3H04.Server.Services {
    public class GebruikerService : IGebruikerService {
        private readonly ApplicationDbcontext dbContext;
        private readonly IStorageService storageService;

        public GebruikerService(ApplicationDbcontext dbContext, IStorageService storageService) {
            this.dbContext = dbContext;
            this.storageService = storageService;
        }

        public async Task<GebruikerResponse.Detail> GetDetailAsync(int id) {
            var gebruiker = await dbContext.Gebruikers.SingleOrDefaultAsync(x => x.GebruikerId == id);
            var gebruiker_DTO = new Gebruiker_DTO {
                Gebruikersnaam = gebruiker.Gebruikersnaam,
                GebruikerId = gebruiker.GebruikerId,
                Details = gebruiker.Details,
                Email = gebruiker.Email,
                Fotopad = gebruiker.FotoPad,
                GeboorteDatum = gebruiker.Geboortedatum
            };

            return new GebruikerResponse.Detail() { Gebruiker = gebruiker_DTO };
        }

        public async Task<GebruikerResponse.Edit> EditAsync(GebruikerRequest.Edit request) {
            var gebruiker_DTO = request.Model;
            GebruikerResponse.Edit response = new();
            var gebruiker = dbContext.Gebruikers.FirstOrDefault(g => g.GebruikerId == gebruiker_DTO.GebruikerId);

            if(request.newImage) {
                var imageFilename = Path.Combine("Kunstenaars", gebruiker_DTO.GebruikerId.ToString(), Guid.NewGuid().ToString() + request.newImageName);
                var imagePath = $"{storageService.StorageBaseUri}{imageFilename}";
                gebruiker_DTO.Fotopad = imagePath;
                response.Sas = storageService.CreateUploadUri(imageFilename);
            }

            gebruiker.Edit(gebruiker_DTO.Gebruikersnaam, gebruiker_DTO.GeboorteDatum/*, gebruiker_DTO.Email*/, gebruiker_DTO.Fotopad, gebruiker_DTO.Details);
            dbContext.Update(gebruiker);
            await dbContext.SaveChangesAsync();

            return response;
        }
    }
}
