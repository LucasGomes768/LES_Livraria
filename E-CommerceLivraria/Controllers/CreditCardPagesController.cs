using E_CommerceLivraria.DTO.CreditCardDTO;
using E_CommerceLivraria.Enums;
using E_CommerceLivraria.Services.CreditCardS;
using E_CommerceLivraria.Services.CustomerS;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace E_CommerceLivraria.Controllers
{
    public class CreditCardPagesController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly ICreditCardFlagService _creditCardFlagService;
        private readonly ICreditCardService _creditCardService;

        public CreditCardPagesController(ICustomerService customerService, ICreditCardFlagService creditCardFlagService, ICreditCardService creditCardService)
        {
            _customerService = customerService;
            _creditCardFlagService = creditCardFlagService;
            _creditCardService = creditCardService;
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
                AddToAccount = !(origin == (int)ECreditCardCreate.PAYMENT)
            };

            ViewBag.Layout = (origin < (int)ECreditCardCreate.DETAILED_CTM_PAGE) ? "~/Views/Shared/_PublicLayout.cshtml" : "~/Views/Shared/_AdminLayout.cshtml";
            ViewBag.Flags = _creditCardFlagService.GetAll();

            return View("~/Views/Shared/CreditCard/createCreditCard.cshtml", ccc);
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

                if (!ccc.AddToAccount)
                    crd = _creditCardService.Create(crd);
                else
                    crd = _creditCardService.Create(crd, ctm);


                ECreditCardCreate pageRedirect = (ECreditCardCreate)ccc.RedirectTo;

                switch (pageRedirect)
                {
                    case ECreditCardCreate.PAYMENT:
                        TempData["AddedCard"] = JsonSerializer.Serialize(new
                        {
                            id = crd.CrdId.ToString(),
                            number = crd.CrdNumberHidden.Substring(0,19),
                            flag = crd.CrdCcf.CcfName,
                            value = 10
                        }, new JsonSerializerOptions()
                        {
                            WriteIndented = true,
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        });

                        return RedirectToAction("PaymentMethodPage", "Payment", new { CtmId = ctm.CtmId });

                    case ECreditCardCreate.PROFILE:
                        return RedirectToAction("CreditCardsList", "ProfileCreditCards", new {CtmId = ctm.CtmId});

                    case ECreditCardCreate.DETAILED_CTM_PAGE:
                        return RedirectToAction("DetailedCustomerPage", "AdmCustomer", new { id = ctm.CtmId });

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
