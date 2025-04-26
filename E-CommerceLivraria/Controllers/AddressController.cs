using System.Numerics;
using E_CommerceLivraria.Data;
using E_CommerceLivraria.Enums;
using E_CommerceLivraria.Models;
using E_CommerceLivraria.Models.ModelsStructGroups.AddressSG;
using E_CommerceLivraria.Repository.AddressR;
using E_CommerceLivraria.Services.AddressS;
using E_CommerceLivraria.Services.CustomerS;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceLivraria.Controllers
{
    public class AddressController : Controller
    {
        private readonly IAddressService _addressService;
        private readonly ICustomerService _customerService;
        private readonly IPublicPlaceTypeRepository _publicPlaceTypeRepository;
        private readonly IResidenceTypeRepository _residenceTypeRepository;

        public AddressController(IAddressService addressService,
            ICustomerService customerService,
            IPublicPlaceTypeRepository publicPlaceTypeRepository,
            IResidenceTypeRepository residenceTypeRepository) {

            _addressService = addressService;
            _customerService = customerService;
            _publicPlaceTypeRepository = publicPlaceTypeRepository;
            _residenceTypeRepository = residenceTypeRepository;
        }

        [HttpGet("Address/Detailed/{idGroup}")]
        public IActionResult Detailed([FromRoute] string idGroup) {
            string[] vet = idGroup.Split(",");

            if (vet.Length < 3 || !int.TryParse(vet[0], out int addressId) || !int.TryParse(vet[1], out int ctmId) || !int.TryParse(vet[2], out int addType)) {
                return BadRequest("ID inválido");
            }

            var address = _addressService.Get(int.Parse(vet[0]));
            if (address == null) return NotFound();

            var ctm = _customerService.Get(int.Parse(vet[1]));
            if (ctm == null) return NotFound();

            CreateAddressGroup cag = new CreateAddressGroup {
                Address = address,
                Ctm = ctm,
                Type = (EAddressType)addType
            };

            return View("~/Views/Admin/Address/detailedAddress.cshtml", cag);
        }

        [HttpGet("Address/Create/{dataGroup}")]
        public IActionResult Create([FromRoute] string dataGroup) {
            string[] vet = dataGroup.Split(",");
            decimal ctmId = decimal.Parse(vet[0]);
            EAddressType addType = (EAddressType)int.Parse(vet[1]);

            var ctm = _customerService.Get(ctmId);
            if (ctm == null) return NotFound();

            CreateAddressGroup cag = new CreateAddressGroup {
                Ctm = ctm,
                Type = addType,
                PublicplaceTypes = _publicPlaceTypeRepository.GetAll(),
                ResidenceTypes = _residenceTypeRepository.GetAll()
            };

            return View("~/Views/Admin/Address/createAddress.cshtml", cag);
        }

        [HttpGet("Address/Alter/{dataGroup}")]
        public IActionResult Alter([FromRoute] string dataGroup) {
            string[] ids = dataGroup.Split(",");
            decimal addId = decimal.Parse(ids[0]);
            decimal ctmId = decimal.Parse(ids[1]);
            int addType = int.Parse(ids[2]);

            var add = _addressService.Get(addId);
            if (add == null) return NotFound();

            var ctm = _customerService.Get(ctmId);
            if (ctm == null) return NotFound();

            CreateAddressGroup cag = new CreateAddressGroup {
                Address = add,
                Ctm = ctm,
                Type = (EAddressType)addType,
                PublicplaceTypes = _publicPlaceTypeRepository.GetAll(),
                ResidenceTypes = _residenceTypeRepository.GetAll()
            };

            return View("~/Views/Admin/Address/updateAddress.cshtml", cag);
        }

        [HttpPost]
        public IActionResult Update(CreateAddressGroup cag) {
            _addressService.Update(cag.Address);
            var ctm = _customerService.Get(cag.Ctm.CtmId);

            return View("~/Views/Admin/Customer/Detailed.cshtml", ctm);
        }

        [HttpPost]
        public IActionResult Add(CreateAddressGroup cag) {
            var ctm = _customerService.Get(cag.Ctm.CtmId);
            if (ctm == null) throw new Exception("Um cliente com esse ID não foi encontrado");

            var add = _addressService.Create(cag.Address);
            if (cag.Type == EAddressType.BILLING) {
                ctm.BadAdds.Add(add);
            } else {
                ctm.DadAdds.Add(add);
            }

            _customerService.Update(ctm);
            string crtTxt = ctm.CtmId.ToString() + "," + (int)cag.Type;

            return Create(crtTxt);
        }

        [HttpGet("Address/RemoveRelation/{removeData}")]
        public IActionResult RemoveRelation([FromRoute] string removeData) {
            string[] ids = removeData.Split(",");
            decimal addId = decimal.Parse(ids[0]);
            decimal ctmId = decimal.Parse(ids[1]);
            EAddressType addType = (EAddressType)int.Parse(ids[2]);

            var ctm = _customerService.Get(ctmId);
            if (ctm == null) throw new Exception("Um cliente com esse ID não foi encontrado");

            var add = _addressService.Get(addId);
            if (add == null) return NotFound();

            if (addType == EAddressType.BILLING)
                ctm.BadAdds.Remove(add);
            else
                ctm.DadAdds.Remove(add);

            _customerService.Update(ctm);

            return View("~/Views/Admin/Customer/Detailed.cshtml", ctm);
        }
    }
}
