using E_CommerceLivraria.DTO.ProfileDTO.InfoDTO;
using E_CommerceLivraria.Models;
using E_CommerceLivraria.Services.CustomerS;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceLivraria.Controllers
{
    public class CustomerCRUDController : Controller
    {
        private ICustomerService _customerService;

        public CustomerCRUDController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        public IActionResult UpdateCustomer(InfoDTO info)
        {
            try
            {
                if (info == null) return BadRequest("Cadastro do cliente nulo");

                return Ok(new {
                    Sucess = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Sucess = false,
                    Message = ex.Message
                });
            }
        }
    }
}
