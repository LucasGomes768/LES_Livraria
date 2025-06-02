namespace E_CommerceLivraria.DTO.ChatbotDTO
{
    public class RelevantCtmInfoAI
    {
        public string Name { get; set; }
        public int Age { get; set; }
        public string Gender { get; set; }
        public string Country { get; set; }
        public List<RelevantPrcItemAI> BoughtBooks { get; set; }
    }
}
