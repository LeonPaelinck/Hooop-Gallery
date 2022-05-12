using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; //=>>>>>>>>altijd deze usen !!!
using Project3H04.Server.Data;
using Project3H04.Shared;
using Project3H04.Shared.DTO;
using Project3H04.Shared.Fotos;
using Project3H04.Shared.Kunstenaars;
using Project3H04.Shared.Kunstwerken;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Project3H04.Server.Services {
    public class KunstwerkService : IKunstwerkService {
        private readonly ApplicationDbcontext dbContext;
        private readonly IStorageService storageService;

        public KunstwerkService(ApplicationDbcontext dbContext, IStorageService storageService) {
            this.dbContext = dbContext;
            this.storageService = storageService;
        }

        public async Task<int> GetAantalKunst() {
            return await dbContext.Kunstwerken.CountAsync();
        }

        public async Task<KunstwerkResponse.Detail> GetDetailAsync(int id) {
            var kunstwerk = await dbContext.Kunstwerken.Include(k => k.Fotos).Select(x => new Kunstwerk_DTO.Detail {
                Id = x.Id,
                Naam = x.Naam,
                Fotos = (List<Foto_DTO>)x.Fotos.Select(x => new Foto_DTO { Id = x.Id, Naam = x.Naam, Locatie = x.Locatie, Uploaded = true }),
                Kunstenaar = new Kunstenaar_DTO {
                    Gebruikersnaam = x.Kunstenaar.Gebruikersnaam,
                    GebruikerId = x.Kunstenaar.GebruikerId,
                    Email = x.Kunstenaar.Email //email toegevoegd
                },
                Prijs = x.Prijs,
                Lengte = x.Lengte,
                Breedte = x.Breedte,
                Hoogte = x.Hoogte,
                Gewicht = x.Gewicht ?? default(decimal),
                Beschrijving = x.Beschrijving,
                Materiaal = x.Materiaal,
                TeKoop = x.TeKoop,
                IsVeilbaar = x.IsVeilbaar
            }).SingleOrDefaultAsync(x => x.Id == id);
            var response = new KunstwerkResponse.Detail() { Kunstwerk = kunstwerk };
            return response;
        }

        //.EntityFrameworkCore; //=>>>>>>>>altijd deze usen !!!
        public async Task<KunstwerkResponse.Index> GetKunstwerken(Kunstwerk_DTO.Filter request) {
            var respons = new KunstwerkResponse.Index();
            
            var kunstwerken = await dbContext.Kunstwerken.Include(k=>k.Fotos)
                .Where(x => request.Materiaal == null || request.Materiaal.Contains(x.Materiaal))
                .Where(x => request.Grootte == null || (request.Grootte.Contains("Large") && (x.Lengte >= 100 || x.Breedte >= 100 || x.Hoogte >= 100)) ||
                            (request.Grootte.Contains("Medium") && (x.Lengte >= 50 && x.Lengte < 100 || x.Breedte >= 50 && x.Breedte < 100 || x.Hoogte >= 50 && x.Hoogte < 100))
                            || (request.Grootte.Contains("Small") && (x.Lengte < 50 || x.Breedte < 50 || x.Hoogte < 50)))
                .Where(x => request.BetaalOpties == null || (request.BetaalOpties.Contains("Buy") && x.TeKoop) || (request.BetaalOpties.Contains("Bid") && x.IsVeilbaar))
                .Select(x => new Kunstwerk_DTO.Detail() {
                    Id = x.Id,
                    Naam = x.Naam,
                    HoofdFoto = new Foto_DTO(x.Fotos.FirstOrDefault().Naam, x.Fotos.FirstOrDefault().Locatie), //enkel eerste foto is nodig voor index
                    Materiaal = x.Materiaal,
                    Kunstenaar = new Kunstenaar_DTO {
                        Gebruikersnaam = x.Kunstenaar.Gebruikersnaam,
                        GebruikerId = x.Kunstenaar.GebruikerId,
                    },
                    Prijs = x.Prijs,
                    Lengte = x.Lengte,
                    Breedte = x.Breedte,
                    Hoogte = x.Hoogte,
                    Gewicht = x.Gewicht ?? default(decimal),
                    Beschrijving = x.Beschrijving,
                    Fotos = (List<Foto_DTO>)x.Fotos.Select(x => new Foto_DTO { Id = x.Id, Naam = x.Naam, Locatie = x.Locatie, Uploaded = true }),
                    TeKoop = x.TeKoop, //voor tekoop icon
                    IsVeilbaar = x.IsVeilbaar
                })
                .Where(x => string.IsNullOrEmpty(request.Naam) || x.Naam.Contains(request.Naam))
                .Where(x => string.IsNullOrEmpty(request.Kunstenaar) || x.Kunstenaar.Gebruikersnaam.Contains(request.Kunstenaar))
                .Where(x => request.MinimumPrijs.Equals(default(int)) || x.Prijs >= request.MinimumPrijs)
                .Where(x => request.MaximumPrijs.Equals(default(int)) || x.Prijs <= request.MaximumPrijs).OrderBy(x=>x.Naam).Skip(4 * request.Page)
                .Take(4).ToListAsync();
            
            Console.WriteLine("pagee"+request.Page); //skip moet eerst en dan take, in oef opl is andersom=fout

            respons.Kunstwerken = kunstwerken;
            return respons;
        }
        public async Task<KunstwerkResponse.Index> GetKunstwerkenZonderPaging(Kunstwerk_DTO.Filter request)
        {
            var respons = new KunstwerkResponse.Index();

            var kunstwerken = await dbContext.Kunstwerken.Include(k => k.Fotos)
                .Where(x => request.Materiaal == null || request.Materiaal.Contains(x.Materiaal))
                .Where(x => request.Grootte == null || (request.Grootte.Contains("Large") && (x.Lengte >= 100 || x.Breedte >= 100 || x.Hoogte >= 100)) ||
                            (request.Grootte.Contains("Medium") && (x.Lengte >= 50 && x.Lengte < 100 || x.Breedte >= 50 && x.Breedte < 100 || x.Hoogte >= 50 && x.Hoogte < 100))
                            || (request.Grootte.Contains("Small") && (x.Lengte < 50 || x.Breedte < 50 || x.Hoogte < 50)))
                .Where(x => request.BetaalOpties == null || (request.BetaalOpties.Contains("Buy") && x.TeKoop) || (request.BetaalOpties.Contains("Bid") && x.IsVeilbaar))
                .Select(x => new Kunstwerk_DTO.Detail()
                {
                    Id = x.Id,
                    Naam = x.Naam,
                    HoofdFoto = new Foto_DTO(x.Fotos.FirstOrDefault().Naam, x.Fotos.FirstOrDefault().Locatie), //enkel eerste foto is nodig voor index
                    Materiaal = x.Materiaal,
                    Kunstenaar = new Kunstenaar_DTO
                    {
                        Gebruikersnaam = x.Kunstenaar.Gebruikersnaam,
                        GebruikerId = x.Kunstenaar.GebruikerId,
                    },
                    Prijs = x.Prijs,
                    Lengte = x.Lengte,
                    Breedte = x.Breedte,
                    Hoogte = x.Hoogte,
                    Gewicht = x.Gewicht ?? default(decimal),
                    Beschrijving = x.Beschrijving,
                    Fotos = (List<Foto_DTO>)x.Fotos.Select(x => new Foto_DTO { Id = x.Id, Naam = x.Naam, Locatie = x.Locatie, Uploaded = true }),
                    TeKoop = x.TeKoop, //voor tekoop icon
                    IsVeilbaar = x.IsVeilbaar
                })
                .Where(x => string.IsNullOrEmpty(request.Naam) || x.Naam.Contains(request.Naam))
                .Where(x => string.IsNullOrEmpty(request.Kunstenaar) || x.Kunstenaar.Gebruikersnaam.Contains(request.Kunstenaar))
                .Where(x => request.MinimumPrijs.Equals(default(int)) || x.Prijs >= request.MinimumPrijs)
                .Where(x => request.MaximumPrijs.Equals(default(int)) || x.Prijs <= request.MaximumPrijs).OrderBy(x => x.Naam).ToListAsync();

            respons.Kunstwerken = kunstwerken;
            return respons;
        }

        public async Task<KunstwerkResponse.Create> CreateAsync(Kunstwerk_DTO.Create kunstwerk/*, int gebruikerId*/) {
            var kunstenaar = (Kunstenaar)dbContext.Gebruikers.Where(x => x is Kunstenaar).SingleOrDefault(g => g.Email == kunstwerk.KunstenaarEmail);
            //eerst nieuwe foto's regelen, belangrijk want fotonamen worden ook aangepast!
            var uploadUris = ManageUploadUris(kunstwerk.NieuweFotos, kunstenaar.GebruikerId);

            var fotos = kunstwerk.NieuweFotos.Select(fotoDTO => new Foto(fotoDTO.Naam, fotoDTO.Locatie)).ToList();

            var kunstwerkToCreate = new Kunstwerk(kunstwerk.Naam, DateTime.Now.AddDays(25), kunstwerk.Prijs, kunstwerk.Beschrijving, kunstwerk.Lengte, kunstwerk.Breedte, kunstwerk.Hoogte, kunstwerk.Gewicht, fotos, kunstwerk.IsVeilbaar, kunstwerk.Materiaal, kunstenaar);

            await dbContext.Kunstwerken.AddAsync(kunstwerkToCreate);
            await dbContext.SaveChangesAsync();

            return new KunstwerkResponse.Create() {
                KunstwerkId = kunstwerkToCreate.Id,
                UploadUris = uploadUris
            };
        }

        public async Task<KunstwerkResponse.Edit> UpdateAsync(Kunstwerk_DTO.Edit kunstwerk, int gebruikerId) {
            KunstwerkResponse.Edit response = new() {
                UploadUris = ManageUploadUris(kunstwerk.NieuweFotos, gebruikerId)
            };
            //eerst nieuwe foto's regelen

            /*if (kunstwerk.KunstenaarId != gebruikerId) {
                throw new ArgumentException();
            }*/

            var updatedFotoLijst = kunstwerk.Fotos.Select(fotoDTO => new Foto() { Id = fotoDTO.Id, Naam = fotoDTO.Naam, Locatie = fotoDTO.Locatie }).ToList(); //id wordt meegegeven als de foto al in de databank zit

            var kunstwerkToUpdate = dbContext.Kunstwerken.Include(k => k.Fotos).FirstOrDefault(x => x.Id == kunstwerk.Id);
            kunstwerkToUpdate.Edit(kunstwerk.Naam, DateTime.Now.AddDays(25), kunstwerk.Prijs, kunstwerk.Lengte, kunstwerk.Breedte, kunstwerk.Hoogte, kunstwerk.Gewicht, kunstwerk.Beschrijving, kunstwerk.IsVeilbaar, kunstwerk.Materiaal);

            //nodige fotos verwijderen
            var teVerwijderenFotos = kunstwerkToUpdate.Fotos.Where(x => !updatedFotoLijst.Contains(x)).ToList(); //alle fotos in de databank die niet in de updatedFotlijst staan moeten verwijderd worden
            if (teVerwijderenFotos.Any()) {
                await DeleteFotos(teVerwijderenFotos); //meegeven aan frontend om blob leeg te halen
            }

            //final foto update
            kunstwerkToUpdate.Fotos = updatedFotoLijst;

            dbContext.Kunstwerken.Update(kunstwerkToUpdate);
            await dbContext.SaveChangesAsync();

            return response;
        }

        public async Task<List<string>> GetMediums(int amount) { //aantal meestvoorkomende mediums ophalen
            var kunstwerken = await dbContext.Kunstwerken
                .ToListAsync();

            return kunstwerken.OrderByDescending(g => g.Materiaal.Count())
                .Select(g => g.Materiaal)
                .Distinct()
                .Take(amount)
                .ToList();
        }

        private IList<Uri> ManageUploadUris(IList<Foto_DTO> nieuweFotos, int gebruikerId) {
            IList<Uri> uploadUris = new List<Uri>();
            foreach (var foto in nieuweFotos) {
                var imageFolderPath = Path.Combine("Kunstwerken", $"{gebruikerId}", Guid.NewGuid().ToString());
                var imageFileName = Path.Combine(imageFolderPath, foto.Naam);
                uploadUris.Add(storageService.CreateUploadUri(imageFileName));

                foto.Locatie = Path.Combine(storageService.StorageBaseUri, imageFolderPath); //opslaan in db
                //foto.Naam = foto.Naam;
            }

            return uploadUris;
        }

        private async Task DeleteFotos(IList<Foto> teVerwijderenFotos) {
            foreach (var foto in teVerwijderenFotos) {
                await storageService.DeleteImage(foto.Pad);
            }
        }

        public async Task<KunstwerkResponse.Delete> DeleteAsync(int id) {
            KunstwerkResponse.Delete response = new();
            var kunstwerk = await dbContext.Kunstwerken.Include(x => x.Fotos).FirstOrDefaultAsync(x => x.Id == id);
            if (kunstwerk.IsVeilbaar) {
                response.Deleted = false;
                response.Message = "Artwork cannot be deleted, it contains an active auction";
                return response;
            }

            dbContext.Kunstwerken.Remove(kunstwerk);
            await dbContext.SaveChangesAsync();
            await DeleteFotos(kunstwerk.Fotos);

            response.Deleted = true;
            response.Message = "Artwork successfully deleted.";
            return response;
        }
    }
}