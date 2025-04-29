namespace E_CommerceLivraria.Models
{
    public class CreditCardsPurchase
    {
        public decimal CcpPrcId { get; set; }
        public decimal CcpCrdId { get; set; }
        public decimal CcpAmount { get; set; }

        public Purchase CcpPrc {  get; set; }
        public CreditCard CcpCrd { get; set; }
    }
}
