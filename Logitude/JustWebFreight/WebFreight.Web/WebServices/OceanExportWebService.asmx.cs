using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web.Services;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using WebFreight.Web.DataProviders;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using WebFreight.Web.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using System.Text.RegularExpressions;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.WebServices
{
    /// <summary>
    /// Summary description for OceanExportWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class OceanExportWebService : System.Web.Services.WebService
    {
        private int tenant;
        FBLDataProvider myDataProvider;
        private WebServiceHelper myServicHelper;
        private IShipmentsContext shipmentsContext;
        private ICommonDataContext commonContext;
        private AddressRepository addressRepository;
        private ContactRepository contactRepository; 

        [WebMethod]
        public byte[] GetFBLData(string shipmentId, int tenant, string documentTypeCopyId)
        {
            this.tenant = tenant;
            this.myServicHelper = new WebServiceHelper(tenant);

            FBLDataProvider myDataProvider = GetFBLDataProvider(shipmentId, tenant, documentTypeCopyId);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(FBLDataProvider));
            MemoryStream memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream, myDataProvider);
            memoryStream.Seek(0, SeekOrigin.Begin);

            var reader = new StreamReader(memoryStream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memoryStream.ToArray();
            return bytearray;
        }

        private FBLDataProvider GetFBLDataProvider(string shipmentId, int tenant, string documentTypeCopyId)
        {
            myDataProvider = new FBLDataProvider();

            shipmentsContext = ShipmentsContext.GetContext(tenant);
            IWebFreightContext webFreightContext = WebFreightContext.GetContext(tenant);
            commonContext = CommonDataContext.GetContext(tenant);

            ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentsContext);
            ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRepository);

            PortRepository portRepository = new PortRepository(commonContext);
            addressRepository = new AddressRepository(commonContext);
            contactRepository = new ContactRepository(commonContext);

            ShipmentPM shipment = shipmentQuery.GetSinglePM(shipmentId, tenant);
            Tenant tenantSettings = (from a in commonContext.Tenants.Include("Address").Include("Address.Country") where a.Id == tenant select a).FirstOrDefault();
            DocumentTypeCopy documenttypecopy = (from d in commonContext.DocumentTypeCopies where d.Id == documentTypeCopyId select d).FirstOrDefault();

            ShipmentPickUpDeliveryRepository shipmentPickUpDeliveryRepository = new ShipmentPickUpDeliveryRepository(shipmentsContext);
            ShipmentDeliveryQuery shipmentDeliveryQuery = new ShipmentDeliveryQuery(shipmentPickUpDeliveryRepository);
            ShipmentPickUpQuery shipmentPickUpQuery = new ShipmentPickUpQuery(shipmentPickUpDeliveryRepository);

            if (shipment != null && tenantSettings != null)
            {
                myDataProvider.TenantCAAT = tenantSettings.CAAT;
                myDataProvider.TenantCBSA = tenantSettings.CBSA;

                PrepaidCollect shipmentprepaidcollect = (from a in webFreightContext.PrepaidCollects where a.Id == shipment.FreightPrepaidCollectId select a).FirstOrDefault();

                myDataProvider.ShipmentType = shipment.ShipmentTypeName != null ? shipment.ShipmentTypeName : "";
                myDataProvider.ShipmentNumber = shipment.ShipmentNumber != null ? shipment.ShipmentNumber : "";
                myDataProvider.CompanyName = tenantSettings.Company != null ? tenantSettings.Company : "";
                myDataProvider.MainCarriageETA = shipment.MainCarriageETA != null ? String.Format("{0:dd MMM yyyy}", shipment.MainCarriageETA) : "";
                myDataProvider.MainCarriageFirstLegETD = shipment.MainCarriageETD;
                myDataProvider.MainCarriageATD = shipment.MainCarriageATD;
                myDataProvider.ValueOfGoods = shipment.ValueOfGoods;
                myDataProvider.FreightRelease = shipment.FreightRelease;
                myDataProvider.TerminalAvailable = shipment.TerminalAvailable;
                myDataProvider.ISFNumber = shipment.ISFNumber;
                myDataProvider.ISFDate = shipment.ISFDate;
                myDataProvider.ITNumber = shipment.ITNumber;
                myDataProvider.ITDate = shipment.ITDate;
                myDataProvider.ENSNumber = shipment.ENSNumber;
                myDataProvider.ENSDate = shipment.ENSDate;
                myDataProvider.FMCNumber = tenantSettings.FMCNumber;
                myDataProvider.GrossWeightUnitCode = shipment.GrossWeightUnitCode;
                myDataProvider.VolumeUnitCode = shipment.VolumeUnitCode;
                myDataProvider.ShipmentSubTypeName = shipment.ShipmentSubTypeName;

                if (shipment.DocumentsClosingDate != null)
                {
                    myDataProvider.DocumentsClosingDate = shipment.DocumentsClosingDate;
                    myDataProvider.DocumentsClosingTime = shipment.DocumentsClosingDate.Value.TimeOfDay;
                }

                if (!string.IsNullOrEmpty(shipment.OBLTypeCode))
                {
                    OBLType type = shipmentsContext.OBLTypes.Where(d => d.Code == shipment.OBLTypeCode).FirstOrDefault();

                    if (type != null)
                    {
                        myDataProvider.OBLType = type.Name;
                    }
                }

                if (shipment.ValueOfGoodsCurrencyId != null)
                {
                    Simplog.Data.CommonDataModel.EntityPOCOs.Currency ValueOfGoodCurrency = CurrencyRepository.GetSingleCurrency(shipment.ValueOfGoodsCurrencyId, tenant, false);
                    myDataProvider.CurrencyOfValueofGoods = ValueOfGoodCurrency.EnglishName;
                    myDataProvider.CurrencyOfValueofGoodsLocal = ValueOfGoodCurrency.LocalName;
                }

                if (!string.IsNullOrEmpty(shipment.IncotermId))
                {
                    IncotermRepository incotermRepository = new IncotermRepository(shipment.Tenant);
                    Incoterm incoterm = incotermRepository.GetSingleIncoterm(shipment.IncotermId, shipment.Tenant);
                    if (incoterm != null)
                    {
                        myDataProvider.IncotermCode = incoterm.Code;
                        myDataProvider.IncotermName = incoterm.Name;

                        if (!string.IsNullOrEmpty(incoterm.Code))
                        {
                            if (incoterm.Code.ToUpper() == "COD")
                            {
                                myDataProvider.OpenReceivablesInLocalCurrency = shipment.OpenReceivablesInLocalCurrency;
                                myDataProvider.OpenReceivablesInProfitCurrency = shipment.OpenReceivablesInProfitCurrency;
                                myDataProvider.AccountedReceivablesInLocalCurrency = shipment.AccountedReceivablesInLocalCurrency;
                                myDataProvider.AccountedReceivablesInProfitCurrency = shipment.AccountedReceivablesInProfitCurrency;
                            }
                        }
                    }
                }

                if (documenttypecopy != null)
                {
                    myDataProvider.CopyName = documenttypecopy.Name;
                }

                if (shipment.ShipmentTypeId == "FCL" || shipment.ShipmentTypeId == "FTL" || shipment.ShipmentTypeId == "FCLD")
                {
                    myDataProvider.Containerized = true;
                }

                #region Ports
                Port preCarriageToPort = null;
                Port mainCarriageFromPort = null;
                Port mainCarriageToPort = null;
                Port onCarriageToPort = null;
                Card mainCarriageCarrier = null;

                if (shipment.PreCarriageToPortId != null)
                {
                    preCarriageToPort = (from a in commonContext.Ports.Include("State") where a.Id == shipment.PreCarriageToPortId select a).FirstOrDefault();
                }

                if (shipment.MainCarriageFromPortId != null)
                {
                    mainCarriageFromPort = (from a in commonContext.Ports.Include("State") where a.Id == shipment.MainCarriageFromPortId select a).FirstOrDefault();
                }

                if (shipment.MainCarriageToPortId != null)
                {
                    mainCarriageToPort = (from a in commonContext.Ports.Include("State") where a.Id == shipment.MainCarriageToPortId select a).FirstOrDefault();
                }

                if (shipment.OnCarriageToPortId != null)
                {
                    onCarriageToPort = (from a in commonContext.Ports.Include("State") where a.Id == shipment.OnCarriageToPortId select a).FirstOrDefault();
                }

                if (shipment.MainCarriageCarrierId != null)
                {
                    mainCarriageCarrier = (from a in commonContext.Cards where a.Id == shipment.MainCarriageCarrierId select a).FirstOrDefault();
                }

                if (!string.IsNullOrEmpty(shipment.CreatedByUserId))
                {
                    Contact createdByUser = contactRepository.GetSingleContact(shipment.CreatedByUserId, tenant);

                    if (createdByUser != null)
                    {
                        myDataProvider.CreatedBy = createdByUser.EnglishName;
                    }

                }
                #endregion

                #region Shipper
                string myShipperId = shipment.ShipperId;
                string myShipperAddressId = shipment.ShipperAddressId;
                if (!string.IsNullOrEmpty(myShipperId))
                {
                    Card myPartnerCard = CardRepository.GetSingleCard(myShipperId, tenant, true);

                    if (myPartnerCard != null)
                    {
                        myDataProvider.ShipperRef1 = shipment.ShipperReference1;
                        myDataProvider.ShipperRef2 = shipment.ShipperReference2;
                        myDataProvider.ShipperCode = myPartnerCard.Code;
                        myDataProvider.ShipperAddress = myPartnerCard.EnglishName != null ? myPartnerCard.EnglishName + Environment.NewLine : "";
                        myDataProvider.ShipperAddress_NoTel = myPartnerCard.EnglishName != null ? myPartnerCard.EnglishName + Environment.NewLine : "";
                        myDataProvider.ShipperAddress_NoTelFax = myPartnerCard.EnglishName != null ? myPartnerCard.EnglishName + Environment.NewLine : "";
                        myDataProvider.ShipperVAT = myPartnerCard.VatNumber;

                        if (!string.IsNullOrEmpty(myShipperAddressId))
                        {
                            Address myPartnerAddress = addressRepository.GetSingleAddress(myShipperAddressId, tenant);

                            if (myPartnerAddress != null)
                            {
                                if (myPartnerAddress.IsLocalLanguage && !string.IsNullOrEmpty(myPartnerCard.LocalName))
                                {
                                    myDataProvider.ShipperAddress = myPartnerCard.LocalName + Environment.NewLine;
                                    myDataProvider.ShipperAddress_NoTel = myPartnerCard.LocalName + Environment.NewLine;
                                    myDataProvider.ShipperAddress_NoTelFax = myPartnerCard.LocalName + Environment.NewLine;
                                }

                                myDataProvider.ShipperAddress_WithName = DataProviders.General.GetAddressWithName(myPartnerAddress);
                                myDataProvider.ShipperAddress = myDataProvider.ShipperAddress + DataProviders.General.GetAddress(myPartnerAddress);
                                myDataProvider.ShipperAddress_NoTel = myDataProvider.ShipperAddress_NoTel + DataProviders.General.GetAddress(myPartnerAddress);
                                myDataProvider.ShipperAddress_NoTelFax = myDataProvider.ShipperAddress_NoTelFax + DataProviders.General.GetAddress(myPartnerAddress);
                                myDataProvider.ShipperATTN = myPartnerAddress.ATTN;

                                if (myPartnerAddress.PhoneNumber != null || myPartnerAddress.FaxNumber != null)
                                {
                                    myDataProvider.ShipperAddress = myDataProvider.ShipperAddress + Environment.NewLine + (myPartnerAddress.PhoneNumber != null ? "Tel: " + myPartnerAddress.PhoneNumber + " " : "") + (myPartnerAddress.FaxNumber != null ? "Fax: " + myPartnerAddress.FaxNumber + " " : "");
                                }

                                if (myPartnerAddress.FaxNumber != null)
                                {
                                    myDataProvider.ShipperAddress_NoTel = myDataProvider.ShipperAddress_NoTel + Environment.NewLine + "Fax: " + myPartnerAddress.FaxNumber + " ";
                                }

                                myDataProvider.ShipperAddress_NoState = myDataProvider.ShipperAddress;

                                if (myPartnerAddress.State != null)
                                {
                                    myDataProvider.ShipperAddress = myDataProvider.ShipperAddress + ", " + myPartnerAddress.State.Code;
                                    myDataProvider.ShipperAddress_NoTel = myDataProvider.ShipperAddress_NoTel + ", " + myPartnerAddress.State.Code;
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(shipment.ShipperContactId))
                        {
                            Contact shipperContact = contactRepository.GetSingleContact(shipment.ShipperContactId, tenant);

                            if (shipperContact != null)
                            {
                                myDataProvider.ShipperContactDetails = shipperContact.EnglishName;

                                if (!string.IsNullOrEmpty(shipperContact.BusinessPhone))
                                {
                                    myDataProvider.ShipperContactDetails = myDataProvider.ShipperContactDetails + Environment.NewLine + "Ph: " + shipperContact.BusinessPhone;

                                    if (!string.IsNullOrEmpty(shipperContact.Fax))
                                    {
                                        myDataProvider.ShipperContactDetails = myDataProvider.ShipperContactDetails + " - Fx: " + shipperContact.Fax;
                                    }
                                }

                                else
                                {
                                    if (!string.IsNullOrEmpty(shipperContact.Fax))
                                    {
                                        myDataProvider.ShipperContactDetails = myDataProvider.ShipperContactDetails + Environment.NewLine + "Fx: " + shipperContact.Fax;
                                    }
                                }

                                if (!string.IsNullOrEmpty(shipperContact.Email))
                                {
                                    myDataProvider.ShipperContactDetails = myDataProvider.ShipperContactDetails + Environment.NewLine + "Email: " + shipperContact.Email;
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Consignee
                string myConsigneeId = !string.IsNullOrEmpty(shipment.ConsigneeNotImporterId) ? shipment.ConsigneeNotImporterId : shipment.ConsigneeId;
                string myConsigneeAddressId = !string.IsNullOrEmpty(shipment.ConsigneeNotImporterAddressId) ? shipment.ConsigneeNotImporterAddressId : shipment.ConsigneeAddressId;

                if (!string.IsNullOrEmpty(myConsigneeId))
                {
                    Card myPartnerCard = CardRepository.GetSingleCard(myConsigneeId, tenant, true);
                    if (myPartnerCard != null)
                    {
                        myDataProvider.ConsigneeRef1 = shipment.ConsigneeReference1;
                        myDataProvider.ConsigneeRef2 = shipment.ConsigneeReference2;
                        myDataProvider.ConsigneeCode = myPartnerCard.Code;
                        myDataProvider.ConsigneeAddress = myPartnerCard.EnglishName != null ? myPartnerCard.EnglishName + Environment.NewLine : "";
                        myDataProvider.ConsigneeAddress_NoState = myPartnerCard.EnglishName != null ? myPartnerCard.EnglishName + Environment.NewLine : "";

                        if (!string.IsNullOrEmpty(myConsigneeAddressId))
                        {
                            Address myPartnerAddress = addressRepository.GetSingleAddress(myConsigneeAddressId, tenant);

                            if (myPartnerAddress != null)
                            {
                                if (myPartnerAddress.IsLocalLanguage && !string.IsNullOrEmpty(myPartnerCard.LocalName))
                                {
                                    myDataProvider.ConsigneeAddress = myPartnerCard.LocalName + Environment.NewLine;
                                    myDataProvider.ConsigneeAddress_NoState = myPartnerCard.LocalName + Environment.NewLine;
                                }

                                myDataProvider.ConsigneeAddress_WithName = DataProviders.General.GetAddressWithName(myPartnerAddress);
                                myDataProvider.ConsigneeAddress = myDataProvider.ConsigneeAddress + DataProviders.General.GetAddress(myPartnerAddress);
                                myDataProvider.ConsigneeAddress_NoState = myDataProvider.ConsigneeAddress_NoState + DataProviders.General.GetAddress(myPartnerAddress);

                                if (myPartnerAddress.PhoneNumber != null || myPartnerAddress.FaxNumber != null)
                                {
                                    myDataProvider.ConsigneeAddress = myDataProvider.ConsigneeAddress + Environment.NewLine + (myPartnerAddress.PhoneNumber != null ? "Tel: " + myPartnerAddress.PhoneNumber + " " : "") + (myPartnerAddress.FaxNumber != null ? "Fax: " + myPartnerAddress.FaxNumber + " " : "");
                                    myDataProvider.ConsigneeAddress_NoState = myDataProvider.ConsigneeAddress_NoState + Environment.NewLine + (myPartnerAddress.PhoneNumber != null ? "Tel: " + myPartnerAddress.PhoneNumber + " " : "") + (myPartnerAddress.FaxNumber != null ? "Fax: " + myPartnerAddress.FaxNumber + " " : "");
                                }

                                if (myPartnerAddress.State != null)
                                {
                                    myDataProvider.ConsigneeAddress = myDataProvider.ConsigneeAddress + ", " + myPartnerAddress.State.Code;
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Consignee Always
                if (!string.IsNullOrEmpty(shipment.ConsigneeId))
                {
                    Card myPartnerCard = CardRepository.GetSingleCard(shipment.ConsigneeId, tenant, true);
                    if (myPartnerCard != null)
                    {
                        myDataProvider.ConsigneeAddress_NoTelFax = myPartnerCard.EnglishName != null ? myPartnerCard.EnglishName + Environment.NewLine : "";

                        myDataProvider.ConsigneeAlways = myPartnerCard.EnglishName != null ? myPartnerCard.EnglishName + Environment.NewLine : "";
                        myDataProvider.ConsigneeVAT = myPartnerCard.VatNumber;

                        if (!string.IsNullOrEmpty(shipment.ConsigneeAddressId))
                        {
                            Address myPartnerAddress = addressRepository.GetSingleAddress(shipment.ConsigneeAddressId, tenant);

                            if (myPartnerAddress != null)
                            {
                                myDataProvider.ConsigneeATTN = myPartnerAddress.ATTN;

                                if (myPartnerAddress.IsLocalLanguage && !string.IsNullOrEmpty(myPartnerCard.LocalName))
                                {
                                    myDataProvider.ConsigneeAlways = myPartnerCard.LocalName + Environment.NewLine;
                                    myDataProvider.ConsigneeAddress_NoTelFax = myPartnerCard.LocalName + Environment.NewLine;
                                }

                                myDataProvider.ConsigneeAlways = myDataProvider.ConsigneeAlways + DataProviders.General.GetAddress(myPartnerAddress);
                                myDataProvider.ConsigneeAddress_NoTelFax = myDataProvider.ConsigneeAddress_NoTelFax + DataProviders.General.GetAddress(myPartnerAddress);

                                if (myPartnerAddress.PhoneNumber != null || myPartnerAddress.FaxNumber != null)
                                {
                                    myDataProvider.ConsigneeAlways = myDataProvider.ConsigneeAlways + Environment.NewLine + (myPartnerAddress.PhoneNumber != null ? "Tel: " + myPartnerAddress.PhoneNumber + " " : "") + (myPartnerAddress.FaxNumber != null ? "Fax: " + myPartnerAddress.FaxNumber + " " : "");
                                }

                                if (myPartnerAddress.State != null)
                                {
                                    myDataProvider.ConsigneeAlways = myDataProvider.ConsigneeAlways + ", " + myPartnerAddress.State.Code;
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(shipment.ConsigneeContactId))
                        {
                            Contact consigneeContact = contactRepository.GetSingleContact(shipment.ConsigneeContactId, tenant);

                            if (consigneeContact != null)
                            {
                                myDataProvider.ConsigneeContactDetails = consigneeContact.EnglishName;

                                if (!string.IsNullOrEmpty(consigneeContact.BusinessPhone))
                                {
                                    myDataProvider.ConsigneeContactDetails = myDataProvider.ConsigneeContactDetails + Environment.NewLine + "Ph: " + consigneeContact.BusinessPhone;

                                    if (!string.IsNullOrEmpty(consigneeContact.Fax))
                                    {
                                        myDataProvider.ConsigneeContactDetails = myDataProvider.ConsigneeContactDetails + " - Fx: " + consigneeContact.Fax;
                                    }
                                }

                                else
                                {
                                    if (!string.IsNullOrEmpty(consigneeContact.Fax))
                                    {
                                        myDataProvider.ConsigneeContactDetails = myDataProvider.ConsigneeContactDetails + Environment.NewLine + "Fx: " + consigneeContact.Fax;
                                    }
                                }

                                if (!string.IsNullOrEmpty(consigneeContact.Email))
                                {
                                    myDataProvider.ConsigneeContactDetails = myDataProvider.ConsigneeContactDetails + Environment.NewLine + "Email: " + consigneeContact.Email;
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Notify1
                string myNotify1Id = !string.IsNullOrEmpty(shipment.Notify1Id) ? shipment.Notify1Id : shipment.ConsigneeId;
                string myNotify1AddressId = !string.IsNullOrEmpty(shipment.Notify1AddressId) ? shipment.Notify1AddressId : shipment.ConsigneeAddressId;

                if (!string.IsNullOrEmpty(myNotify1Id))
                {
                    Card myPartnerCard = CardRepository.GetSingleCard(myNotify1Id, tenant, true);

                    if (myPartnerCard != null)
                    {
                        myDataProvider.NotifyAddress = myPartnerCard.EnglishName != null ? myPartnerCard.EnglishName + Environment.NewLine : "";

                        if (!string.IsNullOrEmpty(myNotify1AddressId))
                        {
                            Address myPartnerAddress = addressRepository.GetSingleAddress(myNotify1AddressId, tenant);

                            if (myPartnerAddress != null)
                            {
                                if (myPartnerAddress.IsLocalLanguage && !string.IsNullOrEmpty(myPartnerCard.LocalName))
                                {
                                    myDataProvider.NotifyAddress = myPartnerCard.LocalName + Environment.NewLine;
                                }

                                myDataProvider.NotifyAddress_WithName = DataProviders.General.GetAddressWithName(myPartnerAddress);
                                myDataProvider.NotifyAddress = myDataProvider.NotifyAddress + DataProviders.General.GetAddress(myPartnerAddress);

                                if (myPartnerAddress.PhoneNumber != null || myPartnerAddress.FaxNumber != null)
                                {
                                    myDataProvider.NotifyAddress = myDataProvider.NotifyAddress + Environment.NewLine + (myPartnerAddress.PhoneNumber != null ? "Tel: " + myPartnerAddress.PhoneNumber + " " : "") + (myPartnerAddress.FaxNumber != null ? "Fax: " + myPartnerAddress.FaxNumber + " " : "");
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(shipment.Notify1ContactId))
                        {
                            Contact myContact = contactRepository.GetSingleContact(shipment.Notify1ContactId, tenant);

                            if (myContact != null)
                            {
                                myDataProvider.Notify1ContactDetails = myContact.EnglishName;

                                if (!string.IsNullOrEmpty(myContact.BusinessPhone))
                                {
                                    myDataProvider.Notify1ContactDetails = myDataProvider.Notify1ContactDetails + Environment.NewLine + "Ph: " + myContact.BusinessPhone;

                                    if (!string.IsNullOrEmpty(myContact.Fax))
                                    {
                                        myDataProvider.Notify1ContactDetails = myDataProvider.Notify1ContactDetails + " - Fx: " + myContact.Fax;
                                    }
                                }

                                else
                                {
                                    if (!string.IsNullOrEmpty(myContact.Fax))
                                    {
                                        myDataProvider.Notify1ContactDetails = myDataProvider.Notify1ContactDetails + Environment.NewLine + "Fx: " + myContact.Fax;
                                    }
                                }

                                if (!string.IsNullOrEmpty(myContact.Email))
                                {
                                    myDataProvider.Notify1ContactDetails = myDataProvider.Notify1ContactDetails + Environment.NewLine + "Email: " + myContact.Email;
                                }
                            }
                        }
                    }
                }

                //Actual
                if (!string.IsNullOrEmpty(shipment.Notify1Id))
                {
                    if (!string.IsNullOrEmpty(shipment.Notify1AddressId))
                    {
                        Address myPartnerAddress = addressRepository.GetSingleAddress(shipment.Notify1AddressId, tenant);

                        if (myPartnerAddress != null)
                        {
                            myDataProvider.Notify1ATTN = myPartnerAddress.ATTN;
                        }
                    }
                }
                #endregion

                #region Notify2
                string myNotify2Id = shipment.Notify2Id;
                string myNotify2AddressId = shipment.Notify2AddressId;

                if (!string.IsNullOrEmpty(myNotify2Id))
                {
                    Card myPartnerCard = CardRepository.GetSingleCard(myNotify2Id, tenant, true);

                    if (myPartnerCard != null)
                    {
                        myDataProvider.Notify2Address = "Notify 2:" + myPartnerCard.EnglishName != null ? myPartnerCard.EnglishName + Environment.NewLine : "";

                        if (!string.IsNullOrEmpty(myNotify2AddressId))
                        {
                            Address myPartnerAddress = addressRepository.GetSingleAddress(myNotify2AddressId, tenant);

                            if (myPartnerAddress != null)
                            {
                                if (myPartnerAddress.IsLocalLanguage && !string.IsNullOrEmpty(myPartnerCard.LocalName))
                                {
                                    myDataProvider.Notify2Address = "Notify 2:" + myPartnerCard.LocalName + Environment.NewLine;
                                }

                                myDataProvider.NotifyAddress2_WithName = DataProviders.General.GetAddressWithName(myPartnerAddress);
                                myDataProvider.Notify2Address = myDataProvider.Notify2Address + DataProviders.General.GetAddress(myPartnerAddress);
                                myDataProvider.Notify2ATTN = myPartnerAddress.ATTN;

                                if (myPartnerAddress.PhoneNumber != null || myPartnerAddress.FaxNumber != null)
                                {
                                    myDataProvider.Notify2Address = myDataProvider.Notify2Address + Environment.NewLine + (myPartnerAddress.PhoneNumber != null ? "Tel: " + myPartnerAddress.PhoneNumber + " " : "") + (myPartnerAddress.FaxNumber != null ? "Fax: " + myPartnerAddress.FaxNumber + " " : "");
                                }
                            }
                        }

                        myDataProvider.NotifyAddress = myDataProvider.NotifyAddress + " *";
                        myDataProvider.Notify2Address = "* " + myDataProvider.Notify2Address;

                        if (!string.IsNullOrEmpty(shipment.Notify2ContactId))
                        {
                            Contact myContact = contactRepository.GetSingleContact(shipment.Notify2ContactId, tenant);

                            if (myContact != null)
                            {
                                myDataProvider.Notify2ContactDetails = myContact.EnglishName;

                                if (!string.IsNullOrEmpty(myContact.BusinessPhone))
                                {
                                    myDataProvider.Notify2ContactDetails = myDataProvider.Notify2ContactDetails + Environment.NewLine + "Ph: " + myContact.BusinessPhone;

                                    if (!string.IsNullOrEmpty(myContact.Fax))
                                    {
                                        myDataProvider.Notify2ContactDetails = myDataProvider.Notify2ContactDetails + " - Fx: " + myContact.Fax;
                                    }
                                }

                                else
                                {
                                    if (!string.IsNullOrEmpty(myContact.Fax))
                                    {
                                        myDataProvider.Notify2ContactDetails = myDataProvider.Notify2ContactDetails + Environment.NewLine + "Fx: " + myContact.Fax;
                                    }
                                }

                                if (!string.IsNullOrEmpty(myContact.Email))
                                {
                                    myDataProvider.Notify2ContactDetails = myDataProvider.Notify2ContactDetails + Environment.NewLine + "Email: " + myContact.Email;
                                }
                            }
                        }
                    }
                }
                #endregion

                #region FreightForwarder
                string myForwarderId = shipment.FreightForwarderId;
                string myForwarderAddressId = shipment.FreightForwarderAddressId;
                if (!string.IsNullOrEmpty(myForwarderId))
                {
                    Card myPartnerCard = CardRepository.GetSingleCard(myForwarderId, tenant, true);

                    if (myPartnerCard != null)
                    {
                        myDataProvider.ForwarderAgentCode = myPartnerCard.Code;
                        myDataProvider.ForwarderAgentAddress = myPartnerCard.EnglishName != null ? myPartnerCard.EnglishName + Environment.NewLine : "";

                        if (!string.IsNullOrEmpty(myForwarderAddressId))
                        {
                            Address myPartnerAddress = addressRepository.GetSingleAddress(myForwarderAddressId, tenant);

                            if (myPartnerAddress != null)
                            {
                                if (myPartnerAddress.IsLocalLanguage && !string.IsNullOrEmpty(myPartnerCard.LocalName))
                                {
                                    myDataProvider.ForwarderAgentAddress = myPartnerCard.LocalName + Environment.NewLine;
                                }

                                myDataProvider.ForwarderAgentAddress = myDataProvider.ForwarderAgentAddress + DataProviders.General.GetAddress(myPartnerAddress);

                                if (myPartnerAddress.PhoneNumber != null || myPartnerAddress.FaxNumber != null)
                                {
                                    myDataProvider.ForwarderAgentAddress = myDataProvider.ForwarderAgentAddress + Environment.NewLine + (myPartnerAddress.PhoneNumber != null ? "Tel: " + myPartnerAddress.PhoneNumber + " " : "") + (myPartnerAddress.FaxNumber != null ? "Fax: " + myPartnerAddress.FaxNumber + " " : "");
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Agent

                string myAgentId = shipment.AgentId;
                string myAgentAddressId = shipment.AgentAddressId;
                string agentContactId = shipment.AgentContactId;

                if (string.IsNullOrEmpty(myAgentId))
                {
                    if (shipment.ShipmentLevelCode == "H" && !string.IsNullOrEmpty(shipment.MasterShipmentDataId))
                    {
                        Shipment myMasterShipment = shipmentRepository.GetSingleShipment(shipment.MasterShipmentDataId, tenant);
                        if (myMasterShipment != null)
                        {
                            myAgentId = myMasterShipment.AgentId;
                            myAgentAddressId = myMasterShipment.AgentAddressId;
                            agentContactId = myMasterShipment.AgentContactId;
                        }
                    }
                }

                this.SetAgentContact(myDataProvider, agentContactId);

                if (!string.IsNullOrEmpty(myAgentId))
                {
                    Card myCard = CardRepository.GetSingleCard(myAgentId, tenant, true);
                    if (myCard != null)
                    {
                        myDataProvider.AgentCode = myCard.Code;
                        myDataProvider.AgentInfo = myCard.EnglishName;
                        myDataProvider.AgentFullDetails = myCard.EnglishName;
                        myDataProvider.AgentVAT = myCard.VatNumber;

                        Address myAddress = addressRepository.GetSingleAddress(myAgentAddressId, tenant);
                        if (myAddress != null)
                        {
                            myDataProvider.AgentInfo = myDataProvider.AgentInfo + ", " + myAddress.City + ", " + myAddress.Country.Code + Environment.NewLine;
                            myDataProvider.AgentFullDetails = myCard.EnglishName + Environment.NewLine + DataProviders.General.GetAddress(myAddress);

                            myDataProvider.AgentPhone = myAddress.PhoneNumber;
                            myDataProvider.AgentFax = myAddress.FaxNumber;
                            myDataProvider.AgentATTN = myAddress.ATTN;
                        }

                        if (!string.IsNullOrEmpty(myCard.PrimaryContactId))
                        {
                            Contact myContact = contactRepository.GetSingleContact(myCard.PrimaryContactId, tenant);
                            if (myContact != null)
                            {
                                myDataProvider.AgentInfo = myDataProvider.AgentInfo + myContact.EnglishName + ", " + myContact.BusinessPhone;

                                string agentContactDetails = "";
                                agentContactDetails = myContact.EnglishName;

                                if (!string.IsNullOrEmpty(myContact.Email))
                                {
                                    agentContactDetails = agentContactDetails + ", mail: " + myContact.Email;
                                }

                                if (!string.IsNullOrEmpty(myContact.BusinessPhone))
                                {
                                    agentContactDetails = agentContactDetails + ", Tel: " + myContact.BusinessPhone;
                                }

                                myDataProvider.AgentPrimaryContactDetails = agentContactDetails;
                            }
                        }

                        else
                        {
                            CardContactRepository cardContactRepository = new CardContactRepository(tenant);
                            IQueryable<Contact> myContacts = cardContactRepository.GetContactsByCardId(myCard.Id);
                            Contact myContact = myContacts.FirstOrDefault();
                            if (myContact != null)
                            {
                                myDataProvider.AgentInfo = myDataProvider.AgentInfo + myContact.EnglishName + ", " + myContact.BusinessPhone;
                            }
                        }
                    }
                }
                #endregion

                #region ConsigneeNotImporter
                if (!string.IsNullOrEmpty(shipment.ConsigneeNotImporterId))
                {
                    Card myPartnerCard = CardRepository.GetSingleCard(shipment.ConsigneeNotImporterId, tenant, true);

                    if (myPartnerCard != null)
                    {
                        myDataProvider.ConsigneeNotImporterAddress = myPartnerCard.EnglishName != null ? myPartnerCard.EnglishName + Environment.NewLine : "";
                    }

                    if (!string.IsNullOrEmpty(shipment.ConsigneeNotImporterAddressId))
                    {
                        Address myPartnerAddress = addressRepository.GetSingleAddress(shipment.ConsigneeNotImporterAddressId, tenant);

                        if (myPartnerAddress != null)
                        {
                            myDataProvider.ConsigneeNotImporterATTN = myPartnerAddress.ATTN;

                            if (myPartnerAddress.IsLocalLanguage && !string.IsNullOrEmpty(myPartnerCard.LocalName))
                            {
                                myDataProvider.ConsigneeNotImporterAddress = myPartnerCard.LocalName + Environment.NewLine;
                            }

                            myDataProvider.ConsigneeNotImporterAddress = myDataProvider.ConsigneeNotImporterAddress + DataProviders.General.GetAddress(myPartnerAddress);

                            if (myPartnerAddress.PhoneNumber != null || myPartnerAddress.FaxNumber != null)
                            {
                                myDataProvider.ConsigneeNotImporterAddress = myDataProvider.ConsigneeNotImporterAddress + Environment.NewLine + (myPartnerAddress.PhoneNumber != null ? "Tel: " + myPartnerAddress.PhoneNumber + " " : "") + (myPartnerAddress.FaxNumber != null ? "Fax: " + myPartnerAddress.FaxNumber + " " : "");
                            }

                            myDataProvider.ConsigneeNotImporterAddress_NoState = myDataProvider.ConsigneeNotImporterAddress;
                            if (myPartnerAddress.State != null)
                            {
                                myDataProvider.ConsigneeNotImporterAddress = myDataProvider.ConsigneeNotImporterAddress + ", " + myPartnerAddress.State.Code;
                            }
                        }
                    }
                }
                #endregion

                #region ShipperNotExporter
                if (!string.IsNullOrEmpty(shipment.ShipperNotExporterId))
                {
                    Card myPartnerCard = CardRepository.GetSingleCard(shipment.ShipperNotExporterId, tenant, true);

                    if (myPartnerCard != null)
                    {
                        myDataProvider.ShipperNotExporterAddress = myPartnerCard.EnglishName != null ? myPartnerCard.EnglishName + Environment.NewLine : "";
                    }

                    if (!string.IsNullOrEmpty(shipment.ShipperNotExporterAddressId))
                    {
                        Address myPartnerAddress = addressRepository.GetSingleAddress(shipment.ShipperNotExporterAddressId, tenant);

                        if (myPartnerAddress != null)
                        {
                            myDataProvider.ShipperNotExporterATTN = myPartnerAddress.ATTN;

                            if (myPartnerAddress.IsLocalLanguage && !string.IsNullOrEmpty(myPartnerCard.LocalName))
                            {
                                myDataProvider.ShipperNotExporterAddress = myPartnerCard.LocalName + Environment.NewLine;
                            }

                            myDataProvider.ShipperNotExporterAddress_WithName = DataProviders.General.GetAddressWithName(myPartnerAddress);
                            myDataProvider.ShipperNotExporterAddress = myDataProvider.ShipperNotExporterAddress + DataProviders.General.GetAddress(myPartnerAddress);

                            if (myPartnerAddress.PhoneNumber != null || myPartnerAddress.FaxNumber != null)
                            {
                                myDataProvider.ShipperNotExporterAddress = myDataProvider.ShipperNotExporterAddress + Environment.NewLine + (myPartnerAddress.PhoneNumber != null ? "Tel: " + myPartnerAddress.PhoneNumber + " " : "") + (myPartnerAddress.FaxNumber != null ? "Fax: " + myPartnerAddress.FaxNumber + " " : "");
                            }

                            myDataProvider.ShipperNotExporterAddress_NoState = myDataProvider.ShipperNotExporterAddress;

                            if (myPartnerAddress.State != null)
                            {
                                myDataProvider.ShipperNotExporterAddress = myDataProvider.ShipperNotExporterAddress + ", " + myPartnerAddress.State.Code;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(shipment.ShipperNotExporterContactId))
                    {
                        Contact contact = contactRepository.GetSingleContact(shipment.ShipperNotExporterContactId, tenant);

                        if (contact != null)
                        {
                            myDataProvider.ShipperNotExporterContactDetails = contact.EnglishName;

                            if (!string.IsNullOrEmpty(contact.BusinessPhone))
                            {
                                myDataProvider.ShipperNotExporterContactDetails = myDataProvider.ShipperNotExporterContactDetails + Environment.NewLine + "Ph: " + contact.BusinessPhone;

                                if (!string.IsNullOrEmpty(contact.Fax))
                                {
                                    myDataProvider.ShipperNotExporterContactDetails = myDataProvider.ShipperNotExporterContactDetails + " - Fx: " + contact.Fax;
                                }
                            }

                            else
                            {
                                if (!string.IsNullOrEmpty(contact.Fax))
                                {
                                    myDataProvider.ShipperNotExporterContactDetails = myDataProvider.ShipperNotExporterContactDetails + Environment.NewLine + "Fx: " + contact.Fax;
                                }
                            }

                            if (!string.IsNullOrEmpty(contact.Email))
                            {
                                myDataProvider.ShipperNotExporterContactDetails = myDataProvider.ShipperNotExporterContactDetails + Environment.NewLine + "Email: " + contact.Email;
                            }
                        }
                    }
                }
                #endregion

                #region First Pick up

                ShipmentPickUpDelivery myFirstPickup
                    = (from d in shipmentsContext.ShipmentPickUpDeliveries
                       where d.ShipmentId == shipment.Id && d.PickUpDeliveryTypeCode == "PICK"
                       select d).OrderBy(s => s.PickUpDeliveryNumber).FirstOrDefault();

                if (myFirstPickup == null)
                {
                    PortPM preCarriageFromPort = PortQuery.GetSinglePort(tenant, shipment.PreCarriageFromPortId, true);
                    PortPM preForwardingFromPort = PortQuery.GetSinglePort(tenant, shipment.PreForwardingFromPortId, true);

                    //FirstPickupAddress
                    if (!string.IsNullOrEmpty(shipment.ShipperNotExporterAddressId))
                    {
                        Address myAddress = addressRepository.GetSingleAddress(shipment.ShipperNotExporterAddressId, tenant);
                        if (myAddress != null)
                        {
                            myDataProvider.FirstPickupAddress = myAddress.City != null ? myAddress.City : "";
                        }
                    }

                    else if (preForwardingFromPort != null)
                    {
                        myDataProvider.FirstPickupAddress = preForwardingFromPort.EnglishName;
                    }

                    else if (preCarriageFromPort != null)
                    {
                        myDataProvider.FirstPickupAddress = preCarriageFromPort.EnglishName;
                    }

                    //PlaceOfReceipt
                    if (preForwardingFromPort != null)
                    {
                        myDataProvider.PlaceOfReceipt = preForwardingFromPort.EnglishName;
                    }

                    else if (preCarriageFromPort != null)
                    {
                        myDataProvider.PlaceOfReceipt = preCarriageFromPort.EnglishName;
                    }

                    else if (!string.IsNullOrEmpty(shipment.ShipperAddressId))
                    {
                        Address myPartnerAddress = addressRepository.GetSingleAddress(shipment.ShipperAddressId, tenant);
                        if (myPartnerAddress != null)
                        {
                            myDataProvider.PlaceOfReceipt = myPartnerAddress.City;

                            if (myPartnerAddress.State != null)
                            {
                                myDataProvider.PlaceOfReceipt = myDataProvider.PlaceOfReceipt + ", " + myPartnerAddress.State.Code;
                            }
                        }
                    }
                }

                else if (myFirstPickup != null)
                {
                    #region
                    myDataProvider.PickupDate = myFirstPickup.ETD != null ? String.Format("{0:dd/MMM/yyyy}", myFirstPickup.ETD) : "";

                    //FirstPickupAddress
                    switch (myFirstPickup.PickUpDeliveryFromTypeCode)
                    {
                        case "PART":
                            {
                                if (!string.IsNullOrEmpty(myFirstPickup.FromPartnerCardId))
                                {
                                    Card myPartnerCard = CardRepository.GetSingleCard(myFirstPickup.FromPartnerCardId, tenant, true);

                                    if (myPartnerCard != null)
                                    {
                                        myDataProvider.FirstPickupAddress = myPartnerCard.EnglishName != null ? myPartnerCard.EnglishName + Environment.NewLine : "";

                                        Address myPartnerAddress = addressRepository.GetSingleAddress(myFirstPickup.FromAddressId, tenant);

                                        if (myPartnerAddress != null)
                                        {
                                            if (!string.IsNullOrEmpty(myPartnerAddress.City))
                                            {
                                                myDataProvider.PlaceOfReceipt = myPartnerAddress.City;
                                                myDataProvider.PlaceOfReceiptPickUp = myPartnerAddress.City;
                                            }

                                            if (myPartnerAddress.Country != null)
                                            {
                                                if (!string.IsNullOrEmpty(myPartnerAddress.Country.EnglishName))
                                                {
                                                    myDataProvider.PlaceOfReceiptCountryName = myPartnerAddress.Country.EnglishName;
                                                }
                                            }

                                            if (myPartnerAddress.State != null)
                                            {
                                                myDataProvider.PlaceOfReceipt = myDataProvider.PlaceOfReceipt + ", " + myPartnerAddress.State.Code;
                                                myDataProvider.PlaceOfReceiptPickUp = myDataProvider.PlaceOfReceiptPickUp + ", " + myPartnerAddress.State.Code;
                                            }

                                            if (myPartnerAddress.IsLocalLanguage && !string.IsNullOrEmpty(myPartnerCard.LocalName))
                                            {
                                                myDataProvider.FirstPickupAddress = myPartnerCard.LocalName + Environment.NewLine;
                                            }

                                            myDataProvider.FirstPickupAddress = myDataProvider.FirstPickupAddress + DataProviders.General.GetAddress(myPartnerAddress);

                                            if (myPartnerAddress.PhoneNumber != null || myPartnerAddress.FaxNumber != null)
                                            {
                                                myDataProvider.FirstPickupAddress = myDataProvider.FirstPickupAddress + Environment.NewLine + (myPartnerAddress.PhoneNumber != null ? "Tel: " + myPartnerAddress.PhoneNumber + " " : "") + (myPartnerAddress.FaxNumber != null ? "Fax: " + myPartnerAddress.FaxNumber + " " : "");
                                            }
                                        }
                                    }
                                }

                                break;
                            }

                        case "PORT":
                            {
                                if (!string.IsNullOrEmpty(myFirstPickup.FromPortId))
                                {
                                    Port myPort = portRepository.GetSinglePort(tenant, myFirstPickup.FromPortId);

                                    if (myPort != null)
                                    {
                                        if (!string.IsNullOrEmpty(myPort.EnglishName))
                                        {
                                            myDataProvider.PlaceOfReceipt = myPort.EnglishName;
                                            myDataProvider.PlaceOfReceiptPickUp = myPort.EnglishName;
                                        }

                                        if (myPort.State != null)
                                        {
                                            myDataProvider.PlaceOfReceipt = myDataProvider.PlaceOfReceipt + ", " + myPort.State.Code;
                                            myDataProvider.PlaceOfReceiptPickUp = myDataProvider.PlaceOfReceiptPickUp + ", " + myPort.State.Code;
                                        }

                                        if (myPort.Country != null)
                                        {
                                            if (!string.IsNullOrEmpty(myPort.Country.EnglishName))
                                            {
                                                myDataProvider.PlaceOfReceiptCountryName = myPort.Country.EnglishName;
                                            }
                                        }

                                        myDataProvider.FirstPickupAddress = myDataProvider.PlaceOfReceipt;
                                    }
                                }

                                break;
                            }

                        case "CASL":
                            {
                                string myCity = myFirstPickup.FromAddressCity;
                                string myCode = myFirstPickup.FromAddressZipCode;

                                if (!string.IsNullOrEmpty(myCity))
                                {
                                    myDataProvider.PlaceOfReceipt = myCity;
                                    myDataProvider.FirstPickupAddress = myCity;
                                    myDataProvider.PlaceOfReceiptPickUp = myCity;
                                }

                                if (!string.IsNullOrEmpty(myCode))
                                {
                                    myDataProvider.FirstPickupAddress = string.IsNullOrEmpty(myDataProvider.FirstPickupAddress) ? myCode : myDataProvider.FirstPickupAddress + " " + myCode;
                                }

                                if (!string.IsNullOrEmpty(myFirstPickup.FromAddressCountryId))
                                {
                                    Country myCountry = CountryRepository.GetSingleCountry(myFirstPickup.FromAddressCountryId, tenant, true);

                                    if (myCountry != null)
                                    {
                                        myDataProvider.PlaceOfReceiptCountryName = myCountry.EnglishName;

                                        myDataProvider.FirstPickupAddress = string.IsNullOrEmpty(myDataProvider.FirstPickupAddress) ? myCountry.EnglishName : myDataProvider.FirstPickupAddress + Environment.NewLine + myCountry.EnglishName;
                                    }
                                }

                                break;
                            }
                    }

                    #endregion
                }
                #endregion

                #region First Delivery

                ShipmentPickUpDelivery myFirstDelivery
                    = (from d in shipmentsContext.ShipmentPickUpDeliveries
                       where d.ShipmentId == shipment.Id && d.PickUpDeliveryTypeCode == "DELV"
                       select d).OrderBy(s => s.PickUpDeliveryNumber).FirstOrDefault();

                if (myFirstDelivery != null)
                {
                    switch (myFirstDelivery.PickUpDeliveryToTypeCode)
                    {
                        case "PART":
                            {
                                if (!string.IsNullOrEmpty(myFirstDelivery.ToPartnerCardId))
                                {
                                    if (!string.IsNullOrEmpty(myFirstDelivery.ToAddressId))
                                    {
                                        Address myAddress = addressRepository.GetSingleAddress(myFirstDelivery.ToAddressId, tenant);

                                        if (myAddress != null)
                                        {
                                            if (!string.IsNullOrEmpty(myAddress.City))
                                            {
                                                myDataProvider.PlaceOfDelivery = myAddress.City;
                                            }

                                            if (myAddress.State != null)
                                            {
                                                myDataProvider.PlaceOfDelivery = myDataProvider.PlaceOfDelivery + ", " + myAddress.State.Code;
                                            }

                                            if (myAddress.Country != null)
                                            {
                                                if (!string.IsNullOrEmpty(myAddress.Country.EnglishName))
                                                {
                                                    myDataProvider.PlaceOfDeliveryCountryName = myAddress.Country.EnglishName;
                                                }
                                            }
                                        }
                                    }
                                }

                                break;
                            }

                        case "PORT":
                            {
                                if (!string.IsNullOrEmpty(myFirstDelivery.ToPortId))
                                {
                                    Port myPort = portRepository.GetSinglePort(tenant, myFirstDelivery.ToPortId);

                                    if (myPort != null)
                                    {
                                        if (!string.IsNullOrEmpty(myPort.EnglishName))
                                        {
                                            myDataProvider.PlaceOfDelivery = myPort.EnglishName;
                                        }

                                        if (myPort.State != null)
                                        {
                                            myDataProvider.PlaceOfDelivery = myDataProvider.PlaceOfDelivery + ", " + myPort.State.Code;
                                        }

                                        if (myPort.Country != null)
                                        {
                                            if (!string.IsNullOrEmpty(myPort.Country.EnglishName))
                                            {
                                                myDataProvider.PlaceOfDeliveryCountryName = myPort.Country.EnglishName;
                                            }
                                        }
                                    }
                                }

                                break;
                            }

                        case "CASL":
                            {
                                if (!string.IsNullOrEmpty(myFirstDelivery.ToAddressCity))
                                {
                                    myDataProvider.PlaceOfDelivery = myFirstDelivery.ToAddressCity;
                                }

                                if (!string.IsNullOrEmpty(myFirstDelivery.ToAddressCountryId))
                                {
                                    Country myCountry = CountryRepository.GetSingleCountry(myFirstDelivery.ToAddressCountryId, tenant, true);

                                    if (myCountry != null)
                                    {
                                        myDataProvider.PlaceOfDeliveryCountryName = myCountry.EnglishName;
                                    }
                                }

                                break;
                            }
                    }
                }
                #endregion
                                
                myDataProvider.PreForwardingFromPort = shipment.PreForwardingFromPortName;
                myDataProvider.PreForwardingBy = shipment.PreForwardingCarrierName;
                myDataProvider.PreForwardingVesselName = shipment.PreForwardingVesselName;
                myDataProvider.OnForwardingToPort = shipment.OnForwardingToPortName;

                #region Pre Carriage
                if (!string.IsNullOrEmpty(shipment.PreCarriageFromPortId) && !string.IsNullOrEmpty(shipment.PreCarriageToPortId))
                {
                    PortPM myPort = PortQuery.GetSinglePort(tenant, shipment.PreCarriageFromPortId, true);
                    myDataProvider.PreCarriageFromPort = myPort.EnglishName;

                    if (!string.IsNullOrEmpty(shipment.PreCarriageCarrierId))
                    {
                        Card myCard = CardRepository.GetSingleCard(shipment.PreCarriageCarrierId, tenant, true);
                        myDataProvider.PreCarriageBy = myCard.EnglishName;
                    }
                }
                #endregion

                #region Main carriage
                if (mainCarriageCarrier != null)
                {
                    myDataProvider.MainCarriageCarrierName = mainCarriageCarrier.EnglishName;

                    if (mainCarriageCarrier.PartnerTypeId == "SL")
                    {
                        ShippingLineRepository shippingLineRepository = new ShippingLineRepository(tenant);
                        ShippingLine shippingLine = shippingLineRepository.GetSingleShippingLine(mainCarriageCarrier.Id, tenant);
                        myDataProvider.CarrierCAAT = shippingLine != null ? shippingLine.CAAT : null;
                        myDataProvider.CarrierCBSA = shippingLine != null ? shippingLine.CBSA : null;
                    }
                }
                #endregion
                 
                #region Move Type
                if (!string.IsNullOrEmpty(shipment.MoveTypeId))
                {
                    MoveType moveType = webFreightContext.MoveTypes.Where(m => m.Id == shipment.MoveTypeId).FirstOrDefault();

                    if (moveType != null)
                    {
                        myDataProvider.MoveTypeCode = moveType.Code;
                        myDataProvider.MoveTypeName = moveType.MoveTypeEnglishName;
                    }
                }
                #endregion

                #region Tenant Info
                if (tenantSettings.Address != null)
                {
                    myDataProvider.TenantCountryCode = tenantSettings.Address.Country.Code;
                    myDataProvider.Signature = tenantSettings.Signature;
                }
                #endregion

                #region Charge Types

                List<ReceivablesCharges> shipmentReceivables = (from a in shipmentsContext.ShipmentReceivables.Include("ChargesType").Include("Currency")
                                                                where a.Tenant == tenant && a.ShipmentId == shipment.Id
                                                                group a by new { a.ChargesTypeId, a.PrepaidCollectId, a.Notes, a.CurrencyId } into g
                                                                where ((g.Select(s => s.PrepaidCollectId).FirstOrDefault() == "P" || g.Select(s => s.PrepaidCollectId).FirstOrDefault() == "C") && (g.Sum(s => s.TotalAmount) != 0 && g.Sum(s => s.TotalAmount) != null))
                                                                select new ReceivablesCharges
                                                                {
                                                                    ChargeTypeEnglish = g.Select(s => s.ChargesType.EnglishName).FirstOrDefault(),
                                                                    ChargeTypeLocal = g.Select(s => s.ChargesType.EnglishName).FirstOrDefault(),
                                                                    PrepaidChargeAmount = g.Select(s => s.PrepaidCollectId).FirstOrDefault() == "P" ? g.Sum(s => s.TotalAmount) : 0,
                                                                    PrepaidChargeAmountInProfitCurrency = g.Select(s => s.PrepaidCollectId).FirstOrDefault() == "P" ? g.Sum(s => s.AmountInProfitCurrency) : 0,
                                                                    PrepaidChargeAmountInLocalCurrency = g.Select(s => s.PrepaidCollectId).FirstOrDefault() == "P" ? g.Sum(s => s.TotalAmountLocal) : 0,
                                                                    CollectChargeAmount = g.Select(s => s.PrepaidCollectId).FirstOrDefault() == "C" ? g.Sum(s => s.TotalAmount) : 0,
                                                                    CollectChargeAmountInProfitCurrency = g.Select(s => s.PrepaidCollectId).FirstOrDefault() == "C" ? g.Sum(s => s.AmountInProfitCurrency) : 0,
                                                                    CollectChargeAmountInLocalCurrency = g.Select(s => s.PrepaidCollectId).FirstOrDefault() == "C" ? g.Sum(s => s.TotalAmountLocal) : 0,
                                                                    Remark = g.Select(s => s.Notes).FirstOrDefault(),
                                                                    CurrencyCode = g.Select(s => s.Currency.Code).FirstOrDefault(),
                                                                    CurrencyName = g.Select(s => s.Currency.EnglishName).FirstOrDefault(),
                                                                }).ToList();


                myDataProvider.ReceivablesCharges = shipmentReceivables;
                if (shipmentReceivables != null)
                {
                    myDataProvider.TotalPrepaid = shipmentReceivables.Sum(a => a.PrepaidChargeAmount);
                    myDataProvider.TotalPrepaidInProfitCurrency = shipmentReceivables.Sum(a => a.PrepaidChargeAmountInProfitCurrency);
                    myDataProvider.TotalPrepaidInLocalCurrency = shipmentReceivables.Sum(a => a.PrepaidChargeAmountInLocalCurrency);
                    myDataProvider.TotalCollect = shipmentReceivables.Sum(a => a.CollectChargeAmount);
                    myDataProvider.TotalCollectInProfitCurrency = shipmentReceivables.Sum(a => a.CollectChargeAmountInProfitCurrency);
                    myDataProvider.TotalCollectInLocalCurrency = shipmentReceivables.Sum(a => a.CollectChargeAmountInLocalCurrency);
                }

                //ShipmentReceivableRepository receivableRepository = new ShipmentReceivableRepository(tenant);
                //List<ShipmentReceivable> receivables = receivableRepository.GetShipmentReceivablesByShipmentId(shipment.Id, tenant);
                //receivables[0].

                #endregion

                #region ReleasingAgent
                string myReleasingAgentId = shipment.ReleasingAgentId;
                string myReleasingAgentAddressId = shipment.ReleasingAgentAddressId;
                if (!string.IsNullOrEmpty(myReleasingAgentId))
                {
                    Card myPartnerCard = CardRepository.GetSingleCard(myReleasingAgentId, tenant, true);

                    if (myPartnerCard != null)
                    {
                        myDataProvider.ReleasingAgentAddress = myPartnerCard.EnglishName != null ? myPartnerCard.EnglishName + Environment.NewLine : "";
                        myDataProvider.ReleasingAgentName = myPartnerCard.EnglishName;
                        if (!string.IsNullOrEmpty(myReleasingAgentAddressId))
                        {
                            Address myPartnerAddress = addressRepository.GetSingleAddress(myReleasingAgentAddressId, tenant);

                            if (myPartnerAddress != null)
                            {
                                if (myPartnerAddress.IsLocalLanguage && !string.IsNullOrEmpty(myPartnerCard.LocalName))
                                {
                                    myDataProvider.ReleasingAgentAddress = myPartnerCard.LocalName + Environment.NewLine;
                                }

                                myDataProvider.ReleasingAgentAddress = myDataProvider.ReleasingAgentAddress + DataProviders.General.GetAddress(myPartnerAddress);

                                if (myPartnerAddress.PhoneNumber != null || myPartnerAddress.FaxNumber != null)
                                {
                                    myDataProvider.ReleasingAgentAddress = myDataProvider.ReleasingAgentAddress + Environment.NewLine + (myPartnerAddress.PhoneNumber != null ? "Tel: " + myPartnerAddress.PhoneNumber + " " : "") + (myPartnerAddress.FaxNumber != null ? "Fax: " + myPartnerAddress.FaxNumber + " " : "");
                                }
                            }
                        }
                    }
                }
                #endregion

                #region pickups
                List<ShipmentPickUpPM> pickUps = shipmentPickUpQuery.GetShipmentPickUpPMsByTenantAndShipment(shipment.Id, tenant);

                if (pickUps.Count > 0)
                {
                    myDataProvider.PickUpsLines = new List<PickUpDeliveryLine>();

                    foreach (ShipmentPickUpPM pick in pickUps)
                    {
                        PickUpDeliveryLine newItem = new PickUpDeliveryLine();
                        newItem.ETD = pick.ETD;
                        newItem.ETA = pick.ETA;
                        newItem.ATD = pick.ATD;
                        newItem.ATA = pick.ATA;
                        newItem.Notes = pick.Notes;
                        newItem.TransportMode = pick.TransportModeName;
                        newItem.Weight = pick.ShipmentPickUpDeliveryPackages.Sum(s => s.Weight);
                        myServicHelper.GetPickUpAddresses(pick, newItem, addressRepository, tenant);

                        foreach (ShipmentPickUpDeliveryPackagePM package in pick.ShipmentPickUpDeliveryPackages)
                        {
                            PackageLine newPackage = new PackageLine();
                            newPackage.PackageDescriptionOfGoods = package.Description;
                            newPackage.ContainerNumber = package.ContainerNumber;
                            newPackage.SealNumber = package.ShipperSeal;
                            newPackage.PackageQuantity = package.Quantity.ToString();
                            newPackage.PackageTypeName = package.PackageTypeName;
                            newPackage.PackageVolume_Double = package.Volume;
                            newPackage.PackageGrossWeight = String.Format("{0:0,0.00}", package.Weight);

                            if (package.Length != null && package.Width != null && package.Height != null)
                            {
                                newPackage.Dimensions = package.Length + "x" + package.Width + "x" + package.Height;
                            }

                            #region Car Details
                            newPackage.Make = package.Make;
                            newPackage.Model = package.Model;
                            newPackage.Year = package.Year;
                            newPackage.Color = package.Color;
                            newPackage.ChassisNumber = package.ChassisNumber;
                            newPackage.RegistrationNumber = package.RegistrationNumber;

                            if (!string.IsNullOrEmpty(package.CountryId))
                            {
                                Country country = CountryRepository.GetSingleCountry(package.CountryId, tenant, true);
                                if (country != null)
                                {
                                    newPackage.CountryName = country.EnglishName;
                                }
                            }
                            #endregion

                            #region Harmonize
                            if (package.IsMultiHarmonize)
                            {
                                List<PickUpDeliveryPackageHarmonize> allHarmonizes = shipmentsContext.PickUpDeliveryPackageHarmonizes.Where(d => d.PackageId == package.Id && d.Tenant == package.Tenant).ToList();
                                foreach (PickUpDeliveryPackageHarmonize itemHarmonize in allHarmonizes)
                                {
                                    if (string.IsNullOrEmpty(newPackage.HSCode))
                                    {
                                        newPackage.HSCode = itemHarmonize.Harmonize;
                                    }
                                    else
                                    {
                                        newPackage.HSCode += "," + itemHarmonize.Harmonize;
                                    }
                                }
                            }
                            else
                            {
                                newPackage.HSCode = package.Harmonize;
                            }
                            #endregion

                            newItem.PickUpDeliveryPackages.Add(newPackage);
                        }

                        myDataProvider.PickUpsLines.Add(newItem);
                    }
                }
                #endregion

                #region deliveries
                List<ShipmentDeliveryPM> deliveries = shipmentDeliveryQuery.GetShipmentDeliveryPMsByTenantAndShipment(shipment.Id, tenant);

                if (deliveries.Count > 0)
                {
                    myDataProvider.DeliveriesLines = new List<PickUpDeliveryLine>();

                    foreach (ShipmentDeliveryPM deliv in deliveries)
                    {
                        PickUpDeliveryLine newItem = new PickUpDeliveryLine();
                        newItem.ETD = deliv.ETD;
                        newItem.ETA = deliv.ETA;
                        newItem.ATD = deliv.ATD;
                        newItem.ATA = deliv.ATA;
                        newItem.Notes = deliv.Notes;
                        newItem.TransportMode = deliv.TransportModeName;
                        newItem.Weight = deliv.ShipmentPickUpDeliveryPackages.Sum(s => s.Weight);
                        myServicHelper.GetDeliveryToAddress(deliv, newItem, addressRepository, tenant);

                        foreach (ShipmentPickUpDeliveryPackagePM package in deliv.ShipmentPickUpDeliveryPackages)
                        {
                            PackageLine newPackage = new PackageLine();
                            newPackage.PackageDescriptionOfGoods = package.Description;
                            newPackage.ContainerNumber = package.ContainerNumber;
                            newPackage.SealNumber = package.ShipperSeal;
                            newPackage.PackageQuantity = package.Quantity.ToString();
                            newPackage.PackageTypeName = package.PackageTypeName;
                            newPackage.PackageVolume_Double = package.Volume;
                            newPackage.PackageGrossWeight = String.Format("{0:0,0.00}", package.Weight);

                            if (package.Length != null && package.Width != null && package.Height != null)
                            {
                                newPackage.Dimensions = package.Length + "x" + package.Width + "x" + package.Height;
                            }

                            #region Car Details
                            newPackage.Make = package.Make;
                            newPackage.Model = package.Model;
                            newPackage.Year = package.Year;
                            newPackage.Color = package.Color;
                            newPackage.ChassisNumber = package.ChassisNumber;
                            newPackage.RegistrationNumber = package.RegistrationNumber;

                            if (!string.IsNullOrEmpty(package.CountryId))
                            {
                                Country country = CountryRepository.GetSingleCountry(package.CountryId, tenant, true);
                                if (country != null)
                                {
                                    newPackage.CountryName = country.EnglishName;
                                }
                            }
                            #endregion

                            #region Harmonize
                            if (package.IsMultiHarmonize)
                            {
                                List<PickUpDeliveryPackageHarmonize> allHarmonizes = shipmentsContext.PickUpDeliveryPackageHarmonizes.Where(d => d.PackageId == package.Id && d.Tenant == package.Tenant).ToList();
                                foreach (PickUpDeliveryPackageHarmonize itemHarmonize in allHarmonizes)
                                {
                                    if (string.IsNullOrEmpty(newPackage.HSCode))
                                    {
                                        newPackage.HSCode = itemHarmonize.Harmonize;
                                    }
                                    else
                                    {
                                        newPackage.HSCode += "," + itemHarmonize.Harmonize;
                                    }
                                }
                            }
                            else
                            {
                                newPackage.HSCode = package.Harmonize;
                            }
                            #endregion

                            newItem.PickUpDeliveryPackages.Add(newPackage);
                        }

                        myDataProvider.DeliveriesLines.Add(newItem);
                    }
                }
                #endregion

                #region Customer
                if (!string.IsNullOrEmpty(shipment.CustomerId))
                {
                    myDataProvider.CustomerLogo = DataProviders.General.GetCarrierLogo(shipment.CustomerId, tenant);

                    Card myPartnerCard = CardRepository.GetSingleCard(shipment.CustomerId, tenant, true);
                    if (myPartnerCard != null)
                    {
                        myDataProvider.CustomerVAT = myPartnerCard.VatNumber;
                    }

                    if (!string.IsNullOrEmpty(shipment.CustomerContactId))
                    {
                        Contact customerContact = contactRepository.GetSingleContact(shipment.CustomerContactId, tenant);
                        if (customerContact != null)
                        {
                            myDataProvider.ContactDetails = customerContact.EnglishName;

                            if (!string.IsNullOrEmpty(customerContact.Email))
                            {
                                myDataProvider.ContactDetails = myDataProvider.ContactDetails + ", " + customerContact.Email;
                            }

                            if (!string.IsNullOrEmpty(customerContact.Mobile))
                            {
                                myDataProvider.ContactDetails = myDataProvider.ContactDetails + ", " + customerContact.Mobile;
                            }

                            if (!string.IsNullOrEmpty(customerContact.Fax))
                            {
                                myDataProvider.ContactDetails = myDataProvider.ContactDetails + ", " + customerContact.Fax;
                            }
                        }
                    }
                }
                #endregion

                #region Last Delivery

                ShipmentPickUpDelivery myLastDelivery = (from d in shipmentsContext.ShipmentPickUpDeliveries
                                                         where d.ShipmentId == shipment.Id && d.PickUpDeliveryTypeCode == "DELV"
                                                         select d).OrderByDescending(s => s.PickUpDeliveryNumber).FirstOrDefault();

                if (myLastDelivery != null)
                {
                    switch (myLastDelivery.PickUpDeliveryToTypeCode)
                    {
                        case "PART":
                            {
                                if (!string.IsNullOrEmpty(myLastDelivery.ToPartnerCardId))
                                {
                                    Card toPartner = CardRepository.GetSingleCard(myLastDelivery.ToPartnerCardId, tenant, false);

                                    if (toPartner != null)
                                    {
                                        if (!string.IsNullOrEmpty(toPartner.PrimaryContactId))
                                        {
                                            Contact toPartnerContact = contactRepository.GetSingleContact(toPartner.PrimaryContactId, tenant);
                                            if (toPartnerContact != null)
                                            {
                                                myDataProvider.DeliveryToContactName = toPartnerContact.EnglishName;
                                                myDataProvider.DeliveryToContactEmail = toPartnerContact.Email;
                                                myDataProvider.DeliveryToContactPhone = toPartnerContact.BusinessPhone;
                                            }
                                        }
                                    }
                                }

                                break;
                            }

                        case "CASL":
                            {
                                break;
                            }

                        case "PORT":
                            {
                                break;
                            }
                    }
                    myDataProvider.DeliveryInstructions = myLastDelivery.Notes != null ? myLastDelivery.Notes : "";
                }
                #endregion

                myDataProvider.FinalDestination = myServicHelper.GetFinalDestination(myFirstDelivery, shipment);
                myDataProvider.CustomsDeclarationNumber = shipment.DeclarationNumber;
                myDataProvider.CustomsDeclarationDate = shipment.DeclarationDate;
                myDataProvider.CustomsClearanceDate = shipment.CustomsClearanceDate;
                //HBL
                myDataProvider.House = shipment.House != null ? shipment.House : "";
                myDataProvider.Master = shipment.Master != null ? shipment.Master : "";
                myDataProvider.LongMaster = shipment.LongMaster != null ? shipment.LongMaster : "";
                myDataProvider.GeneralDescriptionOfGoods = shipment.DescriptionOfGoods != null ? shipment.DescriptionOfGoods : "";

                //Ports
                if (mainCarriageFromPort != null)
                {
                    myDataProvider.LoadingPortName = mainCarriageFromPort.EnglishName;

                    if (mainCarriageFromPort.State != null)
                    {
                        myDataProvider.LoadingPortName = myDataProvider.LoadingPortName + ", " + mainCarriageFromPort.State.Code;
                    }

                    if (!string.IsNullOrEmpty(mainCarriageFromPort.CountryId))
                    {
                        Country myCountry = CountryRepository.GetSingleCountry(mainCarriageFromPort.CountryId, tenant, true);
                        if (myCountry != null)
                        {
                            myDataProvider.LoadingPortCountryName = myCountry.EnglishName;
                        }
                    }
                }

                if (shipment.BranchId != null)
                {
                    Branch currentBranch = (from br in commonContext.Branches
                                            where br.Id == shipment.BranchId
                                            select br).FirstOrDefault();

                    if (currentBranch != null)
                    {
                        myDataProvider.PlaceAndDateOfIssue = currentBranch.EnglishName;
                        myDataProvider.PlaceAndDateOfIssue_Local = currentBranch.LocalName;
                    }
                }

                if (shipment.Transshipment3ToPortId != null)
                {
                    Port myPort = portRepository.GetSinglePort(tenant, shipment.Transshipment3ToPortId);
                    if (myPort != null)
                    {
                        myDataProvider.DischargePortName = myPort.EnglishName;

                        if (myPort.Country != null)
                        {
                            myDataProvider.DischargePortCountryName = myPort.Country.EnglishName;
                        }

                        if (myPort.State != null)
                        {
                            myDataProvider.DischargePortName = myDataProvider.DischargePortName + ", " + myPort.State.Code;
                        }
                    }
                }

                else if (shipment.Transshipment2ToPortId != null)
                {
                    Port myPort = portRepository.GetSinglePort(tenant, shipment.Transshipment2ToPortId);
                    if (myPort != null)
                    {
                        myDataProvider.DischargePortName = myPort.EnglishName;

                        if (myPort.State != null)
                        {
                            myDataProvider.DischargePortName = myDataProvider.DischargePortName + ", " + myPort.State.Code;
                        }

                        if (myPort.Country != null)
                        {
                            myDataProvider.DischargePortCountryName = myPort.Country.EnglishName;
                        }
                    }
                }

                else if (shipment.Transshipment1ToPortId != null)
                {
                    Port myPort = portRepository.GetSinglePort(tenant, shipment.Transshipment1ToPortId);
                    if (myPort != null)
                    {
                        myDataProvider.DischargePortName = myPort.EnglishName;

                        if (myPort.State != null)
                        {
                            myDataProvider.DischargePortName = myDataProvider.DischargePortName + ", " + myPort.State.Code;
                        }

                        if (myPort.Country != null)
                        {
                            myDataProvider.DischargePortCountryName = myPort.Country.EnglishName;
                        }
                    }
                }

                else if (shipment.MainCarriageToPortId != null)
                {
                    Port myPort = portRepository.GetSinglePort(tenant, shipment.MainCarriageToPortId);
                    if (myPort != null)
                    {
                        myDataProvider.DischargePortName = myPort.EnglishName;

                        if (myPort.State != null)
                        {
                            myDataProvider.DischargePortName = myDataProvider.DischargePortName + ", " + myPort.State.Code;
                        }

                        if (myPort.Country != null)
                        {
                            myDataProvider.DischargePortCountryName = myPort.Country.EnglishName;
                        }
                    }
                }

                if (onCarriageToPort != null)
                {
                    myDataProvider.OnCarriageToPort = onCarriageToPort.EnglishName;
                }

                if (shipment.PreCarriageVesselId != null)
                {
                    Vessel precarriagevessel = (from a in commonContext.Vessels
                                                 where a.Id == shipment.PreCarriageVesselId
                                                 select a).FirstOrDefault();
                    if (precarriagevessel != null)
                    {
                        myDataProvider.PreCarriageVesselName = precarriagevessel.EnglishName;
                    }
                }

                if (shipment.MainCarriageVesselId != null)
                {
                    Vessel maincarriagevessel = (from a in commonContext.Vessels
                                                 where a.Id == shipment.MainCarriageVesselId
                                                 select a).FirstOrDefault();
                    if (maincarriagevessel != null)
                    {
                        myDataProvider.MainCarriageVesselNameAndNumber = maincarriagevessel.EnglishName + " \\ " + shipment.MainCarriageCarrierNumber;
                        myDataProvider.MainCarriageVesselName = maincarriagevessel.EnglishName;
                        myDataProvider.VesselName = maincarriagevessel.EnglishName;
                        myDataProvider.VoyageNumber = shipment.MainCarriageCarrierNumber;
                    }
                }

                if (!string.IsNullOrEmpty(shipment.Transshipment1VesselId))
                {
                    myDataProvider.TransshipmentsVesselNameAndNumber = shipment.Transshipment1VesselName + " \\ " + shipment.Transshipment1CarrierNumber;
                    myDataProvider.VesselName = shipment.Transshipment1VesselName;
                    myDataProvider.VoyageNumber = shipment.Transshipment1CarrierNumber;
                }

                if (!string.IsNullOrEmpty(shipment.Transshipment2VesselId))
                {
                    if (string.IsNullOrEmpty(myDataProvider.TransshipmentsVesselNameAndNumber))
                    {
                        myDataProvider.TransshipmentsVesselNameAndNumber = shipment.Transshipment2VesselName + " \\ " + shipment.Transshipment2CarrierNumber;
                        myDataProvider.VesselName = shipment.Transshipment2VesselName;
                        myDataProvider.VoyageNumber = shipment.Transshipment2CarrierNumber;
                    }
                    else
                    {
                        myDataProvider.TransshipmentsVesselNameAndNumber = myDataProvider.TransshipmentsVesselNameAndNumber + Environment.NewLine + shipment.Transshipment2VesselName + " \\ " + shipment.Transshipment2CarrierNumber;
                        myDataProvider.VesselName = myDataProvider.VesselName + Environment.NewLine + shipment.Transshipment2VesselName;
                        myDataProvider.VoyageNumber = myDataProvider.VoyageNumber + Environment.NewLine + shipment.Transshipment2CarrierNumber;

                    }
                }

                if (!string.IsNullOrEmpty(shipment.Transshipment3VesselId))
                {
                    if (string.IsNullOrEmpty(myDataProvider.TransshipmentsVesselNameAndNumber))
                    {
                        myDataProvider.TransshipmentsVesselNameAndNumber = shipment.Transshipment3VesselName + " \\ " + shipment.Transshipment3CarrierNumber;
                        myDataProvider.VesselName = shipment.Transshipment3VesselName;
                        myDataProvider.VoyageNumber = shipment.Transshipment3CarrierNumber;
                    }
                    else
                    {
                        myDataProvider.TransshipmentsVesselNameAndNumber = myDataProvider.TransshipmentsVesselNameAndNumber + Environment.NewLine + shipment.Transshipment3VesselName + " \\ " + shipment.Transshipment3CarrierNumber;
                        myDataProvider.VesselName = myDataProvider.VesselName + Environment.NewLine + shipment.Transshipment3VesselName;
                        myDataProvider.VoyageNumber = myDataProvider.VoyageNumber + Environment.NewLine + shipment.Transshipment3CarrierNumber;
                    }
                }

                myDataProvider.BookingNumber = shipment.BookingConfirmationNumber != null ? shipment.BookingConfirmationNumber : "";

                //Custom fields
                string haSattachment = "";
                DocumentType currentdocumentType = commonContext.DocumentTypes.Where(doc => doc.Code == "716" && doc.Tenant == tenant).FirstOrDefault();
                if (currentdocumentType != null)
                {
                    List<FormCustomField> customfieldsList = commonContext.FormCustomFields.Where(fc => fc.DocumentTypeId == currentdocumentType.Id).ToList();

                    List<DocumentTypeCustomField> documentCustomfieldsList = commonContext.DocumentTypeCustomFields.Where(fc => fc.DocumentTypeId == currentdocumentType.Id).ToList();

                    FormCustomField numberOfOriginalsCustomField = (from a in customfieldsList
                                                                    where a.FieldCode == "NumberOfOriginals" && a.EntityId == shipment.Id
                                                                    select a).FirstOrDefault();

                    DocumentTypeCustomField numberOfOriginalsDocumentCustom = (from a in documentCustomfieldsList
                                                                               where a.FieldCode == "NumberOfOriginals"
                                                                               select a).FirstOrDefault();

                    myDataProvider.NumberOfOriginals = numberOfOriginalsCustomField != null ? numberOfOriginalsCustomField.Value : numberOfOriginalsDocumentCustom != null ? numberOfOriginalsDocumentCustom.DefaultValue : null;

                    FormCustomField copyOrOriginalCustomField = (from a in customfieldsList
                                                                 where a.FieldCode == "CopyOrOriginal" && a.EntityId == shipment.Id
                                                                 select a).FirstOrDefault();

                    DocumentTypeCustomField copyOrOriginalDocumentCustom = (from a in documentCustomfieldsList
                                                                            where a.FieldCode == "CopyOrOriginal"
                                                                            select a).FirstOrDefault();

                    FormCustomField hasAttachmentListCustomField = (from a in customfieldsList
                                                                    where a.FieldCode == "HasAttachmentList" && a.EntityId == shipment.Id
                                                                    select a).FirstOrDefault();

                    DocumentTypeCustomField hasAttachmentListDocumentCustom = (from a in documentCustomfieldsList
                                                                               where a.FieldCode == "HasAttachmentList"
                                                                               select a).FirstOrDefault();

                    haSattachment = hasAttachmentListCustomField != null ? hasAttachmentListCustomField.Value : hasAttachmentListDocumentCustom.DefaultValue;

                    FormCustomField fblNotesCustomField = (from a in customfieldsList
                                                           where a.FieldCode == "FBLNotes" && a.EntityId == shipment.Id
                                                           select a).FirstOrDefault();

                    DocumentTypeCustomField fblNotesDocumentCustom = (from a in documentCustomfieldsList
                                                                      where a.FieldCode == "FBLNotes"
                                                                      select a).FirstOrDefault();

                    FormCustomField fblNotesCustomField2 = (from a in customfieldsList
                                                            where a.FieldCode == "FBLNotes2" && a.EntityId == shipment.Id
                                                            select a).FirstOrDefault();

                    DocumentTypeCustomField fblNotesDocumentCustom2 = (from a in documentCustomfieldsList
                                                                       where a.FieldCode == "FBLNotes2"
                                                                       select a).FirstOrDefault();

                    myDataProvider.FBLNotes = fblNotesCustomField != null ? fblNotesCustomField.Value : (fblNotesDocumentCustom != null ? fblNotesDocumentCustom.DefaultValue : "");
                    myDataProvider.FBLNotes2 = fblNotesCustomField2 != null ? fblNotesCustomField2.Value : (fblNotesDocumentCustom2 != null ? fblNotesDocumentCustom2.DefaultValue : "");


                    if (haSattachment == "True")
                    {
                        myDataProvider.HasAttachmentList = true;
                    }
                    else if (haSattachment == "False")
                    {
                        myDataProvider.HasAttachmentList = false;
                    }
                    myDataProvider.CopyOrOriginal = copyOrOriginalCustomField != null ? copyOrOriginalCustomField.Value : copyOrOriginalDocumentCustom.DefaultValue;
                }

                //Packages
                List<ShipmentPackage> packages = shipmentsContext.ShipmentPackages.Include("PackageType").Where(d => d.ShipmentId == shipment.Id && d.Tenant == tenant).ToList();
                myDataProvider.PackagesLines = new List<PackageLine>();
                myDataProvider.AttachmentList = new List<PackageLine>();

                string volumeUnitCode = shipment.VolumeUnitCode != null ? shipment.VolumeUnitCode : "";

                if (packages.Count > 0)
                {
                    ShipmentPackage firstContainer = packages.FirstOrDefault();
                    if (firstContainer != null)
                    {
                        myDataProvider.Temperature = firstContainer.Temperature;
                    }

                    int? totalInsidePackagesCount = 0;

                    foreach (ShipmentPackage package in packages)
                    {
                        List<InsideShipmentPackage> insidePackages = shipmentsContext.InsideShipmentPackages.Where(d => d.ShipmentPackageId == package.Id && d.Tenant == package.Tenant).ToList();

                        totalInsidePackagesCount += insidePackages.Sum(s => s.Quantity);

                        PackageLine packageline = new PackageLine();
                        packageline.InsidePackagesLines = new List<InsidePackageLine>();

                        packageline.InsidePackagesDetails = this.ComputeInsidePackagesDetailsPerPackage(package);
                        packageline.PackageDescriptionOfGoods = package.Description != null ? (package.Description + Environment.NewLine) : "";
                        packageline.ContainerNumber = package.ContainerNumber;
                        packageline.Seal1 = package.ShipperSeal;
                        packageline.Seal2 = package.CarrierSeal;
                        packageline.Length = package.Length.ToString();
                        packageline.Width = package.Width.ToString();
                        packageline.Height = package.Height.ToString();

                        #region Car Details
                        packageline.Make = package.Make;
                        packageline.Model = package.Model;
                        packageline.Year = package.Year;
                        packageline.Color = package.Color;
                        packageline.ChassisNumber = package.ChassisNumber;
                        packageline.RegistrationNumber = package.RegistrationNumber;

                        if (!string.IsNullOrEmpty(package.CountryId))
                        {
                            Country country = CountryRepository.GetSingleCountry(package.CountryId, tenant, true);
                            if (country != null)
                            {
                                packageline.CountryName = country.EnglishName;
                            }
                        }
                        #endregion

                        packageline.Reference1 = package.Reference1;
                        packageline.Reference2 = package.Reference2;
                        packageline.Reference3 = package.Reference3;
                        packageline.Reference4 = package.Reference4;

                        #region Harmonize
                        if (package.IsMultiHarmonize)
                        {
                            List<ShipmentPackageHarmonize> allHarmonizes = shipmentsContext.ShipmentPackageHarmonizes.Where(d => d.PackageId == package.Id && d.Tenant == package.Tenant).ToList();
                            foreach (ShipmentPackageHarmonize itemHarmonize in allHarmonizes)
                            {
                                if (string.IsNullOrEmpty(packageline.HSCode))
                                {
                                    packageline.HSCode = itemHarmonize.Harmonize;
                                }

                                else
                                {
                                    packageline.HSCode += "," + itemHarmonize.Harmonize;
                                }
                            }
                        }

                        else
                        {
                            packageline.HSCode = package.Harmonize;
                        }
                        #endregion

                        if (package.Length != null && package.Width != null && package.Height != null)
                        {
                            packageline.Dimensions = package.Length + "x" + package.Width + "x" + package.Height;
                        }

                        if (package.IsDangerous)
                        {
                            packageline.PackageDescriptionOfGoods +=
                                "CONTAINS DANGEROUS GOODS: " + Environment.NewLine +
                                (package.MaterialDescription != null ? package.MaterialDescription : "") +
                                " - Class: " + (package.ClassNumber != null ? package.ClassNumber : "") +
                                ", UN-N: " + (package.UnNumber != null ? package.UnNumber : "") +
                                ", PACKING GROUP " + (package.PackagingGroup != null ? package.PackagingGroup : "");
                        }

                        if (!string.IsNullOrEmpty(package.Harmonize))
                        {
                            if (!string.IsNullOrEmpty(packageline.PackageDescriptionOfGoods))
                            {
                                packageline.PackageDescriptionOfGoods += Environment.NewLine;
                            }

                            if (package.IsMultiHarmonize)
                            {
                                packageline.PackageDescriptionOfGoods += this.GetMultiHarmonizeHSCode(packageline.PackageDescriptionOfGoods, package);
                            }
                            else
                            {
                                packageline.PackageDescriptionOfGoods += "HS Code: " + package.Harmonize;
                            }
                        }

                        packageline.PackageGrossWeight = package.Weight != null ? String.Format("{0:N2}", package.Weight) + (shipment.GrossWeightUnitCode != null ? shipment.GrossWeightUnitCode : "") : "";
                        packageline.PackageQuantity = package.Quantity != null ? package.Quantity.Value.ToString() : "";
                        packageline.PackageVolume = package.Volume != null ? (package.Volume + " " + volumeUnitCode) : "";
                        packageline.PackageTare = package.Tare != null ? String.Format("{0:N2}", package.Tare) + (shipment.GrossWeightUnitCode != null ? shipment.GrossWeightUnitCode : "") : "";

                        PackageType packagetype = (from pa in commonContext.PackageTypes
                                                   where pa.Id == package.PackageTypeId
                                                   select pa).FirstOrDefault();
                        if (packagetype != null)
                        {
                            packageline.PackageQuantityAndType = packageline.PackageQuantity + "x" + packagetype.PrintAs;
                            packageline.PackageTypeName = packagetype.EnglishName;

                            if (packagetype.IsContainer)
                            {
                                packageline.PackageType = packagetype.PrintAs;
                            }

                            else
                            {
                                packageline.PackageType = packagetype.EnglishName;
                            }

                            if (package.MarksAndNumbers == null)
                            {
                                if (packagetype.IsContainer)
                                {
                                    if (package.ContainerNumber != null)
                                    {
                                        if (package.ContainerNumber.Length > 10)
                                        {
                                            packageline.PackageMarksAndNumbers = !string.IsNullOrEmpty(package.ContainerNumber) ? package.ContainerNumber.Substring(0, 4) + " " + package.ContainerNumber.Substring(4, 6) + "/" + package.ContainerNumber.Substring(10, 1) + Environment.NewLine : "";
                                        }
                                        else
                                        {
                                            packageline.PackageMarksAndNumbers = package.ContainerNumber;
                                        }

                                        packageline.PackageMarksAndNumbersNew = package.ContainerNumber;
                                        packageline.PackageMarksAndNumbers_OneLine = package.ContainerNumber;
                                    }

                                    if (!string.IsNullOrEmpty(package.ShipperSeal))
                                    {
                                        packageline.PackageMarksAndNumbers = packageline.PackageMarksAndNumbers + "SEAL:" + package.ShipperSeal + Environment.NewLine;
                                        packageline.PackageMarksAndNumbersNew = packageline.PackageMarksAndNumbersNew + "SEAL:" + package.ShipperSeal + Environment.NewLine;
                                        packageline.PackageMarksAndNumbers_OneLine = packageline.PackageMarksAndNumbers_OneLine + " / SEAL:" + package.ShipperSeal + Environment.NewLine;
                                    }

                                    if (package.Tare != null)
                                    {
                                        packageline.PackageGrossWeight += Environment.NewLine + "TARE:" + package.Tare + (shipment.GrossWeightUnitCode != null ? shipment.GrossWeightUnitCode : "");
                                    }
                                }
                            }
                            else
                            {
                                packageline.PackageMarksAndNumbers = package.MarksAndNumbers != null ? package.MarksAndNumbers : "";
                                packageline.PackageMarksAndNumbersNew = package.MarksAndNumbers != null ? package.MarksAndNumbers : "";
                                packageline.PackageMarksAndNumbers_OneLine = package.MarksAndNumbers;
                            }
                        }

                        packageline.IsDangerous = package.IsDangerous ? "Yes" : "No";

                        //InsidePackages
                        foreach (InsideShipmentPackage insideItem in insidePackages)
                        {
                            PackageType insidePackageType = (from pa in commonContext.PackageTypes
                                                             where pa.Id == insideItem.PackageTypeId
                                                             select pa).FirstOrDefault();

                            InsidePackageLine insidePackage = new InsidePackageLine();

                            insidePackage.PackageType = insidePackageType == null ? "" : insidePackageType.EnglishName;
                            insidePackage.Quantity = insideItem.Quantity;

                            if (insideItem.Length != null && insideItem.Width != null && insideItem.Height != null)
                            {
                                insidePackage.Dimensions = insideItem.Length + "x" + insideItem.Width + "x" + insideItem.Height;
                            }

                            insidePackage.Volume = insideItem.Volume;
                            insidePackage.VolumetricWeight = insideItem.VolumetricWeight;
                            insidePackage.Weight = insideItem.Weight;
                            insidePackage.Description = insideItem.Description;

                            if (insideItem.IsMultiHarmonize)
                            {
                                List<ShipmentPackageHarmonize> allHarmonizes = shipmentsContext.ShipmentPackageHarmonizes.Where(d => d.InsidePackageId == insideItem.Id && d.Tenant == tenant).ToList();
                                foreach (ShipmentPackageHarmonize itemHarmonize in allHarmonizes)
                                {
                                    if (string.IsNullOrEmpty(insidePackage.HSCode))
                                    {
                                        insidePackage.HSCode = itemHarmonize.Harmonize;
                                    }

                                    else
                                    {
                                        insidePackage.HSCode += "," + itemHarmonize.Harmonize;
                                    }
                                }
                            }

                            else
                            {
                                insidePackage.HSCode = insideItem.Harmonize;
                            }

                            #region Car Details
                            insidePackage.Make = insideItem.Make;
                            insidePackage.Model = insideItem.Model;
                            insidePackage.Year = insideItem.Year;
                            insidePackage.Color = insideItem.Color;
                            insidePackage.ChassisNumber = insideItem.ChassisNumber;
                            insidePackage.RegistrationNumber = insideItem.RegistrationNumber;

                            if (!string.IsNullOrEmpty(insideItem.CountryId))
                            {
                                Country country = CountryRepository.GetSingleCountry(insideItem.CountryId, tenant, true);
                                if (country != null)
                                {
                                    insidePackage.CountryName = country.EnglishName;
                                }
                            }
                            #endregion

                            packageline.InsidePackagesLines.Add(insidePackage);
                        }

                        int insidePackagesCount = (from d in shipmentsContext.InsideShipmentPackages
                                                   where d.ShipmentPackageId == package.Id
                                                   && d.Tenant == package.Tenant
                                                   select d).Count();

                        string insidePackagesCountString = insidePackagesCount > 0 ? insidePackagesCount.ToString() : "";
                        packageline.InsidePackagesCount = package.NumberOfInsidePackages.ToString();

                        if (!myDataProvider.HasAttachmentList)
                        {
                            myDataProvider.PackagesLines.Add(packageline);
                        }

                        else // add to attachment list
                        {
                            if (shipment.ShipmentTypeName == "My Groupage")
                            {
                                PackageLine newLine = new PackageLine();

                                newLine.InsidePackagesCount = insidePackagesCountString;
                                this.GetInsidePackagesData(shipmentsContext, package, shipment, newLine);
                                myDataProvider.AttachmentList.Add(newLine);
                            }

                            else
                            {
                                myDataProvider.AttachmentList.Add(packageline);
                            }
                        }
                    }

                    myDataProvider.NumberOfInsidePackages = totalInsidePackagesCount;
                }

                if (myDataProvider.HasAttachmentList == true && packages.Count > 0)
                {
                    var resultquery = from att in packages
                                      group new { att.Quantity, att.Weight, att.Volume } by att.PackageType != null ? att.PackageType.EnglishName : "" into newGroup
                                      orderby newGroup.Sum(s => Convert.ToInt32(s.Quantity)) descending
                                      select new { Type = newGroup.Key, Count = newGroup.Sum(s => Convert.ToInt32(s.Quantity != null ? s.Quantity.Value : 0)), Weight = newGroup.Sum(s => s.Weight != null ? s.Weight.Value : 0), Volume = newGroup.Sum(s => s.Volume != null ? s.Volume.Value : 0) };

                    StringBuilder packageNumberstrbuilder = new StringBuilder();
                    StringBuilder packageTypestrbuilder = new StringBuilder();
                    StringBuilder packageGrossweighttrbuilder = new StringBuilder();
                    StringBuilder packageVolumeBuilder = new StringBuilder();

                    foreach (var res in resultquery)
                    {
                        packageNumberstrbuilder.AppendLine(res.Count.ToString());
                        packageTypestrbuilder.AppendLine(res.Type.ToString());
                        packageGrossweighttrbuilder.AppendLine((String.Format("{0:#,0.00}", res.Weight)) + (shipment.GrossWeightUnitCode != null ? shipment.GrossWeightUnitCode : ""));
                        packageVolumeBuilder.AppendLine((String.Format("{0:#,0.000}", res.Volume)) + (shipment.VolumeUnitCode != null ? shipment.VolumeUnitCode : ""));
                    }

                    myDataProvider.CustomPackagesNumber = packageNumberstrbuilder.ToString();
                    myDataProvider.CustomPackageType = packageTypestrbuilder.ToString();
                    myDataProvider.CustomWeight = packageGrossweighttrbuilder.ToString();
                    myDataProvider.CustomVolume = packageVolumeBuilder.ToString().StartsWith("00.00") == false ? packageVolumeBuilder.ToString() : "";
                }

                //int numberofpackages = shipment.NumberOfPackages != null ? shipment.NumberOfPackages.Value : 0;
                //int numberofcontainers = shipment.NumberOfContainers != null ? shipment.NumberOfContainers.Value : 0;
                //myDataProvider.TotalQuantity = (numberofpackages + numberofcontainers).ToString();
                //myDataProvider.TotalQuantity_Double = numberofpackages + numberofcontainers;

                if (shipment.PackagesQuantity != null)
                {
                    myDataProvider.TotalQuantity = shipment.PackagesQuantity.ToString();
                    myDataProvider.TotalQuantity_Double = shipment.PackagesQuantity;
                }

                myDataProvider.TotalVolume = shipment.Volume != null && shipment.Volume != 0 ? (shipment.Volume + " " + volumeUnitCode) : "";
                myDataProvider.TotalWeight = shipment.GrossWeight != null && shipment.GrossWeight != 0 ? String.Format("{0:#,0.00}", shipment.GrossWeight.Value) + " " + (shipment.GrossWeightUnitCode != null ? shipment.GrossWeightUnitCode : "") : "";

                //Confimation Note
                myDataProvider.ConfirmationNotes = shipment.BookingConfirmationNotes != null ? shipment.BookingConfirmationNotes : "";
                //PrepaidCollect
                myDataProvider.PrepaidCollect = shipmentprepaidcollect != null ? shipmentprepaidcollect.Name : "";

                if (shipment.HAWBDate != null)
                {
                    myDataProvider.PlaceAndDateOfIssue = myDataProvider.PlaceAndDateOfIssue + " " + String.Format("{0:dd MMM yyyy}", shipment.HAWBDate.Value);
                    myDataProvider.PlaceAndDateOfIssue_Local = myDataProvider.PlaceAndDateOfIssue_Local + " " + String.Format("{0:dd MMM yyyy}", shipment.HAWBDate.Value);
                }
                else
                {
                    myDataProvider.PlaceAndDateOfIssue = myDataProvider.PlaceAndDateOfIssue + " " + String.Format("{0:dd MMM yyyy}", TenantServerConfigration.GetCurrentDateTime(tenant));
                    myDataProvider.PlaceAndDateOfIssue_Local = myDataProvider.PlaceAndDateOfIssue_Local + " " + String.Format("{0:dd MMM yyyy}", TenantServerConfigration.GetCurrentDateTime(tenant));
                }                

                myDataProvider.TenantLogo = DataProviders.General.GetLogo(tenantSettings.Id);

                CustomFieldResolver customFieldResolver = new CustomFieldResolver();
                customFieldResolver.SetDataProviderCustomFieldsValues("Shipment", tenant, shipment, myDataProvider);
                ShipmentPickUpDelivery lastPickUp = GetLastPickUp(shipment.Id);
                if (lastPickUp != null)
                {
                    myDataProvider.PickUpInstructions = lastPickUp.Notes != null ? lastPickUp.Notes : "";
                }
            }

            #region Custom Agent Import|Custom Agent Export
            this.GetAllBrokerVariables(shipment.CustomAgentImportId, shipment.CustomAgentImportAddressId);
            this.GetCustomAgentVariable(shipment.CustomAgentImportId, shipment.CustomAgentExportId);
            #endregion

            //------------------------------------------------
            try
            {
                Type fbldpType = myDataProvider.GetType();
                PropertyInfo[] properties = fbldpType.GetProperties();
                foreach (PropertyInfo pi in properties)
                {

                    if (pi.GetValue(myDataProvider, null) == null || pi.GetValue(myDataProvider, null).ToString() == "0" || pi.GetValue(myDataProvider, null).ToString() == "00.00")
                    {
                        pi.SetValue(myDataProvider, "", null);
                    }
                }
            }
            catch
            { }

            #region From/To CountryCode

            CountryRepository countryRepository = new CountryRepository(tenant);

            if (shipment.DirectionId == "I" && shipment.TransportModeId == "D")
            {
                Address fromAddress = addressRepository.GetSingleAddress(shipment.MainCarriageFromAddressId, tenant);
                Address toAddress = addressRepository.GetSingleAddress(shipment.MainCarriageToAddressId, tenant);

                if (fromAddress != null)
                {
                    Country fromCountry = countryRepository.GetSingleCountry(fromAddress.CountryId, tenant);
                    if (fromCountry != null)
                    {
                        myDataProvider.FromLocationCountryCode = fromCountry.Code;
                    }
                }

                if (toAddress != null)
                {
                    Country toCountry = countryRepository.GetSingleCountry(toAddress.CountryId, tenant);
                    if (toCountry != null)
                    {
                        myDataProvider.ToLocationCountryCode = toCountry.Code;
                    }
                }
            }

            else
            {
                Port fromPort = portRepository.GetSinglePort(tenant, shipment.MainCarriageFromPortId);
                Port toPort = portRepository.GetSinglePort(tenant, shipment.MainCarriageToPortId);

                if (fromPort != null)
                {
                    Country fromCountry = countryRepository.GetSingleCountry(fromPort.CountryId, tenant);
                    if (fromCountry != null)
                    {
                        myDataProvider.FromLocationCountryCode = fromCountry.Code;
                    }
                }

                if (toPort != null)
                {
                    Country toCountry = countryRepository.GetSingleCountry(toPort.CountryId, tenant);
                    if (toCountry != null)
                    {
                        myDataProvider.ToLocationCountryCode = toCountry.Code;
                    }
                }
            }
            #endregion

            myDataProvider = this.MapTransshipmentOneFields(myDataProvider,shipment);
            myDataProvider = this.MapTransshipmentTwoFields(myDataProvider, shipment);
            myDataProvider.UserSignature = this.GetUserSignatureImage(tenant);

            return myDataProvider;
        }

        private void SetAgentContact(FBLDataProvider myDataProvider, string agentContactId)
        {
            Contact agentContact = contactRepository.GetSingleContact(agentContactId, tenant);
            if (agentContact != null)
            {
                myDataProvider.AgentContact = agentContact.BusinessPhone + ", " + agentContact.Email;
            }
        }

        private string GetMultiHarmonizeHSCode(string packageDescriptionOfGoods, ShipmentPackage package)
        {
            var hsCodeString = "";
            List<ShipmentPackageHarmonize> allHarmonizes = shipmentsContext.ShipmentPackageHarmonizes.Where(d => d.PackageId == package.Id && d.Tenant == package.Tenant).ToList();
            foreach (ShipmentPackageHarmonize itemHarmonize in allHarmonizes)
            {
                if (string.IsNullOrEmpty(packageDescriptionOfGoods))
                {
                    hsCodeString += "HS Code: " + itemHarmonize.Harmonize;
                }
                else
                {
                    hsCodeString += "," + itemHarmonize.Harmonize;
                }
            }
            return hsCodeString;
        }

        private void GetCustomAgentVariable(string customAgentImportId, string customAgentExportId)
        {
            Card customAgentImporter = GetCustomAgent(customAgentImportId);
            Card customAgentExporter = GetCustomAgent(customAgentExportId);
            if (customAgentImporter != null)
            {
                myDataProvider.CustomsAgent = customAgentImporter.EnglishName != null ? customAgentImporter.EnglishName : "";
            }
            if (customAgentExporter != null)
            {
                myDataProvider.CustomsAgent = customAgentExporter.EnglishName != null ? customAgentExporter.EnglishName : "";
            }
        }

        private void GetAllBrokerVariables(string customAgentImportId, string customAgentImportAddressId)
        {
            Card customAgentImporter = GetCustomAgent(customAgentImportId);
            Address customAgentImportAddress;
            if (customAgentImporter != null)
            {
                customAgentImportAddress = GetCustomAgentImporterAddress(customAgentImportAddressId);
                myDataProvider.BrokerName = customAgentImporter.EnglishName != null ? customAgentImporter.EnglishName : "";
                myDataProvider.Broker = BuildBrokerValue(customAgentImporter, customAgentImportAddress);
            }
        }

        private string BuildBrokerValue(Card customAgentImporter, Address customAgentImportAddress)
        {
            string broker = "";
            if (customAgentImporter != null)
            {
                broker = customAgentImporter.EnglishName != null ? customAgentImporter.EnglishName : "";
            }
            if (customAgentImportAddress != null)
            {
                if (customAgentImportAddress.IsLocalLanguage)
                {
                    if (customAgentImporter != null && !string.IsNullOrEmpty(customAgentImporter.LocalName))
                    {
                        broker = customAgentImporter.LocalName;
                    }
                }
                broker = broker + Environment.NewLine + DataProviders.General.GetAddress(customAgentImportAddress);
                if (customAgentImportAddress.PhoneNumber != null || customAgentImportAddress.FaxNumber != null)
                {
                    broker = broker + Environment.NewLine
                        + (customAgentImportAddress.PhoneNumber != null ? "Tel: " + customAgentImportAddress.PhoneNumber + " " : "")
                        + (customAgentImportAddress.FaxNumber != null ? "Fax: " + customAgentImportAddress.FaxNumber + " " : "");
                }
            }
            return broker;
        }

        private Address GetCustomAgentImporterAddress(string customAgentImportAddressId)
        {
            if (!string.IsNullOrEmpty(customAgentImportAddressId))
            {
                return addressRepository.GetSingleAddress(customAgentImportAddressId, tenant);
            }
            return null;
        }

        private Card GetCustomAgent(string customAgentId)
        {
            if (!string.IsNullOrEmpty(customAgentId))
            {
                return (from a in commonContext.Cards
                        where a.Id == customAgentId
                        select a).FirstOrDefault();
            }
            return null;
        }

        private ShipmentPickUpDelivery GetLastPickUp(string shipmentId)
        {
            return (from pickUp in shipmentsContext.ShipmentPickUpDeliveries
                    where pickUp.ShipmentId == shipmentId && pickUp.PickUpDeliveryTypeCode == "PICK"
                    select pickUp).OrderByDescending(s => s.PickUpDeliveryNumber).FirstOrDefault();
        }
        private void GetInsidePackagesData(IShipmentsContext context, ShipmentPackage package, ShipmentPM shipment, PackageLine line)
        {
            List<InsideShipmentPackage> insidePackages = context.InsideShipmentPackages.Include("PackageType").Where(d => d.ShipmentPackageId == package.Id && d.Tenant == package.Tenant).ToList();

            //line.InsidePackageList = new List<InsidePackage>();
            //line.TotalFor = package.ContainerNumber != null ? "Total For " + package.ContainerNumber : "";

            StringBuilder qty = new StringBuilder();
            StringBuilder type = new StringBuilder();
            StringBuilder desc = new StringBuilder();
            StringBuilder weight = new StringBuilder();
            StringBuilder volume = new StringBuilder();

            int count = 0;

            foreach (InsideShipmentPackage insPackage in insidePackages)
            {
                count = myServicHelper.GetStringLinesCount(insPackage.Description);

                qty.Append(insPackage.Quantity.ToString(), count, 1).Append('\n');
                type.Append(insPackage.PackageType != null ? insPackage.PackageType.EnglishName : "").Append('\n');
                desc.Append(insPackage.Description != null ? insPackage.Description : "").Append('\n');
                weight.Append(String.Format("{0:#0.00}", insPackage.Weight.Value) + " " + (shipment.GrossWeightUnitCode != null ? shipment.GrossWeightUnitCode : "")).Append('\n');
                volume.Append(String.Format("{0:#0.00}", insPackage.Volume.Value) + " " + (shipment.VolumeUnitCode != null ? shipment.VolumeUnitCode : "")).Append('\n');
            }

            line.PackageQuantity = qty.ToString() + package.Quantity;
            line.PackageType = type.ToString() + (package.PackageType != null ? package.PackageType.EnglishName : "");
            line.PackageDescriptionOfGoods = desc.ToString() + (package.Description != null ? package.Description : "");
            line.PackageGrossWeight = weight.ToString() + String.Format("{0:#0.00}", package.Weight.Value) + " " + (shipment.GrossWeightUnitCode != null ? shipment.GrossWeightUnitCode : "");
            line.PackageVolume = volume.ToString() + String.Format("{0:#0.00}", package.Volume.Value) + " " + (shipment.VolumeUnitCode != null ? shipment.VolumeUnitCode : "");
            line.ContainerNumber = package.ContainerNumber;
            line.Seal1 = package.ShipperSeal;
            line.Seal2 = package.CarrierSeal;

            #region Car Details
            line.Make = package.Make;
            line.Model = package.Model;
            line.Year = package.Year;
            line.Color = package.Color;
            line.ChassisNumber = package.ChassisNumber;
            line.RegistrationNumber = package.RegistrationNumber;

            if (!string.IsNullOrEmpty(package.CountryId))
            {
                Country country = CountryRepository.GetSingleCountry(package.CountryId, tenant, true);
                if (country != null)
                {
                    line.CountryName = country.EnglishName;
                }
            }
            #endregion

            if (package.MarksAndNumbers == null)
            {
                if (package.PackageType != null)
                {
                    if (package.PackageType.IsContainer)
                    {
                        if (package.ContainerNumber != null)
                        {
                            if (package.ContainerNumber.Length > 10)
                            {
                                line.PackageMarksAndNumbers = !string.IsNullOrEmpty(package.ContainerNumber) ? package.ContainerNumber.Substring(0, 4) + " " + package.ContainerNumber.Substring(4, 6) + "/" + package.ContainerNumber.Substring(10, 1) + Environment.NewLine : "";
                            }
                            else
                            {
                                line.PackageMarksAndNumbers = package.ContainerNumber;
                            }

                            line.PackageMarksAndNumbersNew = package.ContainerNumber;
                        }
                        if (!string.IsNullOrEmpty(package.ShipperSeal))
                        {
                            line.PackageMarksAndNumbers = line.PackageMarksAndNumbers + "SEAL:" + package.ShipperSeal + Environment.NewLine;
                            line.PackageMarksAndNumbersNew = line.PackageMarksAndNumbersNew + "SEAL:" + package.ShipperSeal + Environment.NewLine;
                        }

                        if (package.Tare != null)
                        {
                            line.PackageGrossWeight += Environment.NewLine + "TARE:" + package.Tare + (shipment.GrossWeightUnitCode != null ? shipment.GrossWeightUnitCode : "");
                        }
                    }
                }

                line.PackageMarksAndNumbers = line.PackageMarksAndNumbers + "\n" + (package.ContainerNumber != null ? "Total For " + package.ContainerNumber : "");
                line.PackageMarksAndNumbersNew = line.PackageMarksAndNumbersNew + "\n" + (package.ContainerNumber != null ? "Total For " + package.ContainerNumber : "");
            }
            else
            {
                line.PackageMarksAndNumbers = package.MarksAndNumbers != null ? package.MarksAndNumbers : "";
                line.PackageMarksAndNumbersNew = package.MarksAndNumbers != null ? package.MarksAndNumbers : "";

                line.PackageMarksAndNumbers = line.PackageMarksAndNumbers + "\n" + (package.ContainerNumber != null ? "Total For " + package.ContainerNumber : "");
                line.PackageMarksAndNumbersNew = line.PackageMarksAndNumbersNew + "\n" + (package.ContainerNumber != null ? "Total For " + package.ContainerNumber : "");
            }
        }

        private string ComputeInsidePackagesDetailsPerPackage(ShipmentPackage package)
        {
            string result = "";

            InsideShipmentPackageQuery query = new InsideShipmentPackageQuery(package.Tenant);
            List<InsideShipmentPackagePM> insidePackages = query.GetInsideShipmentPackages(package.Id, package.Tenant);

            if (insidePackages.Count > 0)
            {
                List<InsidePackageGroup> groupedList = (from b in insidePackages
                                                        group b by new { b.PackageTypeName } into g
                                                        select new InsidePackageGroup()
                                                        {
                                                            PackageTypeName = g.Key.PackageTypeName,
                                                            Count = g.Sum(b => b.Quantity),
                                                        }).ToList();

                foreach (InsidePackageGroup item in groupedList)
                {
                    if (string.IsNullOrEmpty(result))
                    {
                        result = item.Count + " " + item.PackageTypeName;
                    }

                    else
                    {
                        result = result + " + " + item.Count + " " + item.PackageTypeName;
                    }
                }
            }

            return result;
        }

        public byte[] GetFBLDataForPickUp(string shipmentId, string pickUpId, int tenant)
        {
            this.tenant = tenant;
            this.myServicHelper = new WebServiceHelper(tenant);

            FBLDataProvider myDataProvider = GetFBLDataProviderForPickUp(shipmentId, pickUpId, tenant);

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(FBLDataProvider));
            MemoryStream memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream, myDataProvider);
            memoryStream.Seek(0, SeekOrigin.Begin);

            var reader = new StreamReader(memoryStream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memoryStream.ToArray();
            return bytearray;
        }
        private FBLDataProvider GetFBLDataProviderForPickUp(string shipmentId, string pickUpId, int tenant)
        {
            FBLDataProvider myDataProvider = new FBLDataProvider();
            myDataProvider.PackagesLines = new List<PackageLine>();

            IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(tenant);
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentsContext);
            ShipmentPickUpDeliveryRepository shipmentPickUpDeliveryRepository = new ShipmentPickUpDeliveryRepository(shipmentsContext);
            ShipmentPickUpDeliveryPackageRepository shipmentPickUpDeliveryPackageRepository = new ShipmentPickUpDeliveryPackageRepository(shipmentsContext);
            TenantRepository tenantRepository = new TenantRepository(commonContext);
            AddressRepository addressRepository = new AddressRepository(commonContext);

            ShipmentDataView myShipment = shipmentRepository.GetSingleShipmentDataView(shipmentId, tenant);
            ShipmentPickUpDelivery myShipmentPickUp = shipmentPickUpDeliveryRepository.GetSingleShipmentPickUpDelivery(tenant, pickUpId);

            if (myShipment != null && myShipmentPickUp != null)
            {
                myDataProvider.ShipmentNumber = myShipment.ShipmentNumber;
                myDataProvider.ConsigneeRef1 = myShipment.ConsigneeReference1;
                myDataProvider.MainCarriageVesselNameAndNumber = myShipment.MainCarriageVesselName + " \\ " + myShipment.MainCarriageCarrierNumber;

                if (!string.IsNullOrEmpty(myShipment.ShipperId))
                {
                    Card shipper = CardRepository.GetSingleCard(myShipment.ShipperId, tenant, true);
                    if (shipper != null)
                    {
                        myDataProvider.ShipperAddress = shipper.EnglishName + Environment.NewLine;

                        if (!string.IsNullOrEmpty(myShipment.ShipperAddressId))
                        {
                            Address shipperAddress = addressRepository.GetSingleAddress(myShipment.ShipperAddressId, tenant);

                            if (shipperAddress != null)
                            {
                                if (shipperAddress.IsLocalLanguage && !string.IsNullOrEmpty(shipper.LocalName))
                                {
                                    myDataProvider.ShipperAddress = shipper.LocalName + Environment.NewLine;
                                }

                                myDataProvider.ShipperAddress = myDataProvider.ShipperAddress + DataProviders.General.GetAddress(shipperAddress);

                                if (shipperAddress.PhoneNumber != null || shipperAddress.FaxNumber != null)
                                {
                                    myDataProvider.ShipperAddress = myDataProvider.ShipperAddress + Environment.NewLine + (shipperAddress.PhoneNumber != null ? "Tel: " + shipperAddress.PhoneNumber + " " : "") + (shipperAddress.FaxNumber != null ? "Fax: " + shipperAddress.FaxNumber + " " : "");
                                }

                                if (shipperAddress.State != null)
                                {
                                    myDataProvider.ShipperAddress = myDataProvider.ShipperAddress + ", " + shipperAddress.State.Code;
                                }
                            }
                        }
                    }
                }

                if (!string.IsNullOrEmpty(myShipment.ConsigneeId))
                {
                    Card consignee = CardRepository.GetSingleCard(myShipment.ConsigneeId, tenant, true);
                    if (consignee != null)
                    {
                        myDataProvider.ConsigneeAddress = consignee.EnglishName + Environment.NewLine;

                        if (!string.IsNullOrEmpty(myShipment.ConsigneeAddressId))
                        {
                            Address consigneeAddress = addressRepository.GetSingleAddress(myShipment.ConsigneeAddressId, tenant);

                            if (consigneeAddress != null)
                            {
                                if (consigneeAddress.IsLocalLanguage && !string.IsNullOrEmpty(consignee.LocalName))
                                {
                                    myDataProvider.ConsigneeAddress = consignee.LocalName + Environment.NewLine;
                                }

                                myDataProvider.ConsigneeAddress = myDataProvider.ConsigneeAddress + DataProviders.General.GetAddress(consigneeAddress);

                                if (consigneeAddress.PhoneNumber != null || consigneeAddress.FaxNumber != null)
                                {
                                    myDataProvider.ConsigneeAddress = myDataProvider.ConsigneeAddress + Environment.NewLine + (consigneeAddress.PhoneNumber != null ? "Tel: " + consigneeAddress.PhoneNumber + " " : "") + (consigneeAddress.FaxNumber != null ? "Fax: " + consigneeAddress.FaxNumber + " " : "");
                                }

                                if (consigneeAddress.State != null)
                                {
                                    myDataProvider.ConsigneeAddress = myDataProvider.ConsigneeAddress + ", " + consigneeAddress.State.Code;
                                }
                            }
                        }
                    }
                }

                IQueryable<ShipmentPickUpDeliveryPackage> packages = shipmentPickUpDeliveryPackageRepository.GetPackagesByDeliveryId(pickUpId, tenant);
                if (packages.Count() > 0)
                {
                    foreach (ShipmentPickUpDeliveryPackage package in packages)
                    {
                        PackageLine packageline = new PackageLine();
                        PackageType packageType = (from pa in commonContext.PackageTypes
                                                   where pa.Id == package.PackageTypeId
                                                   select pa).FirstOrDefault();

                        packageline.PackageQuantity = package.Quantity != null ? package.Quantity.Value.ToString() : "";
                        packageline.PackageGrossWeight = package.Weight != null ? String.Format("{0:N2}", package.Weight) + (myShipment.GrossWeightUnitCode != null ? myShipment.GrossWeightUnitCode : "") : "";
                        packageline.PackageDescriptionOfGoods = package.Description != null ? (package.Description + Environment.NewLine) : "";

                        if (packageType != null)
                        {
                            if (packageType.IsContainer)
                            {
                                packageline.PackageType = packageType.PrintAs;
                            }

                            else
                            {
                                packageline.PackageType = packageType.EnglishName;
                            }                            
                        }
                        
                        if (package.Length != null && package.Width != null && package.Height != null)
                        {
                            packageline.Dimensions = package.Length + "x" + package.Width + "x" + package.Height;
                        }
                        
                        myDataProvider.PackagesLines.Add(packageline);
                    }
                }

                Tenant myTenant = tenantRepository.GetSingleTenant(tenant);
                if (myTenant != null)
                {
                    myDataProvider.CompanyName = myTenant.Company;
                    myDataProvider.Signature = myTenant.Signature;

                    if (!string.IsNullOrEmpty(myTenant.AddressId))
                    {
                        Address tenantAddress = addressRepository.GetSingleAddress(myTenant.AddressId, tenant);
                        if (tenantAddress != null)
                        {
                            myDataProvider.TenantCountryCode = tenantAddress.Country == null ? null : tenantAddress.Country.Code;
                        }
                    }
                }
            }

            return myDataProvider;
        }

        private FBLDataProvider MapTransshipmentOneFields(FBLDataProvider fBLDataProvider,ShipmentPM shipmentPM)
        {
            fBLDataProvider.Transshipment1ATA = shipmentPM.Transshipment1ATA;
            fBLDataProvider.Transshipment1ATD = shipmentPM.Transshipment1ATD;
            fBLDataProvider.Transshipment1ETA = shipmentPM.Transshipment1ETA;
            fBLDataProvider.Transshipment1ETD = shipmentPM.Transshipment1ETD;
            fBLDataProvider.Transshipment1CarrierName = shipmentPM.Transshipment1CarrierName;
            fBLDataProvider.Transshipment1CarrierNumber = shipmentPM.Transshipment1CarrierNumber;
            fBLDataProvider.Transshipment1FromPortCode = shipmentPM.Transshipment1FromPortCode;
            fBLDataProvider.Transshipment1FromPortName = shipmentPM.Transshipment1FromPortName;
            fBLDataProvider.Transshipment1ToPortCode = shipmentPM.Transshipment1ToPortCode;
            fBLDataProvider.Transshipment1ToPortName = shipmentPM.Transshipment1ToPortName;

            return fBLDataProvider;
        }

        private FBLDataProvider MapTransshipmentTwoFields(FBLDataProvider fBLDataProvider, ShipmentPM shipmentPM)
        {
            fBLDataProvider.Transshipment2ATA = shipmentPM.Transshipment2ATA;
            fBLDataProvider.Transshipment2ATD = shipmentPM.Transshipment2ATD;
            fBLDataProvider.Transshipment2ETA = shipmentPM.Transshipment2ETA;
            fBLDataProvider.Transshipment2ETD = shipmentPM.Transshipment2ETD;
            fBLDataProvider.Transshipment2CarrierName = shipmentPM.Transshipment2CarrierName;
            fBLDataProvider.Transshipment2CarrierNumber = shipmentPM.Transshipment2CarrierNumber;
            fBLDataProvider.Transshipment2FromPortCode = shipmentPM.Transshipment2FromPortCode;
            fBLDataProvider.Transshipment2FromPortName = shipmentPM.Transshipment2FromPortName;
            fBLDataProvider.Transshipment2ToPortCode = shipmentPM.Transshipment2ToPortCode;
            fBLDataProvider.Transshipment2ToPortName = shipmentPM.Transshipment2ToPortName;

            return fBLDataProvider;
        }

        private byte[] GetUserSignatureImage(int tenant)
        {
            string contactEmail = AuthenticationUtil.GetLoggedUserEmail(tenant);
            if (string.IsNullOrEmpty(contactEmail))
                return null;

            User currentUser = GetCurrentUserByContactEmail(tenant, contactEmail);
            if (currentUser == null)
                return null;

            return DataProviders.General.GetUserSignatureImage(currentUser.SignatureImageId, tenant); ;
        }

        public User GetCurrentUserByContactEmail(int tenant , string contactEmail)
        {
            User currentUser = (from a in commonContext.Users
                                where a.Contact.Email == contactEmail && a.Tenant == tenant
                                select a).FirstOrDefault();

            return currentUser;
        }

    }
}

