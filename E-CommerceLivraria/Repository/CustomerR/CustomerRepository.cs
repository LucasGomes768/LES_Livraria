using E_CommerceLivraria.Data;
using E_CommerceLivraria.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace E_CommerceLivraria.Repository.CustomerR {
    public class CustomerRepository : ICustomerRepository {
        private readonly ECommerceDbContext _dbContext;

        public CustomerRepository(ECommerceDbContext dbContext) {
            _dbContext = dbContext;
        }

        public Customer Add(Customer customer) {
            _dbContext.Customers.Add(customer);
            _dbContext.SaveChanges();

            return customer;
        }

        public bool Exists(decimal id) {
            var ctm = _dbContext.Customers.AsNoTracking().FirstOrDefault(x => x.CtmId == id);

            if (ctm == null) {
                return false;
            } else {
                return true;
            }
        }

        public Customer? Get(decimal id) {
            return _dbContext.Customers
                .Include(x => x.CtmGnd)
                .Include(x => x.CtcCrds)
                    .ThenInclude(x => x.CrdCcf)
                .Include(x => x.ExchangeCoupons)
                    .ThenInclude(x => x.Xcp)
                .Include(x => x.CtcCrds)
                    .ThenInclude(x => x.CrdCcf)
                .Include(x => x.Purchases)
                .Include(x => x.CtmTlp)
                    .ThenInclude(x => x.TlpTpt)
                .Include(x => x.CtmAdd)
                    .ThenInclude(x => x.AddRst)
                .Include(x => x.CtmAdd)
                    .ThenInclude(x => x.AddPpt)
                .Include(x => x.CtmAdd)
                    .ThenInclude(x => x.AddNbh)
                        .ThenInclude(x => x.NbhCty)
                            .ThenInclude(x => x.CtyStt)
                                .ThenInclude(x => x.SttCtr)
                .Include(x => x.BadAdds)
                    .ThenInclude(x => x.AddRst)
                .Include(x => x.BadAdds)
                    .ThenInclude(x => x.AddPpt)
                .Include(x => x.BadAdds)
                    .ThenInclude(x => x.AddNbh)
                        .ThenInclude(x => x.NbhCty)
                            .ThenInclude(x => x.CtyStt)
                                .ThenInclude(x => x.SttCtr)
                .Include(x => x.DadAdds)
                    .ThenInclude(x => x.AddRst)
                .Include(x => x.DadAdds)
                    .ThenInclude(x => x.AddPpt)
                .Include(x => x.DadAdds)
                    .ThenInclude(x => x.AddNbh)
                        .ThenInclude(x => x.NbhCty)
                            .ThenInclude(x => x.CtyStt)
                                .ThenInclude(x => x.SttCtr)
                .Include(x => x.CtmCrt)
                    .ThenInclude(x => x.CartItems)
                        .ThenInclude(x => x.CriStc)
                .FirstOrDefault(x => x.CtmId == id);
        }

        public List<Customer> GetAll() {
             return _dbContext.Customers
                .Include(x => x.CtmGnd)
                .Include(x => x.CtcCrds)
                    .ThenInclude(x => x.CrdCcf)
                .Include(x => x.ExchangeCoupons)
                    .ThenInclude(x => x.Xcp)
                .Include(x => x.CtcCrds)
                    .ThenInclude(x => x.CrdCcf)
                .Include(x => x.Purchases)
                .Include(x => x.CtmTlp)
                    .ThenInclude(x => x.TlpTpt)
                .Include(x => x.CtmAdd)
                    .ThenInclude(x => x.AddRst)
                .Include(x => x.CtmAdd)
                    .ThenInclude(x => x.AddPpt)
                .Include(x => x.CtmAdd)
                    .ThenInclude(x => x.AddNbh)
                        .ThenInclude(x => x.NbhCty)
                            .ThenInclude(x => x.CtyStt)
                                .ThenInclude(x => x.SttCtr)
                .Include(x => x.BadAdds)
                    .ThenInclude(x => x.AddRst)
                .Include(x => x.BadAdds)
                    .ThenInclude(x => x.AddPpt)
                .Include(x => x.BadAdds)
                    .ThenInclude(x => x.AddNbh)
                        .ThenInclude(x => x.NbhCty)
                            .ThenInclude(x => x.CtyStt)
                                .ThenInclude(x => x.SttCtr)
                .Include(x => x.DadAdds)
                    .ThenInclude(x => x.AddRst)
                .Include(x => x.DadAdds)
                    .ThenInclude(x => x.AddPpt)
                .Include(x => x.DadAdds)
                    .ThenInclude(x => x.AddNbh)
                        .ThenInclude(x => x.NbhCty)
                            .ThenInclude(x => x.CtyStt)
                                .ThenInclude(x => x.SttCtr)
                .Include(x => x.CtmCrt)
                .Include(x => x.Cart)
                .ToList();
        }

        public bool Remove(Customer customer) {
            _dbContext.Customers.Remove(customer);
            _dbContext.SaveChanges();

            return true;
        }

        public Customer Update(Customer customer) {
            var ctm = Get(customer.CtmId);

            if (ctm == null) throw new System.Exception("Um cliente com esse ID não foi encontrado.");
            _dbContext.Entry(ctm).State = EntityState.Detached;
            _dbContext.Entry(customer.CtmAdd).State = EntityState.Detached;

            _dbContext.Customers.Update(customer);
            _dbContext.SaveChanges();

            return customer;
        }
    }
}
