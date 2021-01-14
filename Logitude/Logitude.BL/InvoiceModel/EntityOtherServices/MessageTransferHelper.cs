using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.ShipmentsModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Logitude.BL.InvoiceModel.EntityOtherServices
{
    public class MessageTransferHelper
    {
        private int tenant;
        private IInvoiceContext invoiceCotnext;
        private ICommonDataContext commonContext;
        private IShipmentsContext shipmentsContext;
        private StateRepository stateRepository;
        public MessageTransferHelper(int tenant, IInvoiceContext invoiceCotnext, ICommonDataContext commonContext, IShipmentsContext shipmentsContext)
        {
            this.tenant = tenant;
            this.invoiceCotnext = invoiceCotnext;
            this.commonContext = commonContext;
            this.shipmentsContext = shipmentsContext;
            this.stateRepository = new StateRepository(commonContext);
        }

        public string GetAddress(AddressPM address)
        {
            string resultAddress = "";

            if (address != null)
            {
                resultAddress = address.Address1 != null ? address.Address1 : "";

                if (!string.IsNullOrEmpty(address.Address2))
                {
                    resultAddress = resultAddress + Environment.NewLine + address.Address2;
                }

                if (!string.IsNullOrEmpty(address.City))
                {
                    resultAddress = resultAddress + Environment.NewLine + address.City;
                }

                if (!string.IsNullOrEmpty(address.StateId))
                {
                    State state = this.stateRepository.GetSingleState(address.StateId, tenant, true);

                    if (state != null)
                    {
                        if (address.IsLocalLanguage)
                        {
                            resultAddress = resultAddress + " " + (state.LocalName != null ? state.LocalName : "");
                        }

                        else
                        {
                            resultAddress = resultAddress + " " + (state.EnglishName != null ? state.EnglishName : "");
                        }
                    }
                }

                if (!string.IsNullOrEmpty(address.ZipCode))
                {
                    resultAddress = resultAddress + " " + address.ZipCode;
                }

                if (!string.IsNullOrEmpty(address.CountryId))
                {
                    Country country = CountryRepository.GetSingleCountry(address.CountryId, tenant, true);

                    if (country != null)
                    {
                        if (address.IsLocalLanguage)
                        {
                            resultAddress = resultAddress + Environment.NewLine + country.LocalName;
                        }

                        else
                        {
                            resultAddress = resultAddress + Environment.NewLine + country.EnglishName;
                        }
                    }
                }
            }

            return resultAddress;
        }
        public string GetPickUpDeliveryFromCityOrPortName(ShipmentPickUpPM entity)
        {
            string myResult = "";
            AddressRepository addressRepository = new AddressRepository(tenant);

            if (entity != null)
            {
                switch (entity.PickUpDeliveryFromTypeCode)
                {
                    case "PART":
                        {
                            if (!string.IsNullOrEmpty(entity.FromPartnerCardId))
                            {
                                Address myPartnerAddress = addressRepository.GetMainAddressByCardId(entity.FromPartnerCardId, tenant);
                                if (myPartnerAddress != null)
                                {
                                    myResult = myPartnerAddress.City;
                                }
                            }

                            break;
                        }

                    case "PORT":
                        {
                            if (!string.IsNullOrEmpty(entity.FromPortId))
                            {
                                PortPM myPort = PortQuery.GetSinglePort(tenant, entity.FromPortId, true);
                                if (myPort != null)
                                {
                                    myResult = myPort.EnglishName;
                                }
                            }

                            break;
                        }

                    case "CASL":
                        {
                            myResult = entity.FromAddressCity;
                            break;
                        }
                }
            }

            if (myResult == null)
            {
                myResult = "";
            }

            return myResult;
        }
        public string GetTransportModes(string transPortMode)
        {
            string result = "";

            if (transPortMode == "A")
            {
                result = "Air";
            }
            else if (transPortMode == "I")
            {
                result = "Inland";
            }
            else if (transPortMode == "O")
            {
                result = "Ocean";
            }

            return result;
        }

    }

    // AR Invoice GenericInterface
    [XmlRoot("Logitude")]
    public class ARInvoiceRoot
    {
        [XmlArray("Invoices")]
        [XmlArrayItem("Invoice")]
        public List<ARInvoiceElement> Invoices { get; set; }
    }
    public class ARInvoiceElement
    {
        public string DocumentType { get; set; }
        public string InvoiceType { get; set; }
        public string InvoiceNumber { get; set; }

        [XmlElement(DataType = "date")]
        public DateTime? InvoiceDate { get; set; }

        public string InvoiceNotes { get; set; }
        public string InvoiceCurrency { get; set; }
        public string ShipmentNumber { get; set; }
        public string MasterReference { get; set; }
        public string HouseReference { get; set; }
        public string CustomerReference { get; set; }
        public int Tenant { get; set; }
        public string Intercompany { get; set; }

        [XmlElement(ElementName = "BillTo")]
        public CardElement Card { get; set; }

        public string VATNumber { get; set; }
        public string PaymentTermExternalId { get; set; }
        public string SATPaymentMethodCode { get; set; }
        public string SATPaymentMethodName { get; set; }

        [XmlElement(DataType = "date")]
        public DateTime? DueDate { get; set; }

        public decimal TotalTaxAmountInInvoiceCurrency { get; set; }
        public decimal InvoiceTotalLineAmount { get; set; }
        public decimal TotalInvoiceInInvoiceCurrency { get; set; }
        public decimal RateInvoiceCurrency { get; set; }

        [XmlElement(DataType = "date")]
        public DateTime? ETD { get; set; }

        [XmlElement(DataType = "date")]
        public DateTime? ATD { get; set; }

        [XmlElement(DataType = "date")]
        public DateTime? ETA { get; set; }

        [XmlElement(DataType = "date")]
        public DateTime? ATA { get; set; }

        public ARShipmentDetailsElement ShipmentDetails { get; set; }

        [XmlArray("InvoiceLines")]
        [XmlArrayItem("InvoiceLine")]
        public List<ARInvoiceLineElement> InvoiceLines { get; set; }

        [XmlArray("TaxTotalsInInvoiceCurrency")]
        [XmlArrayItem("TaxTotalInInvoiceCurrency")]
        public List<InvoiceTaxElement> TaxTotalsInInvoiceCurrency { get; set; }
    }

    public class ARInvoiceLineElement
    {
        public int LineNumber { get; set; }
        public string ChargeTypeCode { get; set; }
        public string Description { get; set; }
        public string InvoiceLineNote { get; set; }
        public string OriginalCurrency { get; set; }
        public decimal AmountInOriginalCurrency { get; set; }
        public decimal AmountInInvoiceCurrency { get; set; }
        public decimal AmountInLocalCurrency { get; set; }
        public decimal TaxPercentage { get; set; }
        public string TaxCode { get; set; }
        public string VATExternalId { get; set; }

        public string CreditAccount { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal Quantity { get; set; }
        public string MeasurementCode { get; set; }
        public string PrepaidCollect { get; set; }

        public bool IsMultiTAX { get; set; }
        public bool AppliesRegionalTax { get; set; }

        [XmlElement(ElementName = "Advanced")]
        public LineAdvancedElement Advanced { get; set; }

        [XmlArray("TaxDetails")]
        [XmlArrayItem("TaxDetail")]
        public List<LineTaxDetailsElement> TaxDetails { get; set; }
    }
    public class ARShipmentDetailsElement
    {
        //*********** General ***********\\
        public string Level { get; set; }
        public string TransportMode { get; set; }
        public string Direction { get; set; }
        public string ShipmentNumber { get; set; }
        public string MasterNumber { get; set; }
        public string HouseNumber { get; set; }
        public string AMSBL { get; set; }
        public string BookingNumber { get; set; }
        public string SalesMan { get; set; }
        public string Branch { get; set; }
        public string MoveTypeCode { get; set; }
        public string MoveTypeName { get; set; }
        public string IncotermCode { get; set; }
        public string IncotermName { get; set; }

        //*********** Partners ***********\\
        #region Shipper
        public string ShipperName { get; set; }
        public string ShipperFullAddress { get; set; }
        public string ShipperAddress1 { get; set; }
        public string ShipperAddress2 { get; set; }
        public string ShipperCity { get; set; }
        public string ShipperCountryCode { get; set; }
        public string ShipperCountryName { get; set; }
        public string ShipperStateCode { get; set; }
        public string ShipperStateName { get; set; }
        public string ShipperZipCode { get; set; }
        public string ShipperTelephone { get; set; }
        public string ShipperFax { get; set; }
        public string ShipperRef1 { get; set; }
        public string ShipperRef2 { get; set; }
        public string ShipperContactName { get; set; }
        public string ShipperContactEmail { get; set; }
        public string ShipperContactPhone { get; set; }
        public string ShipperContactFax { get; set; }
        public string ShipperContactPosition { get; set; }
        #endregion

        #region Consignee
        public string ConsigneeName { get; set; }
        public string ConsigneeFullAddress { get; set; }
        public string ConsigneeAddress1 { get; set; }
        public string ConsigneeAddress2 { get; set; }
        public string ConsigneeCity { get; set; }
        public string ConsigneeCountryCode { get; set; }
        public string ConsigneeCountryName { get; set; }
        public string ConsigneeStateCode { get; set; }
        public string ConsigneeStateName { get; set; }
        public string ConsigneeZipCode { get; set; }
        public string ConsigneeTelephone { get; set; }
        public string ConsigneeFax { get; set; }
        public string ConsigneeRef1 { get; set; }
        public string ConsigneeRef2 { get; set; }
        public string ConsigneeContactName { get; set; }
        public string ConsigneeContactEmail { get; set; }
        public string ConsigneeContactPhone { get; set; }
        public string ConsigneeContactFax { get; set; }
        public string ConsigneeContactPosition { get; set; }
        #endregion

        #region Agent
        public string AgentName { get; set; }
        public string AgentFullAddress { get; set; }
        public string AgentAddress1 { get; set; }
        public string AgentAddress2 { get; set; }
        public string AgentCity { get; set; }
        public string AgentCountryCode { get; set; }
        public string AgentCountryName { get; set; }
        public string AgentStateCode { get; set; }
        public string AgentStateName { get; set; }
        public string AgentZipCode { get; set; }
        public string AgentTelephone { get; set; }
        public string AgentFax { get; set; }
        public string AgentRef1 { get; set; }
        public string AgentRef2 { get; set; }
        public string AgentContactName { get; set; }
        public string AgentContactEmail { get; set; }
        public string AgentContactPhone { get; set; }
        public string AgentContactFax { get; set; }
        public string AgentContactPosition { get; set; }
        #endregion

        #region Notify 1
        public string Notify1Name { get; set; }
        public string Notify1FullAddress { get; set; }
        public string Notify1Address1 { get; set; }
        public string Notify1Address2 { get; set; }
        public string Notify1City { get; set; }
        public string Notify1CountryCode { get; set; }
        public string Notify1CountryName { get; set; }
        public string Notify1StateCode { get; set; }
        public string Notify1StateName { get; set; }
        public string Notify1ZipCode { get; set; }
        public string Notify1Telephone { get; set; }
        public string Notify1Fax { get; set; }
        public string Notify1ContactName { get; set; }
        public string Notify1ContactEmail { get; set; }
        public string Notify1ContactPhone { get; set; }
        public string Notify1ContactFax { get; set; }
        public string Notify1ContactPosition { get; set; }
        #endregion

        #region Notify 2
        public string Notify2Name { get; set; }
        public string Notify2FullAddress { get; set; }
        public string Notify2Address1 { get; set; }
        public string Notify2Address2 { get; set; }
        public string Notify2City { get; set; }
        public string Notify2CountryCode { get; set; }
        public string Notify2CountryName { get; set; }
        public string Notify2StateCode { get; set; }
        public string Notify2StateName { get; set; }
        public string Notify2ZipCode { get; set; }
        public string Notify2Telephone { get; set; }
        public string Notify2Fax { get; set; }
        public string Notify2ContactName { get; set; }
        public string Notify2ContactEmail { get; set; }
        public string Notify2ContactPhone { get; set; }
        public string Notify2ContactFax { get; set; }
        public string Notify2ContactPosition { get; set; }
        #endregion

        //*********** Routing ***********\\
        public string PickUpAddress { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime? PickUpATD { get; set; }
        public DateTime? DeliveryATA { get; set; }

        #region PreCarriage
        public string PreCarriageTransportMode { get; set; }
        public string PreCarriageFromPortCode { get; set; }
        public string PreCarriageFromPortName { get; set; }
        public string PreCarriageFromPortCountryCode { get; set; }
        public string PreCarriageFromPortCountryName { get; set; }
        public string PreCarriageToPortCode { get; set; }
        public string PreCarriageToPortName { get; set; }
        public string PreCarriageToPortCountryCode { get; set; }
        public string PreCarriageToPortCountryName { get; set; }
        public string PreCarriageCarrierName { get; set; }
        public string PreCarriageCarrierNumber { get; set; }
        public DateTime? PreCarriageETD { get; set; }
        public DateTime? PreCarriageETA { get; set; }
        public DateTime? PreCarriageATD { get; set; }
        public DateTime? PreCarriageATA { get; set; }
        #endregion

        #region OnCarriage
        public string OnCarriageTransportMode { get; set; }
        public string OnCarriageFromPortCode { get; set; }
        public string OnCarriageFromPortName { get; set; }
        public string OnCarriageFromPortCountryCode { get; set; }
        public string OnCarriageFromPortCountryName { get; set; }
        public string OnCarriageToPortCode { get; set; }
        public string OnCarriageToPortName { get; set; }
        public string OnCarriageToPortCountryCode { get; set; }
        public string OnCarriageToPortCountryName { get; set; }
        public string OnCarriageCarrierName { get; set; }
        public string OnCarriageCarrierNumber { get; set; }
        public DateTime? OnCarriageETD { get; set; }
        public DateTime? OnCarriageETA { get; set; }
        public DateTime? OnCarriageATD { get; set; }
        public DateTime? OnCarriageATA { get; set; }
        #endregion

        #region MainCarriage
        public string MainCarriageCarrierCode { get; set; }
        public string MainCarriageCarrierName { get; set; }
        public string MainCarriageAirlinePrefix { get; set; }
        public string MainCarriageCarrierNumber { get; set; }
        public DateTime? MainCarriageETD { get; set; }
        public DateTime? MainCarriageATD { get; set; }
        public DateTime? MainCarriageETA { get; set; }
        public DateTime? MainCarriageATA { get; set; }
        public string MainCarriageVessel { get; set; }
        #endregion

        #region Via1
        public string Via1CarrierCode { get; set; }
        public string Via1CarrierName { get; set; }
        public string Via1AirlinePrefix { get; set; } //in case of air shipment
        public string Via1CarrierNumber { get; set; }
        public DateTime? Via1ETD { get; set; }
        public DateTime? Via1ATD { get; set; }
        public DateTime? Via1ETA { get; set; }
        public DateTime? Via1ATA { get; set; }
        public string Via1Vessel { get; set; } //in case of ocean shipment
        #endregion

        #region Via2
        public string Via2CarrierCode { get; set; }
        public string Via2CarrierName { get; set; }
        public string Via2AirlinePrefix { get; set; } //in case of air shipment
        public string Via2CarrierNumber { get; set; }
        public DateTime? Via2ETD { get; set; }
        public DateTime? Via2ATD { get; set; }
        public DateTime? Via2ETA { get; set; }
        public DateTime? Via2ATA { get; set; }
        public string Via2Vessel { get; set; } //in case of ocean shipment
        #endregion

        #region Via3
        public string Via3CarrierCode { get; set; }
        public string Via3CarrierName { get; set; }
        public string Via3AirlinePrefix { get; set; } //in case of air shipment
        public string Via3CarrierNumber { get; set; }
        public DateTime? Via3ETD { get; set; }
        public DateTime? Via3ATD { get; set; }
        public DateTime? Via3ETA { get; set; }
        public DateTime? Via3ATA { get; set; }
        public string Via3Vessel { get; set; } //in case of ocean shipment
        #endregion

        #region FromLocation
        public string FromPortCode { get; set; }
        public string FromPortName { get; set; }
        public string FromCountryCode { get; set; }
        public string FromCountryName { get; set; }
        public string FromAddress { get; set; }	//for the case of inland domestic
        #endregion

        #region ToLocation
        public string ToPortCode { get; set; }
        public string ToPortName { get; set; }
        public string ToCountryCode { get; set; }
        public string ToCountryName { get; set; }
        public string ToAddress { get; set; }	//for the case of inland domestic
        #endregion

        #region FinalLocation
        public string FinalPortCode { get; set; }
        public string FinalPortName { get; set; }
        public string FinalCountryCode { get; set; }
        public string FinalCountryName { get; set; }
        public string FinalAddress { get; set; }	//for the case of inland domestic
        #endregion

        //*********** Packages Details ***********\\
        public double? TotalGrossWeight { get; set; }
        public string GrossWeightUnitCode { get; set; }
        public double? TotalChargeableWeight { get; set; }
        public string ChargeableWeightUnitCode { get; set; }
        public double? TotalVolume { get; set; }
        public string VolumeUnitCode { get; set; }
        public int? TotalNumberOfPackage { get; set; }
        public int? TotalNumberOfContainers { get; set; }
        public string TotalContainers { get; set; }
        public string ContainersNumbersAndTypesArray { get; set; }
        public string DescriptionOfGoods { get; set; }

        public List<PackageLineElement> PackageLines { get; set; }

        //*********** Custom Fields ***********\\        
        #region Shipment
        public string ShipmentField1Name { get; set; }
        public string ShipmentField2Name { get; set; }
        public string ShipmentField3Name { get; set; }
        public string ShipmentField4Name { get; set; }
        public string ShipmentField5Name { get; set; }
        public string ShipmentField6Name { get; set; }
        public string ShipmentField7Name { get; set; }
        public string ShipmentField8Name { get; set; }
        public string ShipmentField9Name { get; set; }
        public string ShipmentField10Name { get; set; }
        public string ShipmentField11Name { get; set; }
        public string ShipmentField12Name { get; set; }
        public string ShipmentField13Name { get; set; }
        public string ShipmentField14Name { get; set; }
        public string ShipmentField15Name { get; set; }
        public string ShipmentField16Name { get; set; }
        public string ShipmentField17Name { get; set; }
        public string ShipmentField18Name { get; set; }
        public string ShipmentField19Name { get; set; }
        public string ShipmentField20Name { get; set; }
        public string ShipmentField21Name { get; set; }
        public string ShipmentField22Name { get; set; }
        public string ShipmentField23Name { get; set; }
        public string ShipmentField24Name { get; set; }
        public string ShipmentField25Name { get; set; }
        public string ShipmentField26Name { get; set; }
        public string ShipmentField27Name { get; set; }
        public string ShipmentField28Name { get; set; }
        public string ShipmentField29Name { get; set; }
        public string ShipmentField30Name { get; set; }
        public string ShipmentField31Name { get; set; }
        public string ShipmentField32Name { get; set; }
        public string ShipmentField33Name { get; set; }
        public string ShipmentField34Name { get; set; }
        public string ShipmentField35Name { get; set; }
        public string ShipmentField36Name { get; set; }
        public string ShipmentField37Name { get; set; }
        public string ShipmentField38Name { get; set; }
        public string ShipmentField39Name { get; set; }
        public string ShipmentField40Name { get; set; }

        public string ShipmentField1Value { get; set; }
        public string ShipmentField2Value { get; set; }
        public string ShipmentField3Value { get; set; }
        public string ShipmentField4Value { get; set; }
        public string ShipmentField5Value { get; set; }
        public string ShipmentField6Value { get; set; }
        public string ShipmentField7Value { get; set; }
        public string ShipmentField8Value { get; set; }
        public string ShipmentField9Value { get; set; }
        public string ShipmentField10Value { get; set; }
        public string ShipmentField11Value { get; set; }
        public string ShipmentField12Value { get; set; }
        public string ShipmentField13Value { get; set; }
        public string ShipmentField14Value { get; set; }
        public string ShipmentField15Value { get; set; }
        public string ShipmentField16Value { get; set; }
        public string ShipmentField17Value { get; set; }
        public string ShipmentField18Value { get; set; }
        public string ShipmentField19Value { get; set; }
        public string ShipmentField20Value { get; set; }
        public string ShipmentField21Value { get; set; }
        public string ShipmentField22Value { get; set; }
        public string ShipmentField23Value { get; set; }
        public string ShipmentField24Value { get; set; }
        public string ShipmentField25Value { get; set; }
        public string ShipmentField26Value { get; set; }
        public string ShipmentField27Value { get; set; }
        public string ShipmentField28Value { get; set; }
        public string ShipmentField29Value { get; set; }
        public string ShipmentField30Value { get; set; }
        public string ShipmentField31Value { get; set; }
        public string ShipmentField32Value { get; set; }
        public string ShipmentField33Value { get; set; }
        public string ShipmentField34Value { get; set; }
        public string ShipmentField35Value { get; set; }
        public string ShipmentField36Value { get; set; }
        public string ShipmentField37Value { get; set; }
        public string ShipmentField38Value { get; set; }
        public string ShipmentField39Value { get; set; }
        public string ShipmentField40Value { get; set; }

        #endregion

        #region Customer
        public string CustomerField1Name { get; set; }
        public string CustomerField2Name { get; set; }
        public string CustomerField3Name { get; set; }
        public string CustomerField4Name { get; set; }
        public string CustomerField5Name { get; set; }
        public string CustomerField6Name { get; set; }
        public string CustomerField7Name { get; set; }
        public string CustomerField8Name { get; set; }
        public string CustomerField9Name { get; set; }
        public string CustomerField10Name { get; set; }

        public string CustomerField1Value { get; set; }
        public string CustomerField2Value { get; set; }
        public string CustomerField3Value { get; set; }
        public string CustomerField4Value { get; set; }
        public string CustomerField5Value { get; set; }
        public string CustomerField6Value { get; set; }
        public string CustomerField7Value { get; set; }
        public string CustomerField8Value { get; set; }
        public string CustomerField9Value { get; set; }
        public string CustomerField10Value { get; set; }
        #endregion

        #region ARInvoice
        public string ARInvoiceField1Name { get; set; }
        public string ARInvoiceField2Name { get; set; }
        public string ARInvoiceField3Name { get; set; }
        public string ARInvoiceField4Name { get; set; }
        public string ARInvoiceField5Name { get; set; }
        public string ARInvoiceField6Name { get; set; }
        public string ARInvoiceField7Name { get; set; }
        public string ARInvoiceField8Name { get; set; }
        public string ARInvoiceField9Name { get; set; }
        public string ARInvoiceField10Name { get; set; }

        public string ARInvoiceField1Value { get; set; }
        public string ARInvoiceField2Value { get; set; }
        public string ARInvoiceField3Value { get; set; }
        public string ARInvoiceField4Value { get; set; }
        public string ARInvoiceField5Value { get; set; }
        public string ARInvoiceField6Value { get; set; }
        public string ARInvoiceField7Value { get; set; }
        public string ARInvoiceField8Value { get; set; }
        public string ARInvoiceField9Value { get; set; }
        public string ARInvoiceField10Value { get; set; }
        #endregion
    }
    
    [XmlRoot("Logitude")]
    public class APInvoiceRoot
    {
        [XmlArray("Invoices")]
        [XmlArrayItem("Invoice")]
        public List<APInvoiceElement> Invoices { get; set; }
    }
    public class APInvoiceElement
    {
        public string DocumentType { get; set; }
        public string InvoiceType { get; set; }
        public string InvoiceNumber { get; set; }
        public string InternalRef { get; set; }

        [XmlElement(DataType = "date")]
        public DateTime? InvoiceDate { get; set; }

        public string InvoiceNotes { get; set; }
        public string InvoiceCurrency { get; set; }
        public string ShipmentNumber { get; set; }
        public string MasterReference { get; set; }
        public string HouseReference { get; set; }

        [XmlElement(ElementName = "Vendor")]
        public CardElement Card { get; set; }

        public string VATNumber { get; set; }

        public string PaymentTermExternalId { get; set; }

        [XmlElement(DataType = "date")]
        public DateTime? DueDate { get; set; }

        public decimal TotalTaxAmountInInvoiceCurrency { get; set; }
        public decimal InvoiceTotalLineAmount { get; set; }
        public decimal TotalInvoiceInInvoiceCurrency { get; set; }
        public decimal RateInvoiceCurrency { get; set; }

        [XmlElement(DataType = "date")]
        public DateTime? ETD { get; set; }

        [XmlElement(DataType = "date")]
        public DateTime? ATD { get; set; }

        [XmlElement(DataType = "date")]
        public DateTime? ETA { get; set; }

        [XmlElement(DataType = "date")]
        public DateTime? ATA { get; set; }

        public APShipmentDetailsElement ShipmentDetails { get; set; }

        [XmlArray("InvoiceLines")]
        [XmlArrayItem("InvoiceLine")]
        public List<APInvoiceLineElement> InvoiceLines { get; set; }

        [XmlArray("TaxTotalsInInvoiceCurrency")]
        [XmlArrayItem("TaxTotalInInvoiceCurrency")]
        public List<InvoiceTaxElement> TaxTotalsInInvoiceCurrency { get; set; }
    }
    public class APInvoiceLineElement
    {
        public int LineNumber { get; set; }
        public string ChargeTypeCode { get; set; }
        public string Description { get; set; }
        public string InvoiceLineNote { get; set; }
        public string OriginalCurrency { get; set; }
        public decimal AmountInOriginalCurrency { get; set; }
        public decimal AmountInInvoiceCurrency { get; set; }
        public decimal AmountInLocalCurrency { get; set; }
        public decimal TaxPercentage { get; set; }
        public string TaxCode { get; set; }
        public string VATExternalId { get; set; }
        public string PayableLineId { get; set; }

        public string DebitAccount { get; set; }
        public decimal? Quantity { get; set; }
        public string MeasurementCode { get; set; }
        public string PrepaidCollect { get; set; }

        public bool IsMultiTAX { get; set; }

        [XmlElement(ElementName = "Advanced")]
        public LineAdvancedElement Advanced { get; set; }

        [XmlArray("TaxDetails")]
        [XmlArrayItem("TaxDetail")]
        public List<LineTaxDetailsElement> TaxDetails { get; set; }
    }
    public class APShipmentDetailsElement
    {
        //*********** General ***********\\
        public string Level { get; set; }
        public string TransportMode { get; set; }
        public string Direction { get; set; }
        public string ShipmentNumber { get; set; }
        public string MasterNumber { get; set; }
        public string HouseNumber { get; set; }
        public string AMSBL { get; set; }
        public string BookingNumber { get; set; }
        public string SalesMan { get; set; }
        public string Branch { get; set; }
        public string MoveTypeCode { get; set; }
        public string MoveTypeName { get; set; }
        public string IncotermCode { get; set; }
        public string IncotermName { get; set; }

        //*********** Partners ***********\\
        #region Shipper
        public string ShipperName { get; set; }
        public string ShipperFullAddress { get; set; }
        public string ShipperAddress1 { get; set; }
        public string ShipperAddress2 { get; set; }
        public string ShipperCity { get; set; }
        public string ShipperCountryCode { get; set; }
        public string ShipperCountryName { get; set; }
        public string ShipperStateCode { get; set; }
        public string ShipperStateName { get; set; }
        public string ShipperZipCode { get; set; }
        public string ShipperTelephone { get; set; }
        public string ShipperFax { get; set; }
        public string ShipperRef1 { get; set; }
        public string ShipperRef2 { get; set; }
        public string ShipperContactName { get; set; }
        public string ShipperContactEmail { get; set; }
        public string ShipperContactPhone { get; set; }
        public string ShipperContactFax { get; set; }
        public string ShipperContactPosition { get; set; }
        #endregion

        #region Consignee
        public string ConsigneeName { get; set; }
        public string ConsigneeFullAddress { get; set; }
        public string ConsigneeAddress1 { get; set; }
        public string ConsigneeAddress2 { get; set; }
        public string ConsigneeCity { get; set; }
        public string ConsigneeCountryCode { get; set; }
        public string ConsigneeCountryName { get; set; }
        public string ConsigneeStateCode { get; set; }
        public string ConsigneeStateName { get; set; }
        public string ConsigneeZipCode { get; set; }
        public string ConsigneeTelephone { get; set; }
        public string ConsigneeFax { get; set; }
        public string ConsigneeRef1 { get; set; }
        public string ConsigneeRef2 { get; set; }
        public string ConsigneeContactName { get; set; }
        public string ConsigneeContactEmail { get; set; }
        public string ConsigneeContactPhone { get; set; }
        public string ConsigneeContactFax { get; set; }
        public string ConsigneeContactPosition { get; set; }
        #endregion

        #region Agent
        public string AgentName { get; set; }
        public string AgentFullAddress { get; set; }
        public string AgentAddress1 { get; set; }
        public string AgentAddress2 { get; set; }
        public string AgentCity { get; set; }
        public string AgentCountryCode { get; set; }
        public string AgentCountryName { get; set; }
        public string AgentStateCode { get; set; }
        public string AgentStateName { get; set; }
        public string AgentZipCode { get; set; }
        public string AgentTelephone { get; set; }
        public string AgentFax { get; set; }
        public string AgentRef1 { get; set; }
        public string AgentRef2 { get; set; }
        public string AgentContactName { get; set; }
        public string AgentContactEmail { get; set; }
        public string AgentContactPhone { get; set; }
        public string AgentContactFax { get; set; }
        public string AgentContactPosition { get; set; }
        #endregion

        #region Notify 1
        public string Notify1Name { get; set; }
        public string Notify1FullAddress { get; set; }
        public string Notify1Address1 { get; set; }
        public string Notify1Address2 { get; set; }
        public string Notify1City { get; set; }
        public string Notify1CountryCode { get; set; }
        public string Notify1CountryName { get; set; }
        public string Notify1StateCode { get; set; }
        public string Notify1StateName { get; set; }
        public string Notify1ZipCode { get; set; }
        public string Notify1Telephone { get; set; }
        public string Notify1Fax { get; set; }
        public string Notify1ContactName { get; set; }
        public string Notify1ContactEmail { get; set; }
        public string Notify1ContactPhone { get; set; }
        public string Notify1ContactFax { get; set; }
        public string Notify1ContactPosition { get; set; }
        #endregion

        #region Notify 2
        public string Notify2Name { get; set; }
        public string Notify2FullAddress { get; set; }
        public string Notify2Address1 { get; set; }
        public string Notify2Address2 { get; set; }
        public string Notify2City { get; set; }
        public string Notify2CountryCode { get; set; }
        public string Notify2CountryName { get; set; }
        public string Notify2StateCode { get; set; }
        public string Notify2StateName { get; set; }
        public string Notify2ZipCode { get; set; }
        public string Notify2Telephone { get; set; }
        public string Notify2Fax { get; set; }
        public string Notify2ContactName { get; set; }
        public string Notify2ContactEmail { get; set; }
        public string Notify2ContactPhone { get; set; }
        public string Notify2ContactFax { get; set; }
        public string Notify2ContactPosition { get; set; }
        #endregion

        //*********** Routing ***********\\
        public string PickUpAddress { get; set; }
        public string DeliveryAddress { get; set; }
        public DateTime? PickUpATD { get; set; }
        public DateTime? DeliveryATA { get; set; }

        #region PreCarriage
        public string PreCarriageTransportMode { get; set; }
        public string PreCarriageFromPortCode { get; set; }
        public string PreCarriageFromPortName { get; set; }
        public string PreCarriageFromPortCountryCode { get; set; }
        public string PreCarriageFromPortCountryName { get; set; }
        public string PreCarriageToPortCode { get; set; }
        public string PreCarriageToPortName { get; set; }
        public string PreCarriageToPortCountryCode { get; set; }
        public string PreCarriageToPortCountryName { get; set; }
        public string PreCarriageCarrierName { get; set; }
        public string PreCarriageCarrierNumber { get; set; }
        public DateTime? PreCarriageETD { get; set; }
        public DateTime? PreCarriageETA { get; set; }
        public DateTime? PreCarriageATD { get; set; }
        public DateTime? PreCarriageATA { get; set; }
        #endregion

        #region OnCarriage
        public string OnCarriageTransportMode { get; set; }
        public string OnCarriageFromPortCode { get; set; }
        public string OnCarriageFromPortName { get; set; }
        public string OnCarriageFromPortCountryCode { get; set; }
        public string OnCarriageFromPortCountryName { get; set; }
        public string OnCarriageToPortCode { get; set; }
        public string OnCarriageToPortName { get; set; }
        public string OnCarriageToPortCountryCode { get; set; }
        public string OnCarriageToPortCountryName { get; set; }
        public string OnCarriageCarrierName { get; set; }
        public string OnCarriageCarrierNumber { get; set; }
        public DateTime? OnCarriageETD { get; set; }
        public DateTime? OnCarriageETA { get; set; }
        public DateTime? OnCarriageATD { get; set; }
        public DateTime? OnCarriageATA { get; set; }
        #endregion

        #region MainCarriage
        public string MainCarriageCarrierCode { get; set; }
        public string MainCarriageCarrierName { get; set; }
        public string MainCarriageAirlinePrefix { get; set; }
        public string MainCarriageCarrierNumber { get; set; }
        public DateTime? MainCarriageETD { get; set; }
        public DateTime? MainCarriageATD { get; set; }
        public DateTime? MainCarriageETA { get; set; }
        public DateTime? MainCarriageATA { get; set; }
        public string MainCarriageVessel { get; set; }
        #endregion

        #region Via1
        public string Via1CarrierCode { get; set; }
        public string Via1CarrierName { get; set; }
        public string Via1AirlinePrefix { get; set; } //in case of air shipment
        public string Via1CarrierNumber { get; set; }
        public DateTime? Via1ETD { get; set; }
        public DateTime? Via1ATD { get; set; }
        public DateTime? Via1ETA { get; set; }
        public DateTime? Via1ATA { get; set; }
        public string Via1Vessel { get; set; } //in case of ocean shipment
        #endregion

        #region Via2
        public string Via2CarrierCode { get; set; }
        public string Via2CarrierName { get; set; }
        public string Via2AirlinePrefix { get; set; } //in case of air shipment
        public string Via2CarrierNumber { get; set; }
        public DateTime? Via2ETD { get; set; }
        public DateTime? Via2ATD { get; set; }
        public DateTime? Via2ETA { get; set; }
        public DateTime? Via2ATA { get; set; }
        public string Via2Vessel { get; set; } //in case of ocean shipment
        #endregion

        #region Via3
        public string Via3CarrierCode { get; set; }
        public string Via3CarrierName { get; set; }
        public string Via3AirlinePrefix { get; set; } //in case of air shipment
        public string Via3CarrierNumber { get; set; }
        public DateTime? Via3ETD { get; set; }
        public DateTime? Via3ATD { get; set; }
        public DateTime? Via3ETA { get; set; }
        public DateTime? Via3ATA { get; set; }
        public string Via3Vessel { get; set; } //in case of ocean shipment
        #endregion

        #region FromLocation
        public string FromPortCode { get; set; }
        public string FromPortName { get; set; }
        public string FromCountryCode { get; set; }
        public string FromCountryName { get; set; }
        public string FromAddress { get; set; }	//for the case of inland domestic
        #endregion

        #region ToLocation
        public string ToPortCode { get; set; }
        public string ToPortName { get; set; }
        public string ToCountryCode { get; set; }
        public string ToCountryName { get; set; }
        public string ToAddress { get; set; }	//for the case of inland domestic
        #endregion

        #region FinalLocation
        public string FinalPortCode { get; set; }
        public string FinalPortName { get; set; }
        public string FinalCountryCode { get; set; }
        public string FinalCountryName { get; set; }
        public string FinalAddress { get; set; }	//for the case of inland domestic
        #endregion

        //*********** Packages Details ***********\\
        public double? TotalGrossWeight { get; set; }
        public string GrossWeightUnitCode { get; set; }
        public double? TotalChargeableWeight { get; set; }
        public string ChargeableWeightUnitCode { get; set; }
        public double? TotalVolume { get; set; }
        public string VolumeUnitCode { get; set; }
        public int? TotalNumberOfPackage { get; set; }
        public int? TotalNumberOfContainers { get; set; }
        public string TotalContainers { get; set; }
        public string ContainersNumbersAndTypesArray { get; set; }
        public string DescriptionOfGoods { get; set; }

        public List<PackageLineElement> PackageLines { get; set; }

        //*********** Custom Fields ***********\\        
        #region Shipment
        public string ShipmentField1Name { get; set; }
        public string ShipmentField2Name { get; set; }
        public string ShipmentField3Name { get; set; }
        public string ShipmentField4Name { get; set; }
        public string ShipmentField5Name { get; set; }
        public string ShipmentField6Name { get; set; }
        public string ShipmentField7Name { get; set; }
        public string ShipmentField8Name { get; set; }
        public string ShipmentField9Name { get; set; }
        public string ShipmentField10Name { get; set; }
        public string ShipmentField11Name { get; set; }
        public string ShipmentField12Name { get; set; }
        public string ShipmentField13Name { get; set; }
        public string ShipmentField14Name { get; set; }
        public string ShipmentField15Name { get; set; }
        public string ShipmentField16Name { get; set; }
        public string ShipmentField17Name { get; set; }
        public string ShipmentField18Name { get; set; }
        public string ShipmentField19Name { get; set; }
        public string ShipmentField20Name { get; set; }
        public string ShipmentField21Name { get; set; }
        public string ShipmentField22Name { get; set; }
        public string ShipmentField23Name { get; set; }
        public string ShipmentField24Name { get; set; }
        public string ShipmentField25Name { get; set; }
        public string ShipmentField26Name { get; set; }
        public string ShipmentField27Name { get; set; }
        public string ShipmentField28Name { get; set; }
        public string ShipmentField29Name { get; set; }
        public string ShipmentField30Name { get; set; }
        public string ShipmentField31Name { get; set; }
        public string ShipmentField32Name { get; set; }
        public string ShipmentField33Name { get; set; }
        public string ShipmentField34Name { get; set; }
        public string ShipmentField35Name { get; set; }
        public string ShipmentField36Name { get; set; }
        public string ShipmentField37Name { get; set; }
        public string ShipmentField38Name { get; set; }
        public string ShipmentField39Name { get; set; }
        public string ShipmentField40Name { get; set; }

        public string ShipmentField1Value { get; set; }
        public string ShipmentField2Value { get; set; }
        public string ShipmentField3Value { get; set; }
        public string ShipmentField4Value { get; set; }
        public string ShipmentField5Value { get; set; }
        public string ShipmentField6Value { get; set; }
        public string ShipmentField7Value { get; set; }
        public string ShipmentField8Value { get; set; }
        public string ShipmentField9Value { get; set; }
        public string ShipmentField10Value { get; set; }
        public string ShipmentField11Value { get; set; }
        public string ShipmentField12Value { get; set; }
        public string ShipmentField13Value { get; set; }
        public string ShipmentField14Value { get; set; }
        public string ShipmentField15Value { get; set; }
        public string ShipmentField16Value { get; set; }
        public string ShipmentField17Value { get; set; }
        public string ShipmentField18Value { get; set; }
        public string ShipmentField19Value { get; set; }
        public string ShipmentField20Value { get; set; }
        public string ShipmentField21Value { get; set; }
        public string ShipmentField22Value { get; set; }
        public string ShipmentField23Value { get; set; }
        public string ShipmentField24Value { get; set; }
        public string ShipmentField25Value { get; set; }
        public string ShipmentField26Value { get; set; }
        public string ShipmentField27Value { get; set; }
        public string ShipmentField28Value { get; set; }
        public string ShipmentField29Value { get; set; }
        public string ShipmentField30Value { get; set; }
        public string ShipmentField31Value { get; set; }
        public string ShipmentField32Value { get; set; }
        public string ShipmentField33Value { get; set; }
        public string ShipmentField34Value { get; set; }
        public string ShipmentField35Value { get; set; }
        public string ShipmentField36Value { get; set; }
        public string ShipmentField37Value { get; set; }
        public string ShipmentField38Value { get; set; }
        public string ShipmentField39Value { get; set; }
        public string ShipmentField40Value { get; set; }
        #endregion

        #region Customer
        public string CustomerField1Name { get; set; }
        public string CustomerField2Name { get; set; }
        public string CustomerField3Name { get; set; }
        public string CustomerField4Name { get; set; }
        public string CustomerField5Name { get; set; }
        public string CustomerField6Name { get; set; }
        public string CustomerField7Name { get; set; }
        public string CustomerField8Name { get; set; }
        public string CustomerField9Name { get; set; }
        public string CustomerField10Name { get; set; }

        public string CustomerField1Value { get; set; }
        public string CustomerField2Value { get; set; }
        public string CustomerField3Value { get; set; }
        public string CustomerField4Value { get; set; }
        public string CustomerField5Value { get; set; }
        public string CustomerField6Value { get; set; }
        public string CustomerField7Value { get; set; }
        public string CustomerField8Value { get; set; }
        public string CustomerField9Value { get; set; }
        public string CustomerField10Value { get; set; }
        #endregion
    }

    public class CardElement
    {
        public string Code { get; set; }
        public string AccountingCard { get; set; }
        public string IntercompanyCode { get; set; }
        public string Name { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public string Country { get; set; }
        public string CountryCode { get; set; }
        public string State { get; set; }
        public string VATNumber { get; set; }        

        [XmlElement(ElementName = "Advanced")]
        public CardAdvancedElement Advanced { get; set; }
    }
    public class CardAdvancedElement
    {
        public string GLAccount { get; set; }
        public string CostCenter { get; set; }
        public string BusinessArea { get; set; }
    }
    public class PackageLineElement
    {
        public double? Width { get; set; }
        public double? Height { get; set; }
        public double? Length { get; set; }
        public string Dimensions { get; set; }
        public string PackageMarksAndNumbers { get; set; }
        public int? PackageQuantity { get; set; }
        public string PackageType { get; set; }
        public string PackageDescriptionOfGoods { get; set; }
        public string PackageGrossWeight { get; set; }
        public double? PackageVolume { get; set; }
        public string ContainerNumber { get; set; }
        public double? PackageTare { get; set; }
        public string PackageQuantityAndType { get; set; }
        public bool IsDangerous { get; set; }
        public int InsidePackagesCount { get; set; }
        public string InsidePackageList { get; set; }
        public string SealNumber { get; set; }
        public int ContainerSize { get; set; }
    }
    public class LineAdvancedElement
    {
        public string GLAccount { get; set; }
        public string CostCenter { get; set; }
    }
    public class InvoiceTaxElement
    {
        public string TaxCode { get; set; }
        public decimal TaxPercentage { get; set; }
        public decimal TaxAmount { get; set; }
        public bool IsRegionalTax { get; set; }
    }
    public class LineTaxDetailsElement
    {
        public string TaxCode { get; set; }
        public decimal TaxPercentage { get; set; }
        public string VATExternalId { get; set; }
    }
}
