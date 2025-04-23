using System.Globalization;
using E_CommerceLivraria.Models;
using E_CommerceLivraria.Repository.AddressR.RegionsR;
using E_CommerceLivraria.Repository.CustomerR;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceLivraria.Services.CustomerS {
    public class CartService : ICartService {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository) {
            _cartRepository = cartRepository;
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
    }
}
