namespace E_CommerceLivraria.DTO.ChatbotDTO
{
    public class PromptRelevantInfoAI
    {
        public string Message { get; set; }
        public RelevantCtmInfoAI CustomerInfo { get; set; }
        public List<RelevantBookInfoAI> StoreBooksInfo { get; set; }
    }
}
