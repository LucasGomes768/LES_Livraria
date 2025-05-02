namespace E_CommerceLivraria.Models.ModelsStructGroups.PaymentSG
{
    public class PaymentOptionsData
    {
        public List<CreditCardPaymentData> CreditCards { get; set; }
        public List<decimal> Exchanges { get; set; }
        public String PromotionalCode {  get; set; }
    }
}
