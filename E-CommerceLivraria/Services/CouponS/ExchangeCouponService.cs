using E_CommerceLivraria.Models;
using E_CommerceLivraria.Repository.CustomerR.CouponR;
using E_CommerceLivraria.Services.CustomerS;

namespace E_CommerceLivraria.Services.CouponS
{
    public class ExchangeCouponService : IExchangeCouponService
    {
        private readonly IExchangeCouponRepository _exchangeCouponRepository;
        private readonly ICustomerService _customerService;
        
        public ExchangeCouponService(IExchangeCouponRepository exchangeCouponRepository, ICustomerService customerService)
        {
            _exchangeCouponRepository = exchangeCouponRepository;
            _customerService = customerService;
        }

        public ExchangeCoupon AddToCtm(Customer customer, decimal value)
        {
            Coupon coupon = new Coupon()
            {
                CpnValue = value,
                CpnDateGen = DateTime.Now
            };

            ExchangeCoupon Xcoupon = new ExchangeCoupon()
            {
                XcpCtmId = customer.CtmId,
                XcpCtm = customer,
                Xcp = coupon
            };

            Xcoupon = _exchangeCouponRepository.Add(Xcoupon);

            customer.ExchangeCoupons.Add(Xcoupon);
            _customerService.Update(customer);

            return Xcoupon;
        }

        public ExchangeCoupon Get(decimal id)
        {
            throw new NotImplementedException();
        }

        public List<ExchangeCoupon> GetAllByCtm(Customer customer)
        {
            throw new NotImplementedException();
        }

        public ExchangeCoupon Remove(ExchangeCoupon coupon)
        {
            throw new NotImplementedException();
        }
    }
}
