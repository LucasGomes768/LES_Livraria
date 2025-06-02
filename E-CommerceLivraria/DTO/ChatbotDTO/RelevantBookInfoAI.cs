namespace E_CommerceLivraria.DTO.ChatbotDTO
{
    public class RelevantBookInfoAI
    {
        public string Title { get; set; }
        public string Sinopsis { get; set; }
        public int Edition { get; set; }
        public int PagesAmount { get; set; }
        public int Year { get; set; }
        public string Publisher { get; set; }
        public string Author { get; set; }
        public List<string> Categories { get; set; }
        public decimal Price { get; set; }
        public int AvailableAmount { get; set; }
    }
}
