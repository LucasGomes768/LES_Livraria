using E_CommerceLivraria.Models;
using E_CommerceLivraria.Repository.CustomerR.CouponR;
using E_CommerceLivraria.Services.CustomerS;
using Newtonsoft.Json.Linq;
using System.Runtime.CompilerServices;

namespace E_CommerceLivraria.Services.CouponS
{
    public class PromotionalCouponService : IPromotionalCouponService
    {
        private readonly IPromotionalCouponRepository _promotionalCouponRepository;
        private readonly ICustomerService _customerService;
        
        public PromotionalCouponService(IPromotionalCouponRepository promotionalCouponRepository,  ICustomerService customerService)
        {
            _promotionalCouponRepository = promotionalCouponRepository;
            _customerService = customerService;
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
                PcpCode = code,
                PcmCtms = _customerService.GetAll()
            };
        
            return _promotionalCouponRepository.Create(coupon);
        }
        public PromotionalCoupon? GetByCode(string code) {
            var cpns = _promotionalCouponRepository.GetAll();

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

        public PromotionalCoupon RemoveFromAccount(Customer customer, PromotionalCoupon coupon)
        {
            coupon.PcmCtms.Remove(customer);
            return _promotionalCouponRepository.Update(coupon);
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
