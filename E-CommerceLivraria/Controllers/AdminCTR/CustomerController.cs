using E_CommerceLivraria.Models;
using E_CommerceLivraria.Models.ModelsStructGroups;
using E_CommerceLivraria.Repository.AddressR;
using E_CommerceLivraria.Repository.CustomerR.GenderR;
using E_CommerceLivraria.Services.AddressS;
using E_CommerceLivraria.Services.CustomerS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace E_CommerceLivraria.Controllers.AdminCTR
{
    public class CustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IGenderRepository _genderRepository;
        private readonly IPublicPlaceTypeRepository _publicPlaceTypeRepository;
        private readonly IResidenceTypeRepository _residenceTypeRepository;
        private readonly ITelephoneTypeService _telephoneTypeService;
        private readonly IAddressService _addressService;
        private readonly ICartService _cartService;
        private readonly ITelephoneService _telephoneService;

        public CustomerController(ICustomerService customerService,
            IGenderRepository genderRepository, IPublicPlaceTypeRepository publicPlaceTypeRepository,
            IResidenceTypeRepository residenceTypeRepository, IAddressService addressService,
            ICartService cartService, ITelephoneService telephoneService,
            ITelephoneTypeService telephoneTypeService) {

            _customerService = customerService;
            _genderRepository = genderRepository;
            _publicPlaceTypeRepository = publicPlaceTypeRepository;
            _residenceTypeRepository = residenceTypeRepository;
            _addressService = addressService;
            _cartService = cartService;
            _telephoneService = telephoneService;
            _telephoneTypeService = telephoneTypeService;
        }

        public IActionResult Read()
        {
            ReadData rdg = new ReadData {
                Customers = _customerService.GetAll(),
                Genders = _genderRepository.GetAll(),
                TlpTypes = _telephoneTypeService.GetAll()
            };

            return View("~/Views/Admin/crud/customer/readAll/AllCustomers.cshtml", rdg);
        }

        public IActionResult FilterRead(ReadData rdg) {
            IEnumerable<Customer> query = _customerService.GetAll();

            CustomerFilterData filter = rdg.FilterData;

            // Nome
            if (filter.Name != null && query.Count() != 0) {
                query = query.Where(x => x.CtmName == filter.Name);
            }
            // CPF
            if (filter.Cpf != null && query.Count() != 0) {
                query = query.Where(x => x.CtmCpf == filter.Cpf);
            }
            // Tipo do Telefone
            if (filter.TelephoneTypeId != null && query.Count() != 0) {
                query = query.Where(x => x.CtmTlp.TlpTptId == filter.TelephoneTypeId);
            }
            // E-Mail
            if (filter.Email != null && query.Count() != 0) {
                query = query.Where(x => x.CtmEmail == filter.Email);
            }
            // Gênero
            if (filter.GndId != null && query.Count() != 0) {
                query = query.Where(x => x.CtmGndId == filter.GndId);
            }
            // Telefone
            if (filter.Active != null && query.Count() != 0) {
                query = query.Where(x => x.CtmActive == filter.Active);
            }
            // Idade Mínima
            if ((filter.MinAge != null || filter.MinAge > 0) && query.Count() != 0) {
                query = query.Where(x => filter.MinAge <= DateTime.Now.Year - x.CtmBirthdate.Year);
            }
            // Idade Máxima
            if ((filter.MaxAge != null || filter.MaxAge > 0) && query.Count() != 0) {
                query = query.Where(x => DateTime.Now.Year - x.CtmBirthdate.Year <= filter.MaxAge);
            }
            // Ranking
            if ((filter.Ranking != null || filter.Ranking > 0) && query.Count() != 0) {
                query = query.Where(x => x.CtmRanking == filter.Ranking);
            }

            ReadData newRdg = new ReadData {
                Customers = query.ToList(),
                Genders = _genderRepository.GetAll(),
                TlpTypes = _telephoneTypeService.GetAll()
            };

            return View("~/Views/Admin/Customer/Read.cshtml", newRdg);
        }

        public IActionResult Create() {
            CreateData cdg = new CreateData {
                Genders = _genderRepository.GetAll(),
                PublicplaceTypes = _publicPlaceTypeRepository.GetAll(),
                ResidenceTypes = _residenceTypeRepository.GetAll()
            };

            return View("~/Views/Admin/Customer/Create.cshtml", cdg);
        }

        [Route("Customer/Alter/{id:decimal}")]
        public IActionResult Alter(decimal id) {
            var ctm = _customerService.Get(id);

            if (ctm == null) {
                return NotFound();
            }

            UpdateData udg = new UpdateData {
                Ctm = ctm,
                Genders = _genderRepository.GetAll(),
                PublicplaceTypes = _publicPlaceTypeRepository.GetAll(),
                ResidenceTypes = _residenceTypeRepository.GetAll()
            };

            return View("~/Views/Admin/Customer/Update.cshtml", udg);
        }

        [Route("Customer/{id:decimal}")]
        public IActionResult Detailed(decimal id) {
            var ctm = _customerService.Get(id);


            if (ctm == null) {
                return NotFound();
            }

            if (ViewData == null) {
                ViewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary());
            }

            return View("~/Views/Admin/Customer/Detailed.cshtml", ctm);
        }

        public IActionResult Remove(decimal id) {
            _customerService.Remove(id);
            return Read();
        }

        [HttpPost]
        public IActionResult Create(CreateData createDataGroup) {

            Customer newCtm = createDataGroup.Ctm;

            newCtm.CtmBirthdate = DateTime.Parse(createDataGroup.Birthdate);

            newCtm.CtmAdd = _addressService.Create(newCtm.CtmAdd);

            var gndTemp = _genderRepository.Get(newCtm.CtmGndId);
            if(gndTemp == null) {
                return View("~/Views/Admin/Customer/Create.cshtml", createDataGroup);
            }

            Address newDelAdd = _addressService.Create(createDataGroup.Delivery);
            newCtm.DadAdds.Add(newDelAdd);
            newDelAdd.DadCtms.Add(newCtm);

            Address newBilAdd = _addressService.Create(createDataGroup.Shipping);
            newCtm.BadAdds.Add(newBilAdd);
            newBilAdd.BadCtms.Add(newCtm);

            newCtm.CtmTlp = _telephoneService.Create(newCtm.CtmTlp);

            _customerService.Create(newCtm);

            newCtm.Cart = _cartService.Create(newCtm);

            return RedirectToAction("Create");
        }

        [HttpPost]
        public IActionResult Update(UpdateData udg) {
            Customer ctm = udg.Ctm;

            ctm.CtmBirthdate = DateTime.Parse(udg.Birthdate);

            _customerService.Update(ctm);

            return Detailed(ctm.CtmId);
        }

        public IActionResult UpdatePassword(Customer data) {
            var ctm = _customerService.Get(data.CtmId);

            if (ctm == null) throw new Exception("Um cliente com esse ID não foi encontrado");
            ctm.CtmPass = data.CtmPass;

            UpdateData udg = new UpdateData {
                Ctm = ctm,
                Birthdate = ctm.CtmBirthdate.ToString("yyyy-MM-dd")
            };

            return Update(udg);
        }


    }
}
