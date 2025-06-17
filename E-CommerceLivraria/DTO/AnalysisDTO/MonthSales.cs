namespace E_CommerceLivraria.DTO.AnalysisDTO
{
    public class MonthSales
    {
        public DateTime Time { get; set; }
        public decimal TotalProfit { get; set; } = 0;
        public decimal TotalSales { get; set; } = 0;
    }
}
