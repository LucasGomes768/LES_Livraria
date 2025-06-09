using E_CommerceLivraria.DTO.ProfileDTO.AddressDTO;
using E_CommerceLivraria.Models;
using E_CommerceLivraria.Repository.AddressR;
using E_CommerceLivraria.Services.AddressS;
using E_CommerceLivraria.Services.CustomerS;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceLivraria.Controllers.CustomerCTR.ProfileCTR
{
    public class ProfileAddressController : Controller
    {
        readonly private ICustomerService _customerService;
        readonly private IAddressService _addressService;
        readonly private IResidenceTypeRepository _residenceTypeRepository;
        readonly private IPublicPlaceTypeRepository _publicPlaceTypeRepository;

        public ProfileAddressController(ICustomerService customerService,
            IAddressService addressService,
            IResidenceTypeRepository residenceTypeRepository,
            IPublicPlaceTypeRepository publicPlaceTypeRepository)
        {
            _customerService = customerService;
            _addressService = addressService;
            _residenceTypeRepository = residenceTypeRepository;
            _publicPlaceTypeRepository = publicPlaceTypeRepository;
        }

        [HttpGet("Profile/Address/{CtmId:decimal}")]
        public IActionResult ResidentialAddress([FromRoute] decimal CtmId)
        {
            try
            {
                var ctm = _customerService.Get(CtmId);
                if (ctm == null) return NotFound("O cliente não foi encontrado");

                return RedirectToAction("DetailedAddressPage", "ProfileAddress",
                    new { Type = "Residential", CtmId = ctm.CtmId, AddId = ctm.CtmAddId});
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

        [HttpGet("Profile/Addresses/{Type}/{CtmId:decimal}/{AddId:decimal}")]
        public IActionResult DetailedAddressPage([FromRoute] decimal CtmId, [FromRoute] decimal AddId, [FromRoute] string Type)
        {
            try
            {
                var add = _addressService.Get(AddId);
                if (add == null) return NotFound("O Id não foi encontrado");

                DetailedAddDTO addInfo = new DetailedAddDTO()
                {
                    Id = add.AddId,
                    CtmId = CtmId,
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

        [HttpGet("Profile/Addresses/{Type}/{CtmId:decimal}")]
        public IActionResult AddressList([FromRoute] string Type, [FromRoute] decimal CtmId)
        {
            try
            {
                var ctm = _customerService.Get(CtmId);
                if (ctm == null) return NotFound("Cliente não foi encontrado");

                List<Address> addresses = new List<Address>();

                if (Type.ToLower() == "delivery")
                {
                    addresses = ctm.DadAdds.ToList();
                }
                else
                {
                    addresses = ctm.BadAdds.ToList();
                }

                ViewBag.Type = Type;
                ViewBag.CtmId = CtmId;

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
