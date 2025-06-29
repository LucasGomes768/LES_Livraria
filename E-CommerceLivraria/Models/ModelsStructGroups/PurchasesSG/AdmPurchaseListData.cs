﻿using E_CommerceLivraria.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace E_CommerceLivraria.Models.ModelsStructGroups.PurchasesSG
{
    public class AdmPurchaseListData
    {
        public List<Purchase> Purchases { get; set; } = new List<Purchase>();
        public decimal? StatusId { get; set; }
    }
}
