using E_CommerceLivraria.DTO.AdmCustomerDTO;
using E_CommerceLivraria.Models;
using E_CommerceLivraria.Models.ModelsStructGroups;
using E_CommerceLivraria.Repository.AddressR;
using E_CommerceLivraria.Repository.CustomerR.GenderR;
using E_CommerceLivraria.Services.CustomerS;
using E_CommerceLivraria.Services.CustomerS.TelephoneS;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceLivraria.Controllers.AdminCTR
{
    public class AdmCustomerController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly IGenderRepository _genderRepository;
        private readonly ITelephoneTypeService _telephoneTypeService;
        private readonly IPublicPlaceTypeRepository _publicPlaceTypeRepository;
        private readonly IResidenceTypeRepository _residenceTypeRepository;

        public AdmCustomerController(ICustomerService customerService, IGenderRepository genderRepository, ITelephoneTypeService telephoneTypeService, IPublicPlaceTypeRepository publicPlaceTypeRepository, IResidenceTypeRepository residenceTypeRepository)
        {
            _customerService = customerService;
            _genderRepository = genderRepository;
            _telephoneTypeService = telephoneTypeService;
            _publicPlaceTypeRepository = publicPlaceTypeRepository;
            _residenceTypeRepository = residenceTypeRepository;
        }

        [HttpGet("AdmCustomer/ReadAll")]
        public IActionResult Read()
        {
            ReadAllCustomerDTO rdg = new ReadAllCustomerDTO
            {
                Customers = _customerService.GetAll(),
                Genders = _genderRepository.GetAll(),
                TlpTypes = _telephoneTypeService.GetAll()
            };

            return View("~/Views/Admin/crud/customer/readAll/AllCustomers.cshtml", rdg);
        }

        public IActionResult FilterRead(ReadAllCustomerDTO rdg)
        {
            IEnumerable<Customer> query = _customerService.GetAll();

            CustomerFilterDTO filter = rdg.FilterData;

            // Nome
            if (filter.Name != null && query.Count() != 0)
            {
                query = query.Where(x => x.CtmName == filter.Name);
            }
            // CPF
            if (filter.Cpf != null && query.Count() != 0)
            {
                query = query.Where(x => x.CtmCpf == filter.Cpf);
            }
            // Tipo do Telefone
            if (filter.TelephoneTypeId != null && query.Count() != 0)
            {
                query = query.Where(x => x.CtmTlp.TlpTptId == filter.TelephoneTypeId);
            }
            // E-Mail
            if (filter.Email != null && query.Count() != 0)
            {
                query = query.Where(x => x.CtmEmail == filter.Email);
            }
            // Gênero
            if (filter.GndId != null && query.Count() != 0)
            {
                query = query.Where(x => x.CtmGndId == filter.GndId);
            }
            // Telefone
            if (filter.Active != null && query.Count() != 0)
            {
                query = query.Where(x => x.CtmActive == filter.Active);
            }
            // Idade Mínima
            if ((filter.MinAge != null || filter.MinAge > 0) && query.Count() != 0)
            {
                query = query.Where(x => filter.MinAge <= DateTime.Now.Year - x.CtmBirthdate.Year);
            }
            // Idade Máxima
            if ((filter.MaxAge != null || filter.MaxAge > 0) && query.Count() != 0)
            {
                query = query.Where(x => DateTime.Now.Year - x.CtmBirthdate.Year <= filter.MaxAge);
            }
            // Ranking
            if ((filter.Ranking != null || filter.Ranking > 0) && query.Count() != 0)
            {
                query = query.Where(x => x.CtmRanking == filter.Ranking);
            }

            ReadAllCustomerDTO newRdg = new ReadAllCustomerDTO
            {
                Customers = query.ToList(),
                Genders = _genderRepository.GetAll(),
                TlpTypes = _telephoneTypeService.GetAll()
            };

            return View("~/Views/Admin/Customer/Read.cshtml", newRdg);
        }

        [HttpGet("AdmCustomer/Register")]
        public IActionResult CreateCustomerPage()
        {
            ViewBag.Genders = _genderRepository.GetAll();
            ViewBag.TelTypes = _telephoneTypeService.GetAll();
            ViewBag.PpTypes = _publicPlaceTypeRepository.GetAll();
            ViewBag.ResTypes = _residenceTypeRepository.GetAll();

            return View("~/Views/Admin/crud/customer/create/createCustomer.cshtml");
        }

        [HttpPost]
        public IActionResult RegisterCustomer(CreateCustomerDTO createCustomerDTO)
        {
            try
            {
                if (createCustomerDTO == null) return BadRequest("Nenhum dado enviado");

                _customerService.Create(createCustomerDTO);

                return RedirectToAction("CreateCustomerPage", "AdmCustomer");
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
