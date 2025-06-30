using E_CommerceLivraria.Services.CreditCardS;
using E_CommerceLivraria.Services.CustomerS;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceLivraria.Controllers.CrudCTR
{
    public class CreditCardCRUDController : Controller
    {
        private readonly ICreditCardService _creditCardService;
        private readonly ICustomerService _customerService;
        
        public CreditCardCRUDController(ICreditCardService creditCardService, ICustomerService customerService)
        {
            _creditCardService = creditCardService;
            _customerService = customerService;
        }

        [HttpDelete("CreditCardsProfile/Remove/{ctmId:int}/{crdId:int}")]
        public IActionResult RemoveCreditCardFromAccount([FromRoute] int ctmId, [FromRoute] int crdId)
        {
            try
            {
                var ctm = _customerService.Get(ctmId);
                if (ctm == null) return NotFound();

                var crd = _creditCardService.Get(crdId);
                if (crd == null) return NotFound();

                _customerService.RemoveCreditCard(ctm, crd);

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
