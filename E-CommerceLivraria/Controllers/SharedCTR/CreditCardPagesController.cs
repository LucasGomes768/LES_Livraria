using E_CommerceLivraria.DTO.CreditCardDTO;
using E_CommerceLivraria.Enums;
using E_CommerceLivraria.Models;
using E_CommerceLivraria.Services.CreditCardS;
using E_CommerceLivraria.Services.CustomerS;
using E_CommerceLivraria.Services.LoginS;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace E_CommerceLivraria.Controllers.SharedCTR
{
    public class CreditCardPagesController : Controller
    {
        private readonly ICustomerService _customerService;
        private readonly ICreditCardFlagService _creditCardFlagService;
        private readonly ICreditCardService _creditCardService;
        private readonly LoginSingleton _loginSingleton;

        public CreditCardPagesController(ICustomerService customerService, ICreditCardFlagService creditCardFlagService, ICreditCardService creditCardService, LoginSingleton loginSingleton)
        {
            _customerService = customerService;
            _creditCardFlagService = creditCardFlagService;
            _creditCardService = creditCardService;
            _loginSingleton = loginSingleton;
        }

        [HttpGet("CreditCard/Create/{origin:int}/{ctmId:int?}")]
        public IActionResult CreateCreditCardPage([FromRoute] int origin, [FromRoute] int? ctmId)
        {
            if (ctmId == null || origin != (int)ECreditCardCreate.DETAILED_CTM_PAGE)
            {
                if (_loginSingleton.CtmId == null) return RedirectToAction("LoginPage", "Login");
                else ctmId = (int)_loginSingleton.CtmId;
            } else
            {
                bool exists = _customerService.Exists((int)ctmId);
                if (!exists) return NotFound();
            }

            CreateCreditCardDTO ccc = new CreateCreditCardDTO()
            {
                CtmId = (int)ctmId,
                RedirectTo = origin,
                AddToAccount = !(origin == (int)ECreditCardCreate.PAYMENT)
            };

            ViewBag.Layout = origin < (int)ECreditCardCreate.DETAILED_CTM_PAGE ? "~/Views/Shared/_PublicLayout.cshtml" : "~/Views/Shared/_AdminLayout.cshtml";
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

                Customer? ctm;

                if (_loginSingleton?.CtmId == null)
                {
                    ctm = _customerService.Get(ccc.CtmId);
                    if (ctm == null) return NotFound();
                }
                else
                {
                    ctm = _customerService.Get((decimal)_loginSingleton.CtmId);
                    if (ctm == null) return NotFound();
                }


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
                            number = crd.CrdNumberHidden[..19],
                            flag = crd.CrdCcf.CcfName,
                            value = 10
                        }, new JsonSerializerOptions()
                        {
                            WriteIndented = true,
                            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                        });

                        return RedirectToAction("PaymentMethodPage", "Payment", new { ctm.CtmId });

                    case ECreditCardCreate.PROFILE:
                        return RedirectToAction("CreditCardsList", "ProfileCreditCards", new {ctm.CtmId});

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
