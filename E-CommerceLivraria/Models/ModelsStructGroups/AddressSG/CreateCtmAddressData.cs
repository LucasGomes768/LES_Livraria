using E_CommerceLivraria.Enums.Customer;

namespace E_CommerceLivraria.Models.ModelsStructGroups.AddressSG
{
    public class CreateCtmAddressData
    {
        public decimal CtmId { get; set; }
        public int RedirectTo { get; set; }
        public Address Address { get; set; } = new Address();
        public bool AddToAccount { get; set; } = false;
        public string? AddToList { get; set; }
    }
}
