using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.DTO.PaymentDTO.ChoosenAddress
{
    public class ChoosenAddressDTO
    {
        public decimal AddId { get; set; }
        public string PublicPlace { get; set; } = "";
        public string PublicPlaceTypeName { get; set; } = "";
        public string ResidenceTypeName { get; set; } = "";
        public decimal AddNumber {  get; set; }
        public string AddShortPhrase { get; set; } = "";
        public string? AddObservations { get; set; }
        public decimal AddShipping { get; set; }
        public string NeighborhoodName { get; set; } = "";
        public string CityName { get; set; } = "";
        public string StateName { get; set; } = "";
        public string CountryName { get; set; } = "";

        public ChoosenAddressDTO(Address address)
        {
            AddId = address.AddId;
            PublicPlace = address.AddPublicPlace;
            PublicPlaceTypeName = address.AddPpt.PptName;
            ResidenceTypeName = address.AddRst.RstName;
            AddNumber = address.AddNumber;
            AddShortPhrase = address.AddShortPhrase;
            AddObservations = address.AddObservations;
            AddShipping = address.AddShipping;
            NeighborhoodName = address.AddNbh.NbhName;
            CityName = address.AddNbh.NbhCty.CtyName;
            StateName = address.AddNbh.NbhCty.CtyStt.SttName;
            CountryName = address.AddNbh.NbhCty.CtyStt.SttCtr.CtrName;
        }
    }
}
