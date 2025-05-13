using E_CommerceLivraria.DTO.ExchangesDTO;
using E_CommerceLivraria.DTO.PaymentDTO;
using E_CommerceLivraria.Enums;
using E_CommerceLivraria.Models;
using E_CommerceLivraria.Models.ModelsStructGroups.PaymentSG;
using E_CommerceLivraria.Repository.PurchaseR;
using static E_CommerceLivraria.DTO.PaymentDTO.FinishPurchaseRequestDTO;

namespace E_CommerceLivraria.Services.PurchaseS
{
    public class PurchaseService : IPurchaseService
    {
        private IPurchaseRepository _purchaseRepository;
        private IPurchaseItemService _purchaseItemService;

        public PurchaseService(IPurchaseRepository purchaseRepository, IPurchaseItemService purchaseItemService)
        {
            _purchaseRepository = purchaseRepository;
            _purchaseItemService = purchaseItemService;
        }

        public Purchase Add(PurchaseDataDTO purchaseData)
        {
            decimal valuePayed = purchaseData.Request.CreditCards.Sum(x => x.value);

            if (valuePayed < purchaseData.Request.FinalPrice)
                throw new Exception("Saldo insuficiente para realizar pagamento");

            var purchase = new Purchase();

            foreach (CreditCardPaymentData data in purchaseData.Request.CreditCards)
            {
                var ccp = new CreditCardsPurchase
                {
                    CcpCrdId = data.id,
                    CcpAmount = data.value
                };
                purchase.CreditCards.Add(ccp);
            }

            foreach (CartItem item in purchaseData.Customer.Cart.CartItems)
            {
                var purchaseItem = new PurchaseItem()
                {
                    PciStc = item.CriStc,
                    PciQuantity = item.CriQuantity,
                    PciTotalPrice = item.CriTotalprice,
                    PciStatus = (decimal)EStatus.EM_PROCESSAMENTO
                };

                purchase.PurchaseItems.Add(purchaseItem);
            }

            purchase.PrcDate = DateTime.Now;
            purchase.PrcCtm = purchaseData.Customer;
            purchase.PrcAdd = purchaseData.DeliveryAddress;

            return _purchaseRepository.Add(purchase);
        }

        public Purchase AddExchange(ExchangeRequestDTO exchangeData)
        {
            foreach (PurchaseItem itemToExc in exchangeData.ItemsToExchange)
            {
                var currentItem = _purchaseItemService.Get(itemToExc.PciStcId, exchangeData.PrcId, EStatus.ENTREGUE);

                if (currentItem == null) continue;

                if (itemToExc.PciQuantity > currentItem.PciQuantity) throw new Exception("A quantidade de itens solicitadas para troca excede a quantidade comprada.");

                itemToExc.PciStc = currentItem.PciStc;
                itemToExc.PciStatus = (decimal)EStatus.TROCA_SOLICITADA;
                itemToExc.PciTotalPrice = itemToExc.PciStc.StcSalePrice * itemToExc.PciQuantity;

                if (itemToExc.PciQuantity < currentItem.PciQuantity)
                {
                    currentItem.PciQuantity -= itemToExc.PciQuantity;
                    currentItem.PciTotalPrice = currentItem.PciStc.StcSalePrice * currentItem.PciQuantity;
                    _purchaseItemService.Update(currentItem);
                } else
                {
                    _purchaseItemService.Delete(currentItem);
                }

                _purchaseItemService.Add(itemToExc);
            }

            return Get(exchangeData.PrcId);
        }

        public bool Delete(decimal id)
        {
            return _purchaseRepository.Delete(id);
        }

        public Purchase? Get(decimal id)
        {
            return _purchaseRepository.Get(id);
        }

        public List<Purchase> GetAll()
        {
            return _purchaseRepository.GetAll();
        }

        public Purchase Update(Purchase purchase)
        {
            return _purchaseRepository.Update(purchase);
        }

        public Purchase UpdateStatus(Purchase purchase, EStatus newStatus)
        {
            if ((EStatus)purchase.PrcStatus == EStatus.COMPRA_REPROVADA) throw new Exception("Essa compra já foi recusada");

            if ((EStatus)purchase.PrcStatus == EStatus.TROCA_REPROVADA) throw new Exception("Esse pedido de troca já foi reprovado");

            foreach (PurchaseItem item in purchase.PurchaseItems)
            {
                _purchaseItemService.UpdateStatus(item, newStatus);
            }

            return _purchaseRepository.Get(purchase.PrcId);
        }
    }
}
