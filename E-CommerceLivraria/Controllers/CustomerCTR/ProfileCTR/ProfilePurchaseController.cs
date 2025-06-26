using E_CommerceLivraria.Enums;
using E_CommerceLivraria.Services.CustomerS;
using E_CommerceLivraria.Services.PurchaseS;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceLivraria.Controllers.CustomerCTR.ProfileCTR
{
    public class ProfilePurchaseController : Controller
    {
        private ICustomerService _customerService;
        private IPurchaseService _purchaseService;

        public ProfilePurchaseController(ICustomerService customerService, IPurchaseService purchaseService)
        {
            _customerService = customerService;
            _purchaseService = purchaseService;
        }

        [HttpGet("Profile/Purchases/{CtmId:decimal}")]
        public IActionResult PurchasesList([FromRoute] decimal CtmId)
        {
            try
            {
                var customer = _customerService.Get(CtmId);
                if (customer == null) throw new Exception("Cliente não encontrado");

                customer.Purchases = customer.Purchases.Where(x => x.PrcStatus < (int)EStatus.TROCA_SOLICITADA && x.PrcStatus != (int)EStatus.TROCA_REPROVADA).OrderByDescending(x => x.PrcDate).ToList();

                return View("~/Views/Customer/Profile/Purchases/PurchasesList.cshtml", customer);
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

        [HttpGet("Profile/Purchases/{CtmId:decimal}/{PrcId:decimal}")]
        public IActionResult DetailedPurchaseList([FromRoute] decimal CtmId, [FromRoute] decimal PrcId)
        {
            try
            {
                var purchase = _purchaseService.Get(PrcId);
                if (purchase == null) throw new Exception("Compra não foi encontrada");

                if (purchase.PrcCtmId != CtmId) throw new Exception("Tentativa de acesso de compra de outro usuário");

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
