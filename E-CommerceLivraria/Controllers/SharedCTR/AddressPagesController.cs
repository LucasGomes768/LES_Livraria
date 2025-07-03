using E_CommerceLivraria.Repository.AddressR;
using E_CommerceLivraria.Services.AddressS;
using E_CommerceLivraria.Services.CustomerS;
using Microsoft.AspNetCore.Mvc;
using E_CommerceLivraria.Enums;
using E_CommerceLivraria.DTO.AddressDTO;
using E_CommerceLivraria.DTO.PaymentDTO.ChoosenAddress;
using E_CommerceLivraria.Services.LoginS;
using E_CommerceLivraria.Models;
using E_CommerceLivraria.Specifications;
using E_CommerceLivraria.Specifications.CustomerSpecs.Addresses;
using E_CommerceLivraria.Specifications.CustomerSpecs.Cart;
using E_CommerceLivraria.Extensions.Specifications;

namespace E_CommerceLivraria.Controllers.SharedCTR
{
    public class AddressPagesController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IAddressService _addressService;
        private readonly IResidenceTypeRepository _residenceTypeRepository;
        private readonly IPublicPlaceTypeRepository _publicPlaceTypeRepository;
        private readonly LoginSingleton _loginSingleton;

        public AddressPagesController(ICustomerService customerService, IAddressService addressService, IResidenceTypeRepository residenceTypeRepository, IPublicPlaceTypeRepository publicPlaceTypeRepository, LoginSingleton loginSingleton)
        {
            _customerService = customerService;
            _addressService = addressService;
            _residenceTypeRepository = residenceTypeRepository;
            _publicPlaceTypeRepository = publicPlaceTypeRepository;
            _loginSingleton = loginSingleton;
        }

        [HttpGet("Address/Create/{origin:int}/{addToList:int}/{ctmId:int?}")]
        public IActionResult CreateAddressPage([FromRoute] int origin, [FromRoute] int addToList, [FromRoute] int? ctmId)
        {
            if (ctmId == null || origin != (int)EAddressCreate.DETAILED_CTM_PAGE)
            {
                if (_loginSingleton?.CtmId == null) return RedirectToAction("LoginPage", "Login");
                else ctmId = (int)_loginSingleton.CtmId;
            }
            else
                if (!_customerService.Exists((int)ctmId)) return BadRequest("Cliente não foi encontrado."); ;


            var cad = new CreateAddressDTO()
            {
                CtmId = (int)ctmId,
                RedirectTo = origin,
                AddToList = addToList,
                AddToAccount = !(origin == (int)EAddressCreate.PAYMENT)
            };

            ViewBag.PptList = _publicPlaceTypeRepository.GetAll();
            ViewBag.RstList = _residenceTypeRepository.GetAll();
            ViewBag.Layout = origin < (int)EAddressCreate.DETAILED_CTM_PAGE ? "~/Views/Shared/_PublicLayout.cshtml" : "~/Views/Shared/_AdminLayout.cshtml";

            return View("~/Views/Shared/Address/CreateAdd/createAddress.cshtml", cad);
        }

        [HttpPost]
        public IActionResult CreateAddress(CreateAddressDTO cad)
        {
            var add = cad.Address;
            if (add == null) return BadRequest("Dados do endereço não foram enviados.");
            add = _addressService.Create(add);

            decimal id = _loginSingleton.CtmId ?? cad.CtmId;
            var addTo = (EAddressType)cad.AddToList;

            ISpecification<Customer> spec = addTo == EAddressType.DELIVERY ? new GetCtmsDelAddresses(id) : new GetCtmBilAddresses(id);
            ISpecification<Customer> specCrt = new GetCtmsCart(id);
            var combinedSpecs = spec.And(specCrt);

            Customer? ctm = _customerService.Get(combinedSpecs);
            if (ctm == null) return NotFound("O cliente não foi encontrado ou não existe.");

            EAddressCreate pageRedirect = (EAddressCreate)cad.RedirectTo;

            if (cad.AddToAccount)
            {

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
                    PayAddressPageDTO papd = new PayAddressPageDTO()
                    {
                        Cart = ctm.CtmCrt,
                        Addresses = ctm.DadAdds.ToList(),
                        Total = ctm.Cart.CartItems.Sum(x => x.CriTotalprice),
                        ChoosenAddId = add.AddId,
                        ChoosenAdd = _addressService.Get(add.AddId),
                        tempAdded = true
                    };

                    return RedirectToAction("DeliveryAddressPage", "Payment", papd);

                case EAddressCreate.BILLING_PAGE:
                    return RedirectToAction("AddressList", "ProfileAddress", new { Type = (int)EAddressType.BILLING });

                case EAddressCreate.DELIVERY_PAGE:
                    return RedirectToAction("AddressList", "ProfileAddress", new { Type = (int)EAddressType.DELIVERY });

                case EAddressCreate.DETAILED_CTM_PAGE:
                    return RedirectToAction("DetailedCustomerPage", "AdmCustomer", new {id = ctm.CtmId});

                default: return BadRequest("Nenhuma página de redireção foi encontrada.");
            }
        }
    }
}
