using E_CommerceLivraria.DTO.ProfileDTO.AddressDTO;
using E_CommerceLivraria.Models;
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

        [HttpPost("CRUD/Address/Update")]
        public IActionResult updateAddress([FromBody] DetailedAddDTO dto)
        {
            try
            {
                var add = _addressService.Get(dto.Id);
                if (add == null) return NotFound("Endereço não foi encontrado");

                Country newCtr = new Country()
                {
                    CtrName = dto.Country ?? add.AddNbh.NbhCty.CtyStt.SttCtr.CtrName
                };

                State newStt = new State()
                {
                    SttName = dto.State ?? add.AddNbh.NbhCty.CtyStt.SttName,
                    SttCtr = newCtr
                };

                City newCty = new City()
                {
                    CtyName = dto.City ?? add.AddNbh.NbhCty.CtyName,
                    CtyStt = newStt
                };

                Neighborhood newNbh = new Neighborhood()
                {
                    NbhName = dto.Neighborhood ?? add.AddNbh.NbhName,
                    NbhCty = newCty
                };

                Address newAdd = new Address()
                {
                    AddId = add.AddId,
                    AddPublicPlace = dto.PublicPlace ?? add.AddPublicPlace,
                    AddNumber = dto.Number ?? add.AddNumber,
                    AddCep = dto.Cep != null ? decimal.Parse(dto.Cep.Trim().Replace("-","")) : add.AddCep,
                    AddShortPhrase = dto.ShortPhrase ?? add.AddShortPhrase,
                    AddShipping = add.AddShipping,
                    AddPptId = dto.PublicPlaceType ?? add.AddPptId,
                    AddRstId = dto.ResidenceType ?? add.AddRstId,
                    AddObservations = dto.Observations ?? add.AddObservations,
                    AddNbh = newNbh,
                    Customers = add.Customers,
                    Purchases = add.Purchases,
                    BadCtms = add.BadCtms,
                    DadCtms = add.DadCtms
                };

                _addressService.Update(newAdd);

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
