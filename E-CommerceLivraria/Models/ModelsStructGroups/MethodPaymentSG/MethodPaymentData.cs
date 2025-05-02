namespace E_CommerceLivraria.Models.ModelsStructGroups.MethodPaymentSG
{
    public class MethodPaymentData
    {
        public decimal CtmId { get; set; }
        public Address Address { get; set; }
        public decimal Total {  get; set; }
        public PromotionalCoupon? PromotionalCoupon { get; set; }
        public List<CreditCard>? CreditCards { get; set; }
        public List<ExchangeCoupon>? ExchangeCoupons { get; set; }
    }
}
