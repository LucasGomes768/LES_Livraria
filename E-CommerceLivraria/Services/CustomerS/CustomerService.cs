using E_CommerceLivraria.DTO.ChatbotDTO;
using E_CommerceLivraria.DTO.ProfileDTO.InfoDTO;
using E_CommerceLivraria.Enums;
using E_CommerceLivraria.Models;
using E_CommerceLivraria.Repository.CustomerR;
using E_CommerceLivraria.Services.CreditCardS;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceLivraria.Services.CustomerS {
    public class CustomerService : ICustomerService{
        private readonly ICustomerRepository _customerRepository;
        private readonly ICartService _cartService;
        private readonly ICreditCardService _creditCardService;

        public CustomerService(ICustomerRepository customerRepository, ICartService cartService, ICreditCardService creditCardService) {
            _customerRepository = customerRepository;
            _cartService = cartService;
            _creditCardService = creditCardService;
        }

        public Customer Create(Customer customer) {
            return _customerRepository.Add(customer);
        }

        public bool Exists(decimal id) {
            return _customerRepository.Exists(id);
        }

        public Customer? Get(decimal id) {
            return _customerRepository.Get(id);
        }

        public List<Customer> GetAll() {
            return _customerRepository.GetAll();
        }

        public bool Remove(decimal id) {
            var ctm = Get(id);

            if (ctm == null) throw new System.Exception("Um cliente com esse ID não foi encontrado.");

            ctm.BadAdds.Clear();
            ctm.DadAdds.Clear();
            ctm.ExchangeCoupons.Clear();

            if (ctm.Cart != null) _cartService.Remove(ctm.Cart);

            return _customerRepository.Remove(ctm);
        }

        public Customer Update(Customer customer) {
            return _customerRepository.Update(customer);
        }

        public void ClearCart(Customer customer)
        {
            if (Exists(customer.CtmId)) return;
            if (customer.Cart.CartItems.Count < 1 || !customer.Cart.CartItems.Any()) return;

            customer.Cart.CartItems.Clear();
            Update(customer);
        }

        public RelevantCtmInfoAI GetInfoForChatbot(decimal id)
        {
            var ctm = _customerRepository.Get(id);
            if (ctm == null) throw new Exception("Cliente não foi encontrado");

            RelevantCtmInfoAI info = new RelevantCtmInfoAI()
            {
                Name = ctm.CtmName,
                Age = DateTime.Now.Year - ctm.CtmBirthdate.Year,
                Gender = ctm.CtmGnd.GndName,
                Country = ctm.CtmAdd.AddNbh.NbhCty.CtyStt.SttCtr.CtrName,
                BoughtBooks = new List<RelevantPrcItemAI>()
            };

            foreach (Purchase purchase in ctm.Purchases)
            {
                foreach (PurchaseItem item in purchase.PurchaseItems)
                {
                    if (item.PciStatus >= (int)EStatus.TROCA_SOLICITADA ||
                        item.PciStatus <= (int)EStatus.EM_PROCESSAMENTO)
                        continue;

                    var bookInfo = new RelevantPrcItemAI()
                    {
                        BookTitle = item.PciStc.StcBok.BokTitle,
                        QuantityBought = (int)item.PciQuantity
                    };

                    int index = info.BoughtBooks.IndexOf(bookInfo);
                    if (index == -1)
                        info.BoughtBooks.Add(bookInfo);
                    else
                        info.BoughtBooks[index].QuantityBought += bookInfo.QuantityBought;
                }
            }

            return info;
        }

        public Customer UpdateBasicInfo(InfoDTO info)
        {
            Customer? ctm = _customerRepository.Get(info.Id);
            if (ctm == null) throw new Exception("Cliente não foi encontrado");

            ctm.CtmName = info.Name;
            ctm.CtmEmail = info.Email;
            ctm.CtmPass = info.Pass ?? ctm.CtmPass;
            ctm.CtmCpf = info.Cpf != null ? decimal.Parse(info.Cpf.Replace("-","").Replace(".","").Trim()) : ctm.CtmCpf;
            ctm.CtmActive = info.Active;
            ctm.CtmBirthdate = info.BirthDate;
            ctm.CtmGndId = info.Gender;
            ctm.CtmTlp.TlpDdd = info.Ddd;
            ctm.CtmTlp.TlpNumber = info.TlpNum;
            ctm.CtmTlp.TlpTptId = info.Tpt;

            return _customerRepository.Update(ctm);
        }

        public Customer UpdatePassword(InfoDTO info)
        {
            Customer? ctm = _customerRepository.Get(info.Id);
            if (ctm == null) throw new Exception("Cliente não foi encontrado");

            if (info.Pass == null) throw new Exception("Senha nova não foi enviada");

            ctm.CtmPass = info.Pass;

            return _customerRepository.Update(ctm);
        }

        public Customer addCreditCard(CreditCard creditCard, Customer customer)
        {
            if (creditCard == null) throw new ArgumentNullException("Nenhum cartão foi enviado para ser registrado");

            var crd = _creditCardService.Create(creditCard);
            customer.CtcCrds.Add(crd);

            return _customerRepository.Update(customer);
        }
    }
}
