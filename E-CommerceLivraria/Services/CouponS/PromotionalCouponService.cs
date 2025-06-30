using E_CommerceLivraria.Models;
using E_CommerceLivraria.Repository.CustomerR.CouponR;
using E_CommerceLivraria.Services.CustomerS;

namespace E_CommerceLivraria.Services.CouponS
{
    public class PromotionalCouponService : IPromotionalCouponService
    {
        private readonly IPromotionalCouponRepository _promotionalCouponRepository;
        
        public PromotionalCouponService(IPromotionalCouponRepository promotionalCouponRepository)
        {
            _promotionalCouponRepository = promotionalCouponRepository;
        }

        public PromotionalCoupon Create(decimal value, string? code = null) {
            if (value <= 0) throw new Exception("Valor do cupom menor ou igual a zero");
            
            if (code == null || code == "") code = generateRndCode();
            if (code.Length != 10) throw new Exception("Código do cupom deve ter exatamente 10 caracteres");

            PromotionalCoupon coupon = new PromotionalCoupon()
            {
                Pcp = new Coupon
                {
                    CpnDateGen = DateTime.Now,
                    CpnValue = value
                },
                PcpActive = true,
                PcpCode = code.ToUpper()
            };
        
            return _promotionalCouponRepository.Create(coupon);
        }

        public PromotionalCoupon? GetIfCtmHas(Customer ctm, string code)
        {
            var cpn = GetByCode(code);
            if (cpn == null) return null;

            if (cpn.PcmCtms.Contains(ctm)) return cpn;
            else throw new Exception("O cupom promocional já foi usado pelo cliente");
        }

        public PromotionalCoupon? GetByCode(string code) {
            if (code == null || code == string.Empty) throw new Exception("Nenhum código foi fornecido");
            if (code.Length != 10) throw new Exception("O código de um cupom promocional deve ter 10 caracteres");

            var cpns = GetAllActive();

            return cpns.FirstOrDefault(x => x.PcpCode == code);
        }

        public List<PromotionalCoupon> GetAllActive()
        {
            return _promotionalCouponRepository.GetAll().Where(x => x.PcpActive == true).ToList();
        }

        public List<PromotionalCoupon> GetAll() {
            return _promotionalCouponRepository.GetAll();    
        }

        public PromotionalCoupon Update(PromotionalCoupon coupon) {
            if (coupon.Pcp.CpnValue <= 0) throw new Exception("Valor do cupom menor ou igual a zero");
            if (coupon.PcpCode.Length != 10) throw new Exception("Código do cupom deve ter exatamente 10 caracteres");

            return _promotionalCouponRepository.Update(coupon);
        }

        public PromotionalCoupon Deactivate(decimal id) {
            var cpn = _promotionalCouponRepository.Get(id);
            if (cpn == null) throw new Exception("Cupom promocional não foi encontrado");
            if (!cpn.PcpActive) throw new Exception("Cupom promocional já está inativo");

            cpn.PcmCtms.Clear();
            cpn.PcpActive = false;
            cpn.PcpDeactivatedAt = DateTime.Now;

            return _promotionalCouponRepository.Update(cpn);
        }

        private string generateRndCode()
        {
            Random rnd = new Random();

            int rndValue;
            string code = "";

            for (int i = 0; i < 10; i++)
            {
                rndValue = rnd.Next(0, 26);
                code += Convert.ToChar(rndValue + 65);
            }

            return code;
        }
    }
}
