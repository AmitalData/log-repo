using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports
{
    public class VendorChargesAnalysisManager
    {
        private int tenant;
        private string VendorId = null;
        private string DateType = null;
        private DateTime FromDate;
        private DateTime ToDate;
        private string ChargesTypeId = null;
        private bool IncludeAccountedOnly = false;//
        private bool IsLocalCurrency = false;//
        private string SelectedCurrencyCode = null;//
        private string OperationalType = null;
        private string AccountingType = null;
        private string Direction = null;
        private string TransportMode = null;
        private string ShipmentNumber = null;
        private IShipmentsContext shipmentsContext;
        private AddressRepository addressRepository;
        private PortRepository portRepository;
        private WebServiceHelper webServiceHelper;
        public VendorChargesAnalysisManager(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime myStartDate = todayDate.AddMonths(-1);

            FromDate = new DateTime(myStartDate.Year, myStartDate.Month, 1);
            ToDate = new DateTime(todayDate.Year, todayDate.Month, DateTime.DaysInMonth(todayDate.Year, todayDate.Month));

            MemoryStream memoryStream = new MemoryStream(xmlFilters);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations myQueryOperations = (QueryOperations)xmlSerializer.Deserialize(memoryStream);

            QueryFilterItem filterItem_VendorId = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "VendorId").FirstOrDefault();
            QueryFilterItem filterItem_DateType = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "DateType").FirstOrDefault();
            QueryFilterItem filterItem_FromDate = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
            QueryFilterItem filterItem_ChargesTypeId = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ChargesTypeId").FirstOrDefault();
            QueryFilterItem filterItem_IncludeAccountedOnly = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "IncludeAccountedOnly").FirstOrDefault();
            QueryFilterItem filterItem_IsLocalCurrency = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "IsLocalCurrency").FirstOrDefault();
            QueryFilterItem filterItem_SelectedCurrencyCode = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "SelectedCurrencyCode").FirstOrDefault();
            QueryFilterItem filterItem_OperationalType = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "OperationalType").FirstOrDefault();
            QueryFilterItem filterItem_AccountingType = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "AccountingType").FirstOrDefault();
            QueryFilterItem filterItem_Direction = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "Direction").FirstOrDefault();
            QueryFilterItem filterItem_TransportMode = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "TransportMode").FirstOrDefault();
            QueryFilterItem filterItem_ShipmentNumber = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ShipmentNumber").FirstOrDefault();

            if (filterItem_VendorId != null)
            {
                if (filterItem_VendorId.FieldValue != null)
                {
                    VendorId = filterItem_VendorId.FieldValue.ToString();
                }
            }

            if (filterItem_DateType != null)
            {
                if (filterItem_DateType.FieldValue != null)
                {
                    DateType = filterItem_DateType.FieldValue.ToString();
                }
            }

            if (filterItem_FromDate != null)
            {
                DateTime.TryParse(filterItem_FromDate.FieldValue.ToString(), out FromDate);
            }

            if (filterItem_ToDate != null)
            {
                DateTime.TryParse(filterItem_ToDate.FieldValue.ToString(), out ToDate);
            }

            if (filterItem_ChargesTypeId != null)
            {
                if (filterItem_ChargesTypeId.FieldValue != null)
                {
                    ChargesTypeId = filterItem_ChargesTypeId.FieldValue.ToString();
                }
            }

            if (filterItem_IncludeAccountedOnly != null)
            {
                if (filterItem_IncludeAccountedOnly.FieldValue != null)
                {
                    IncludeAccountedOnly = (bool)filterItem_IncludeAccountedOnly.FieldValue;
                }
            }

            if (filterItem_IsLocalCurrency != null)
            {
                if (filterItem_IsLocalCurrency.FieldValue != null)
                {
                    IsLocalCurrency = (bool)filterItem_IsLocalCurrency.FieldValue;
                }
            }

            if (filterItem_SelectedCurrencyCode != null)
            {
                if (filterItem_SelectedCurrencyCode.FieldValue != null)
                {
                    SelectedCurrencyCode = filterItem_SelectedCurrencyCode.FieldValue.ToString();
                }
            }

            if (filterItem_OperationalType != null)
            {
                if (filterItem_OperationalType.FieldValue != null)
                {
                    OperationalType = filterItem_OperationalType.FieldValue.ToString();
                }
            }

            if (filterItem_AccountingType != null)
            {
                if (filterItem_AccountingType.FieldValue != null)
                {
                    AccountingType = filterItem_AccountingType.FieldValue.ToString();
                }
            }

            if (filterItem_Direction != null)
            {
                if (filterItem_Direction.FieldValue != null)
                {
                    Direction = filterItem_Direction.FieldValue.ToString();
                }
            }

            if (filterItem_TransportMode != null)
            {
                if (filterItem_TransportMode.FieldValue != null)
                {
                    TransportMode = filterItem_TransportMode.FieldValue.ToString();
                }
            }

            if (filterItem_ShipmentNumber != null)
            {
                if (filterItem_ShipmentNumber.FieldValue != null)
                {
                    ShipmentNumber = filterItem_ShipmentNumber.FieldValue.ToString();
                }
            }
        }

        public byte[] GetData()
        {
            VendorChargesAnalysisDataProvider myDataProvider = this.LoadDataProvider();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(VendorChargesAnalysisDataProvider));
            MemoryStream memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream, myDataProvider);
            memoryStream.Seek(0, SeekOrigin.Begin);

            StreamReader streamReader = new StreamReader(memoryStream);
            string content = streamReader.ReadToEnd();
            byte[] bytearray = memoryStream.ToArray();
            return bytearray;
        }

        private VendorChargesAnalysisDataProvider LoadDataProvider()
        {
            VendorChargesAnalysisDataProvider myDataProvider = new VendorChargesAnalysisDataProvider();
            myDataProvider.Shipments = new List<VendorChargesShipment>();

            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            addressRepository = new AddressRepository(commonContext);
            portRepository = new PortRepository(commonContext);
            shipmentsContext = ShipmentsContext.GetContext(tenant);
            webServiceHelper = new WebServiceHelper(tenant);

            ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentsContext);
            ShipmentPayableRepository shipmentPayableRepository = new ShipmentPayableRepository(shipmentsContext);
            ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRepository);
            IQueryable<ShipmentList> iQueryable_shipments = shipmentQuery.GetAllShipmentListTenant(tenant);

            if (iQueryable_shipments.Count() > 0)
            {
                if (!string.IsNullOrEmpty(Direction) && Direction != "All")
                {
                    iQueryable_shipments = iQueryable_shipments.Where(d => d.DirectionId == Direction);
                }

                if (!string.IsNullOrEmpty(TransportMode) && TransportMode != "All")
                {
                    iQueryable_shipments = iQueryable_shipments.Where(d => d.TransportModeId == TransportMode);
                }

                if (!string.IsNullOrEmpty(ShipmentNumber))
                {
                    iQueryable_shipments = iQueryable_shipments.Where(d => d.ShipmentNumber == ShipmentNumber);
                }

                switch (DateType)
                {
                    case "CRT":
                        {
                            if (FromDate != null)
                            {
                                iQueryable_shipments = iQueryable_shipments.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) >= System.Data.Entity.DbFunctions.TruncateTime(FromDate));
                            }

                            if (ToDate != null)
                            {
                                iQueryable_shipments = iQueryable_shipments.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) <= System.Data.Entity.DbFunctions.TruncateTime(ToDate));
                            }

                            break;
                        }

                    case "OPE":
                        {
                            if (FromDate != null)
                            {
                                iQueryable_shipments = iQueryable_shipments.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.OperationalDate) >= System.Data.Entity.DbFunctions.TruncateTime(FromDate));
                            }

                            if (ToDate != null)
                            {
                                iQueryable_shipments = iQueryable_shipments.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.OperationalDate) <= System.Data.Entity.DbFunctions.TruncateTime(ToDate));
                            }

                            break;
                        }
                }

                switch (OperationalType)
                {
                    case "All":
                        {
                            break;
                        }

                    case "Open":
                        {
                            iQueryable_shipments = iQueryable_shipments.Where(d => !d.IsOperationalClosed);
                            break;
                        }

                    case "Close":
                        {
                            iQueryable_shipments = iQueryable_shipments.Where(d => d.IsOperationalClosed);
                            break;
                        }
                }

                switch (AccountingType)
                {
                    case "All":
                        {
                            break;
                        }

                    case "Open":
                        {
                            iQueryable_shipments = iQueryable_shipments.Where(d => !d.IsAccountingClosed);
                            break;
                        }

                    case "Close":
                        {
                            iQueryable_shipments = iQueryable_shipments.Where(d => d.IsAccountingClosed);
                            break;
                        }
                }

                iQueryable_shipments = iQueryable_shipments.Where(d => (d.AccountedPayablesInLocalCurrency != null && d.AccountedPayablesInLocalCurrency != 0) || (d.OpenPayablesInLocalCurrency != null && d.OpenPayablesInLocalCurrency != 0));

                List<string> allShipmentIds = iQueryable_shipments.Select(s => s.Id).ToList();
                IQueryable<ShipmentPayable> iQueryable_payables = shipmentPayableRepository.GetShipmentPayablesByShipmentIds(allShipmentIds, tenant);

                if (iQueryable_payables != null && iQueryable_payables.Count() > 0)
                {
                    if (!string.IsNullOrEmpty(VendorId))
                    {
                        iQueryable_payables = iQueryable_payables.Where(d => d.VendorId == VendorId);
                    }

                    if (!string.IsNullOrEmpty(ChargesTypeId))
                    {
                        iQueryable_payables = iQueryable_payables.Where(d => d.ChargesTypeId == ChargesTypeId);
                    }

                    iQueryable_payables = iQueryable_payables.Where(d => (d.ExpectedAmountLocal != null && d.ExpectedAmountLocal != 0) || (d.AccountedAmountInLocalCurrency != null && d.AccountedAmountInLocalCurrency != 0));

                    List<string> myShipmentIds = iQueryable_payables.Select(s => s.ShipmentId).ToList();
                    iQueryable_shipments = iQueryable_shipments.Where(d => myShipmentIds.Contains(d.Id));

                    List<ShipmentsJoinPayablesList> myResult = (from myShipment in iQueryable_shipments
                                                                join myPayable in iQueryable_payables on myShipment.Id equals myPayable.ShipmentId into myShipmentPayable
                                                                join shipmentComputed in shipmentsContext.ShipmentComputedFields
                                                                on new { Id = myShipment.Id }
                                                                equals new { Id = shipmentComputed.Id }
                                                                from myItem in myShipmentPayable.DefaultIfEmpty()
                                                                select new ShipmentsJoinPayablesList()
                                                                {
                                                                    MainCarriageCarrierNumber = myShipment.MainCarriageCarrierNumber,
                                                                    ShipmentId = myShipment.Id,
                                                                    CreateDateTime = myShipment.CreateDateTime,
                                                                    FirstOpCloseDate = myShipment.FirstOperationalCloseDate,
                                                                    ShipmentNumber = myShipment.ShipmentNumber,
                                                                    CustomerName = myShipment.CustomerName,
                                                                    OpenedBy = myShipment.CreatedByUserName,
                                                                    Level = myShipment.ShipmentLevelName,
                                                                    ShipmentType = myShipment.ShipmentType,
                                                                    SpecialServices = myShipment.SpecialServicesTypeName,
                                                                    ChargeableWeightInKG = myShipment.ChargeableWeightInKG,
                                                                    ChargeableWeight = myShipment.ChargeableWeightInKG,
                                                                    VolumeInCBM = myShipment.VolumeInCBM,
                                                                    PreCarriageFromPortId = myShipment.PreCarriageFromPortId,
                                                                    PreForwardingFromPortId = myShipment.PreForwardingFromPortId,
                                                                    MainCarriageFromPortId = myShipment.MainCarriageFromPortId,
                                                                    MainCarriageToPortId = myShipment.MainCarriageToPortId,
                                                                    OnCarriageToPortId = myShipment.OnCarriageToPortId,
                                                                    OnForwardingToPortId = myShipment.OnForwardingToPortId,
                                                                    Transshipment1ToPortId = myShipment.Transshipment1ToPortId,
                                                                    Transshipment2ToPortId = myShipment.Transshipment2ToPortId,
                                                                    Transshipment3ToPortId = myShipment.Transshipment3ToPortId,
                                                                    VendorId = myItem.VendorId,
                                                                    VendorName = myItem.VendorCard == null ? null : myItem.VendorCard.EnglishName,
                                                                    ChargeTypeId = myItem.ChargesTypeId,
                                                                    ChargeTypeName = myItem.ChargesType == null ? null : myItem.ChargesType.EnglishName,
                                                                    Notes = myItem.Notes,
                                                                    Payables_OPEN = IsLocalCurrency ? myItem.OpenAmountInLocalCurrency : myItem.OpenAmountInProfitCurrency,
                                                                    Payables_ACCT = IsLocalCurrency ? myItem.AccountedAmountInLocalCurrency : myItem.AccountedAmountInProfitCurrency,
                                                                    FinalArrivalDate = myShipment.ActualFinalArrivalDate,
                                                                    TransportModeId = myShipment.TransportModeId,
                                                                    ContainersNumbersAndTypesArray = shipmentComputed.ContainersNumbersAndTypesArray,
                                                                    TruckNumber = myShipment.TruckNumber,
                                                                    DirectionId = myShipment.DirectionId,
                                                                }).ToList();
                    if (myResult.Count > 0)
                    {
                        foreach (ShipmentsJoinPayablesList item in myResult)
                        {
                            VendorChargesShipment myRecord = new VendorChargesShipment();
                            myRecord.CreateDate = item.CreateDateTime;
                            myRecord.FirstOpCloseDate = item.FirstOpCloseDate;
                            myRecord.ShipmentNumber = item.ShipmentNumber;
                            myRecord.CustomerName = item.CustomerName;
                            myRecord.OpenedBy = item.OpenedBy;
                            myRecord.SpecialServices = item.SpecialServices;
                            myRecord.ChargeableWeightInKG = item.ChargeableWeightInKG;
                            myRecord.ChargeableWeight = item.ChargeableWeight;
                            myRecord.VolumeInCBM = item.VolumeInCBM;
                            myRecord.CarrierId = item.VendorId;
                            myRecord.Carrier = item.VendorName;
                            myRecord.ChargesTypeId = item.ChargeTypeId;
                            myRecord.ChargesType = item.ChargeTypeName;
                            myRecord.FinalArrivalDate = item.FinalArrivalDate;
                            myRecord.TruckContainerNumber = this.GetContainerNumbers(item);
                            if (IncludeAccountedOnly)
                            {
                                myRecord.OpenAmount = null;
                            }
                            else
                            {
                                myRecord.OpenAmount = item.Payables_OPEN;
                            }

                            myRecord.AccountedAmount = item.Payables_ACCT;
                            myRecord.Notes = item.Notes;

                            if (!string.IsNullOrEmpty(item.ShipmentType))
                            {
                                myRecord.Type = item.ShipmentType + " " + item.Level;
                            }

                            else
                            {
                                myRecord.Type = item.Level;
                            }

                            this.ComputeFromLocationProperties(myRecord, item);
                            this.ComputeToLocationProperties(myRecord, item);

                            myDataProvider.Shipments.Add(myRecord);
                        }
                    }
                }
            }

            return myDataProvider;
        }

        private string GetContainerNumbers(ShipmentsJoinPayablesList item)
        {
            var containerNumbers = "";
            bool isInlandDomesticShipment = (item.DirectionId == "D" && item.TransportModeId == "I");
            if (item.TransportModeId == "O")
            {
                if (!string.IsNullOrEmpty(item.ContainersNumbersAndTypesArray))
                {
                    containerNumbers = Regex.Replace(item.ContainersNumbersAndTypesArray, "(\\[.*?\\])", "");
                }
            }

            else
            {
                containerNumbers = isInlandDomesticShipment ? item.TruckNumber : item.MainCarriageCarrierNumber;
            }

            return containerNumbers;
        }

        private void ComputeToLocationProperties(VendorChargesShipment myRecord, ShipmentsJoinPayablesList item)
        {
            ShipmentPickUpDelivery myLastDelivery = (from d in shipmentsContext.ShipmentPickUpDeliveries
                                                     where d.ShipmentId == item.ShipmentId && d.PickUpDeliveryTypeCode == "DELV"
                                                     select d).OrderByDescending(s => s.PickUpDeliveryNumber).FirstOrDefault();

            if (myLastDelivery != null)
            {
                PickUpDeliveryPlaceData myLastDeliveryResult = webServiceHelper.GetPickUpDeliveryPlaceData(myLastDelivery, "Delivery");

                if (myLastDeliveryResult != null)
                {
                    myRecord.To = myLastDeliveryResult.City;
                    myRecord.ToState = myLastDeliveryResult.StateName;
                    myRecord.ToCountry = myLastDeliveryResult.CountryName;
                }
            }

            else
            {
                string toLocationPortId = this.GetToLocationPortId(item);

                if (!string.IsNullOrEmpty(toLocationPortId))
                {
                    Port myPort = portRepository.GetSinglePort(tenant, toLocationPortId);
                    if (myPort != null)
                    {
                        myRecord.To = myPort.EnglishName;
                        myRecord.ToState = myPort.State == null ? null : myPort.State.EnglishName;
                        myRecord.ToCountry = myPort.Country == null ? null : myPort.Country.EnglishName;
                    }
                }
            }
        }

        private string GetToLocationPortId(ShipmentsJoinPayablesList item)
        {
            if (item.OnForwardingToPortId != null)
            {
                return item.OnForwardingToPortId;
            }

            else if (item.OnCarriageToPortId != null)
            {
                return item.OnCarriageToPortId;
            }

            else if (item.Transshipment3ToPortId != null)
            {
                return item.Transshipment3ToPortId;
            }

            else if (item.Transshipment2ToPortId != null)
            {
                return item.Transshipment2ToPortId;
            }

            else if (item.Transshipment1ToPortId != null)
            {
                return item.Transshipment1ToPortId;
            }

            else
            {
                return item.MainCarriageToPortId;
            }
        }

        private void ComputeFromLocationProperties(VendorChargesShipment myRecord, ShipmentsJoinPayablesList item)
        {
            ShipmentPickUpDelivery myFirstPickup = (from d in shipmentsContext.ShipmentPickUpDeliveries
                                                    where d.ShipmentId == item.ShipmentId && d.PickUpDeliveryTypeCode == "PICK"
                                                    select d).OrderBy(s => s.PickUpDeliveryNumber).FirstOrDefault();

            if (myFirstPickup != null)
            {
                PickUpDeliveryPlaceData myFirstPickupResult = webServiceHelper.GetPickUpDeliveryPlaceData(myFirstPickup, "Pickup");

                if (myFirstPickupResult != null)
                {
                    myRecord.From = myFirstPickupResult.City;
                    myRecord.FromState = myFirstPickupResult.StateName;
                    myRecord.FromCountry = myFirstPickupResult.CountryName;
                }
            }

            else
            {
                string fromLocationPortId = this.GetFromLocationPortId(item);

                if (!string.IsNullOrEmpty(fromLocationPortId))
                {
                    Port myPort = portRepository.GetSinglePort(tenant, fromLocationPortId);
                    if (myPort != null)
                    {
                        myRecord.To = myPort.EnglishName;
                        myRecord.ToState = myPort.State == null ? null : myPort.State.EnglishName;
                        myRecord.ToCountry = myPort.Country == null ? null : myPort.Country.EnglishName;
                    }
                }
            }
        }

        private string GetFromLocationPortId(ShipmentsJoinPayablesList item)
        {
            if (item.PreForwardingFromPortId != null)
            {
                return item.PreForwardingFromPortId;
            }

            else if (item.PreCarriageFromPortId != null)
            {
                return item.PreCarriageFromPortId;
            }

            else
            {
                return item.MainCarriageFromPortId;
            }
        }
    }

    public class ShipmentsJoinPayablesList
    {
        public string MainCarriageCarrierNumber { get; set; }
        public string DirectionId { get; set; }
        public string TruckNumber { get; set; }
        public string ContainersNumbersAndTypesArray { get; set; }
        public string TransportModeId { get; set; }
        public string TruckContainerNumber { get; set; }
        public string ShipmentId { get; set; }
        public string ShipmentNumber { get; set; }
        public string CustomerName { get; set; }
        public DateTime? CreateDateTime { get; set; }
        public DateTime? FirstOpCloseDate { get; set; }
        public string OpenedBy { get; set; }
        public string Level { get; set; }
        public string ShipmentType { get; set; }
        public string SpecialServices { get; set; }
        public double? ChargeableWeightInKG { get; set; }
        public double? ChargeableWeight { get; set; }
        public double? VolumeInCBM { get; set; }
        public string PreCarriageFromPortId { get; set; }
        public string PreForwardingFromPortId { get; set; }
        public string MainCarriageFromPortId { get; set; }
        public string MainCarriageToPortId { get; set; }
        public string OnCarriageToPortId { get; set; }
        public string OnForwardingToPortId { get; set; }
        public string Transshipment1ToPortId { get; set; }
        public string Transshipment2ToPortId { get; set; }
        public string Transshipment3ToPortId { get; set; }

        public string VendorId { get; set; }
        public string VendorName { get; set; }
        public string ChargeTypeId { get; set; }
        public string ChargeTypeName { get; set; }
        public double? Payables_OPEN { get; set; }
        public double? Payables_ACCT { get; set; }
        public string Notes { get; set; }
        public DateTime? FinalArrivalDate { get; set; }
    }
}