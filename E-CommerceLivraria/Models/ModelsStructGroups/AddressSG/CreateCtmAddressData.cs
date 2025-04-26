using E_CommerceLivraria.Enums.Customer;

namespace E_CommerceLivraria.Models.ModelsStructGroups.AddressSG
{
    public class CreateCtmAddressData
    {
        public decimal CtmId { get; set; }
        public List<PublicplaceType> PublicplaceTypes { get; set; } = new List<PublicplaceType>();
        public List<ResidenceType> ResidenceTypes { get; set; } = new List<ResidenceType>();
        public string RedirectTo { get; set; } = "";
        public Address Address { get; set; } = new Address();
        public bool AddToAccount {  get; set; } = false;
    }
}
