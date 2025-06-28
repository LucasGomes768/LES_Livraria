using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.DTO.AdmCustomerDTO {
    public class CreateCustomerDTO {
        public Customer Ctm { get; set; } = new Customer();
        public string Birthdate { get; set; } = "";
        public Address Billing { get; set; } = new Address();
        public Address Delivery { get; set; } = new Address();
    }
}
