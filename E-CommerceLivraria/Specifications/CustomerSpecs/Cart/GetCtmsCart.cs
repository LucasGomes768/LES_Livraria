using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Specifications.CustomerSpecs.Cart
{
    public class GetCtmsCart : BaseSpecification<Customer>
    {
        public GetCtmsCart() : base()
        {
            AddIncludes();
        }

        public GetCtmsCart(decimal id) : base(x => x.CtmId == id)
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            AddInclude(x => x.CtmCrt);
            AddInclude("CtmCrt.CartItems");
            AddInclude("CtmCrt.CartItems.CriStc");
            AddInclude("CtmCrt.CartItems.CriStc.StcBok");
        }
    }
}
