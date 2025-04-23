namespace E_CommerceLivraria.Models.ModelsStructGroups {
    public class ReadDataGroup {
        public List<Customer> Customers { get; set; } = new List<Customer>();
        public CustomerFilter FilterData { get; set; } = new CustomerFilter();
        public List<Gender> Genders { get; set; } = new List<Gender>();
        public List<TelephoneType> TlpTypes { get; set; } = new List<TelephoneType>();
    }
}
