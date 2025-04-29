namespace E_CommerceLivraria.Models.ModelsStructGroups.CreditCardSG
{
    public class CreditCardPurchaseData
    {
        public CreditCard CreditCard { get; set; } = new CreditCard();
        public decimal PurchaseValue { get; set; }
    }
}
