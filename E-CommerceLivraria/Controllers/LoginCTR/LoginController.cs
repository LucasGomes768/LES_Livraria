using E_CommerceLivraria.DTO.LoginDTO;
using E_CommerceLivraria.Models;
using E_CommerceLivraria.Services.CustomerS;
using E_CommerceLivraria.Services.LoginS;
using E_CommerceLivraria.Specifications;
using E_CommerceLivraria.Specifications.CustomerSpecs;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceLivraria.Controllers.LoginCTR
{
    public class LoginController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly LoginSingleton _loginSingleton;

        public LoginController(ICustomerService customerService, LoginSingleton loginSingleton)
        {
            _customerService = customerService;
            _loginSingleton = loginSingleton;
        }

        public IActionResult LoginPage()
        {
            if (_loginSingleton.CtmId != null)
                _loginSingleton.CtmId = null;

            List<LoginDTO> logins = new List<LoginDTO>();

            ISpecification<Customer> spec = new GetCtmsBasicInfo();
            List<Customer> allActive = _customerService.GetAll(spec);

            foreach (Customer ctm in allActive) {
                logins.Add(new LoginDTO()
                {
                    CtmId = ctm.CtmId,
                    CtmName = ctm.CtmName
                });
            }

            return View("~/Views/Login/Login.cshtml", logins);
        }

        [HttpGet("LoginAs/{ctmId:int}")]
        public IActionResult LoginAs([FromRoute] int ctmId)
        {
            try
            {
                ISpecification<Customer> spec = new GetCtmsBasicInfo(ctmId);

                var ctm = _customerService.Get(spec);
                if (ctm == null) return NotFound("Cliente não foi encontrado");

                _loginSingleton.CtmId = ctm.CtmId;

                return RedirectToAction("HomePage", "Home");
            } catch (Exception ex)
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
