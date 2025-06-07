using E_CommerceLivraria.DTO.ProfileDTO.InfoDTO;
using E_CommerceLivraria.Models;
using E_CommerceLivraria.Services.CustomerS;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceLivraria.Controllers.CrudCTR
{
    public class CustomerCRUDController : Controller
    {
        private ICustomerService _customerService;

        public CustomerCRUDController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost("/CRUD/Customer/InfoUpdate")]
        public IActionResult UpdateCustomer([FromBody] InfoDTO info)
        {
            try
            {
                if (info == null) return BadRequest("Cadastro do cliente nulo");

                _customerService.UpdateBasicInfo(info);

                return Ok(new {
                    Sucess = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Sucess = false,
                    ex.Message
                });
            }
        }

        [HttpPost("/CRUD/Customer/PasswordUpdate")]
        public IActionResult UpdatePassword([FromBody] InfoDTO infoDTO)
        {
            try
            {
                if (infoDTO == null) return BadRequest("Nenhuma senha enviada");

                _customerService.UpdatePassword(infoDTO);

                return Ok(new
                {
                    Sucess = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Sucess = false,
                    ex.Message
                });
            }
        }
    }
}
