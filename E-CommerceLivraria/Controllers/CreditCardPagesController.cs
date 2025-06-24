using E_CommerceLivraria.DTO.CreditCardDTO;
using E_CommerceLivraria.Enums.Customer;
using E_CommerceLivraria.Models.ModelsStructGroups.PaymentSG;
using E_CommerceLivraria.Services.CreditCardS;
using E_CommerceLivraria.Services.CustomerS;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceLivraria.Controllers
{
    public class CreditCardPagesController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly ICreditCardFlagService _creditCardFlagService;

        public CreditCardPagesController(ICustomerService customerService, ICreditCardFlagService creditCardFlagService)
        {
            _customerService = customerService;
            _creditCardFlagService = creditCardFlagService;
        }

        [HttpGet("CreditCard/Create/{origin:int}/{ctmId:decimal}")]
        public IActionResult CreateCreditCardPage([FromRoute] int origin, [FromRoute] decimal ctmId)
        {
            bool exists = _customerService.Exists(ctmId);
            if (!exists) return NotFound();

            CreateCreditCardDTO ccc = new CreateCreditCardDTO()
            {
                CtmId = ctmId,
                RedirectTo = origin,
                AddToAccount = !(origin == (int)ECtmCreditCardCreate.PAYMENT)
            };

            ViewBag.Flags = _creditCardFlagService.GetAll();

            return View("~/Views/Customer/CreditCard/createCreditCard.cshtml", ccc);
        }

        [HttpPost]
        public IActionResult CreateCreditCard(CreateCreditCardDTO ccc)
        {
            try
            {
                var crd = ccc.creditCard;
                if (crd == null) return BadRequest();

                var ctm = _customerService.Get(ccc.CtmId);
                if (ctm == null) return NotFound();

                _customerService.addCreditCard(crd, ctm);

                ECtmCreditCardCreate pageRedirect = (ECtmCreditCardCreate)ccc.RedirectTo;

                switch (pageRedirect)
                {
                    case ECtmCreditCardCreate.PROFILE:
                        return RedirectToAction("CreditCardsList", "ProfileCreditCards", new {CtmId = ctm.CtmId});

                    default: return BadRequest();
                }
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
