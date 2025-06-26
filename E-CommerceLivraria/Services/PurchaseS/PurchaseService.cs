using E_CommerceLivraria.DTO.AnalysisDTO;
using E_CommerceLivraria.DTO.ExchangesDTO;
using E_CommerceLivraria.DTO.PaymentDTO;
using E_CommerceLivraria.Enums;
using E_CommerceLivraria.Models;
using E_CommerceLivraria.Models.ModelsStructGroups.PaymentSG;
using E_CommerceLivraria.Repository.PurchaseR;
using E_CommerceLivraria.Services.CouponS;
using E_CommerceLivraria.Services.CustomerS;
using E_CommerceLivraria.Services.StockS;
using static E_CommerceLivraria.DTO.PaymentDTO.FinishPurchaseRequestDTO;

namespace E_CommerceLivraria.Services.PurchaseS
{
    public class PurchaseService : IPurchaseService
    {
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IStockService _stockService;
        private readonly IPurchaseItemService _purchaseItemService;
        private readonly IExchangeCouponService _exchangeCouponService;

        public PurchaseService(IPurchaseRepository purchaseRepository, IPurchaseItemService purchaseItemService, IStockService stockService, IExchangeCouponService exchangeCouponService)
        {
            _purchaseRepository = purchaseRepository;
            _purchaseItemService = purchaseItemService;
            _stockService = stockService;
            _exchangeCouponService = exchangeCouponService;
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
            purchase.PrcAdd = purchaseData.DeliveryAddress;

            purchaseData.Customer.Cart.CartItems.Clear();
            purchase.PrcCtm = purchaseData.Customer;

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

        public List<DataSalesDTO> GetSalesByCategories(DateTime start, DateTime end)
        {
            var allSales = _purchaseRepository.GetAll();
            if (allSales.Count < 0) return new List<DataSalesDTO>();

            var datas = new List<DataSalesDTO>();

            foreach (var sale in allSales)
            {
                if (sale.PrcDate.Year < start.Year || sale.PrcDate.Year > end.Year)
                    continue;

                if (sale.PrcDate.Month < start.Month || sale.PrcDate.Month > end.Month)
                    continue;

                foreach (var item in sale.PurchaseItems)
                {
                    if (!(item.PciStatus > (int)EStatus.EM_PROCESSAMENTO && item.PciStatus < (int)EStatus.TROCA_SOLICITADA))
                        continue;

                    foreach (Category category in item.PciStc.StcBok.BcrBcts)
                    {
                        int index = datas.FindIndex(x => x.Name == category.BctName);
                        if (index == -1)
                        {
                            datas.Add(new DataSalesDTO { Name = category.BctName });
                            index = datas.Count() - 1;
                        }

                        int timeIndex = datas[index].GetYearDate(sale.PrcDate);
                        if (timeIndex == -1)
                        {
                            DateTime newTime = new DateTime(sale.PrcDate.Year, sale.PrcDate.Month, 1);

                            datas[index].MonthSales.Add(new MonthSales() { Time = newTime });
                            timeIndex = datas[index].MonthSales.Count() - 1;
                        }

                        datas[index].MonthSales[timeIndex].TotalSales += item.PciQuantity;
                        datas[index].MonthSales[timeIndex].TotalProfit += item.PciTotalPrice;
                    }
                }
            }

            return datas;
        }

        public List<DataSalesDTO> GetSalesByProduct(DateTime start, DateTime end)
        {
            var allSales = _purchaseRepository.GetAll();
            if (allSales.Count() < 0) return new List<DataSalesDTO>();

            var datas = new List<DataSalesDTO>();

            foreach (var sale in allSales)
            {
                if (sale.PrcDate.Year < start.Year || sale.PrcDate.Year > end.Year)
                    continue;

                if (sale.PrcDate.Month < start.Month || sale.PrcDate.Month > end.Month)
                    continue;

                foreach (var item in sale.PurchaseItems)
                {
                    if (!(item.PciStatus > (int)EStatus.EM_PROCESSAMENTO && item.PciStatus < (int)EStatus.TROCA_SOLICITADA))
                        continue;

                    int index = datas.FindIndex(x => x.Id == item.PciStcId);
                    if (index == -1)
                    {
                        datas.Add(new DataSalesDTO { Id = item.PciStcId, Name = item.PciStc.StcBok.BokTitle });
                        index = datas.Count() - 1;
                        datas[index].MonthSales.Add(new MonthSales()
                        {
                            Time = new DateTime(2000, 1, 1)
                        });
                    }

                    datas[index].MonthSales[0].TotalSales += item.PciQuantity;
                    datas[index].MonthSales[0].TotalProfit += item.PciTotalPrice;
                }
            }

            return datas;
        }

        public Purchase Update(Purchase purchase)
        {
            return _purchaseRepository.Update(purchase);
        }

        public Purchase UpdateExchangeStatus(Purchase purchase, EStatus newStatus, bool returnStock)
        {
            if ((EStatus)purchase.PrcStatusExchange == null) throw new Exception("Não há itens para troca");

            if ((EStatus)purchase.PrcStatusExchange == EStatus.TROCA_REPROVADA) throw new Exception("Esse pedido de troca já foi recusado");

            var tempList = purchase.PurchaseItems.Where(x => x.PciStatus > (int)EStatus.ENTREGUE && (x.PciStatus < (int)newStatus || newStatus == EStatus.TROCA_REPROVADA)).ToList();
            if (tempList.Count < 1) throw new Exception("Nenhum item encontrado");

            foreach (var item in tempList)
            {
                if (returnStock && newStatus == EStatus.EM_TROCA)
                {
                    var stock = _stockService.Get(item.PciStcId);
                    _stockService.AddToStock(stock, item.PciQuantity);
                }

                _purchaseItemService.UpdateStatus(item, newStatus);
            }

            if (newStatus == EStatus.EM_TROCA)
            {
                var totalValue = tempList.Sum(x => x.PciTotalPrice);
                _exchangeCouponService.AddToCtm(purchase.PrcCtm, totalValue);
            }

            return purchase;
        }

        public Purchase UpdatePurchaseStatus(Purchase purchase, EStatus newStatus)
        {
            if ((EStatus)purchase.PrcStatus == EStatus.COMPRA_REPROVADA) throw new Exception("Essa compra já foi recusada");

            if ((EStatus)purchase.PrcStatus >= EStatus.TROCA_SOLICITADA) throw new Exception("Isso não é um pedido de compra");

            var tempList = purchase.PurchaseItems.Where(x => (x.PciStatus >= (int)EStatus.EM_PROCESSAMENTO && x.PciStatus <= (int)EStatus.ENTREGUE) && (x.PciStatus < (int)newStatus || newStatus == EStatus.COMPRA_REPROVADA)).ToList();
            if (tempList.Count < 1) throw new Exception("Nenhum item encontrado");

            foreach (PurchaseItem item in tempList)
            {
                _purchaseItemService.UpdateStatus(item, newStatus);
            }

            return _purchaseRepository.Get(purchase.PrcId);
        }
    }
}
