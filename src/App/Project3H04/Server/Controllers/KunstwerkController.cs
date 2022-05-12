using Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project3H04.Server.Data;
using Project3H04.Shared;
using Project3H04.Shared.Kunstwerken;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;  //AUTH

namespace Project3H04.Server.Controllers {
    [Authorize] //AUTH, op controller level altijd Authorize en dan bij de methods AllowAnonymous !
    [Route("api/[controller]")]
    [ApiController]
    public class KunstwerkController : ControllerBase {
        private readonly IKunstwerkService kunstwerkService;

        public KunstwerkController(IKunstwerkService kunstwerkService) {
            this.kunstwerkService = kunstwerkService;
        }

        //GET: api/<KunstwerkController>
        [AllowAnonymous]
        [HttpGet]
        public Task<KunstwerkResponse.Index> GetKunstwerken([FromQuery] Kunstwerk_DTO.Filter request) {
            return kunstwerkService.GetKunstwerken(request);
        }
        [AllowAnonymous]
        [HttpGet("ZonderPaging")]
        public Task<KunstwerkResponse.Index> GetKunstwerkenZonderPaging([FromQuery] Kunstwerk_DTO.Filter request)
        {
            return kunstwerkService.GetKunstwerkenZonderPaging(request);
        }

        // GET api/<KunstwerkController>/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public Task<KunstwerkResponse.Detail> Get(int id) {
            return kunstwerkService.GetDetailAsync(id);
        }

        // POST api/<KunstwerkController>
        [Authorize(Roles = "Administrator,Kunstenaar")]
        [HttpPost]
        public Task<KunstwerkResponse.Create> Create([FromBody] Kunstwerk_DTO.Create kunst) {
            return kunstwerkService.CreateAsync(kunst);
        }

        // PUT api/<KunstwerkController>
        [Authorize(Roles = "Administrator,Kunstenaar")]
        [HttpPut]
        public Task<KunstwerkResponse.Edit> Put([FromBody] Kunstwerk_DTO.Edit kunst) {
            return kunstwerkService.UpdateAsync(kunst, kunst.KunstenaarId);
        }

        // DELETE api/<KunstwerkController>/5
        [HttpDelete("{id}")]
        public  Task<KunstwerkResponse.Delete> Delete(int id) {
            return kunstwerkService.DeleteAsync(id);
        }

        //GET: api/<KunstwerkController>/materiaal/5
        [AllowAnonymous]
        [HttpGet("materiaal/{amount}")]
        public Task<List<string>> GetMediums(int amount) {
            return kunstwerkService.GetMediums(amount);
        }

        [AllowAnonymous]
        [HttpGet("aantalKunst")]
        public Task<int> GetAantalKunst() {
            return kunstwerkService.GetAantalKunst();
        }
    }
}