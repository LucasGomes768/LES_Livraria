using System.Globalization;
using E_CommerceLivraria.Models;
using E_CommerceLivraria.Repository.AddressR.RegionsR;
using E_CommerceLivraria.Repository.CustomerR;
using E_CommerceLivraria.Services.StockS;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceLivraria.Services.CustomerS {
    public class CartService : ICartService {
        private readonly ICartRepository _cartRepository;
        private readonly IStockService _stockService;

        public CartService(ICartRepository cartRepository, IStockService stockService) {
            _cartRepository = cartRepository;
            _stockService = stockService;
        }

        public Cart Create(Customer ctm) {
            Cart cart = new Cart();

            cart.CrtCtm = ctm;
            cart.CrtCtmId = ctm.CtmId;

            return _cartRepository.Add(cart);
        }

        public Cart? Get(decimal id) {
            return _cartRepository.Get(id);
        }

        public bool Remove(Cart cart) {
            return _cartRepository.Remove(cart);
        }

        public Cart Update(Cart cart) {
            return _cartRepository.Update(cart);
        }

        public Cart UpdateItemAmount(Cart cart, Stock itemStock, decimal newAmount)
        {
            var cartItemsTemp = cart.CartItems.ToList();
            int index = cartItemsTemp.FindIndex(x => (x.CriCrtId == cart.CrtId) && (x.CriStcId == itemStock.StcId));

            if (index != -1)
            {
                decimal amountDiff = newAmount - cartItemsTemp[index].CriQuantity;
                _stockService.BlockItems(itemStock, amountDiff);

                CartItem itemTemp = new CartItem()
                {
                    CriCrtId = cart.CrtId,
                    CriStcId = itemStock.StcId,
                    CriQuantity = newAmount,
                    CriTotalprice = itemStock.StcSalePrice * newAmount,
                    CriLastTimeAltered = DateTime.Now
                };

                cartItemsTemp[index] = itemTemp;
                cart.CartItems = cartItemsTemp;
                
                return _cartRepository.Update(cart);
            }

            return cart;
        }

        public Cart AddItem(Cart cart, Stock stock, decimal quantity)
        {
            _stockService.BlockItems(stock, quantity);

            CartItem newItem = new CartItem()
            {
                CriCrt = cart,
                CriCrtId = cart.CrtId,
                CriStc = stock,
                CriStcId = stock.StcId,
                CriQuantity = quantity,
                CriTotalprice = stock.StcSalePrice * quantity,
                CriLastTimeAltered = DateTime.Now
            };

            cart.CartItems.Add(newItem);
            return _cartRepository.Update(cart);
        }

        public Cart RemoveItem(Cart cart, Stock stock)
        {
            var itemTemp = cart.CartItems.FirstOrDefault(x => x.CriStcId == stock.StcId);
            if (itemTemp == null) throw new Exception("O item não foi encontrado no carrinho");

            _stockService.BlockItems(stock, itemTemp.CriQuantity * -1);
            cart.CartItems.Remove(itemTemp);

            return _cartRepository.Update(cart);
        }
    }
}
