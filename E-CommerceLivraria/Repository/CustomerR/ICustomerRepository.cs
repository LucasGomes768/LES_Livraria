using E_CommerceLivraria.Models;
using E_CommerceLivraria.Specifications;

namespace E_CommerceLivraria.Repository.CustomerR {
    public interface ICustomerRepository {
        public Customer Add(Customer customer);
        public bool Remove(Customer customer);
        public Customer Update(Customer customer);
        public Customer? Get(decimal id);
        public Customer? Get(ISpecification<Customer> specification);
        public List<Customer> GetAll();
        public List<Customer> GetAll(ISpecification<Customer> specification);
    }
}
