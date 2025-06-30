namespace E_CommerceLivraria.DTO.ProfileDTO.AddressDTO
{
    public class DetailedAddDTO
    {
        public decimal Id { get; set; }
        public decimal CtmId { get; set; }
        public int Type { get; set; }
        public string PublicPlace { get; set; }
        public decimal? PublicPlaceType { get; set; }
        public decimal? ResidenceType { get; set; }
        public string Cep { get; set; }
        public decimal? Number { get; set; }
        public string Neighborhood { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string ShortPhrase { get; set; }
        public string? Observations { get; set; }
    }
}
