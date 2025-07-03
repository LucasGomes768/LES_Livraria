using E_CommerceLivraria.Enums;
using E_CommerceLivraria.Models;
using E_CommerceLivraria.Services.CustomerS;
using E_CommerceLivraria.Services.LoginS;
using E_CommerceLivraria.Services.PurchaseS;
using E_CommerceLivraria.Specifications;
using E_CommerceLivraria.Specifications.CustomerSpecs;
using E_CommerceLivraria.Specifications.CustomerSpecs.Purchases;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceLivraria.Controllers.CustomerCTR.ProfileCTR
{
    public class ProfilePurchaseController : Controller
    {
        private ICustomerService _customerService;
        private IPurchaseService _purchaseService;
        private LoginSingleton _loginSingleton;

        public ProfilePurchaseController(ICustomerService customerService, IPurchaseService purchaseService, LoginSingleton loginSingleton)
        {
            _customerService = customerService;
            _purchaseService = purchaseService;
            _loginSingleton = loginSingleton;
        }

        [HttpGet("Profile/Purchases")]
        public IActionResult PurchasesList()
        {
            try
            {
                if (_loginSingleton.CtmId == null || _loginSingleton.CtmId == 0) return RedirectToAction("LoginPage", "Login");

                int id = (int)_loginSingleton.CtmId;
                ISpecification<Customer> spec = new GetCtmsPurchases(id);

                var ctm = _customerService.Get(spec);
                if (ctm == null) return NotFound("Cliente não foi encontrado ou não existe");

                ctm.Purchases = ctm.Purchases.Where(x => x.PrcStatus < (int)EStatus.TROCA_SOLICITADA && x.PrcStatus != (int)EStatus.TROCA_REPROVADA).OrderByDescending(x => x.PrcDate).ToList();

                return View("~/Views/Customer/Profile/Purchases/PurchasesList.cshtml", ctm);
            }
            catch (Exception ex)
            {
                return StatusCode(404, new
                {
                    Sucess = false,
                    Message = $"Erro ao processar compra: {ex.Message}"
                });
            }
        }

        [HttpGet("Profile/Purchases/{PrcId:decimal}")]
        public IActionResult DetailedPurchaseList([FromRoute] decimal PrcId)
        {
            try
            {
                if (_loginSingleton.CtmId == null || _loginSingleton.CtmId == 0) return RedirectToAction("LoginPage", "Login");

                var purchase = _purchaseService.Get(PrcId);
                if (purchase == null) throw new Exception("Compra não foi encontrada");

                if (purchase.PrcCtmId != _loginSingleton.CtmId) throw new Exception("Tentativa de acesso de compra de outro usuário");

                purchase.PurchaseItems = purchase.PurchaseItems
                    .Where(x => (x.PciStatus >= (int)EStatus.COMPRA_REPROVADA) && (x.PciStatus < (int)EStatus.TROCA_SOLICITADA))
                    .ToList();

                return View("~/Views/Customer/Profile/Purchases/Detailed/DetailedPurchase.cshtml", purchase);
            }
            catch (Exception ex)
            {
                return StatusCode(404, new
                {
                    Sucess = false,
                    Message = $"Erro ao processar compra: {ex.Message}"
                });
            }
        }
    }
}
