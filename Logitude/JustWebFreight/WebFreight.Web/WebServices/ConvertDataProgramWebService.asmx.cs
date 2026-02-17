using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Services;

using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;

using WebFreight.Web.DataContracts;
using WebFreight.Web.GlobalModel;
using WebFreight.Web.Helpers;
using Logitude.BL.InfrastructureModel.EntityPMs;
using WebFreight.Web.ShipmentsModel;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.DataContracts;
using Simplog.Global.Data.GlobalModel.Repositories;
using Logitude.BL.ShipmentsModel;
using Logitude.BL.Helpers;

namespace WebFreight.Web.WebServices
{
    /// <summary>
    /// Summary description for ConvertDataProgramWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ConvertDataProgramWebService : System.Web.Services.WebService
    {


        [WebMethod]
        public List<int> GetTenantIds(int fromTenant,int toTenant)
        {
            //TenantRepository tenantRep = new TenantRepository();
            //List<Tenant> TenantList = tenantRep.GetTenants().ToList();
            //List<int> tenantIds = (from a in TenantList
            //                       where a.Version != -1 && a.Id >= fromTenant && a.Id <= toTenant
            //                       select a.Id).ToList();
            //return tenantIds;

            List<int> tenantIds = new List<int>();
            TenantRepository tenantRep;

            for (int i = 0; i <= toTenant; i++)
            {
                tenantRep = new TenantRepository(i);
                tenantIds.Add(i);
            }

                return tenantIds;
        }
        
        [WebMethod]
        public int GetShipmentsCount(int tenant)
        {
            ShipmentRepository shipmentRep = new ShipmentRepository(tenant);
            int count = shipmentRep.GetShipmentsCount(tenant);
            return count;
        }

        [WebMethod]
        public string ConvertProgramForShipment(int tenant,int skip,int take)
        {
            string status = null;
            
                CalculateProfitInProfitCurrencyForShipments(tenant, skip, take);
                //CalculateNextETDAndETAAndStatusForShipments(tenant, skip, take);
          
            status = "Success";
            return status;
        }

        [WebMethod]
        public string ConvertUsersInGlobalContact(int tenant)
        {
            UserRepository userRep = new UserRepository(tenant);
            GlobalContactRepository globalContactRep=new GlobalContactRepository();
            List<User> usersList = userRep.GetUsers(tenant).ToList();
            List<GlobalContact> globalContacts = globalContactRep.GetGlobalContactByTenant(tenant).ToList();

            foreach (User user in usersList)
            {
                GlobalContact contact = (from a in globalContacts
                                         where a.Id == user.Id
                                         select a).FirstOrDefault();
                contact.IsUser = true;
                globalContactRep.Update(contact);
            }
            globalContactRep.SubmitChanges();
            return "success";

        }

        public void CalculateProfitInProfitCurrencyForShipments(int tenant, int skip, int take)
        {
            IShipmentsContext context = ShipmentsContext.GetContext(tenant);
            ShipmentRepository shipmentRep = new ShipmentRepository(context);
            ShipmentReceivableRepository shipmentReceivableRep = new ShipmentReceivableRepository(context);
            ShipmentPayableRepository shipmentPayableRep = new ShipmentPayableRepository(context);

            List<Shipment> shipments = shipmentRep.GetSpecificAmountOfShipmentsByTenant(tenant, skip, take).ToList();
            RatesTableRepository ratesTablesRepository = new RatesTableRepository(tenant);
            RatesTableQuery ratesTableQuery = new RatesTableQuery(ratesTablesRepository);
            //TenantRepository TenantRep = new TenantRepository(tenant);
            ARInvoiceRepository arInvoiceRep = new ARInvoiceRepository(tenant);            
            Tenant currentTenant = TenantRepository.GetSingleTenant(tenant, true);
            ARInvoiceLineRepository arInvoicelineRep = new ARInvoiceLineRepository(tenant);
            ARInvoiceTotalVATRepository arInvoiceTotalvatRep = new ARInvoiceTotalVATRepository(tenant);
            foreach (Shipment shipment in shipments)
            {
                if (string.IsNullOrEmpty(shipment.ProfitCurrencyId))
                {
                    shipment.ProfitCurrencyId = currentTenant.ProfitCurrencyId;
                }

                List<ShipmentReceivable> receivables = shipmentReceivableRep.GetShipmentReceivablesByShipmentId(shipment.Id, tenant);
                List<ShipmentPayable> payables = shipmentPayableRep.GetShipemntPayablesByShipmentId(shipment.Id, tenant);
                List<ARInvoice> arInvoices = arInvoiceRep.GetInvoicesByMainEntityId(shipment.Id, shipment.Tenant);

                int shipmentChanges = 0;

                #region Update Receivables
                foreach (ShipmentReceivable item in receivables)
                {
                    double? itemCurrencyRate = null;
                    if (item.CurrencyId == currentTenant.CurrencyId)
                    {
                        itemCurrencyRate = 1;
                    }

                    else
                    {
                        LastRate lastRate = ratesTableQuery.GetLastRecordByValueDate(tenant, item.CurrencyId, currentTenant.CurrencyId, item.CreateDate);
                        if (lastRate != null)
                        {
                            itemCurrencyRate = lastRate.Rate;
                        }
                    }

                    double? itemCurrencyRateRounded = MethodHelper.Round(itemCurrencyRate, 5);
                    if (item.Rate != itemCurrencyRateRounded)
                    {
                        item.Rate = itemCurrencyRateRounded;
                        shipmentChanges++;
                    }


                    double? amountLocal = item.TotalAmount * item.Rate;
                    double? amountLocalRounded = amountLocal == null ? 0 : MethodHelper.Round(amountLocal, 2);
                    if (item.TotalAmountLocal != amountLocalRounded)
                    {
                        item.TotalAmountLocal = amountLocalRounded;
                        shipmentChanges++;
                    }

                    double? profitRate = null;
                    if (shipment.ProfitCurrencyId == currentTenant.CurrencyId)
                    {
                        profitRate = 1;
                    }

                    else
                    {
                        LastRate lastRate = ratesTableQuery.GetLastRecordByValueDate(tenant, shipment.ProfitCurrencyId, currentTenant.CurrencyId, item.CreateDate);
                        if (lastRate != null)
                        {
                            profitRate = lastRate.Rate;
                        }
                    }

                    double? profitRateRounded = MethodHelper.Round(profitRate, 5);
                    if (item.ProfitCurrencyExchangeRate != profitRateRounded)
                    {
                        item.ProfitCurrencyExchangeRate = profitRateRounded;
                        shipmentChanges++;
                    }


                    double? amountProfit = item.TotalAmountLocal / item.ProfitCurrencyExchangeRate;
                    double? amountProfitRounded = amountProfit == null ? 0 : MethodHelper.Round(amountProfit, 2);
                    if (item.AmountInProfitCurrency != amountProfitRounded)
                    {
                        item.AmountInProfitCurrency = amountProfitRounded;
                        shipmentChanges++;
                    }

                    shipmentReceivableRep.Update(item);
                }
                #endregion

                #region Update Payables
                foreach (ShipmentPayable item in payables)
                {

                    if (item.UnitPrice == null || item.Quantity == null)
                    {
                        item.ExpectedAmount = 0;
                        item.AccountedAmount = 0;
                        item.OpenAmount = 0;
                    }

                    else
                    {
                        double? expectedAmount = item.UnitPrice * item.Quantity;
                        double? expectedAmountRounded = expectedAmount == null ? 0 : MethodHelper.Round(expectedAmount, 2);
                        if (item.ExpectedAmount != expectedAmountRounded)
                        {
                            item.ExpectedAmount = expectedAmountRounded;
                        }

                        if (item.ShipmentPayableLineStatusCode == "ACCT")
                        {
                            item.OpenAmount = 0;
                            item.AccountedAmount = expectedAmountRounded;
                        }

                        else
                        {
                            item.OpenAmount = expectedAmountRounded;
                            item.AccountedAmount = 0;
                        }
                    }

                    double? itemCurrencyRate = null;
                    if (item.CurrencyId == currentTenant.CurrencyId)
                    {
                        itemCurrencyRate = 1;
                    }

                    else
                    {
                        LastRate lastRate = ratesTableQuery.GetLastRecordByValueDate(tenant, item.CurrencyId, currentTenant.CurrencyId, item.CreateDate);
                        if (lastRate != null)
                        {
                            itemCurrencyRate = lastRate.Rate;
                        }
                    }

                    double? itemCurrencyRateRounded = MethodHelper.Round(itemCurrencyRate, 5);
                    if (item.Rate != itemCurrencyRateRounded)
                    {
                        item.Rate = itemCurrencyRateRounded;
                        shipmentChanges++;
                    }


                    if (item.ExpectedAmount == null) { item.ExpectedAmount = 0; }
                    if (item.AccountedAmount == null) { item.AccountedAmount = 0; }
                    if (item.OpenAmount == null) { item.OpenAmount = 0; }

                    double? expectedLocal = item.ExpectedAmount * item.Rate;
                    double? accountedLocal = item.AccountedAmount * item.Rate;
                    double? openLocal = item.OpenAmount * item.Rate;

                    double? expectedLocalRounded = expectedLocal == null ? 0 : MethodHelper.Round(expectedLocal, 2);
                    double? accountedLocalRounded = accountedLocal == null ? 0 : MethodHelper.Round(accountedLocal, 2);
                    double? openLocalRounded = openLocal == null ? 0 : MethodHelper.Round(openLocal, 2);

                    if (item.ExpectedAmountLocal != expectedLocalRounded)
                    {
                        item.ExpectedAmountLocal = expectedLocalRounded;
                        shipmentChanges++;
                    }

                    if (item.AccountedAmountInLocalCurrency != accountedLocalRounded)
                    {
                        item.AccountedAmountInLocalCurrency = accountedLocalRounded;
                        shipmentChanges++;
                    }

                    if (item.OpenAmountInLocalCurrency != openLocalRounded)
                    {
                        item.OpenAmountInLocalCurrency = openLocalRounded;
                        shipmentChanges++;
                    }


                    double? profitRate = null;
                    if (shipment.ProfitCurrencyId == currentTenant.CurrencyId)
                    {
                        profitRate = 1;
                    }

                    else
                    {
                        LastRate lastRate = ratesTableQuery.GetLastRecordByValueDate(tenant, shipment.ProfitCurrencyId, currentTenant.CurrencyId, item.CreateDate);
                        if (lastRate != null)
                        {
                            profitRate = lastRate.Rate;
                        }
                    }

                    double? profitRateRounded = MethodHelper.Round(profitRate, 5);
                    if (item.ProfitCurrencyExchangeRate != profitRateRounded)
                    {
                        item.ProfitCurrencyExchangeRate = profitRateRounded;
                        shipmentChanges++;
                    }

                    double? expectedProfit = item.ExpectedAmountLocal / item.ProfitCurrencyExchangeRate;
                    double? accountedProfit = item.AccountedAmountInLocalCurrency / item.ProfitCurrencyExchangeRate;
                    double? openProfit = item.OpenAmountInLocalCurrency / item.ProfitCurrencyExchangeRate;

                    double? expectedProfitRounded = MethodHelper.Round(expectedProfit, 2);
                    double? accountedProfitRounded = MethodHelper.Round(accountedProfit, 2);
                    double? openProfitRounded = MethodHelper.Round(openProfit, 2);

                    if (item.ExpectedAmountInProfitCurrency != expectedProfitRounded)
                    {
                        item.ExpectedAmountInProfitCurrency = expectedProfitRounded;
                        shipmentChanges++;
                    }

                    if (item.AccountedAmountInProfitCurrency != accountedProfitRounded)
                    {
                        item.AccountedAmountInProfitCurrency = accountedProfitRounded;
                        shipmentChanges++;
                    }

                    if (item.OpenAmountInProfitCurrency != openProfitRounded)
                    {
                        item.OpenAmountInProfitCurrency = openProfitRounded;
                        shipmentChanges++;
                    }


                    if (string.IsNullOrEmpty(item.ShipmentPayableAmountTypeCode))
                    {
                        item.ShipmentPayableAmountTypeCode = "ACCU";
                    }

                    if (string.IsNullOrEmpty(item.ShipmentPayableLineStatusCode) || item.ShipmentPayableLineStatusCode == "EMPT")
                    {
                        if (item.UnitPrice != null && item.Quantity != null)
                        {
                            item.ShipmentPayableLineStatusCode = "OAMT";
                        }
                    }

                    shipmentPayableRep.Update(item);
                }
                #endregion

                #region Update Shipment
                //ShipmentProfitUtility.CalculateShipmentProfit(shipment, Payables, Receivables);
                shipmentRep.Update(shipment);
                #endregion

                int invoiceChanges = 0;
                foreach (ARInvoice item in arInvoices)
                {
                    #region Update Invoice
                    if (string.IsNullOrEmpty(item.ProfitCurrencyId))
                    {
                        item.ProfitCurrencyId = shipment.ProfitCurrencyId;
                        invoiceChanges++;
                    }

                    double? itemCurrencyRate = null;
                    if (item.InvoiceCurrencyId == item.LocalCurrencyId)
                    {
                        itemCurrencyRate = 1;
                    }

                    else
                    {
                        LastRate lastRate = ratesTableQuery.GetLastRecordByValueDate(tenant, item.InvoiceCurrencyId, item.LocalCurrencyId, item.CreateDate);
                        if (lastRate != null)
                        {
                            itemCurrencyRate = lastRate.Rate;
                        }
                    }

                    double? itemCurrencyRateRounded = MethodHelper.Round(itemCurrencyRate, 5);
                    if (item.InvoiceCurrencyExchangeRate != itemCurrencyRateRounded)
                    {
                        item.InvoiceCurrencyExchangeRate = itemCurrencyRateRounded;
                        invoiceChanges++;
                    }

                    if (item.AmountInInvoiceCurrency == null) { item.AmountInInvoiceCurrency = 0; }
                    if (item.AmountDue == null) { item.AmountInInvoiceCurrency = 0; }
                    if (item.SubTotalInInvoiceCurrency == null) { item.AmountInInvoiceCurrency = 0; }

                    double? invoiceAmountLocal = item.AmountInInvoiceCurrency * item.InvoiceCurrencyExchangeRate;
                    double? amountDueLocal = item.AmountDue * item.InvoiceCurrencyExchangeRate;
                    double? subTotalLocal = item.SubTotalInInvoiceCurrency * item.InvoiceCurrencyExchangeRate;

                    double? invoiceAmountLocalRounded = MethodHelper.Round(invoiceAmountLocal, 2);
                    double? amountDueLocalRounded = MethodHelper.Round(amountDueLocal, 2);
                    double? subTotalLocalRounded = MethodHelper.Round(subTotalLocal, 2);

                    if (item.AmountInLocalCurrency != invoiceAmountLocalRounded)
                    {
                        item.AmountInLocalCurrency = invoiceAmountLocalRounded;
                        invoiceChanges++;
                    }

                    if (item.AmountDueInLocalCurrency != amountDueLocalRounded)
                    {
                        item.AmountDueInLocalCurrency = amountDueLocalRounded;
                        invoiceChanges++;
                    }

                    if (item.SubTotalInLocalCurrency != subTotalLocalRounded)
                    {
                        item.SubTotalInLocalCurrency = subTotalLocalRounded;
                        invoiceChanges++;
                    }


                    double? profitRate = null;
                    if (shipment.ProfitCurrencyId == item.LocalCurrencyId)
                    {
                        profitRate = 1;
                    }

                    else
                    {
                        LastRate lastRate = ratesTableQuery.GetLastRecordByValueDate(tenant, shipment.ProfitCurrencyId, item.LocalCurrencyId, item.CreateDate);
                        if (lastRate != null)
                        {
                            profitRate = lastRate.Rate;
                        }
                    }

                    double? profitRateRounded = MethodHelper.Round(profitRate, 5);
                    if (item.ProfitCurrencyExchangeRate != profitRateRounded)
                    {
                        item.ProfitCurrencyExchangeRate = profitRateRounded;
                        invoiceChanges++;
                    }

                    double? invoiceAmountProfit = item.AmountInLocalCurrency / item.ProfitCurrencyExchangeRate;
                    double? amountDueProfit = item.AmountDueInLocalCurrency / item.ProfitCurrencyExchangeRate;
                    double? invoiceAmountProfitRounded = MethodHelper.Round(invoiceAmountProfit, 2);
                    double? amountDueProfitRounded = MethodHelper.Round(amountDueProfit, 2);

                    if (item.AmountInProfitCurrency != invoiceAmountProfitRounded)
                    {
                        item.AmountInProfitCurrency = invoiceAmountProfitRounded;
                        invoiceChanges++;
                    }

                    if (item.AmountDueInProfitCurrency != amountDueProfitRounded)
                    {
                        item.AmountDueInProfitCurrency = amountDueProfitRounded;
                        invoiceChanges++;
                    }
                    arInvoiceRep.Update(item);
                    #endregion

                    #region Update Lines
                    List<ARInvoiceLine> invoiceLines = arInvoicelineRep.GetInvoiceLinesByInvoiceId(item.Id, item.Tenant).ToList();
                    foreach (ARInvoiceLine line in invoiceLines)
                    {
                        double? lineProfitAmount = line.LocalCurrencyAmount / item.ProfitCurrencyExchangeRate;
                        double? lineProfitAmountRounded = MethodHelper.Round(lineProfitAmount, 2);
                        if (line.ProfitCurrencyAmount != lineProfitAmountRounded)
                        {
                            line.ProfitCurrencyAmount = lineProfitAmountRounded;
                            invoiceChanges++;
                        }

                        arInvoicelineRep.Update(line);
                    }
                    #endregion

                    #region Update Total Vats
                    List<ARInvoiceTotalVAT> totalVats = arInvoiceTotalvatRep.GetInvoiceTotalVatsForInvoice(item.Id, item.Tenant).ToList();
                    foreach (ARInvoiceTotalVAT vat in totalVats)
                    {
                        double? vatProfitAmount = vat.LocalVATAmount / item.ProfitCurrencyExchangeRate;
                        double? vatableProfitAmount = vat.LocalVatableAmount / item.ProfitCurrencyExchangeRate;

                        double? vatProfitAmountRounded = MethodHelper.Round(vatProfitAmount, 2);
                        double? vatableProfitAmountRounded = MethodHelper.Round(vatableProfitAmount, 2);

                        if (vat.ProfitCurrencyVATAmount != vatProfitAmountRounded)
                        {
                            vat.ProfitCurrencyVATAmount = vatProfitAmountRounded;
                            invoiceChanges++;
                        }

                        if (vat.ProfitVatableAmount != vatableProfitAmountRounded)
                        {
                            vat.ProfitVatableAmount = vatableProfitAmountRounded;
                            invoiceChanges++;
                        }

                        arInvoiceTotalvatRep.Update(vat);
                    }
                    #endregion
                }

            }

            arInvoiceRep.SubmitChanges();
            arInvoicelineRep.SubmitChanges();
            arInvoiceTotalvatRep.SubmitChanges();
            context.SaveChanges();

            foreach (Shipment shipment in shipments)
            {
                UpdateShipmentProfitClass.UpdateProfit(shipment.Id,shipment.Tenant);
            }
        }

        public List<LastRate> GetCurrenciesExchangeRateByValueDate(int tenant, string baseCurrencyId, DateTime? date)
        {
            RatesTableRepository ratesTablesRepository = new RatesTableRepository(tenant);
            RatesTableQuery ratesTableQuery = new RatesTableQuery(ratesTablesRepository);
            List<LastRate> resultList = new List<LastRate>();

            if (string.IsNullOrEmpty(baseCurrencyId))
            {
                string msg = TranslateTextsClass.Translate("General.M.AccountingCurrencyIsNotSet", tenant);
                throw new ApplicationException(msg);
            }

            CurrencyRepository currencyRepository = new CurrencyRepository(tenant);
            Currency baseCurrency = currencyRepository.GetCurrencies(tenant).Where(r => r.Id == baseCurrencyId).FirstOrDefault();
            List<Currency> foreignCurrencies = currencyRepository.GetCurrencies(tenant).Where(c => c.Id != baseCurrencyId).ToList();

            foreach (Currency currency in foreignCurrencies)
            {
                LastRate lastRate = ratesTableQuery.GetLastRecordByValueDate(tenant, currency.Id, baseCurrencyId, date);
                if (lastRate != null)
                {
                    lastRate.BaseCurrencyId = baseCurrencyId;
                    lastRate.BaseCurrencyCode = baseCurrency.Code;
                    resultList.Add(lastRate);
                }

                else
                {
                    LastRate newLastRate = new LastRate()
                    {
                        Id = IdCounter.GetNumber("LastRate", tenant).ToString(),
                        Tenant = tenant,
                        ForeignCurrencyId = currency.Id,
                        ForeignCurrencyCode = currency.Code,
                        ForeignCurrencyName = currency.EnglishName,
                        BaseCurrencyId = baseCurrency.Id,
                        BaseCurrencyCode = baseCurrency.Code,
                        HistoryCount = 0,
                        Rate = null,
                    };

                    resultList.Add(newLastRate);
                }
            }
            return resultList;
        }

        public void CalculateNextETDAndETAAndStatusForShipments(int tenant, int skip, int take)
        {
            IShipmentsContext context = ShipmentsContext.GetContext(tenant);
            ShipmentRepository shipmentRep = new ShipmentRepository(context);
            ShipmentPickUpDeliveryRepository shipemntPickUpDeliveryRep = new ShipmentPickUpDeliveryRepository(context);

            List<Shipment> shipments = shipmentRep.GetSpecificAmountOfShipmentsByTenant(tenant,skip,take).ToList();
            //int shipmentCounter = 0;
            foreach (Shipment shipment in shipments)
            {
                
                CheckNextETAAndETDAndStatus(shipment,shipmentRep);
               
                shipmentRep.Update(shipment);

                //if (shipmentCounter == 20)
                //{
                //    Context.SaveChanges();
                //    shipmentCounter = 0;
                //}
            }
            context.SaveChanges();
        }

        public void CheckNextETAAndETDAndStatus(Shipment shipment, ShipmentRepository shipmentRep)
        {
            ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRep);
            ShipmentPM shipmentPm = shipmentQuery.GetSinglePM(shipment.Id, shipment.Tenant);
            EventTypeRepository eventTypeRep = new EventTypeRepository(shipment.Tenant);
            EventTypeQuery eventTypeQuery = new EventTypeQuery(eventTypeRep);


            ShipmentMasterDataRepository shipmentMasterDataRep = new ShipmentMasterDataRepository(shipment.Tenant);
            ShipmentMasterData masterData = shipmentMasterDataRep.GetSingleMasterData(shipment.MasterShipmentDataId);

            #region ETAAndETD
            List<ShipmentPickUpPM> shipmentPickUps = null;
            List<ShipmentDeliveryPM> shipmentDeliveries = null;
            bool deliveryExists = false;
            bool pickUpExists = false;
            bool deliveryAtaAllExists = true;
            bool deliveryAtdAllExists = true;

            bool continueEta = true;
            bool continueEtd = true;
            bool continueActualCheck = true;
            DateTime? nextDeliveryEta = null;
            DateTime? nextDeliveryEtd = null;
            DateTime? nextPickUpEta = null;
            DateTime? nextPickUpEtd = null;

            DateTime? nextPreCarriageEtd = null;
            DateTime? nextOnCarriageEtd = null;
            DateTime? nextMainCarriageLeg1Etd = null;
            DateTime? nextMainCarriageLeg2Etd = null;
            DateTime? nextMainCarriageLeg3Etd = null;
            DateTime? nextMainCarriageLeg4Etd = null;

            DateTime? nextPreCarriageEta = null;
            DateTime? nextOnCarriageEta = null;
            DateTime? nextMainCarriageLeg1Eta = null;
            DateTime? nextMainCarriageLeg2Eta = null;
            DateTime? nextMainCarriageLeg3Eta = null;
            DateTime? nextMainCarriageLeg4Eta = null;
            bool expectedEtaExists = false;
            bool expectedEtdExists = false;

            if (shipmentPm.ShipmentPickUps != null)
            {
                if (shipmentPm.ShipmentPickUps.Count != 0)
                {
                    shipmentPickUps = (from a in shipmentPm.ShipmentPickUps
                                       select a).ToList();
                }
                
                if (shipmentPickUps != null)
                {
                    if (shipmentPickUps.Count != 0)
                    {
                        pickUpExists = true;
                    }
                }
            }

            if (shipmentPm.ShipmentDeliveries != null)
            {
                if (shipmentPm.ShipmentDeliveries.Count != 0)
                {
                    shipmentDeliveries = (from a in shipmentPm.ShipmentDeliveries
                                          select a).ToList();

                }

                if (shipmentDeliveries != null)
                {
                    if (shipmentDeliveries.Count != 0)
                    {
                        deliveryExists = true;
                    }
                }
            }


            if (deliveryExists)//if there is deliveries in shipment.
            {

                foreach (ShipmentDeliveryPM delivery in shipmentDeliveries)
                {
                    if (delivery.ATA == null)
                    {
                        deliveryAtaAllExists = false;
                        if (nextDeliveryEta == null && delivery.ETA != null)
                        {
                            nextDeliveryEta = delivery.ETA;
                            expectedEtaExists = true;
                        }
                        else
                        {
                            if (delivery.ETA != null)
                            {
                                if (DateTime.Compare(delivery.ETA.Value, nextDeliveryEta.Value) < 0)
                                {
                                    nextDeliveryEta = delivery.ETA;
                                }
                            }
                        }
                    }
                    else
                    {
                        continueEta = false;
                        continueEtd = false;
                        continueActualCheck = false;
                    }

                    if (delivery.ATD == null)
                    {
                        deliveryAtdAllExists = false;
                        if (nextDeliveryEtd == null)
                        {
                            nextDeliveryEtd = delivery.ETD;
                            expectedEtdExists = true;
                        }
                        else
                        {
                            if (delivery.ETD != null)
                            {
                                if (DateTime.Compare(delivery.ETD.Value, nextDeliveryEtd.Value) < 0)
                                {
                                    nextDeliveryEtd = delivery.ETD;
                                }
                            }
                        }
                    }
                    else
                    {
                        continueEta = false;
                        continueEtd = false;
                        continueActualCheck = false;


                    }

                }
                //if (DeliveryATAAllExists)
                //{
                //    ContinueETA = false;
                //}
                //if (DeliveryATDAllExists)
                //{
                //    ContinueETD = false;
                //}
                shipment.NextETA = nextDeliveryEta;
                shipment.NextETD = nextDeliveryEtd;

            }

            #region NextETD

            if (shipmentPm.OnCarriageATD == null && shipmentPm.OnCarriageETD != null && continueEtd)
            {
                nextOnCarriageEtd = shipmentPm.OnCarriageETD;
                shipment.NextETD = nextOnCarriageEtd;
                expectedEtdExists = true;
            }
            else if (shipmentPm.OnCarriageATD != null && continueEtd)
            {
                continueEtd = false;
                continueEta = false;
                nextOnCarriageEtd = null;
                expectedEtdExists = true;
                //NextOnCarriageETA =shipmentPM.OnCarriageATA==null? shipmentPM.OnCarriageETA:NextDeliveryETA;
                shipment.NextETA = shipmentPm.OnCarriageATA == null ? shipmentPm.OnCarriageETA : nextDeliveryEta;
                #region fill nextETA

                if (shipmentPm.OnCarriageATA == null && shipmentPm.OnCarriageETA != null)
                {
                    shipment.NextETA = shipmentPm.OnCarriageETA;
                }
                else
                {
                    shipment.NextETA = nextDeliveryEta;
                }



                #endregion
            }
            if (shipmentPm.Transshipment3ATD == null && shipmentPm.Transshipment3ETD != null && continueEtd)
            {
                nextMainCarriageLeg4Etd = shipmentPm.Transshipment3ETD;
                shipment.NextETD = nextMainCarriageLeg4Etd;
                expectedEtdExists = true;
            }
            else if (shipmentPm.Transshipment3ATD != null && continueEtd)
            {
                continueEtd = false;
                continueEta = false;
                nextMainCarriageLeg4Etd = null;
                #region fill nextETA

                if (shipmentPm.Transshipment3ATA == null && shipmentPm.Transshipment3ETA != null)
                {
                    shipment.NextETA = shipmentPm.Transshipment3ETA;
                }
                else if (shipmentPm.OnCarriageATA == null && shipmentPm.OnCarriageETA != null)
                {
                    shipment.NextETA = shipmentPm.OnCarriageETA;
                }
                else
                {
                    shipment.NextETA = nextDeliveryEta;
                }



                #endregion
                // NextMainCarriageLeg4ETA = shipmentPM.Transshipment3ETA;
                //shipmentPM.NextETA =shipmentPM.Transshipment3ATA==null?shipmentPM.Transshipment3ETA:shipmentPM.OnCarriageATA == null ? shipmentPM.OnCarriageETA : NextDeliveryETA;
                expectedEtdExists = true;
            }
            if (shipmentPm.Transshipment2ATD == null && shipmentPm.Transshipment2ETD != null && continueEtd)
            {
                nextMainCarriageLeg3Etd = shipmentPm.Transshipment2ETD;
                shipment.NextETD = nextMainCarriageLeg3Etd;
                expectedEtdExists = true;
            }
            else if (shipmentPm.Transshipment2ATD != null && continueEtd)
            {
                continueEtd = false;
                continueEta = false;
                nextMainCarriageLeg3Etd = null;
                #region fill nextETA

                if (shipmentPm.Transshipment2ATA == null && shipmentPm.Transshipment2ETA != null)
                {
                    shipment.NextETA = shipmentPm.Transshipment2ETA;
                }
                else if (shipmentPm.Transshipment3ATA == null && shipmentPm.Transshipment3ETA != null)
                {
                    shipment.NextETA = shipmentPm.Transshipment3ETA;
                }
                else if (shipmentPm.OnCarriageATA == null && shipmentPm.OnCarriageETA != null)
                {
                    shipment.NextETA = shipmentPm.OnCarriageETA;
                }
                else
                {
                    shipment.NextETA = nextDeliveryEta;
                }



                #endregion
                expectedEtdExists = true;
                //NextMainCarriageLeg3ETA = shipmentPM.Transshipment2ETA;
                //  shipmentPM.NextETA = shipmentPM.Transshipment2ATA == null ? shipmentPM.Transshipment2ETA : shipmentPM.Transshipment3ATA == null ? shipmentPM.Transshipment3ETA : shipmentPM.OnCarriageATA == null ? shipmentPM.OnCarriageETA : NextDeliveryETA;
            }
            if (shipmentPm.Transshipment1ATD == null && shipmentPm.Transshipment1ETD != null && continueEtd)
            {
                nextMainCarriageLeg2Etd = shipmentPm.Transshipment1ETD;
                shipment.NextETD = nextMainCarriageLeg2Etd;
                expectedEtdExists = true;
            }
            else if (shipmentPm.Transshipment1ATD != null && continueEtd)
            {
                continueEtd = false;
                continueEta = false;
                nextMainCarriageLeg2Etd = null;
                expectedEtdExists = true;
                #region fill nextETA

                if (shipmentPm.Transshipment1ATA == null && shipmentPm.Transshipment1ETA != null)
                {
                    shipment.NextETA = shipmentPm.Transshipment1ETA;
                }
                else if (shipmentPm.Transshipment2ATA == null && shipmentPm.Transshipment2ETA != null)
                {
                    shipment.NextETA = shipmentPm.Transshipment2ETA;
                }
                else if (shipmentPm.Transshipment3ATA == null && shipmentPm.Transshipment3ETA != null)
                {
                    shipment.NextETA = shipmentPm.Transshipment3ETA;
                }
                else if (shipmentPm.OnCarriageATA == null && shipmentPm.OnCarriageETA != null)
                {
                    shipment.NextETA = shipmentPm.OnCarriageETA;
                }
                else
                {
                    shipment.NextETA = nextDeliveryEta;
                }



                #endregion
                //NextMainCarriageLeg2ETA = shipmentPM.Transshipment1ETA;
                //shipmentPM.NextETA = shipmentPM.Transshipment1ATA == null ? shipmentPM.Transshipment1ETA : shipmentPM.Transshipment2ATA == null ? shipmentPM.Transshipment2ETA : shipmentPM.Transshipment3ATA == null ? shipmentPM.Transshipment3ETA : shipmentPM.OnCarriageATA == null ? shipmentPM.OnCarriageETA : NextDeliveryETA;
            }
            if (shipmentPm.MainCarriageATD == null && shipmentPm.MainCarriageETD != null && continueEtd)
            {
                nextMainCarriageLeg1Etd = shipmentPm.MainCarriageETD;
                shipment.NextETD = nextMainCarriageLeg1Etd;
                expectedEtdExists = true;
            }
            else if (shipmentPm.MainCarriageATD != null && continueEtd)
            {
                continueEtd = false;
                continueEta = false;
                nextMainCarriageLeg1Etd = null;
                #region fill nextETA

                if (shipmentPm.MainCarriageATA == null && shipmentPm.MainCarriageETA != null)
                {
                    shipment.NextETA = shipmentPm.MainCarriageETA;
                }
                else if (shipmentPm.Transshipment1ATA == null && shipmentPm.Transshipment1ETA != null)
                {
                    shipment.NextETA = shipmentPm.Transshipment1ETA;
                }
                else if (shipmentPm.Transshipment2ATA == null && shipmentPm.Transshipment2ETA != null)
                {
                    shipment.NextETA = shipmentPm.Transshipment2ETA;
                }
                else if (shipmentPm.Transshipment3ATA == null && shipmentPm.Transshipment3ETA != null)
                {
                    shipment.NextETA = shipmentPm.Transshipment3ETA;
                }
                else if (shipmentPm.OnCarriageATA == null && shipmentPm.OnCarriageETA != null)
                {
                    shipment.NextETA = shipmentPm.OnCarriageETA;
                }
                else
                {
                    shipment.NextETA = nextDeliveryEta;
                }



                #endregion
                expectedEtdExists = true;
                //NextMainCarriageLeg1ETA = shipmentPM.MainCarriageETA;
                //shipmentPM.NextETA =shipmentPM.MainCarriageATA==null ?shipmentPM.MainCarriageETA : shipmentPM.Transshipment1ATA == null ? shipmentPM.Transshipment1ETA : shipmentPM.Transshipment2ATA == null ? shipmentPM.Transshipment2ETA : shipmentPM.Transshipment3ATA == null ? shipmentPM.Transshipment3ETA : shipmentPM.OnCarriageATA == null ? shipmentPM.OnCarriageETA: NextDeliveryETA;
            }
            if (shipmentPm.PreCarriageATD == null && shipmentPm.PreCarriageETD != null && continueEtd)
            {
                nextPreCarriageEtd = shipmentPm.PreCarriageETD;
                shipment.NextETD = nextPreCarriageEtd;
                expectedEtdExists = true;
            }
            else if (shipmentPm.PreCarriageATD != null && continueEtd)
            {
                continueEtd = false;
                continueEta = false;
                nextPreCarriageEtd = null;
                #region fill nextETA
                if (shipmentPm.PreCarriageATA == null && shipmentPm.PreCarriageETA != null)
                {
                    shipment.NextETA = shipmentPm.PreCarriageETA;
                }
                else if (shipmentPm.MainCarriageATA == null && shipmentPm.MainCarriageETA != null)
                {
                    shipment.NextETA = shipmentPm.MainCarriageETA;
                }
                else if (shipmentPm.Transshipment1ATA == null && shipmentPm.Transshipment1ETA != null)
                {
                    shipment.NextETA = shipmentPm.Transshipment1ETA;
                }
                else if (shipmentPm.Transshipment2ATA == null && shipmentPm.Transshipment2ETA != null)
                {
                    shipment.NextETA = shipmentPm.Transshipment2ETA;
                }
                else if (shipmentPm.Transshipment3ATA == null && shipmentPm.Transshipment3ETA != null)
                {
                    shipment.NextETA = shipmentPm.Transshipment3ETA;
                }
                else if (shipmentPm.OnCarriageATA == null && shipmentPm.OnCarriageETA != null)
                {
                    shipment.NextETA = shipmentPm.OnCarriageETA;
                }
                else
                {
                    shipment.NextETA = nextDeliveryEta;
                }



                #endregion
                expectedEtdExists = true;
                // NextPreCarriageETA = shipmentPM.PreCarriageETA;
                // shipmentPM.NextETA =shipmentPM.PreCarriageATA==null?shipmentPM.PreCarriageETA:shipmentPM.MainCarriageATA == null ? shipmentPM.MainCarriageETA : shipmentPM.Transshipment1ATA == null ? shipmentPM.Transshipment1ETA : shipmentPM.Transshipment2ATA == null ? shipmentPM.Transshipment2ETA : shipmentPM.Transshipment3ATA == null ? shipmentPM.Transshipment3ETA : shipmentPM.OnCarriageATA == null ? shipmentPM.OnCarriageETA : NextDeliveryETA;
            }
            if (pickUpExists && continueEtd)
            {
                foreach (ShipmentPickUpPM pickUp in shipmentPickUps)
                {
                    if (pickUp.ATD == null)
                    {

                        if (nextPickUpEtd == null && pickUp.ETD != null)
                        {
                            nextPickUpEtd = pickUp.ETD;
                            expectedEtdExists = true;
                        }
                        else
                        {
                            if (pickUp.ETD != null)
                            {
                                if (DateTime.Compare(pickUp.ETD.Value, nextPickUpEtd.Value) < 0)
                                {
                                    nextPickUpEtd = pickUp.ETD;
                                }
                            }
                        }
                    }


                }
                if (nextPickUpEtd != null)
                {
                    shipment.NextETD = nextPickUpEtd;
                    expectedEtdExists = true;
                }

            }

            #endregion

            #region NextETA

            if (shipmentPm.OnCarriageATA == null && shipmentPm.OnCarriageETA != null && continueEta)
            {
                shipment.NextETA = shipmentPm.OnCarriageETA;
                expectedEtaExists = true;
            }
            else if (shipmentPm.OnCarriageATA != null && continueActualCheck)
            {
                continueEta = false;
                shipment.NextETD = nextDeliveryEtd;
                shipment.NextETA = nextDeliveryEta;
                continueActualCheck = false;
                expectedEtaExists = true;
            }
            if (shipmentPm.Transshipment3ATA == null && shipmentPm.Transshipment3ETA != null && continueEta)
            {
                shipment.NextETA = shipmentPm.Transshipment3ETA;
                expectedEtaExists = true;
            }
            else if (shipmentPm.Transshipment3ATA != null && continueActualCheck)
            {
                continueEta = false;
                shipment.NextETD = nextOnCarriageEtd != null ? nextOnCarriageEtd : nextDeliveryEtd;

                #region fill nextETA

                if (shipmentPm.OnCarriageATA == null && shipmentPm.OnCarriageETA != null)
                {
                    shipment.NextETA = shipmentPm.OnCarriageETA;
                }
                else
                {
                    shipment.NextETA = nextDeliveryEta;
                }



                #endregion
                expectedEtaExists = true;
                // shipmentPM.NextETA =  shipmentPM.OnCarriageATA == null ? shipmentPM.OnCarriageETA : NextDeliveryETA;
                continueActualCheck = false;
            }
            if (shipmentPm.Transshipment2ATA == null && shipmentPm.Transshipment2ETA != null && continueEta)
            {
                shipment.NextETA = shipmentPm.Transshipment2ETA;
                expectedEtaExists = true;
            }
            else if (shipmentPm.Transshipment2ATA != null && continueActualCheck)
            {
                continueEta = false;
                continueActualCheck = false;
                shipment.NextETD = nextMainCarriageLeg4Etd != null ? nextMainCarriageLeg4Etd : nextOnCarriageEtd != null ? nextOnCarriageEtd : nextDeliveryEtd;
                #region fill nextETA

                if (shipmentPm.Transshipment3ATA == null && shipmentPm.Transshipment3ETA != null)
                {
                    shipment.NextETA = shipmentPm.Transshipment3ETA;
                }
                else if (shipmentPm.OnCarriageATA == null && shipmentPm.OnCarriageETA != null)
                {
                    shipment.NextETA = shipmentPm.OnCarriageETA;
                }
                else
                {
                    shipment.NextETA = nextDeliveryEta;
                }
                #endregion
                expectedEtaExists = true;
                //shipmentPM.NextETA = shipmentPM.Transshipment3ATA == null ? shipmentPM.Transshipment3ETA : shipmentPM.OnCarriageATA == null ? shipmentPM.OnCarriageETA : NextDeliveryETA;
            }
            if (shipmentPm.Transshipment1ATA == null && shipmentPm.Transshipment1ETA != null && continueEta)
            {
                shipment.NextETA = shipmentPm.Transshipment1ETA;
                expectedEtaExists = true;
            }
            else if (shipmentPm.Transshipment1ATA != null && continueActualCheck)
            {
                continueEta = false;
                continueActualCheck = false;
                shipment.NextETD = nextMainCarriageLeg3Etd != null ? nextMainCarriageLeg3Etd : nextMainCarriageLeg4Etd != null ? nextMainCarriageLeg4Etd : nextOnCarriageEtd != null ? nextOnCarriageEtd : nextDeliveryEtd;
                #region fill nextETA

                if (shipmentPm.Transshipment2ATA == null && shipmentPm.Transshipment2ETA != null)
                {
                    shipment.NextETA = shipmentPm.Transshipment2ETA;
                }
                else if (shipmentPm.Transshipment3ATA == null && shipmentPm.Transshipment3ETA != null)
                {
                    shipment.NextETA = shipmentPm.Transshipment3ETA;
                }
                else if (shipmentPm.OnCarriageATA == null && shipmentPm.OnCarriageETA != null)
                {
                    shipment.NextETA = shipmentPm.OnCarriageETA;
                }
                else
                {
                    shipment.NextETA = nextDeliveryEta;
                }



                #endregion
                expectedEtaExists = true;
                //shipmentPM.NextETA = shipmentPM.Transshipment2ATA == null ? shipmentPM.Transshipment2ETA : shipmentPM.Transshipment3ATA == null ? shipmentPM.Transshipment3ETA : shipmentPM.OnCarriageATA == null ? shipmentPM.OnCarriageETA : NextDeliveryETA;
            }
            if (shipmentPm.MainCarriageATA == null && shipmentPm.MainCarriageETA != null && continueEta)
            {
                shipment.NextETA = shipmentPm.MainCarriageETA;
                expectedEtaExists = true;
            }
            else if (shipmentPm.MainCarriageATA != null && continueActualCheck)
            {
                continueEta = false;
                continueActualCheck = false;
                shipment.NextETD = nextMainCarriageLeg2Etd != null ? nextMainCarriageLeg2Etd : nextMainCarriageLeg3Etd != null ? nextMainCarriageLeg3Etd : nextMainCarriageLeg4Etd != null ? nextMainCarriageLeg4Etd : nextOnCarriageEtd != null ? nextOnCarriageEtd : nextDeliveryEtd;
                #region fill nextETA

                if (shipmentPm.Transshipment1ATA == null && shipmentPm.Transshipment1ETA != null)
                {
                    shipment.NextETA = shipmentPm.Transshipment1ETA;
                }
                else if (shipmentPm.Transshipment2ATA == null && shipmentPm.Transshipment2ETA != null)
                {
                    shipment.NextETA = shipmentPm.Transshipment2ETA;
                }
                else if (shipmentPm.Transshipment3ATA == null && shipmentPm.Transshipment3ETA != null)
                {
                    shipment.NextETA = shipmentPm.Transshipment3ETA;
                }
                else if (shipmentPm.OnCarriageATA == null && shipmentPm.OnCarriageETA != null)
                {
                    shipment.NextETA = shipmentPm.OnCarriageETA;
                }
                else
                {
                    shipment.NextETA = nextDeliveryEta;
                }



                #endregion
                expectedEtaExists = true;
                //shipmentPM.NextETA = shipmentPM.Transshipment1ATA == null ? shipmentPM.Transshipment1ETA : shipmentPM.Transshipment2ATA == null ? shipmentPM.Transshipment2ETA : shipmentPM.Transshipment3ATA == null ? shipmentPM.Transshipment3ETA : shipmentPM.OnCarriageATA == null ? shipmentPM.OnCarriageETA : NextDeliveryETA;
            }
            if (shipmentPm.PreCarriageATA == null && shipmentPm.PreCarriageETA != null && continueEta)
            {
                shipment.NextETA = shipmentPm.PreCarriageETA;
                expectedEtaExists = true;
            }
            else if (shipmentPm.PreCarriageATA != null && continueActualCheck)
            {
                continueEta = false;
                continueActualCheck = false;
                shipment.NextETD = nextMainCarriageLeg1Etd != null ? nextMainCarriageLeg1Etd : nextMainCarriageLeg3Etd != null ? nextMainCarriageLeg3Etd : nextMainCarriageLeg4Etd != null ? nextMainCarriageLeg4Etd : nextOnCarriageEtd != null ? nextOnCarriageEtd : nextDeliveryEtd;
                #region fill nextETA

                if (shipmentPm.MainCarriageATA == null && shipmentPm.MainCarriageETA != null)
                {
                    shipment.NextETA = shipmentPm.MainCarriageETA;
                }
                else if (shipmentPm.Transshipment1ATA == null && shipmentPm.Transshipment1ETA != null)
                {
                    shipment.NextETA = shipmentPm.Transshipment1ETA;
                }
                else if (shipmentPm.Transshipment2ATA == null && shipmentPm.Transshipment2ETA != null)
                {
                    shipment.NextETA = shipmentPm.Transshipment2ETA;
                }
                else if (shipmentPm.Transshipment3ATA == null && shipmentPm.Transshipment3ETA != null)
                {
                    shipment.NextETA = shipmentPm.Transshipment3ETA;
                }
                else if (shipmentPm.OnCarriageATA == null && shipmentPm.OnCarriageETA != null)
                {
                    shipment.NextETA = shipmentPm.OnCarriageETA;
                }
                else
                {
                    shipment.NextETA = nextDeliveryEta;
                }



                #endregion
                expectedEtaExists = true;
                //shipmentPM.NextETA = shipmentPM.MainCarriageATA == null ? shipmentPM.MainCarriageETA : shipmentPM.Transshipment1ATA == null ? shipmentPM.Transshipment1ETA : shipmentPM.Transshipment2ATA == null ? shipmentPM.Transshipment2ETA : shipmentPM.Transshipment3ATA == null ? shipmentPM.Transshipment3ETA : shipmentPM.OnCarriageATA == null ? shipmentPM.OnCarriageETA : NextDeliveryETA;
            }
            if (pickUpExists && continueEta)
            {
                foreach (ShipmentPickUpPM pickUp in shipmentPickUps)
                {
                    if (pickUp.ATA == null)
                    {

                        if (nextPickUpEta == null && pickUp.ETA != null)
                        {
                            nextPickUpEta = pickUp.ETA;
                            expectedEtaExists = true;
                        }
                        else
                        {
                            if (pickUp.ETA != null)
                            {
                                if (DateTime.Compare(pickUp.ETA.Value, nextPickUpEta.Value) < 0)
                                {
                                    nextPickUpEta = pickUp.ETA;
                                }
                            }
                        }
                    }
                }
                if (nextPickUpEta != null)
                {
                    shipment.NextETA = nextPickUpEta;
                }

            }


            if (!expectedEtaExists)
            {
                shipment.NextETA = null;
            }
            if (!expectedEtdExists)
            {
                shipment.NextETD = null;
            }
            #endregion

            #endregion

            #region Status Calculations

            #region PickUp region

            foreach (ShipmentPickUpPM shipmentPickUpDelivery in shipmentPm.ShipmentPickUps)
            {
                if (shipmentPickUpDelivery.ATA != null)
                {
                    eventTypeQuery = new EventTypeQuery(eventTypeRep);
                    EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("RCS", shipmentPickUpDelivery.Tenant);
                    shipment.StatusId = EntityStatusHelper.GetHighestStatusId(shipment.StatusId, eventType.EntityStatusId, shipment.Tenant);
                }

                if (shipmentPickUpDelivery.ATD != null)
                {
                    eventTypeQuery = new EventTypeQuery(eventTypeRep);
                    EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("PICD", shipmentPickUpDelivery.Tenant);
                    shipment.StatusId = EntityStatusHelper.GetHighestStatusId(shipment.StatusId, eventType.EntityStatusId, shipment.Tenant);
                }
            }

            #endregion

            #region delivery region
            foreach (ShipmentDeliveryPM shipmentPickUpDelivery in shipmentPm.ShipmentDeliveries)
            {
                if (shipmentPickUpDelivery.ATA != null)
                {
                    eventTypeQuery = new EventTypeQuery(eventTypeRep);
                    EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("PIOD", shipmentPickUpDelivery.Tenant);
                    shipment.StatusId = EntityStatusHelper.GetHighestStatusId(shipment.StatusId, eventType.EntityStatusId, shipment.Tenant);
                }

                if (shipmentPickUpDelivery.ATD != null)
                {
                    eventTypeQuery = new EventTypeQuery(eventTypeRep);
                    EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("DELD", shipmentPickUpDelivery.Tenant);
                    shipment.StatusId = EntityStatusHelper.GetHighestStatusId(shipment.StatusId, eventType.EntityStatusId, shipment.Tenant);
                }
            }

            #endregion

            #region MainCarriage Region

            if (shipmentPm.MainCarriageATD != null)
            {
                eventTypeQuery = new EventTypeQuery(eventTypeRep);
                EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("DEP", shipment.Tenant);
                shipment.StatusId = EntityStatusHelper.GetHighestStatusId(shipment.StatusId, eventType.EntityStatusId, shipment.Tenant);
                if (masterData != null)
                {
                    masterData.StatusId = shipment.StatusId;
                }
            }

            if (shipmentPm.Transshipment3FromPortId != null)
            {
                if (shipmentPm.Transshipment3ATA != null)
                {
                    eventTypeQuery = new EventTypeQuery(eventTypeRep);
                    EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("ARR", shipment.Tenant);
                    shipment.StatusId = EntityStatusHelper.GetHighestStatusId(shipment.StatusId, eventType.EntityStatusId, shipment.Tenant);
                    if (masterData != null)
                    {
                        masterData.StatusId = shipment.StatusId;
                    }
                }
            }
            else if (shipmentPm.Transshipment2FromPortId != null)
            {
                if (shipmentPm.Transshipment2ATA != null)
                {
                    eventTypeQuery = new EventTypeQuery(eventTypeRep);
                    EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("ARR", shipment.Tenant);
                    shipment.StatusId = EntityStatusHelper.GetHighestStatusId(shipment.StatusId, eventType.EntityStatusId, shipment.Tenant);
                    if (masterData != null)
                    {
                        masterData.StatusId = shipment.StatusId;
                    }
                }
            }
            else if (shipmentPm.Transshipment1FromPortId != null)
            {
                if (shipmentPm.Transshipment1ATA != null)
                {
                    eventTypeQuery = new EventTypeQuery(eventTypeRep);
                    EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("ARR", shipment.Tenant);
                    shipment.StatusId = EntityStatusHelper.GetHighestStatusId(shipment.StatusId, eventType.EntityStatusId, shipment.Tenant);
                    if (masterData != null)
                    {
                        masterData.StatusId = shipment.StatusId;
                    }
                }
            }
            else if (shipmentPm.MainCarriageFromPortId != null)
            {
                if (shipmentPm.MainCarriageATA != null)
                {
                    eventTypeQuery = new EventTypeQuery(eventTypeRep);
                    EventTypePM eventType = eventTypeQuery.GetSingleEventTypePMByCode("ARR", shipment.Tenant);
                    shipment.StatusId = EntityStatusHelper.GetHighestStatusId(shipment.StatusId, eventType.EntityStatusId, shipment.Tenant);
                    if (masterData != null)
                    {
                        masterData.StatusId = shipment.StatusId;
                    }
                }
            }

         

           
            #endregion

            #endregion
        }
    }
}
