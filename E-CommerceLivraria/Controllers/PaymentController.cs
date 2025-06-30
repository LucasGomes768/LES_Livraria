using System.Xml.Serialization;
using E_CommerceLivraria.Models.ModelsStructGroups.CartSG;
using Microsoft.AspNetCore.Mvc;
using E_CommerceLivraria.Models.ModelsStructGroups.PaymentSG;
using E_CommerceLivraria.Models;
using E_CommerceLivraria.Services.CustomerS;
using E_CommerceLivraria.Services.AddressS;
using E_CommerceLivraria.DTO.PaymentDTO.Method;
using E_CommerceLivraria.DTO.PaymentDTO;
using E_CommerceLivraria.Services.PurchaseS;
using System.Text.Json;
using E_CommerceLivraria.DTO.PaymentDTO.ChoosenAddress;
using Newtonsoft.Json.Serialization;
using E_CommerceLivraria.Services.CouponS;

namespace E_CommerceLivraria.Controllers
{
    public class PaymentController : Controller
    {
        private ICustomerService _customerService;
        private IAddressService _addressService;
        private IPurchaseService _purchaseService;
        private IPromoCouponAssignmentService _promoCouponAssignmentService;

        public PaymentController(ICustomerService customerService,
            IAddressService addressService,
            IPurchaseService purchaseService,
            IPromoCouponAssignmentService promoCouponAssignmentService)
        {
            _customerService = customerService;
            _addressService = addressService;
            _purchaseService = purchaseService;
            _promoCouponAssignmentService = promoCouponAssignmentService;
        }

        // ENDEREÇO
        public IActionResult DeliveryAddressPageRedirect(CartDataGroup cdg)
        {
            var ctm = _customerService.Get(cdg.CtmId);
            if (ctm == null) return NotFound();

            PayAddressPageData papd = new PayAddressPageData()
            {
                CtmId = cdg.CtmId,
                Cart = ctm.Cart,
                Addresses = ctm.DadAdds.ToList(),
                Total = ctm.Cart.CartItems.Sum(x => x.CriTotalprice)
            };

            return RedirectToAction("DeliveryAddressPage", papd);
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

            ViewBag.NewAddId = papd.ChoosenAddId;

            return View("~/Views/Customer/Cart/addressPayment/addressPayment.cshtml", papdNew);
        }

        [HttpGet("Payment/GetDeliveryAddress/{addId:decimal}")]
        public IActionResult GetChoosenAddress([FromRoute] decimal addId)
        {
            try
            {
                var add = _addressService.Get(addId);
                if (add == null) return NotFound(new
                {
                    Sucess = false,
                    Message = "Endereço não encontrado"
                });

                ChoosenAddressDTO addDTO = new ChoosenAddressDTO(add);

                return Ok(new
                {
                    Sucess = true,
                    Data = addDTO
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Sucess = false,
                    error = $"Erro ao selecionar endereço: {ex.Message}"
                });
            }
        }

        // MÉTODO DE PAGAMENTO
        public IActionResult RedirectPaymentMethodPage(PayAddressPageData papd)
        {
            var ctm = _customerService.Get(papd.CtmId);
            if (ctm == null) return BadRequest();

            if (ctm.Cart.CartItems.Count == 0) return RedirectToAction("homePage", "Home");

            if (papd.ChoosenAddId == null) return BadRequest();

            var add = _addressService.Get((decimal)papd.ChoosenAddId);
            if (add == null) return BadRequest();

            var mpd = new MethodPaymentDTO()
            {
                CtmId = ctm.CtmId,
                Total = ctm.CtmCrt.CartItems.Sum(x => x.CriTotalprice),
                Address = add,
                CreditCards = ctm.CtcCrds.ToList(),
                ExchangeCoupons = ctm.ExchangeCoupons.ToList(),
            };

            ViewBag.AddedCard = null;

            return View("~/Views/Customer/Cart/addressPayment/methodPayment/methodPayment.cshtml", mpd);
        }

        [HttpGet]
        public IActionResult PaymentMethodPage(decimal ctmId)
        {
            var addedCard = TempData["AddedCard"];

            if (addedCard is string)
            {
                ViewBag.AddedCard = addedCard;
            } else
            {
                ViewBag.AddedCard = null;
            }

            var ctm = _customerService.Get(ctmId);
            if (ctm == null) return NotFound();

            if (ctm.Cart.CartItems.Count == 0) return RedirectToAction("homePage", "Home");

            var mpd = new MethodPaymentDTO()
            {
                CtmId = ctm.CtmId,
                Total = ctm.CtmCrt.CartItems.Sum(x => x.CriTotalprice),
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

                if (request.PromotionalCode != null && request.PromotionalCode != "")
                    ctm = _promoCouponAssignmentService.RemovePromoCouponFromCtm(ctm, request.PromotionalCode);

                var purchaseData = new PurchaseDataDTO
                {
                    Request = request,
                    DeliveryAddress = deliveryAdd,
                    Customer = ctm
                };

                _purchaseService.Add(purchaseData);

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
