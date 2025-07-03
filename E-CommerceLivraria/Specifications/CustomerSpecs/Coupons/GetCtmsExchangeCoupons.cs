using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Specifications.CustomerSpecs.Coupons
{
    public class GetCtmsExchangeCoupons : BaseSpecification<Customer>
    {
        public GetCtmsExchangeCoupons() : base()
        {
            AddIncludes();
        }

        public GetCtmsExchangeCoupons(decimal id) : base(x => x.CtmId == id)
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            AddInclude(x => x.ExchangeCoupons);
            AddInclude("ExchangeCoupons.Xcp");
        }
    }
}
