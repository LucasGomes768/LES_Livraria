using E_CommerceLivraria.DTO;
using E_CommerceLivraria.DTO.AdmPurchasesDTO;
using E_CommerceLivraria.Enums;
using E_CommerceLivraria.Services.CouponS;
using E_CommerceLivraria.Services.CustomerS;
using E_CommerceLivraria.Services.PurchaseS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_CommerceLivraria.Controllers.AdminCTR
{
    public class AdmExchangesController : Controller
    {
        private readonly IPurchaseService _purchaseService;
        private readonly IPurchaseItemService _purchaseItemService;
        private readonly IExchangeCouponService _exchangeCouponService;
        private readonly ICustomerService _customerService;

        public AdmExchangesController(IPurchaseService purchaseService, IPurchaseItemService purchaseItemService, ICustomerService customerService, IExchangeCouponService exchangeCouponService)
        {
            _purchaseService = purchaseService;
            _purchaseItemService = purchaseItemService;
            _customerService = customerService;
            _exchangeCouponService = exchangeCouponService;
        }

        [HttpGet]
        public IActionResult ExchangesList()
        {
            var exchanges = _purchaseService.GetAll()
                .Where(x => (
                    x.PrcStatusExchange >= (int)EStatus.TROCA_SOLICITADA)
                    || (x.PrcStatusExchange == (int)EStatus.TROCA_REPROVADA)
                );

            var filterOptions = Enum.GetValues(typeof(EStatus))
                                    .Cast<EStatus>()
                                    .SkipWhile(e => e != EStatus.TROCA_SOLICITADA)
                                    .TakeWhile(e => e != EStatus.COMPRA_REPROVADA)
                                    .Select(e => new SelectListItem
                                    {
                                        Value = ((int)e).ToString(),
                                        Text = e.ToString().Replace("_", " ")
                                    })
                                    .OrderBy(x => x.Value)
                                    .ToList();

            var apl = new AdmPurchaseListDTO()
            {
                Purchases = exchanges.OrderByDescending(x => x.PrcDate).ToList()
            };

            ViewBag.FilterOptions = filterOptions;
            ViewBag.FilterController = "AdmExchanges";
            ViewBag.FilterAction = "ExchangesListFilter";
            ViewBag.DetailedAction = "DetailedExchange";

            return View("~/Views/Admin/ctmRequests/purchases/Purchases.cshtml", apl);
        }

        public IActionResult ExchangesListFilter(AdmPurchaseListDTO apl)
        {
            var exchanges = _purchaseService.GetAll()
                .Where(x => (
                    x.PrcStatusExchange >= (int)EStatus.TROCA_SOLICITADA)
                    || (x.PrcStatusExchange == (int)EStatus.TROCA_REPROVADA)
                );

            if (apl.StatusId != null)
            {
                exchanges = exchanges.Where(x => x.PurchaseItems
                    .Any(item => item.PciStatus == apl.StatusId.Value));
            }

            var filterOptions = Enum.GetValues(typeof(EStatus))
                                    .Cast<EStatus>()
                                    .SkipWhile(e => e != EStatus.TROCA_SOLICITADA)
                                    .TakeWhile(e => e != EStatus.COMPRA_REPROVADA)
                                    .Select(e => new SelectListItem
                                    {
                                        Value = ((int)e).ToString(),
                                        Text = e.ToString().Replace("_", " ")
                                    })
                                    .OrderBy(x => x.Value)
                                    .ToList();

            var aplNew = new AdmPurchaseListDTO()
            {
                Purchases = exchanges.OrderByDescending(x => x.PrcDate).ToList()
            };

            ViewBag.FilterOptions = filterOptions;
            ViewBag.FilterController = "AdmExchanges";
            ViewBag.FilterAction = "ExchangesListFilter";
            ViewBag.DetailedAction = "DetailedExchange";

            return View("~/Views/Admin/ctmRequests/purchases/Purchases.cshtml", aplNew);
        }

        [HttpGet("Admin/Exchanges/{id:decimal}")]
        public IActionResult DetailedExchange([FromRoute] decimal id)
        {
            try
            {
                var purchase = _purchaseService.Get(id);
                if (purchase == null) throw new Exception("Compra não foi encontrada");

                ViewBag.ExchangeItems = purchase.PurchaseItems.Where(x => !(x.PciStatus >= (int)EStatus.COMPRA_REPROVADA && x.PciStatus <= (int)EStatus.ENTREGUE)).ToList();

                return View("~/Views/Admin/ctmRequests/dtlExchange/dtlExchange.cshtml", purchase);
            }
            catch (Exception ex)
            {
                return BadRequest("Erro: " + ex.Message);
            }
        }

        [HttpPut("Admin/Exchanges/UpdateExchangeStatus")]
        public IActionResult ReturnToStock([FromBody] UpdateStatusDTO updateStatusDTO)
        {
            try
            {
                if (updateStatusDTO == null) return BadRequest("Nenhum valor enviado");

                var purchase = _purchaseService.Get(updateStatusDTO.PrcId);
                if (purchase == null) return NotFound("Compra não foi encontrada");

                _purchaseService.UpdateExchangeStatus(purchase, (EStatus)updateStatusDTO.Status, updateStatusDTO.ReturnStock);

                return Ok(new
                {
                    Sucess = true,
                    Message = "Pedido atualizado"
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
