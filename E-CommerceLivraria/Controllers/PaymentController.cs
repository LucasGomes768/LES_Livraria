using System.Xml.Serialization;
using E_CommerceLivraria.Models.ModelsStructGroups.CartSG;
using Microsoft.AspNetCore.Mvc;
using E_CommerceLivraria.Models.ModelsStructGroups.PaymentSG;
using E_CommerceLivraria.Models;
using E_CommerceLivraria.Services.CustomerS;
using E_CommerceLivraria.Services.AddressS;
using E_CommerceLivraria.Models.ModelsStructGroups.MethodPaymentSG;
using E_CommerceLivraria.Models.ModelsStructGroups.CreditCardSG;

namespace E_CommerceLivraria.Controllers
{
    public class PaymentController : Controller
    {
        private ICustomerService _customerService;
        private IAddressService _addressService;

        public PaymentController(ICustomerService customerService,
            IAddressService addressService)
        {
            _customerService = customerService;
            _addressService = addressService;
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

        public IActionResult PaymentMethodPage(MethodPaymentData mpd)
        {
            if (!_customerService.Exists(mpd.CtmId)) return BadRequest();

            var add = _addressService.Get(mpd.Address.AddId);
            if (add == null) return BadRequest();

            mpd.Address = add;

            return View("~/Views/Customer/Cart/addressPayment/methodPayment/methodPayment.cshtml", mpd);
        }

        [HttpGet]
        public IActionResult AddCreditCard(MethodPaymentData mpd)
        {
            var ctm = _customerService.Get(mpd.CtmId);
            if (ctm == null) return BadRequest();

            var ccp = new CreditCardPurchaseData()
            {
                CreditCard = ctm.CtcCrds.First(x => x.CrdId == mpd.ChoosenCardId),
                PurchaseValue = 10
            };

            if (!mpd.CreditCardsUsed.Contains(ccp))
                mpd.CreditCardsUsed.Add(ccp);

            return RedirectToAction("PaymentMethodPage", "Payment", mpd);
        }
    }
}
