using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web.Services;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.ShipmentsModel.Repositories;
using WebFreight.Web.CommonDataModel.DomainServices;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.DataProviders;
using WebFreight.Web.ShipmentsModel.DomainServices;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using WebFreight.Web.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.Helpers;
using Simplog.Data.Helpers;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.EntityPMs;

namespace WebFreight.Web.ReportsWebServices
{
    /// <summary>
    /// Summary description for ShipmentProfitWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.  
    // [System.Web.Script.Services.ScriptService]
    public class ShipmentProfitWebService : System.Web.Services.WebService
    {
        [WebMethod]
        public byte[] GetProfitData(string shipmentId, int tenant, string accountingCurrencyId, string currentUser)
        {
            ShipmentProfitDataProvider provider = GetProfitDataProvider(shipmentId, tenant, accountingCurrencyId, currentUser);
            XmlSerializer serializer = new XmlSerializer(typeof(ShipmentProfitDataProvider));
            MemoryStream memstream = new MemoryStream();
            serializer.Serialize(memstream, provider);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();
            return bytearray;
        }

        [WebMethod]
        public byte[] GetProfitInvoicesData(string shipmentId, int tenant, string accountingCurrencyId, string currentUser)
        {
            ShipmentProfitInvoicesDataProvider provider = this.BuildProfitInvoicesProvider(shipmentId, tenant, accountingCurrencyId, currentUser);
            XmlSerializer serializer = new XmlSerializer(typeof(ShipmentProfitInvoicesDataProvider));
            MemoryStream memstream = new MemoryStream();
            serializer.Serialize(memstream, provider);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();
            return bytearray;
        }

        public ShipmentProfitDataProvider GetProfitDataProvider(string shipmentId, int tenant, string accountingCurrencyId, string currentUser)
        {
            ShipmentProfitDataProvider provider = new ShipmentProfitDataProvider();
            IWebFreightContext context = WebFreightContext.GetContext(tenant);
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);
            ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRepository);
            CurrencyRepository currencyRepository = new CurrencyRepository(commonContext);
            AddressRepository addressRepository = new AddressRepository(commonContext);
            VatTypeRepository vatTypeRepository = new VatTypeRepository(commonContext);
            VatTypeQuery vatTypeQuery = new VatTypeQuery(vatTypeRepository);
            List<Currency> listCurrency = currencyRepository.GetCurrencies(tenant).ToList();
            ShipmentPM shipmentPM = shipmentQuery.GetSinglePM(shipmentId, tenant);

            TenantRepository tenantRepository = new TenantRepository(tenant);
            Tenant tenantSettings = (from a in commonContext.Tenants where a.Id == tenant select a).FirstOrDefault();

            provider.IssueDate = TenantServerConfigration.GetCurrentDateTime(tenant);

            #region Get Profit Currency
            string profitCurrencyId = String.Empty;
            string profitCurrencyCode = String.Empty;
            string profitCurrencyName = String.Empty;
            Currency profitCurrency = null;

            if (shipmentPM != null)
            {
                profitCurrencyId = shipmentPM.ProfitCurrencyId;
            }

            if (!string.IsNullOrEmpty(profitCurrencyId))
            {
                profitCurrency = listCurrency.Where(d => d.Id == profitCurrencyId).FirstOrDefault();
                if (profitCurrency != null)
                {
                    profitCurrencyCode = profitCurrency.Code;
                    profitCurrencyName = profitCurrency.EnglishName;
                }
            }
            else
            {
                if (tenantSettings != null)
                {
                    profitCurrency = tenantSettings.ProfitCurrency;
                    if (profitCurrency != null)
                    {
                        profitCurrencyId = profitCurrency.Id;
                        profitCurrencyCode = profitCurrency.Code;
                        profitCurrencyName = profitCurrency.EnglishName;
                    }
                }
            }

            provider.ProfitCurrencyCode = profitCurrencyCode;
            provider.ProfitCurrencyName = profitCurrencyName;
            #endregion

            #region Get Local Currency
            string localCurrencyCode = String.Empty;
            string localCurrencyName = String.Empty;
            Currency localCurrency = null;

            if (!string.IsNullOrEmpty(accountingCurrencyId))
            {
                localCurrency = listCurrency.Where(d => d.Id == accountingCurrencyId).FirstOrDefault();
                if (localCurrency != null)
                {
                    localCurrencyCode = localCurrency.Code;
                    localCurrencyName = localCurrency.EnglishName;
                }
            }
            else
            {
                if (tenantSettings != null)
                {
                    localCurrency = listCurrency.Where(d => d.Id == tenantSettings.CurrencyId).FirstOrDefault();

                    if (localCurrency != null)
                    {
                        accountingCurrencyId = localCurrency.Id;
                        localCurrencyCode = localCurrency.Code;
                        localCurrencyName = localCurrency.EnglishName;
                    }
                }
            }

            provider.LocalCurrencyCode = localCurrencyCode;
            provider.LocalCurrencyName = localCurrencyName;
            #endregion

            #region Get Profit Exchange Rate
            //double? ProfitExchangeRate = 1;
            //if (!string.IsNullOrEmpty(ProfitCurrencyId) && shipmentPM == null)
            //{
            //    if (ProfitCurrencyId == AccountingCurrencyId)
            //    {
            //        ProfitExchangeRate = 1;
            //    }
            //    else
            //    {
            //        LastRate lastRate = ratesTableRepository.GetLastRecordByValueDate(tenant, ProfitCurrencyId, AccountingCurrencyId, shipmentPM.CreateDateTime);
            //        if (lastRate != null)
            //        {
            //            ProfitExchangeRate = lastRate.Rate;
            //        }
            //    }
            //}            
            #endregion

