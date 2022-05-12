using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Project3H04.Shared.DTO;
using Project3H04.Shared.Veilingen;

namespace Project3H04.Server.Controllers {
    [Authorize]
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class VeilingController : ControllerBase {
        private readonly IVeilingService _veilingService;

        public VeilingController(IVeilingService veilingService) {
            this._veilingService = veilingService;
        }

        [HttpGet, ActionName("")]
        public Task<List<Veiling_DTO>> GetVeilings(string term = "", int take = 25, bool almostFinishedVeilingen = false) {
            return _veilingService.GetVeilingen(term, take, almostFinishedVeilingen);
        }

        [HttpGet("{id}"), ActionName("Get")]
        public Task<Veiling_DTO> Get(int id) {
            return _veilingService.GetVeilingById(id);
        }

        [HttpGet("{id}"), ActionName("GetByKunstwerkId")]
        public Task<Veiling_DTO> GetByKunstwerkId(int id) {
            return _veilingService.GetVeilingByKunstwerkId(id);
        }

        [HttpPut("{veilingId}"), ActionName("AddBodToVeiling")]
        public Task<bool> AddBodToVeiling(int veilingId, Bod_DTO bod) {
            return _veilingService.AddBodToVeiling(bod, veilingId);
        }

        [Authorize(Roles = "Administrator,Kunstenaar")]
        [HttpPut, ActionName("Edit")]
        public Task<bool> Edit(Veiling_DTO veiling) {
            return _veilingService.EditVeiling(veiling);
        }

        [Authorize(Roles = "Klant,Administrator,Kunstenaar")]
        [HttpPut("{kunstwerkId}"), ActionName("AddBodToKunstwerk")]
        public Task<bool> AddBodToKunstwerk(int kunstwerkId, Bod_DTO bod) {
            return _veilingService.AddBodToKunstwerk(bod, kunstwerkId);
        }

        [Authorize(Roles = "Klant,Administrator,Kunstenaar")]
        [HttpPost, ActionName("Create")]
        public Task<bool> Create(Veiling_DTO veiling) {
            return _veilingService.CreateVeiling(veiling);
        }
    }
}
