using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Specifications.CustomerSpecs.Purchases
{
    public class GetCtmsPurchases : BaseSpecification<Customer>
    {
        public GetCtmsPurchases() : base()
        {
            AddIncludes();
        }

        public GetCtmsPurchases(decimal id) : base(x => x.CtmId == id)
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            AddInclude(x => x.Purchases);
            AddInclude("Purchases.PrcAdd");
            AddInclude("Purchases.PrcCpp");
            AddInclude("Purchases.PrcCpp.Pcp");
            AddInclude("Purchases.PxcCpns");
            AddInclude("Purchases.PxcCpns.Xcp");
            AddInclude("Purchases.CreditCards.CcpCrd");
            AddInclude("Purchases.CreditCards.CcpCrd.CrdCcf");
            AddInclude("Purchases.PurchaseItems");
            AddInclude("Purchases.PurchaseItems.PciStc");
            AddInclude("Purchases.PurchaseItems.PciStc.StcBok");
        }
    }
}
