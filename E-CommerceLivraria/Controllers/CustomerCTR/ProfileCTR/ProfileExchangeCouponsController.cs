using E_CommerceLivraria.Services.CustomerS;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceLivraria.Controllers.CustomerCTR.ProfileCTR
{
    public class ProfileExchangeCouponsController : Controller
    {
        private readonly ICustomerService _customerService;

        public ProfileExchangeCouponsController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("CouponsProfile/{CtmId:decimal}")]
        public IActionResult CouponsList([FromRoute] decimal CtmId)
        {
            try
            {
                var customer = _customerService.Get(CtmId);
                if (customer == null) throw new Exception("Cliente não foi encontrado");

                return View("~/Views/Customer/Profile/Coupons/CouponsList.cshtml", customer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
