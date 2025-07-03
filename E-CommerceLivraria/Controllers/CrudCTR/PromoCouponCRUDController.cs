using E_CommerceLivraria.DTO.CouponsDTO;
using E_CommerceLivraria.Models;
using E_CommerceLivraria.Services.CouponS;
using E_CommerceLivraria.Services.CustomerS;
using E_CommerceLivraria.Services.LoginS;
using E_CommerceLivraria.Specifications;
using E_CommerceLivraria.Specifications.CustomerSpecs.Coupons;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace E_CommerceLivraria.Controllers.CrudCTR
{
    public class PromoCouponCRUDController : Controller
    {
        private readonly IPromotionalCouponService _promotionalCouponService;
        private readonly IPromoCouponAssignmentService _promoCouponAssignmentService;
        private readonly ICustomerService _customerService;
        private readonly LoginSingleton _loginSingleton;

        public PromoCouponCRUDController(IPromotionalCouponService promotionalCouponService, IPromoCouponAssignmentService promoCouponAssignmentService, ICustomerService customerService, LoginSingleton loginSingleton)
        {
            _promotionalCouponService = promotionalCouponService;
            _promoCouponAssignmentService = promoCouponAssignmentService;
            _customerService = customerService;
            _loginSingleton = loginSingleton;
        }

        [HttpGet("CRUD/PromoCoupons/{ctmId:int}/{code}")]
        public IActionResult GetPromoCouponByCode([FromRoute] int ctmId, [FromRoute] string code)
        {
            try
            {
                int id = (int?)_loginSingleton.CtmId ?? ctmId;

                ISpecification <Customer> spec = new GetCtmsPromoCoupons(ctmId);

                var ctm = _customerService.Get(spec);
                if (ctm == null) return NotFound(new
                {
                    Sucess = false,
                    Message = "Cliente não foi encontrado"
                });

                var cpn = _promotionalCouponService.GetIfCtmHas(ctm, code);

                if (cpn == null) return NotFound(new {
                    Sucess = false,
                    Message = "O cupom promocional não foi encontrado ou não existe."
                });

                var cpnDTO = new PaymentPromoCouponDTO()
                {
                    Id = cpn.PcpId,
                    Code = cpn.PcpCode,
                    Value = cpn.Pcp.CpnValue
                };

                var jsonString = JsonSerializer.Serialize(cpnDTO, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    ReferenceHandler = ReferenceHandler.IgnoreCycles
                });

                return Ok(new
                {
                    Sucess = true,
                    JsonString = jsonString
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
                _promoCouponAssignmentService.AddPromoCouponToAllCtms(cpn);

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
