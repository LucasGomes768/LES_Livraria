using E_CommerceLivraria.Data;
using E_CommerceLivraria.Models;
using E_CommerceLivraria.Specifications;
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

        public Customer? Get(ISpecification<Customer> specification)
        {
            return SpecificationEvaluator.GetQuery(_dbContext.Customers, specification).FirstOrDefault();
        }

        public Customer? Get(decimal id) {
            return _dbContext.Customers
                .Include(x => x.CtmGnd)
                .Include(x => x.CtcCrds)
                    .ThenInclude(x => x.CrdCcf)
                .Include(x => x.ExchangeCoupons)
                    .ThenInclude(x => x.Xcp)
                .Include(x => x.Purchases)
                    .ThenInclude(x => x.PurchaseItems)
                        .ThenInclude(x => x.PciStc)
                            .ThenInclude(x => x.StcBok)
                .Include(x => x.Purchases)
                    .ThenInclude(x => x.PrcAdd)
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

        public List<Customer> GetAll(ISpecification<Customer> specification)
        {
            return SpecificationEvaluator.GetQuery(_dbContext.Customers, specification).ToList();
        }

        public List<Customer> GetAll() {
             return _dbContext.Customers
                .Include(x => x.CtmGnd)
                .Include(x => x.CtcCrds)
                    .ThenInclude(x => x.CrdCcf)
                .Include(x => x.ExchangeCoupons)
                    .ThenInclude(x => x.Xcp)
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
            bool exists = _dbContext.Customers.Any(x => x.CtmId == customer.CtmId);
            if (!exists) throw new System.Exception("Um cliente com esse ID não foi encontrado.");
            
            _dbContext.Customers.Update(customer);
            _dbContext.SaveChanges();

            return customer;
        }
    }
}
