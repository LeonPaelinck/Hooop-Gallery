using Project3H04.Shared.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project3H04.Shared.Gebruiker {
    public interface IGebruikerService {
        Task<GebruikerResponse.Detail> GetDetailAsync(int id);
        Task<GebruikerResponse.Edit> EditAsync(GebruikerRequest.Edit request);
    }
}
