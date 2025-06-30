using E_CommerceLivraria.Services.CreditCardS;
using E_CommerceLivraria.Services.CustomerS;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceLivraria.Controllers.CustomerCTR.ProfileCTR
{
    public class ProfileCreditCardsController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly ICreditCardService _creditCardService;

        public ProfileCreditCardsController(ICustomerService customerService, ICreditCardService creditCardService)
        {
            _customerService = customerService;
            _creditCardService = creditCardService;
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
    }
}
