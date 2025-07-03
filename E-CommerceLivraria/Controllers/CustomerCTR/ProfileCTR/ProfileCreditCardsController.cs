using E_CommerceLivraria.Models;
using E_CommerceLivraria.Services.CreditCardS;
using E_CommerceLivraria.Services.CustomerS;
using E_CommerceLivraria.Services.LoginS;
using E_CommerceLivraria.Specifications;
using E_CommerceLivraria.Specifications.CustomerSpecs.CreditCards;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceLivraria.Controllers.CustomerCTR.ProfileCTR
{
    public class ProfileCreditCardsController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly ICreditCardService _creditCardService;
        private readonly LoginSingleton _loginSingleton;

        public ProfileCreditCardsController(ICustomerService customerService, ICreditCardService creditCardService, LoginSingleton loginSingleton)
        {
            _customerService = customerService;
            _creditCardService = creditCardService;
            _loginSingleton = loginSingleton;
        }

        [HttpGet("CreditCardsProfile")]
        public IActionResult CreditCardsList()
        {
            try
            {
                if (_loginSingleton.CtmId == null || _loginSingleton.CtmId == 0) return RedirectToAction("LoginPage", "Login");

                int id = (int)_loginSingleton.CtmId;
                ISpecification<Customer> spec = new GetCtmsCreditCards(id);

                var ctm = _customerService.Get(spec);
                if (ctm == null) return NotFound("O cliente não foi encontrado ou não existe");

                return View("~/Views/Customer/Profile/CreditCards/CreditCardsList.cshtml", ctm);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
