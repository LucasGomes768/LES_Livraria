using System.Xml.Serialization;
using E_CommerceLivraria.Models.ModelsStructGroups.CartSG;
using Microsoft.AspNetCore.Mvc;
using E_CommerceLivraria.Models.ModelsStructGroups.PaymentSG;
using E_CommerceLivraria.Models;
using E_CommerceLivraria.Services.CustomerS;
using E_CommerceLivraria.Services.AddressS;
using E_CommerceLivraria.Models.ModelsStructGroups.MethodPaymentSG;
using E_CommerceLivraria.DTO.PaymentDTO;
using E_CommerceLivraria.Services.PurchaseS;

namespace E_CommerceLivraria.Controllers
{
    public class PaymentController : Controller
    {
        private ICustomerService _customerService;
        private IAddressService _addressService;
        private IPurchaseService _purchaseService;

        public PaymentController(ICustomerService customerService,
            IAddressService addressService,
            IPurchaseService purchaseService)
        {
            _customerService = customerService;
            _addressService = addressService;
            _purchaseService = purchaseService;
        }

        // ENDEREÇO
        public IActionResult DeliveryAddressPage(CartDataGroup cdg)
        {
            var ctm = _customerService.Get(cdg.CtmId);
            if (ctm == null) return NotFound();

            PayAddressPageData papd = new PayAddressPageData()
            {
                CtmId = cdg.CtmId,
                Cart = ctm.Cart,
                Addresses = ctm.DadAdds.ToList(),
                Total = ctm.Cart.CartItems.Sum(x => x.CriTotalprice),
                
            };

            return View("~/Views/Customer/Cart/addressPayment/addressPayment.cshtml", papd);
        }

        [HttpGet]
        public IActionResult DeliveryAddressPage(PayAddressPageData papd)
        {
            var ctm = _customerService.Get(papd.CtmId);
            if (ctm == null) return NotFound();

            PayAddressPageData papdNew = new PayAddressPageData()
            {
                CtmId = papd.CtmId,
                Cart = ctm.Cart,
                Addresses = ctm.DadAdds.ToList(),
                Total = ctm.Cart.CartItems.Sum(x => x.CriTotalprice),
            };

            if (papd.ChoosenAddId.HasValue)
            {
                papdNew.ChoosenAddId = papd.ChoosenAddId;
                papdNew.ChoosenAdd = _addressService.Get((decimal)papd.ChoosenAddId);
            }

            return View("~/Views/Customer/Cart/addressPayment/addressPayment.cshtml", papdNew);
        }

        // MÉTODO DE PAGAMENTO
        public IActionResult RedirectPaymentMethodPage(PayAddressPageData papd)
        {
            var ctm = _customerService.Get(papd.CtmId);
            if (ctm == null) return BadRequest();

            if (papd.ChoosenAddId == null) return BadRequest();

            var add = _addressService.Get((decimal)papd.ChoosenAddId);
            if (add == null) return BadRequest();

            var mpd = new MethodPaymentData()
            {
                CtmId = ctm.CtmId,
                Total = ctm.CtmCrt.CartItems.Sum(x => x.CriTotalprice),
                Address = add,
                CreditCards = ctm.CtcCrds.ToList(),
                ExchangeCoupons = ctm.ExchangeCoupons.ToList(),
            };

            return View("~/Views/Customer/Cart/addressPayment/methodPayment/methodPayment.cshtml", mpd);
        }

        [HttpPost("Payment/ProcessPurchase")]
        public IActionResult ProcessPurchase([FromBody] FinishPurchaseRequestDTO request)
        {
            try
            {
                var ctm = _customerService.Get(request.CtmId);
                if (ctm == null) return BadRequest("O cliente não foi encontrado");

                var deliveryAdd = _addressService.Get(request.AddressId);
                if (deliveryAdd == null) return BadRequest("O endereço de entrega não foi encontrado");

                var purchaseData = new PurchaseDataDTO
                {
                    Request = request,
                    DeliveryAddress = deliveryAdd,
                    Customer = ctm
                };
                var addedPurchase = _purchaseService.Add(purchaseData);

                ctm.Purchases.Add(addedPurchase);
                ctm.Cart.CartItems.Clear();
                _customerService.Update(ctm);

                return Ok(new {
                    Sucess = true,
                    Message = "Compra processada com sucesso"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { 
                    Sucess = false,
                    Message = $"Erro ao processar compra: {ex.Message}"
                });
            }
        }
    }
}
