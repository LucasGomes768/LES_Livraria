using E_CommerceLivraria.Models;
using E_CommerceLivraria.Repository.CustomerR;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceLivraria.Services.CustomerS {
    public class CustomerService : ICustomerService{
        private readonly ICustomerRepository _customerRepository;
        private readonly ICartService _cartService;

        public CustomerService(ICustomerRepository customerRepository, ICartService cartService) {
            _customerRepository = customerRepository;
            _cartService = cartService;
        }

        public Customer Create(Customer customer) {
            return _customerRepository.Add(customer);
        }

        public bool Exists(decimal id) {
            return _customerRepository.Exists(id);
        }

        public Customer? Get(decimal id) {
            return _customerRepository.Get(id);
        }

        public List<Customer> GetAll() {
            return _customerRepository.GetAll();
        }

        public bool Remove(decimal id) {
            var ctm = Get(id);

            if (ctm == null) throw new System.Exception("Um cliente com esse ID não foi encontrado.");

            ctm.BadAdds.Clear();
            ctm.DadAdds.Clear();
            ctm.ExchangeCoupons.Clear();

            if (ctm.Cart != null) _cartService.Remove(ctm.Cart);

            return _customerRepository.Remove(ctm);
        }

        public Customer Update(Customer customer) {
            return _customerRepository.Update(customer);
        }
    }
}
