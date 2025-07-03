using E_CommerceLivraria.Models;
using E_CommerceLivraria.Services.CustomerS;
using E_CommerceLivraria.Services.LoginS;
using E_CommerceLivraria.Specifications;
using E_CommerceLivraria.Specifications.CustomerSpecs.Coupons;
using E_CommerceLivraria.Specifications.CustomerSpecs.CreditCards;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceLivraria.Controllers.CustomerCTR.ProfileCTR
{
    public class ProfileExchangeCouponsController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly LoginSingleton _loginSingleton;

        public ProfileExchangeCouponsController(ICustomerService customerService, LoginSingleton loginSingleton)
        {
            _customerService = customerService;
            _loginSingleton = loginSingleton;
        }

        [HttpGet("CouponsProfile")]
        public IActionResult CouponsList()
        {
            try
            {
                if (_loginSingleton.CtmId == null || _loginSingleton.CtmId == 0) return RedirectToAction("LoginPage", "Login");

                int id = (int)_loginSingleton.CtmId;
                ISpecification<Customer> spec = new GetCtmsExchangeCoupons(id);

                var ctm = _customerService.Get(spec);
                if (ctm == null) return NotFound("O cliente não foi encontrado ou não existe");

                return View("~/Views/Customer/Profile/Coupons/CouponsList.cshtml", ctm);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
