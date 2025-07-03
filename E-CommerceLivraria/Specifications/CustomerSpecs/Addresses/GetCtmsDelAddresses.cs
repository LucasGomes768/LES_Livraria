using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Specifications.CustomerSpecs.Addresses
{
    public class GetCtmsDelAddresses : BaseSpecification<Customer>
    {
        public GetCtmsDelAddresses() : base() {
            AddIncludes();
        }

        public GetCtmsDelAddresses(decimal id) : base(x => x.CtmId == id)
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            AddInclude(x => x.DadAdds);
            AddInclude("DadAdds.AddNbh.NbhCty.CtyStt.SttCtr");
            AddInclude("DadAdds.AddNbh.NbhCty.CtyStt");
            AddInclude("DadAdds.AddNbh.NbhCty");
            AddInclude("DadAdds.AddNbh");
            AddInclude("DadAdds.AddRst");
            AddInclude("DadAdds.AddPpt");
        }
    }
}
