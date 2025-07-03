using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.DTO.CartDTO {
    public class CartItemsDTO {
        public decimal CtmId { get; set; }
        public decimal? removedStockId { get; set; }
        public List<CartItem>? Items { get; set; } = new List<CartItem>();
    }
}
