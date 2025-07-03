using E_CommerceLivraria.DTO;
using E_CommerceLivraria.DTO.AdmPurchasesDTO;
using E_CommerceLivraria.Enums;
using E_CommerceLivraria.Services.PurchaseS;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_CommerceLivraria.Controllers.AdminCTR
{
    public class AdmPurchasesController : Controller
    {
        private IPurchaseService _purchaseService;
        private IPurchaseItemService _purchaseItemService;

        public AdmPurchasesController(IPurchaseService purchaseService, IPurchaseItemService purchaseItemService)
        {
            _purchaseService = purchaseService;
            _purchaseItemService = purchaseItemService;
        }

        [HttpGet]
        public IActionResult PurchasesList()
        {
            var purchases = _purchaseService.GetAll()
                .Where(x => (
                    x.PrcStatus < (int)EStatus.TROCA_SOLICITADA)
                    && (x.PrcStatus >= (int)EStatus.EM_PROCESSAMENTO)
                );

            var filterOptions = Enum.GetValues(typeof(EStatus))
                                    .Cast<EStatus>()
                                    .SkipWhile(e => e != EStatus.EM_PROCESSAMENTO)
                                    .TakeWhile(e => e != EStatus.TROCA_SOLICITADA)
                                    .Append(EStatus.COMPRA_REPROVADA)
                                    .Select(e => new SelectListItem
                                    {
                                        Value = ((int)e).ToString(),
                                        Text = e.ToString().Replace("_", " ")
                                    })
                                    .OrderBy(x => x.Value)
                                    .ToList();

            var apl = new AdmPurchaseListDTO()
            {
                Purchases = purchases.OrderByDescending(x => x.PrcDate).ToList()
            };

            ViewBag.FilterOptions = filterOptions;
            ViewBag.FilterController = "AdmPurchases";
            ViewBag.FilterAction = "PurchasesListFilter";
            ViewBag.DetailedAction = "DetailedPurchase";

            return View("~/Views/Admin/ctmRequests/purchases/Purchases.cshtml", apl);
        }

        public IActionResult PurchasesListFilter(AdmPurchaseListDTO apl)
        {
            var purchases = _purchaseService.GetAll()
                .Where(x => (
                    x.PrcStatus < (int)EStatus.TROCA_SOLICITADA)
                    && (x.PrcStatus >= (int)EStatus.EM_PROCESSAMENTO)
                );

            if (apl.StatusId != null)
            {
                purchases = purchases.Where(x => x.PurchaseItems
                    .Any(item => item.PciStatus == apl.StatusId.Value));
            }


            var filterOptions = Enum.GetValues(typeof(EStatus))
                                    .Cast<EStatus>()
                                    .SkipWhile(e => e != EStatus.EM_PROCESSAMENTO)
                                    .TakeWhile(e => e != EStatus.TROCA_SOLICITADA)
                                    .Append(EStatus.COMPRA_REPROVADA)
                                    .Select(e => new SelectListItem
                                    {
                                        Value = ((int)e).ToString(),
                                        Text = e.ToString().Replace("_", " ")
                                    })
                                    .OrderBy(x => x.Value)
                                    .ToList();

            var aplNew = new AdmPurchaseListDTO()
            {
                Purchases = purchases.OrderByDescending(x => x.PrcDate).ToList(),
                StatusId = apl.StatusId
            };

            ViewBag.FilterOptions = filterOptions;
            ViewBag.FilterController = "AdmPurchases";
            ViewBag.FilterAction = "PurchasesListFilter";
            ViewBag.DetailedAction = "DetailedPurchase";

            return View("~/Views/Admin/ctmRequests/purchases/Purchases.cshtml", aplNew);
        }

        [HttpGet("Admin/Purchases/{id:decimal}")]
        public IActionResult DetailedPurchase([FromRoute] decimal id)
        {
            try
            {
                var purchase = _purchaseService.Get(id);
                if (purchase == null) throw new Exception("Compra não foi encontrada");

                ViewBag.ExchangeItems = purchase.PurchaseItems.Where(x => !(x.PciStatus <= (int)EStatus.ENTREGUE)).ToList();
                purchase.PurchaseItems = purchase.PurchaseItems.Where(x => x.PciStatus <= (int)EStatus.ENTREGUE).ToList();

                return View("~/Views/Admin/ctmRequests/dtlPurchase/dtlPurchase.cshtml", purchase);
            }
            catch (Exception ex)
            {
                return BadRequest("Erro: " + ex.Message);
            }
        }

        [HttpPut]
        public IActionResult UpdatePurchaseStatus([FromBody] UpdateStatusDTO updateStatusDTO)
        {
            try
            {
                var purchase = _purchaseService.Get(updateStatusDTO.PrcId);
                if (purchase == null) return NotFound("Compra não foi encontrada");

                purchase = _purchaseService.UpdatePurchaseStatus(purchase, (EStatus)updateStatusDTO.Status);

                return Ok(new
                {
                    Sucess = true,
                    Message = "Compra atualizada"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Sucess = false,
                    Message = $"{ex.Message}"
                });
            }
        }

        public IActionResult UpdatePurchaseItemStatus([FromBody] UpdateStatusDTO updateStatusDTO)
        {
            try
            {
                if (updateStatusDTO == null) return BadRequest("Estoque nulo");

                EStatus status = (EStatus)(updateStatusDTO.Status - 1);

                var purchaseItem = _purchaseItemService.Get((decimal)updateStatusDTO.StcId, updateStatusDTO.PrcId, status);
                if (purchaseItem == null) throw new Exception("Item da compra não foi encontrado");

                purchaseItem = _purchaseItemService.UpdateStatus(purchaseItem, (EStatus)updateStatusDTO.Status);

                return Ok(new
                {
                    Sucess = true,
                    Message = "Compra atualizada"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Sucess = false,
                    Message = $"{ex.Message}"
                });
            }
        }
    }
}
