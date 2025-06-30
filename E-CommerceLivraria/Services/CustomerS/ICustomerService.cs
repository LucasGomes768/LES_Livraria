using E_CommerceLivraria.DTO.AdmCustomerDTO;
using E_CommerceLivraria.DTO.ChatbotDTO;
using E_CommerceLivraria.DTO.PaymentDTO;
using E_CommerceLivraria.DTO.ProfileDTO.InfoDTO;
using E_CommerceLivraria.Models;
using System.Runtime.InteropServices;

namespace E_CommerceLivraria.Services.CustomerS {
    public interface ICustomerService {
        public Customer Create(CreateCustomerDTO createData);
        public Customer? Get(decimal id);
        public List<Customer> GetAll();
        public Customer Deactivate(Customer ctm);
        public Customer Activate(Customer ctm);
        public Customer RemoveCreditCard(Customer customer, CreditCard creditCard);
        public Customer Update(Customer customer);
        public Customer UpdateBasicInfo(InfoDTO info);
        public Customer UpdatePassword(InfoDTO info);
        public bool Exists(decimal id);
        public RelevantCtmInfoAI GetInfoForChatbot(decimal id);
    }
}
