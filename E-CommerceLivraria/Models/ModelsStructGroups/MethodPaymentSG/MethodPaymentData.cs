using E_CommerceLivraria.Models.ModelsStructGroups.CreditCardSG;

namespace E_CommerceLivraria.Models.ModelsStructGroups.MethodPaymentSG
{
    public class MethodPaymentData
    {
        public decimal CtmId { get; set; }
        public Address Address { get; set; }
        public decimal Total {  get; set; }
        public string? PromotionalCode { get; set; }
        public PromotionalCoupon? PromotionalCoupon { get; set; }
        public decimal ChoosenCardId { get; set; }
        public List<CreditCard>? CreditCards { get; set; }
        public List<CreditCardPurchaseData> CreditCardsUsed { get; set; } = new List<CreditCardPurchaseData>();
        public decimal ChoosenExCoupon { get; set; }
        public List<ExchangeCoupon>? ExchangeCoupons { get; set; }
        public List<ExchangeCoupon> ExchangeCouponsUsed { get; set; }
    }
}
