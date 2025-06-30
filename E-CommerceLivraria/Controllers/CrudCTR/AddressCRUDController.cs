using E_CommerceLivraria.DTO.ProfileDTO.AddressDTO;
using E_CommerceLivraria.Enums;
using E_CommerceLivraria.Models;
using E_CommerceLivraria.Services.AddressS;
using E_CommerceLivraria.Services.CustomerS;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        [HttpGet("CRUD/Address/Get/{id:int}")]
        public IActionResult GetAddress([FromRoute] int id)
        {
            try
            {
                var add = _addressService.Get(id);
                if (add == null) return NotFound(new
                {
                    Sucess = false,
                    Message = "Endereço não foi encontrado"
                });

                var jsonString = JsonSerializer.Serialize(add, new JsonSerializerOptions
                {
                    WriteIndented = true,
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    ReferenceHandler = ReferenceHandler.IgnoreCycles
                });

                return Ok(new
                {
                    Sucess = true,
                    AddressJson = jsonString
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Sucess = true,
                    ex.Message
                });
            }
        }

        [HttpPost("CRUD/Address/Update")]
        public IActionResult UpdateAddress([FromBody] DetailedAddDTO dto)
        {
            try
            {
                _addressService.Update(dto);

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

        [HttpDelete("CRUD/Address/RemoveFromAccount/{Type:int}/{CtmId:decimal}/{AddId:decimal}")]
        public IActionResult deleteAddress([FromRoute] int Type, [FromRoute] decimal CtmId,[FromRoute] decimal AddId)
        {
            try
            {
                var add = _addressService.Get(AddId);
                if (add == null) return NotFound("Endereço não foi encontrado");

                var ctm = _customerService.Get(CtmId);
                if (ctm == null) return NotFound("Cliente não foi encontrado");

                bool result = _addressService.AccountRemove(add, ctm, (EAddressType)Type);

                if (!result)
                    return NotFound("O endereço não foi encontrado na conta");

                return Ok(new
                {
                    Sucess = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Success = false,
                    ex.Message
                });
            }
        }
    }
}
