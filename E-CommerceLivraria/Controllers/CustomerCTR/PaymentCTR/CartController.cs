using E_CommerceLivraria.DTO.CartDTO;
using E_CommerceLivraria.DTO.StoreDTO;
using E_CommerceLivraria.Models;
using E_CommerceLivraria.Services.CustomerS;
using E_CommerceLivraria.Services.LoginS;
using E_CommerceLivraria.Services.StockS;
using E_CommerceLivraria.Specifications;
using E_CommerceLivraria.Specifications.CustomerSpecs.Cart;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceLivraria.Controllers.CustomerCTR.PaymentCTR
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly ICustomerService _customerService;
        private readonly IStockService _stockService;
        private readonly LoginSingleton _loginSingleton;

        public CartController(ICartService cartService, ICustomerService customerService, IStockService stockService, LoginSingleton loginSingleton) {
            _cartService = cartService;
            _customerService = customerService;
            _stockService = stockService;
            _loginSingleton = loginSingleton;
        }

        public IActionResult CartPage() {
            if (_loginSingleton.CtmId == null || _loginSingleton.CtmId == 0) return RedirectToAction("LoginPage", "Login");

            decimal id = (decimal)_loginSingleton.CtmId;
            ISpecification<Customer> spec = new GetCtmsCart(id);

            var ctm = _customerService.Get(spec);
            if (ctm == null) return NotFound("O cliente não foi encontrado ou não existe");

            CartItemsDTO cdg = new CartItemsDTO() {
                CtmId = id,
                Items = ctm.CtmCrt.CartItems.ToList()
            };

            return View("~/Views/Customer/Cart/Cart.cshtml", cdg);
        }

        public IActionResult AddItemToCart(ProductPageDTO pdg)
        {
            if (_loginSingleton.CtmId == null || _loginSingleton.CtmId == 0) return RedirectToAction("LoginPage", "Login");

            decimal id = (decimal)_loginSingleton.CtmId;
            ISpecification<Customer> spec = new GetCtmsCart(id);

            var ctm = _customerService.Get(spec);
            if (ctm == null) return NotFound("O cliente não foi encontrado ou não existe");

            var stock = _stockService.Get(pdg.StockId);
            if (ctm == null) return NotFound("O cliente não foi encontrado ou não existe");

            _cartService.AddItem(ctm.CtmCrt, stock, pdg.Quantity);

            ProductPageDTO pdgNew = new ProductPageDTO {
                Stock = stock,
                InCart = true,
                StockId = stock.StcId,
                Quantity = pdg.Quantity
            };

            return View("~/Views/Customer/Home/Product/Product.cshtml", pdgNew);
        }

        public IActionResult RemoveItemFromCart(CartItemsDTO cdg) {
            try
            {
                if (_loginSingleton.CtmId == null || _loginSingleton.CtmId == 0) return RedirectToAction("LoginPage", "Login");

                decimal id = (decimal)_loginSingleton.CtmId;
                ISpecification<Customer> spec = new GetCtmsCart(id);

                var ctm = _customerService.Get(spec);
                if (ctm == null) return NotFound("O cliente não foi encontrado ou não existe");

                if (cdg.removedStockId == null) return BadRequest("ID inválido");

                var stock = _stockService.Get((decimal)cdg.removedStockId);
                if (stock == null) return NotFound("O item não foi encontrado");

                _cartService.RemoveItem(ctm.CtmCrt, stock);

                return RedirectToAction("cartPage", "Cart");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("Cart/UpdateQuantity")]
        public IActionResult updateItemFromCart([FromBody] UpdateItemDTO updateData)
        {
            try
            {
                if (_loginSingleton.CtmId == null || _loginSingleton.CtmId == 0) return RedirectToAction("LoginPage", "Login");

                decimal id = (decimal)_loginSingleton.CtmId;
                ISpecification<Customer> spec = new GetCtmsCart(id);

                var ctm = _customerService.Get(spec);
                if (ctm == null) return NotFound("O cliente não foi encontrado ou não existe");

                var stock = _stockService.Get(updateData.ItemStockID);
                if (stock == null) return NotFound("O estoque do item não foi encontrado");

                _cartService.UpdateItemAmount(ctm.CtmCrt, stock, updateData.NewAmount);

                return Ok(new {
                    Sucess = true
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
