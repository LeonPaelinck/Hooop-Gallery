using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Mollie.Api.Client;
using Mollie.Api.Client.Abstract;
using Mollie.Api.Models;
using Mollie.Api.Models.Payment;
using Mollie.Api.Models.Payment.Request;
using Mollie.Api.Models.Payment.Response;
using Project3H04.Shared.DTO;
using Project3H04.Shared.Klant;
using Project3H04.Shared.Kunstwerken;
using Project3H04.Shared.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Project3H04.Server.Controllers {
    [Authorize]
    [Route("api/[controller]/[Action]")]
    [ApiController]
    public class OrderController : ControllerBase {
        private readonly IOrderService OrderService;
        private readonly IKlantService KlantService;

        public OrderController(IOrderService orderservice, IKlantService klantservice) {
            OrderService = orderservice;
            KlantService = klantservice;
        }

        [HttpGet("{id}"), ActionName("get")]
        public async Task<Bestelling_DTO.Index> GetBestelling(int id) {
            return await OrderService.GetBestelling(id);
        }

        [AllowAnonymous]
        [HttpGet("{Id}"), ActionName("exists")]
        public bool CheckIfBestellingExists(int id) {
            return OrderService.Bestellingexists(id);
        }

        [HttpGet("{userEmail}"), ActionName("myOrders")]
        public async Task<IEnumerable<Bestelling_DTO.Index>> GetBestellingenByUser(string userEmail) {
            OrderResponse.Detail response = await OrderService.GetUserOrders(userEmail);
            return response.Bestellingen;
        }

        // creates payment and order
        [HttpPost, ActionName("Mollie")]
        public async Task<IActionResult> CreateOrder(Bestelling_DTO.Create bestelling) {
            var bestellingId = await OrderService.PostOrderAsync(bestelling);
            IPaymentClient paymentClient = new PaymentClient("test_5hj5GaUDpQDyrhVK4yqRfhV4PnERfn");
            var paymentRequest = new PaymentRequest() {
                Amount = new Amount(Currency.EUR, bestelling.TotalePrijs),
                Description = $"HoopGallery test payment",
                WebhookUrl = "https://hooopgallery-h04-productie.azurewebsites.net/api/order/orderstatus",      
                 RedirectUrl = $"https://hooopgallery-h04-productie.azurewebsites.net/ordersuccessful/{bestellingId}",
                 //WebhookUrl = "https://14d3-2a02-a03f-eaee-3d00-ed6d-8a8f-5a2b-7c8a.ngrok.io/api/order/orderstatus", // uses ngrok 
                 //RedirectUrl = $"https://localhost:5001/ordersuccessful/{bestellingId}",
                Methods = new List<string>() {
                   PaymentMethod.Ideal,
                   PaymentMethod.CreditCard,
                   PaymentMethod.DirectDebit
                }
            };

            var paymentResponse = await paymentClient.CreatePaymentAsync(paymentRequest);
            var paymentId = paymentResponse.Id;
            await OrderService.PutOrderAsync(paymentId, bestellingId);
            Console.WriteLine(paymentRequest.RedirectUrl);

            return Ok(paymentResponse);
        }

        
        [HttpPost, ActionName("MollieAndroid")]
        public async Task<IActionResult> CreateOrderAndroid(Bestelling_DTO.Create bestelling) {
            var bestellingId = await OrderService.PostOrderAsync(bestelling);
            IPaymentClient paymentClient = new PaymentClient("test_5hj5GaUDpQDyrhVK4yqRfhV4PnERfn");
            var paymentRequest = new PaymentRequest() {
                Amount = new Amount(Currency.EUR, bestelling.TotalePrijs),
                Description = $"HoopGallery test payment",
               // WebhookUrl = "https://14d3-2a02-a03f-eaee-3d00-ed6d-8a8f-5a2b-7c8a.ngrok.io/api/order/orderstatus", // uses ngrok      
                WebhookUrl = "https://hooopgallery-h04-productie.azurewebsites.net/api/order/orderstatus",
                RedirectUrl = "com.hooop.android://payment-return",
               
                Methods = new List<string>() {
                   PaymentMethod.Ideal,
                   PaymentMethod.CreditCard,
                   PaymentMethod.DirectDebit 
                }
            };
            var paymentResponse = await paymentClient.CreatePaymentAsync(paymentRequest);
            var paymentId = paymentResponse.Id;
            await OrderService.PutOrderAsync(paymentId, bestellingId);
            Console.WriteLine(paymentRequest.RedirectUrl);

            return Ok(paymentResponse.Links.Checkout.Href);
        }

        [AllowAnonymous]
        [HttpPost, ActionName("orderstatus")]
        public async Task<IActionResult> GetOrderStatus([FromForm] string id) {
            IPaymentClient paymentClient = new PaymentClient("test_5hj5GaUDpQDyrhVK4yqRfhV4PnERfn");
            var response =  await paymentClient.GetPaymentAsync(id);
            Console.WriteLine(response.Status);

            if (response.Status == "paid") {
               // await OrderService.CreateBestelling();
            } else if(response.Status != "paid") {
                await OrderService.RemoveBestelling(id);
            }

            return Ok(response);
        }
    }
}
