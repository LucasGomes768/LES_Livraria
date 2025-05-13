using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using E_CommerceLivraria.Models;
using E_CommerceLivraria.Services.StockS;
using E_CommerceLivraria.Services.StockS.BookS.CategoryS;
using E_CommerceLivraria.Models.ModelsStructGroups.StockSG;
using E_CommerceLivraria.Services.CustomerS;

namespace E_CommerceLivraria.Controllers;

public class HomeController : Controller
{
    private IStockService _stockService;
    private ICategoryService _categoryService;
    private ICustomerService _customerService;

    public HomeController(IStockService stockService,
        ICategoryService categoryService,
        ICustomerService customerService) {
        _stockService = stockService;
        _categoryService = categoryService;
        _customerService = customerService;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet("Home/HomePage")]
    public IActionResult homePage() {
        return searchbar("");
    }

    public IActionResult searchbar(string title) {
        StoreDataGroup sdg = new StoreDataGroup {
            Stocks = _stockService.GetAll()
            .Where(s => s.StcBok.BokTitle.Contains(title ?? "", StringComparison.OrdinalIgnoreCase))
            .ToList(),
            Categories = _categoryService.GetAll()
        };

        return View("~/Views/Customer/Home/Store/Store.cshtml", sdg);
    }

    public IActionResult filteredStore(StoreDataGroup model) {
        IEnumerable<Stock> query = _stockService.GetAll();

        if(!string.IsNullOrWhiteSpace(model.Title)) {
            query = query.Where(s => s.StcBok.BokTitle.Contains(model.Title, StringComparison.InvariantCultureIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(model.Author)) {
            query = query.Where(s => s.StcBok.BokBat.BatName.Contains(model.Author, StringComparison.InvariantCultureIgnoreCase));
        }

        if (!string.IsNullOrWhiteSpace(model.Publisher)) {
            query = query.Where(s => s.StcBok.BokPbl.PblName.Contains(model.Publisher, StringComparison.InvariantCultureIgnoreCase));
        }

        if (model.CategoryId.HasValue) {
            var cat = _categoryService.Get((decimal)model.CategoryId);

            if (cat != null) query = query.Where(s => s.StcBok.BcrBcts.Contains(cat));
        }

        if (model.PriceMin.HasValue) {
            query = query.Where(s => s.StcSalePrice >= model.PriceMin);
        }

        if (model.PriceMax.HasValue) {
            query = query.Where(s => s.StcSalePrice <= model.PriceMax);
        }

        model.Stocks = query.ToList();
        model.Categories = _categoryService.GetAll();

        return View("~/Views/Customer/Home/Store/Store.cshtml", model);
    }

    [Route("Store/Product/{id:decimal}")]
    public IActionResult productPage(decimal id) {
        var stock = _stockService.Get(id);
        if (stock == null) return NotFound("O item não foi encontrado");

        var ctm = _customerService.Get(1);
        if (ctm == null) return NotFound("O cliente não foi encontrado");

        ProductDataGroup pdg = new ProductDataGroup {
            Stock = stock,
            InCart = (ctm.CtmCrt.CartItems.FirstOrDefault(x => x.CriStcId == stock.StcId) != null),
            StockId = stock.StcId,
            CtmId = 1,
            Quantity = 0
        };

        return View("~/Views/Customer/Home/Product/Product.cshtml", pdg);
    }
}
