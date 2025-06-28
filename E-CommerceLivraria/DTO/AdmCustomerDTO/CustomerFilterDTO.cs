namespace E_CommerceLivraria.DTO.AdmCustomerDTO {
    public class CustomerFilterDTO {
        public decimal? Id { get; set; }
        public string? Name { get; set; }
        public decimal? Cpf { get; set; }
        public decimal? TelephoneTypeId { get; set; }
        public string? Email { get; set; }
        public decimal? GndId { get; set; }
        public bool? Active { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public decimal? Ranking { get; set; }
    }
}
