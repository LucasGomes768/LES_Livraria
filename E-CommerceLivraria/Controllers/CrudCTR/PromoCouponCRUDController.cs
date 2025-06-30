using E_CommerceLivraria.DTO.CouponsDTO;
using E_CommerceLivraria.Services.CouponS;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceLivraria.Controllers.CrudCTR
{
    public class PromoCouponCRUDController : Controller
    {
        private readonly IPromotionalCouponService _promotionalCouponService;

        public PromoCouponCRUDController(IPromotionalCouponService promotionalCouponService)
        {
            _promotionalCouponService = promotionalCouponService;
        }

        [HttpGet("CRUD/PromoCoupons/All")]
        public IActionResult GetAllPromoCoupons()
        {
            try
            {
                var cpns = _promotionalCouponService.GetAll();

                return Ok(new
                {
                    Sucess = true,
                    cpns
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Sucess = false,
                    Message = ex.Message
                });
            }
        }

        [HttpPost("CRUD/PromoCoupons/Add")]
        public IActionResult createPromoCoupons([FromBody] CreatePromoCouponsDTO cpc)
        {
            try
            {
                if (cpc == null) return BadRequest("Nenhum valor foi recebido");

                var cpn = _promotionalCouponService.Create(cpc.Value, cpc.Code);

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

        [HttpPost("CRUD/PromoCoupons/Deactivate/{id:int}")]
        public IActionResult DeactivatePromoCoupon([FromRoute] int id)
        {
            try
            {
                if (id == 0) return BadRequest("Nenhum valor enviado");

                _promotionalCouponService.Deactivate(id);

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
