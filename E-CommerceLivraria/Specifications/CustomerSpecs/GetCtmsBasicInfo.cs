using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Specifications.CustomerSpecs
{
    public class GetCtmsBasicInfo : BaseSpecification<Customer>
    {
        public GetCtmsBasicInfo() : base()
        {
            AddInclude(x => x.CtmGnd);
            AddInclude(x => x.CtmTlp);
            AddInclude("CtmTlp.TlpTpt");
            AddInclude("CtmAdd.AddNbh.NbhCty.CtyStt.SttCtr");
            AddInclude("CtmAdd.AddNbh.NbhCty.CtyStt");
            AddInclude("CtmAdd.AddNbh.NbhCty");
            AddInclude("CtmAdd.AddNbh");
            AddInclude("CtmAdd.AddRst");
            AddInclude("CtmAdd.AddPpt");
        }

        public GetCtmsBasicInfo(decimal ctmId) : base(x => x.CtmId == ctmId)
        {
            AddInclude(x => x.CtmGnd);
            AddInclude(x => x.CtmTlp);
            AddInclude("CtmAdd.AddNbh.NbhCty.CtyStt.SttCtr");
            AddInclude("CtmAdd.AddNbh.NbhCty.CtyStt");
            AddInclude("CtmAdd.AddNbh.NbhCty");
            AddInclude("CtmAdd.AddNbh");
            AddInclude("CtmAdd.AddRst");
            AddInclude("CtmAdd.AddPpt");
        }
    }
}
