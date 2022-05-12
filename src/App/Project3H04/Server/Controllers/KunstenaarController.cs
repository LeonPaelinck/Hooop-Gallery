using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; //deze voor de async toevoegen, de andere is kek
using Project3H04.Server.Data;
using Project3H04.Shared.Kunstenaars;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Project3H04.Server.Controllers {
    [AllowAnonymous] //AUTH
    [Route("api/[controller]")]
    [ApiController]
    public class KunstenaarController : ControllerBase {
        private readonly IKunstenaarService kunstenaarService;

        public KunstenaarController(IKunstenaarService KunstenaarService) {
            this.kunstenaarService = KunstenaarService;
        }

        //GET: api/<KunstenaarController>
        [HttpGet]
        public Task<KunstenaarResponse.Index> GetKunstenaars(string term = "", int take = 25, bool recentArtists = false) {
            return kunstenaarService.GetKunstenaars(term, take, recentArtists);
        }

        //GET api/<KunstenaarController>/5
        [HttpGet("{id}")]
        public Task<KunstenaarResponse.Detail> Get(int id) {
            return kunstenaarService.GetDetailAsync(id);
            //if(k == null)
            //    return NotFound();

            //return k;
        }

        [HttpGet, Route("byEmail/{email}")]
        public Task<KunstenaarResponse.Detail> Get(string email) {
            return kunstenaarService.GetKunstenaarByEmail(email);
            //if(k == null)
            //    return NotFound();

            //return k;
        }
    }
}
