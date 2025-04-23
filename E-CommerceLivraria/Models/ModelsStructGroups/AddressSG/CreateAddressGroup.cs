using E_CommerceLivraria.Enums;

namespace E_CommerceLivraria.Models.ModelsStructGroups.AddressSG {
    public class CreateAddressGroup {
        public Address Address { get; set; } = new Address();
        public Customer Ctm { get; set; } = new Customer();
        public EAddressType Type { get; set; }
        public List<PublicplaceType> PublicplaceTypes { get; set; } = new List<PublicplaceType>();
        public List<ResidenceType> ResidenceTypes { get; set; } = new List<ResidenceType>();
    }
}
