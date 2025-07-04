﻿using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.DTO.StoreDTO {
    public class StorePageDTO {
        public List<Stock>? Stocks { get; set; } = new List<Stock>();

        // Filtro
        public List<Category> Categories { get; set; } = new List<Category>();
        public string? Title { get; set; }
        public string? Author { get; set; }
        public string? Publisher { get; set; }
        public decimal? CategoryId { get; set; }
        public decimal? PriceMin { get; set; }
        public decimal? PriceMax { get; set; }
    }
}
