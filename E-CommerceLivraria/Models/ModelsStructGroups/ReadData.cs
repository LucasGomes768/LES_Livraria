namespace E_CommerceLivraria.Models.ModelsStructGroups {
    public class ReadData {
        public List<Customer> Customers { get; set; } = new List<Customer>();
        public CustomerFilterData FilterData { get; set; } = new CustomerFilterData();
        public List<Gender> Genders { get; set; } = new List<Gender>();
        public List<TelephoneType> TlpTypes { get; set; } = new List<TelephoneType>();
    }
}