            if (shipmentPM != null)
            {
                #region Shipment Fields

                if (shipmentPM.ShipmentLevelCode == "H")
                {
                    Shipment tempMaterShipment = shipmentRepository.GetSingleShipment(shipmentPM.MasterShipmentDataId, tenant);
                    if (tempMaterShipment != null)
                    {
                        provider.MasterShipmentNumber = tempMaterShipment.ShipmentNumber;
                    }
                }

                else if (shipmentPM.ShipmentLevelCode == "C")
                {
                    provider.MasterShipmentNumber = shipmentPM.ShipmentNumber;
                }

                double? openReceivablesLocal = shipmentPM.OpenReceivablesInLocalCurrency;
                double? acctReceivablesLocal = shipmentPM.AccountedReceivablesInLocalCurrency;
                double? allReceivablesLocal = openReceivablesLocal.Value + acctReceivablesLocal.Value;

                double? openReceivablesProfit = shipmentPM.OpenReceivablesInProfitCurrency;
                double? acctReceivablesProfit = shipmentPM.AccountedReceivablesInProfitCurrency;
                double? allReceivablesProfit = openReceivablesProfit + acctReceivablesProfit;

                double? openPayablesLocal = shipmentPM.OpenPayablesInLocalCurrency;
                double? acctPayablesLocal = shipmentPM.AccountedPayablesInLocalCurrency;
                double? allPayablesLocal = openPayablesLocal.Value + acctPayablesLocal.Value;

                double? openPayablesProfit = shipmentPM.OpenPayablesInProfitCurrency;
                double? acctPayablesProfit = shipmentPM.AccountedPayablesInProfitCurrency;
                double? allPayablesProfit = openPayablesProfit.Value + acctPayablesProfit.Value;

                provider.ReceivablesInLocalCurrency = String.Format("{0:#,0.00}", allReceivablesLocal);
                provider.ReceivablesInProfitCurrency = String.Format("{0:#,0.00}", allReceivablesProfit);
                provider.PayablesInLocalCurrency = String.Format("{0:#,0.00}", allPayablesLocal);
                provider.PayablesInProfitCurrency = String.Format("{0:#,0.00}", allPayablesProfit);

                double? profitInLocalCurrency = shipmentPM.ProfitInLocalCurrency;
                double? profitInProfitCurrency = shipmentPM.ProfitInProfitCurrency;
                double? estimateProfitInLocal = shipmentPM.EstimateProfitInLocalCurrency == null ? 0 : shipmentPM.EstimateProfitInLocalCurrency;
                double? estimateProfitInProfit = shipmentPM.EstimateProfitInProfitCurrency == null ? 0 : shipmentPM.EstimateProfitInProfitCurrency;
                double? differenceInLocalCurrency = profitInLocalCurrency.Value - estimateProfitInLocal.Value;
                double? differenceInProfitCurrency = profitInProfitCurrency.Value - estimateProfitInProfit.Value;

                provider.ProfitInLocalCurrency = String.Format("{0:#,0.00}", profitInLocalCurrency);
                provider.ProfitInProfitCurrency = String.Format("{0:#,0.00}", profitInProfitCurrency);
                provider.EstimateProfitInLocalCurrency = String.Format("{0:#,0.00}", estimateProfitInLocal);
                provider.EstimateProfitInProfitCurrency = String.Format("{0:#,0.00}", estimateProfitInProfit);
                provider.DifferenceInLocalCurrency = String.Format("{0:#,0.00}", differenceInLocalCurrency);
                provider.DifferenceInProfitCurrency = String.Format("{0:#,0.00}", differenceInProfitCurrency);
                provider.ShipmentVolume = shipmentPM.Volume;
                provider.VolumeUnitCode = shipmentPM.VolumeUnitCode;
                provider.IsAccrualsApproved = shipmentPM.IsAccrualsApproved;
                provider.AccrualsApprovalDate = shipmentPM.AccrualsApprovalDate;
                #endregion

                #region Group by ChargeType

                List<ShipmentReceivablePM> allReceivables = new List<ShipmentReceivablePM>();
                List<ShipmentPayablePM> allPayables = new List<ShipmentPayablePM>();

                if (shipmentPM.ShipmentLevelCode == "C" && shipmentPM.ShipmentConsoleShipments.Count > 0)
                {
                    ShipmentsDomainService shipmentContext = new ShipmentsDomainService();
                    allReceivables = shipmentContext.GetMasterReceivablesForProfit(shipmentPM.Id, shipmentPM.Tenant);
                    allPayables = shipmentContext.GetMasterPayablesForProfit(shipmentPM.Id, shipmentPM.Tenant);
                }
                else
                {
                    allReceivables = shipmentPM.ShipmentReceivables.ToList();
                    allPayables = shipmentPM.ShipmentPayables.ToList();
                }

                provider.ChargeTypesList = new List<ProfitDetailsClass>();

                //where item.UnitPrice != null && item.Quantity != null
                List<ProfitDetailsClass> payablesGroupByList =
                    (from item in allPayables
                     group item by new { item.ChargesTypeId, item.VendorId } into g
                     select new ProfitDetailsClass
                     {
                         ChargeTypeId = g.Select(s => s.ChargesTypeId).FirstOrDefault(),
                         ChargeTypeCode = g.Select(s => s.ChargesTypeCode).FirstOrDefault(),
                         ChargeTypeName = g.Select(s => s.ChargesTypeName).FirstOrDefault(),
                         IsExpenseCharge = g.Select(s => s.IsExpenseCharge).FirstOrDefault(),
                         OpenPayablesInLocal = g.Sum(s => s.OpenAmountInLocalCurrency),
                         OpenPayablesInProfit = g.Sum(s => s.OpenAmountInProfitCurrency),
                         ACCTPayablesInLocal = g.Sum(s => s.AccountedAmountInLocalCurrency),
                         ACCTPayablesInProfit = g.Sum(s => s.AccountedAmountInProfitCurrency),
                         Vendor = g.Select(s => s.VendorName).FirstOrDefault(),
                     }).ToList();

                List<ProfitDetailsClass> receivablesGroupByList =
                    (from item in allReceivables
                     where item.UnitPrice != null && item.Quantity != null
                     group item by new { item.ChargesTypeId } into g
                     select new ProfitDetailsClass
                     {
                         ChargeTypeId = g.Select(s => s.ChargesTypeId).FirstOrDefault(),
                         ChargeTypeCode = g.Select(s => s.ChargesTypeCode).FirstOrDefault(),
                         ChargeTypeName = g.Select(s => s.ChargesTypeName).FirstOrDefault(),
                         IsExpenseCharge = g.Select(s => s.IsExpenseCharge).FirstOrDefault(),
                         ReceivablesInLocalCurrency = String.Format("{0:#,0.00}", g.Sum(s => s.TotalAmountLocal)),
                         ReceivablesInProfitCurrency = String.Format("{0:#,0.00}", g.Sum(s => s.AmountInProfitCurrency)),
                     }).ToList();

                foreach (ProfitDetailsClass item in payablesGroupByList)
                {
                    ProfitDetailsClass record = new ProfitDetailsClass();
                    record.ChargeTypeId = item.ChargeTypeId;
                    record.ChargeTypeCode = item.ChargeTypeCode;
                    record.ChargeTypeName = item.ChargeTypeName;
                    record.IsExpenseCharge = item.IsExpenseCharge;
                    record.Vendor = item.Vendor;

                    double? theOpenPayablesLocal = item.OpenPayablesInLocal;
                    double? theAcctPayablesLocal = item.ACCTPayablesInLocal;
                    double? theAllPayablesLocal = theOpenPayablesLocal.Value + theAcctPayablesLocal.Value;

                    double? theOpenPayablesProfit = item.OpenPayablesInProfit;
                    double? theAcctPayablesProfit = item.ACCTPayablesInProfit;
                    double? theAllPayablesProfit = theOpenPayablesProfit.Value + theAcctPayablesProfit.Value;

                    record.PayablesInLocalCurrency = String.Format("{0:#,0.00}", theAllPayablesLocal);
                    record.PayablesInProfitCurrency = String.Format("{0:#,0.00}", theAllPayablesProfit);

                    ProfitDetailsClass rec = receivablesGroupByList.Where(d => d.ChargeTypeName == item.ChargeTypeName).FirstOrDefault();
                    if (rec != null)
                    {
                        receivablesGroupByList.Remove(rec);
                        record.ReceivablesInLocalCurrency = rec.ReceivablesInLocalCurrency;
                        record.ReceivablesInProfitCurrency = rec.ReceivablesInProfitCurrency;
                        record.ProfitInLocalCurrency = String.Format("{0:#,0.00}", Convert.ToDouble(record.ReceivablesInLocalCurrency) - Convert.ToDouble(record.PayablesInLocalCurrency));
                        record.ProfitInProfitCurrency = String.Format("{0:#,0.00}", Convert.ToDouble(record.ReceivablesInProfitCurrency) - Convert.ToDouble(record.PayablesInProfitCurrency));
                    }
                    else
                    {
                        record.ProfitInLocalCurrency = "-" + record.PayablesInLocalCurrency;
                        record.ProfitInProfitCurrency = "-" + record.PayablesInProfitCurrency;
                    }

                    string varVatTypeCode = String.Empty;
                    string varVatTypeName = String.Empty;
                    string varVatTypePercentage = String.Empty;
                    ChargesType chargesType = ChargesTypeRepository.GetSingleChargesType(item.ChargeTypeId, shipmentPM.Tenant, true);
                    if (chargesType != null)
                    {
                        if (chargesType.VatTypeId != null)
                        {
                            VatTypePM vatTypePM = vatTypeQuery.GetSinglePM(chargesType.VatTypeId, chargesType.Tenant);
                            if (vatTypePM != null)
                            {
                                varVatTypeCode = vatTypePM.Code;
                                varVatTypeName = vatTypePM.EnglishName;

                                if (vatTypePM.VatTypePercentages.Count > 0)
                                {
                                    VatTypePercentagePM vatTypePercentagePM = vatTypePM.VatTypePercentages.OrderByDescending(d => d.FromDate).FirstOrDefault();
                                    if (vatTypePercentagePM != null)
                                    {
                                        varVatTypePercentage = vatTypePercentagePM.Percentage == null ? String.Empty : String.Format("{0:#,0.00}", vatTypePercentagePM.Percentage);
                                    }
                                }
                            }
                        }
                    }

                    record.VatTypeCode = varVatTypeCode;
                    record.VatTypeName = varVatTypeName;
                    record.VatTypePercentage = varVatTypePercentage;

                    bool hasPayableAmount = (Convert.ToDouble(record.PayablesInLocalCurrency) != 0 && Convert.ToDouble(record.PayablesInProfitCurrency) != 0);
                    bool hasReceivableAmount = (Convert.ToDouble(record.ReceivablesInLocalCurrency) != 0 && Convert.ToDouble(record.ReceivablesInProfitCurrency) != 0);

                    if (hasPayableAmount || hasReceivableAmount)
                    {
                        provider.ChargeTypesList.Add(record);
                    }
                }

                foreach (ProfitDetailsClass item in receivablesGroupByList)
                {
                    ProfitDetailsClass record = new ProfitDetailsClass();
                    record.ChargeTypeId = item.ChargeTypeId;
                    record.ChargeTypeCode = item.ChargeTypeCode;
                    record.ChargeTypeName = item.ChargeTypeName;
                    record.IsExpenseCharge = item.IsExpenseCharge;
                    record.ReceivablesInLocalCurrency = item.ReceivablesInLocalCurrency;
                    record.ReceivablesInProfitCurrency = item.ReceivablesInProfitCurrency;
                    record.ProfitInLocalCurrency = item.ReceivablesInLocalCurrency;
                    record.ProfitInProfitCurrency = item.ReceivablesInProfitCurrency;

                    string varVatTypeCode = String.Empty;
                    string varVatTypeName = String.Empty;
                    string varVatTypePercentage = String.Empty;
                    ChargesType chargesType = ChargesTypeRepository.GetSingleChargesType(item.ChargeTypeId, shipmentPM.Tenant, true);
                    if (chargesType != null)
                    {
                        VatTypePM vatTypePM = vatTypeQuery.GetSinglePM(chargesType.VatTypeId, chargesType.Tenant);
                        if (vatTypePM != null)
                        {
                            varVatTypeCode = vatTypePM.Code;
                            varVatTypeName = vatTypePM.EnglishName;

                            if (vatTypePM.VatTypePercentages.Count > 0)
                            {
                                VatTypePercentagePM vatTypePercentagePM = vatTypePM.VatTypePercentages.OrderByDescending(d => d.FromDate).FirstOrDefault();
                                if (vatTypePercentagePM != null)
                                {
                                    varVatTypePercentage = vatTypePercentagePM.Percentage == null ? String.Empty : String.Format("{0:#,0.00}", vatTypePercentagePM.Percentage);
                                }
                            }
                        }
                    }

                    record.VatTypeCode = varVatTypeCode;
                    record.VatTypeName = varVatTypeName;
                    record.VatTypePercentage = varVatTypePercentage;
                    provider.ChargeTypesList.Add(record);
                }
                #endregion

                #region Other Fields
                provider.Notes = ServiceStringConvertor(shipmentPM.Notes);
                provider.ShipmentNumber = ServiceStringConvertor(shipmentPM.ShipmentNumber);
                provider.DescriptionOfGoods = ServiceStringConvertor(shipmentPM.DescriptionOfGoods);
                provider.Direction = context.Directions.Where(d => d.Id == shipmentPM.DirectionId).FirstOrDefault().Name;
                provider.ATD = ServiceDateConvertor(shipmentPM.MainCarriageATD);
                provider.ATA = ServiceDateConvertor(shipmentPM.MainCarriageATA);
                provider.ATD_DateTime = shipmentPM.MainCarriageATD;
                provider.ATA_DateTime = shipmentPM.MainCarriageATA;
                provider.BranchName = shipmentPM.BranchName;

                if (currentUser != null)
                {
                    provider.CurrentUser = currentUser;
                }
                else
                {
                    string loggedUserEmail = AuthenticationUtil.GetLoggedUserEmail(tenant);
                    if (!string.IsNullOrEmpty(loggedUserEmail))
                    {
                        Contact currentContact = (from a in commonContext.Contacts
                                                  where a.Email == loggedUserEmail && a.Tenant == tenant
                                                  select a).FirstOrDefault();

                        if (currentContact != null)
                        {
                            provider.CurrentUser = currentContact.EnglishName;
                        }
                    }
                }
                
                if (shipmentPM.TransportModeId == "A")
                {
                    provider.MasterNumber = shipmentPM.LongMaster;
                }
                else
                {
                    provider.MasterNumber = shipmentPM.Master;
                }
                #endregion

                #region TransportMode
                string varTransportMode = String.Empty;
                switch (shipmentPM.TransportModeId)
                {
                    case "A": { varTransportMode = "Air"; break; }
                    case "O": { varTransportMode = "Ocean"; break; }
                    case "I": { varTransportMode = "Inland"; break; }
                }
                provider.TransportMode = varTransportMode;
                #endregion

                #region Incoterm
                string varIncoterm = String.Empty;
                if (!string.IsNullOrEmpty(shipmentPM.IncotermId))
                {
                    IncotermRepository incotermRepository = new IncotermRepository(tenant);
                    Incoterm incoterm = incotermRepository.GetSingleIncoterm(shipmentPM.IncotermId, shipmentPM.Tenant);
                    if (incoterm != null)
                    {
                        varIncoterm = incoterm.Name;
                    }
                }
                provider.Incoterm = varIncoterm;
                #endregion

                #region Salseman
                string varSalseman = String.Empty;
                if (!string.IsNullOrEmpty(shipmentPM.SalesmanUserId))
                {
                    UserRepository userRepository = new UserRepository(tenant);
                    User user = userRepository.GetSingleUser(shipmentPM.SalesmanUserId, shipmentPM.Tenant, true);
                    if (user != null)
                    {
                        varSalseman = user.Contact.EnglishName;
                    }
                }
                provider.Salesman = varSalseman;
                #endregion

                #region Partners

                provider.AgentName = ServiceStringConvertor(shipmentPM.AgentName);
                provider.AgentRef1 = ServiceStringConvertor(shipmentPM.AgentReference1);
                provider.AgentRef2 = ServiceStringConvertor(shipmentPM.AgentReference2);

                provider.ShipperName = ServiceStringConvertor(shipmentPM.ShipperName);
                provider.ConsigneeName = ServiceStringConvertor(shipmentPM.ConsigneeName);

                provider.ShipperReference1 = shipmentPM.ShipperReference1;
                provider.ShipperReference2 = shipmentPM.ShipperReference2;

                string varAgentAddress = String.Empty;
                string varShipperAddress = String.Empty;
                string varConsigneeAddress = String.Empty;

                if (!string.IsNullOrEmpty(shipmentPM.AgentAddressId))
                {
                    Address address = addressRepository.GetSingleAddress(shipmentPM.AgentAddressId, shipmentPM.Tenant);                    
                    varAgentAddress = DataProviders.General.GetAddress(address);
                }

                if (!string.IsNullOrEmpty(shipmentPM.ShipperAddressId))
                {
                    Address address = addressRepository.GetSingleAddress(shipmentPM.ShipperAddressId, shipmentPM.Tenant);
                    varShipperAddress = DataProviders.General.GetAddress(address);
                }

                if (!string.IsNullOrEmpty(shipmentPM.ConsigneeAddressId))
                {
                    Address address = addressRepository.GetSingleAddress(shipmentPM.ConsigneeAddressId, shipmentPM.Tenant);
                    varConsigneeAddress = DataProviders.General.GetAddress(address);
                }

                provider.AgentAddress = varAgentAddress;
                provider.ShipperAddress = varShipperAddress;
                provider.ConsigneeAddress = varConsigneeAddress;

                #endregion

                #region Lables

                string fromLabel = String.Empty;
                string toLable = String.Empty;
                string carrierLable = String.Empty;
                string carrierNumber = String.Empty;
                string carrierNumberLabel = String.Empty;
                switch (shipmentPM.TransportModeId)
                {
                    case "A":
                        {
                            fromLabel = "Gateway";
                            toLable = "Destination";
                            carrierLable = "Airline";
                            carrierNumberLabel = "Flight No";
                            carrierNumber = shipmentPM.MainCarriageCarrierCode + shipmentPM.MainCarriageCarrierNumber;
                            break;
                        }

                    case "O":
                        {
                            fromLabel = "Loading Port";
                            toLable = "Discharge Port";
                            carrierLable = "Shipping line";
                            carrierNumberLabel = "Vessel/Voyage No";

                            string vesselName = String.Empty;
                            if (!string.IsNullOrEmpty(shipmentPM.MainCarriageVesselId))
                            {
                                VesselRepository vesselRepository = new VesselRepository(tenant);
                                Vessel vessel = vesselRepository.GetSingleVessel(shipmentPM.MainCarriageVesselId, tenant);
                                if (vessel != null)
                                {
                                    vesselName = vessel.EnglishName;
                                }
                            }

                            if (!string.IsNullOrEmpty(vesselName))
                            {
                                carrierNumber = vesselName + "/" + shipmentPM.MainCarriageCarrierCode + shipmentPM.MainCarriageCarrierNumber;
                            }
                            else
                            {
                                carrierNumber = shipmentPM.MainCarriageCarrierCode + shipmentPM.MainCarriageCarrierNumber;

                            }
                            break;
                        }

                    case "I":
                        {
                            fromLabel = "From";
                            toLable = "To";
                            carrierLable = "Trucker";
                            carrierNumberLabel = "Trucker No";
                            carrierNumber = shipmentPM.MainCarriageCarrierCode + shipmentPM.MainCarriageCarrierNumber;
                            break;
                        }
                }
                #endregion

                #region Carrier
                provider.CarrierCode = ServiceStringConvertor(shipmentPM.MainCarriageCarrierCode);
                provider.CarrierName = ServiceStringConvertor(shipmentPM.MainCarriageCarrierName);
                provider.CarrierNumber = carrierNumber;
                provider.CarrierLable = carrierLable;
                provider.CarrierNumberLabel = carrierNumberLabel;
                #endregion

                #region From / To
                provider.POLCode = ServiceStringConvertor(shipmentPM.MainCarriageFromPortCode);
                provider.POLName = ServiceStringConvertor(shipmentPM.MainCarriageFromPortName);
                provider.POLLable = ServiceStringConvertor(fromLabel);
                provider.PODCode = ServiceStringConvertor(shipmentPM.MainCarriageToPortCode);
                provider.PODName = ServiceStringConvertor(shipmentPM.MainCarriageToPortName);
                provider.PODLable = ServiceStringConvertor(toLable);

                // *** Final Destination
                string varDestinationCode = String.Empty;
                string varDestinationName = String.Empty;
                if (!string.IsNullOrEmpty(shipmentPM.FinalDistenationPortId))
                {
                    PortPM port = PortQuery.GetSinglePort(shipmentPM.Tenant, shipmentPM.FinalDistenationPortId, true);
                    if (port != null)
                    {
                        varDestinationCode = port.Code;
                        varDestinationName = port.EnglishName;
                    }
                }
                provider.DestinationCode = varDestinationCode;
                provider.DestinationName = varDestinationName;
                #endregion

                #region Invoices
                string varInvoices = String.Empty;
                if (shipmentPM.ShipmentARInvoices != null)
                {
                    foreach (ShipmentARInvoicePM item in shipmentPM.ShipmentARInvoices)
                    {
                        if (!string.IsNullOrEmpty(item.InvoiceNumber))
                        {
                            varInvoices = varInvoices + item.InvoiceNumber + " , ";
                        }
                    }

                    if (!string.IsNullOrEmpty(varInvoices))
                    {
                        varInvoices = varInvoices + "(%)";
                        varInvoices = varInvoices.Replace(" , (%)", String.Empty);
                    }
                }
                provider.Invoices = varInvoices;
                #endregion

                #region Containers
                string varContainers = String.Empty;
                if (shipmentPM.ShipmentPackages != null)
                {
                    foreach (ShipmentPackagePM item in shipmentPM.ShipmentPackages)
                    {
                        if (!string.IsNullOrEmpty(item.ContainerNumber))
                        {
                            varContainers = varContainers + item.ContainerNumber + " , ";
                        }
                    }

                    if (!string.IsNullOrEmpty(varContainers))
                    {
                        varContainers = varContainers + "(%)";
                        varContainers = varContainers.Replace(" , (%)", String.Empty);
                    }
                }
                provider.Containers = varContainers;
                #endregion

                #region Orign PickUp
                string varOrign = String.Empty;
                List<ShipmentPickUpPM> shipmentPickUps = shipmentPM.ShipmentPickUps.ToList();
                if (shipmentPickUps.Count > 0)
                {
                    string removalStr = shipmentPM.ShipmentNumber + "/";
                    List<string> pickups = new List<string>();
                    foreach (ShipmentPickUpPM item in shipmentPickUps)
                    {
                        string pickIndex = item.PickUpDeliveryNumber.Replace(removalStr, String.Empty);
                        pickups.Add(pickIndex);
                    }

                    pickups.Sort();
                    string originPickNumber = removalStr + pickups.First();

                    ShipmentPickUpPM originPickUp = shipmentPM.ShipmentPickUps.Where(d => d.PickUpDeliveryNumber == originPickNumber).FirstOrDefault();
                    if (originPickUp != null)
                    {
                        switch (originPickUp.PickUpDeliveryFromTypeCode.ToUpper())
                        {
                            case "PART":
                                {
                                    PartnersDomainService partnersDomainService = new PartnersDomainService();
                                    AddressPM addr1 = partnersDomainService.GetMainAddressByCardId(originPickUp.FromPartnerCardId, originPickUp.Tenant);
                                    if (addr1 != null)
                                    {
                                        varOrign = addr1.City;
                                    }
                                    break;
                                }
                            case "PORT":
                                {
                                    varOrign = originPickUp.FromPortCode;
                                    break;
                                }

                            default: { break; }
                        }
                    }
                }
                provider.Orign = varOrign;
                #endregion

                #region Inland + Domestic shipment
                if (shipmentPM.DirectionId == "D" && shipmentPM.TransportModeId == "I")
                {
                    Address fromAddress = addressRepository.GetSingleAddress(shipmentPM.MainCarriageFromAddressId,tenant);
                    Address toAddress = addressRepository.GetSingleAddress(shipmentPM.MainCarriageToAddressId,tenant);
                  

                    if (fromAddress != null)
                    {
                        provider.FromLocation = fromAddress.City + " " + (fromAddress.Country != null ? fromAddress.Country.Code : "");
                    }

                    if (toAddress != null)
                    {
                        provider.ToLocation = toAddress.City + " " + (toAddress.Country != null ? toAddress.Country.Code : "");
                    }
                }
                else
                {
                    provider.FromLocation = ServiceStringConvertor(shipmentPM.MainCarriageFromPortCode) + " " + ServiceStringConvertor(shipmentPM.MainCarriageFromPortName);
                    provider.ToLocation = ServiceStringConvertor(shipmentPM.MainCarriageToPortCode) + " " + ServiceStringConvertor(shipmentPM.MainCarriageToPortName);
                }
                #endregion

                #region Move Type
                if (!string.IsNullOrEmpty(shipmentPM.MoveTypeId))
                {
                    MoveType moveType = context.MoveTypes.Where(m => m.Id == shipmentPM.MoveTypeId).FirstOrDefault();
                
                    if(moveType != null)
                    {
                        provider.MoveTypeCode = moveType.Code;
                        provider.MoveTypeName = moveType.MoveTypeEnglishName;
                    }
                }
                #endregion

                CustomFieldResolver customFieldResolver = new CustomFieldResolver();
                customFieldResolver.SetDataProviderCustomFieldsValues("Shipment", tenant, shipmentPM, provider);
            }

