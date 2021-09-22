using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Web.Services;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.DataProviders;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.WebServices;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.QuoteModel.Repositories;
using Simplog.Data.QuoteModel.EntityPOCOs;

namespace WebFreight.Web.ReportsWebServices
{
    /// <summary>
    /// Summary description for ManifestWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class ManifestWebService : System.Web.Services.WebService
    {
        ShipmentPM master;
        ShipmentPackageQuery packagesQuery;
        int tenant;
        ICommonDataContext commonContext;

        [WebMethod]
        public byte[] GetManifestData(string masterId, int tenant)
        {
            ManifestDataProvider manifestDataProvider = GetManifestDataProvider(masterId, tenant);

            #region Serialize and remove null region
            try
            {
                Type manifestType = manifestDataProvider.GetType();
                PropertyInfo[] properties = manifestType.GetProperties();

                foreach (PropertyInfo pi in properties)
                {
                    Type piType = pi.PropertyType;

                    if (piType.Name != "Double")
                    {
                        if (pi.GetValue(manifestDataProvider, null) == null || pi.GetValue(manifestDataProvider, null).ToString() == "0" || pi.GetValue(manifestDataProvider, null).ToString() == "00.00")
                        {
                            pi.SetValue(manifestDataProvider, "", null);
                        }
                    }
                }
            }
            catch { }

            XmlSerializer serializer = new XmlSerializer(typeof(ManifestDataProvider));
            MemoryStream memstream = new MemoryStream();
            serializer.Serialize(memstream, manifestDataProvider);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();
            return bytearray;
            #endregion
        }

        public ManifestDataProvider GetManifestDataProvider(string masterId, int tenant)
        {
            ManifestDataProvider manifestDataProvider = new ManifestDataProvider();
            IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(tenant);
            IWebFreightContext context = WebFreightContext.GetContext(tenant);
            this.commonContext = CommonDataContext.GetContext(tenant);
            ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentsContext);
            ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRepository);
            master = shipmentQuery.GetSinglePM(masterId, tenant);
            PortRepository portRepository = new PortRepository(tenant);
            CustomFieldResolver customFieldResolver = new CustomFieldResolver();
            ShipmentAssemblyRepository shipmentAssemblyRepository = new ShipmentAssemblyRepository(shipmentsContext);
            ShipmentAssemblyQuery shipmentAssemblyQuery = new ShipmentAssemblyQuery(shipmentAssemblyRepository);
            this.tenant = tenant;
            if (master != null)
            {
                CardQuery cardQuery = new CardQuery(tenant);
                ShippingLineQuery shippingLineQuery = new ShippingLineQuery(tenant);
                AddressRepository addressRepository = new AddressRepository(tenant);
                DocumentTypeCustomFieldRepository documentTypeCustomFieldsRepository = new DocumentTypeCustomFieldRepository(tenant);
                FormCustomFieldRepository formCustomFieldRepository = new FormCustomFieldRepository(tenant);
                packagesQuery = new ShipmentPackageQuery(tenant);
                IncotermQuery incotermQuery = new IncotermQuery(tenant);
                WebServiceHelper myServiceHelper = new WebServiceHelper(tenant);
                CustomerQuery customerQuery = new CustomerQuery(tenant);

                manifestDataProvider.ShipmentType = master.ShipmentTypeName != null ? master.ShipmentTypeName : "";
                manifestDataProvider.MasterNumber = master.ShipmentNumber;
                manifestDataProvider.Notes = master.Notes;
                manifestDataProvider.BookingNumber = master.BookingConfirmationNumber;
                manifestDataProvider.MainCarriageETA = master.MainCarriageETA == null ? "" : String.Format("{0:dd/MMM/yyyy}", master.MainCarriageETA);
                manifestDataProvider.ETD = master.MainCarriageETD == null ? "" : String.Format("{0:dd/MMM/yyyy}", master.MainCarriageETD);
                manifestDataProvider.MainCarriageCarrier = master.MainCarriageCarrierName;
                manifestDataProvider.MainCarriageETD_DateTime = master.MainCarriageETD;
                manifestDataProvider.MainCarriageETA_DateTime = master.MainCarriageETA;
                manifestDataProvider.MainCarriageATD_DateTime = master.MainCarriageATD;
                manifestDataProvider.MainCarriageATA_DateTime = master.MainCarriageATA;
                manifestDataProvider.OpenPayablesInLocalCurrency = master.OpenPayablesInLocalCurrency;
                manifestDataProvider.OpenPayablesInProfitCurrency = master.OpenPayablesInProfitCurrency;
                manifestDataProvider.DocumentsClosingDate = master.DocumentsClosingDate;
                manifestDataProvider.AWBHandlingInformation = master.AWBHandlingInformation;
                manifestDataProvider.FreightPC = master.FreightPrepaidCollectId;
                manifestDataProvider.DescriptionOfGoods = BuildDescriptionOfGoods();
                manifestDataProvider.ChargeableWeight = master.ChargeableWeight != null ? master.ChargeableWeight != 0 ? (String.Format("{0:#,0.00}", master.ChargeableWeight) + " " + (master.ChargeableWeightUnitCode != null ? master.ChargeableWeightUnitCode : "")) : "" : "";
                manifestDataProvider.TrailerNumber = master.TrailerNumber;
                manifestDataProvider.ProjectNumber = master.ProjectNumber;
                manifestDataProvider.MasterPreCarriageCarrierNumber = master.MasterPreCarriageCarrierNumber;
                manifestDataProvider.MasterPreCarriageVesselName = master.MasterPreCarriageVesselName;
                manifestDataProvider.MasterPreCarriageFromPortName = master.MasterPreCarriageFromPortName;
                manifestDataProvider.HousesNumbers = master.HousesNumbers;
                                
                if (master.BranchId != null)
                {
                    Branch myBranch = (from d in commonContext.Branches where d.Tenant == tenant && d.Id == master.BranchId select d).FirstOrDefault();
                    if (myBranch != null)
                    {
                        manifestDataProvider.BranchSignature = myBranch.Signature;
                    }
                }

                if (!string.IsNullOrEmpty(master.OBLTypeCode))
                {
                    OBLType type = shipmentsContext.OBLTypes.Where(d => d.Code == master.OBLTypeCode).FirstOrDefault();

                    if (type != null)
                    {
                        manifestDataProvider.OBLType = type.Name;
                    }
                }

                customFieldResolver.SetDataProviderCustomFieldsValues("Shipment", tenant, master, manifestDataProvider);

                Address mainCarriageCarrierAddress = addressRepository.GetSingleAddress(master.MainCarriageCarrierAddressId, tenant);
               
                if (mainCarriageCarrierAddress != null)
                {
                    manifestDataProvider.MainCarriageCarrierAddress = General.GetAddress(mainCarriageCarrierAddress);
                }

                TenantRepository tenantRepository = new TenantRepository(tenant);
                Tenant myLoggedTenant = tenantRepository.GetSingleTenant(tenant);

                if (myLoggedTenant != null)
                {
                    manifestDataProvider.FMCNumber = myLoggedTenant.FMCNumber;
                    manifestDataProvider.TenantVATNumber = myLoggedTenant.VatNumber != null ? myLoggedTenant.VatNumber : "";
                }

                #region Master Freight Location                
                if (!string.IsNullOrEmpty(master.FreightLocationId))
                {
                    Card freightLocationWarehouse = (from a in commonContext.Cards where a.Id == master.FreightLocationId select a).FirstOrDefault();
                    Address freightLocationWarehouseAddress = addressRepository.GetMainAddressByCardId(master.FreightLocationId, tenant);

                    if (freightLocationWarehouse != null)
                    {
                        manifestDataProvider.FreightLocationName = freightLocationWarehouse.EnglishName;
                    }

                    manifestDataProvider.FreightLocationAddress = General.GetAddress(freightLocationWarehouseAddress);
                }
                #endregion

                #region IssuingCarrierAgent
                if (!string.IsNullOrEmpty(master.IssuingCarrierAgentId))
                {
                    Card myCard = CardRepository.GetSingleCard(master.IssuingCarrierAgentId, tenant, true);
                    if (myCard != null)
                    {
                        manifestDataProvider.IssuingCarrierAgentName = myCard.EnglishName;

                        if (!string.IsNullOrEmpty(master.IssuingCarrierAddressId))
                        {
                            Address myAddress = addressRepository.GetSingleAddress(master.IssuingCarrierAddressId, tenant);

                            if (myAddress != null)
                            {
                                if (myAddress.IsLocalLanguage && !string.IsNullOrEmpty(myCard.LocalName))
                                {
                                    manifestDataProvider.IssuingCarrierAgentName = myCard.LocalName;
                                }

                                manifestDataProvider.IssuingCarrierAgentAddress = DataProviders.General.GetAddress(myAddress);
                            }
                        }
                    }
                }
                #endregion

                #region Master Shipper
                if (!string.IsNullOrEmpty(master.ShipperId))
                {
                    Card myCard = CardRepository.GetSingleCard(master.ShipperId, tenant, false);
                    if (myCard != null)
                    {
                        manifestDataProvider.ShipperName = myCard.EnglishName;

                        if (!string.IsNullOrEmpty(master.ShipperAddressId))
                        {
                            Address myAddress = addressRepository.GetSingleAddress(master.ShipperAddressId, tenant);
                            if (myAddress != null)
                            {
                                if (myAddress.IsLocalLanguage && !string.IsNullOrEmpty(myCard.LocalName))
                                {
                                    manifestDataProvider.ShipperName = myCard.LocalName;
                                }

                                manifestDataProvider.ShipperAddress = DataProviders.General.GetAddress(myAddress);

                                if (!string.IsNullOrEmpty(myAddress.PhoneNumber) || !string.IsNullOrEmpty(myAddress.FaxNumber))
                                {
                                    manifestDataProvider.ShipperAddress += System.Environment.NewLine;

                                    if (!string.IsNullOrEmpty(myAddress.PhoneNumber))
                                    {
                                        manifestDataProvider.ShipperAddress += "Tel: " + myAddress.PhoneNumber + " ";
                                    }

                                    if (!string.IsNullOrEmpty(myAddress.FaxNumber))
                                    {
                                        manifestDataProvider.ShipperAddress += "Fax: " + myAddress.FaxNumber;
                                    }
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(myCard.PrimaryContactId))
                        {
                            Contact contact = ContactRepository.GetSingleContact(myCard.PrimaryContactId, tenant, false);
                            if (contact != null)
                            {
                                manifestDataProvider.ShipperContactPhone = contact.BusinessPhone;
                                manifestDataProvider.ShipperContactName = contact.EnglishName;
                            }
                        }
                    }
                }
                #endregion

                #region Master Consignee
                if (!string.IsNullOrEmpty(master.ConsigneeId))
                {
                    CardPM consignee = cardQuery.GetSinglePM(master.ConsigneeId, tenant);
                    CustomerPM consigneePM = customerQuery.GetSinglePM(master.ConsigneeId, tenant);

                    if (consignee != null)
                    {
                        manifestDataProvider.ConsigneeVATNumber = consignee.VatNumber;
                        manifestDataProvider.ConsigneeName = consignee.EnglishName;
                        manifestDataProvider.ConsigneeAddress = GetConsigneAddress(master.ConsigneeAddressId, consignee.Tenant, addressRepository);
                        if (!string.IsNullOrEmpty(consignee.PrimaryContactId))
                        {
                            Contact contact = ContactRepository.GetSingleContact(consignee.PrimaryContactId, tenant, false);
                            if (contact != null)
                            {
                                manifestDataProvider.ConsigneeContactName = contact.EnglishName;
                                manifestDataProvider.ConsigneeContactEmail = contact.Email;
                                manifestDataProvider.ConsigneePhoneNumber = contact.BusinessPhone;
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(master.SalesmanUserId))
                {
                    manifestDataProvider.SalesmanName = master.SalesmanUserName;
                }

                #endregion

                #region Consolidator
                if (!string.IsNullOrEmpty(master.ConsolidatorId))
                {
                    Card myCard = CardRepository.GetSingleCard(master.ConsolidatorId, tenant, true);
                    if (myCard != null)
                    {
                        manifestDataProvider.ConsolidatorName = myCard.EnglishName;

                        if (!string.IsNullOrEmpty(master.ConsolidatorAddressId))
                        {
                            Address myAddress = addressRepository.GetSingleAddress(master.ConsolidatorAddressId, tenant);
                            if (myAddress != null)
                            {
                                if (myAddress.IsLocalLanguage && !string.IsNullOrEmpty(myCard.LocalName))
                                {
                                    manifestDataProvider.ConsolidatorName = myCard.LocalName;
                                }

                                manifestDataProvider.ConsolidatorAddress = DataProviders.General.GetAddress(myAddress);

                                if (!string.IsNullOrEmpty(myAddress.PhoneNumber) || !string.IsNullOrEmpty(myAddress.FaxNumber))
                                {
                                    manifestDataProvider.ConsolidatorAddress += System.Environment.NewLine;

                                    if (!string.IsNullOrEmpty(myAddress.PhoneNumber))
                                    {
                                        manifestDataProvider.ConsolidatorAddress += "Tel: " + myAddress.PhoneNumber + " ";
                                    }

                                    if (!string.IsNullOrEmpty(myAddress.FaxNumber))
                                    {
                                        manifestDataProvider.ConsolidatorAddress += "Fax: " + myAddress.FaxNumber;
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                #region ReleasingAgent
                if (!string.IsNullOrEmpty(master.ReleasingAgentId))
                {
                    Card myCard = CardRepository.GetSingleCard(master.ReleasingAgentId, tenant, true);
                    if (myCard != null)
                    {
                        manifestDataProvider.ReleasingAgentName = myCard.EnglishName;

                        if (!string.IsNullOrEmpty(master.ReleasingAgentAddressId))
                        {
                            Address myAddress = addressRepository.GetSingleAddress(master.ReleasingAgentAddressId, tenant);
                            if (myAddress != null)
                            {
                                if (myAddress.IsLocalLanguage && !string.IsNullOrEmpty(myCard.LocalName))
                                {
                                    manifestDataProvider.ReleasingAgentName = myCard.LocalName;
                                }

                                manifestDataProvider.ReleasingAgentAddress = DataProviders.General.GetAddress(myAddress);

                                if (!string.IsNullOrEmpty(myAddress.PhoneNumber) || !string.IsNullOrEmpty(myAddress.FaxNumber))
                                {
                                    manifestDataProvider.ReleasingAgentAddress += System.Environment.NewLine;

                                    if (!string.IsNullOrEmpty(myAddress.PhoneNumber))
                                    {
                                        manifestDataProvider.ReleasingAgentAddress += "Tel: " + myAddress.PhoneNumber + " ";
                                    }

                                    if (!string.IsNullOrEmpty(myAddress.FaxNumber))
                                    {
                                        manifestDataProvider.ReleasingAgentAddress += "Fax: " + myAddress.FaxNumber;
                                    }
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Agent Region
                if (!string.IsNullOrEmpty(master.AgentId))
                {
                    CardPM agent = cardQuery.GetSinglePM(master.AgentId, tenant);
                    if (agent != null)
                    {
                        manifestDataProvider.AgentName = agent.EnglishName;
                        Address agentAddress = addressRepository.GetSingleAddress(master.AgentAddressId, tenant);
                        if (agentAddress != null)
                        {
                            if (agentAddress.IsLocalLanguage && !string.IsNullOrEmpty(agent.LocalName))
                            {
                                manifestDataProvider.AgentName = agent.LocalName;
                            }

                            if (agentAddress != null)
                            {
                                manifestDataProvider.AgentAddress = DataProviders.General.GetAddress(agentAddress);
                                manifestDataProvider.AgentPhoneNumber = agentAddress.PhoneNumber;
                            }
                        }

                        if(!string.IsNullOrEmpty(agent.PrimaryContactId))
                        {
                            Contact contact = ContactRepository.GetSingleContact(agent.PrimaryContactId, tenant, true);
                            if(contact != null)
                            {
                                manifestDataProvider.AgentContactName = contact.EnglishName;
                                manifestDataProvider.AgentContactEmail = contact.Email;
                            }
                        }
                        manifestDataProvider.AgentVATNumber = agent.VatNumber != null ? agent.VatNumber : "";
                    }
                }
                else
                {
                    //if no agent in the master to take from the consignee in the export, domestic and from the shipper in the import
                    if (master.DirectionId == "E" || master.DirectionId == "D")
                    {
                        CardPM agent = cardQuery.GetSinglePM(master.ConsigneeId, tenant);
                        if (agent != null)
                        {
                            manifestDataProvider.AgentName = agent.EnglishName;
                            Address agentAddress = addressRepository.GetSingleAddress(master.ConsigneeAddressId, tenant);
                            if (agentAddress != null)
                            {
                                if (agentAddress.IsLocalLanguage && !string.IsNullOrEmpty(agent.LocalName))
                                {
                                    manifestDataProvider.AgentName = agent.LocalName;
                                }

                                if (agentAddress != null)
                                {
                                    manifestDataProvider.AgentAddress = DataProviders.General.GetAddress(agentAddress);
                                    manifestDataProvider.AgentPhoneNumber = agentAddress.PhoneNumber;
                                }
                            }

                            if (!string.IsNullOrEmpty(agent.PrimaryContactId))
                            {
                                Contact contact = ContactRepository.GetSingleContact(agent.PrimaryContactId, tenant, true);
                                if (contact != null)
                                {
                                    manifestDataProvider.AgentContactName = contact.EnglishName;
                                    manifestDataProvider.AgentContactEmail = contact.Email;
                                }
                            }
                        }
                    }
                    else
                    {
                        CardPM agent = cardQuery.GetSinglePM(master.ShipperId, tenant);
                        if (agent != null)
                        {
                            manifestDataProvider.AgentName = agent.EnglishName;
                            Address agentAddress = addressRepository.GetSingleAddress(master.ShipperAddressId, tenant);
                            if (agentAddress != null)
                            {
                                if (agentAddress.IsLocalLanguage && !string.IsNullOrEmpty(agent.LocalName))
                                {
                                    manifestDataProvider.AgentName = agent.LocalName;
                                }

                                if (agentAddress != null)
                                {
                                    manifestDataProvider.AgentAddress = DataProviders.General.GetAddress(agentAddress);
                                    manifestDataProvider.AgentPhoneNumber = agentAddress.PhoneNumber;
                                }
                            }

                            if (!string.IsNullOrEmpty(agent.PrimaryContactId))
                            {
                                Contact contact = ContactRepository.GetSingleContact(agent.PrimaryContactId, tenant, true);
                                if (contact != null)
                                {
                                    manifestDataProvider.AgentContactName = contact.EnglishName;
                                    manifestDataProvider.AgentContactEmail = contact.Email;
                                }
                            }
                        }
                    }
                }
                #endregion

                #region Master Region and airline and vessel and voyage
                manifestDataProvider.WeightUnit = master.GrossWeightUnitCode != null ? master.GrossWeightUnitCode : ""; // "KG";
                manifestDataProvider.VolumeUnit = master.VolumeUnitCode != null ? master.VolumeUnitCode : ""; //"M";
                manifestDataProvider.FlightInfo = master.MainCarriageCarrierCode + master.MainCarriageCarrierNumber;
                manifestDataProvider.FlightInfo_New = master.MainCarriageCarrierNumber;

                if (master.TransportModeId == "A")
                {
                    manifestDataProvider.MAWB = master.LongMaster;
                    manifestDataProvider.Airline = master.MainCarriageCarrierName;

                    if (master.MainCarriageETD != null)
                    {
                        manifestDataProvider.FlightDate = String.Format("{0:dd.MMM.yy}", master.MainCarriageETD.Value);
                    }
                }

                else if (master.TransportModeId == "O")
                {
                    if (master.MainCarriageCarrierId != null)
                    {
                        ShippingLinePM shippingLine = shippingLineQuery.GetSinglePM(master.MainCarriageCarrierId, tenant);
                        manifestDataProvider.ShippingAgentName = shippingLine.ShippingAgentName;

                        if (!string.IsNullOrEmpty(shippingLine.ShippingAgentId))
                        {
                            Address address = addressRepository.GetSingleAddressByCardIdAndTypeId(shippingLine.ShippingAgentId, "M", tenant);
                            if (address != null)
                            {
                                manifestDataProvider.ShippingAgentAddress = DataProviders.General.GetAddress(address);
                            }
                        }
                    }

                    manifestDataProvider.OBLNo = master.Master;
                    manifestDataProvider.VoyageNumber = master.MainCarriageCarrierNumber;
                    manifestDataProvider.VesselName = master.MainCarriageVesselName;
                    if (master.MainCarriageETD != null)
                    {
                        manifestDataProvider.VoyageDate = master.MainCarriageETD.Value.ToShortDateString();
                    }
                }
                #endregion

                List<ShipmentDataView> connectedShipments = shipmentRepository.GetShipmentViewsByTenantAndMasterId(masterId, tenant).ToList();
                manifestDataProvider.NumberOfHBLs = connectedShipments.Count;

                #region manifest details region

                List<FormCustomField> customfieldsList = formCustomFieldRepository.GetFormCustomFields(tenant).ToList();
                List<DocumentTypeCustomField> documentCustomfieldsList = documentTypeCustomFieldsRepository.GetDocumentTypeCustomFields(tenant).ToList();
                double grandTotalCollect = 0;
                double grandTotalPrepaid = 0;
                double totalWeight = 0;
                double totalVolume = 0;
                double totalPackagesQuantity = 0;
                double totalContainersQuantity = 0;

                foreach (ShipmentDataView shipmentView in connectedShipments)
                {
                    #region Start

                    ManifestDetailsClass detail = new ManifestDetailsClass();
                    NewManifestDetailsClass newDetail = new NewManifestDetailsClass();
      

                    detail.FileNumber = newDetail.FileNumber = shipmentView.ShipmentNumber;
                    detail.Direction = newDetail.Direction = shipmentView.DirectionName;
                    detail.ENSNumber = newDetail.ENSNumber = shipmentView.ENSNumber;
                    detail.ENSDate = newDetail.ENSDate = shipmentView.ENSDate;
                    detail.DocumentsClosingDate = newDetail.DocumentsClosingDate = shipmentView.DocumentsClosingDate;
                    detail.AWBHandlingInformation = newDetail.AWBHandlingInformation = shipmentView.AWBHandlingInformation;
                    detail.ITNumber = shipmentView.ITNumber;
                   
                    if (!string.IsNullOrEmpty(shipmentView.OBLTypeCode))
                    {
                        OBLType type = shipmentsContext.OBLTypes.Where(d => d.Code == shipmentView.OBLTypeCode).FirstOrDefault();

                        if (type != null)
                        {
                            detail.OBLType = newDetail.OBLType = type.Name;
                        }
                    }
                  
                    if (myLoggedTenant != null)
                    {
                        detail.FMCNumber = newDetail.FMCNumber = myLoggedTenant.FMCNumber;
                    }

                    string volumeUnitCode = shipmentView.VolumeUnitCode != null ? shipmentView.VolumeUnitCode : "";

                    #region Shipper
                    CardPM shipper = cardQuery.GetSinglePM(shipmentView.ShipperId, tenant);
                    CustomerPM shipperPM = customerQuery.GetSinglePM(shipmentView.ShipperId, tenant);

                    if (shipperPM != null)
                    {
                        customFieldResolver.SetDataProviderCustomFieldsValues("Customer", tenant, shipperPM, detail, "Shipper");
                        customFieldResolver.SetDataProviderCustomFieldsValues("Customer", tenant, shipperPM, newDetail, "Shipper");
                    }

                    if (shipper != null)
                    {
                        detail.ShipperVAT = newDetail.ShipperVAT = shipper.VatNumber;

                        detail.ShipperName = newDetail.ShipperName = shipper.EnglishName;
                        Address shipperAdderss = addressRepository.GetSingleAddress(shipmentView.ShipperAddressId, tenant);

                        if (shipperAdderss != null)
                        {
                            if (shipperAdderss.IsLocalLanguage && !string.IsNullOrEmpty(shipper.LocalName))
                            {
                                detail.ShipperName = newDetail.ShipperName = shipper.LocalName;
                            }

                            detail.ShipperAddress = newDetail.ShipperAddress = DataProviders.General.GetAddress(shipperAdderss);

                            if (!string.IsNullOrEmpty(shipperAdderss.PhoneNumber) || !string.IsNullOrEmpty(shipperAdderss.FaxNumber))
                            {
                                detail.ShipperAddress = newDetail.ShipperAddress += System.Environment.NewLine;

                                if (!string.IsNullOrEmpty(shipperAdderss.PhoneNumber))
                                {
                                    detail.ShipperAddress = detail.ShipperAddress + "Tel: " + shipperAdderss.PhoneNumber + " ";
                                    newDetail.ShipperAddress = newDetail.ShipperAddress + "Tel: " + shipperAdderss.PhoneNumber + " ";
                                }
                                if (!string.IsNullOrEmpty(shipperAdderss.FaxNumber))
                                {
                                    detail.ShipperAddress = detail.ShipperAddress + "Fax: " + shipperAdderss.FaxNumber;
                                    newDetail.ShipperAddress = newDetail.ShipperAddress + "Fax: " + shipperAdderss.FaxNumber;
                                }
                            }
                        }
                        else
                        {
                            detail.ShipperName = newDetail.ShipperName = "";
                            detail.ShipperAddress = newDetail.ShipperAddress = "";
                        }

                        if (!string.IsNullOrEmpty(shipper.PrimaryContactId))
                        {
                            Contact contact = ContactRepository.GetSingleContact(shipper.PrimaryContactId, tenant, false);
                            if (contact != null)
                            {
                                detail.ShipperContactName = contact.EnglishName;
                                detail.ShipperContactPhone = newDetail.ShipperContactPhone = contact.BusinessPhone;
                            }
                        }
                    }
                    else
                    {
                        detail.ShipperName = newDetail.ShipperName = "";
                        detail.ShipperAddress = newDetail.ShipperAddress = "";
                    }
                    #endregion

                    #region Consignee
                    if (!string.IsNullOrEmpty(shipmentView.ConsigneeId))
                    {
                        CardPM consignee = cardQuery.GetSinglePM(shipmentView.ConsigneeId, tenant);
                        CustomerPM consigneePM = customerQuery.GetSinglePM(shipmentView.ConsigneeId, tenant);

                        if (consigneePM != null)
                        {
                            customFieldResolver.SetDataProviderCustomFieldsValues("Customer", tenant, consigneePM, detail, "Consignee");
                            customFieldResolver.SetDataProviderCustomFieldsValues("Customer", tenant, consigneePM, newDetail, "Consignee");
                        }

                        if (consignee != null)
                        {
                            detail.ConsigneeVAT = newDetail.ConsigneeVAT = consignee.VatNumber;
                            detail.ConsigneeVATNumber = newDetail.ConsigneeVATNumber = consignee.VatNumber;
                            detail.ConsigneeName = newDetail.ConsigneeName = consignee.EnglishName;
                            Address consigneeAdderss = addressRepository.GetSingleAddress(shipmentView.ConsigneeAddressId, tenant);

                            if (consigneeAdderss != null)
                            {
                                if (consigneeAdderss.IsLocalLanguage && !string.IsNullOrEmpty(consignee.LocalName))
                                {
                                    detail.ConsigneeName = newDetail.ConsigneeName = consignee.LocalName;
                                }

                                detail.ConsigneeAddress = newDetail.ConsigneeAddress = DataProviders.General.GetAddress(consigneeAdderss);

                                if (!string.IsNullOrEmpty(consigneeAdderss.PhoneNumber) || !string.IsNullOrEmpty(consigneeAdderss.FaxNumber))
                                {
                                    detail.ConsigneeAddress = newDetail.ConsigneeAddress += System.Environment.NewLine;

                                    if (!string.IsNullOrEmpty(consigneeAdderss.PhoneNumber))
                                    {
                                        detail.ConsigneeAddress = detail.ConsigneeAddress + "Tel: " + consigneeAdderss.PhoneNumber + " ";
                                        newDetail.ConsigneeAddress = newDetail.ConsigneeAddress + "Tel: " + consigneeAdderss.PhoneNumber + " ";
                                    }
                                    if (!string.IsNullOrEmpty(consigneeAdderss.FaxNumber))
                                    {
                                        detail.ConsigneeAddress = detail.ConsigneeAddress + "Fax: " + consigneeAdderss.FaxNumber;
                                        newDetail.ConsigneeAddress = newDetail.ConsigneeAddress + "Fax: " + consigneeAdderss.FaxNumber;
                                    }
                                }
                            }

                            if (!string.IsNullOrEmpty(consignee.PrimaryContactId))
                            {
                                Contact contact = ContactRepository.GetSingleContact(consignee.PrimaryContactId, tenant, true);
                                if (contact != null)
                                {
                                    detail.ConsigneeContactName = newDetail.ConsigneeContactName = contact.EnglishName;
                                    detail.ConsigneeContactEmail = newDetail.ConsigneeContactEmail = contact.Email;
                                    detail.ConsigneePhoneNumber = newDetail.ConsigneePhoneNumber = contact.BusinessPhone;
                                }
                            }
                        }
                        else
                        {
                            detail.ConsigneeName = newDetail.ConsigneeName = "";
                            detail.ConsigneeAddress = newDetail.ConsigneeAddress = "";
                        }
                    }
                    else
                    {
                        detail.ConsigneeName = newDetail.ConsigneeName = "";
                        detail.ConsigneeAddress = newDetail.ConsigneeAddress = "";
                    }

                    #endregion

                    #region Notify
                    CardPM notify = cardQuery.GetSinglePM(shipmentView.Notify1Id, tenant);

                    if (notify != null)
                    {
                        detail.NotifyName = newDetail.NotifyName = notify.EnglishName;
                        Address notifyAdderss = addressRepository.GetSingleAddress(shipmentView.Notify1AddressId, tenant);
                        if (notifyAdderss != null)
                        {
                            if (notifyAdderss.IsLocalLanguage && !string.IsNullOrEmpty(notify.LocalName))
                            {
                                detail.NotifyName = newDetail.NotifyName = notify.LocalName;
                            }

                            detail.NotifyAddress = newDetail.NotifyAddress = DataProviders.General.GetAddress(notifyAdderss);
                        }
                        else
                        {
                            detail.NotifyName = newDetail.NotifyName = "";
                            detail.NotifyAddress = newDetail.NotifyAddress = "";
                        }
                    }
                    else
                    {
                        detail.NotifyName = newDetail.NotifyName = "";
                        detail.NotifyAddress = newDetail.NotifyAddress = "";
                    }
                    #endregion

                    #region Assemblies

                    List<ShipmentAssemblyPM> shipmentAssemblies = shipmentAssemblyQuery.GetShipmentAssemblies(shipmentView.Id, tenant).ToList();
                    if (shipmentAssemblies.Count > 0)
                    {
                        detail.Assemblies = new List<ShipmentAssemblyLine>();
                        newDetail.Assemblies = new List<ShipmentAssemblyLine>();

                        foreach (ShipmentAssemblyPM assembly in shipmentAssemblies)
                        {
                            detail.Assemblies.Add(new ShipmentAssemblyLine()
                            {
                                House = assembly.House,
                                ShipperName = assembly.ShipperName
                            });

                            newDetail.Assemblies.Add(new ShipmentAssemblyLine()
                            {
                                House = assembly.House,
                                ShipperName = assembly.ShipperName
                            });
                        }
                    }
                    #endregion

                    #region PlaceOfReceipt
                    ShipmentPickUpDelivery myFirstPickup =
                        (from d in shipmentsContext.ShipmentPickUpDeliveries
                         where d.ShipmentId == shipmentView.Id && d.PickUpDeliveryTypeCode == "PICK"
                         select d).OrderBy(s => s.PickUpDeliveryNumber).FirstOrDefault();

                    if (myFirstPickup != null)
                    {
                        PlaceOfReceiptData data = myServiceHelper.GetPlaceOfReceiptData(myFirstPickup);
                        if (data != null)
                        {
                            detail.PlaceOfReceipt = newDetail.PlaceOfReceipt = data.City;
                        }

                        PickUpAndDeliveriesArguments firstPickUpArguments = new PickUpAndDeliveriesArguments();
                        firstPickUpArguments.TypeCode = myFirstPickup.PickUpDeliveryFromTypeCode;
                        firstPickUpArguments.AddressId = myFirstPickup.FromAddressId;
                        firstPickUpArguments.PortId = myFirstPickup.FromPortId;
                        firstPickUpArguments.PartnerCardId = myFirstPickup.FromPartnerCardId;
                        firstPickUpArguments.AddressCity = myFirstPickup.FromAddressCity;
                        firstPickUpArguments.AddressZipCode = myFirstPickup.FromAddressZipCode;
                        firstPickUpArguments.AddressCountryId = myFirstPickup.FromAddressCountryId;
                        string firtPickUpFullAddress =  myServiceHelper.GetDeliveryPickUpAddress(firstPickUpArguments);                       

                        if (firtPickUpFullAddress != null)
                        {
                             newDetail.FisrtPickupFullAddress = firtPickUpFullAddress;
                        }
                    }

                    else
                    {
                        Port preCarriageFromPort = null;
                        Port preForwardingFromPort = null;

                        if (shipmentView.PreCarriageFromPortId != null)
                        {
                            preCarriageFromPort = (from a in commonContext.Ports where a.Id == shipmentView.PreCarriageFromPortId select a).FirstOrDefault();
                        }

                        if (shipmentView.PreForwardingFromPortId != null)
                        {
                            preForwardingFromPort = (from a in commonContext.Ports where a.Id == shipmentView.PreForwardingFromPortId select a).FirstOrDefault();
                        }

                        if(preForwardingFromPort != null)
                        {
                            detail.PlaceOfReceipt = newDetail.PlaceOfReceipt = preForwardingFromPort.EnglishName;
                        }

                        else if (preCarriageFromPort != null)
                        {
                            detail.PlaceOfReceipt = newDetail.PlaceOfReceipt = preCarriageFromPort.EnglishName;
                        }

                        else if (!string.IsNullOrEmpty(shipmentView.ShipperAddressId))
                        {
                            Address myPartnerAddress = addressRepository.GetSingleAddress(shipmentView.ShipperAddressId, tenant);
                            if (myPartnerAddress != null)
                            {
                                detail.PlaceOfReceipt = newDetail.PlaceOfReceipt = myPartnerAddress.City;
                            }
                        }
                    }
                    #endregion

                    #region Freight Location                
                    if (!string.IsNullOrEmpty(shipmentView.FreightLocationId))
                    {
                        Card freightLocationWarehouse = (from a in commonContext.Cards where a.Id == shipmentView.FreightLocationId select a).FirstOrDefault();
                        Address freightLocationWarehouseAddress = addressRepository.GetMainAddressByCardId(shipmentView.FreightLocationId, tenant);

                        if (freightLocationWarehouse != null)
                        {
                            detail.FreightLocationName = newDetail.FreightLocationName = freightLocationWarehouse.EnglishName;
                        }

                        detail.FreightLocationAddress = newDetail.FreightLocationAddress = General.GetAddress(freightLocationWarehouseAddress);
                    }
                    #endregion

                    Port onCarriageFromPort = null;
                    Port onCarriageToPort = null;
                    Port onForwardingFromPort = null;
                    Port onForwardingToPort = null;
                    Port preCarriage_FromPort = null;

                    if (shipmentView.OnCarriageFromPortId != null)
                    {
                        onCarriageFromPort = (from a in commonContext.Ports where a.Id == shipmentView.OnCarriageFromPortId select a).FirstOrDefault();
                    }                    

                    if (shipmentView.OnCarriageToPortId != null)
                    {
                        onCarriageToPort = (from a in commonContext.Ports where a.Id == shipmentView.OnCarriageToPortId select a).FirstOrDefault();
                    }                    

                    if (shipmentView.OnForwardingFromPortId != null)
                    {
                        onForwardingFromPort = (from a in commonContext.Ports where a.Id == shipmentView.OnForwardingFromPortId select a).FirstOrDefault();
                    }                    

                    if (shipmentView.OnForwardingToPortId != null)
                    {
                        onForwardingToPort = (from a in commonContext.Ports where a.Id == shipmentView.OnForwardingToPortId select a).FirstOrDefault();
                    }

                    if (shipmentView.PreCarriageFromPortId != null)
                    {
                        preCarriage_FromPort = (from a in commonContext.Ports where a.Id == shipmentView.PreCarriageFromPortId select a).FirstOrDefault();
                    }

                    newDetail.OnCarriageFromPort = onCarriageFromPort == null ? null : onCarriageFromPort.EnglishName;
                    newDetail.OnCarriageToPort = onCarriageToPort == null ? null : onCarriageToPort.EnglishName;
                    newDetail.OnForwardingFromPort = onForwardingFromPort == null ? null : onForwardingFromPort.EnglishName;
                    newDetail.OnForwardingToPort = onForwardingToPort ==  null ? null : onForwardingToPort.EnglishName;
                    newDetail.PreCarriageFromPort = preCarriage_FromPort == null ? null : preCarriage_FromPort.EnglishName;

                    ShipmentPickUpDelivery myLineFirstDelivery = (from d in shipmentsContext.ShipmentPickUpDeliveries
                                                                  where d.ShipmentId == shipmentView.Id && d.PickUpDeliveryTypeCode == "DELV"
                                                                  select d).OrderBy(s => s.PickUpDeliveryNumber).FirstOrDefault();

                    string myPlaceOfDelivery = myServiceHelper.GetPlaceOfDelivery(shipmentView, myLineFirstDelivery);
                    detail.PlaceOfDelivery = myPlaceOfDelivery;
                    newDetail.PlaceOfDelivery = myPlaceOfDelivery;

                    ShipmentPickUpDelivery lastDelivery = (from d in shipmentsContext.ShipmentPickUpDeliveries
                                                           where d.ShipmentId == shipmentView.Id && d.PickUpDeliveryTypeCode == "DELV"
                                                           select d).OrderByDescending(s => s.PickUpDeliveryNumber).FirstOrDefault();
                    if (lastDelivery != null)
                    {
                        PickUpAndDeliveriesArguments lastDeliveryArguments = new PickUpAndDeliveriesArguments();
                        lastDeliveryArguments.TypeCode = lastDelivery.PickUpDeliveryToTypeCode;
                        lastDeliveryArguments.AddressId = lastDelivery.ToAddressId;
                        lastDeliveryArguments.PortId = lastDelivery.ToPortId;
                        lastDeliveryArguments.PartnerCardId = lastDelivery.ToPartnerCardId;
                        lastDeliveryArguments.AddressCity = lastDelivery.ToAddressCity;
                        lastDeliveryArguments.AddressZipCode = lastDelivery.ToAddressZipCode;
                        lastDeliveryArguments.AddressCountryId = lastDelivery.ToAddressCountryId;
                        string lastDeliveryFullAddress = myServiceHelper.GetDeliveryPickUpAddress(lastDeliveryArguments);
                        if ( lastDeliveryFullAddress!= null)
                        {
                            newDetail.LastDeliveryFullAddress = lastDeliveryFullAddress;
                        }
                    }

                    detail.Weight = shipmentView.GrossWeight != null ? shipmentView.GrossWeight != 0 ? (String.Format("{0:#,0.00}", shipmentView.GrossWeight) + " " + (shipmentView.GrossWeightUnitCode != null ? shipmentView.GrossWeightUnitCode : "")) : "" : "";
                    detail.ChargeableWeight = newDetail.ChargeableWeight = shipmentView.ChargeableWeight != null ? shipmentView.ChargeableWeight != 0 ? (String.Format("{0:#,0.00}", shipmentView.ChargeableWeight) + " " + (shipmentView.ChargeableWeightUnitCode != null ? shipmentView.ChargeableWeightUnitCode : "")) : "" : "";
                    detail.Volume = shipmentView.Volume != null ? shipmentView.Volume != 0 ? (shipmentView.Volume + " " + volumeUnitCode) : "" : "";
                    newDetail.Weight = shipmentView.GrossWeight;
                    newDetail.Volume = shipmentView.Volume;

                    IncotermPM incoterm = incotermQuery.GetSinglePM(shipmentView.IncotermId, tenant);
                    detail.Incoterm = newDetail.Incoterm = incoterm != null ? incoterm.Name : "";

                    detail.House = newDetail.House = shipmentView.House != null ? shipmentView.House : "";

                    detail.DestinationPortCode = newDetail.DestinationPortCode = shipmentView.MainCarriageFinalDestinationPortCode != null ? shipmentView.MainCarriageFinalDestinationPortCode : "";
                    detail.DestinationPortName = newDetail.DestinationPortName = shipmentView.MainCarriageFinalDestinationPortName != null ? shipmentView.MainCarriageFinalDestinationPortName : "";
                    detail.AMSBL = newDetail.AMSBL = shipmentView.AMSBL;
                    List<ShipmentPackagePM> shipmentPackagesList = packagesQuery.GetShipmentPackages(shipmentView.Id, shipmentView.ShipmentNumber, tenant);

                    if (shipmentView.TransportModeId == "A")
                    {
                        detail.HAWB = newDetail.HAWB = shipmentView.House;

                        double totalPrepaid = 0;
                        double totalCollect = 0;
                        this.GetOtherCharges(shipmentView.Id, tenant, ref totalPrepaid, ref totalCollect);
                        grandTotalCollect = grandTotalCollect + (shipmentView.AWBFreightAmountCollect != null ? shipmentView.AWBFreightAmountCollect.Value : 0) + (totalCollect);
                        grandTotalPrepaid = grandTotalPrepaid + (shipmentView.AWBFreightAmountPrepaid != null ? shipmentView.AWBFreightAmountPrepaid.Value : 0) + (totalPrepaid);

                        detail.Quantity = shipmentView.NumberOfPackages != null ? shipmentView.NumberOfPackages.ToString() : "";
                        newDetail.Quantity = shipmentView.NumberOfPackages;
                        totalPackagesQuantity = totalPackagesQuantity + (shipmentView.NumberOfPackages != null ? shipmentView.NumberOfPackages.Value : 0);

                        detail.Dimensions = this.GetHousesPackagesDimensions(shipmentPackagesList);
                        #region custom fields
                        //----Freight Cusotmfield---//
                        FormCustomField freightField = (from a in customfieldsList
                                                        where a.FieldCode == "IsFreight" && a.EntityId == masterId
                                                        select a).FirstOrDefault();
                        DocumentTypeCustomField freightDocumentCustom = (from a in documentCustomfieldsList
                                                                         where a.FieldCode == "IsFreight"
                                                                         select a).FirstOrDefault();

                        FormCustomField otherField = (from a in customfieldsList
                                                      where a.FieldCode == "IsOther" && a.EntityId == masterId
                                                      select a).FirstOrDefault();

                        DocumentTypeCustomField otherDocumentCustom = (from a in documentCustomfieldsList
                                                                       where a.FieldCode == "IsOther"
                                                                       select a).FirstOrDefault();

                        if (freightField != null)
                        {
                            if (freightField.Value == "True")
                            {
                                detail.Prepaid = newDetail.Prepaid = "Freight: " + (shipmentView.AWBFreightAmountPrepaid != null ? shipmentView.AWBFreightAmountPrepaid.ToString() : "");
                                detail.Collect = newDetail.Collect = "Freight: " + (shipmentView.AWBFreightAmountCollect != null ? shipmentView.AWBFreightAmountCollect.ToString() : "");
                                if (otherField != null)
                                {
                                    if (otherField.Value == "True")
                                    {
                                        detail.Prepaid = newDetail.Prepaid = detail.Prepaid + Environment.NewLine + "Charges: " + totalPrepaid.ToString();
                                        detail.Collect = newDetail.Collect = detail.Collect + Environment.NewLine + "Charges: " + totalCollect.ToString();
                                    }
                                    else
                                    {
                                        detail.Prepaid = newDetail.Prepaid = detail.Prepaid + Environment.NewLine + "Charges: ";
                                        detail.Collect = newDetail.Collect = detail.Collect + Environment.NewLine + "Charges: ";
                                    }
                                }
                                else
                                {
                                    detail.Prepaid = newDetail.Prepaid = detail.Prepaid + Environment.NewLine + "Charges: ";
                                    detail.Collect = newDetail.Collect = detail.Collect + Environment.NewLine + "Charges: ";
                                }
                            }
                            else
                            {
                                detail.Prepaid = newDetail.Prepaid = "Freight: ";
                                detail.Collect = newDetail.Collect = "Freight: ";
                            }
                        }
                        else if (freightDocumentCustom != null)
                        {
                            if (freightDocumentCustom.DefaultValue == "True")
                            {
                                detail.Prepaid = newDetail.Prepaid = "Freight: " + (shipmentView.AWBFreightAmountPrepaid != null ? shipmentView.AWBFreightAmountPrepaid.ToString() : "");
                                detail.Collect = newDetail.Collect = "Freight: " + (shipmentView.AWBFreightAmountCollect != null ? shipmentView.AWBFreightAmountCollect.ToString() : "");
                            }
                            else
                            {
                                detail.Prepaid = newDetail.Prepaid = "Freight: ";
                                detail.Collect = newDetail.Collect = "Freight: ";
                            }
                        }
                        #endregion
                    }

                    else
                    {
                        detail.PortOfDischarge = newDetail.PortOfDischarge = shipmentView.MainCarriageToPortCode;

                        totalPackagesQuantity = totalPackagesQuantity + (shipmentView.NumberOfPackages != null ? shipmentView.NumberOfPackages.Value : 0);
                        totalContainersQuantity = totalContainersQuantity + (shipmentView.NumberOfContainers != null ? shipmentView.NumberOfContainers.Value : 0);
                    }

                    this.BuildPackageDetails(newDetail, shipmentView, volumeUnitCode);

                    StringBuilder strGoods = new StringBuilder();
                    strGoods.Append(shipmentView.DescriptionOfGoods != null ? shipmentView.DescriptionOfGoods : "");

                    if (!string.IsNullOrEmpty(shipmentView.SLAC))
                    {
                        if (!string.IsNullOrEmpty(strGoods.ToString()))
                        {
                            strGoods.Append(Environment.NewLine);
                        }

                        strGoods.Append("SLAC: " + shipmentView.SLAC);
                    }

                    if (shipmentView.ShipmentTypeName == "FCL" || shipmentView.ShipmentTypeName == "LCL")
                    {
                        strGoods.Append(Environment.NewLine);
                        strGoods.Append(shipmentView.ShipmentTypeName);
                        strGoods.Append(Environment.NewLine);

                        for (int i = 0; i < shipmentPackagesList.Count; i++)
                        {
                            strGoods.Append(shipmentPackagesList[i].ContainerNumber != null ? "CNT " + shipmentPackagesList[i].ContainerNumber : "");
                            strGoods.Append(Environment.NewLine);
                        }
                    }

                    detail.DescriptionOfGoods = newDetail.DescriptionOfGoods = strGoods.ToString();

                    totalWeight = totalWeight + (shipmentView.GrossWeight != null ? shipmentView.GrossWeight.Value : 0);
                    totalVolume = totalVolume + (shipmentView.Volume != null ? shipmentView.Volume.Value : 0);

                    detail.PC = newDetail.PC = shipmentView.FreightPrepaidCollectId;

                    detail.OpenPayablesInLocalCurrency = shipmentView.OpenPayablesInLocalCurrency;
                    detail.OpenPayablesInProfitCurrency = shipmentView.OpenPayablesInProfitCurrency;
                    newDetail.OpenPayablesInLocalCurrency = shipmentView.OpenPayablesInLocalCurrency;
                    newDetail.OpenPayablesInProfitCurrency = shipmentView.OpenPayablesInProfitCurrency;

                    if (!string.IsNullOrEmpty(shipmentView.QuoteId))
                    {
                        QuoteRepository quoteRepositoy = new QuoteRepository(tenant);
                        Quote connectedQuote = quoteRepositoy.GetSingleQuote(shipmentView.QuoteId, tenant);
                        detail.QuoteNumberConnectedToHouse = newDetail.QuoteNumberConnectedToHouse = connectedQuote.QuoteNumber != null ? connectedQuote.QuoteNumber : "";
                    }
                    
                    detail.ShipperRefernce1 = newDetail.ShipperRefernce1 = shipmentView.ShipperReference1 != null ? shipmentView.ShipperReference1 : "";
                    detail.ValueOfGoods = newDetail.ValueOfGoods = shipmentView.ValueOfGoods;

                    if (!string.IsNullOrEmpty(shipmentView.ValueOfGoodsCurrencyId))
                    {
                        CurrencyRepository currencyRepository = new CurrencyRepository(tenant);
                        Currency valueOfGoodsCurrency = currencyRepository.GetSingleCurrency(shipmentView.ValueOfGoodsCurrencyId, tenant);
                        detail.ValueOfGoodsCurrency = newDetail.ValueOfGoodsCurrency = valueOfGoodsCurrency.Code;
                    }

                    customFieldResolver.SetDataProviderCustomFieldsValues("Shipment", tenant, shipmentView, detail);
                    customFieldResolver.SetDataProviderCustomFieldsValues("Shipment", tenant, shipmentView, newDetail);

                    manifestDataProvider.ManifestDetails.Add(detail);
                    manifestDataProvider.NewManifestDetails.Add(newDetail);
                    #endregion
                }
                
                manifestDataProvider.TotalCollect = grandTotalCollect.ToString();
                manifestDataProvider.TotalPrepaid = grandTotalPrepaid.ToString();

                if (totalPackagesQuantity != 0)
                    manifestDataProvider.TotalQuantity = totalPackagesQuantity.ToString();// + " Pcs" + Environment.NewLine;

                if (totalContainersQuantity != 0)
                    manifestDataProvider.TotalQuantity += totalContainersQuantity.ToString();// +" Con";

                manifestDataProvider.TotalVolume = totalVolume != 0 ? (String.Format("{0:#,0.00}", totalVolume) + " " + (manifestDataProvider.VolumeUnit)) : ""; //CBM
                manifestDataProvider.TotalWeight = totalWeight != 0 ? (String.Format("{0:#,0.00}", totalWeight) + " " + (manifestDataProvider.WeightUnit)) : ""; //KGS 
                #endregion

                #region NEW DESIGN

                if (connectedShipments.Count > 0)
                {
                    manifestDataProvider.GroupedManifestDetailsList = new List<GroupedContainersClass>();

                    ShipmentPackageRepository shipmentPackageRepository = new ShipmentPackageRepository(shipmentsContext);
                    List<ShipmentPackage> masterPackages = shipmentPackageRepository.GetShipmentPackagesForShipmentTenant(master.Id, tenant).ToList();

                    List<string> connectedShipmentsIds = connectedShipments.Select(s => s.Id).ToList();
                    List<ShipmentPackage> connectedShipmentsPackages = shipmentPackageRepository.GetPackagesFromShipmentsIds(connectedShipmentsIds, tenant).ToList();

                    foreach (ShipmentPackage masterPackage in masterPackages)
                    {
                        PackageType masterPackagetype = (from pa in commonContext.PackageTypes
                                                   where pa.Id == masterPackage.PackageTypeId
                                                   select pa).FirstOrDefault();


                        GroupedContainersClass myBigItem = new GroupedContainersClass();
                        myBigItem.MasterContainerNumber = masterPackage.ContainerNumber;
                        myBigItem.ContainerType = masterPackagetype == null ? null : masterPackagetype.EnglishName;
                        myBigItem.ContainerGrossWeight = masterPackage.Weight;
                        myBigItem.ContainerGrossWeightUnitCode = master.GrossWeightUnitCode;
                        myBigItem.ContainerVolume = masterPackage.Volume;
                        myBigItem.ContainerVolumeUnitCode = master.VolumeUnitCode;
                        myBigItem.ContainerTare = masterPackage.Tare;
                        myBigItem.ContainerVolumetricWeight = masterPackage.VolumetricWeight;
                        myBigItem.ContainerVolumetricWeightUnitCode = master.VolumeUnitCode;
                        myBigItem.GroupedShipmentList = new List<GroupedShipmentClass>();

                        List<ShipmentPackage> containerPackages = connectedShipmentsPackages.Where(d => d.ContainerNumber == masterPackage.ContainerNumber).ToList();

                        List<NewTemplateClass> newTemplateList = new List<NewTemplateClass>();

                        foreach (ShipmentPackage package in containerPackages)
                        {
                            NewTemplateClass newItem = new NewTemplateClass();

                            newItem.MarksAndNumbers = package.MarksAndNumbers;
                            newItem.Quantity = package.Quantity;
                            newItem.DescriptionOfGoods = package.Description;
                            newItem.Weight = package.Weight;
                            newItem.Volume = package.Volume;
                            newItem.ContainerNumber = package.ContainerNumber;
                            newItem.ShipmentId = package.ShipmentId;

                            newTemplateList.Add(newItem);
                        }

                        var groupedData = from item in newTemplateList
                                          group item by new { item.ShipmentId, } into g
                                          select new
                                          {
                                              ShipmentId = g.Key.ShipmentId,
                                              GroupedPackageItems = g,
                                          };

                        foreach (var item in groupedData)
                        {
                            GroupedShipmentClass myShipmentItem = new GroupedShipmentClass()
                            {
                                GroupedManifestPackagesList = new List<GroupedManifestPackages>(),
                                ShipmentId = item.ShipmentId,
                            };

                            ShipmentDataView myShipment = connectedShipments.Where(d => d.Id == item.ShipmentId).FirstOrDefault();
                            myShipmentItem.ShipmentNumber = myShipment.ShipmentNumber;
                            myShipmentItem.ShipperName = myShipment.ShipperName;
                            myShipmentItem.ConsigneeName = myShipment.ConsigneeName;
                            myShipmentItem.PortOfDischarge = myShipment.MainCarriageToPortCode;
                            myShipmentItem.PC = myShipment.FreightPrepaidCollectId;

                            #region Shipper
                            CardPM shipper = cardQuery.GetSinglePM(myShipment.ShipperId, tenant);
                            if (shipper != null)
                            {
                                Address shipperAdderss = addressRepository.GetSingleAddress(myShipment.ShipperAddressId, tenant);
                                if (shipperAdderss != null)
                                {
                                    myShipmentItem.ShipperAddress = DataProviders.General.GetAddress(shipperAdderss);

                                    if (!string.IsNullOrEmpty(shipperAdderss.PhoneNumber) || !string.IsNullOrEmpty(shipperAdderss.FaxNumber))
                                    {
                                        myShipmentItem.ShipperAddress += System.Environment.NewLine;

                                        if (!string.IsNullOrEmpty(shipperAdderss.PhoneNumber))
                                        {
                                            myShipmentItem.ShipperAddress = myShipmentItem.ShipperAddress + "Tel: " + shipperAdderss.PhoneNumber + " ";
                                        }
                                        if (!string.IsNullOrEmpty(shipperAdderss.FaxNumber))
                                        {
                                            myShipmentItem.ShipperAddress = myShipmentItem.ShipperAddress + "Fax: " + shipperAdderss.FaxNumber;
                                        }
                                    }
                                }
                                else
                                {
                                    myShipmentItem.ShipperAddress = "";
                                }
                            }
                            else
                            {
                                myShipmentItem.ShipperAddress = "";
                            }
                            #endregion

                            #region Consignee
                            if (!string.IsNullOrEmpty(myShipment.ConsigneeId))
                            {
                                CardPM consignee = cardQuery.GetSinglePM(myShipment.ConsigneeId, tenant);

                                if (consignee != null)
                                {
                                    Address consigneeAdderss = addressRepository.GetSingleAddress(myShipment.ConsigneeAddressId, tenant);
                                    if (consigneeAdderss != null)
                                    {
                                        myShipmentItem.ConsigneeAddress = DataProviders.General.GetAddress(consigneeAdderss);

                                        if (!string.IsNullOrEmpty(consigneeAdderss.PhoneNumber) || !string.IsNullOrEmpty(consigneeAdderss.FaxNumber))
                                        {
                                            myShipmentItem.ConsigneeAddress += System.Environment.NewLine;

                                            if (!string.IsNullOrEmpty(consigneeAdderss.PhoneNumber))
                                            {
                                                myShipmentItem.ConsigneeAddress = myShipmentItem.ConsigneeAddress + "Tel: " + consigneeAdderss.PhoneNumber + " ";
                                            }
                                            if (!string.IsNullOrEmpty(consigneeAdderss.FaxNumber))
                                            {
                                                myShipmentItem.ConsigneeAddress = myShipmentItem.ConsigneeAddress + "Fax: " + consigneeAdderss.FaxNumber;
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    myShipmentItem.ConsigneeAddress = "";
                                }
                            }
                            else
                            {
                                myShipmentItem.ConsigneeAddress = "";
                            }
                            #endregion

                            foreach (var item11 in item.GroupedPackageItems)
                            {
                                myShipmentItem.GroupedManifestPackagesList.Add(new GroupedManifestPackages()
                                {
                                    ContainerNumber = item11.ContainerNumber,
                                    MarksAndNumbers = item11.MarksAndNumbers,
                                    DescriptionOfGoods = item11.DescriptionOfGoods,
                                    Quantity = item11.Quantity,
                                    Weight = item11.Weight,
                                    Volume = item11.Volume,
                                });
                            }

                            myBigItem.GroupedShipmentList.Add(myShipmentItem);
                        }

                        manifestDataProvider.GroupedManifestDetailsList.Add(myBigItem);
                    }
                }

                #endregion

                #region From to ports
                manifestDataProvider.DeparturePortCode = master.MainCarriageFromPortCode != null ? master.MainCarriageFromPortCode : "";
                manifestDataProvider.DestinationPortCode = master.MainCarriageFinalDestinationPortCode != null ? master.MainCarriageFinalDestinationPortCode : "";
                #endregion

                #region First Delivery | PlaceOfDelivery

                ShipmentPickUpDelivery myFirstDelivery
                    = (from d in shipmentsContext.ShipmentPickUpDeliveries
                       where d.ShipmentId == master.Id && d.PickUpDeliveryTypeCode == "DELV"
                       select d).OrderBy(s => s.PickUpDeliveryNumber).FirstOrDefault();


                manifestDataProvider.PlaceOfDelivery = myServiceHelper.GetPlaceOfDelivery(master, myFirstDelivery);

                #endregion

                manifestDataProvider.PortOfDischargeName = master.MainCarriageToPortName != null ? master.MainCarriageToPortName : "";

                if (master.Transshipment3ToPortCountryCode != null)
                {
                    manifestDataProvider.PortOfDischargeCountryCode = master.Transshipment3ToPortCountryCode;
                }

                else if (master.Transshipment2ToPortCountryCode != null)
                {
                    manifestDataProvider.PortOfDischargeCountryCode = master.Transshipment2ToPortCountryCode;
                }
                else if (master.Transshipment1ToPortCountryCode != null)
                {
                    manifestDataProvider.PortOfDischargeCountryCode = master.Transshipment1ToPortCountryCode;
                }

                else if (master.MainCarriageToPortCountryCode != null)
                {
                    manifestDataProvider.PortOfDischargeCountryCode = master.MainCarriageToPortCountryCode;
                }

                manifestDataProvider.PortOfLoadingName = master.MainCarriageFromPortName != null ? master.MainCarriageFromPortName : "";
                manifestDataProvider.PortOfLoadingCountryCode = master.MainCarriageFromPortCountryCode != null ? master.MainCarriageFromPortCountryCode : "";
                manifestDataProvider.TodaysDate = String.Format("{0:dd.MMM.yy}", TenantServerConfigration.GetCurrentDateTime(tenant));

                if (!string.IsNullOrEmpty(master.MoveTypeId))
                {
                    MoveType moveType = context.MoveTypes.Where(m => m.Id == master.MoveTypeId).FirstOrDefault();

                    if (moveType != null)
                    {
                        manifestDataProvider.MoveTypeCode = moveType.Code;
                        manifestDataProvider.MoveTypeName = moveType.MoveTypeEnglishName;
                    }
                }

                StringBuilder strCon = new StringBuilder();
                StringBuilder strSeal = new StringBuilder();

                List<ShipmentPackagePM> packagesList = packagesQuery.GetShipmentPackages(master.Id, master.ShipmentNumber, tenant);
                manifestDataProvider.ContainerNumbersLabel = "Container Numbers";
                manifestDataProvider.SealNumbersLablel = "Seal Numbers";

                manifestDataProvider.ContainerNumbers = "";
                manifestDataProvider.SealNumbers = "";

                foreach (ShipmentPackagePM package in packagesList)
                {
                    if (!string.IsNullOrEmpty(package.ContainerNumber))
                    {
                        strCon.Append(' ');
                        strCon.Append(package.ContainerNumber);
                        strCon.Append(',');                        
                    }

                    if (!string.IsNullOrEmpty(package.ShipperSeal))
                    {
                        strSeal.Append(' ');
                        strSeal.Append(package.ShipperSeal);
                        strSeal.Append(',');
                    }
                }

                string str_String_con = strCon.ToString();
                if (!string.IsNullOrEmpty(str_String_con))
                {
                    str_String_con = str_String_con.TrimStart(' ');
                    str_String_con = str_String_con.TrimEnd(',');
                }

                string str_String_sel = strSeal.ToString();
                if (!string.IsNullOrEmpty(str_String_sel))
                {
                    str_String_sel = str_String_sel.TrimStart(' ');
                    str_String_sel = str_String_sel.TrimEnd(',');
                }

                manifestDataProvider.ContainerNumbers = str_String_con;
                manifestDataProvider.SealNumbers = str_String_sel;
                manifestDataProvider.Logo = DataProviders.General.GetLogo(tenant);
            }

            #region Serialize and remove null region
            try
            {
                Type manifestType = manifestDataProvider.GetType();
                PropertyInfo[] properties = manifestType.GetProperties();

                foreach (PropertyInfo pi in properties)
                {
                    Type piType = pi.PropertyType;

                    if (piType.Name != "Double")
                    {
                        if (pi.GetValue(manifestDataProvider, null) == null || pi.GetValue(manifestDataProvider, null).ToString() == "0" || pi.GetValue(manifestDataProvider, null).ToString() == "00.00")
                        {
                            pi.SetValue(manifestDataProvider, "", null);
                        }
                    }
                }
            }
            catch { }

            return manifestDataProvider;
            #endregion
        }

        private string GetHousesPackagesDimensions(List<ShipmentPackagePM> packagse)
        {
            string dimensions = "";
            string myDimensions = "";
            foreach (ShipmentPackagePM item in packagse)
            {
                if (item.Length == null && item.Width == null && item.Height == null)
                {
                    myDimensions = " - - ";
                }

                else
                {
                    double? myLength = 0;
                    double? myWidth = 0;
                    double? myHeight = 0;

                    if (item.Length != null)
                    {
                        myLength = item.Length;
                    }

                    if (item.Width != null)
                    {
                        myWidth = item.Width;
                    }

                    if (item.Height != null)
                    {
                        myHeight = item.Height;
                    }

                    myDimensions = myLength + "-" + myWidth + "-" + myHeight;
                }

                dimensions = dimensions + item.Quantity + " X "  + myDimensions + Environment.NewLine;
            }

            return dimensions;
        }

        private void BuildPackageDetails(NewManifestDetailsClass newDetail, ShipmentDataView shipmentView, string volumeUnitCode)
        {
            ShipmentPackageQuery shipmentPackagesQuery = new ShipmentPackageQuery(tenant);
            List<ShipmentPackagePM> packages = shipmentPackagesQuery.GetShipmentPackages(shipmentView.Id, shipmentView.ShipmentNumber, tenant);

            foreach (ShipmentPackagePM package in packages)
            {
                PackageDetails packageDetail = new PackageDetails();

                packageDetail.DescriptionOfGoods = !string.IsNullOrEmpty(package.Description) ? package.Description : "";

                if (!string.IsNullOrEmpty(package.Harmonize))
                {
                    if (!string.IsNullOrEmpty(packageDetail.DescriptionOfGoods))
                    {
                        packageDetail.DescriptionOfGoods += Environment.NewLine;
                    }

                    packageDetail.DescriptionOfGoods += "HS Code: " + package.Harmonize;
                }

                packageDetail.Weight = package.Weight != null ? (package.Weight != 0 ? String.Format("{0:#,0.00}", package.Weight + " " + (shipmentView.GrossWeightUnitCode != null ? shipmentView.GrossWeightUnitCode : "")) : "") : "";
                packageDetail.Volume = package.Volume != null ? (package.Volume != 0 ? package.Volume.ToString() + " " + volumeUnitCode : "") : "";
                packageDetail.Quantity = package.Quantity == null ? "" : package.Quantity.ToString();

                if (package.Length != null && package.Width != null && package.Height != null)
                {
                    packageDetail.Dimensions = package.Length + "x" + package.Width + "x" + package.Height;
                }

                if (package.PackageTypeId != null)
                {
                    PackageType packageType = (from pa in commonContext.PackageTypes
                                               where pa.Id == package.PackageTypeId
                                               select pa).FirstOrDefault();

                    if (packageType != null)
                    {
                        packageDetail.PackageKind = packageType.EnglishName;

                        if (packageType.IsContainer)
                        {
                            string alphaFormat = @"[^A-Za-z]*";
                            string numericFormat = @"[^0-9]*";
                            string alpha = Regex.Replace(packageType.Code, alphaFormat, string.Empty, RegexOptions.Compiled);
                            string num = Regex.Replace(packageType.Code, numericFormat, string.Empty, RegexOptions.Compiled);

                            packageDetail.Quantity +=  " x " + (num + "'" + alpha);
                        }

                        else
                        {
                            packageDetail.Quantity += " x " + packageType.EnglishName;
                        }
                    }
                }

                if (string.IsNullOrEmpty(package.MarksAndNumbers))
                {
                    // build logic
                    string str = String.Empty;
                    string importer = String.Empty;
                    string importerRef1 = String.Empty;

                    if (shipmentView.DirectionId == "E" || shipmentView.DirectionId == "D")
                    {
                        importer = shipmentView.ConsigneeName;
                        importerRef1 = shipmentView.ConsigneeReference1;
                    }
                    else if (shipmentView.DirectionId == "I")
                    {
                        importer = shipmentView.ShipperName;
                        importerRef1 = shipmentView.ShipperReference1;
                    }

                    if (package.IsContainer)
                    {
                        if (!string.IsNullOrEmpty(package.ContainerNumber))
                        {
                            str = str + package.ContainerNumber + "\n";
                        }

                        if (!string.IsNullOrEmpty(package.ShipperSeal))
                        {
                            str = str + "Shipper Seal: " + package.ShipperSeal + "\n";
                        }

                        if (!string.IsNullOrEmpty(package.CarrierSeal))
                        {
                            str = str + "Carrier Seal: " + package.CarrierSeal + "\n";
                        }

                        if (package.Tare != null)
                        {
                            str = str + "Tare: " + package.Tare.ToString() + "\n";
                        }

                        if (!string.IsNullOrEmpty(importer))
                        {
                            str = str + importer;
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(importer))
                        {
                            str = str + importer + "\n";
                        }

                        if (!string.IsNullOrEmpty(importerRef1))
                        {
                            str = str + "PO: " + importerRef1;
                        }
                    }
                    packageDetail.MarksAndNumbers = str;
                }
                else
                {
                    packageDetail.MarksAndNumbers = package.MarksAndNumbers != null ? package.MarksAndNumbers : "";
                }

                packageDetail.Reference1 = package.Reference1;
                packageDetail.Reference2 = package.Reference2;
                packageDetail.Reference3 = package.Reference3;
                packageDetail.Reference4 = package.Reference4;
                packageDetail.CommodityNumber = package.CommodityNumber;
                packageDetail.Notes = package.Notes;
                packageDetail.Harmonize = package.Harmonize;
                packageDetail.Tare = package.Tare;

                if (!string.IsNullOrEmpty(package.HorseId))
                {
                    Horse horse = (from pa in commonContext.Horses
                                   where pa.Id == package.HorseId
                                   select pa).FirstOrDefault();

                    if (horse != null)
                    {
                        packageDetail.HorseName = horse.Name;
                        packageDetail.HorseYearOfBirth = horse.YearOfBirth;
                        packageDetail.HorseColor = horse.Color;
                        packageDetail.HorseGender = horse.Gender;
                        packageDetail.HorseBreed = horse.Breed;
                        packageDetail.HorseDiscipline = horse.Discipline;
                        packageDetail.HorseTravelBehavior = horse.TravelBehavior;
                        packageDetail.HorseMicochipNumber = horse.MicochipNumber;
                        packageDetail.HorsePassportNumber = horse.PassportNumber;
                        packageDetail.HorseCurrentStable = horse.CurrentStable;
                        packageDetail.HorseOwner = horse.Owner;
                        packageDetail.HorseRemarks = horse.Remarks;

                        if (!string.IsNullOrEmpty(horse.CountryOfBirthId))
                        {
                            Country country = (from pa in commonContext.Countries
                                               where pa.Id == horse.CountryOfBirthId
                                               select pa).FirstOrDefault();

                            if (country != null)
                            {
                                packageDetail.HorseCountryOfBirthName = country.EnglishName;
                            }
                        }
                    }
                }

                newDetail.PackageDetails.Add(packageDetail);

                #region commented Code
                //StringBuilder qty = new StringBuilder();
                //StringBuilder weight = new StringBuilder();
                //StringBuilder volume = new StringBuilder();
                //StringBuilder marks = new StringBuilder();
                //StringBuilder desc = new StringBuilder();

                //desc.Append(!string.IsNullOrEmpty(package.Description) ? package.Description : "");
                //if (!string.IsNullOrEmpty(package.Harmonize))
                //{
                //    if (!string.IsNullOrEmpty(desc.ToString()))
                //    {
                //        desc.Append(Environment.NewLine);
                //    }
                //    desc.Append("HS Code: " + package.Harmonize);
                //}

                //weight.Append(package.Weight != null ? (package.Weight != 0 ? String.Format("{0:#,0.00}", package.Weight + " " + (shipmentView.GrossWeightUnitCode != null ? shipmentView.GrossWeightUnitCode : "")) : "") : "");
                //volume.Append(package.Volume != null ? (package.Volume != 0 ? package.Volume.ToString() + " " + volumeUnitCode : "") : "");

                //if (string.IsNullOrEmpty(packageKinds))
                //{
                //    packageKinds = package.PackageTypeName;
                //}
                //else
                //{
                //    packageKinds = packageKinds + Environment.NewLine + package.PackageTypeName;
                //}

                //if (string.IsNullOrEmpty(packageQty))
                //{
                //    packageQty = (package.Quantity != null ? package.Quantity.ToString() : "");
                //}
                //else
                //{
                //    packageQty = packageQty + Environment.NewLine + (package.Quantity != null ? package.Quantity.ToString() : "");
                //}

                //if (packagetype.IsContainer)
                //{
                //    string alphaFormat = @"[^A-Za-z]*";
                //    string numericFormat = @"[^0-9]*";
                //    string alpha = Regex.Replace(packagetype.Code, alphaFormat, string.Empty, RegexOptions.Compiled);
                //    string num = Regex.Replace(packagetype.Code, numericFormat, string.Empty, RegexOptions.Compiled);

                //    qty.Append((package.Quantity != null ? package.Quantity.ToString() : "") + " x " + (num + "'" + alpha));
                //}
                //else
                //{
                //    qty.Append((package.Quantity != null ? package.Quantity.ToString() : "") + " x " + packagetype.EnglishName);
                //}

                //if (package.MarksAndNumbers == null)
                //{
                //    if (packagetype.IsContainer)
                //    {
                //        if (package.ContainerNumber != null)
                //        {
                //            if (package.ContainerNumber.Length > 10)
                //            {
                //                marks.Append(!string.IsNullOrEmpty(package.ContainerNumber) ? package.ContainerNumber.Substring(0, 4) + " " + package.ContainerNumber.Substring(4, 6) + "/" + package.ContainerNumber.Substring(10, 1) + Environment.NewLine : "");
                //            }
                //            else
                //            {
                //                marks.Append(package.ContainerNumber);
                //            }
                //        }

                //        if (!string.IsNullOrEmpty(package.Seal))
                //        {
                //            marks.Append("SEAL:" + package.Seal);
                //        }
                //    }
                //}
                //else
                //{
                //    marks.Append(package.MarksAndNumbers != null ? package.MarksAndNumbers : "");
                //}

                //countDesc = this.countLines(desc.ToString());
                //countMarks = this.countLines(marks.ToString());

                //if (countDesc > countMarks)
                //{
                //    for (int i = 0; i < countDesc; i++)
                //    {
                //        qty.Append('\n');
                //        marks.Append('\n');
                //        weight.Append('\n');
                //        volume.Append('\n');
                //    }
                //    desc.Append('\n');
                //    qty.Append('\n');
                //    weight.Append('\n');
                //    volume.Append('\n');
                //}
                //else if (countMarks > countDesc)
                //{
                //    for (int i = 0; i < countMarks; i++)
                //    {
                //        qty.Append('\n');
                //        desc.Append('\n');
                //        weight.Append('\n');
                //        volume.Append('\n');
                //    }
                //    marks.Append('\n');
                //    marks.Append('\n');
                //    qty.Append('\n');
                //    weight.Append('\n');
                //    volume.Append('\n');
                //}
                //else if (countMarks == countDesc)
                //{
                //    for (int i = 0; i <= countMarks; i++)
                //    {
                //        qty.Append('\n');
                //        weight.Append('\n');
                //        volume.Append('\n');
                //    }
                //    desc.Append('\n');
                //    desc.Append('\n');
                //    marks.Append('\n');
                //    marks.Append('\n');
                //}

                //newDetail.MarksAndNumbers += marks.ToString();
                //newDetail.PackageQtyKind += qty.ToString();
                //newDetail.Weight += weight.ToString();
                //newDetail.Volume += volume.ToString();
                //newDetail.DescriptionOfGoods += desc.ToString();
                #endregion
            }

        }

        private string BuildDescriptionOfGoods()
        {
            List<ShipmentPackagePM> shipmentPackagesList = GetPackagesList();
            StringBuilder strGoods = new StringBuilder();
            strGoods.Append(master.DescriptionOfGoods != null ? master.DescriptionOfGoods : "");

            if (!string.IsNullOrEmpty(master.SLAC))
            {
                if (!string.IsNullOrEmpty(strGoods.ToString()))
                {
                    strGoods.Append(Environment.NewLine);
                }

                strGoods.Append("SLAC: " + master.SLAC);
            }

            if (master.ShipmentTypeName == "FCL" || master.ShipmentTypeName == "LCL")
            {
                strGoods.Append(Environment.NewLine);
                strGoods.Append(master.ShipmentTypeName);
                strGoods.Append(Environment.NewLine);
                
                for (int i = 0; i < shipmentPackagesList.Count; i++)
                {
                    strGoods.Append(shipmentPackagesList[i].ContainerNumber != null ? "CNT " + shipmentPackagesList[i].ContainerNumber : "");
                    strGoods.Append(Environment.NewLine);
                }
            }
            return strGoods.ToString();
        }

        private List<ShipmentPackagePM> GetPackagesList()
        {
            return packagesQuery.GetShipmentPackages(master.Id, master.ShipmentNumber, tenant);
        }

        private void GetOtherCharges(string shipmentId, int tenant, ref double totalPrepaidString, ref double totalCollectString) //test
        {
            IShipmentsContext shipmentsContext = ShipmentsContext.GetContext(tenant);
            List<ShipmentReceivable> receivables = shipmentsContext.ShipmentReceivables.Include("ChargesType").Where(d => d.ShipmentId == shipmentId && d.Tenant == tenant).ToList();
            List<ShipmentPayable> payables = shipmentsContext.ShipmentPayables.Include("ChargesType").Where(d => d.ShipmentId == shipmentId && d.Tenant == tenant).ToList();
            List<ShipmentAWBPrintOnly> shipmentAWBPrintOnlies = shipmentsContext.ShipmentAWBPrintOnlies.Where(d => d.ShipmentId == shipmentId && d.Tenant == tenant).ToList();
            double totalPrepaid = 0;
            double totalCollect = 0;

            #region Receivables
            foreach (ShipmentReceivable receivable in receivables)
            {
                if (receivable.AWBPrint)
                {
                    if (receivable.ChargesType.ChargesGroupCode != "FRT")
                    {
                        if (receivable.AWBPrint && receivable.PrepaidCollectId == "P")
                        {
                            totalPrepaid = totalPrepaid + (receivable.TotalAmount != null ? receivable.TotalAmount.Value : 0);
                        }
                        if (receivable.AWBPrint && receivable.PrepaidCollectId == "C")
                        {
                            totalCollect = totalCollect + (receivable.TotalAmount != null ? receivable.TotalAmount.Value : 0);
                        }
                    }
                }
            }
            #endregion

            #region Payables
            foreach (ShipmentPayable payable in payables)
            {
                if (payable.AWBPrint)
                {
                    if (payable.ChargesType.ChargesGroupCode != "FRT")
                    {
                        if (payable.AWBPrint && payable.PrepaidCollectId == "P")
                        {
                            totalPrepaid = totalPrepaid + (payable.ExpectedAmount != null ? payable.ExpectedAmount.Value : 0);
                        }
                        if (payable.AWBPrint && payable.PrepaidCollectId == "C")
                        {
                            totalCollect = totalCollect + (payable.ExpectedAmount != null ? payable.ExpectedAmount.Value : 0);
                        }
                    }
                }
            }
            #endregion

            #region awb only
            foreach (ShipmentAWBPrintOnly awbOnly in shipmentAWBPrintOnlies)
            {
                if (awbOnly.PrepaidCollectId == "P")
                {
                    totalPrepaid = totalPrepaid + (awbOnly.Amount != null ? awbOnly.Amount.Value : 0);
                }

                if (awbOnly.PrepaidCollectId == "C")
                {
                    totalCollect = totalCollect + (awbOnly.Amount != null ? awbOnly.Amount.Value : 0);
                }
            }
            #endregion

            totalPrepaidString = totalPrepaid;
            totalCollectString = totalCollect;
        }

        private int countLines(string str)
        {
            if (!string.IsNullOrEmpty(str))
            {
                int counter = 1;
                string[] strTemp = str.Split(new string[] { "\r" }, StringSplitOptions.None);
                if (strTemp.Length > 0)
                {
                    counter = strTemp.Length;
                }
                return counter;
            }
            else return 0;
        }
        private string GetConsigneAddress(string id, int tenat, AddressRepository addressRepository)
        {
            Address consigneeAdderss = addressRepository.GetSingleAddress(id, tenat);
            string address = "";
            if (consigneeAdderss != null)
            {
                address = DataProviders.General.GetAddress(consigneeAdderss);
                if (!string.IsNullOrEmpty(consigneeAdderss.PhoneNumber) || !string.IsNullOrEmpty(consigneeAdderss.FaxNumber))
                {
                    address += System.Environment.NewLine;
                    address = (!string.IsNullOrEmpty(consigneeAdderss.PhoneNumber)) ? address + "Tel: " + consigneeAdderss.PhoneNumber + " " : address;
                    address = (!string.IsNullOrEmpty(consigneeAdderss.FaxNumber)) ? address + "Fax: " + consigneeAdderss.FaxNumber : address;
                }
            }
            return address;
        }
    }

    public class NewTemplateClass
    {
        public string MarksAndNumbers { get; set; }
        public int? Quantity { get; set; }
        public string DescriptionOfGoods { get; set; }
        public double? Weight { get; set; }
        public double? Volume { get; set; }
        public string ContainerNumber { get; set; }

        public string ShipmentId { get; set; }
        //public string ShipperName { get; set; }
        //public string ShipperAddress { get; set; }
        //public string ConsigneeName { get; set; }
        //public string ConsigneeAddress { get; set; }
        //public string PortOfDischarge { get; set; }
        //public string PC { get; set; }
    }
}
