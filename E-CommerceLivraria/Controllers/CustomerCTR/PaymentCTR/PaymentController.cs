using Microsoft.AspNetCore.Mvc;
using E_CommerceLivraria.Services.CustomerS;
using E_CommerceLivraria.Services.AddressS;
using E_CommerceLivraria.DTO.PaymentDTO.Method;
using E_CommerceLivraria.DTO.PaymentDTO;
using E_CommerceLivraria.Services.PurchaseS;
using E_CommerceLivraria.DTO.PaymentDTO.ChoosenAddress;
using E_CommerceLivraria.Services.CouponS;
using E_CommerceLivraria.DTO.CartDTO;
using E_CommerceLivraria.Services.LoginS;
using E_CommerceLivraria.Specifications;
using E_CommerceLivraria.Models;
using E_CommerceLivraria.Specifications.CustomerSpecs.Addresses;
using E_CommerceLivraria.Specifications.CustomerSpecs.Cart;
using E_CommerceLivraria.Extensions.Specifications;
using E_CommerceLivraria.Specifications.CustomerSpecs.CreditCards;
using E_CommerceLivraria.Specifications.CustomerSpecs.Coupons;

namespace E_CommerceLivraria.Controllers.CustomerCTR.PaymentCTR
{
    public class PaymentController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IAddressService _addressService;
        private readonly IPurchaseService _purchaseService;
        private readonly IPromoCouponAssignmentService _promoCouponAssignmentService;
        private readonly LoginSingleton _loginSingleton;

        public PaymentController(ICustomerService customerService, IAddressService addressService, IPurchaseService purchaseService, IPromoCouponAssignmentService promoCouponAssignmentService, LoginSingleton loginSingleton)
        {
            _customerService = customerService;
            _addressService = addressService;
            _purchaseService = purchaseService;
            _promoCouponAssignmentService = promoCouponAssignmentService;
            _loginSingleton = loginSingleton;
        }

        // ENDEREÇO
        public IActionResult DeliveryAddressPageRedirect(CartItemsDTO cdg)
        {
            if (_loginSingleton.CtmId == null || _loginSingleton.CtmId == 0) return RedirectToAction("LoginPage", "Login");

            decimal id = (decimal)_loginSingleton.CtmId;

            ISpecification<Customer> specAdd = new GetCtmsDelAddresses(id);
            ISpecification<Customer> specCrt = new GetCtmsCart(id);
            var combinedSpec = specAdd.And(specCrt);

            var ctm = _customerService.Get(combinedSpec);
            if (ctm == null) return NotFound("O cliente não foi encontrado ou não existe");

            if (ctm.Cart.CartItems.Count == 0) return RedirectToAction("HomePage", "Home");

            PayAddressPageDTO papd = new PayAddressPageDTO()
            {
                Cart = ctm.Cart,
                Addresses = ctm.DadAdds.ToList(),
                Total = ctm.Cart.CartItems.Sum(x => x.CriTotalprice)
            };

            return RedirectToAction("DeliveryAddressPage", papd);
        }

        [HttpGet]
        public IActionResult DeliveryAddressPage(PayAddressPageDTO papd)
        {
            if (_loginSingleton.CtmId == null || _loginSingleton.CtmId == 0) return RedirectToAction("LoginPage", "Login");

            decimal id = (decimal)_loginSingleton.CtmId;

            ISpecification<Customer> specAdd = new GetCtmsDelAddresses(id);
            ISpecification<Customer> specCrt = new GetCtmsCart(id);
            var combinedSpec = specAdd.And(specCrt);

            var ctm = _customerService.Get(combinedSpec);
            if (ctm == null) return NotFound("O cliente não foi encontrado ou não existe");

            if (ctm.Cart.CartItems.Count == 0) return RedirectToAction("HomePage", "Home");

            PayAddressPageDTO papdNew = new PayAddressPageDTO()
            {
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
        public IActionResult RedirectPaymentMethodPage(PayAddressPageDTO papd)
        {
            if (_loginSingleton.CtmId == null || _loginSingleton.CtmId == 0) return RedirectToAction("LoginPage", "Login");

            decimal id = (decimal)_loginSingleton.CtmId;

            ISpecification<Customer> specAdd = new GetCtmsDelAddresses(id);
            ISpecification<Customer> specCrt = new GetCtmsCart(id);
            ISpecification<Customer> specCrd = new GetCtmsCreditCards(id);
            ISpecification<Customer> specXcp = new GetCtmsExchangeCoupons(id);
            var combinedSpec = specAdd
                .And(specCrt)
                .And(specCrd)
                .And(specXcp);

            var ctm = _customerService.Get(combinedSpec);
            if (ctm == null) return NotFound("O cliente não foi encontrado ou não existe");

            if (ctm.Cart.CartItems.Count == 0) return RedirectToAction("HomePage", "Home");

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
        public IActionResult PaymentMethodPage()
        {
            var addedCard = TempData["AddedCard"];

            if (addedCard is string)
            {
                ViewBag.AddedCard = addedCard;
            } else
            {
                ViewBag.AddedCard = null;
            }

            if (_loginSingleton.CtmId == null || _loginSingleton.CtmId == 0) return RedirectToAction("LoginPage", "Login");

            decimal ctmId = (decimal)_loginSingleton.CtmId;

            ISpecification<Customer> specAdd = new GetCtmsDelAddresses(ctmId);
            ISpecification<Customer> specCrt = new GetCtmsCart(ctmId);
            ISpecification<Customer> specCrd = new GetCtmsCreditCards(ctmId);
            ISpecification<Customer> specXcp = new GetCtmsExchangeCoupons(ctmId);
            var combinedSpec = specAdd
                .And(specCrt)
                .And(specCrd)
                .And(specXcp);

            var ctm = _customerService.Get(combinedSpec);
            if (ctm == null) return NotFound("O cliente não foi encontrado ou não existe");

            if (ctm.Cart.CartItems.Count == 0) return RedirectToAction("HomePage", "Home");

            var mpd = new MethodPaymentDTO()
            {
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
                if (_loginSingleton.CtmId == null || _loginSingleton.CtmId == 0) return BadRequest();

                decimal id = (decimal)_loginSingleton.CtmId;

                ISpecification<Customer> specAdd = new GetCtmsDelAddresses(id);
                ISpecification<Customer> specCrt = new GetCtmsCart(id);
                ISpecification<Customer> specCrd = new GetCtmsCreditCards(id);
                ISpecification<Customer> specXcp = new GetCtmsExchangeCoupons(id);
                ISpecification<Customer> specPcp = new GetCtmsPromoCoupons(id);
                var combinedSpec = specAdd
                    .And(specCrt)
                    .And(specCrd)
                    .And(specXcp)
                    .And(specPcp);

                var ctm = _customerService.Get(combinedSpec);
                if (ctm == null) return NotFound("O cliente não foi encontrado ou não existe");

                if (ctm.Cart.CartItems.Count == 0) return RedirectToAction("HomePage", "Home");

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
