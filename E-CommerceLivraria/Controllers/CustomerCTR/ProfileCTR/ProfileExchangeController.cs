using AspNetCoreGeneratedDocument;
using E_CommerceLivraria.DTO.ExchangesDTO;
using E_CommerceLivraria.Enums;
using E_CommerceLivraria.Models;
using E_CommerceLivraria.Services.CustomerS;
using E_CommerceLivraria.Services.LoginS;
using E_CommerceLivraria.Services.PurchaseS;
using E_CommerceLivraria.Specifications;
using E_CommerceLivraria.Specifications.CustomerSpecs.Purchases;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceLivraria.Controllers.CustomerCTR.ProfileCTR
{
    public class ProfileExchangeController : Controller
    {
        private ICustomerService _customerService;
        private IPurchaseService _purchaseService;
        private LoginSingleton _loginSingleton;

        public ProfileExchangeController(ICustomerService customerService, IPurchaseService purchaseService, LoginSingleton loginSingleton)
        {
            _customerService = customerService;
            _purchaseService = purchaseService;
            _loginSingleton = loginSingleton;
        }

        [HttpGet("ExchangeProfile")]
        public IActionResult ExchangesList()
        {
            try
            {
                if (_loginSingleton.CtmId == null || _loginSingleton.CtmId == 0) return RedirectToAction("LoginPage", "Login");

                int id = (int)_loginSingleton.CtmId;
                ISpecification<Customer> spec = new GetCtmsPurchases(id);

                var ctm = _customerService.Get(spec);
                if (ctm == null) return NotFound("Cliente não foi encontrado ou não existe");

                return View("~/Views/Customer/Profile/Exchanges/ExchangesList.cshtml", ctm);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("ExchangeProfile/{PrcId:decimal}")]
        public IActionResult DetailedExchange([FromRoute] decimal PrcId)
        {
            try
            {
                if (_loginSingleton.CtmId == null || _loginSingleton.CtmId == 0) return RedirectToAction("LoginPage", "Login");

                int ctmId = (int)_loginSingleton.CtmId;

                var purchase = _purchaseService.Get(PrcId);
                if (purchase == null) throw new Exception("Compra não foi encontrada");

                if (purchase.PrcCtmId != ctmId) throw new Exception("Tentativa de acesso de compra de outro usuário");

                purchase.PurchaseItems = purchase.PurchaseItems.Where(x => (x.PciStatus >= (int)EStatus.TROCA_SOLICITADA) || (x.PciStatus == (int)EStatus.TROCA_REPROVADA)).ToList();

                return View("~/Views/Customer/Profile/Exchanges/Detailed/DetailedExchange.cshtml", purchase);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("RequestExchange/{PrcId:decimal}")]
        public IActionResult RequestExchangePage([FromRoute] decimal PrcId)
        {
            try
            {
                if (_loginSingleton.CtmId == null || _loginSingleton.CtmId == 0) return RedirectToAction("LoginPage", "Login");

                int ctmId = (int)_loginSingleton.CtmId;

                var purchase = _purchaseService.Get(PrcId);
                if (purchase == null) throw new Exception("Compra não foi encontrada");

                if (purchase.PurchaseItems.Count < 1 || !purchase.PurchaseItems.Any()) throw new Exception("Compra sem itens");

                EStatus status = (EStatus)purchase.PrcStatus;
                if (status != EStatus.ENTREGUE) throw new Exception("A compra deve ter o status \"ENTREGUE\" para efetuação de troca");

                if (purchase.PrcCtmId != ctmId) throw new Exception("Tentativa de acesso de compra de outro usuário");

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
