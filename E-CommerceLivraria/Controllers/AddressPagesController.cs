using E_CommerceLivraria.Models;
using E_CommerceLivraria.Repository.AddressR;
using E_CommerceLivraria.Repository.CustomerR;
using E_CommerceLivraria.Services.AddressS;
using E_CommerceLivraria.Services.CustomerS;
using Microsoft.AspNetCore.Mvc;
using E_CommerceLivraria.Models.ModelsStructGroups.PaymentSG;
using E_CommerceLivraria.Enums;
using E_CommerceLivraria.DTO.AddressDTO;

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

        [HttpGet("Address/Create/{origin:int}/{addToList:int}/{ctmId:decimal}")]
        public IActionResult CreateAddressPage([FromRoute] decimal ctmId, [FromRoute] int origin, [FromRoute] int addToList)
        {
            bool exists = _customerService.Exists(ctmId);
            if (!exists) return NotFound();

            var cad = new CreateAddressDTO()
            {
                CtmId = ctmId,
                RedirectTo = origin,
                AddToList = addToList,
                AddToAccount = !(origin == (int)EAddressCreate.PAYMENT)
            };

            ViewBag.PptList = _publicPlaceTypeRepository.GetAll();
            ViewBag.RstList = _residenceTypeRepository.GetAll();
            ViewBag.Layout = (origin < (int)EAddressCreate.DETAILED_CTM_PAGE) ? "~/Views/Shared/_PublicLayout.cshtml" : "~/Views/Shared/_AdminLayout.cshtml";

            return View("~/Views/Shared/Address/CreateAdd/createAddress.cshtml", cad);
        }

        [HttpPost]
        public IActionResult CreateAddress(CreateAddressDTO cad)
        {
            var add = cad.Address;
            if (add == null) return BadRequest();
            add = _addressService.Create(add);

            var ctm = _customerService.Get(cad.CtmId);
            if (ctm == null) return NotFound();

            EAddressCreate pageRedirect = (EAddressCreate)cad.RedirectTo;

            if (cad.AddToAccount)
            {
                var addTo = (EAddressType)cad.AddToList;

                switch (addTo)
                {
                    case EAddressType.DELIVERY:
                        ctm.DadAdds.Add(add);
                        break;

                    case EAddressType.BILLING:
                        ctm.BadAdds.Add(add);
                        break;
                }
                ctm = _customerService.Update(ctm);
            }

            switch (pageRedirect)
            {
                case EAddressCreate.PAYMENT:
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

                case EAddressCreate.BILLING_PAGE:
                    return RedirectToAction("AddressList", "ProfileAddress", new { Type = "billing", CtmId = ctm.CtmId });

                case EAddressCreate.DELIVERY_PAGE:
                    return RedirectToAction("AddressList", "ProfileAddress", new { Type = "delivery", CtmId = ctm.CtmId });

                case EAddressCreate.DETAILED_CTM_PAGE:
                    return RedirectToAction("DetailedCustomerPage", "AdmCustomer", new {id = ctm.CtmId});

                default: return BadRequest();
            }
        }
    }
}
