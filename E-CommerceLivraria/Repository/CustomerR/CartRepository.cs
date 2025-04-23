using E_CommerceLivraria.Data;
using E_CommerceLivraria.Models;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceLivraria.Repository.CustomerR {
    public class CartRepository : ICartRepository {
        private readonly ECommerceDbContext _dbContext;

        public CartRepository(ECommerceDbContext dbContext) {
            _dbContext = dbContext;
        }

        public Cart Add(Cart cart) {
            _dbContext.Carts.Add(cart);
            _dbContext.SaveChanges();

            _dbContext.Entry(cart).State = EntityState.Detached;

            return cart;
        }

        public Cart? Get(decimal id) {
            return _dbContext.Carts
                .Include(x => x.CrtCtm)
                .Include(x => x.CartItems)
                    .ThenInclude(x => x.CriStc)
                        .ThenInclude(x => x.StcSpp)
                .Include(x => x.CartItems)
                    .ThenInclude(x => x.CriStc)
                        .ThenInclude(x => x.Book)
                            .ThenInclude(x => x.BokBat)
                .Include(x => x.CartItems)
                    .ThenInclude(x => x.CriStc)
                        .ThenInclude(x => x.Book)
                            .ThenInclude(x => x.BokPrg)
                .Include(x => x.CartItems)
                    .ThenInclude(x => x.CriStc)
                        .ThenInclude(x => x.Book)
                            .ThenInclude(x => x.BokPbl)
                .Include(x => x.CartItems)
                    .ThenInclude(x => x.CriStc)
                        .ThenInclude(x => x.Book)
                            .ThenInclude(x => x.BcrBcts)
                .FirstOrDefault(x => x.CrtId == id);
        }

        public bool Remove(Cart cart) {
            var crt = Get(cart.CrtId);

            if (crt == null) throw new Exception("Um carrinho com esse ID não foi encontrado");

            _dbContext.Carts.Remove(cart);
            _dbContext.SaveChanges();

            return true;
        }

        public Cart Update(Cart cart) {
            var crt = Get(cart.CrtId);

            if (crt == null) throw new Exception("Um carrinho com esse ID não foi encontrado");

            _dbContext.Carts.Update(cart);
            _dbContext.SaveChanges();

            return cart;
        }
    }
}
