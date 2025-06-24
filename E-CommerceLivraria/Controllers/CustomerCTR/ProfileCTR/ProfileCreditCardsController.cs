using E_CommerceLivraria.Services.CustomerS;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceLivraria.Controllers.CustomerCTR.ProfileCTR
{
    public class ProfileCreditCardsController : Controller
    {
        private readonly ICustomerService _customerService;

        public ProfileCreditCardsController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("CreditCardsProfile/{CtmId:decimal}")]
        public IActionResult CreditCardsList([FromRoute] decimal CtmId)
        {
            try
            {
                var customer = _customerService.Get(CtmId);
                if (customer == null) throw new Exception("Cliente não foi encontrado");

                return View("~/Views/Customer/Profile/CreditCards/CreditCardsList.cshtml", customer);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("CreditCardsProfile/Remove/{ctmId:int}/{crdId:int}")]
        public IActionResult RemoveCreditCardFromAccount([FromRoute] int ctmId, [FromRoute] int crdId)
        {
            try
            {
                return Ok(new
                {
                    Sucess = true
                });

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
