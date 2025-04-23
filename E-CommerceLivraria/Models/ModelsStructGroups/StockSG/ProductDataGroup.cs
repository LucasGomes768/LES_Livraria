namespace E_CommerceLivraria.Models.ModelsStructGroups.StockSG {
    public class ProductDataGroup {
        public Stock? Stock { get; set; }
        public bool InCart { get; set; }

        // Adicionar ao carrinho
        public decimal StockId { get; set; }
        public decimal CtmId { get; set; }
        public decimal Quantity { get; set; }
    }
}
