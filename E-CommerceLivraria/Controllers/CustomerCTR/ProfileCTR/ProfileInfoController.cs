using E_CommerceLivraria.DTO.ProfileDTO.InfoDTO;
using E_CommerceLivraria.Models;
using E_CommerceLivraria.Repository.CustomerR.GenderR;
using E_CommerceLivraria.Services.CustomerS;
using E_CommerceLivraria.Services.CustomerS.TelephoneS;
using E_CommerceLivraria.Services.LoginS;
using E_CommerceLivraria.Specifications;
using E_CommerceLivraria.Specifications.CustomerSpecs;
using E_CommerceLivraria.Specifications.CustomerSpecs.Purchases;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceLivraria.Controllers.CustomerCTR.ProfileCTR
{
    public class ProfileInfoController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly ITelephoneTypeService _telephoneTypeService;
        private readonly IGenderRepository _genderRepository;
        private readonly LoginSingleton _loginSingleton;

        public ProfileInfoController(ICustomerService customerService, ITelephoneTypeService telephoneTypeService, IGenderRepository genderRepository, LoginSingleton loginSingleton)
        {
            _customerService = customerService;
            _telephoneTypeService = telephoneTypeService;
            _genderRepository = genderRepository;
            _loginSingleton = loginSingleton;
        }

        [HttpGet("InfoProfile")]
        public IActionResult ProfileInfo()
        {
            try
            {
                if (_loginSingleton.CtmId == null || _loginSingleton.CtmId == 0) return RedirectToAction("LoginPage", "Login");

                int id = (int)_loginSingleton.CtmId;
                ISpecification<Customer> spec = new GetCtmsBasicInfo(id);

                var ctm = _customerService.Get(spec);
                if (ctm == null) return NotFound("Cliente não foi encontrado ou não existe");

                ViewBag.TlpTypes = _telephoneTypeService.GetAll();
                ViewBag.Genders = _genderRepository.GetAll();

                InfoDTO info = new InfoDTO()
                {
                    Id = ctm.CtmId,
                    Name = ctm.CtmName,
                    Pass = ctm.CtmPass,
                    Email = ctm.CtmEmail,
                    Cpf = ctm.CtmCpfStyled,
                    Ddd = ctm.CtmTlp.TlpDdd,
                    TlpNum = ctm.CtmTlp.TlpNumber,
                    Tpt = ctm.CtmTlp.TlpTpt.TptId,
                    Gender = ctm.CtmGndId,
                    BirthDate = ctm.CtmBirthdate
                };

                return View("~/Views/Customer/Profile/Info/Info.cshtml", info);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
