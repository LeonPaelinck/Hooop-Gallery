using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project3H04.Shared.DTO;
using Project3H04.Shared.Gebruiker;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project3H04.Server.Controllers {
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GebruikerController : ControllerBase {
        private readonly IGebruikerService gebruikerService;

        public GebruikerController(IGebruikerService gebruikerService) {
            this.gebruikerService = gebruikerService;
        }

        [AllowAnonymous]
        [HttpGet("{id}")]
        public Task<GebruikerResponse.Detail> Get(int id) {
            return gebruikerService.GetDetailAsync(id);
            //if(k == null)
            //    return NotFound();

            //return k;
        }

        [HttpPut]
        public Task<GebruikerResponse.Edit> EditAsync([FromBody] GebruikerRequest.Edit request) {
            return gebruikerService.EditAsync(request);
        }
    }
}
