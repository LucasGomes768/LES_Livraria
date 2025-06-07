using E_CommerceLivraria.Services.AddressS;
using E_CommerceLivraria.Services.CustomerS;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceLivraria.Controllers.CrudCTR
{
    public class AddressCRUDController : Controller
    {
        readonly private ICustomerService _customerService;
        readonly private IAddressService _addressService;

        public AddressCRUDController(ICustomerService customerService, IAddressService addressService)
        {
            _customerService = customerService;
            _addressService = addressService;
        }

        [HttpGet()]
        public IActionResult Index()
        {
            return View();
        }
    }
}
