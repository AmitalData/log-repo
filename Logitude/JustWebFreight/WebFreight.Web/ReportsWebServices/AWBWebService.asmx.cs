using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Services;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using WebFreight.Web.DataProviders;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.WebServices;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.ShipmentsModel.Tools.DataMapping;
using Simplog.Data.InfrastructureModel.Repositories;
using Logitude.BL.Helpers;
using System.Transactions;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.ReportsWebServices
{
    /// <summary>
    /// Summary description for AWBWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class AWBWebService : System.Web.Services.WebService
    {
        private int myTenant;
        private string myCCSTypeCode;
        private bool isRegulatedAgentActivated;

        [WebMethod]
        public int GetByte()
        {
            return 0;
        }

        [WebMethod]
        public byte[] LoadDataToAWB(string shipmentId, int tenant,string documentTypeCopyId)
        {
            this.myTenant = tenant;
            return StartLoadingDataToAWB(shipmentId, tenant, documentTypeCopyId,false);
        }

        public byte[] StartLoadingDataToAWB(string shipmentId, int tenant, string documentTypeCopyId,bool isPrint)
        {
            this.myTenant = tenant;
            AWBDataProvider awbDp = LoadAWBDataProvider(shipmentId, tenant, documentTypeCopyId,isPrint);
            XmlSerializer serializer = new XmlSerializer(typeof(AWBDataProvider));
            MemoryStream memstream = new MemoryStream();
            serializer.Serialize(memstream, awbDp);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();
            return bytearray;
        }

        private ICommonDataContext myCommonContext =null;
        private AWBDataProvider LoadAWBDataProvider(string shipmentId, int tenant, string documentTypeCopyId,bool isPrint)
        {
            this.myTenant = tenant;
            AWBDataProvider awbDp = new AWBDataProvider();
            IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(tenant);
            ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentsContext);
            ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRepository);            
            AddressRepository addressRepository = new AddressRepository(tenant);
            ShipmentPM shipmentPM = shipmentQuery.GetSinglePM(shipmentId, tenant);            
            WebServiceHelper myServiceHelper = new WebServiceHelper(tenant);

            this.myCommonContext = CommonDataContext.GetContext(tenant);

            if (shipmentPM != null)
            {
                awbDp.AWBAccount = shipmentPM.AccountNumber == null ? "" : shipmentPM.AccountNumber;
                awbDp.SCI = shipmentPM.SCI == null ? "" : shipmentPM.SCI;

                awbDp.AWBComments = shipmentPM.AWBComments == null ? "" : shipmentPM.AWBComments;
                if (!string.IsNullOrEmpty(shipmentPM.AWBPrintingComments))
                {
                    awbDp.AWBComments = shipmentPM.AWBPrintingComments;
                }
                
                awbDp.MAWBShort = shipmentPM.Master == null ? "" : shipmentPM.Master;
                awbDp.HAWB = shipmentPM.House == null ? "" : shipmentPM.House;
                awbDp.LeadingCurrency = shipmentPM.AWBCurrencyCode == null ? "" : shipmentPM.AWBCurrencyCode;
                awbDp.Signature = shipmentPM.AWBSignature;
                awbDp.ShipmentNumber = shipmentPM.ShipmentNumber;
                awbDp.ChargesCode = shipmentPM.AWBChargesCodeCode;
                awbDp.InsurrenceValue = !string.IsNullOrEmpty(shipmentPM.AWBInsurrenceValue) ? shipmentPM.AWBInsurrenceValue : "";
                awbDp.DeclaredValueForCarriage = !string.IsNullOrEmpty(shipmentPM.AWBDeclaredValueForCarriage) ? shipmentPM.AWBDeclaredValueForCarriage : "";
                awbDp.DeclaredValueForCustoms = !string.IsNullOrEmpty(shipmentPM.AWBDeclaredValueForCustoms) ? shipmentPM.AWBDeclaredValueForCustoms : "";
                awbDp.CarrierTarrifRef = !string.IsNullOrEmpty(shipmentPM.AWBCarrierTarrifReference) ? "AIRLINE REF.:" + shipmentPM.AWBCarrierTarrifReference : "";
                awbDp.Notes = !string.IsNullOrEmpty(shipmentPM.Notes) ? shipmentPM.Notes : "";
                awbDp.AgentReference1 = !string.IsNullOrEmpty(shipmentPM.AgentReference1) ? shipmentPM.AgentReference1 : "";
                awbDp.AgentReference2 = !string.IsNullOrEmpty(shipmentPM.AgentReference2) ? shipmentPM.AgentReference2 : "";
                awbDp.MainCarriageETD = shipmentPM.MainCarriageETD;
                awbDp.Transshipment1ETD = shipmentPM.Transshipment1ETD;
                awbDp.Transshipment2ETD = shipmentPM.Transshipment2ETD;
                awbDp.Transshipment3ETD = shipmentPM.Transshipment3ETD;
                awbDp.IncotermCode = shipmentPM.IncotermCode == null ? "" : shipmentPM.IncotermCode;
                awbDp.ENSDate = shipmentPM.ENSDate;
                awbDp.ENSNumber = shipmentPM.ENSNumber;
                awbDp.ITNumber = shipmentPM.ITNumber;
                awbDp.VolumeInCBM = shipmentPM.VolumeInCBM + " CBM";
                awbDp.VolumeUnitCode = shipmentPM.VolumeUnitCode;
                awbDp.ChargeableWeightEdited = shipmentPM.ChargeableWeightEdited;
                awbDp.MainCarriageLeg2_MAWB = shipmentPM.Transshipment1AdditionalMAWBOBLBL;

                if (shipmentPM.BranchId != null)
                {
                    Branch myBranch = (from d in myCommonContext.Branches where d.Tenant == tenant && d.Id == shipmentPM.BranchId select d).FirstOrDefault();
                    if (myBranch != null)
                    {
                        awbDp.BranchSignature = myBranch.Signature;
                        awbDp.Branch = myBranch.EnglishName;
                    }
                }

                this.GetLoggedTenantData(awbDp);
                this.GetLoggedContactData(awbDp, tenant);
                this.GetPortsData(awbDp, shipmentPM);
                this.GetCarriersData(awbDp, shipmentPM, addressRepository);
                this.GetShipperData(awbDp, shipmentPM, addressRepository);
                this.GetConsigneeData(awbDp, shipmentPM, addressRepository);
                this.GetFlightsNumberAndDate(awbDp, shipmentPM);
                this.GetCompanyIATACode(awbDp, shipmentPM);
                this.GetCompanyAddress(awbDp, shipmentPM, addressRepository);
                this.GetMAWBOBLDate(awbDp, shipmentPM);
                this.GetPrepaidCollectCharges(awbDp, shipmentPM, shipmentsContext);
                this.GetHandlingAndAccountingData(awbDp, shipmentPM, addressRepository);                
                this.GetCopyNameData(awbDp, documentTypeCopyId, tenant);
                this.GetCommoditiesData(awbDp, shipmentPM);
                this.GetCustomFieldsData(awbDp, shipmentPM);
                this.GetAWBPrintingFields(awbDp, shipmentPM);
                this.GetNotify1Data(awbDp, shipmentPM, addressRepository);
                this.GetNotify2Data(awbDp, shipmentPM, addressRepository);
                this.GetAgentData(awbDp, shipmentPM, addressRepository);
                this.GetConsolidatorData(awbDp, shipmentPM);

                #region PlaceOfDelivery

                ShipmentPickUpDelivery myDelivery = (from d in shipmentsContext.ShipmentPickUpDeliveries
                                                     where d.ShipmentId == shipmentPM.Id && d.PickUpDeliveryTypeCode == "DELV"
                                                     select d).OrderBy(s => s.PickUpDeliveryNumber).FirstOrDefault();
                
                awbDp.PlaceOfDelivery = myServiceHelper.GetPlaceOfDelivery(shipmentPM, myDelivery);

                #endregion

                if (isPrint)
                {                    
                    Shipment shipmentEntity = shipmentRepository.GetSingleShipment(shipmentId, tenant);

                    shipmentEntity.AWBPrint = true;
                    shipmentEntity.FNAReason = null;
                    shipmentRepository.Update(shipmentEntity);
                    shipmentRepository.SubmitChanges();
                }
            }

            try
            {
                Type awbDpType = awbDp.GetType();
                PropertyInfo[] properties = awbDpType.GetProperties();
                foreach (PropertyInfo pi in properties)
                {
                    if (pi.GetValue(awbDp, null) == null || pi.GetValue(awbDp, null).ToString() == "0" || pi.GetValue(awbDp, null).ToString() == "00.00" || pi.GetValue(awbDp, null).ToString() == "0.00")
                    {
                        pi.SetValue(awbDp, "", null);
                    }
                }
            }

            catch
            {

            }

            return awbDp;
        }

        private void GetLoggedTenantData(AWBDataProvider awbDp)
        {
            TenantRepository tenantRepository = new TenantRepository(myTenant);
            Tenant myLoggedTenant = tenantRepository.GetSingleTenant(myTenant);

            if (myLoggedTenant != null)
            {
                if (myCCSTypeCode == "GLSHK")
                {
                    if (myLoggedTenant.RegulatedAgentRegimeActivated)
                    {
                        this.isRegulatedAgentActivated = true;
                    }
                }

                awbDp.FMCNumber = myLoggedTenant.FMCNumber;
            }

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                TenantManagementRepository tenantManagementRepository = new TenantManagementRepository();
                TenantManagement tenantManagement = tenantManagementRepository.GetSingleTenantManagement(myTenant);

                if (tenantManagement != null)
                {
                    this.myCCSTypeCode = tenantManagement.AWBMessagesCCSTypeCode;
                }

                scope.Complete();
            }
        }

        private void GetLoggedContactData(AWBDataProvider awbDp, int tenant)
        {
            if (User != null)
            {
                ContactQuery contactQuery = new ContactQuery(tenant);
                ContactPM contactPM = contactQuery.GetContactByEmailOnly(User.Identity.Name, tenant);

                if (contactPM != null)
                {
                    awbDp.UserName = contactPM.EnglishName;
                    awbDp.UserEmail = contactPM.Email != null ? contactPM.Email : "";
                }
            }
        }

        private void GetPortsData(AWBDataProvider awbDp, ShipmentPM shipmentPM)
        {
            int tenant = shipmentPM.Tenant;
            PortPM mainCarriageFromPort = null;
            PortPM mainCarriageToPort = null;
            PortPM transshipment1ToPort = null;
            PortPM transshipment2ToPort = null;
            PortPM transshipment3ToPort = null;

            awbDp.Place = shipmentPM.AWBPlace;

            if (!string.IsNullOrEmpty(shipmentPM.MainCarriageFromPortId))
            {
                mainCarriageFromPort = PortQuery.GetSinglePort(tenant, shipmentPM.MainCarriageFromPortId, false);
                if (mainCarriageFromPort != null)
                {
                    awbDp.MainCarriageFromPortCode = mainCarriageFromPort.Code;
                    awbDp.MainCarriageFromPortName = mainCarriageFromPort.EnglishName;                    
                }
            }

            if (!string.IsNullOrEmpty(shipmentPM.MainCarriageToPortId))
            {
                mainCarriageToPort = PortQuery.GetSinglePort(tenant, shipmentPM.MainCarriageToPortId, false);
                if (mainCarriageToPort != null)
                {
                    awbDp.MainCarriageToPortCode = mainCarriageToPort.Code;
                }
            }

            if (!string.IsNullOrEmpty(shipmentPM.Transshipment1ToPortId))
            {
                transshipment1ToPort = PortQuery.GetSinglePort(tenant, shipmentPM.Transshipment1ToPortId, false);
                if (transshipment1ToPort != null)
                {
                    awbDp.Transshipment1ToPortCode = transshipment1ToPort.Code;
                }
            }

            if (!string.IsNullOrEmpty(shipmentPM.Transshipment2ToPortId))
            {
                transshipment2ToPort = PortQuery.GetSinglePort(tenant, shipmentPM.Transshipment2ToPortId, false);
                if (transshipment2ToPort != null)
                {
                    awbDp.Transshipment2ToPortCode = transshipment2ToPort.Code;
                }
            }

            if (!string.IsNullOrEmpty(shipmentPM.Transshipment2ToPortId))
            {
                transshipment3ToPort = PortQuery.GetSinglePort(tenant, shipmentPM.Transshipment3ToPortId, false);
            }
        

            if (shipmentPM.OnCarriageToPortId != null)
            {
                if (shipmentPM.ShipmentLevelCode == "H")
                {
                    awbDp.LastToInMainCarriage = shipmentPM.OnCarriageToPortName;
                }
            }

            else
            {
                if (transshipment3ToPort != null)
                {
                    awbDp.LastToInMainCarriage = transshipment3ToPort.EnglishName;
                }

                else if (transshipment2ToPort != null)
                {
                    awbDp.LastToInMainCarriage = transshipment2ToPort.EnglishName;
                }

                else if (transshipment1ToPort != null)
                {
                    awbDp.LastToInMainCarriage = transshipment1ToPort.EnglishName;
                }

                else if (mainCarriageToPort != null)
                {
                    awbDp.LastToInMainCarriage = mainCarriageToPort.EnglishName;
                }
            }
        }

        private void GetCarriersData(AWBDataProvider awbDp, ShipmentPM shipmentPM, AddressRepository addressRepository)
        {
            int tenant = shipmentPM.Tenant;

            #region MainCarriageCarrier
            string mainCarrierPrefix = string.IsNullOrEmpty(shipmentPM.MainCarriageCarrierPrefix) ? "" : shipmentPM.MainCarriageCarrierPrefix.Trim();

            if (!string.IsNullOrEmpty(mainCarrierPrefix))
            {
                awbDp.MainCarriageCarrierCode = mainCarrierPrefix.ToUpper();
            }
            
            if (!string.IsNullOrEmpty(shipmentPM.MainCarriageCarrierId))
            {
                string mainCarrierNumber = string.IsNullOrEmpty(shipmentPM.MainCarriageCarrierNumber) ? "" : shipmentPM.MainCarriageCarrierNumber.Trim();

                Card mainCarrier = CardRepository.GetSingleCard(shipmentPM.MainCarriageCarrierId, tenant, false);

                if (mainCarrier != null)
                {
                    if (string.IsNullOrEmpty(awbDp.MainCarriageCarrierCode))
                    {
                        awbDp.MainCarriageCarrierCode = mainCarrier.Code;
                    }

                    awbDp.MainCarriageCarrierName = mainCarrier.EnglishName;
                    awbDp.MainCarriageFullCarrierNumber = mainCarrier.Code + mainCarrierNumber;

                    Address mainCarrierAddress = addressRepository.GetMainAddressByCardId(mainCarrier.Id, tenant);
                    if (mainCarrierAddress != null)
                    {
                        if (mainCarrierAddress.IsLocalLanguage && !string.IsNullOrEmpty(mainCarrier.LocalName))
                        {
                            awbDp.MainCarriageCarrierName = mainCarrier.LocalName;
                        }

                        awbDp.MainCarriageCarrierAddress = DataProviders.General.GetAddress(mainCarrierAddress);
                    }

                    awbDp.MAWBFull = shipmentPM.LongMaster;
                    awbDp.MainCarriageCarrierPrefix = shipmentPM.AirlinePrefix;
                }
            }
            #endregion

            #region Transshipment1Carrier
            string prefix1 = string.IsNullOrEmpty(shipmentPM.Transshipment1CarrierPrefix) ? "" : shipmentPM.Transshipment1CarrierPrefix.Trim();
          
            if (!string.IsNullOrEmpty(prefix1))
            {
                awbDp.Transshipment1CarrierCode = prefix1.ToUpper();
            }

            else
            {
                Card card = CardRepository.GetSingleCard(shipmentPM.Transshipment1CarrierId, tenant, false);

                if (card != null)
                {
                    awbDp.Transshipment1CarrierCode = card.Code;
                }
            }
            #endregion

            #region Transshipment2Carrier
            string prefix2 = string.IsNullOrEmpty(shipmentPM.Transshipment2CarrierPrefix) ? "" : shipmentPM.Transshipment2CarrierPrefix.Trim();

            if (!string.IsNullOrEmpty(prefix2))
            {
                awbDp.Transshipment2CarrierCode = prefix2.ToUpper();
            }

            else
            {
                Card card = CardRepository.GetSingleCard(shipmentPM.Transshipment2CarrierId, tenant, false);

                if (card != null)
                {
                    awbDp.Transshipment2CarrierCode = card.Code;
                }
            }
            #endregion

            #region Interline Carrier            
            if (!string.IsNullOrEmpty(shipmentPM.InterlineId))
            {
                Card interlineCarrier = CardRepository.GetSingleCard(shipmentPM.InterlineId, tenant, false);
                if (interlineCarrier != null)
                {
                    awbDp.InterlineCarrierName = interlineCarrier.EnglishName;

                    Address interlineCarrierAddress = addressRepository.GetMainAddressByCardId(interlineCarrier.Id, tenant);
                    if (interlineCarrierAddress != null)
                    {
                        if (interlineCarrierAddress.IsLocalLanguage && !string.IsNullOrEmpty(interlineCarrier.LocalName))
                        {
                            awbDp.InterlineCarrierName = interlineCarrier.LocalName;
                        }

                        awbDp.InterlineCarrierAddress = DataProviders.General.GetAddress(interlineCarrierAddress);
                    }
                }
            }

            else
            {
                awbDp.InterlineCarrierName = awbDp.MainCarriageCarrierName;
                awbDp.InterlineCarrierAddress = awbDp.MainCarriageCarrierAddress;
            }
            #endregion
        }

        private void GetShipperData(AWBDataProvider awbDp, ShipmentPM shipmentPM, AddressRepository addressRepository)
        {
            int tenant = shipmentPM.Tenant;
            string shipperId = !string.IsNullOrEmpty(shipmentPM.ShipperNotExporterId) ? shipmentPM.ShipperNotExporterId : shipmentPM.ShipperId;
            string shipperAddressId = !string.IsNullOrEmpty(shipmentPM.ShipperNotExporterAddressId) ? shipmentPM.ShipperNotExporterAddressId : shipmentPM.ShipperAddressId;
            ContactRepository contactRepository = new ContactRepository(myCommonContext);

            if (!string.IsNullOrEmpty(shipperId))
            {
                Card shipperCard = CardRepository.GetSingleCard(shipperId, tenant, false);
                if (shipperCard != null)
                {
                    awbDp.ShipperNameAddress = shipperCard.EnglishName != null ? shipperCard.EnglishName + Environment.NewLine : "";
                    awbDp.ShipperName = shipperCard.EnglishName == null ? "" : shipperCard.EnglishName;
                    awbDp.ShipperVATNo = shipperCard.VatNumber == null ? "" : shipperCard.VatNumber;
                    awbDp.ShipperReference1 = shipmentPM.ShipperReference1;
                    awbDp.ShipperReference2 = shipmentPM.ShipperReference2;

                    if (!string.IsNullOrEmpty(shipperAddressId))
                    {
                        Address shipperAddress = addressRepository.GetSingleAddress(shipperAddressId, tenant);

                        if (shipperAddress != null)
                        {
                            if (shipperAddress.IsLocalLanguage)
                            {
                                if (shipperCard != null && !string.IsNullOrEmpty(shipperCard.LocalName))
                                {
                                    awbDp.ShipperNameAddress = shipperCard.LocalName + Environment.NewLine;
                                }
                            }

                            awbDp.ShipperNameAddress = awbDp.ShipperNameAddress + DataProviders.General.GetAddress(shipperAddress) + Environment.NewLine;

                            if (!string.IsNullOrEmpty(shipperAddress.PhoneNumber))
                            {
                                awbDp.ShipperNameAddress = awbDp.ShipperNameAddress + "Tel: " + shipperAddress.PhoneNumber + " ";
                                awbDp.ShipperTel = shipperAddress.PhoneNumber;
                            }

                            if (!string.IsNullOrEmpty(shipperAddress.FaxNumber))
                            {
                                awbDp.ShipperNameAddress = awbDp.ShipperNameAddress + "Fax: " + shipperAddress.FaxNumber;
                                awbDp.ShipperFax = shipperAddress.FaxNumber;
                            }

                            awbDp.ShipperAddress1 = shipperAddress.Address1 == null ? "" : shipperAddress.Address1;
                            awbDp.ShipperAddress2 = shipperAddress.Address2 == null ? "" : shipperAddress.Address2;
                            awbDp.ShipperCity = shipperAddress.City == null ? "" : shipperAddress.City;
                            awbDp.ShipperCountry = shipperAddress.Country == null ? "" : shipperAddress.Country.EnglishName;
                            awbDp.ShipperZipCode = shipperAddress.ZipCode == null ? "" : shipperAddress.ZipCode;                            
                        }
                    }
                }
            }

            //Actual
            if (!string.IsNullOrEmpty(shipmentPM.ShipperId))
            {
                Card actualShipperCard = CardRepository.GetSingleCard(shipmentPM.ShipperId, tenant, false);
                if (actualShipperCard != null)
                {
                    awbDp.ActualShipperNameAddress = actualShipperCard.EnglishName != null ? actualShipperCard.EnglishName + Environment.NewLine : "";
                    
                    if (!string.IsNullOrEmpty(shipmentPM.ShipperAddressId))
                    {
                        Address actualShipperAddress = addressRepository.GetSingleAddress(shipmentPM.ShipperAddressId, tenant);

                        if (actualShipperAddress != null)
                        {
                            awbDp.ShipperAddress_WithName = DataProviders.General.GetAddressWithName(actualShipperAddress);
                            awbDp.ShipperATTN = actualShipperAddress.ATTN;

                            if (actualShipperAddress.IsLocalLanguage)
                            {
                                if (actualShipperCard != null && !string.IsNullOrEmpty(actualShipperCard.LocalName))
                                {
                                    awbDp.ActualShipperNameAddress = actualShipperCard.LocalName + Environment.NewLine;
                                }
                            }

                            awbDp.ActualShipperNameAddress = awbDp.ActualShipperNameAddress + DataProviders.General.GetAddress(actualShipperAddress) + Environment.NewLine;

                            if (!string.IsNullOrEmpty(actualShipperAddress.PhoneNumber))
                            {
                                awbDp.ActualShipperNameAddress = awbDp.ActualShipperNameAddress + "Tel: " + actualShipperAddress.PhoneNumber + " ";
                            }

                            if (!string.IsNullOrEmpty(actualShipperAddress.FaxNumber))
                            {
                                awbDp.ActualShipperNameAddress = awbDp.ActualShipperNameAddress + "Fax: " + actualShipperAddress.FaxNumber;
                            }
                        }
                    }

                    if(!string.IsNullOrEmpty(actualShipperCard.PrimaryContactId))
                    {
                        Contact primaryContact = contactRepository.GetSingleContact(actualShipperCard.PrimaryContactId, tenant);
                        if (primaryContact != null)
                        {
                            awbDp.ShipperPrimaryContactName = primaryContact.EnglishName;
                            awbDp.ShipperPrimaryContactPhone = primaryContact.BusinessPhone;                            
                        }
                    }
                }
            }

            // ShipperNotExporter
            if (!string.IsNullOrEmpty(shipmentPM.ShipperNotExporterId))
            {
                if (!string.IsNullOrEmpty(shipmentPM.ShipperNotExporterAddressId))
                {
                    Address myPartnerAddress = addressRepository.GetSingleAddress(shipmentPM.ShipperNotExporterAddressId, tenant);

                    if (myPartnerAddress != null)
                    {
                        awbDp.ShipperNotExporterAddress_WithName = DataProviders.General.GetAddressWithName(myPartnerAddress);
                        awbDp.ShipperNotExporterATTN = myPartnerAddress.ATTN;
                    }
                }
            }

            if (!string.IsNullOrEmpty(shipmentPM.ShipperNotExporterContactId))
            {               
                Contact contact = contactRepository.GetSingleContact(shipmentPM.ShipperNotExporterContactId, tenant);
                if (contact != null)
                {
                    awbDp.ShipperNotExporterContactDetails = contact.EnglishName;

                    if (!string.IsNullOrEmpty(contact.BusinessPhone))
                    {
                        awbDp.ShipperNotExporterContactDetails = awbDp.ShipperNotExporterContactDetails + Environment.NewLine + "Ph: " + contact.BusinessPhone;

                        if (!string.IsNullOrEmpty(contact.Fax))
                        {
                            awbDp.ShipperNotExporterContactDetails = awbDp.ShipperNotExporterContactDetails + " - Fx: " + contact.Fax;
                        }
                    }

                    else
                    {
                        if (!string.IsNullOrEmpty(contact.Fax))
                        {
                            awbDp.ShipperNotExporterContactDetails = awbDp.ShipperNotExporterContactDetails + Environment.NewLine + "Fx: " + contact.Fax;
                        }
                    }

                    if (!string.IsNullOrEmpty(contact.Email))
                    {
                        awbDp.ShipperNotExporterContactDetails = awbDp.ShipperNotExporterContactDetails + Environment.NewLine + "Email: " + contact.Email;
                    }
                }
            }

            if (!string.IsNullOrEmpty(shipmentPM.ShipperContactId))
            {
                Contact shipperContact = contactRepository.GetSingleContact(shipmentPM.ShipperContactId, tenant);

                if (shipperContact != null)
                {
                    awbDp.ShipperContactDetails = shipperContact.EnglishName;

                    if (!string.IsNullOrEmpty(shipperContact.BusinessPhone))
                    {
                        awbDp.ShipperContactDetails = awbDp.ShipperContactDetails + Environment.NewLine + "Ph: " + shipperContact.BusinessPhone;

                        if (!string.IsNullOrEmpty(shipperContact.Fax))
                        {
                            awbDp.ShipperContactDetails = awbDp.ShipperContactDetails + " - Fx: " + shipperContact.Fax;
                        }
                    }

                    else
                    {
                        if (!string.IsNullOrEmpty(shipperContact.Fax))
                        {
                            awbDp.ShipperContactDetails = awbDp.ShipperContactDetails + Environment.NewLine + "Fx: " + shipperContact.Fax;
                        }
                    }

                    if (!string.IsNullOrEmpty(shipperContact.Email))
                    {
                        awbDp.ShipperContactDetails = awbDp.ShipperContactDetails + Environment.NewLine + "Email: " + shipperContact.Email;
                    }
                }
            }
        }

        private void GetConsigneeData(AWBDataProvider awbDp, ShipmentPM shipmentPM, AddressRepository addressRepository)
        {
            int tenant = shipmentPM.Tenant;
            string consigneeId = !string.IsNullOrEmpty(shipmentPM.ConsigneeNotImporterId) ? shipmentPM.ConsigneeNotImporterId : shipmentPM.ConsigneeId;
            string consigneeAddressId = !string.IsNullOrEmpty(shipmentPM.ConsigneeNotImporterAddressId) ? shipmentPM.ConsigneeNotImporterAddressId : shipmentPM.ConsigneeAddressId;
            ContactRepository contactRepository = new ContactRepository(myCommonContext);

            if (!string.IsNullOrEmpty(consigneeId))
            {
                Card consigneeCard = CardRepository.GetSingleCard(consigneeId, tenant, false);
                if (consigneeCard != null)
                {
                    awbDp.ConsigneeNameAddress = consigneeCard.EnglishName != null ? consigneeCard.EnglishName + Environment.NewLine : "";
                    awbDp.ConsigneeName = consigneeCard.EnglishName == null ? "" : consigneeCard.EnglishName;
                    awbDp.ConsigneeVATNo = consigneeCard.VatNumber == null ? "" : consigneeCard.VatNumber;

                    if (!string.IsNullOrEmpty(consigneeAddressId))
                    {
                        Address consigneeAddress = addressRepository.GetSingleAddress(consigneeAddressId, tenant);

                        if (consigneeAddress != null)
                        {
                            if (consigneeAddress.IsLocalLanguage)
                            {
                                if (consigneeCard != null && !string.IsNullOrEmpty(consigneeCard.LocalName))
                                {
                                    awbDp.ConsigneeNameAddress = consigneeCard.LocalName + Environment.NewLine;
                                }
                            }

                            awbDp.ConsigneeNameAddress = awbDp.ConsigneeNameAddress + DataProviders.General.GetAddress(consigneeAddress) + Environment.NewLine;

                            if (!string.IsNullOrEmpty(consigneeAddress.PhoneNumber))
                            {
                                awbDp.ConsigneeNameAddress = awbDp.ConsigneeNameAddress + "Tel: " + consigneeAddress.PhoneNumber + " ";
                                awbDp.ConsigneeTel = consigneeAddress.PhoneNumber;
                            }

                            if (!string.IsNullOrEmpty(consigneeAddress.FaxNumber))
                            {
                                awbDp.ConsigneeNameAddress = awbDp.ConsigneeNameAddress + "Fax: " + consigneeAddress.FaxNumber;
                                awbDp.ConsigneeFax = consigneeAddress.FaxNumber;
                            }

                            awbDp.ConsigneeAddress1 = consigneeAddress.Address1 == null ? "" : consigneeAddress.Address1;
                            awbDp.ConsigneeAddress2 = consigneeAddress.Address2 == null ? "" : consigneeAddress.Address2;
                            awbDp.ConsigneeCity = consigneeAddress.City == null ? "" : consigneeAddress.City;
                            awbDp.ConsigneeCountry = consigneeAddress.Country == null ? "" : consigneeAddress.Country.EnglishName;
                            awbDp.ConsigneeZipCode = consigneeAddress.ZipCode == null ? "" : consigneeAddress.ZipCode;
                        }
                    }
                }
            }

            //Actual
            if (!string.IsNullOrEmpty(shipmentPM.ConsigneeId))
            {
                Card actualConsigneeCard = CardRepository.GetSingleCard(shipmentPM.ConsigneeId, tenant, false);
                if (actualConsigneeCard != null)
                {
                    awbDp.ActualConsigneeNameAddress = actualConsigneeCard.EnglishName != null ? actualConsigneeCard.EnglishName + Environment.NewLine : "";
                    
                    if (!string.IsNullOrEmpty(shipmentPM.ConsigneeAddressId))
                    {
                        Address actualConsigneeAddress = addressRepository.GetSingleAddress(shipmentPM.ConsigneeAddressId, tenant);

                        if (actualConsigneeAddress != null)
                        {
                            awbDp.ConsigneeAddress_WithName = DataProviders.General.GetAddressWithName(actualConsigneeAddress);
                            awbDp.ConsigneeATTN = actualConsigneeAddress.ATTN;

                            if (actualConsigneeAddress.IsLocalLanguage)
                            {
                                if (actualConsigneeCard != null && !string.IsNullOrEmpty(actualConsigneeCard.LocalName))
                                {
                                    awbDp.ActualConsigneeNameAddress = actualConsigneeCard.LocalName + Environment.NewLine;
                                }
                            }

                            awbDp.ActualConsigneeNameAddress = awbDp.ActualConsigneeNameAddress + DataProviders.General.GetAddress(actualConsigneeAddress) + Environment.NewLine;

                            if (!string.IsNullOrEmpty(actualConsigneeAddress.PhoneNumber))
                            {
                                awbDp.ActualConsigneeNameAddress = awbDp.ActualConsigneeNameAddress + "Tel: " + actualConsigneeAddress.PhoneNumber + " ";
                            }

                            if (!string.IsNullOrEmpty(actualConsigneeAddress.FaxNumber))
                            {
                                awbDp.ActualConsigneeNameAddress = awbDp.ActualConsigneeNameAddress + "Fax: " + actualConsigneeAddress.FaxNumber;
                            }
                        }
                    }

                    if (!string.IsNullOrEmpty(actualConsigneeCard.PrimaryContactId))
                    {
                        Contact primaryContact = contactRepository.GetSingleContact(actualConsigneeCard.PrimaryContactId, tenant);
                        if (primaryContact != null)
                        {
                            awbDp.ConsigneePrimaryContactName = primaryContact.EnglishName;
                            awbDp.ConsigneePrimaryContactPhone = primaryContact.BusinessPhone;
                        }
                    }
                }
            }

            // ConsigneeNotImporter
            if (!string.IsNullOrEmpty(shipmentPM.ConsigneeNotImporterId))
            {
                if (!string.IsNullOrEmpty(shipmentPM.ConsigneeNotImporterAddressId))
                {
                    Address myPartnerAddress = addressRepository.GetSingleAddress(shipmentPM.ConsigneeNotImporterAddressId, tenant);

                    if (myPartnerAddress != null)
                    {
                        awbDp.ConsigneeNotImporterATTN = myPartnerAddress.ATTN;
                    }
                }
            }

            if (!string.IsNullOrEmpty(shipmentPM.ConsigneeContactId))
            {
                Contact consigneeContact = contactRepository.GetSingleContact(shipmentPM.ConsigneeContactId, tenant);

                if (consigneeContact != null)
                {
                    awbDp.ConsigneeContactDetails = consigneeContact.EnglishName;

                    if (!string.IsNullOrEmpty(consigneeContact.BusinessPhone))
                    {
                        awbDp.ConsigneeContactDetails = awbDp.ConsigneeContactDetails + Environment.NewLine + "Ph: " + consigneeContact.BusinessPhone;

                        if (!string.IsNullOrEmpty(consigneeContact.Fax))
                        {
                            awbDp.ConsigneeContactDetails = awbDp.ConsigneeContactDetails + " - Fx: " + consigneeContact.Fax;
                        }
                    }

                    else
                    {
                        if (!string.IsNullOrEmpty(consigneeContact.Fax))
                        {
                            awbDp.ConsigneeContactDetails = awbDp.ConsigneeContactDetails + Environment.NewLine + "Fx: " + consigneeContact.Fax;
                        }
                    }

                    if (!string.IsNullOrEmpty(consigneeContact.Email))
                    {
                        awbDp.ConsigneeContactDetails = awbDp.ConsigneeContactDetails + Environment.NewLine + "Email: " + consigneeContact.Email;
                    }
                }
            }
        }

        private void GetFlightsNumberAndDate(AWBDataProvider awbDp, ShipmentPM shipmentPM)
        {
            if (shipmentPM.MainCarriageETD != null)
            {
                awbDp.FlightDate = shipmentPM.MainCarriageETD;

                if (shipmentPM.MainCarriageCarrierNumber != null)
                {
                    awbDp.MainCarriageFlightNumberAndDate = shipmentPM.MainCarriageCarrierNumber + "/" + String.Format("{0:dd-MMM}", shipmentPM.MainCarriageETD);
                    awbDp.FlightNumber = shipmentPM.MainCarriageCarrierNumber;
                }

                else
                {
                    awbDp.MainCarriageFlightNumberAndDate = String.Format("{0:dd-MMM}", shipmentPM.MainCarriageETD);
                }
            }

            else if (shipmentPM.MainCarriageATD != null)
            {
                awbDp.FlightDate = shipmentPM.MainCarriageATD;

                if (shipmentPM.MainCarriageCarrierNumber != null)
                {
                    awbDp.MainCarriageFlightNumberAndDate = shipmentPM.MainCarriageCarrierNumber + "/" + String.Format("{0:dd-MMM}", shipmentPM.MainCarriageATD);
                    awbDp.FlightNumber = shipmentPM.MainCarriageCarrierNumber;
                }

                else
                {
                    awbDp.MainCarriageFlightNumberAndDate = String.Format("{0:dd-MMM}", shipmentPM.MainCarriageATD);
                }
            }

            else
            {
                awbDp.MainCarriageFlightNumberAndDate = shipmentPM.MainCarriageCarrierNumber;
                awbDp.FlightNumber = shipmentPM.MainCarriageCarrierNumber;
            }

            if (shipmentPM.Transshipment1ETD != null)
            {
                if (shipmentPM.Transshipment1CarrierNumber != null)
                {
                    awbDp.Transshipment1FlightNumberAndDate = shipmentPM.Transshipment1CarrierNumber + "/" + String.Format("{0:dd-MMM}", shipmentPM.Transshipment1ETD);
                }

                else
                {
                    awbDp.Transshipment1FlightNumberAndDate = String.Format("{0:dd-MMM}", shipmentPM.Transshipment1ETD);
                }
            }

            else if (shipmentPM.Transshipment1ATD != null)
            {
                if (shipmentPM.Transshipment1CarrierNumber != null)
                {
                    awbDp.Transshipment1FlightNumberAndDate = shipmentPM.Transshipment1CarrierNumber + "/" + String.Format("{0:dd-MMM}", shipmentPM.Transshipment1ATD);
                }

                else
                {
                    awbDp.Transshipment1FlightNumberAndDate = String.Format("{0:dd-MMM}", shipmentPM.Transshipment1ATD);
                }
            }

            else
            {
                awbDp.Transshipment1FlightNumberAndDate = shipmentPM.Transshipment1CarrierNumber;
            }

            if (shipmentPM.Transshipment2ETD != null)
            {
                if (shipmentPM.Transshipment2CarrierNumber != null)
                {
                    awbDp.Transshipment2FlightNumberAndDate = shipmentPM.Transshipment2CarrierNumber + "/" + String.Format("{0:dd-MMM}", shipmentPM.Transshipment2ETD);
                }

                else
                {
                    awbDp.Transshipment2FlightNumberAndDate = String.Format("{0:dd-MMM}", shipmentPM.Transshipment2ETD);
                }
            }

            else if (shipmentPM.Transshipment2ATD != null)
            {
                if (shipmentPM.Transshipment2CarrierNumber != null)
                {
                    awbDp.Transshipment2FlightNumberAndDate = shipmentPM.Transshipment2CarrierNumber + "/" + String.Format("{0:dd-MMM}", shipmentPM.Transshipment2ATD);
                }

                else
                {
                    awbDp.Transshipment2FlightNumberAndDate = String.Format("{0:dd-MMM}", shipmentPM.Transshipment2ATD);
                }
            }

            else
            {
                awbDp.Transshipment2FlightNumberAndDate = shipmentPM.Transshipment2CarrierNumber;
            }
        }

        private void GetCompanyIATACode(AWBDataProvider awbDp, ShipmentPM shipmentPM)
        {
            awbDp.CompanyIATACode = shipmentPM.IssuingCarrierIATACode == null ? "" : shipmentPM.IssuingCarrierIATACode;

            if (!string.IsNullOrEmpty(shipmentPM.CASSCode))
            {
                if (string.IsNullOrEmpty(awbDp.CompanyIATACode))
                {
                    awbDp.CompanyIATACode = shipmentPM.CASSCode;
                }

                else
                {
                    awbDp.CompanyIATACode += " / " + shipmentPM.CASSCode;
                }
            }
        }

        private void GetCompanyAddress(AWBDataProvider awbDp, ShipmentPM shipmentPM, AddressRepository addressRepository)
        {
            int tenant = shipmentPM.Tenant;
            Address issuingCarrierAddress = addressRepository.GetSingleAddress(shipmentPM.IssuingCarrierAddressId, tenant);
            if (issuingCarrierAddress != null)
            {
                awbDp.TenantCompanyNameAddress = issuingCarrierAddress.Name + Environment.NewLine + this.GetAddress(issuingCarrierAddress) + Environment.NewLine;
            }
        }

        private void GetMAWBOBLDate(AWBDataProvider awbDp, ShipmentPM shipmentPM)
        {
            if (shipmentPM.MAWBOBLDate != null)
            {
                awbDp.MAWBOBLDate = String.Format("{0:dd/MMM/yyyy}", shipmentPM.MAWBOBLDate.Value);
            }

            else
            {
                awbDp.MAWBOBLDate = String.Format("{0:dd/MMM/yyyy}", TenantServerConfigration.GetCurrentDateTime(shipmentPM.Tenant));
            }
        }

        private void GetPrepaidCollectCharges(AWBDataProvider awbDp, ShipmentPM shipmentPM, IShipmentsContext shipmentsContext)
        {
            int tenant = shipmentPM.Tenant;

            if (shipmentPM.OtherPrepaidCollectId == "P")
            {
                awbDp.OtherPrepaid = "X";
            }

            else if (shipmentPM.OtherPrepaidCollectId == "C")
            {
                awbDp.OtherCollect = "X";
            }

            if (shipmentPM.FreightPrepaidCollectId == "P")
            {
                awbDp.FreightPrepaid = "X";
            }

            else if (shipmentPM.FreightPrepaidCollectId == "C")
            {
                awbDp.FreightCollect = "X";
            } 

            awbDp.OtherCharges = "";
            awbDp.OtherCharges_ChargeEnglishName = "";
            awbDp.OtherCharges_AsAgreed = "";
            awbDp.ChargeDetails = "";
            int lineMemberCounter = 0;
            bool lastIsIata = false;
            bool hasDescription = false;

            double totalPrepaid = 0;
            double taxTotalPrepaid = 0;
            double valuationTotalPrepaid = 0;
            double totalOtherChargesPrepaidAgent = 0;
            double totalOtherChargesPrepaidCarrier = 0;

            double totalCollect = 0;
            double taxTotalCollect = 0;
            double valuationtotalcollect = 0;
            double totalOtherChargesCollectAgent = 0;
            double totalOtherChargesCollectCarrier = 0;

            List<ShipmentReceivable> receivables = shipmentsContext.ShipmentReceivables.Include("ChargesType").Where(d => d.ShipmentId == shipmentPM.Id && d.Tenant == tenant).ToList();
            List<ShipmentPayable> payables = shipmentsContext.ShipmentPayables.Include("ChargesType").Where(d => d.ShipmentId == shipmentPM.Id && d.Tenant == tenant && d.ShipmentPayableAmountTypeCode != "NEXP").ToList();
            List<ShipmentAWBPrintOnly> shipmentAwbPrintOnlies = shipmentsContext.ShipmentAWBPrintOnlies.Where(d => d.ShipmentId == shipmentPM.Id && d.Tenant == tenant).ToList();

            IATACodeRepository iATACodeRepository = new IATACodeRepository(shipmentPM.Tenant);
            List<IATACode> allIATACodes = iATACodeRepository.GetIATACodes().ToList();

            #region Receivables
            foreach (ShipmentReceivable receivable in receivables.Where(d => d.TotalAmount != 0 && d.TotalAmount != null))
            {
                if (receivable.AWBPrint)
                {
                    if (receivable.ChargesType.ChargesGroupCode != "FRT" && (receivable.DueTypeCode == "AG" || receivable.DueTypeCode == "CA"))
                    {
                        if (receivable.AWBPrint && receivable.DueTypeCode == "AG" && receivable.PrepaidCollectId == "P")
                        {
                            totalOtherChargesPrepaidAgent = totalOtherChargesPrepaidAgent + (receivable.TotalAmount != null ? receivable.TotalAmount.Value : 0);
                        }
                        if (receivable.AWBPrint && receivable.DueTypeCode == "AG" && receivable.PrepaidCollectId == "C")
                        {
                            totalOtherChargesCollectAgent = totalOtherChargesCollectAgent + (receivable.TotalAmount != null ? receivable.TotalAmount.Value : 0);
                        }
                        if (receivable.AWBPrint && receivable.DueTypeCode == "CA" && receivable.PrepaidCollectId == "P")
                        {
                            totalOtherChargesPrepaidCarrier = totalOtherChargesPrepaidCarrier + (receivable.TotalAmount != null ? receivable.TotalAmount.Value : 0);
                        }
                        if (receivable.AWBPrint && receivable.DueTypeCode == "CA" && receivable.PrepaidCollectId == "C")
                        {
                            totalOtherChargesCollectCarrier = totalOtherChargesCollectCarrier + (receivable.TotalAmount != null ? receivable.TotalAmount.Value : 0);
                        }

                        if (receivable.IATACodeId != null)
                        {
                            string myIATACode = "";
                            IATACode iATACode = allIATACodes.Where(d => d.Id == receivable.IATACodeId).FirstOrDefault();
                            if (iATACode != null)
                            {
                                myIATACode = iATACode.Code;
                            }

                            awbDp.ChargeDetails = awbDp.ChargeDetails + "  " + myIATACode + " " + receivable.ChargesType.EnglishName + " " + String.Format("{0:#,0.00}", receivable.TotalAmount != null ? receivable.TotalAmount.Value : 0);
                        }
                        else
                        {
                            awbDp.ChargeDetails = awbDp.ChargeDetails + "  " + receivable.ChargesType.EnglishName + " " + String.Format("{0:#,0.00}", receivable.TotalAmount != null ? receivable.TotalAmount.Value : 0);
                        }

                        if (receivable.IATACodeId != null && !receivable.ChargesType.AWBPrintDescription)
                        {
                            string myIATACode = "";
                            IATACode iATACode = allIATACodes.Where(d => d.Id == receivable.IATACodeId).FirstOrDefault();
                            if (iATACode != null)
                            {
                                myIATACode = iATACode.Code;
                            }

                            if (lastIsIata && !hasDescription)
                            {
                                if (lineMemberCounter < 3)
                                {
                                    awbDp.OtherCharges = awbDp.OtherCharges + "  " + myIATACode + (receivable.DueTypeCode == "CA" ? "C" : "A") + " " + String.Format("{0:#,0.00}", receivable.TotalAmount != null ? receivable.TotalAmount.Value : 0);
                                    awbDp.OtherCharges_ChargeEnglishName = awbDp.OtherCharges_ChargeEnglishName + "  " + receivable.ChargesType.Code + (receivable.DueTypeCode == "CA" ? "C" : "A") + " " + receivable.ChargesType.EnglishName + " " + String.Format("{0:#,0.00}", receivable.TotalAmount != null ? receivable.TotalAmount.Value : 0);
                                    lineMemberCounter++;
                                }
                                else
                                {
                                    awbDp.OtherCharges = awbDp.OtherCharges + Environment.NewLine + " " + myIATACode + (receivable.DueTypeCode == "CA" ? "C" : "A") + " " + String.Format("{0:#,0.00}", receivable.TotalAmount != null ? receivable.TotalAmount.Value : 0);
                                    awbDp.OtherCharges_ChargeEnglishName = awbDp.OtherCharges_ChargeEnglishName + Environment.NewLine + "  " + receivable.ChargesType.Code + (receivable.DueTypeCode == "CA" ? "C" : "A") + " " + receivable.ChargesType.EnglishName + " " + String.Format("{0:#,0.00}", receivable.TotalAmount != null ? receivable.TotalAmount.Value : 0);
                                    lineMemberCounter = 1;
                                    hasDescription = false;
                                }
                            }
                            else
                            {
                                if (lineMemberCounter < 2)
                                {
                                    awbDp.OtherCharges = awbDp.OtherCharges + "  " + myIATACode + (receivable.DueTypeCode == "CA" ? "C" : "A") + " " + String.Format("{0:#,0.00}", receivable.TotalAmount != null ? receivable.TotalAmount.Value : 0);
                                    awbDp.OtherCharges_ChargeEnglishName = awbDp.OtherCharges_ChargeEnglishName + "  " + receivable.ChargesType.Code + (receivable.DueTypeCode == "CA" ? "C" : "A") + " " + receivable.ChargesType.EnglishName + " " + String.Format("{0:#,0.00}", receivable.TotalAmount != null ? receivable.TotalAmount.Value : 0);
                                    lineMemberCounter++;
                                }
                                else
                                {
                                    awbDp.OtherCharges = awbDp.OtherCharges + Environment.NewLine + myIATACode + (receivable.DueTypeCode == "CA" ? "C" : "A") + " " + String.Format("{0:#,0.00}", receivable.TotalAmount != null ? receivable.TotalAmount.Value : 0);
                                    awbDp.OtherCharges_ChargeEnglishName = awbDp.OtherCharges_ChargeEnglishName + Environment.NewLine + "  " + receivable.ChargesType.Code + (receivable.DueTypeCode == "CA" ? "C" : "A") + " " + receivable.ChargesType.EnglishName + " " + String.Format("{0:#,0.00}", receivable.TotalAmount != null ? receivable.TotalAmount.Value : 0);
                                    lineMemberCounter = 1;
                                    hasDescription = false;
                                }
                            }

                            lastIsIata = true;
                        }

                        else
                        {
                            if (lineMemberCounter < 2)
                            {
                                awbDp.OtherCharges = awbDp.OtherCharges + "  " + receivable.ChargesType.EnglishName + " " + String.Format("{0:#,0.00}", receivable.TotalAmount != null ? receivable.TotalAmount.Value : 0);
                                awbDp.OtherCharges_ChargeEnglishName = awbDp.OtherCharges_ChargeEnglishName + "  " + receivable.ChargesType.Code + (receivable.DueTypeCode == "CA" ? "C" : "A") + " " + receivable.ChargesType.EnglishName + " " + String.Format("{0:#,0.00}", receivable.TotalAmount != null ? receivable.TotalAmount.Value : 0);
                                lineMemberCounter++;
                            }

                            else
                            {
                                awbDp.OtherCharges = awbDp.OtherCharges + Environment.NewLine + " " + receivable.ChargesType.EnglishName + " " + String.Format("{0:#,0.00}", receivable.TotalAmount != null ? receivable.TotalAmount.Value : 0);
                                awbDp.OtherCharges_ChargeEnglishName = awbDp.OtherCharges_ChargeEnglishName +  Environment.NewLine + "  " + receivable.ChargesType.Code + (receivable.DueTypeCode == "CA" ? "C" : "A") + " " + receivable.ChargesType.EnglishName + " " + String.Format("{0:#,0.00}", receivable.TotalAmount != null ? receivable.TotalAmount.Value : 0);
                                lineMemberCounter = 1;
                            }

                            lastIsIata = false;
                            hasDescription = true;
                        }
                    }

                    if (receivable.DueTypeCode.ToUpper() == "TX")
                    {
                        if (receivable.PrepaidCollectId == "C")
                        {
                            if (receivable.TotalAmount != null)
                                taxTotalCollect = taxTotalCollect + receivable.TotalAmount.Value;
                        }

                        else
                        {
                            if (receivable.TotalAmount != null)
                                taxTotalPrepaid = taxTotalPrepaid + receivable.TotalAmount.Value;
                        }
                    }

                    if (receivable.DueTypeCode.ToUpper() == "VL")
                    {
                        if (receivable.PrepaidCollectId == "C")
                        {
                            if (receivable.TotalAmount != null)
                                valuationtotalcollect = valuationtotalcollect + receivable.TotalAmount.Value;
                        }
                        else
                        {
                            if (receivable.TotalAmount != null)
                                valuationTotalPrepaid = valuationTotalPrepaid + receivable.TotalAmount.Value;
                        }
                    }
                }
            }
            #endregion

            #region Payables
            foreach (ShipmentPayable payable in payables.Where(d => d.ExpectedAmount != 0 && d.ExpectedAmount != null))
            {
                if (payable.AWBPrint && payable.ShipmentPayableAmountTypeCode != "NEXP")
                {
                    if (payable.ChargesType.ChargesGroupCode != "FRT" && (payable.DueTypeCode == "AG" || payable.DueTypeCode == "CA"))
                    {
                        if (payable.AWBPrint && payable.DueTypeCode == "AG" && payable.PrepaidCollectId == "P")
                        {
                            totalOtherChargesPrepaidAgent = totalOtherChargesPrepaidAgent + (payable.ExpectedAmount != null ? payable.ExpectedAmount.Value : 0);
                        }
                        if (payable.AWBPrint && payable.DueTypeCode == "AG" && payable.PrepaidCollectId == "C")
                        {
                            totalOtherChargesCollectAgent = totalOtherChargesCollectAgent + (payable.ExpectedAmount != null ? payable.ExpectedAmount.Value : 0);
                        }
                        if (payable.AWBPrint && payable.DueTypeCode == "CA" && payable.PrepaidCollectId == "P")
                        {
                            totalOtherChargesPrepaidCarrier = totalOtherChargesPrepaidCarrier + (payable.ExpectedAmount != null ? payable.ExpectedAmount.Value : 0);
                        }
                        if (payable.AWBPrint && payable.DueTypeCode == "CA" && payable.PrepaidCollectId == "C")
                        {
                            totalOtherChargesCollectCarrier = totalOtherChargesCollectCarrier + (payable.ExpectedAmount != null ? payable.ExpectedAmount.Value : 0);
                        }

                        if (payable.IATACodeId != null)
                        {
                            string myIATACode = "";
                            IATACode iATACode = allIATACodes.Where(d => d.Id == payable.IATACodeId).FirstOrDefault();
                            if (iATACode != null)
                            {
                                myIATACode = iATACode.Code;
                            }

                            awbDp.ChargeDetails = awbDp.ChargeDetails + "  " + myIATACode + " " + payable.ChargesType.EnglishName + " " + String.Format("{0:#,0.00}", payable.ExpectedAmount != null ? payable.ExpectedAmount.Value : 0);
                        }
                        else
                        {
                            awbDp.ChargeDetails = awbDp.ChargeDetails + "  " + payable.ChargesType.EnglishName + " " + String.Format("{0:#,0.00}", payable.ExpectedAmount != null ? payable.ExpectedAmount.Value : 0);
                        }

                        if (payable.IATACodeId != null && !payable.ChargesType.AWBPrintDescription)
                        {
                            string myIATACode = "";
                            IATACode iATACode = allIATACodes.Where(d => d.Id == payable.IATACodeId).FirstOrDefault();
                            if (iATACode != null)
                            {
                                myIATACode = iATACode.Code;
                            }

                            if (lastIsIata && !hasDescription)
                            {
                                if (lineMemberCounter < 3)
                                {
                                    awbDp.OtherCharges = awbDp.OtherCharges + "  " + myIATACode + (payable.DueTypeCode == "CA" ? "C" : "A") + " " + String.Format("{0:#,0.00}", payable.ExpectedAmount != null ? payable.ExpectedAmount.Value : 0);
                                    awbDp.OtherCharges_ChargeEnglishName = awbDp.OtherCharges_ChargeEnglishName + "  " + payable.ChargesType.Code + (payable.DueTypeCode == "CA" ? "C" : "A") + " " + payable.ChargesType.EnglishName + " " + String.Format("{0:#,0.00}", payable.ExpectedAmount != null ? payable.ExpectedAmount.Value : 0);
                                    lineMemberCounter++;
                                }
                                else
                                {
                                    awbDp.OtherCharges = awbDp.OtherCharges + Environment.NewLine + " " + myIATACode + (payable.DueTypeCode == "CA" ? "C" : "A") + " " + String.Format("{0:#,0.00}", payable.ExpectedAmount != null ? payable.ExpectedAmount.Value : 0);
                                    awbDp.OtherCharges_ChargeEnglishName = awbDp.OtherCharges_ChargeEnglishName + Environment.NewLine + "  " + payable.ChargesType.Code + (payable.DueTypeCode == "CA" ? "C" : "A") + " " + payable.ChargesType.EnglishName + " " + String.Format("{0:#,0.00}", payable.ExpectedAmount != null ? payable.ExpectedAmount.Value : 0);
                                    lineMemberCounter = 1;
                                    hasDescription = false;
                                }
                            }
                            else
                            {
                                if (lineMemberCounter < 2)
                                {
                                    awbDp.OtherCharges = awbDp.OtherCharges + "  " + myIATACode + (payable.DueTypeCode == "CA" ? "C" : "A") + " " + String.Format("{0:#,0.00}", payable.ExpectedAmount != null ? payable.ExpectedAmount.Value : 0);
                                    awbDp.OtherCharges_ChargeEnglishName = awbDp.OtherCharges_ChargeEnglishName + "  " + payable.ChargesType.Code + (payable.DueTypeCode == "CA" ? "C" : "A") + " " + payable.ChargesType.EnglishName + " " + String.Format("{0:#,0.00}", payable.ExpectedAmount != null ? payable.ExpectedAmount.Value : 0);
                                    lineMemberCounter++;
                                }
                                else
                                {
                                    awbDp.OtherCharges = awbDp.OtherCharges + Environment.NewLine + myIATACode + (payable.DueTypeCode == "CA" ? "C" : "A") + " " + String.Format("{0:#,0.00}", payable.ExpectedAmount != null ? payable.ExpectedAmount.Value : 0);
                                    awbDp.OtherCharges_ChargeEnglishName = awbDp.OtherCharges_ChargeEnglishName + Environment.NewLine + "  " + payable.ChargesType.Code + (payable.DueTypeCode == "CA" ? "C" : "A") + " " + payable.ChargesType.EnglishName + " " + String.Format("{0:#,0.00}", payable.ExpectedAmount != null ? payable.ExpectedAmount.Value : 0);
                                    lineMemberCounter = 1;
                                    hasDescription = false;
                                }
                            }
                            lastIsIata = true;
                        }
                        else
                        {

                            if (lineMemberCounter < 2)
                            {
                                awbDp.OtherCharges = awbDp.OtherCharges + "  " + payable.ChargesType.EnglishName + " " + String.Format("{0:#,0.00}", payable.ExpectedAmount != null ? payable.ExpectedAmount.Value : 0);
                                awbDp.OtherCharges_ChargeEnglishName = awbDp.OtherCharges_ChargeEnglishName + "  " + payable.ChargesType.Code + (payable.DueTypeCode == "CA" ? "C" : "A") + " " + payable.ChargesType.EnglishName + " " + String.Format("{0:#,0.00}", payable.ExpectedAmount != null ? payable.ExpectedAmount.Value : 0);
                                lineMemberCounter++;
                            }
                            else
                            {
                                awbDp.OtherCharges = awbDp.OtherCharges + Environment.NewLine + " " + payable.ChargesType.EnglishName + " " + String.Format("{0:#,0.00}", payable.ExpectedAmount != null ? payable.ExpectedAmount.Value : 0);
                                awbDp.OtherCharges_ChargeEnglishName = awbDp.OtherCharges_ChargeEnglishName + Environment.NewLine + "  " + payable.ChargesType.Code + (payable.DueTypeCode == "CA" ? "C" : "A") + " " + payable.ChargesType.EnglishName + " " + String.Format("{0:#,0.00}", payable.ExpectedAmount != null ? payable.ExpectedAmount.Value : 0);
                                lineMemberCounter = 1;
                            }
                            lastIsIata = false;
                            hasDescription = true;

                        }
                    }

                    if (payable.DueTypeCode.ToUpper() == "TX")
                    {
                        if (payable.PrepaidCollectId == "C")
                        {
                            if (payable.ExpectedAmount != null)
                                taxTotalCollect = taxTotalCollect + payable.ExpectedAmount.Value;
                        }
                        else
                        {
                            if (payable.ExpectedAmount != null)
                                taxTotalPrepaid = taxTotalPrepaid + payable.ExpectedAmount.Value;
                        }
                    }

                    if (payable.DueTypeCode.ToUpper() == "VL")
                    {
                        if (payable.PrepaidCollectId == "C")
                        {
                            if (payable.ExpectedAmount != null)
                                valuationtotalcollect = valuationtotalcollect + payable.ExpectedAmount.Value;
                        }
                        else
                        {
                            if (payable.ExpectedAmount != null)
                                valuationTotalPrepaid = valuationTotalPrepaid + payable.ExpectedAmount.Value;
                        }
                    }
                }
            }
            #endregion

            #region PrintOnlies
            foreach (ShipmentAWBPrintOnly printOnly in shipmentAwbPrintOnlies.Where(d => d.Amount != 0 && d.Amount != null))
            {
                if (printOnly.DueTypeCode == "AG" || printOnly.DueTypeCode == "CA")
                {
                    if (printOnly.DueTypeCode == "AG" && printOnly.PrepaidCollectId == "P")
                    {
                        totalOtherChargesPrepaidAgent = totalOtherChargesPrepaidAgent + (printOnly.Amount != null ? printOnly.Amount.Value : 0);
                    }
                    if (printOnly.DueTypeCode == "AG" && printOnly.PrepaidCollectId == "C")
                    {
                        totalOtherChargesCollectAgent = totalOtherChargesCollectAgent + (printOnly.Amount != null ? printOnly.Amount.Value : 0);
                    }
                    if (printOnly.DueTypeCode == "CA" && printOnly.PrepaidCollectId == "P")
                    {
                        totalOtherChargesPrepaidCarrier = totalOtherChargesPrepaidCarrier + (printOnly.Amount != null ? printOnly.Amount.Value : 0);
                    }
                    if (printOnly.DueTypeCode == "CA" && printOnly.PrepaidCollectId == "C")
                    {
                        totalOtherChargesCollectCarrier = totalOtherChargesCollectCarrier + (printOnly.Amount != null ? printOnly.Amount.Value : 0);
                    }

                    if (printOnly.IATACodeId != null)
                    {
                        string myIATACode = "";
                        string myIATAName = "";
                        IATACode iATACode = allIATACodes.Where(d => d.Id == printOnly.IATACodeId).FirstOrDefault();
                        if (iATACode != null)
                        {
                            myIATACode = iATACode.Code;
                            myIATAName = iATACode.Name;
                        }

                        awbDp.ChargeDetails = awbDp.ChargeDetails + "  " + myIATACode + " " + myIATAName + " " + String.Format("{0:#,0.00}", printOnly.Amount != null ? printOnly.Amount.Value : 0);

                        if (lastIsIata && !hasDescription)
                        {
                            if (lineMemberCounter < 3)
                            {
                                awbDp.OtherCharges = awbDp.OtherCharges + "  " + myIATACode + (printOnly.DueTypeCode == "CA" ? "C" : "A") + " " + String.Format("{0:#,0.00}", printOnly.Amount != null ? printOnly.Amount.Value : 0);
                                awbDp.OtherCharges_ChargeEnglishName = awbDp.OtherCharges_ChargeEnglishName + "  " + myIATACode + (printOnly.DueTypeCode == "CA" ? "C" : "A") + " " + myIATAName + " " + String.Format("{0:#,0.00}", printOnly.Amount != null ? printOnly.Amount.Value : 0);
                                lineMemberCounter++;
                            }
                            else
                            {
                                awbDp.OtherCharges = awbDp.OtherCharges + Environment.NewLine + " " + myIATACode + (printOnly.DueTypeCode == "CA" ? "C" : "A") + " " + String.Format("{0:#,0.00}", printOnly.Amount != null ? printOnly.Amount.Value : 0);
                                awbDp.OtherCharges_ChargeEnglishName = awbDp.OtherCharges_ChargeEnglishName + Environment.NewLine + " " + myIATACode + (printOnly.DueTypeCode == "CA" ? "C" : "A") + " " + myIATAName + " " + String.Format("{0:#,0.00}", printOnly.Amount != null ? printOnly.Amount.Value : 0);
                                lineMemberCounter = 1;
                                hasDescription = false;
                            }
                        }
                        else
                        {
                            if (lineMemberCounter < 2)
                            {
                                awbDp.OtherCharges = awbDp.OtherCharges + "  " + myIATACode + (printOnly.DueTypeCode == "CA" ? "C" : "A") + " " + String.Format("{0:#,0.00}", printOnly.Amount != null ? printOnly.Amount.Value : 0);
                                awbDp.OtherCharges_ChargeEnglishName = awbDp.OtherCharges_ChargeEnglishName + "  " + myIATACode + (printOnly.DueTypeCode == "CA" ? "C" : "A") + " " + myIATAName + " " + String.Format("{0:#,0.00}", printOnly.Amount != null ? printOnly.Amount.Value : 0);
                                lineMemberCounter++;
                            }
                            else
                            {
                                awbDp.OtherCharges = awbDp.OtherCharges + Environment.NewLine + myIATACode + (printOnly.DueTypeCode == "CA" ? "C" : "A") + " " + String.Format("{0:#,0.00}", printOnly.Amount != null ? printOnly.Amount.Value : 0);
                                awbDp.OtherCharges_ChargeEnglishName = awbDp.OtherCharges_ChargeEnglishName + Environment.NewLine + " " + myIATACode + (printOnly.DueTypeCode == "CA" ? "C" : "A") + " " + myIATAName + " " + String.Format("{0:#,0.00}", printOnly.Amount != null ? printOnly.Amount.Value : 0);
                                lineMemberCounter = 1;
                                hasDescription = false;
                            }
                        }
                        lastIsIata = true;
                    }
                    //else
                    //{

                    //    if (lineMemberCounter < 2)
                    //    {
                    //        awbDp.OtherCharges = awbDp.OtherCharges + "  " + printOnly.ChargesType.EnglishName + " " + String.Format("{0:#,0.00}", printOnly.Amount != null ? printOnly.Amount.Value : 0);
                    //        lineMemberCounter++;
                    //    }
                    //    else
                    //    {
                    //        awbDp.OtherCharges = awbDp.OtherCharges + Environment.NewLine + " " + printOnly.ChargesType.EnglishName + " " + String.Format("{0:#,0.00}", printOnly.Amount != null ? printOnly.Amount.Value : 0);
                    //        lineMemberCounter = 1;
                    //    }
                    //    lastIsIata = false;
                    //    hasDescription = true;

                    //}
                }

                if (printOnly.DueTypeCode.ToUpper() == "TX"/*.Equals("TAX",StringComparison.OrdinalIgnoreCase)*/)
                {
                    if (printOnly.PrepaidCollectId == "C")
                    {
                        if (printOnly.Amount != null)
                            taxTotalCollect = taxTotalCollect + printOnly.Amount.Value;
                    }
                    else
                    {
                        if (printOnly.Amount != null)
                            taxTotalPrepaid = taxTotalPrepaid + printOnly.Amount.Value;
                    }
                }

                if (printOnly.DueTypeCode.ToUpper() == "VL")
                {
                    if (printOnly.PrepaidCollectId == "C")
                    {
                        if (printOnly.Amount != null)
                            valuationtotalcollect = valuationtotalcollect + printOnly.Amount.Value;
                    }
                    else
                    {
                        if (printOnly.Amount != null)
                            valuationTotalPrepaid = valuationTotalPrepaid + printOnly.Amount.Value;
                    }
                }

            }
            #endregion
            
            if(shipmentPM.AsAgreedOtherCharges)
            {
                awbDp.OtherCharges_AsAgreed = "As Agreed";
            }
            else
            {
                awbDp.OtherCharges_AsAgreed = awbDp.OtherCharges;
            }

            totalPrepaid = totalOtherChargesPrepaidAgent + totalOtherChargesPrepaidCarrier + (shipmentPM.AWBFreightAmountPrepaid != null ? shipmentPM.AWBFreightAmountPrepaid.Value : 0) + taxTotalPrepaid + valuationTotalPrepaid;
            totalCollect = totalOtherChargesCollectAgent + totalOtherChargesCollectCarrier + (shipmentPM.AWBFreightAmountCollect != null ? shipmentPM.AWBFreightAmountCollect.Value : 0) + taxTotalCollect + valuationtotalcollect;

            double? totalFreight = shipmentPM.AWBChargeAmount == null ? 0 : MethodHelper.Round(shipmentPM.AWBChargeAmount.Value, 2);
            double? freightPrepaid = shipmentPM.AWBFreightAmountPrepaid == null ? 0 : MethodHelper.Round(shipmentPM.AWBFreightAmountPrepaid.Value, 2);
            double? freightCollect = shipmentPM.AWBFreightAmountCollect == null ? 0 : MethodHelper.Round(shipmentPM.AWBFreightAmountCollect.Value, 2);

            if (shipmentPM.AsAgreedFreight)
            {
                awbDp.MAWBRate = " ";

                if (shipmentPM.AsAgreedOtherCharges)
                {
                    #region

                    if (this.HasValue(awbDp.OtherCharges))
                    {
                        awbDp.OtherCharges = "As Agreed";
                    }

                    if (this.HasValue(totalFreight))
                    {
                        awbDp.TotalFreight = "As Agreed";
                    }

                    if (this.HasValue(freightPrepaid))
                    {
                        awbDp.AWBFreightPrepaid = "As Agreed";
                    }

                    if (this.HasValue(freightCollect))
                    {
                        awbDp.AWBFreightCollect = "As Agreed";
                    }

                    double? value_P = totalOtherChargesPrepaidAgent + totalOtherChargesPrepaidCarrier + taxTotalPrepaid + valuationTotalPrepaid;
                    if (value_P != 0 && value_P != null)
                    {
                        awbDp.TotalPrepaid = "As Agreed";
                    }

                    double? value_C = totalOtherChargesCollectAgent + totalOtherChargesCollectCarrier + taxTotalCollect + valuationtotalcollect;
                    if (value_C != 0 && value_C != null)
                    {
                        awbDp.TotalCollect = "As Agreed";
                    }

                    if (taxTotalPrepaid != 0 && taxTotalPrepaid != null)
                    {
                        awbDp.TaxTotalPrepaid = "As Agreed";
                    }

                    if (valuationTotalPrepaid != 0 && valuationTotalPrepaid != null)
                    {
                        awbDp.ValuationTotalPrepaid = "As Agreed";
                    }

                    if (totalOtherChargesPrepaidAgent != 0 && totalOtherChargesPrepaidAgent != null)
                    {
                        awbDp.TotalOtherChargesDueAgentPrepaid = "As Agreed";
                    }

                    if (totalOtherChargesPrepaidCarrier != 0 && totalOtherChargesPrepaidCarrier != null)
                    {
                        awbDp.TotalOtherChargesDueCarrierPrepaid = "As Agreed";
                    }

                    if (taxTotalCollect != 0 && taxTotalCollect != null)
                    {
                        awbDp.TaxTotalCollect = "As Agreed";
                    }

                    if (valuationtotalcollect != 0 && valuationtotalcollect != null)
                    {
                        awbDp.ValuationTotalCollect = "As Agreed";
                    }

                    if (totalOtherChargesCollectAgent != 0 && totalOtherChargesCollectAgent != null)
                    {
                        awbDp.TotalOtherChargesDueAgentCollect = "As Agreed";
                    }

                    if (totalOtherChargesCollectCarrier != 0 && totalOtherChargesCollectCarrier != null)
                    {
                        awbDp.TotalOtherChargesDueCarrierCollect = "As Agreed";
                    }
                    #endregion
                }

                else
                {
                    #region

                    if (freightPrepaid != 0 && freightPrepaid != null)
                    {
                        awbDp.AWBFreightPrepaid = "As Agreed";
                    }

                    if (freightCollect != 0 && freightCollect != null)
                    {
                        awbDp.AWBFreightCollect = "As Agreed";
                    }

                    if (this.HasValue(totalFreight))
                    {
                        awbDp.TotalFreight = "As Agreed";
                    }

                    double? value_P = totalOtherChargesPrepaidAgent + totalOtherChargesPrepaidCarrier + taxTotalPrepaid + valuationTotalPrepaid;
                    double? value_C = totalOtherChargesCollectAgent + totalOtherChargesCollectCarrier + taxTotalCollect + valuationtotalcollect;
                    if (value_P == null)
                    {
                        value_P = 0;
                    }

                    if (value_C == null)
                    {
                        value_C = 0;
                    }

                    awbDp.TotalPrepaid = String.Format("{0:#,0.00}", value_P);
                    awbDp.TotalCollect = String.Format("{0:#,0.00}", value_C);

                    awbDp.TaxTotalPrepaid = String.Format("{0:#,0.00}", taxTotalPrepaid);
                    awbDp.ValuationTotalPrepaid = String.Format("{0:#,0.00}", valuationTotalPrepaid);
                    awbDp.TotalOtherChargesDueAgentPrepaid = String.Format("{0:#,0.00}", totalOtherChargesPrepaidAgent);
                    awbDp.TotalOtherChargesDueCarrierPrepaid = String.Format("{0:#,0.00}", totalOtherChargesPrepaidCarrier);

                    awbDp.TaxTotalCollect = String.Format("{0:#,0.00}", taxTotalCollect);
                    awbDp.ValuationTotalCollect = String.Format("{0:#,0.00}", valuationtotalcollect);
                    awbDp.TotalOtherChargesDueAgentCollect = String.Format("{0:#,0.00}", totalOtherChargesCollectAgent);
                    awbDp.TotalOtherChargesDueCarrierCollect = String.Format("{0:#,0.00}", totalOtherChargesCollectCarrier);
                    #endregion
                }

                if (shipmentPM.AWBChargeAmount == null || shipmentPM.AWBChargeAmount == 0)
                {
                    awbDp.MAWBRate = "As Agreed";
                    awbDp.TotalFreight = "As Agreed";

                    if (shipmentPM.FreightPrepaidCollectId == "P")
                    {
                        awbDp.AWBFreightPrepaid = "As Agreed";
                    }

                    else
                    {
                        awbDp.AWBFreightCollect = "As Agreed";
                    }
                }
            }

            else
            {
                if (shipmentPM.AsAgreedOtherCharges)
                {
                    #region
                    if (!string.IsNullOrEmpty(awbDp.OtherCharges))
                    {
                        awbDp.OtherCharges = "As Agreed";
                    }

                    if (taxTotalPrepaid != 0 && taxTotalPrepaid != null)
                    {
                        awbDp.TaxTotalPrepaid = "As Agreed";
                    }

                    if (valuationTotalPrepaid != 0 && valuationTotalPrepaid != null)
                    {
                        awbDp.ValuationTotalPrepaid = "As Agreed";
                    }

                    if (totalOtherChargesPrepaidAgent != 0 && totalOtherChargesPrepaidAgent != null)
                    {
                        awbDp.TotalOtherChargesDueAgentPrepaid = "As Agreed";
                    }

                    if (totalOtherChargesPrepaidCarrier != 0 && totalOtherChargesPrepaidCarrier != null)
                    {
                        awbDp.TotalOtherChargesDueCarrierPrepaid = "As Agreed";
                    }

                    if (taxTotalCollect != 0 && taxTotalCollect != null)
                    {
                        awbDp.TaxTotalCollect = "As Agreed";
                    }

                    if (valuationtotalcollect != 0 && valuationtotalcollect != null)
                    {
                        awbDp.ValuationTotalCollect = "As Agreed";
                    }

                    if (totalOtherChargesCollectAgent != 0 && totalOtherChargesCollectAgent != null)
                    {
                        awbDp.TotalOtherChargesDueAgentCollect = "As Agreed";
                    }

                    if (totalOtherChargesCollectCarrier != 0 && totalOtherChargesCollectCarrier != null)
                    {
                        awbDp.TotalOtherChargesDueCarrierCollect = "As Agreed";
                    }

                    awbDp.TotalFreight = String.Format("{0:#,0.00}", totalFreight);
                    awbDp.MAWBRate = String.Format("{0:#,##0.00}", shipmentPM.AWBChargeRate);
                    awbDp.AWBFreightPrepaid = String.Format("{0:#,0.00}", freightPrepaid);
                    awbDp.AWBFreightCollect = String.Format("{0:#,0.00}", freightCollect);
                    awbDp.TotalPrepaid = String.Format("{0:#,0.00}", freightPrepaid);
                    awbDp.TotalCollect = String.Format("{0:#,0.00}", freightCollect);
                    #endregion
                }

                else
                {
                    #region
                    awbDp.TotalFreight = String.Format("{0:#,0.00}", totalFreight);
                    awbDp.MAWBRate = String.Format("{0:#,##0.00}", shipmentPM.AWBChargeRate);
                    awbDp.TotalPrepaid = String.Format("{0:#,0.00}", totalPrepaid);
                    awbDp.AWBFreightPrepaid = String.Format("{0:#,0.00}", freightPrepaid);
                    awbDp.TaxTotalPrepaid = String.Format("{0:#,0.00}", taxTotalPrepaid);
                    awbDp.ValuationTotalPrepaid = String.Format("{0:#,0.00}", valuationTotalPrepaid);
                    awbDp.TotalOtherChargesDueAgentPrepaid = String.Format("{0:#,0.00}", totalOtherChargesPrepaidAgent);
                    awbDp.TotalOtherChargesDueCarrierPrepaid = String.Format("{0:#,0.00}", totalOtherChargesPrepaidCarrier);

                    awbDp.TotalCollect = String.Format("{0:#,0.00}", totalCollect);
                    awbDp.AWBFreightCollect = String.Format("{0:#,0.00}", freightCollect);
                    awbDp.TaxTotalCollect = String.Format("{0:#,0.00}", taxTotalCollect);
                    awbDp.ValuationTotalCollect = String.Format("{0:#,0.00}", valuationtotalcollect);
                    awbDp.TotalOtherChargesDueAgentCollect = String.Format("{0:#,0.00}", totalOtherChargesCollectAgent);
                    awbDp.TotalOtherChargesDueCarrierCollect = String.Format("{0:#,0.00}", totalOtherChargesCollectCarrier);
                    #endregion
                }
            }
        }

        private void GetHandlingAndAccountingData(AWBDataProvider myDataProvider, ShipmentPM shipmentPM, AddressRepository addressRepository)
        {           
            string myNotifyData = "";
            string myHandlingInformation = "";
            string myAccountingInformation = "";
            
            List<string> myHandlingInformationList = new List<string>();

            if (!string.IsNullOrEmpty(shipmentPM.AWBHandlingInformation))
            {
                string[] lines = shipmentPM.AWBHandlingInformation.Split('\r');

                foreach (string item in lines)
                {
                    if (!string.IsNullOrEmpty(item))
                    {
                        myHandlingInformationList.Add(item);
                    }
                }
            }

            #region AccountingInformation

            if (!string.IsNullOrEmpty(shipmentPM.AWBAccountingInformation))
            {
                myAccountingInformation = shipmentPM.AWBAccountingInformation;
            }

            else
            {
                if (!string.IsNullOrEmpty(shipmentPM.AccountingInformationIdentifierCode1) && !string.IsNullOrEmpty(shipmentPM.AccountingInformation1))
                {
                    myAccountingInformation = shipmentPM.AccountingInformationIdentifierCode1 + " " + shipmentPM.AccountingInformation1;
                }

                if (!string.IsNullOrEmpty(shipmentPM.AccountingInformationIdentifierCode2) && !string.IsNullOrEmpty(shipmentPM.AccountingInformation2))
                {
                    if (!string.IsNullOrEmpty(myAccountingInformation))
                    {
                        myAccountingInformation = myAccountingInformation + "/ ";
                    }

                    myAccountingInformation = myAccountingInformation + shipmentPM.AccountingInformationIdentifierCode2 + " " + shipmentPM.AccountingInformation2;
                }

                if (!string.IsNullOrEmpty(shipmentPM.AccountingInformationIdentifierCode3) && !string.IsNullOrEmpty(shipmentPM.AccountingInformation3))
                {
                    if (!string.IsNullOrEmpty(myAccountingInformation))
                    {
                        myAccountingInformation = myAccountingInformation + "/ ";
                    }

                    myAccountingInformation = myAccountingInformation + shipmentPM.AccountingInformationIdentifierCode3 + " " + shipmentPM.AccountingInformation3;
                }

                if (!string.IsNullOrEmpty(shipmentPM.AccountingInformationIdentifierCode4) && !string.IsNullOrEmpty(shipmentPM.AccountingInformation4))
                {
                    if (!string.IsNullOrEmpty(myAccountingInformation))
                    {
                        myAccountingInformation = myAccountingInformation + "/ ";
                    }

                    myAccountingInformation = myAccountingInformation + shipmentPM.AccountingInformationIdentifierCode4 + " " + shipmentPM.AccountingInformation4;
                }

                if (!string.IsNullOrEmpty(shipmentPM.AccountingInformationIdentifierCode5) && !string.IsNullOrEmpty(shipmentPM.AccountingInformation5))
                {
                    if (!string.IsNullOrEmpty(myAccountingInformation))
                    {
                        myAccountingInformation = myAccountingInformation + "/ ";
                    }

                    myAccountingInformation = myAccountingInformation + shipmentPM.AccountingInformationIdentifierCode5 + " " + shipmentPM.AccountingInformation5;
                }

                if (!string.IsNullOrEmpty(shipmentPM.AccountingInformationIdentifierCode6) && !string.IsNullOrEmpty(shipmentPM.AccountingInformation6))
                {
                    if (!string.IsNullOrEmpty(myAccountingInformation))
                    {
                        myAccountingInformation = myAccountingInformation + " / ";
                    }

                    myAccountingInformation = myAccountingInformation + shipmentPM.AccountingInformationIdentifierCode6 + " " + shipmentPM.AccountingInformation6;
                }
            }

            #endregion

            #region Notify
            if (!string.IsNullOrEmpty(shipmentPM.Notify1Id))
            {                
                CardRepository cardRepository = new CardRepository(myTenant);
                Card card = cardRepository.GetSingleCard(shipmentPM.Notify1Id, myTenant);
               
                if (card != null)
                {
                    myNotifyData = "Notify:" + card.EnglishName;
                   
                }

                if (!string.IsNullOrEmpty(shipmentPM.Notify1AddressId))
                {
                    Address notify1Address = addressRepository.GetSingleAddress(shipmentPM.Notify1AddressId, myTenant);

                    if (notify1Address != null)
                    {
                        if (notify1Address.IsLocalLanguage)
                        {
                            myNotifyData = "Notify:" + card.LocalName;
                        }

                        if (!string.IsNullOrEmpty(notify1Address.Address1))
                        {
                            myNotifyData += " " + notify1Address.Address1;
                        }

                        if (!string.IsNullOrEmpty(notify1Address.Address2))
                        {
                            myNotifyData += " " + notify1Address.Address2;
                        }

                        if (!string.IsNullOrEmpty(notify1Address.ZipCode))
                        {
                            myNotifyData += " " + notify1Address.ZipCode;
                        }

                        if (notify1Address.Country != null)
                        {
                            myNotifyData += " " + notify1Address.Country.Code;

                            if (notify1Address.IsLocalLanguage)
                            {
                                myNotifyData += " " + notify1Address.Country.LocalName;
                            }

                            else
                            {
                                myNotifyData += " " + notify1Address.Country.EnglishName;
                            }
                        }

                        if (!string.IsNullOrEmpty(notify1Address.PhoneNumber))
                        {
                            myNotifyData += " Tel: " + notify1Address.PhoneNumber;
                        }

                        if (!string.IsNullOrEmpty(notify1Address.FaxNumber))
                        {
                            myNotifyData += " Fax: " + notify1Address.FaxNumber;
                        }
                    }

                    //if (myHandlingInformationList.Count < 3)
                    //{
                        myHandlingInformationList.Add(myNotifyData);
                    //}

                    //else
                    //{
                    //    if (string.IsNullOrEmpty(myAccountingInformation))
                    //    {
                    //        myAccountingInformation = myNotifyData;
                    //    }

                    //    else
                    //    {
                    //        myAccountingInformation += Environment.NewLine + myNotifyData;
                    //    }
                    //}
                }
            }
            #endregion

            #region RAR-RA
            if (isRegulatedAgentActivated)
            {
                //if (myHandlingInformationList.Count < 3)
                //{
                    if (!string.IsNullOrEmpty(shipmentPM.AWBPrintingRANumber))
                    {
                        string myField = "RAR-RA" + shipmentPM.AWBPrintingRANumber;
                        myHandlingInformationList.Add(myField);
                    }
                //}
            }
            #endregion

            if (myHandlingInformationList.Count > 0)
            {
                foreach (string item in myHandlingInformationList)
                {
                    if (string.IsNullOrEmpty(myHandlingInformation))
                    {
                        myHandlingInformation = item;
                    }

                    else
                    {
                        myHandlingInformation += Environment.NewLine + item;
                    }
                }
            }

            #region 1
            string myCodesText = "";

            List<string> mySpecialHandlingCodes = this.GetSpecialHandlingCodes(shipmentPM);
            if (mySpecialHandlingCodes.Count > 0)
            {
                foreach (string item in mySpecialHandlingCodes)
                {
                    if (string.IsNullOrEmpty(myCodesText))
                    {
                        myCodesText = item;
                    }

                    else
                    {
                        myCodesText += " " + item;
                    }
                }
            }
            #endregion

            #region 2
            //string myReferenceText = "";

            //string myReferenceField = "";
            //myReferenceField = shipmentPM.ReferenceNumber;
            //if (!string.IsNullOrEmpty(myReferenceField))
            //{
            //    if (string.IsNullOrEmpty(myReferenceText))
            //    {
            //        myReferenceText = myReferenceField;
            //    }

            //    else
            //    {
            //        myReferenceText += " " + myReferenceField;
            //    }
            //}

            //myReferenceField = shipmentPM.SupplementaryShipmentInformation1;
            //if (!string.IsNullOrEmpty(myReferenceField))
            //{
            //    if (string.IsNullOrEmpty(myReferenceText))
            //    {
            //        myReferenceText = myReferenceField;
            //    }

            //    else
            //    {
            //        myReferenceText += " " + myReferenceField;
            //    }
            //}

            //myReferenceField = shipmentPM.SupplementaryShipmentInformation2;
            //if (!string.IsNullOrEmpty(myReferenceField))
            //{
            //    if (string.IsNullOrEmpty(myReferenceText))
            //    {
            //        myReferenceText = myReferenceField;
            //    }

            //    else
            //    {
            //        myReferenceText += " " + myReferenceField;
            //    }
            //}
            #endregion

            #region 3
            string myOCIsText = "";
            AWBOCIQuery myOCIQuery = new AWBOCIQuery(shipmentPM.Tenant);
            List<AWBOCIPM> myOCIList = myOCIQuery.GetAWBOCIPMsByShipmentId(shipmentPM.Id, shipmentPM.Tenant).ToList();

            if (myOCIList.Count > 0)
            {
                foreach (AWBOCIPM item in myOCIList)
                {
                    string myField = "";

                    if (!string.IsNullOrEmpty(item.CountryId))
                    {
                        Country myCountry = CountryRepository.GetSingleCountry(item.CountryId, item.Tenant, true);
                        if (myCountry != null)
                        {
                            myField = string.IsNullOrEmpty(myField) ? myCountry.Code : (myField + " " + myCountry.Code);
                        }
                    }

                    if (!string.IsNullOrEmpty(item.AWBInformationCode))
                    {
                        myField = string.IsNullOrEmpty(myField) ? item.AWBInformationCode : (myField + " " + item.AWBInformationCode);
                    }

                    if (!string.IsNullOrEmpty(item.AWBCustomsInformationCode))
                    {
                        myField = string.IsNullOrEmpty(myField) ? item.AWBCustomsInformationCode : (myField + " " + item.AWBCustomsInformationCode);
                    }

                    if (!string.IsNullOrEmpty(item.SupplementaryCustomsInfo))
                    {
                        myField = string.IsNullOrEmpty(myField) ? item.SupplementaryCustomsInfo : (myField + " " + item.SupplementaryCustomsInfo);
                    }

                    if (!string.IsNullOrEmpty(myField))
                    {
                        myOCIsText = string.IsNullOrEmpty(myOCIsText) ? myField : (myOCIsText + " / " + myField);
                    }
                }
            }
            #endregion

            string myAddedLine = "";
            if (!string.IsNullOrEmpty(myCodesText))
            {
                myAddedLine = string.IsNullOrEmpty(myAddedLine) ? myCodesText : (myAddedLine + " / " + myCodesText);
            }

            if (!string.IsNullOrEmpty(myOCIsText))
            {
                myAddedLine = string.IsNullOrEmpty(myAddedLine) ? myOCIsText : (myAddedLine + " / " + myOCIsText);
            }

            if (!string.IsNullOrEmpty(myAddedLine))
            {
                if (string.IsNullOrEmpty(myHandlingInformation))
                {
                    myHandlingInformation = myAddedLine;
                }

                else
                {
                    myHandlingInformation += Environment.NewLine + myAddedLine;
                }
            }

            //if (!string.IsNullOrEmpty(shipmentPM.MainHarmonize))
            //{
            //    string myMainHarmonize = "HCC Code " + shipmentPM.MainHarmonize;

            //    if (string.IsNullOrEmpty(myHandlingInformation))
            //    {
            //        myHandlingInformation = myMainHarmonize;
            //    }

            //    else
            //    {
            //        myHandlingInformation += Environment.NewLine + myMainHarmonize;
            //    }
            //}

            List<string> myHandlingInformationList0_3 = new List<string>();
            List<string> myHandlingInformationList3_X = new List<string>();

            if (!string.IsNullOrEmpty(myHandlingInformation))
            {
                string[] lines = myHandlingInformation.Split('\r');

                foreach (string item in lines)
                {
                    if (myHandlingInformationList0_3.Count < 3)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            myHandlingInformationList0_3.Add(item);
                        }
                    }

                    else
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            myHandlingInformationList3_X.Add(item);
                        }
                    }
                }
            }

            foreach (string item in myHandlingInformationList0_3)
            {
                myDataProvider.HandlingInformation += item;
            }
            
            if (myHandlingInformationList3_X.Count > 0)
            {
                string str = "";
                foreach (string item in myHandlingInformationList3_X)
                {
                    str += item;
                }

                myDataProvider.AccountingInformation = myAccountingInformation + Environment.NewLine + str;
            }

            else
            {
                myDataProvider.AccountingInformation = myAccountingInformation;
            }

            myDataProvider.HandlingInformationOnly = shipmentPM.AWBHandlingInformation;
            myDataProvider.ReferenceNumber = string.IsNullOrEmpty(shipmentPM.ReferenceNumber) ? "" : shipmentPM.ReferenceNumber;
            myDataProvider.SupplementaryInformation1 = string.IsNullOrEmpty(shipmentPM.SupplementaryShipmentInformation1) ? "" : shipmentPM.SupplementaryShipmentInformation1;
            myDataProvider.SupplementaryInformation2 = string.IsNullOrEmpty(shipmentPM.SupplementaryShipmentInformation2) ? "" : shipmentPM.SupplementaryShipmentInformation2;
        }

        private List<string> GetSpecialHandlingCodes(ShipmentPM entityPM)
        {
            List<string> myResult = new List<string>();

            string myFieldId = null;
            AWBSpecialHandlingCodeRepository myRepository = new AWBSpecialHandlingCodeRepository(entityPM.Tenant);
            List<AWBSpecialHandlingCode> list = myRepository.GetAWBHandlingCodes().ToList();

            myFieldId = entityPM.AWBSpecialHandlingCodeId1;
            if (!string.IsNullOrEmpty(myFieldId))
            {
                AWBSpecialHandlingCode item = list.Where(d => d.Id == myFieldId).FirstOrDefault();
                if (item != null)
                {
                    myResult.Add(item.Code);
                }
            }

            myFieldId = entityPM.AWBSpecialHandlingCodeId2;
            if (!string.IsNullOrEmpty(myFieldId))
            {
                AWBSpecialHandlingCode item = list.Where(d => d.Id == myFieldId).FirstOrDefault();
                if (item != null)
                {
                    myResult.Add(item.Code);
                }
            }

            myFieldId = entityPM.AWBSpecialHandlingCodeId3;
            if (!string.IsNullOrEmpty(myFieldId))
            {
                AWBSpecialHandlingCode item = list.Where(d => d.Id == myFieldId).FirstOrDefault();
                if (item != null)
                {
                    myResult.Add(item.Code);
                }
            }

            myFieldId = entityPM.AWBSpecialHandlingCodeId4;
            if (!string.IsNullOrEmpty(myFieldId))
            {
                AWBSpecialHandlingCode item = list.Where(d => d.Id == myFieldId).FirstOrDefault();
                if (item != null)
                {
                    myResult.Add(item.Code);
                }
            }

            myFieldId = entityPM.AWBSpecialHandlingCodeId5;
            if (!string.IsNullOrEmpty(myFieldId))
            {
                AWBSpecialHandlingCode item = list.Where(d => d.Id == myFieldId).FirstOrDefault();
                if (item != null)
                {
                    myResult.Add(item.Code);
                }
            }

            myFieldId = entityPM.AWBSpecialHandlingCodeId6;
            if (!string.IsNullOrEmpty(myFieldId))
            {
                AWBSpecialHandlingCode item = list.Where(d => d.Id == myFieldId).FirstOrDefault();
                if (item != null)
                {
                    myResult.Add(item.Code);
                }
            }

            myFieldId = entityPM.AWBSpecialHandlingCodeId7;
            if (!string.IsNullOrEmpty(myFieldId))
            {
                AWBSpecialHandlingCode item = list.Where(d => d.Id == myFieldId).FirstOrDefault();
                if (item != null)
                {
                    myResult.Add(item.Code);
                }
            }

            myFieldId = entityPM.AWBSpecialHandlingCodeId8;
            if (!string.IsNullOrEmpty(myFieldId))
            {
                AWBSpecialHandlingCode item = list.Where(d => d.Id == myFieldId).FirstOrDefault();
                if (item != null)
                {
                    myResult.Add(item.Code);
                }
            }

            myFieldId = entityPM.AWBSpecialHandlingCodeId9;
            if (!string.IsNullOrEmpty(myFieldId))
            {
                AWBSpecialHandlingCode item = list.Where(d => d.Id == myFieldId).FirstOrDefault();
                if (item != null)
                {
                    myResult.Add(item.Code);
                }
            }

            return myResult;
        }

        private void GetCopyNameData(AWBDataProvider awbDp, string documentTypeCopyId, int tenant)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
            DocumentTypeCopy documenttypecopy = (from copy in commonContext.DocumentTypeCopies where copy.Id == documentTypeCopyId select copy).FirstOrDefault();

            if (documenttypecopy != null)
            {
                awbDp.CopyName = documenttypecopy.Name != null ? documenttypecopy.Name.ToUpper() : "";

                switch (documenttypecopy.Code)
                {
                    case "740S":
                    case "740IS":
                    case "740C":
                        awbDp.HasBackPaper = true;
                        break;
                    case "740A":
                    case "740D":
                    case "740X":
                        awbDp.HasBackPaper = false;
                        break;
                }
            }

            else
            {
                awbDp.CopyName = "Full Set";//"ELECTRONIC COPY";
            }
        }

        private void GetCommoditiesData(AWBDataProvider awbDp, ShipmentPM shipmentPM)
        {
            int tenant = shipmentPM.Tenant;

            ShipmentPackageRepository packagesRepository = new ShipmentPackageRepository(tenant);
            ShipmentCommodityRepository commoditiesRepository = new ShipmentCommodityRepository(tenant);

            string volumeUnitCode = string.IsNullOrEmpty(shipmentPM.VolumeUnitCode) ? "" : shipmentPM.VolumeUnitCode.ToUpper();
            string grossWeightUnitCode = string.IsNullOrEmpty(shipmentPM.GrossWeightUnitCode) ? "" : shipmentPM.GrossWeightUnitCode.ToUpper();
            string chargeableWeightUnitCode = string.IsNullOrEmpty(shipmentPM.ChargeableWeightUnitCode) ? "" : shipmentPM.ChargeableWeightUnitCode.ToUpper();

            awbDp.WeightUnit = grossWeightUnitCode;

            awbDp.CommoditiesLinesList = new List<CommodityLine>();

            #region MultipleCommodities
            if (shipmentPM.IsMultipleCommodities)
            {                
                List<ShipmentCommodity> commodities = commoditiesRepository.GetCommoditiesbyShipmentId(shipmentPM.Id, tenant).ToList();
                
                foreach (ShipmentCommodity shipmentCommodity in commodities)
                {
                    CommodityLine commodityLine = new CommodityLine();

                    double? totalFreight = shipmentCommodity.ChargeAmount == null ? 0 : MethodHelper.Round(shipmentCommodity.ChargeAmount.Value, 2);
                    double? myChargeRate = shipmentCommodity.ChargeRate == null ? 0 : MethodHelper.Round(shipmentCommodity.ChargeRate.Value, 2);

                    if (shipmentPM.AsAgreedFreight)
                    {
                        commodityLine.MAWBRate = " ";

                        if (this.HasValue(totalFreight))
                        {
                            awbDp.TotalFreight = "As Agreed";
                        }

                        if (shipmentCommodity.ChargeAmount == null || shipmentCommodity.ChargeAmount == 0)
                        {
                            commodityLine.MAWBRate = "As Agreed";
                            commodityLine.TotalFreight = "As Agreed";
                        }
                    }

                    else
                    {
                        if (shipmentPM.AsAgreedOtherCharges)
                        {
                            commodityLine.TotalFreight = String.Format("{0:#,0.00}", totalFreight);
                            commodityLine.MAWBRate = String.Format("{0:#,##0.00}", myChargeRate);
                        }

                        else
                        {
                            commodityLine.TotalFreight = String.Format("{0:#,0.00}", totalFreight);
                            commodityLine.MAWBRate = String.Format("{0:#,##0.00}", myChargeRate);
                        }
                    }

                    commodityLine.RateClassCode = shipmentCommodity.RateClassCode;
                    commodityLine.CommodityNumber = shipmentCommodity.CommodityNumber;

                    if (shipmentCommodity.GrossWeight != null)
                    {
                        commodityLine.GrossWeight = String.Format("{0:#,0.00}", shipmentCommodity.GrossWeight.Value);

                        double? varGrossWeightInKG = ShipmentMapping.GetWeightInKG("KG", shipmentCommodity.GrossWeight);

                        if (varGrossWeightInKG != null)
                        {
                            commodityLine.GrossWeightInKG = String.Format("{0:#,0.00}", varGrossWeightInKG.Value);
                        }
                    }
                   
                    if (shipmentCommodity.ChargeableWeight != null)
                    {
                        commodityLine.ChargeableWeight = String.Format("{0:#,0.00}", shipmentCommodity.ChargeableWeight.Value);
                    }

                    if (shipmentCommodity.NumberOfPackages != null)
                    {
                        commodityLine.TotalQuantity = shipmentCommodity.NumberOfPackages.ToString();
                    }

                    List<ShipmentPackage> commodityPackages = packagesRepository.GetShipmentPackagesByCommodityId(shipmentPM.Id, shipmentCommodity.Id, tenant).ToList();
                    if (commodityPackages.Count > 0)
                    {
                        string dimentions = "DIM: ";
                        string volume = "AS VOL ";

                        foreach (ShipmentPackage package in commodityPackages)
                        {
                            if (package.Height != null && package.Length != null && package.Width != null)
                            {
                                dimentions = dimentions + package.Quantity + "(" + package.Length.ToString() + "X" + package.Width.ToString() + "X" + package.Height.ToString() + ")" + shipmentPM.DimensionsUnitCode.ToUpper() + "S" + Environment.NewLine;
                            }
                        }

                        if (dimentions == "DIM: ")
                        {
                            dimentions = "";
                        }

                        commodityLine.DescriptionOfGoods = commodityLine.DescriptionOfGoods + Environment.NewLine;
                        commodityLine.DescriptionOfGoods = commodityLine.DescriptionOfGoods + dimentions;

                        if(!string.IsNullOrEmpty(grossWeightUnitCode))
                        {
                            commodityLine.DescriptionOfGoods = commodityLine.DescriptionOfGoods + volume + " " + shipmentCommodity.VolumetricWeight + " " + chargeableWeightUnitCode + "S" + " " + shipmentCommodity.Volume + " " + volumeUnitCode;
                        }

                        if (!string.IsNullOrEmpty(shipmentPM.ShipperReference1))
                        {
                            commodityLine.DescriptionOfGoods = commodityLine.DescriptionOfGoods + Environment.NewLine + "Inv.No:" + shipmentPM.ShipperReference1;
                        }
                    }

                    awbDp.CommoditiesLinesList.Add(commodityLine);
                }
            }
            #endregion

            #region SingleCommodity
            else
            {
                CommodityLine commodityLine = new CommodityLine();

                double? totalFreight = shipmentPM.AWBChargeAmount == null ? 0 : MethodHelper.Round(shipmentPM.AWBChargeAmount.Value, 2);
                double? myChargeRate = shipmentPM.AWBChargeRate == null ? 0 : MethodHelper.Round(shipmentPM.AWBChargeRate.Value, 2);

                if (shipmentPM.AsAgreedFreight)
                {
                    commodityLine.MAWBRate = " ";

                    if (this.HasValue(totalFreight))
                    {
                        awbDp.TotalFreight = "As Agreed";
                    }

                    if (shipmentPM.AWBChargeAmount == null || shipmentPM.AWBChargeAmount == 0)
                    {
                        commodityLine.MAWBRate = "As Agreed";
                        commodityLine.TotalFreight = "As Agreed";
                    }
                }

                else
                {
                    if (shipmentPM.AsAgreedOtherCharges)
                    {
                        commodityLine.TotalFreight = String.Format("{0:#,0.00}", totalFreight);
                        commodityLine.MAWBRate = String.Format("{0:#,##0.00}", myChargeRate);
                    }

                    else
                    {
                        commodityLine.TotalFreight = String.Format("{0:#,0.00}", totalFreight);
                        commodityLine.MAWBRate = String.Format("{0:#,##0.00}", myChargeRate);
                    }
                }

                awbDp.RateTypeCode = shipmentPM.RateClassCode;
                commodityLine.RateClassCode = shipmentPM.RateClassCode;

                awbDp.AWBCommodityItemNumber = shipmentPM.AWBCommodityItemNumber;
                commodityLine.CommodityNumber = shipmentPM.AWBCommodityItemNumber;

                if (shipmentPM.GrossWeight != null)
                {
                    awbDp.GrossWeight = String.Format("{0:#,0.00}", shipmentPM.GrossWeight.Value);
                    commodityLine.GrossWeight = String.Format("{0:#,0.00}", shipmentPM.GrossWeight.Value);
                }

                if (shipmentPM.GrossWeightInKG != null)
                {
                    awbDp.GrossWeightInKG = String.Format("{0:#,0.00}", shipmentPM.GrossWeightInKG.Value);
                    commodityLine.GrossWeightInKG = String.Format("{0:#,0.00}", shipmentPM.GrossWeightInKG.Value);
                }

                if (shipmentPM.ChargeableWeight != null)
                {
                    awbDp.ChargeableWeight = String.Format("{0:#,0.00}", shipmentPM.ChargeableWeight.Value);
                    commodityLine.ChargeableWeight = String.Format("{0:#,0.00}", shipmentPM.ChargeableWeight.Value);
                }

                if (shipmentPM.NumberOfPackages != null)
                {
                    awbDp.TotalQuantity = shipmentPM.NumberOfPackages.ToString();
                    commodityLine.TotalQuantity = shipmentPM.NumberOfPackages.ToString();
                }

                List<ShipmentPackage> packages = packagesRepository.GetShipmentPackagesForShipmentTenant(shipmentPM.Id, tenant).ToList();

                if (packages.Count > 0)
                {
                    string dimentions = "";
                    string dimentionsPrefix = "DIM: ";
                    string dimentionsValue = "";
                    foreach (ShipmentPackage package in packages)
                    {
                        if (package.Height != null && package.Length != null && package.Width != null)
                        {
                            if (!string.IsNullOrEmpty(dimentionsValue))
                            {
                                dimentionsValue += Environment.NewLine;
                            }

                            dimentionsValue += package.Quantity + "(" + package.Length.ToString() + "X" + package.Width.ToString() + "X" + package.Height.ToString() + ")" + shipmentPM.DimensionsUnitCode.ToUpper() + "S";
                        }
                    }

                    if (!string.IsNullOrEmpty(dimentionsValue))
                    {
                        dimentions = dimentionsPrefix + dimentionsValue;
                    }

                    awbDp.VolumetricWeight = shipmentPM.VolumetricWeight;
                    awbDp.Volume = shipmentPM.Volume;
                    awbDp.Dimension = dimentions;

                    string myDescriptionOfGoods = shipmentPM.DescriptionOfGoods == null ? "" : shipmentPM.DescriptionOfGoods;
                    awbDp.JustDescriptionofGoods = myDescriptionOfGoods;

                    if (!string.IsNullOrEmpty(dimentions))
                    {
                        if (!string.IsNullOrEmpty(myDescriptionOfGoods))
                        {
                            myDescriptionOfGoods += Environment.NewLine;
                        }

                        myDescriptionOfGoods += dimentions;
                    }

                    if (shipmentPM.GrossWeightUnitCode != null)
                    {
                        string volume = "AS VOL " + " " + shipmentPM.VolumetricWeight + " " + chargeableWeightUnitCode.ToUpper() + "S" + " " + shipmentPM.Volume + " " + volumeUnitCode.ToUpper();

                        if (!string.IsNullOrEmpty(myDescriptionOfGoods))
                        {
                            myDescriptionOfGoods += Environment.NewLine;
                        }

                        myDescriptionOfGoods += volume;
                    }

                    if (!string.IsNullOrEmpty(shipmentPM.ShipperReference1))
                    {
                        if (!string.IsNullOrEmpty(myDescriptionOfGoods))
                        {
                            myDescriptionOfGoods += Environment.NewLine;
                        }

                        myDescriptionOfGoods += "Inv.No:" + shipmentPM.ShipperReference1;
                    }

                    if (!string.IsNullOrEmpty(shipmentPM.MainHarmonize))
                    {
                        string myMainHarmonize = "HCC Code " + shipmentPM.MainHarmonize;

                        if (!string.IsNullOrEmpty(myDescriptionOfGoods))
                        {
                            myDescriptionOfGoods += Environment.NewLine;
                        }

                        myDescriptionOfGoods += myMainHarmonize;
                    }

                    awbDp.DescriptionOfGoods = myDescriptionOfGoods;
                    commodityLine.DescriptionOfGoods = myDescriptionOfGoods;
                }

                awbDp.CommoditiesLinesList.Add(commodityLine);
            }
            #endregion
        }

        private bool HasValue(object value)
        {
            bool hasValue = false;

            if (value != null)
            {
                string myString = value.ToString();

                if (!string.IsNullOrEmpty(myString) && myString != "0")
                {
                    hasValue = true;
                }
            }

            return hasValue;
        }

        private string GetSCI(Port destinationport, Port departureport)
        {
            string sci="";
            if (destinationport != null)
            {
                if (destinationport.Country.EC)
                {
                    if (departureport != null)
                    {
                        if (departureport.Country.EC)
                        {
                            sci = "C";
                        }

                        else
                        {
                            sci = "X";
                        }
                    }
                }
            }
            return sci; 
        }

        private string GetAddress(Address address)
        {
            string resultAddress = "";

            resultAddress = address.Address1 != null ? address.Address1 : "";
            if (!string.IsNullOrEmpty(address.Address2))
            {
                resultAddress = resultAddress + Environment.NewLine + address.Address2;
            }
            if (!string.IsNullOrEmpty(address.City))
            {
                resultAddress = resultAddress + Environment.NewLine + address.City;
            }
            if (address.State != null)
            {
                resultAddress = resultAddress + " " + (address.State.EnglishName != null ? address.State.EnglishName : "");
            }
            if (!string.IsNullOrEmpty(address.ZipCode))
            {
                resultAddress = resultAddress + " " + address.ZipCode;
            }
            if (address.Country != null)
            {
                resultAddress = resultAddress + " " + address.Country.Code;
            }

            return resultAddress;
        }

        private void GetCustomFieldsData(AWBDataProvider awbDp, ShipmentPM shipmentPM)
        {
            CustomFieldResolver customFieldResolver = new CustomFieldResolver();

            List<ObjectField> customFields = ObjectFieldRepository.GetCustomObjectFieldsByObjectTableName("Shipment", shipmentPM.Tenant).ToList();
                        
            foreach (ObjectField field in customFields)
            {
                object value = customFieldResolver.GetFieldValue(shipmentPM, field, shipmentPM.Tenant);

                if (value != null)
                {
                    if (field.FieldName == "Field1")
                    {
                        awbDp.ShipmentField1 = value.ToString();
                    }
                    else if (field.FieldName == "Field2")
                    {
                        awbDp.ShipmentField2 = value.ToString();
                    }
                    else if (field.FieldName == "Field3")
                    {
                        awbDp.ShipmentField3 = value.ToString();
                    }
                    else if (field.FieldName == "Field4")
                    {
                        awbDp.ShipmentField4 = value.ToString();
                    }
                    else if (field.FieldName == "Field5")
                    {
                        awbDp.ShipmentField5 = value.ToString();
                    }
                    else if (field.FieldName == "Field6")
                    {
                        awbDp.ShipmentField6 = value.ToString();
                    }
                    else if (field.FieldName == "Field7")
                    {
                        awbDp.ShipmentField7 = value.ToString();
                    }
                    else if (field.FieldName == "Field8")
                    {
                        awbDp.ShipmentField8 = value.ToString();
                    }
                    else if (field.FieldName == "Field9")
                    {
                        awbDp.ShipmentField9 = value.ToString();
                    }
                    else if (field.FieldName == "Field10")
                    {
                        awbDp.ShipmentField10 = value.ToString();
                    }
					else if (field.FieldName == "Field11")
					{
						awbDp.ShipmentField11 = value.ToString();
					}
					else if (field.FieldName == "Field12")
					{
						awbDp.ShipmentField12 = value.ToString();
					}
					else if (field.FieldName == "Field13")
					{
						awbDp.ShipmentField13 = value.ToString();
					}
					else if (field.FieldName == "Field14")
					{
						awbDp.ShipmentField14 = value.ToString();
					}
					else if (field.FieldName == "Field15")
					{
						awbDp.ShipmentField15 = value.ToString();
					}
					else if (field.FieldName == "Field16")
					{
						awbDp.ShipmentField16 = value.ToString();
					}
					else if (field.FieldName == "Field17")
					{
						awbDp.ShipmentField17 = value.ToString();
					}
					else if (field.FieldName == "Field18")
					{
						awbDp.ShipmentField18 = value.ToString();
					}
					else if (field.FieldName == "Field19")
					{
						awbDp.ShipmentField19 = value.ToString();
					}
					else if (field.FieldName == "Field20")
					{
						awbDp.ShipmentField20 = value.ToString();
					}
                    else if (field.FieldName == "Field21")
                    {
                        awbDp.ShipmentField21 = value.ToString();
                    }
                    else if (field.FieldName == "Field22")
                    {
                        awbDp.ShipmentField22 = value.ToString();
                    }
                    else if (field.FieldName == "Field23")
                    {
                        awbDp.ShipmentField23 = value.ToString();
                    }
                    else if (field.FieldName == "Field24")
                    {
                        awbDp.ShipmentField24 = value.ToString();
                    }
                    else if (field.FieldName == "Field25")
                    {
                        awbDp.ShipmentField25 = value.ToString();
                    }
                    else if (field.FieldName == "Field26")
                    {
                        awbDp.ShipmentField26 = value.ToString();
                    }
                    else if (field.FieldName == "Field27")
                    {
                        awbDp.ShipmentField27 = value.ToString();
                    }
                    else if (field.FieldName == "Field28")
                    {
                        awbDp.ShipmentField28 = value.ToString();
                    }
                    else if (field.FieldName == "Field29")
                    {
                        awbDp.ShipmentField29 = value.ToString();
                    }
                    else if (field.FieldName == "Field30")
                    {
                        awbDp.ShipmentField30 = value.ToString();
                    }
                    else if (field.FieldName == "Field31")
                    {
                        awbDp.ShipmentField31 = value.ToString();
                    }
                    else if (field.FieldName == "Field32")
                    {
                        awbDp.ShipmentField32 = value.ToString();
                    }
                    else if (field.FieldName == "Field33")
                    {
                        awbDp.ShipmentField33 = value.ToString();
                    }
                    else if (field.FieldName == "Field34")
                    {
                        awbDp.ShipmentField34 = value.ToString();
                    }
                    else if (field.FieldName == "Field35")
                    {
                        awbDp.ShipmentField35 = value.ToString();
                    }
                    else if (field.FieldName == "Field36")
                    {
                        awbDp.ShipmentField36 = value.ToString();
                    }
                    else if (field.FieldName == "Field37")
                    {
                        awbDp.ShipmentField37 = value.ToString();
                    }
                    else if (field.FieldName == "Field38")
                    {
                        awbDp.ShipmentField38 = value.ToString();
                    }
                    else if (field.FieldName == "Field39")
                    {
                        awbDp.ShipmentField39 = value.ToString();
                    }
                    else if (field.FieldName == "Field40")
                    {
                        awbDp.ShipmentField40 = value.ToString();
                    }
                }
            }
        }

        private void GetAWBPrintingFields(AWBDataProvider dataProvider, ShipmentPM shipmentPM)
        {
            if (isRegulatedAgentActivated)
            {
                dataProvider.IssuingShipperSignature = shipmentPM.AWBSignature;
                dataProvider.IssuingCarrierSignature = shipmentPM.AWBSignature;
                if (shipmentPM.ViaColoader)
                {
                    if (!string.IsNullOrEmpty(shipmentPM.ColoaderId))
                    {
                        dataProvider.IssuingCarrierSignature = shipmentPM.ColoaderName;
                    }
                }

                if (shipmentPM.IsKnownCargo)
                {
                    if (!string.IsNullOrEmpty(shipmentPM.ColoaderRANumber))
                    {
                        dataProvider.RA = "RAR-RA" + shipmentPM.AWBPrintingRANumber;

                        if (!string.IsNullOrEmpty(shipmentPM.RegulatedAgentRANumber))
                        {
                            string myFieldCode = null;
                            if (!string.IsNullOrEmpty(shipmentPM.AWBPrintingSecurityStatusId))
                            {
                                AWBSpecialHandlingCodeRepository myRepository = new AWBSpecialHandlingCodeRepository(shipmentPM.Tenant);
                                AWBSpecialHandlingCode myField = myRepository.GetSingleAWBHandlingCode(shipmentPM.AWBPrintingSecurityStatusId);
                                if (myField != null)
                                {
                                    myFieldCode = myField.Code;
                                }
                            }

                            dataProvider.IssuingShipperSecurityCode = myFieldCode;
                        }
                    }

                    else
                    {
                        if (!string.IsNullOrEmpty(shipmentPM.RegulatedAgentRANumber) && !string.IsNullOrEmpty(shipmentPM.KnownConsignorNumber))
                        {
                            dataProvider.RA = "RAR-RA" + shipmentPM.AWBPrintingRANumber;

                            string myFieldCode = null;
                            if (!string.IsNullOrEmpty(shipmentPM.AWBPrintingSecurityStatusId))
                            {
                                AWBSpecialHandlingCodeRepository myRepository = new AWBSpecialHandlingCodeRepository(shipmentPM.Tenant);
                                AWBSpecialHandlingCode myField = myRepository.GetSingleAWBHandlingCode(shipmentPM.AWBPrintingSecurityStatusId);
                                if (myField != null)
                                {
                                    myFieldCode = myField.Code;
                                }
                            }

                            dataProvider.IssuingCarrierSecurityCode = myFieldCode;
                        }
                    }                                    
                }
            }
        }

        private void GetNotify1Data(AWBDataProvider awbDp, ShipmentPM shipmentPM, AddressRepository addressRepository)
        {
            int tenant = shipmentPM.Tenant;
            string notify1Id = shipmentPM.Notify1Id;
            string notify1AddressId = shipmentPM.Notify1AddressId;

            if (!string.IsNullOrEmpty(notify1Id))
            {
                Card notify1Card = CardRepository.GetSingleCard(notify1Id, tenant, false);
                ContactRepository contactRepository = new ContactRepository(myCommonContext);

                if (notify1Card != null)
                {
                    awbDp.NotifyPartyName = string.IsNullOrEmpty(notify1Card.EnglishName) ? "" : notify1Card.EnglishName;
                    awbDp.NotifyPartyLocalName = string.IsNullOrEmpty(notify1Card.LocalName) ? "" : notify1Card.LocalName;

                    if (!string.IsNullOrEmpty(notify1AddressId))
                    {
                        Address notify1Address = addressRepository.GetSingleAddress(notify1AddressId, tenant);

                        if (notify1Address != null)
                        {
                            awbDp.NotifyAddress_WithName = DataProviders.General.GetAddressWithName(notify1Address);
                            awbDp.NotifyPartyAddress1 = string.IsNullOrEmpty(notify1Address.Address1) ? "" : notify1Address.Address1;
                            awbDp.NotifyPartyAddress2 = string.IsNullOrEmpty(notify1Address.Address2) ? "" : notify1Address.Address2;
                            awbDp.NotifyPartyTel = string.IsNullOrEmpty(notify1Address.PhoneNumber) ? "" : notify1Address.PhoneNumber;
                            awbDp.NotifyPartyFax = string.IsNullOrEmpty(notify1Address.FaxNumber) ? "" : notify1Address.FaxNumber;
                            awbDp.NotifyPartyCity = string.IsNullOrEmpty(notify1Address.City) ? "" : notify1Address.City;
                            awbDp.NotifyPartyCountry = notify1Address.Country == null ? "" : notify1Address.Country.EnglishName;
                            awbDp.NotifyPartyZipCode = string.IsNullOrEmpty(notify1Address.ZipCode) ? "" : notify1Address.ZipCode;
                            awbDp.Notify1ATTN = notify1Address.ATTN;
                        }
                    }

                    if (!string.IsNullOrEmpty(shipmentPM.Notify1ContactId))
                    {
                        Contact myContact = contactRepository.GetSingleContact(shipmentPM.Notify1ContactId, myTenant);

                        if (myContact != null)
                        {
                            awbDp.Notify1ContactDetails = myContact.EnglishName;

                            if (!string.IsNullOrEmpty(myContact.BusinessPhone))
                            {
                                awbDp.Notify1ContactDetails = awbDp.Notify1ContactDetails + Environment.NewLine + "Ph: " + myContact.BusinessPhone;

                                if (!string.IsNullOrEmpty(myContact.Fax))
                                {
                                    awbDp.Notify1ContactDetails = awbDp.Notify1ContactDetails + " - Fx: " + myContact.Fax;
                                }
                            }

                            else
                            {
                                if (!string.IsNullOrEmpty(myContact.Fax))
                                {
                                    awbDp.Notify1ContactDetails = awbDp.Notify1ContactDetails + Environment.NewLine + "Fx: " + myContact.Fax;
                                }
                            }

                            if (!string.IsNullOrEmpty(myContact.Email))
                            {
                                awbDp.Notify1ContactDetails = awbDp.Notify1ContactDetails + Environment.NewLine + "Email: " + myContact.Email;
                            }
                        }
                    }

                }
            }
        }

        private void GetNotify2Data(AWBDataProvider awbDp, ShipmentPM shipmentPM, AddressRepository addressRepository)
        {
            int tenant = shipmentPM.Tenant;
            string notify2Id = shipmentPM.Notify2Id;
            string notify2AddressId = shipmentPM.Notify2AddressId;

            if (!string.IsNullOrEmpty(notify2Id))
            {
                Card notify2Card = CardRepository.GetSingleCard(notify2Id, tenant, false);
                ContactRepository contactRepository = new ContactRepository(myCommonContext);
                if (notify2Card != null)
                {
                    if (!string.IsNullOrEmpty(notify2AddressId))
                    {
                        Address notify2Address = addressRepository.GetSingleAddress(notify2AddressId, tenant);

                        if (notify2Address != null)
                        {
                            awbDp.NotifyAddress2_WithName = DataProviders.General.GetAddressWithName(notify2Address);
                            awbDp.Notify2ATTN = notify2Address.ATTN;
                        }
                    }

                    if (!string.IsNullOrEmpty(shipmentPM.Notify2ContactId))
                    {
                        Contact myContact = contactRepository.GetSingleContact(shipmentPM.Notify2ContactId, tenant);

                        if (myContact != null)
                        {
                            awbDp.Notify2ContactDetails = myContact.EnglishName;

                            if (!string.IsNullOrEmpty(myContact.BusinessPhone))
                            {
                                awbDp.Notify2ContactDetails = awbDp.Notify2ContactDetails + Environment.NewLine + "Ph: " + myContact.BusinessPhone;

                                if (!string.IsNullOrEmpty(myContact.Fax))
                                {
                                    awbDp.Notify2ContactDetails = awbDp.Notify2ContactDetails + " - Fx: " + myContact.Fax;
                                }
                            }

                            else
                            {
                                if (!string.IsNullOrEmpty(myContact.Fax))
                                {
                                    awbDp.Notify2ContactDetails = awbDp.Notify2ContactDetails + Environment.NewLine + "Fx: " + myContact.Fax;
                                }
                            }

                            if (!string.IsNullOrEmpty(myContact.Email))
                            {
                                awbDp.Notify2ContactDetails = awbDp.Notify2ContactDetails + Environment.NewLine + "Email: " + myContact.Email;
                            }
                        }
                    }
                }
            }
        }

        private void GetAgentData(AWBDataProvider awbDp, ShipmentPM shipmentPM, AddressRepository addressRepository)
        {
            int tenant = shipmentPM.Tenant;
            string agentId = shipmentPM.AgentId;
            string agentAddressId = shipmentPM.AgentAddressId;

            if (!string.IsNullOrEmpty(agentId))
            {
                if (!string.IsNullOrEmpty(agentAddressId))
                {
                    Address agentAddress = addressRepository.GetSingleAddress(agentAddressId, tenant);

                    if (agentAddress != null)
                    {
                        awbDp.AgentATTN = agentAddress.ATTN;
                    }
                }
            }
        }

        private void GetConsolidatorData(AWBDataProvider awbDp, ShipmentPM shipmentPM)
        {
            if (!string.IsNullOrEmpty(shipmentPM.ConsolidatorId))
            {
                Card card = CardRepository.GetSingleCard(shipmentPM.ConsolidatorId, shipmentPM.Tenant, false);
                if(card != null)
                {
                    awbDp.ConsolidatorName = card.EnglishName;
                }
            }
        }
    }
}
