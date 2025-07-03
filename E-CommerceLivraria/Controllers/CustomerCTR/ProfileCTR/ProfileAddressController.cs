using E_CommerceLivraria.DTO.ProfileDTO.AddressDTO;
using E_CommerceLivraria.Enums;
using E_CommerceLivraria.Models;
using E_CommerceLivraria.Repository.AddressR;
using E_CommerceLivraria.Services.AddressS;
using E_CommerceLivraria.Services.CustomerS;
using E_CommerceLivraria.Services.LoginS;
using E_CommerceLivraria.Specifications;
using E_CommerceLivraria.Specifications.CustomerSpecs;
using E_CommerceLivraria.Specifications.CustomerSpecs.Addresses;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceLivraria.Controllers.CustomerCTR.ProfileCTR
{
    public class ProfileAddressController : Controller
    {
        readonly private ICustomerService _customerService;
        readonly private IAddressService _addressService;
        readonly private IResidenceTypeRepository _residenceTypeRepository;
        readonly private IPublicPlaceTypeRepository _publicPlaceTypeRepository;
        private readonly LoginSingleton _loginSingleton;

        public ProfileAddressController(ICustomerService customerService, IAddressService addressService, IResidenceTypeRepository residenceTypeRepository, IPublicPlaceTypeRepository publicPlaceTypeRepository, LoginSingleton loginSingleton)
        {
            _customerService = customerService;
            _addressService = addressService;
            _residenceTypeRepository = residenceTypeRepository;
            _publicPlaceTypeRepository = publicPlaceTypeRepository;
            _loginSingleton = loginSingleton;
        }

        [HttpGet("Profile/Address")]
        public IActionResult ResidentialAddress()
        {
            try
            {
                if (_loginSingleton.CtmId == null || _loginSingleton.CtmId == 0) return RedirectToAction("LoginPage", "Login");

                ISpecification<Customer> spec = new GetCtmsBasicInfo((decimal)_loginSingleton.CtmId);

                var ctm = _customerService.Get(spec);
                if (ctm == null) return NotFound("O cliente não foi encontrado");

                return RedirectToAction("DetailedAddressPage", "ProfileAddress",
                    new { Type = (int)EAddressType.RESIDENTIAL, CtmId = ctm.CtmId, AddId = ctm.CtmAddId});
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Sucess = false,
                    ex.Message
                });
            }
        }

        [HttpGet("Profile/Addresses/{Type:int}/{AddId:decimal}")]
        public IActionResult DetailedAddressPage([FromRoute] decimal AddId, [FromRoute] int Type)
        {
            try
            {
                if (_loginSingleton.CtmId == null || _loginSingleton.CtmId == 0) return RedirectToAction("LoginPage", "Login");

                var add = _addressService.Get(AddId);
                if (add == null) return NotFound("O Id não foi encontrado");

                DetailedAddDTO addInfo = new DetailedAddDTO()
                {
                    Id = add.AddId,
                    CtmId = (decimal)_loginSingleton.CtmId,
                    Type = Type,
                    PublicPlace = add.AddPublicPlace,
                    PublicPlaceType = add.AddPptId,
                    ResidenceType = add.AddRstId,
                    Cep = add.AddCepStyled,
                    Number = add.AddNumber,
                    Neighborhood = add.AddNbh.NbhName,
                    City = add.AddNbh.NbhCty.CtyName,
                    State = add.AddNbh.NbhCty.CtyStt.SttName,
                    Country = add.AddNbh.NbhCty.CtyStt.SttCtr.CtrName,
                    ShortPhrase = add.AddShortPhrase,
                    Observations = add.AddObservations
                };

                ViewBag.RstList = _residenceTypeRepository.GetAll();
                ViewBag.PptList = _publicPlaceTypeRepository.GetAll();

                return View("~/Views/Customer/Profile/Address/Detailed/DetailedAddress.cshtml",addInfo);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Sucess = false,
                    ex.Message
                });
            }
        }

        [HttpGet("Profile/Addresses/{Type:int}")]
        public IActionResult AddressList([FromRoute] int Type)
        {
            try
            {
                if (_loginSingleton.CtmId == null || _loginSingleton.CtmId == 0) return RedirectToAction("LoginPage", "Login");

                var type = (EAddressType)Type;

                ISpecification<Customer> spec = (type == EAddressType.DELIVERY) ? new GetCtmsDelAddresses((int)_loginSingleton.CtmId) : new GetCtmBilAddresses((int)_loginSingleton.CtmId);

                var ctm = _customerService.Get(spec);
                if (ctm == null) return NotFound("O cliente não foi encontrado ou não existe");

                List<Address> addresses = (type == EAddressType.DELIVERY) ? ctm.DadAdds.ToList() : ctm.BadAdds.ToList();

                ViewBag.Type = Type;
                ViewBag.CtmId = ctm.CtmId;

                return View("~/Views/Customer/Profile/Address/ListAddresses.cshtml", addresses);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Sucess = false,
                    ex.Message
                });
            }
        }
    }
}
