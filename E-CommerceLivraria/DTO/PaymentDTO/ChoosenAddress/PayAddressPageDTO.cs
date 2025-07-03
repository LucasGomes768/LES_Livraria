using E_CommerceLivraria.Models;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceLivraria.DTO.PaymentDTO.ChoosenAddress
{
    public class PayAddressPageDTO
    {
        public Cart? Cart { get; set; } = new Cart();
        public List<Address>? Addresses { get; set; } = new List<Address>();
        public decimal Total { get; set; }
        public bool tempAdded { get; set; } = false;

        [BindProperty(SupportsGet = true)]
        public decimal? ChoosenAddId { get; set; }
        public Address? ChoosenAdd { get; set; }
    }
}
