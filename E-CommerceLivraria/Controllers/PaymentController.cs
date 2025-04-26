using System.Xml.Serialization;
using E_CommerceLivraria.Models.ModelsStructGroups.CartSG;
using Microsoft.AspNetCore.Mvc;
using E_CommerceLivraria.Models.ModelsStructGroups.PaymentSG;
using E_CommerceLivraria.Models;
using E_CommerceLivraria.Services.CustomerS;
using E_CommerceLivraria.Services.AddressS;

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
            Customer? ctm = _customerService.Get(papd.CtmId);
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
                papdNew.ChoosenAdd = _addressService.Get((decimal)papd.ChoosenAddId);
            }

            return View("~/Views/Customer/Cart/addressPayment/addressPayment.cshtml", papdNew);
        }

        // MÉTODO DE PAGAMENTO
    }
}
