namespace E_CommerceLivraria.Models.ModelsStructGroups.CartSG {
    public class CartDataGroup {
        public decimal CtmId { get; set; }
        public decimal? removedStockId { get; set; }
        public List<CartItem>? Items { get; set; } = new List<CartItem>();
    }
}
