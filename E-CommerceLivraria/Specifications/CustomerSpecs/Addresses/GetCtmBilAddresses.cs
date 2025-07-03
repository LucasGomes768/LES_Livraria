using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Specifications.CustomerSpecs.Addresses
{
    public class GetCtmBilAddresses : BaseSpecification<Customer>
    {
        public GetCtmBilAddresses() : base() {
            AddIncludes();
        }

        public GetCtmBilAddresses(decimal id) : base(x => x.CtmId == id)
        {
            AddIncludes();
        }

        private void AddIncludes()
        {
            AddInclude(x => x.BadAdds);
            AddInclude("BadAdds.AddNbh.NbhCty.CtyStt.SttCtr");
            AddInclude("BadAdds.AddNbh.NbhCty.CtyStt");
            AddInclude("BadAdds.AddNbh.NbhCty");
            AddInclude("BadAdds.AddNbh");
            AddInclude("BadAdds.AddRst");
            AddInclude("BadAdds.AddPpt");
        }
    }
}
