using E_CommerceLivraria.DTO.AdmCustomerDTO;
using E_CommerceLivraria.Extensions.Specifications;
using E_CommerceLivraria.Models;
using E_CommerceLivraria.Repository.AddressR;
using E_CommerceLivraria.Repository.CustomerR.GenderR;
using E_CommerceLivraria.Services.CouponS;
using E_CommerceLivraria.Services.CustomerS;
using E_CommerceLivraria.Services.CustomerS.TelephoneS;
using E_CommerceLivraria.Specifications;
using E_CommerceLivraria.Specifications.CustomerSpecs;
using E_CommerceLivraria.Specifications.CustomerSpecs.Addresses;
using E_CommerceLivraria.Specifications.CustomerSpecs.Coupons;
using E_CommerceLivraria.Specifications.CustomerSpecs.CreditCards;
using E_CommerceLivraria.Specifications.CustomerSpecs.Purchases;
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
        private readonly IPromoCouponAssignmentService _promoCouponAssignmentService;

        public AdmCustomerController(ICustomerService customerService, IGenderRepository genderRepository, ITelephoneTypeService telephoneTypeService, IPublicPlaceTypeRepository publicPlaceTypeRepository, IResidenceTypeRepository residenceTypeRepository, IPromoCouponAssignmentService promoCouponAssignmentService)
        {
            _customerService = customerService;
            _genderRepository = genderRepository;
            _telephoneTypeService = telephoneTypeService;
            _publicPlaceTypeRepository = publicPlaceTypeRepository;
            _residenceTypeRepository = residenceTypeRepository;
            _promoCouponAssignmentService = promoCouponAssignmentService;
        }

        [HttpGet("AdmCustomer/ReadAll")]
        public IActionResult Read()
        {
            var specs = new GetCtmsBasicInfo();

            ReadAllCustomerDTO rdg = new ReadAllCustomerDTO
            {
                Customers = _customerService.GetAll(specs),
                Genders = _genderRepository.GetAll(),
                TlpTypes = _telephoneTypeService.GetAll()
            };

            return View("~/Views/Admin/crud/customer/readAll/AllCustomers.cshtml", rdg);
        }

        public IActionResult FilterRead(ReadAllCustomerDTO rdg)
        {
            ISpecification<Customer> spec = new GetCtmsBasicInfo();

            IEnumerable<Customer> query = _customerService.GetAll(spec);

            CustomerFilterDTO filter = rdg.FilterData;

            // Nome
            if (filter.Name != null && query.Any())
            {
                query = query.Where(x => x.CtmName.Contains(filter.Name));
            }
            // CPF
            if (filter.Cpf != null && query.Any())
            {
                query = query.Where(x => x.CtmCpf == decimal.Parse(filter.Cpf.Replace(".","").Replace("-","")));
            }
            // Tipo do Telefone
            if (filter.TelephoneTypeId != null && query.Any())
            {
                query = query.Where(x => x.CtmTlp.TlpTptId == filter.TelephoneTypeId);
            }
            // E-Mail
            if (filter.Email != null && query.Any())
            {
                query = query.Where(x => x.CtmEmail.Contains(filter.Email));
            }
            // Gênero
            if (filter.GndId != null && query.Any())
            {
                query = query.Where(x => x.CtmGndId == filter.GndId);
            }
            // Telefone
            if (filter.Active != null && query.Any())
            {
                query = query.Where(x => x.CtmActive == filter.Active);
            }
            // Idade Mínima
            if ((filter.MinAge != null && filter.MinAge > 0) && query.Any())
            {
                query = query.Where(x => filter.MinAge <= DateTime.Now.Year - x.CtmBirthdate.Year);
            }
            // Idade Máxima
            if ((filter.MaxAge != null && filter.MaxAge > 0) && query.Any())
            {
                query = query.Where(x => DateTime.Now.Year - x.CtmBirthdate.Year <= filter.MaxAge);
            }
            // Ranking
            if ((filter.Ranking != null && filter.Ranking > 0) && query.Any())
            {
                query = query.Where(x => x.CtmRanking == filter.Ranking);
            }

            ReadAllCustomerDTO newRdg = new ReadAllCustomerDTO
            {
                Customers = query.ToList(),
                Genders = _genderRepository.GetAll(),
                TlpTypes = _telephoneTypeService.GetAll()
            };

            return View("~/Views/Admin/crud/customer/readAll/AllCustomers.cshtml", newRdg);
        }

        [HttpGet("AdmCustomer/{id:int}")]
        public IActionResult DetailedCustomerPage([FromRoute] int id)
        {
            try
            {
                ISpecification<Customer> specBsc = new GetCtmsBasicInfo(id);
                ISpecification<Customer> specBil = new GetCtmBilAddresses(id);
                ISpecification<Customer> specDel = new GetCtmsDelAddresses(id);
                ISpecification<Customer> specCrd = new GetCtmsCreditCards(id);
                ISpecification<Customer> specXcp = new GetCtmsExchangeCoupons(id);
                ISpecification<Customer> specPrc = new GetCtmsPurchases(id);

                var combinedSpecs = specBsc
                    .And(specBil)
                    .And(specPrc)
                    .And(specXcp)
                    .And(specCrd)
                    .And(specDel);

                var ctm = _customerService.Get(combinedSpecs);
                if (ctm == null) throw new Exception("Cliente não foi encontrado ou não existe");

                ViewBag.Genders = _genderRepository.GetAll();
                ViewBag.TlpTypes = _telephoneTypeService.GetAll();
                ViewBag.RstList = _residenceTypeRepository.GetAll();
                ViewBag.PptList = _publicPlaceTypeRepository.GetAll();

                return View("~/Views/Admin/crud/customer/detailed/DetailedCustomer.cshtml", ctm);

            } catch (Exception ex) {
                return RedirectToAction("Read");
            }
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

                var ctm = _customerService.Create(createCustomerDTO);
                _promoCouponAssignmentService.AddAllPromoCouponToCtm(ctm);

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

        [HttpPost("AdmCustomer/Deactivate/{id:int}")]
        public IActionResult DeactivateCustomer([FromRoute] int id)
        {
            try
            {
                ISpecification<Customer> spec = new GetCtmsBasicInfo(id);

                var ctm = _customerService.Get(spec);
                if (ctm == null) return NotFound("Cliente não foi encontrado ou não existe");

                _customerService.Deactivate(ctm);

                return Ok(new
                {
                    Sucess = true
                });
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

        [HttpPost("AdmCustomer/Activate/{id:int}")]
        public IActionResult ActivateCustomer([FromRoute] int id)
        {
            try
            {
                ISpecification<Customer> spec = new GetCtmsBasicInfo(id);

                var ctm = _customerService.Get(spec);
                if (ctm == null) return NotFound("Cliente não foi encontrado ou não existe");

                _customerService.Activate(ctm);

                return Ok(new
                {
                    Sucess = true
                });
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