            return provider;
        }

        private ShipmentProfitInvoicesDataProvider BuildProfitInvoicesProvider(string shipmentId, int tenant, string accountingCurrencyId, string currentUser)
        {
            ShipmentProfitInvoicesDataProvider provider = new ShipmentProfitInvoicesDataProvider();

            ShipmentRepository shipmentRepository = new ShipmentRepository(tenant);

            Shipment shipment = shipmentRepository.GetSingleShipment(shipmentId, tenant);

            if (shipment != null)
            {
                provider.ShipmentNumber = string.IsNullOrEmpty(shipment.ShipmentNumber) ? "" : shipment.ShipmentNumber;

                IncotermRepository incotermRepository = new IncotermRepository(tenant);
                Incoterm incoterm = incotermRepository.GetSingleIncoterm(shipment.IncotermId,tenant);
                if (incoterm != null)
                {
                    provider.Incoterm = incoterm.Name;
                }

                #region Activity
                string activity = "";

                DirectionRepository directionRepository = new DirectionRepository(tenant);
                Direction direction = directionRepository.GetSingleDirection(shipment.DirectionId);
                if (direction != null)
                {
                    activity = direction.Name;
                }

                TransportModeRepository transportModeRepository = new TransportModeRepository(tenant);
                TransportMode transportMode = transportModeRepository.GetSingleTransportMode(shipment.TransportModeId);
                if (transportMode != null)
                {
                    activity = string.IsNullOrEmpty(activity) ? transportMode.Name : activity + " " + transportMode.Name;
                }

                provider.Activity = activity;
                #endregion

                #region Shipper & Consignee
                Card shipperCard = CardRepository.GetSingleCard(shipment.ShipperId, tenant, false);
                if (shipperCard != null)
                {
                    provider.Shipper = string.IsNullOrEmpty(shipperCard.EnglishName) ? "" : shipperCard.EnglishName;
                }

                Card consigneeCard = CardRepository.GetSingleCard(shipment.ConsigneeId, tenant, false);
                if (consigneeCard != null)
                {
                    provider.Consignee = string.IsNullOrEmpty(consigneeCard.EnglishName) ? "" : consigneeCard.EnglishName;
                }
                #endregion

                #region Currencies
                CurrencyRepository currencyRepository = new CurrencyRepository(tenant);
                TenantRepository tenantRepository = new TenantRepository(tenant);
                Tenant tenantEntity = tenantRepository.GetSingleTenant(tenant);

                Currency localCurrency = currencyRepository.GetSingleCurrency(accountingCurrencyId, tenant);
                if (localCurrency == null)
                {
                    if (tenantEntity != null)
                    {
                        localCurrency = currencyRepository.GetSingleCurrency(tenantEntity.CurrencyId, tenant);
                    }
                }

                if (localCurrency != null)
                {
                    provider.LocalCurrencyCode = localCurrency.Code;
                }

                Currency profitCurrency = currencyRepository.GetSingleCurrency(shipment.ProfitCurrencyId, tenant);
                if (profitCurrency == null)
                {
                    if (tenantEntity != null)
                    {
                        profitCurrency = currencyRepository.GetSingleCurrency(tenantEntity.ProfitCurrencyId, tenant);
                    }
                }

                if (profitCurrency != null)
                {
                    provider.ProfitCurrencyCode = profitCurrency.Code;
                }
                #endregion

                #region Routings

                PortRepository portRepository = new PortRepository(tenant);
                CountryRepository countryRepository = new CountryRepository(tenant);
                AddressRepository addressRepository = new AddressRepository(tenant);

                bool isInlandDomestic = shipment.DirectionId == "D" && shipment.TransportModeId == "I";
                ShipmentMasterData masterData = shipmentRepository.GetSingleShipmentMasterData(shipment.MasterShipmentDataId, tenant);

                if (isInlandDomestic)
                {
                    Address originAddress = addressRepository.GetSingleAddress(masterData.MainCarriageFromAddressId, tenant);
                    if (originAddress != null)
                    {
                        provider.OriginLocation = originAddress.City;

                        Country country = countryRepository.GetSingleCountry(originAddress.CountryId, tenant);
                        if (country != null)
                        {
                            provider.OriginCountryCode = country.Code;
                            provider.OriginCountryName = country.EnglishName;
                        }
                    }

                    Address destinationAddress = addressRepository.GetSingleAddress(masterData.MainCarriageToAddressId, tenant);
                    if (destinationAddress != null)
                    {
                        provider.DestinationLocation = destinationAddress.City;

                        Country country = countryRepository.GetSingleCountry(destinationAddress.CountryId, tenant);
                        if (country != null)
                        {
                            provider.DestinationCountryCode = country.Code;
                            provider.DestinationCountryName = country.EnglishName;
                        }
                    }

                    provider.POLLocation = provider.OriginLocation;
                    provider.POLCountryCode = provider.OriginCountryCode;
                    provider.POLCountryName = provider.OriginCountryName;

                    provider.PODLocation = provider.DestinationLocation;
                    provider.PODCountryCode = provider.DestinationCountryCode;
                    provider.PODCountryName = provider.DestinationCountryName;

                    DateTime? polATD = masterData == null ? null : masterData.MainCarriageATD;
                    DateTime? podATA = masterData == null ? null : (!string.IsNullOrEmpty(masterData.Transshipment3ToPortId) ? masterData.Transshipment3ATA : (!string.IsNullOrEmpty(masterData.Transshipment2ToPortId) ? masterData.Transshipment2ATA : (!string.IsNullOrEmpty(masterData.Transshipment1ToPortId) ? masterData.Transshipment1ATA : masterData.MainCarriageATA)));
                    provider.POLATD = polATD;
                    provider.PODATA = podATA;
                }

                else
                {
                    string polPortId = masterData == null ? shipment.FromPortId : masterData.MainCarriageFromPortId;
                    string podPortId = masterData == null ? shipment.ToPortId : (!string.IsNullOrEmpty(masterData.Transshipment3ToPortId) ? masterData.Transshipment3ToPortId : (!string.IsNullOrEmpty(masterData.Transshipment2ToPortId) ? masterData.Transshipment2ToPortId : (!string.IsNullOrEmpty(masterData.Transshipment1ToPortId) ? masterData.Transshipment1ToPortId : masterData.MainCarriageToPortId)));

                    DateTime? polATD = masterData == null ? null : masterData.MainCarriageATD;
                    DateTime? podATA = masterData == null ? null : (!string.IsNullOrEmpty(masterData.Transshipment3ToPortId) ? masterData.Transshipment3ATA : (!string.IsNullOrEmpty(masterData.Transshipment2ToPortId) ? masterData.Transshipment2ATA : (!string.IsNullOrEmpty(masterData.Transshipment1ToPortId) ? masterData.Transshipment1ATA : masterData.MainCarriageATA)));
                    provider.POLATD = polATD;
                    provider.PODATA = podATA;

                    Port polPort = portRepository.GetSinglePort(tenant, polPortId);
                    if (polPort != null)
                    {
                        provider.POLLocation = polPort.EnglishName + " - " + polPort.Code;
                        provider.OriginLocation = polPort.EnglishName + " - " + polPort.Code;

                        Country country = countryRepository.GetSingleCountry(polPort.CountryId, tenant);
                        if (country != null)
                        {
                            provider.POLCountryCode = country.Code;
                            provider.OriginCountryCode = country.Code;
                            provider.POLCountryName = country.EnglishName;
                            provider.OriginCountryName = country.EnglishName;
                        }
                    }

                    Port podPort = portRepository.GetSinglePort(tenant, podPortId);
                    if (podPort != null)
                    {
                        provider.PODLocation = podPort.EnglishName + " - " + podPort.Code;
                        provider.DestinationLocation = podPort.EnglishName + " - " + podPort.Code;

                        Country country = countryRepository.GetSingleCountry(podPort.CountryId, tenant);
                        if (country != null)
                        {
                            provider.PODCountryCode = country.Code;
                            provider.DestinationCountryCode = country.Code;
                            provider.PODCountryName = country.EnglishName;
                            provider.DestinationCountryName = country.EnglishName;
                        }
                    }

                    if (shipment.ShipmentLevelCode == "H")
                    {
                        if (!string.IsNullOrEmpty(shipment.PreForwardingFromPortId) && string.IsNullOrEmpty(shipment.PreForwardingToPortId))
                        {
                            Port port = portRepository.GetSinglePort(tenant, shipment.PreForwardingFromPortId);
                            if (port != null)
                            {
                                provider.OriginLocation = port.EnglishName + " - " + port.Code;

                                Country country = countryRepository.GetSingleCountry(port.CountryId, tenant);
                                if (country != null)
                                {
                                    provider.OriginCountryCode = country.Code;
                                    provider.OriginCountryName = country.EnglishName;
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(shipment.OnForwardingFromPortId) && string.IsNullOrEmpty(shipment.OnForwardingToPortId))
                        {
                            Port port = portRepository.GetSinglePort(tenant, shipment.OnForwardingToPortId);
                            if (port != null)
                            {
                                provider.DestinationLocation = port.EnglishName + " - " + port.Code;

                                Country country = countryRepository.GetSingleCountry(port.CountryId, tenant);
                                if (country != null)
                                {
                                    provider.DestinationCountryCode = country.Code;
                                    provider.DestinationCountryName = country.EnglishName;
                                }
                            }
                        }
                    }

                    else
                    {
                        if (masterData != null)
                        {
                            if (!string.IsNullOrEmpty(masterData.PreCarriageFromPortId) && string.IsNullOrEmpty(masterData.PreCarriageToPortId))
                            {
                                Port port = portRepository.GetSinglePort(tenant, masterData.PreCarriageFromPortId);
                                if (port != null)
                                {
                                    provider.OriginLocation = port.EnglishName + " - " + port.Code;

                                    Country country = countryRepository.GetSingleCountry(port.CountryId, tenant);
                                    if (country != null)
                                    {
                                        provider.OriginCountryCode = country.Code;
                                        provider.OriginCountryName = country.EnglishName;
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(masterData.OnCarriageFromPortId) && string.IsNullOrEmpty(masterData.OnCarriageToPortId))
                            {
                                Port port = portRepository.GetSinglePort(tenant, masterData.OnCarriageToPortId);
                                if (port != null)
                                {
                                    provider.DestinationLocation = port.EnglishName + " - " + port.Code;

                                    Country country = countryRepository.GetSingleCountry(port.CountryId, tenant);
                                    if (country != null)
                                    {
                                        provider.DestinationCountryCode = country.Code;
                                        provider.DestinationCountryName = country.EnglishName;
                                    }
                                }
                            }
                        }
                    }                    
                }
                #endregion

                #region Containers

                if (MethodHelper.IsLCLEntity(shipment.TransportModeId, shipment.ShipmentTypeId))
                {
                    provider.ContainersLabel = "Packages";
                    provider.ContainersValue = shipment.NumberOfPackages == null ? "" : shipment.NumberOfPackages.ToString();
                }

                else
                {
                    provider.ContainersLabel = "Containers";

                    string totalContainers = "";
       
                    ShipmentPackageQuery shipmentPackageQuery = new ShipmentPackageQuery(tenant);
                    List<ShipmentPackagePM> shipmentPackages = shipmentPackageQuery.GetShipmentPackages(shipmentId, shipment.ShipmentNumber, tenant);

                    if (shipmentPackages != null)
                    {
                        PackageTypeRepository packageTypeRepository = new PackageTypeRepository(tenant);
                        IEnumerable<IGrouping<string, ShipmentPackagePM>> groupingList = shipmentPackages.Where(p => p.IsContainer == true).GroupBy(s => s.PackageTypeName);

                        foreach (IGrouping<string, ShipmentPackagePM> list in groupingList)
                        {
                            string container = "";
                            string count = list.Count().ToString();

                            string containerTypeCode = string.Empty;

                            if (!string.IsNullOrEmpty(list.FirstOrDefault().PackageTypeId))
                            {
                                PackageType packType = packageTypeRepository.GetSinglePackageType(list.FirstOrDefault().PackageTypeId, tenant);
                                containerTypeCode = packType == null ? "" : packType.PrintAs;
                            }
   
                            container = count + " x " + containerTypeCode;
                            totalContainers = totalContainers + ", " + container;
                        }

                        totalContainers = totalContainers.TrimStart(',').Trim();                      
                    }

                   provider.ContainersValue = totalContainers;

                }
                #endregion

                provider.PayableInvoices = new List<PayableInvoiceProvider>();
                provider.ReceivableInvoices = new List<ReceivableInvoiceProvider>();
                APInvoiceRepository aPInvoiceRepository = new APInvoiceRepository(tenant);
                APInvoiceQuery aPInvoiceQuery = new APInvoiceQuery(aPInvoiceRepository);
                ARInvoiceRepository aRInvoiceRepository = new ARInvoiceRepository(tenant);
                APInvoiceEntityRepository aPInvoiceEntityRepository = new APInvoiceEntityRepository(tenant);
                ARInvoiceEntityRepository aRInvoiceEntityRepository = new ARInvoiceEntityRepository(tenant);
                APInvoiceStatusRepository aPInvoiceStatusRepository = new APInvoiceStatusRepository(tenant);
                ARInvoiceStatusRepository aRInvoiceStatusRepository = new ARInvoiceStatusRepository(tenant);
                List<APInvoiceEntity> aPInvoiceEntities = aPInvoiceEntityRepository.GetInvoiceEntitiesbyEntityId(shipmentId, tenant).ToList();
                List<ARInvoiceEntity> aRInvoiceEntities = aRInvoiceEntityRepository.GetInvoiceEntitiesForEntity(shipmentId, tenant).ToList();

                foreach (APInvoiceEntity item in aPInvoiceEntities)
                {
                    APInvoicePM invoice = aPInvoiceQuery.GetSinglePM(item.APInvoiceId, tenant);

                    if (invoice != null)
                    {
                        PayableInvoiceProvider invoiceProvider = new PayableInvoiceProvider()
                        {
                            InvoiceNumber = invoice.InvoiceNumber
                        };

                        if (invoice.IsMultipleEntities) {
                            invoiceProvider.AmountInLocalCurrency = invoice.InvoiceMultipleShipments.Where(p => p.ShipmentId == shipment.Id).Sum(s => s.SubTotalInLocalCurrency) == null ? 0 : invoice.InvoiceMultipleShipments.Where(p => p.ShipmentId == shipment.Id).Sum(s => s.SubTotalInLocalCurrency.Value);
                            invoiceProvider.AmountInProfitCurrency = invoice.InvoiceMultipleShipments.Where(p => p.ShipmentId == shipment.Id).Sum(s => s.SubTotalInInvoiceCurrency) == null ? 0 : invoice.InvoiceMultipleShipments.Where(p => p.ShipmentId == shipment.Id).Sum(s => s.SubTotalInInvoiceCurrency.Value);
                         }

                        else
                        {
                            invoiceProvider.AmountInLocalCurrency = invoice.AmountInLocalCurrency == null ? 0 : invoice.AmountInLocalCurrency.Value;
                            invoiceProvider.AmountInProfitCurrency = invoice.AmountInProfitCurrency == null ? 0 : invoice.AmountInProfitCurrency.Value;
                
                        }

                        Card partnerCard = CardRepository.GetSingleCard(invoice.VendorId, tenant, false);
                        if (partnerCard != null)
                        {
                            invoiceProvider.Vendor = partnerCard.EnglishName;
                        }

                        APInvoiceStatus status = aPInvoiceStatusRepository.GetSingleAPInvoiceStatus(invoice.StatusCode);
                        if (status != null)
                        {
                            invoiceProvider.Status = status.Name;
                        }

                        provider.PayableInvoices.Add(invoiceProvider);
                    }
                }

                foreach (ARInvoiceEntity item in aRInvoiceEntities)
                {
                    ARInvoice invoice = aRInvoiceRepository.GetSingleInvoice(item.ARInvoiceId);

                    if (invoice != null)
                    {
                        ReceivableInvoiceProvider invoiceProvider = new ReceivableInvoiceProvider()
                        {
                            InvoiceNumber = invoice.InvoiceNumber,
                            AmountInLocalCurrency = invoice.AmountInLocalCurrency == null ? 0 : invoice.AmountInLocalCurrency.Value,
                            AmountInProfitCurrency = invoice.AmountInProfitCurrency == null ? 0 : invoice.AmountInProfitCurrency.Value,
                        };

                        Card partnerCard = CardRepository.GetSingleCard(invoice.BillToId, tenant, false);
                        if (partnerCard != null)
                        {
                            invoiceProvider.BillTo = partnerCard.EnglishName;
                        }

                        ARInvoiceStatus status = aRInvoiceStatusRepository.GetSingleARInvoiceStatus(invoice.StatusCode);
                        if (status != null)
                        {
                            invoiceProvider.Status = status.Name;
                        }

                        provider.ReceivableInvoices.Add(invoiceProvider);
                    }
                }

                // Profit
                provider.PayableAmountInLocalCurrency = provider.PayableInvoices.Sum(s => s.AmountInLocalCurrency);
                provider.PayableAmountInProfitCurrency = provider.PayableInvoices.Sum(s => s.AmountInProfitCurrency);

                provider.ReceivableAmountInLocalCurrency = provider.ReceivableInvoices.Sum(s => s.AmountInLocalCurrency);
                provider.ReceivableAmountInProfitCurrency = provider.ReceivableInvoices.Sum(s => s.AmountInProfitCurrency);

                provider.ProfitAmountInLocalCurrency = provider.ReceivableAmountInLocalCurrency - provider.PayableAmountInLocalCurrency;
                provider.ProfitAmountInProfitCurrency = provider.ReceivableAmountInProfitCurrency - provider.PayableAmountInProfitCurrency;

                //Logo
                provider.Logo = DataProviders.General.GetLogo(tenant);
            }

            return provider;
        }

        private string ServiceStringConvertor(string str)
        {
            return string.IsNullOrEmpty(str) ? String.Empty : str;
        }

        private string ServiceDateConvertor(DateTime? date)
        {
            string result = String.Empty;
            if (date != null)
            {
                DateTime dateTime = (DateTime)date;
                if (dateTime.Date == DateTime.Today.Date)
                {
                    result = "Today";
                }

                else if (dateTime.Date == DateTime.Today.Date.AddDays(-1))
                {
                    result = "Yesterday";
                }

                else if (dateTime.Date == DateTime.Today.Date.AddDays(1))
                {
                    result = "Tomorrow";
                }

                else
                {
                    result = dateTime.ToString("d", CultureInfo.CurrentCulture);
                }
            }

            return result;
        }
    }
}
