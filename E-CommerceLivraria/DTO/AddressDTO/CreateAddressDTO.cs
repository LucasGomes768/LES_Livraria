using E_CommerceLivraria.Enums;
using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.DTO.AddressDTO
{
    public class CreateAddressDTO
    {
        public int CtmId { get; set; }
        public int RedirectTo { get; set; }
        public Address Address { get; set; } = new Address();
        public bool AddToAccount { get; set; } = false;
        public int? AddToList { get; set; }
    }
}
