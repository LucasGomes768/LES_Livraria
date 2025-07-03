using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Specifications.CustomerSpecs.Coupons
{
    public class GetCtmsPromoCoupons : BaseSpecification<Customer>
    {
        public GetCtmsPromoCoupons() : base()
        {
            AddIncludes();
        }

        public GetCtmsPromoCoupons(decimal id) : base(x => x.CtmId == id)
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            AddInclude(x => x.PcmCpns);
            AddInclude("PcmCpns.Pcp");
        }
    }
}
