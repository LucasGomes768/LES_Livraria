namespace E_CommerceLivraria.DTO.AnalysisDTO
{
    public class DataSalesDTO
    {
        public decimal? Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<MonthSales> MonthSales { get; set; } = new List<MonthSales>();

        public int GetYearDate(DateTime date)
        {
            int index = MonthSales.FindIndex(x => (x.Time.Year == date.Year) && (x.Time.Month == date.Month));
            return index;
        }
    }
}
