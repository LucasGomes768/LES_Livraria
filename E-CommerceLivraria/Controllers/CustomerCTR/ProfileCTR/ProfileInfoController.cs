using E_CommerceLivraria.DTO.ProfileDTO.InfoDTO;
using E_CommerceLivraria.Repository.CustomerR.GenderR;
using E_CommerceLivraria.Services.CustomerS;
using E_CommerceLivraria.Services.CustomerS.TelephoneS;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceLivraria.Controllers.CustomerCTR.ProfileCTR
{
    public class ProfileInfoController : Controller
    {
        private ICustomerService _customerService;
        private ITelephoneTypeService _telephoneTypeService;
        private IGenderRepository _genderRepository;

        public ProfileInfoController(ICustomerService customerService,
            ITelephoneTypeService telephoneTypeService,
            IGenderRepository genderRepository)
        {
            _customerService = customerService;
            _telephoneTypeService = telephoneTypeService;
            _genderRepository = genderRepository;
        }

        [HttpGet("InfoProfile/{CtmId:decimal}")]
        public IActionResult ProfileInfo([FromRoute] decimal CtmId)
        {
            try
            {
                var customer = _customerService.Get(CtmId);
                if (customer == null) throw new Exception("Cliente não foi encontrado");

                ViewBag.TlpTypes = _telephoneTypeService.GetAll();
                ViewBag.Genders = _genderRepository.GetAll();

                InfoDTO info = new InfoDTO()
                {
                    Id = customer.CtmId,
                    Name = customer.CtmName,
                    Pass = customer.CtmPass,
                    Email = customer.CtmEmail,
                    Cpf = customer.CtmCpfStyled,
                    Ddd = customer.CtmTlp.TlpDdd,
                    TlpNum = customer.CtmTlp.TlpNumber,
                    Tpt = customer.CtmTlp.TlpTpt.TptId,
                    Gender = customer.CtmGndId,
                    BirthDate = customer.CtmBirthdate
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
