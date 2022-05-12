using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project3H04.Shared.DTO;
using Project3H04.Shared.Klant;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore; //deze voor de async toevoegen, de andere is kek

namespace Project3H04.Server.Controllers {
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class KlantController : ControllerBase {
        private readonly IKlantService klantService;

        public KlantController(IKlantService klantService) {
            this.klantService = klantService;
        }

        [HttpGet("{id}")]
        public Task<KlantResponse.Detail> Get(int id) {
            return klantService.GetKlantById(id);
        }

        //zo andere get route geven
        [HttpGet, Route("byEmail/{email}")]
        public Task<KlantResponse.Detail> Get(string email) {
            return klantService.GetKlantByEmail(email);
        }

        [HttpPost]
        public Task<KlantResponse.Create> CreateAsync(Klant_DTO klant) {
            return klantService.CreateAsync(klant);
        }
    }
}
