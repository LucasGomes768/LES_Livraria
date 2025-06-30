using E_CommerceLivraria.Models;
using E_CommerceLivraria.Services.CustomerS;
using static Microsoft.AspNetCore.Razor.Language.TagHelperMetadata;

namespace E_CommerceLivraria.Services.CouponS
{
    public class PromoCouponAssignmentService : IPromoCouponAssignmentService
    {
        private readonly ICustomerService _customerService;
        private readonly IPromotionalCouponService _promotionalCouponService;

        public PromoCouponAssignmentService(ICustomerService customerService, IPromotionalCouponService promotionalCouponService)
        {
            _customerService = customerService;
            _promotionalCouponService = promotionalCouponService;
        }

        public Customer AddAllPromoCouponToCtm(Customer customer)
        {
            List<PromotionalCoupon> cpns = _promotionalCouponService.GetAllActive();
            
            foreach (PromotionalCoupon cp in cpns)
            {
                if (!customer.PcmCpns.Contains(cp))
                {
                    customer.PcmCpns.Add(cp);
                }
            }
            return _customerService.Update(customer);
        }

        public PromotionalCoupon AddPromoCouponToAllCtms(PromotionalCoupon promotionalCoupon)
        {
            List<Customer> customers = _customerService.GetAll();
            
            foreach (Customer customer in customers)
            {
                if (!promotionalCoupon.PcmCtms.Contains(customer))
                {
                    promotionalCoupon.PcmCtms.Add(customer);
                }
            }

            return _promotionalCouponService.Update(promotionalCoupon);
        }

        public Customer RemovePromoCouponFromCtm(Customer customer, PromotionalCoupon promotionalCoupon)
        {
            customer.PcmCpns.Remove(promotionalCoupon);
            return _customerService.Update(customer);
        }

        public Customer RemovePromoCouponFromCtm(Customer customer, string code)
        {
            var cpn = _promotionalCouponService.GetByCode(code);
            if (cpn == null) throw new Exception("Cupom promocional não foi encontrado");

            return RemovePromoCouponFromCtm(customer, cpn);
        }
    }
}
