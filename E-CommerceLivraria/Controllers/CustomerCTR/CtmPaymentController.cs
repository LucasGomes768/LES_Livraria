using E_CommerceLivraria.Models;
using E_CommerceLivraria.Models.ModelsStructGroups.AddressSG;
using E_CommerceLivraria.Repository.AddressR;
using E_CommerceLivraria.Repository.CustomerR;
using E_CommerceLivraria.Services.AddressS;
using E_CommerceLivraria.Services.CustomerS;
using E_CommerceLivraria.Enums.Customer;
using Microsoft.AspNetCore.Mvc;
using E_CommerceLivraria.Models.ModelsStructGroups.PaymentSG;

namespace E_CommerceLivraria.Controllers.CustomerCTR
{
    [Route("[controller]")]
    public class CtmPaymentController : Controller
    {
        private ICustomerService _customerService;
        private IAddressService _addressService;
        private IResidenceTypeRepository _residenceTypeRepository;
        private IPublicPlaceTypeRepository _publicPlaceTypeRepository;

        public CtmPaymentController(ICustomerService customerService,
            IAddressService addressService,
            IResidenceTypeRepository residenceTypeRepository,
            IPublicPlaceTypeRepository publicPlaceTypeRepository)
        {
            _customerService = customerService;
            _addressService = addressService;
            _residenceTypeRepository = residenceTypeRepository;
            _publicPlaceTypeRepository = publicPlaceTypeRepository;
        }

        [HttpGet("AddressCreate/{origin}/{ctmId:decimal}")]
        public IActionResult CreateAddressPage(decimal ctmId, string origin)
        {
            bool exists = _customerService.Exists(ctmId);
            if (!exists) return NotFound();

            var cad = new CreateCtmAddressData()
            {
                CtmId = ctmId,
                RedirectTo = origin,
                PublicplaceTypes = _publicPlaceTypeRepository.GetAll(),
                ResidenceTypes = _residenceTypeRepository.GetAll()
            };

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

            if (cad.AddToAccount)
            {
                ctm.DadAdds.Add(add);
                ctm = _customerService.Update(ctm);
            }

            ECtmAddressCreate pageRedirect = (ECtmAddressCreate)Enum.Parse(typeof(ECtmAddressCreate), cad.RedirectTo);

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

                default: return BadRequest();
            }
        }
    }
}
