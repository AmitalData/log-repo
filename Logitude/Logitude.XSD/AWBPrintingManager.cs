using Logitude.BL.Security;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Logitude.XSD
{
    public class AWBPrintingManager
    {
        private AWBPrintResult myResult;
        private Shipment myShipment;
        private ShipmentMasterData masterData;
        private IShipmentsContext shipmentsContext;
        ICommonDataContext myCommonContext;
        public AWBPrintResult GetPrintingResult(string myShipmentId, bool isCargonautSending, bool isDEXXSending, bool isConfirmedByUser, int myTenant)
        {
            this.myResult = new AWBPrintResult()
            {
                Id = myTenant,
                Tenant = myTenant,
                ShipmentId = myShipmentId,
                IsDEXXSending = isDEXXSending,
                IsCargonautSending = isCargonautSending,
                IsConfirmedByUser = isConfirmedByUser,
            };

            this.GetLoggedContact();
            this.GetStockTypeCodes();
            this.GetGlobalVariables();
            this.GetShipmentObjects();
            this.GetStockData();

            return myResult;
        }

        private void GetLoggedContact()
        {
            myCommonContext = CommonDataContext.GetContext(myResult.Tenant);
            ContactRepository contactRepository = new ContactRepository(myCommonContext);
            Simplog.Data.CommonDataModel.EntityPOCOs.Contact loggedContact = contactRepository.GetSingleContactByEmail(SecurityUtility.GetAuthenticatedUser(), myResult.Tenant);
            myResult.LoggedContactId = loggedContact.Id;
        }
        private void GetStockTypeCodes()
        {
            string myFHLCode = "FHL";
            string myFWBCode = "FWB";

            if (myResult.IsDEXXSending)
            {
                myFHLCode = "FHL DEXX";
                myFWBCode = "FWB DEXX";
            }

            else if (myResult.IsCargonautSending)
            {
                myFHLCode = "FHL Cargonaut";
                myFWBCode = "FWB Cargonaut";
            }

            myResult.StockFHLCode = myFHLCode;
            myResult.StockFWBCode = myFWBCode;
        }
        private void GetGlobalVariables()
        {
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                TenantManagement tenantManagement = tenantManagementRepository.GetSingleTenantManagement(myResult.Tenant);

                if (tenantManagement != null)
                {
                    myResult.IsAWBStockPrepaid = tenantManagement.IsAWBStockPrepaid;

                    if (myResult.Tenant == 65 || tenantManagement.IsEAWBOnlyDemo)
                    {
                        myResult.IsDemoTenant = true;
                    }
                }

                scope.Complete();
            }
        }
        private void GetShipmentObjects()
        {
            shipmentsContext = ShipmentsContext.GetContext(myResult.Tenant);
            ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentsContext);

            myShipment = shipmentRepository.GetSingleShipment(myResult.ShipmentId, myResult.Tenant);

            if (myShipment != null)
            {
                ShipmentMasterDataRepository shipmentMasterDataRepository = new ShipmentMasterDataRepository(shipmentsContext);
                masterData = shipmentMasterDataRepository.GetSingleMasterData(myShipment.MasterShipmentDataId);
            }
        }
        private void GetStockData()
        {
            if (!myResult.IsAWBStockPrepaid)
            {
                myResult.IsPrintingAllowed = true;
            }

            else if (myResult.IsDemoTenant)
            {
                myResult.IsPrintingAllowed = true;
            }

            else
            {
                MessagingStockUsageHistoryRepository usageHistoryRepository = new MessagingStockUsageHistoryRepository(shipmentsContext);
                IQueryable<MessagingStockUsageHistory> myUsageHistoryData = usageHistoryRepository.GetTenantMessagingStockUsageHistory(myResult.Tenant);

                int sendingCount = 0;
                if (myShipment.ShipmentLevelCode == "H")
                {
                    MessagingStockUsageHistory usageHistory = myUsageHistoryData.Where(d => d.EntityId == myResult.ShipmentId && d.MessageType == myResult.StockFHLCode).FirstOrDefault();
                    if (usageHistory != null)
                    {
                        myResult.IsPrintingAllowed = true;
                        myResult.IsStockAlreadyTaken = true;
                        usageHistory.ActionType = "Printing";
                        usageHistory.LastActionDate = TenantServerConfigration.GetCurrentDateTime(myResult.Tenant);
                        usageHistory.LastActionByUserId = myResult.LoggedContactId;
                        usageHistoryRepository.Update(usageHistory);
                        usageHistoryRepository.SubmitChanges();
                    }

                    else
                    {
                        sendingCount = 1;
                    }
                }

                else
                {
                    MessagingStockUsageHistory usageHistory = myUsageHistoryData.Where(d => d.EntityId == myResult.ShipmentId && d.MessageType == myResult.StockFWBCode).FirstOrDefault();
                    if (usageHistory != null)
                    {
                        myResult.IsPrintingAllowed = true;
                        myResult.IsStockAlreadyTaken = true;
                        usageHistory.ActionType = "Printing";
                        usageHistory.LastActionDate = TenantServerConfigration.GetCurrentDateTime(myResult.Tenant);
                        usageHistory.LastActionByUserId = myResult.LoggedContactId;
                        usageHistoryRepository.Update(usageHistory);
                        usageHistoryRepository.SubmitChanges();
                    }

                    else
                    {
                        sendingCount = 1;
                    }
                }

                if (sendingCount > 0)
                {
                    if (myResult.IsConfirmedByUser)
                    {
                        DateTime todayDateTime = TenantServerConfigration.GetCurrentDateTime(myResult.Tenant);
                        DateTime todayDate = todayDateTime.Date;
                        MessagingStockRepository stockRepository = new MessagingStockRepository(shipmentsContext);
                        IQueryable<MessagingStock> myStocksData = stockRepository.GetMessagingStocksByTenant(myResult.Tenant, "Champ");

                        myStocksData = myStocksData.Where(d => d.StartDate <= todayDate && d.EndDate > todayDate && d.Remaining > 0 && !d.IsCancelled);
                        if (myStocksData.Count() > 0)
                        {
                            myResult.StockRemainingBefore = myStocksData.Sum(s => s.Remaining);
                        }

                        if (myResult.StockRemainingBefore == 0)
                        {
                            myResult.IsPrintingAllowed = false;
                            myResult.IsNoRemainingStocks = true;
                        }

                        else
                        {
                            if (myResult.StockRemainingBefore > sendingCount)
                            {
                                myResult.IsPrintingAllowed = true;

                                string myMessageType = myResult.StockFWBCode;

                                if (myShipment.ShipmentLevelCode == "H")
                                {
                                    myMessageType = myResult.StockFHLCode;
                                }

                                MessagingStock myStock = myStocksData.OrderBy(o => o.EndDate).FirstOrDefault();

                                MessagingStockUsageHistory usageHistory = new MessagingStockUsageHistory()
                                {
                                    Id = IdCounter.GetNumber("MessagingStockUsageHistory", myResult.Tenant),
                                    Tenant = myResult.Tenant,
                                    StockId = myStock.Id,
                                    EntityId = myShipment.Id,
                                    EntityNumber = myShipment.ShipmentNumber,
                                    MessageType = myMessageType,
                                    MAWB = masterData.Master,
                                    HAWB = myShipment.House,
                                    ActionType = "Printing",
                                    FirstActionByUserId = myResult.LoggedContactId,
                                    FirstActionDate = todayDateTime,
                                    LastActionByUserId = myResult.LoggedContactId,
                                    LastActionDate = todayDateTime,
                                };

                                if (!string.IsNullOrEmpty(masterData.MainCarriageCarrierId))
                                {
                                    AirlineRepository airlineRepository = new AirlineRepository(myCommonContext);
                                    Airline airline = airlineRepository.GetSingleAirline(masterData.MainCarriageCarrierId, myResult.Tenant);
                                    if (airline != null)
                                    {
                                        if (!string.IsNullOrEmpty(airline.Prefix) && !string.IsNullOrEmpty(masterData.Master))
                                        {
                                            usageHistory.MAWB = airline.Prefix + "-" + masterData.Master;
                                        }
                                    }
                                }

                                usageHistoryRepository.Add(usageHistory);
                                usageHistoryRepository.SubmitChanges();

                                int myStockUsageCount = usageHistoryRepository.GetStockUsageCount(myStock.Id, myStock.TenantNumber);
                                myStock.Remaining = myStock.Amount - myStockUsageCount;
                                stockRepository.Update(myStock);
                                stockRepository.SubmitChanges();
                            }
                        }

                        int? myStocksRemaining = 0;
                        if (myStocksData.Count() > 0)
                        {
                            myStocksRemaining = myStocksData.Sum(s => s.Remaining);
                        }

                        myResult.StockRemainingAfter = myStocksRemaining == null ? 0 : myStocksRemaining.Value;
                    }
                }
            }
        }
    }

    public class AWBPrintResult
    {
        [Key]
        public int Id { get; set; }
        public int Tenant { get; set; }
        public string ShipmentId { get; set; }
        public bool IsDEXXSending { get; set; }
        public bool IsCargonautSending { get; set; }
        public string LoggedContactId { get; set; }
        public string StockFHLCode { get; set; }
        public string StockFWBCode { get; set; }
        public int? StockRemainingBefore { get; set; }
        public int? StockRemainingAfter { get; set; }
        public bool IsDemoTenant { get; set; }
        public bool IsAWBStockPrepaid { get; set; }
        public bool IsConfirmedByUser { get; set; }
        public bool IsPrintingAllowed { get; set; }
        public bool IsStockAlreadyTaken { get; set; }
        public bool IsNoRemainingStocks { get; set; }
    }
}
