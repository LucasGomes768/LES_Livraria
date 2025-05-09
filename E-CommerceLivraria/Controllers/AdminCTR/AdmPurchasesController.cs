﻿using E_CommerceLivraria.DTO;
using E_CommerceLivraria.Enums;
using E_CommerceLivraria.Models.ModelsStructGroups.PurchasesSG;
using E_CommerceLivraria.Services.PurchaseS;
using Microsoft.AspNetCore.Mvc;

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
            var purchases = _purchaseService.GetAll();

            var apl = new AdmPurchaseListData()
            {
                Purchases = purchases.OrderByDescending(x => x.PrcDate).ToList()
            };
            return View("~/Views/Admin/ctmRequests/purchases/Purchases.cshtml", apl);
        }

        [HttpGet]
        public IActionResult PurchasesListFilter(AdmPurchaseListData apl)
        {
            var purchases = _purchaseService.GetAll();

            if (apl.StatusId != null)
            {
                purchases = purchases.Where(x => x.PurchaseItems
                    .Any(item => item.PciStatus == apl.StatusId.Value))
                    .ToList();
            }

            var aplNew = new AdmPurchaseListData()
            {
                Purchases = purchases,
                StatusId = apl.StatusId
            };

            return View("~/Views/Admin/ctmRequests/purchases/Purchases.cshtml", aplNew);
        }

        [HttpGet("AdmPurchases/DetailedPurchase/{id:decimal}")]
        public IActionResult DetailedPurchase([FromRoute] decimal id)
        {
            try
            {
                var purchase = _purchaseService.Get(id);
                if (purchase == null) throw new Exception("Compra não foi encontrada");

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
                if (purchase == null) throw new Exception("Compra não foi encontrada");

                purchase = _purchaseService.UpdateStatus(purchase, (EStatus)updateStatusDTO.Status);

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

                var purchaseItem = _purchaseItemService.Get((decimal)updateStatusDTO.StcId, updateStatusDTO.PrcId);
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
