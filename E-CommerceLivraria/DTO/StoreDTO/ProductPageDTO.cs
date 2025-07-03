using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.DTO.StoreDTO {
    public class ProductPageDTO {
        public Stock? Stock { get; set; }
        public bool InCart { get; set; }

        // Adicionar ao carrinho
        public decimal StockId { get; set; }
        public decimal Quantity { get; set; }
    }
}
