namespace E_CommerceLivraria.Models.ModelsStructGroups {
    public class UpdateData {
        public Customer Ctm { get; set; } = new Customer();
        public string Birthdate { get; set; } = "";
        public List<Gender> Genders { get; set; } = new List<Gender>();
        public List<PublicplaceType> PublicplaceTypes { get; set; } = new List<PublicplaceType>();
        public List<ResidenceType> ResidenceTypes { get; set; } = new List<ResidenceType>();
    }
}
