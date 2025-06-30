using E_CommerceLivraria.Services.CouponS;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceLivraria.Controllers.AdminCTR
{
    public class AdmPromotionalCouponsController : Controller
    {
        private readonly IPromotionalCouponService _promotionalCouponService;

        public AdmPromotionalCouponsController(IPromotionalCouponService promotionalCouponService)
        {
            _promotionalCouponService = promotionalCouponService;
        }

        [HttpGet]
        public IActionResult PromoCouponsPage()
        {
            var cpns = _promotionalCouponService.GetAllActive();

            return View("~/Views/Admin/coupons/promoCoupons.cshtml", cpns);
        }
    }
}
