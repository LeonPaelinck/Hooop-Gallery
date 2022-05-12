using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Project3H04.Shared.Abonnementen;
using Project3H04.Shared.DTO;
using Project3H04.Shared.Klant;
using Project3H04.Shared.Kunstenaars;
using Project3H04.Shared.Kunstwerken;

namespace Project3H04.Server.Controllers {
    [Authorize]
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class AbonnementController : ControllerBase {
        private readonly IAbonnementService AbonnementService;

        public AbonnementController(IAbonnementService abonnementService) {
            AbonnementService = abonnementService;
        }

        [AllowAnonymous]
        [HttpGet, ActionName("")]
        public async Task<IList<Abonnement_DTO>> GetSubscriptions() {
            return await AbonnementService.GetAllAbonnementen();
        }

        [AllowAnonymous]
        [HttpGet, ActionName("GetTypes")]
        public async Task<IList<AbonnementType_DTO>> GetSubscriptionTypes() {
            return await AbonnementService.GetAllAbonnementTypes();
        }

        [AllowAnonymous]
        [HttpPut, ActionName("UpdateSubscription")]
        public async Task<Abonnement_DTO> UpdateSubscription(Abonnement_DTO abo) {
            return await AbonnementService.UpdateSubscription(abo);
        }
    }
}
