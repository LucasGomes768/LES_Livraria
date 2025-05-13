using AspNetCoreGeneratedDocument;
using E_CommerceLivraria.DTO.ExchangesDTO;
using E_CommerceLivraria.Enums;
using E_CommerceLivraria.Services.CustomerS;
using E_CommerceLivraria.Services.PurchaseS;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceLivraria.Controllers.CustomerCTR.ProfileCTR
{
    public class ProfileExchangeController : Controller
    {
        private ICustomerService _customerService;
        private IPurchaseService _purchaseService;
        private IPurchaseItemService _purchaseItemService;

        public ProfileExchangeController(ICustomerService customerService, IPurchaseService purchaseService, IPurchaseItemService purchaseItemService)
        {
            _customerService = customerService;
            _purchaseService = purchaseService;
            _purchaseItemService = purchaseItemService;
        }

        [HttpGet("RequestExchange/{CtmId:decimal}/{PrcId:decimal}")]
        public IActionResult RequestExchangePage([FromRoute] decimal CtmId, [FromRoute] decimal PrcId)
        {
            try
            {
                var purchase = _purchaseService.Get(PrcId);
                if (purchase == null) throw new Exception("Compra não foi encontrada");

                if (purchase.PurchaseItems.Count < 1 || !purchase.PurchaseItems.Any()) throw new Exception("Compra sem itens");

                EStatus status = (EStatus)purchase.PrcStatus;
                if (status != EStatus.ENTREGUE) throw new Exception("A compra deve ter o status \"ENTREGUE\" para efetuação de troca");

                if (purchase.PrcCtmId != CtmId) throw new Exception("Tentativa de acesso de compra de outro usuário");

                return View("~/Views/Customer/Profile/Exchanges/RequestExchange/RequestExchange.cshtml", purchase);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("RequestExchange/Send")]
        public IActionResult RequestExchange([FromBody] ExchangeRequestDTO exchangeData)
        {

            try
            {
                var purchase = _purchaseService.Get(exchangeData.PrcId);
                if (purchase == null) throw new Exception("Compra não foi encontrada");

                if (purchase.PurchaseItems.Count < 1 || !purchase.PurchaseItems.Any()) throw new Exception("Compra sem itens");

                if (purchase.PrcCtmId != exchangeData.CtmId) throw new Exception("Tentativa de acesso de compra de outro usuário");

                _purchaseService.AddExchange(exchangeData);

                return Ok(new
                {
                    Sucess = true,
                    Message = "Troca solicitada com processo"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Sucess = false,
                    error = $"Erro ao processar troca: {ex.Message}"
                });
            }
        }
    }
}
