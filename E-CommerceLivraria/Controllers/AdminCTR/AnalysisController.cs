using E_CommerceLivraria.DTO.AnalysisDTO;
using E_CommerceLivraria.Enums.Admin;
using E_CommerceLivraria.Services.PurchaseS;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace E_CommerceLivraria.Controllers.AdminCTR
{
    public class AnalysisController : Controller
    {
        private readonly IPurchaseService _purchaseService;

        public AnalysisController(IPurchaseService purchaseService)
        {
            _purchaseService = purchaseService;
        }

        public IActionResult AnalysisPage()
        {
            return View("~/Views/Admin/analysis/Analysis.cshtml");
        }

        [HttpGet("Sales/{data}/{start}/{end}")]
        public IActionResult GetCategoriesData([FromRoute] int data, [FromRoute] DateTime start, [FromRoute] DateTime end)
        {
            try
            {
                List<DataSalesDTO> sales;

                if (data == (int)EDataAnalysis.Categorias)
                {
                    sales = _purchaseService.GetSalesByCategories(start, end);
                } else
                {
                    sales = _purchaseService.GetSalesByProduct(start, end);
                }

                var jsonString = JsonSerializer.Serialize(sales, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });

                return Ok(new
                {
                    Sucess = true,
                    jsonString
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
