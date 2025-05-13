using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Services.CustomerS {
    public interface ICartService {
        public Cart Create(Customer ctm);
        public bool Remove(Cart cart);
        public Cart? Get(decimal id);
        public Cart Update(Cart cart);
        public Cart UpdateItemAmount(Cart cart, Stock itemStock, decimal newAmount);
    }
}
