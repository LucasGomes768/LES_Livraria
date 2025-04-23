using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Repository.CustomerR {
    public interface ICartRepository {
        public Cart Add(Cart cart);
        public bool Remove(Cart cart);
        public Cart? Get(decimal id);
        public Cart Update(Cart cart);
    }
}
