using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.DTO.AdmCustomerDTO {
    public class ReadAllCustomerDTO {
        public List<Customer> Customers { get; set; } = new List<Customer>();
        public CustomerFilterDTO FilterData { get; set; } = new CustomerFilterDTO();
        public List<Gender> Genders { get; set; } = new List<Gender>();
        public List<TelephoneType> TlpTypes { get; set; } = new List<TelephoneType>();
    }
}
