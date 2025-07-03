using E_CommerceLivraria.DTO.StoreDTO;
using E_CommerceLivraria.Models;
using E_CommerceLivraria.Services.CustomerS;
using E_CommerceLivraria.Services.LoginS;
using E_CommerceLivraria.Services.StockS;
using E_CommerceLivraria.Services.StockS.BookS.CategoryS;
using E_CommerceLivraria.Specifications;
using E_CommerceLivraria.Specifications.CustomerSpecs.Cart;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace E_CommerceLivraria.Controllers.CustomerCTR;

public class HomeController : Controller
{
    private readonly IStockService _stockService;
    private readonly ICategoryService _categoryService;
    private readonly ICustomerService _customerService;
    private readonly LoginSingleton _loginSingleton;

    public HomeController(IStockService stockService, ICategoryService categoryService, ICustomerService customerService, LoginSingleton loginSingleton) {
        _stockService = stockService;
        _categoryService = categoryService;
        _customerService = customerService;
        _loginSingleton = loginSingleton;
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    [HttpGet("Home/HomePage")]
    public IActionResult HomePage() {
        return searchbar("");
    }

    public IActionResult searchbar(string title) {
        StorePageDTO sdg = new StorePageDTO {
            Stocks = _stockService.GetAll()
            .Where(s => s.StcBok.BokTitle.Contains(title ?? "", StringComparison.OrdinalIgnoreCase))
            .ToList(),
            Categories = _categoryService.GetAll()
        };

        return View("~/Views/Customer/Home/Store/Store.cshtml", sdg);
    }

    public IActionResult filteredStore(StorePageDTO model) {
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
    public IActionResult productPage(decimal id)
    {
        if (_loginSingleton.CtmId == null || _loginSingleton.CtmId == 0) return RedirectToAction("LoginPage", "Login");

        decimal ctmId = (decimal)_loginSingleton.CtmId;
        ISpecification<Customer> spec = new GetCtmsCart(ctmId);

        var ctm = _customerService.Get(spec);
        if (ctm == null) return NotFound("O cliente não foi encontrado");

        var stock = _stockService.Get(id);
        if (stock == null) return NotFound("O item não foi encontrado");

        ProductPageDTO pdg = new ProductPageDTO {
            Stock = stock,
            InCart = ctm.CtmCrt.CartItems.FirstOrDefault(x => x.CriStcId == stock.StcId) != null,
            StockId = stock.StcId,
            Quantity = 0
        };

        return View("~/Views/Customer/Home/Product/Product.cshtml", pdg);
    }
}
