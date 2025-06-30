using E_CommerceLivraria.DTO.AdmCustomerDTO;
using E_CommerceLivraria.DTO.ChatbotDTO;
using E_CommerceLivraria.DTO.ProfileDTO.InfoDTO;
using E_CommerceLivraria.Enums;
using E_CommerceLivraria.Models;
using E_CommerceLivraria.Repository.CustomerR;
using E_CommerceLivraria.Repository.CustomerR.GenderR;
using E_CommerceLivraria.Services.AddressS;
using E_CommerceLivraria.Services.CustomerS.TelephoneS;

namespace E_CommerceLivraria.Services.CustomerS {
    public class CustomerService : ICustomerService{
        private readonly ICustomerRepository _customerRepository;
        private readonly ICartService _cartService;
        private readonly IAddressService _addressService;
        private readonly IGenderRepository _genderRepository;
        private readonly ITelephoneService _telephoneService;

        public CustomerService(ICustomerRepository customerRepository, ICartService cartService, IAddressService addressService, IGenderRepository genderRepository, ITelephoneService telephoneService) {
            _customerRepository = customerRepository;
            _cartService = cartService;
            _addressService = addressService;
            _genderRepository = genderRepository;
            _telephoneService = telephoneService;
        }

        public Customer Create(CreateCustomerDTO createData) {
            Customer newCtm = createData.Ctm;
            newCtm.CtmCrtId = null;

            newCtm.CtmAdd = _addressService.Create(newCtm.CtmAdd);
            newCtm.DadAdds.Add(_addressService.Create(createData.Delivery));
            newCtm.BadAdds.Add(_addressService.Create(createData.Billing));

            Gender? tempGnd = _genderRepository.Get(newCtm.CtmGndId);
            if (tempGnd == null) throw new Exception("O gênero não foi encontrado");
            else newCtm.CtmGnd = tempGnd;

            newCtm.CtmTlp = _telephoneService.Create(newCtm.CtmTlp);
            newCtm.CtmBirthdate = DateTime.Parse(createData.Birthdate);

            newCtm = _customerRepository.Add(newCtm);

            newCtm.CtmCrt = _cartService.Create(newCtm);
            newCtm.CtmCrtId = newCtm.CtmCrt.CrtId;

            return _customerRepository.Update(newCtm);
        }

        public bool Exists(decimal id) {
            var ctm = _customerRepository.Get(id);

            return (ctm != null);
        }

        public Customer? Get(decimal id) {
            return _customerRepository.Get(id);
        }

        public List<Customer> GetAll() {
            return _customerRepository.GetAll();
        }

        public Customer Deactivate(Customer ctm)
        {
            if (!ctm.CtmActive) return ctm;

            if (ctm.CtmCrt != null) _cartService.ClearCart(_cartService.Get((decimal)ctm.CtmCrtId));
            ctm.CtmActive = false;

            return _customerRepository.Update(ctm);
        }

        public Customer Activate(Customer ctm)
        {
            if (ctm.CtmActive) return ctm;

            ctm.CtmActive = true;

            return _customerRepository.Update(ctm);
        }

        public Customer Update(Customer customer) {
            return _customerRepository.Update(customer);
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

        public Customer RemoveCreditCard(Customer customer, CreditCard creditCard)
        {
            customer.CtcCrds.Remove(creditCard);
            return _customerRepository.Update(customer);
        }
    }
}