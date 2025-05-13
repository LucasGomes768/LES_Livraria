using E_CommerceLivraria.DTO.CartDTO;
using E_CommerceLivraria.Models;
using E_CommerceLivraria.Models.ModelsStructGroups.CartSG;
using E_CommerceLivraria.Models.ModelsStructGroups.StockSG;
using E_CommerceLivraria.Services.CustomerS;
using E_CommerceLivraria.Services.StockS;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceLivraria.Controllers
{
    public class CartController : Controller
    {
        private ICartService _cartService;
        private ICustomerService _customerService;
        private IStockService _stockService;

        public CartController(ICartService cartService,
            ICustomerService customerService,
            IStockService stockService) {
            _cartService = cartService;
            _customerService = customerService;
            _stockService = stockService;
        }

        public IActionResult cartPage() {
            var ctm = _customerService.Get(1);
            if (ctm == null) return NotFound("O cliente não foi encontrado");

            var crt = _cartService.Get(ctm.CtmCrt.CrtId);
            if (crt == null) return NotFound("O carrinho do cliente não foi encontrado");

            ctm.CtmCrt = crt;

            CartDataGroup cdg = new CartDataGroup() {
                CtmId = ctm.CtmId,
                Items = ctm.CtmCrt.CartItems.ToList()
            };

            return View("~/Views/Customer/Cart/Cart.cshtml", cdg);
        }

        public IActionResult addItemToCart(ProductDataGroup pdg) {
            var customer = _customerService.Get(pdg.CtmId);
            if (customer == null) return NotFound("O cliente não foi encontrado");

            var stock = _stockService.Get(pdg.StockId);
            if (stock == null) return NotFound("O item não foi encontrado");

            CartItem cri = new CartItem() {
                CriCrt = customer.CtmCrt,
                CriStc = stock,
                CriQuantity = pdg.Quantity,
                CriTotalprice = stock.StcSalePrice * pdg.Quantity,
                CriLastTimeAltered = DateTime.Now
            };

            stock.StcAvailableAmount -= pdg.Quantity;
            stock.StcBlockedAmount += pdg.Quantity;

            customer.CtmCrt.CartItems.Add(cri);
            _customerService.Update(customer);

            ProductDataGroup pdgNew = new ProductDataGroup {
                Stock = stock,
                InCart = true,
                StockId = stock.StcId,
                CtmId = customer.CtmId,
                Quantity = pdg.Quantity
            };

            return View("~/Views/Customer/Home/Product/Product.cshtml", pdgNew);
        }

        public IActionResult removeItemFromCart(CartDataGroup cdg) {
            var customer = _customerService.Get(cdg.CtmId);
            if (customer == null) return NotFound("O cliente não foi encontrado");

            if (cdg.removedStockId == null) {
                return BadRequest("ID inválido");
            }

            var stock = _stockService.Get((decimal)cdg.removedStockId);
            if (stock == null) return NotFound("O item não foi encontrado");

            var cri = customer.CtmCrt.CartItems.FirstOrDefault(x => x.CriStcId == stock.StcId);
            if (cri == null) return NotFound("Esse item não foi encontrado no carrinho");

            stock.StcAvailableAmount += cri.CriQuantity;
            stock.StcBlockedAmount -= cri.CriQuantity;

            customer.CtmCrt.CartItems.Remove(cri);
            _customerService.Update(customer);

            return RedirectToAction("cartPage", "Cart");
        }

        [HttpPost("Cart/UpdateQuantity")]
        public IActionResult updateItemFromCart([FromBody] UpdateItemDTO updateData)
        {
            try
            {
                var customer = _customerService.Get(updateData.CtmId);
                if (customer == null) return NotFound(updateData.CtmId);

                var stock = _stockService.Get(updateData.ItemStockID);
                if (stock == null) return NotFound(updateData.ItemStockID);

                _cartService.UpdateItemAmount(customer.CtmCrt, stock, updateData.NewAmount);

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
