using E_CommerceLivraria.Models;
using System.Runtime.CompilerServices;

namespace E_CommerceLivraria.Specifications.CustomerSpecs.CreditCards
{
    public class GetCtmsCreditCards : BaseSpecification<Customer>
    {
        public GetCtmsCreditCards() : base()
        {
            AddIncludes();
        }

        public GetCtmsCreditCards(decimal id) : base(x => x.CtmId == id)
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            AddInclude(x => x.CtcCrds);
            AddInclude("CtcCrds.CrdCcf");
        }
    }
}
