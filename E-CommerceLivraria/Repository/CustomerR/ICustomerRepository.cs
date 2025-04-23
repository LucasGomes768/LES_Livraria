using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Repository.CustomerR {
    public interface ICustomerRepository {
        public Customer Add(Customer customer);
        public bool Remove(Customer customer);
        public Customer Update(Customer customer);
        public Customer? Get(decimal id);
        public List<Customer> GetAll();
        public bool Exists(decimal id);
    }
}
