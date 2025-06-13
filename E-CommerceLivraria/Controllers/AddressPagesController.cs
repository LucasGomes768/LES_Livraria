using E_CommerceLivraria.Models;
using E_CommerceLivraria.Models.ModelsStructGroups.AddressSG;
using E_CommerceLivraria.Repository.AddressR;
using E_CommerceLivraria.Repository.CustomerR;
using E_CommerceLivraria.Services.AddressS;
using E_CommerceLivraria.Services.CustomerS;
using E_CommerceLivraria.Enums.Customer;
using Microsoft.AspNetCore.Mvc;
using E_CommerceLivraria.Models.ModelsStructGroups.PaymentSG;

namespace E_CommerceLivraria.Controllers
{
    public class AddressPagesController : Controller
    {
        private ICustomerService _customerService;
        private IAddressService _addressService;
        private IResidenceTypeRepository _residenceTypeRepository;
        private IPublicPlaceTypeRepository _publicPlaceTypeRepository;

        public AddressPagesController(ICustomerService customerService,
            IAddressService addressService,
            IResidenceTypeRepository residenceTypeRepository,
            IPublicPlaceTypeRepository publicPlaceTypeRepository)
        {
            _customerService = customerService;
            _addressService = addressService;
            _residenceTypeRepository = residenceTypeRepository;
            _publicPlaceTypeRepository = publicPlaceTypeRepository;
        }

        [HttpGet("Address/Create/{origin:int}/{addToList}/{ctmId:decimal}")]
        public IActionResult CreateAddressPage([FromRoute] decimal ctmId, [FromRoute] int origin, [FromRoute] string addToList)
        {
            bool exists = _customerService.Exists(ctmId);
            if (!exists) return NotFound();

            var cad = new CreateCtmAddressData()
            {
                CtmId = ctmId,
                RedirectTo = origin,
                AddToList = addToList,
                AddToAccount = !(origin == (int)ECtmAddressCreate.PAYMENT)
            };

            ViewBag.PptList = _publicPlaceTypeRepository.GetAll();
            ViewBag.RstList = _residenceTypeRepository.GetAll();

            return View("~/Views/Customer/Address/CreateAdd/createAddress.cshtml", cad);
        }

        [HttpPost]
        public IActionResult CreateAddress(CreateCtmAddressData cad)
        {
            var add = cad.Address;
            if (add == null) return BadRequest();
            add = _addressService.Create(add);

            var ctm = _customerService.Get(cad.CtmId);
            if (ctm == null) return NotFound();

            ECtmAddressCreate pageRedirect = (ECtmAddressCreate)cad.RedirectTo;

            if (cad.AddToAccount)
            {
                switch (cad.AddToList)
                {
                    case "delivery":
                        ctm.DadAdds.Add(add);
                        break;

                    case "billing":
                        ctm.BadAdds.Add(add);
                        break;
                }
                ctm = _customerService.Update(ctm);
            }

            switch (pageRedirect)
            {
                case ECtmAddressCreate.PAYMENT:
                    PayAddressPageData papd = new PayAddressPageData()
                    {
                        CtmId = ctm.CtmId,
                        Cart = ctm.Cart,
                        Addresses = ctm.DadAdds.ToList(),
                        Total = ctm.Cart.CartItems.Sum(x => x.CriTotalprice),
                        ChoosenAddId = add.AddId,
                        ChoosenAdd = _addressService.Get(add.AddId),
                        tempAdded = true
                    };

                    return RedirectToAction("DeliveryAddressPage", "Payment", papd);

                case ECtmAddressCreate.BILLING_PAGE:
                    return RedirectToAction("AddressList", "ProfileAddress", new { Type = "billing", CtmId = ctm.CtmId });

                case ECtmAddressCreate.DELIVERY_PAGE:
                    return RedirectToAction("AddressList", "ProfileAddress", new { Type = "delivery", CtmId = ctm.CtmId });

                default: return BadRequest();
            }
        }
    }
}
