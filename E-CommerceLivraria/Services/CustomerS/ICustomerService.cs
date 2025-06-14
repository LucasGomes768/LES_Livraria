﻿using System.Runtime.InteropServices;
using E_CommerceLivraria.DTO.ChatbotDTO;
using E_CommerceLivraria.DTO.ProfileDTO.InfoDTO;
using E_CommerceLivraria.Models;

namespace E_CommerceLivraria.Services.CustomerS {
    public interface ICustomerService {
        public Customer Create(Customer customer);
        public Customer? Get(decimal id);
        public List<Customer> GetAll();
        public bool Remove(decimal id);
        public Customer Update(Customer customer);
        public Customer UpdateBasicInfo(InfoDTO info);
        public Customer UpdatePassword(InfoDTO info);
        public bool Exists(decimal id);
        public void ClearCart(Customer customer);
        public RelevantCtmInfoAI GetInfoForChatbot(decimal id);
    }
}
