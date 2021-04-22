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
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;

using WebFreight.Web.DataProviders;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using WebFreight.Web.Helpers;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.Helpers;
using Logitude.Server.Tools.Helpers;

namespace WebFreight.Web.ReportsWebServices
{
    /// <summary>  
    /// Summary description for DeliveryNoteWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line. 
    // [System.Web.Script.Services.ScriptService]
    public class DeliveryNoteWebService : System.Web.Services.WebService
    {
        private int tenant;
        private string entityId;
        private string childEntityId;
        private string entityObjectTableId;
        private string childEntityObjectTableId;
        private string childEntityTypeCode;
        private ShipmentPM shipment;
        private ShipmentPickUpDelivery childEntity;
        private DeliveryNoteDataProvider dataProvider;
        private IShipmentsContext shipmentsContext;
        private ICommonDataContext commonContext;
        private PortRepository portRepository;
        private AddressRepository addressRepository;       
        private byte[] output;
        private BranchRepository branchRepository;

        [WebMethod]
        public byte[] GetPickupData(string entityId, string entityObjectTableId, string childEntityId, string childEntityObjectTableId, int tenant)
        {
            this.tenant = tenant;
            this.entityId = entityId;
            this.childEntityId = childEntityId;
            this.entityObjectTableId = entityObjectTableId;
            this.childEntityObjectTableId = childEntityObjectTableId;
            this.childEntityTypeCode = "PICK";

            this.Initialize();
            this.BuildDataProvider();
            this.FormatDataProvider();
            this.SerializeDataProvider();

            return output;
        }

        [WebMethod]
        public byte[] GetDeliveryData(string entityId, string entityObjectTableId, string childEntityId, string childEntityObjectTableId, int tenant)
        {
            this.tenant = tenant;
            this.entityId = entityId;
            this.childEntityId = childEntityId;
            this.entityObjectTableId = entityObjectTableId;
            this.childEntityObjectTableId = childEntityObjectTableId;
            this.childEntityTypeCode = "DELV";

            this.Initialize();
            this.BuildDataProvider();
            this.SerializeDataProvider();

            return output;
        }

        private void Initialize()
        {
            this.dataProvider = new DeliveryNoteDataProvider()
            {
                TotalNumberOfPackages = 0,
                TotalGrossWeight = 0,
                TotalVolume = 0,
            };

            this.shipmentsContext = ShipmentsContext.GetContext(tenant);
            this.commonContext = CommonDataContext.GetContext(tenant);
            this.portRepository = new PortRepository(commonContext);
            this.addressRepository = new AddressRepository(commonContext);
            this.branchRepository = new BranchRepository(tenant);
            ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentsContext);
            ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRepository);

            shipment = shipmentQuery.GetSinglePM(entityId, tenant);

            childEntity = (from a in shipmentsContext.ShipmentPickUpDeliveries
                           where a.Tenant == tenant
                           && a.Id == childEntityId
                           && a.ShipmentId == entityId
                           && a.PickUpDeliveryTypeCode == childEntityTypeCode
                           select a).FirstOrDefault();
        }
        private void BuildDataProvider()
        {
            if (shipment != null)
            {
                this.MapHeaderFields();
                this.MapTenantFields();
                this.MapLoggedContact();
                this.MapShipmentFields();
                this.MapChildEntityFields();
            }
        }
        private void FormatDataProvider()
        {
            PropertyInfo[] properties = dataProvider.GetType().GetProperties();

            foreach (PropertyInfo pi in properties)
            {
                string propertytype = pi.PropertyType.FullName;

                if (propertytype == "System.String")
                {
                    if (pi.GetValue(dataProvider, null) == null || pi.GetValue(dataProvider, null).ToString() == "0" || pi.GetValue(dataProvider, null).ToString() == "00.00")
                    {
                        pi.SetValue(dataProvider, "", null);
                    }
                }
            }
        }
        private void SerializeDataProvider()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(DeliveryNoteDataProvider));
            MemoryStream memstream = new MemoryStream();
            serializer.Serialize(memstream, dataProvider);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            output = memstream.ToArray();
        }

        private void MapHeaderFields()
        {
            dataProvider.DateSent = String.Format("{0:dd/MMM/yyyy}", DateTime.Now.Date);
            dataProvider.DateSent_New = DateTime.Now.Date;
        }
        private void MapTenantFields()
        {
            Tenant tenantSettings = (from a in commonContext.Tenants where a.Id == tenant select a).FirstOrDefault();

            if (tenantSettings != null)
            {
                dataProvider.Signature = tenantSettings.Signature != null ? tenantSettings.Signature : "";
                dataProvider.TenantName = tenantSettings.Company != null ? tenantSettings.Company : "";
            }
        }
        private void MapLoggedContact()
        {
            ContactQuery contactQuery = new ContactQuery(tenant);
            string email = AuthenticationUtil.GetLoggedUserEmail(tenant);
            ContactPM loggedContact = contactQuery.GetContactByEmailOnly(email, tenant);

            if (loggedContact != null)
            {
                dataProvider.UserName = loggedContact.EnglishName;
                dataProvider.UserPhoneNumber = loggedContact.BusinessPhone;
            }
        }

        private void MapShipmentFields()
        {
            dataProvider.ITNumber = DataProviders.General.GetFieldString(shipment.ITNumber);
            dataProvider.ProjectNumber = DataProviders.General.GetFieldString(shipment.ProjectNumber);
            dataProvider.HAWB = DataProviders.General.GetFieldString(shipment.House);
            dataProvider.OurReferenceNumber = DataProviders.General.GetFieldString(shipment.ShipmentNumber);
            dataProvider.MainCarriageCarrierName = shipment.MainCarriageCarrierName;
            dataProvider.BookingNumber = shipment.BookingConfirmationNumber;
            dataProvider.MasterNumber = shipment.Master;
            dataProvider.ShipmentNotes = shipment.Notes;
            dataProvider.LongMaster = shipment.LongMaster;
            dataProvider.LastFreeDate = shipment.WarehouseLegLastFreeDate;
            dataProvider.StorageFreeDays = shipment.WarehouseStorageFreeDays;
            dataProvider.WarehouseReferenceNumber = shipment.WarehouseLegReference;
            dataProvider.FinalDestinationCode = shipment.MainCarriageFinalDestinationPortCode;
            dataProvider.AMSBL = shipment.AMSBL;
            dataProvider.DescriptionOfGoods = DataProviders.General.GetFieldString(shipment.DescriptionOfGoods);
            dataProvider.ContainersNumbersArray = shipment.ContainersNumbers;
            dataProvider.IncotermName = shipment.IncotermName;
            dataProvider.DeclarationNumber = shipment.DeclarationNumber;
            dataProvider.CustomsClearancePointName = shipment.CustomClearancePointName;
            dataProvider.ValueOfGoods = shipment.ValueOfGoods;
            dataProvider.MainCarriageCarrierNumber = shipment.MainCarriageCarrierNumber;
            dataProvider.CarrierCode = shipment.MainCarriageCarrierCode;

            MapBranchData();

            if (shipment.ValueOfGoodsCurrencyId != null)
            {
                Currency currency = commonContext.Currencies.Where(d => d.Id == shipment.ValueOfGoodsCurrencyId).FirstOrDefault();
                if (currency != null)
                {
                    dataProvider.ValueOfGoodsCurrency = currency.Code;
                }
            }

            this.MapShipmentFrom();
            this.MapShipmentTo();
            this.MapShipmentDates();
            this.MapShipmentCutoffDate();
            this.MapShipmentFreightLocation();
            this.MapShipmentOnCarriage();
            this.MapShipmentOnForwarding();
            this.MapShipmentShipper();
            this.MapShipmentConsignee();
            this.MapShipmentCustomer();
            this.MapShipmentIssuingCarrierAgent();
            this.MapShipmentFreightForwarder();
            this.MapShipmentSalesmanUser();
            this.MapShipmentMainVessel();
            this.MapShipmentLastVessel();
            this.MapShipmentMoveType();
            this.MapShipmentInsidePackages();
            this.MapShipmentCustomFields();
        }
        private void MapBranchData()
        {
            if (!string.IsNullOrEmpty(shipment.BranchId))
            {
                Branch branch = branchRepository.GetSingleBranch(shipment.BranchId, tenant);
                if (branch != null)
                {
                    dataProvider.BranchName = branch.EnglishName;
                    dataProvider.BranchLocalName = branch.LocalName;

                    if (!string.IsNullOrEmpty(branch.AddressId))
                    {
                        Address branchAddress = addressRepository.GetSingleAddress(branch.AddressId, tenant);
                        dataProvider.BranchAddress = DataProviders.General.GetAddress(branchAddress);
                    }
                }
            }
        }
        private void MapShipmentFrom()
        {
            if (!string.IsNullOrEmpty(shipment.MainCarriageFromPortId))
            {
                dataProvider.LoadingPortName = shipment.MainCarriageFromPortName;
                dataProvider.LoadingPortCode = shipment.MainCarriageFromPortCode;
                dataProvider.OriginCountry = shipment.MainCarriageFromPortCountryName;
            }
        }
        private void MapShipmentTo()
        {
            if (!string.IsNullOrEmpty(shipment.Transshipment3ToPortId))
            {
                dataProvider.DischargePortName = shipment.Transshipment3ToPortName;
                dataProvider.DischargePortCode = shipment.Transshipment3ToPortCode;
            }

            else if (!string.IsNullOrEmpty(shipment.Transshipment2ToPortId))
            {
                dataProvider.DischargePortName = shipment.Transshipment2ToPortName;
                dataProvider.DischargePortCode = shipment.Transshipment2ToPortCode;

            }

            else if (!string.IsNullOrEmpty(shipment.Transshipment1ToPortId))
            {
                dataProvider.DischargePortName = shipment.Transshipment1ToPortName;
                dataProvider.DischargePortCode = shipment.Transshipment1ToPortCode;

            }

            else if (!string.IsNullOrEmpty(shipment.MainCarriageToPortId))
            {
                dataProvider.DischargePortName = shipment.MainCarriageToPortName;
                dataProvider.DischargePortCode = shipment.MainCarriageToPortCode;
            }
        }
        private void MapShipmentDates()
        {
            dataProvider.MainCarriageETA = shipment.MainCarriageETA != null ? String.Format("{0:dd MMM yyyy}", shipment.MainCarriageETA) : "";
            dataProvider.MainCarriageETD = shipment.MainCarriageETD != null ? String.Format("{0:dd MMM yyyy}", shipment.MainCarriageETD) : "";
            dataProvider.MainCarriageETD_DateTime = shipment.MainCarriageETD;
            dataProvider.MainCarriageETA_DateTime = shipment.MainCarriageETA;
            dataProvider.MainCarriageATA_DateTime = shipment.MainCarriageATA;
        }
        private void MapShipmentCutoffDate()
        {
            if (shipment.CutoffDate != null)
            {
                dataProvider.CutOffDateAsDate = shipment.CutoffDate;
                dataProvider.CutOffDate = String.Format("{0:dd MMM yyyy}", shipment.CutoffDate);
                dataProvider.CutOffTime = String.Format("{0:hh:mm:ss}", shipment.CutoffDate);
            }
        }
        private void MapShipmentFreightLocation()
        {
            if (!string.IsNullOrEmpty(shipment.FreightLocationId))
            {
                Card card = CardRepository.GetSingleCard(shipment.FreightLocationId, tenant, true);
                if (card != null)
                {
                    dataProvider.FreightLocation = card.EnglishName;
                }
            }
        }
        private void MapShipmentOnCarriage()
        {
            if (shipment.OnCarriageFromPortId != null && shipment.OnCarriageToPortId != null)
            {
                if (shipment.OnCarriageCarrierId != null)
                {
                    dataProvider.OnCarriageCarrier = shipment.OnCarriageCarrierName;
                }
            }
        }
        private void MapShipmentOnForwarding()
        {
            if (shipment.OnForwardingFromPortId != null && shipment.OnForwardingToPortId != null)
            {
                if (shipment.OnForwardingCarrierId != null)
                {
                    dataProvider.OnForwardingCarrier = shipment.OnForwardingCarrierName;
                }
            }
        }
        private void MapShipmentShipper()
        {
            if (!string.IsNullOrEmpty(shipment.ShipperId))
            {
                this.MapShipperReferences();
                this.MapShipperAddress();
                this.MapShipperContact();
                this.MapShipperCardInfo();
            }
        }
        private void MapShipperReferences()
        {
            dataProvider.ClientReferenceNumber = shipment.CustomerReference1 != null ? shipment.CustomerReference1 : "";
            dataProvider.ShipperReference2 = shipment.ShipperReference2;
            dataProvider.ShipperReference1 = shipment.ShipperReference1;
        }
        private void MapShipperAddress()
        {
            if (!string.IsNullOrEmpty(shipment.ShipperAddressId))
            {
                Address address = addressRepository.GetSingleAddress(shipment.ShipperAddressId, tenant);
                if (address != null)
                {
                    dataProvider.ShipperAddress = DataProviders.General.GetAddress(address);

                    if (!string.IsNullOrEmpty(address.PhoneNumber) || !string.IsNullOrEmpty(address.FaxNumber))
                    {
                        dataProvider.ShipperAddress += Environment.NewLine;
                    }

                    if (!string.IsNullOrEmpty(address.PhoneNumber))
                    {
                        dataProvider.ShipperAddress += "Tel: " + address.PhoneNumber + " ";
                    }

                    if (!string.IsNullOrEmpty(address.FaxNumber))
                    {
                        dataProvider.ShipperAddress += "Fax: " + address.FaxNumber;
                    }
                }
            }
        }
        private void MapShipperContact()
        {
            if (!string.IsNullOrEmpty(shipment.ShipperContactId))
            {
                Contact contact = ContactRepository.GetSingleContact(shipment.ShipperContactId, tenant, true);
                if (contact != null)
                {
                    dataProvider.ShipperContactName = contact.EnglishName;
                    dataProvider.ShipperContactMobileNumber = contact.Mobile;
                }
            }
        }
        private void MapShipperCardInfo()
        {
            Card card = CardRepository.GetSingleCard(shipment.ShipperId, tenant, true);
            if (card != null)
            {
                dataProvider.ShipperName = card.EnglishName;
                dataProvider.ShipperVATNumber = card.VatNumber;
            }
        }

        private void MapShipmentConsignee()
        {
            if (!string.IsNullOrEmpty(shipment.ConsigneeId))
            {
                dataProvider.ConsigneeRef1 = shipment.ConsigneeReference1;
                dataProvider.ConsigneeReference2 = shipment.ConsigneeReference2;

                Card card = CardRepository.GetSingleCard(shipment.ConsigneeId, tenant, true);
                if (card != null)
                {
                    dataProvider.ConsigneeName = card.EnglishName;
                    dataProvider.ConsigneeVATNumber = card.VatNumber;

                    if (!string.IsNullOrEmpty(shipment.ConsigneeAddressId))
                    {
                        Address address = addressRepository.GetSingleAddress(shipment.ConsigneeAddressId, tenant);
                        if (address != null)
                        {
                            dataProvider.ConsigneeAddress = DataProviders.General.GetAddress(address);

                            if (!string.IsNullOrEmpty(address.PhoneNumber) || !string.IsNullOrEmpty(address.FaxNumber))
                            {
                                dataProvider.ConsigneeAddress += Environment.NewLine;
                            }

                            if (!string.IsNullOrEmpty(address.PhoneNumber))
                            {
                                dataProvider.ConsigneeAddress += "Tel: " + address.PhoneNumber + " ";
                            }

                            if (!string.IsNullOrEmpty(address.FaxNumber))
                            {
                                dataProvider.ConsigneeAddress += "Fax: " + address.FaxNumber;
                            }
                        }
                    }
                }
            }
        }
        private void MapShipmentCustomer()
        {
            if (!string.IsNullOrEmpty(shipment.CustomerId))
            {
                if (!string.IsNullOrEmpty(shipment.CustomerContactId))
                {
                    Contact contact = ContactRepository.GetSingleContact(shipment.CustomerContactId, tenant, true);
                    if (contact != null)
                    {
                        dataProvider.CustomerContactName = contact.EnglishName;
                        dataProvider.CustomerContactPhoneNumber = contact.BusinessPhone;
                    }
                }
            }
        }
        private void MapShipmentIssuingCarrierAgent()
        {
            if (!string.IsNullOrEmpty(shipment.IssuingCarrierAgentId))
            {
                Card card = CardRepository.GetSingleCard(shipment.IssuingCarrierAgentId, tenant, true);
                if (card != null)
                {
                    dataProvider.IssuingCarrierAgentName = card.EnglishName;

                    if (!string.IsNullOrEmpty(shipment.IssuingCarrierAddressId))
                    {
                        Address address = addressRepository.GetSingleAddress(shipment.IssuingCarrierAddressId, tenant);
                        if (address != null)
                        {
                            dataProvider.IssuingCarrierAgentAddress = DataProviders.General.GetAddress(address);

                            if (!string.IsNullOrEmpty(address.PhoneNumber) || !string.IsNullOrEmpty(address.FaxNumber))
                            {
                                dataProvider.IssuingCarrierAgentAddress += Environment.NewLine;
                            }

                            if (!string.IsNullOrEmpty(address.PhoneNumber))
                            {
                                dataProvider.IssuingCarrierAgentAddress += "Tel: " + address.PhoneNumber + " ";
                            }

                            if (!string.IsNullOrEmpty(address.FaxNumber))
                            {
                                dataProvider.IssuingCarrierAgentAddress += "Fax: " + address.FaxNumber;
                            }
                        }
                    }
                }
            }
        }
        private void MapShipmentFreightForwarder()
        {
            if (!string.IsNullOrEmpty(shipment.FreightForwarderId))
            {
                Card card = CardRepository.GetSingleCard(shipment.FreightForwarderId, tenant, true);

                if (card != null)
                {
                    dataProvider.ForwarderAgentCode = card.Code;
                    dataProvider.ForwarderAgentAddress = card.EnglishName != null ? card.EnglishName + Environment.NewLine : "";

                    if (!string.IsNullOrEmpty(shipment.FreightForwarderAddressId))
                    {
                        Address myPartnerAddress = addressRepository.GetSingleAddress(shipment.FreightForwarderAddressId, tenant);

                        if (myPartnerAddress != null)
                        {
                            if (myPartnerAddress.IsLocalLanguage && !string.IsNullOrEmpty(card.LocalName))
                            {
                                dataProvider.ForwarderAgentAddress = card.LocalName + Environment.NewLine;
                            }

                            dataProvider.ForwarderAgentAddress = dataProvider.ForwarderAgentAddress + DataProviders.General.GetAddress(myPartnerAddress);

                            if (myPartnerAddress.PhoneNumber != null || myPartnerAddress.FaxNumber != null)
                            {
                                dataProvider.ForwarderAgentAddress = dataProvider.ForwarderAgentAddress + Environment.NewLine + (myPartnerAddress.PhoneNumber != null ? "Tel: " + myPartnerAddress.PhoneNumber + " " : "") + (myPartnerAddress.FaxNumber != null ? "Fax: " + myPartnerAddress.FaxNumber + " " : "");
                            }
                        }
                    }
                }
            }
        }
        private void MapShipmentSalesmanUser()
        {
            if (!string.IsNullOrEmpty(shipment.SalesmanUserId))
            {
                dataProvider.ShipmentSalesman = shipment.SalesmanUserName;

                Contact salesmanContact = ContactRepository.GetSingleContact(shipment.SalesmanUserId, tenant, true);
                if (salesmanContact != null)
                {
                    dataProvider.SalesmanEmail = salesmanContact.Email;
                }
            }
        }
        private void MapShipmentMainVessel()
        {
            if (!string.IsNullOrEmpty(shipment.MainCarriageVesselId))
            {
                Vessel vessel = this.GetVessel(shipment.MainCarriageVesselId);
                if (vessel != null)
                {
                    dataProvider.MainCarriageVesselName = vessel.EnglishName;
                    dataProvider.MainCarriageVesselNameAndNumber = vessel.EnglishName + " \\ " + shipment.MainCarriageCarrierNumber;
                }
            }
        }
        private void MapShipmentLastVessel()
        {
            string lastVesselId = shipment.Transshipment3FromPortId;

            if (lastVesselId == null)
            {
                lastVesselId = shipment.Transshipment2FromPortId;
            }

            if (lastVesselId == null)
            {
                lastVesselId = shipment.Transshipment1FromPortId;
            }

            if (lastVesselId == null)
            {
                lastVesselId = shipment.MainCarriageVesselId;
            }

            if (lastVesselId != null)
            {
                Vessel vessel = this.GetVessel(lastVesselId);
                if (vessel != null)
                {
                    dataProvider.LastMainCarriageVesselNameAndNumber = vessel.EnglishName + " \\ " + shipment.MainCarriageCarrierNumber;
                }
            }
        }
        private void MapShipmentMoveType()
        {
            if (!string.IsNullOrEmpty(shipment.MoveTypeId))
            {
                IWebFreightContext webfreightcontext = WebFreightContext.GetContext(tenant);

                MoveType moveType = webfreightcontext.MoveTypes.Where(m => m.Id == shipment.MoveTypeId).FirstOrDefault();

                if (moveType != null)
                {
                    dataProvider.MoveTypeCode = moveType.Code;
                    dataProvider.MoveTypeName = moveType.MoveTypeEnglishName;
                }
            }
        }
        private void MapShipmentInsidePackages()
        {
            foreach (ShipmentPackagePM shipmentPackagePM in shipment.ShipmentPackages)
            {
                foreach (InsideShipmentPackagePM insideShipmentPackagePM in shipmentPackagePM.InsideShipmentPackages)
                {
                    dataProvider.InsidePackagesLines.Add(GetInsidePackageLine(insideShipmentPackagePM));
                }
            }           
        }
        private void MapShipmentCustomFields()
        {
            CustomFieldResolver customFieldResolver = new CustomFieldResolver();
            customFieldResolver.SetDataProviderCustomFieldsValues("Shipment", tenant, shipment, dataProvider);
        }
              
        private void MapChildEntityFields()
        {
            if (this.childEntity != null)
            {
                dataProvider.DriverName = childEntity.Driver;
                dataProvider.TruckNumber = childEntity.TruckNumber;
                dataProvider.TruckerNumber = childEntity.CarrierNumber;
                dataProvider.SpecialInstructions = childEntity.Notes != null ? childEntity.Notes : "";
                dataProvider.PickupDeliveryNumber = childEntity.PickUpDeliveryNumber;

                this.MapChildEntityFrom();
                this.MapChildEntityTo();
                this.MapChildEntityDates();
                this.MapChildEntityCarrier();
                this.MapChildEntityEmptyContainer();
                this.MapChildEntityShipmentPackage();
                this.MapChildEntityTransportMode();
                this.MapChildEntityDocumentType();
                this.MapChildEntityPackages();
            }
        }
        private void MapChildEntityFrom()
        {
            switch (childEntity.PickUpDeliveryFromTypeCode)
            {
                case "PORT":
                    {
                        this.MapChildEntityFromPort();
                        break;
                    }

                case "CASL":
                    {
                        this.MapChildEntityFromCasual();
                        break;
                    }

                case "PART":
                    {
                        this.MapChildEntityFromPartner();
                        break;
                    }

                case "WARH":
                    {
                        break;
                    }
            }
        }
        private void MapChildEntityFromPort()
        {
            if (!string.IsNullOrEmpty(childEntity.FromPortId))
            {
                Port port = portRepository.GetSinglePort(tenant, childEntity.FromPortId);

                if (port != null)
                {
                    dataProvider.PickupCompanyName = port.EnglishName != null ? port.EnglishName : "";
                }
            }

            dataProvider.PickupAddress = childEntity.FromAddress != null ? childEntity.FromAddress : "";
        }
        private void MapChildEntityFromCasual()
        {
            string location = "";

            if (!string.IsNullOrEmpty(childEntity.FromAddressCity))
            {
                location = childEntity.FromAddressCity;
            }

            if (!string.IsNullOrEmpty(childEntity.FromAddressZipCode))
            {
                location = location + " " + childEntity.FromAddressZipCode;
            }

            if (!string.IsNullOrEmpty(childEntity.FromAddressCountryId))
            {
                Country country = CountryRepository.GetSingleCountry(childEntity.FromAddressCountryId, tenant, true);
                if (country != null)
                {
                    location = location + Environment.NewLine + country.EnglishName;
                }
            }

            dataProvider.PickupAddress = location;
        }
        private void MapChildEntityFromPartner()
        {
            if (string.IsNullOrEmpty(childEntity.FromPartnerCardId))
            {
                dataProvider.PickupAddress = childEntity.FromAddress != null ? childEntity.FromAddress : "";
            }

            else
            {
                Card card = CardRepository.GetSingleCard(childEntity.FromPartnerCardId, tenant, true);
                if (card != null)
                {
                    dataProvider.PickupCompanyName = card.EnglishName;

                    CardContact cardContact = (from a in commonContext.CardContacts where a.CardId == card.Id select a).FirstOrDefault();
                    if (cardContact != null)
                    {
                        Contact contact = cardContact.Contact;

                        if (contact != null)
                        {
                            dataProvider.PickupContactPhone = contact.BusinessPhone != null ? contact.BusinessPhone : "";
                        }
                    }

                    if (string.IsNullOrEmpty(childEntity.FromAddressId))
                    {
                        dataProvider.PickupAddress = childEntity.FromAddress != null ? childEntity.FromAddress : "";
                    }

                    else
                    {
                        Address address = addressRepository.GetSingleAddress(childEntity.FromAddressId, tenant);
                        if (address != null)
                        {
                            dataProvider.PickupAddress = DataProviders.General.GetAddress(address)
                                + (!string.IsNullOrEmpty(address.ATTN) ? (Environment.NewLine + "Contact : " + address.ATTN) : "")
                                + (!string.IsNullOrEmpty(address.PhoneNumber) ? (Environment.NewLine + "Phone : " + address.PhoneNumber) : "");

                            dataProvider.FromAddressDescription = address.Description != null ? address.Description : "";
                        }
                    }
                }
            }
        }
        private void MapChildEntityTo()
        {
            switch (childEntity.PickUpDeliveryToTypeCode)
            {
                case "PORT":
                    {
                        this.MapChildEntityToPort();
                        break;
                    }

                case "CASL":
                    {
                        this.MapChildEntityToCasual();
                        break;
                    }

                case "PART":
                    {
                        this.MapChildEntityToPartner();
                        break;
                    }

                case "WARH":
                    {
                        break;
                    }
            }
        }
        private void MapChildEntityToPort()
        {
            if (!string.IsNullOrEmpty(childEntity.ToPortId))
            {
                Port port = portRepository.GetSinglePort(tenant, childEntity.ToPortId);

                if (port != null)
                {
                    dataProvider.DeliveryCompanyName = port.EnglishName != null ? port.EnglishName : "";
                    dataProvider.DeliveryCompanyLocalName = port.LocalName != null ? port.LocalName : "";
                }
            }

            dataProvider.DeliveryAddress = childEntity.ToAddress != null ? childEntity.ToAddress : "";
        }
        private void MapChildEntityToCasual()
        {
            string location = "";

            if (!string.IsNullOrEmpty(childEntity.ToAddressCity))
            {
                location = childEntity.ToAddressCity;
            }

            if (!string.IsNullOrEmpty(childEntity.ToAddressZipCode))
            {
                location = location + " " + childEntity.ToAddressZipCode;
            }

            if (!string.IsNullOrEmpty(childEntity.ToAddressCountryId))
            {
                Country country = CountryRepository.GetSingleCountry(childEntity.ToAddressCountryId, tenant, true);

                if (country != null)
                {
                    location = location + Environment.NewLine + country.EnglishName;
                }
            }

            dataProvider.DeliveryAddress = location;
        }
        private void MapChildEntityToPartner()
        {
            if (string.IsNullOrEmpty(childEntity.ToPartnerCardId))
            {
                dataProvider.DeliveryAddress = childEntity.ToAddress != null ? childEntity.ToAddress : "";
            }

            else
            {
                Card card = CardRepository.GetSingleCard(childEntity.ToPartnerCardId, tenant, true);
                if (card != null)
                {
                    dataProvider.DeliveryCompanyName = card.EnglishName;
                    dataProvider.DeliveryCompanyLocalName = card.LocalName;
                    dataProvider.DeliveryCompanyContactLocalName = card.LocalName;

                    Contact primaryContact = ContactRepository.GetSingleContact(card.PrimaryContactId, tenant, false);
                    if (primaryContact != null)
                    {
                        dataProvider.DeliveryCompanyContactLocalName = primaryContact.LocalName;
                        dataProvider.DeliveryCompanyContactPhone = primaryContact.BusinessPhone == null ? "" : primaryContact.BusinessPhone;
                    }

                    CardContact cardContact = (from a in commonContext.CardContacts where a.CardId == card.Id select a).FirstOrDefault();
                    if (cardContact != null)
                    {
                        ContactRepository contactRepository = new ContactRepository(commonContext);
                        Contact contact = contactRepository.GetSingleContact(cardContact.ContactId, tenant);
                        if (contact != null)
                        {
                            dataProvider.DeliveryContactPhone = contact.BusinessPhone != null ? contact.BusinessPhone : "";
                            dataProvider.DeliveryContactName = contact.EnglishName != null ? contact.EnglishName : "";
                        }
                    }

                    if (string.IsNullOrEmpty(childEntity.ToAddressId))
                    {
                        dataProvider.DeliveryAddress = childEntity.ToAddress != null ? childEntity.ToAddress : "";
                    }

                    else
                    {
                        Address address = addressRepository.GetSingleAddress(childEntity.ToAddressId, tenant);
                        if (address != null)
                        {
                            dataProvider.ToPartnerAddressName = address.Name;

                            dataProvider.DeliveryAddress = DataProviders.General.GetAddress(address)
                                + (!string.IsNullOrEmpty(address.ATTN) ? (Environment.NewLine + "Contact : " + address.ATTN) : "")
                                + (!string.IsNullOrEmpty(address.PhoneNumber) ? (Environment.NewLine + "Phone : " + address.PhoneNumber) : "");
                            dataProvider.ToAddressDescription = address.Description != null ? address.Description : "";

                        }
                    }
                }
            }
        }
        private void MapChildEntityDates()
        {
            if (childEntity.ETD != null)
            {
                dataProvider.PickupDate = String.Format("{0:dd/MMM/yyyy}", childEntity.ETD);
                dataProvider.PickupTime = String.Format("{0:hh:mm}", childEntity.ETD);
                dataProvider.PickupTime_DateTime_New = childEntity.ETD;
            }

            if (childEntity.ETA != null)
            {
                dataProvider.DeliveryETADate = childEntity.ETA;
                dataProvider.DeliveryETATime = childEntity.ETA;
                dataProvider.DeliveryDate = String.Format("{0:dd/MMM/yyyy}", childEntity.ETA);
                dataProvider.DeliveryTime = String.Format("{0:hh:mm}", childEntity.ETA);
                dataProvider.DeliveryTime_DateTime_New = childEntity.ETA;
            }

            dataProvider.PickupDeliveryDeparture = this.GetActualOrExpectedDeparture();
            dataProvider.PickupDeliveryArrival = this.GetActualOrExpectedArrival();           
        }
        private DateTime? GetActualOrExpectedDeparture()
        {
            DateTime? myDate = childEntity.ATD;
            if (myDate == null)
            {
                myDate = childEntity.ETD;
            }

            return myDate;
        } 
        private DateTime? GetActualOrExpectedArrival()
        {
            DateTime? myDate = childEntity.ATA;
            if (myDate == null)
            {
                myDate = childEntity.ETA;
            }

            return myDate;
        }
        private void MapChildEntityCarrier()
        {
            if (childEntity.CarrierId != null)
            {
                Card card = CardRepository.GetSingleCard(childEntity.CarrierId, tenant, true);

                if (card != null)
                {
                    dataProvider.To = card.EnglishName != null ? card.EnglishName : "";
                    dataProvider.TruckerName = dataProvider.To;

                    Address address = addressRepository.GetSingleAddressByCardIdAndTypeId(card.Id, "M", tenant);
                    if (address != null)
                    {
                        dataProvider.Telephone = address != null ? (address.PhoneNumber != null ? address.PhoneNumber : "") : "";
                    }

                    if (!string.IsNullOrEmpty(card.PrimaryContactId))
                    {
                        Contact primaryContact = ContactRepository.GetSingleContact(card.PrimaryContactId, tenant, true);
                        if(primaryContact != null)
                        {
                            dataProvider.TruckerCompanyContactName = primaryContact.EnglishName;
                        }
                    }

                    CardContact cardContact = (from cc in commonContext.CardContacts where cc.CardId == card.Id select cc).FirstOrDefault();
                    if (cardContact != null)
                    {
                        Contact contact = ContactRepository.GetSingleContact(cardContact.ContactId, tenant, true);
                        if(contact != null)
                        {
                            dataProvider.Salesman = contact.EnglishName != null ? contact.EnglishName : "";
                            dataProvider.SalesmanEmail = contact.Email != null ? contact.Email : "";
                        }
                    }
                }
            }
        }
        private void MapChildEntityEmptyContainer()
        {
            if (this.childEntityTypeCode == "PICK")
            {
                dataProvider.EmptyContainerRef = childEntity.EmptyPickupDepotReference;
                dataProvider.EmptyContainer = this.GetEmptyContainer(childEntity.EmptyPickupContainerPartnerId);
            }

            else
            {
                dataProvider.EmptyContainerReturnRef = childEntity.EmptyDeliveryDepotReference;
                dataProvider.EmptyContainerReturn = this.GetEmptyContainer(childEntity.EmptyDeliveryContainerPartnerId);
            }
        }
        private void MapChildEntityShipmentPackage()
        {
            ShipmentPackage shipmentPackage = (from a in shipmentsContext.ShipmentPackages where a.DeliveryId == childEntityId && a.Tenant == tenant select a).FirstOrDefault();

            if (shipmentPackage != null)
            {
                dataProvider.Reference1 = shipmentPackage.Reference1;
                dataProvider.Reference2 = shipmentPackage.Reference2;
                dataProvider.Reference3 = shipmentPackage.Reference3;
                dataProvider.Reference4 = shipmentPackage.Reference4;
            }
        }
        private void MapChildEntityTransportMode()
        {
            if (childEntity.TransportModeCode != null)
            {
                PickUpDeliveryTransportMode myTransportMode = shipmentsContext.PickUpDeliveryTransportModes.Where(d => d.Code == childEntity.TransportModeCode).FirstOrDefault();
                if (myTransportMode != null)
                {
                    dataProvider.TransportMode = myTransportMode.Name;
                }
            }
        }
        private void MapChildEntityDocumentType()
        {
            if (this.childEntityTypeCode == "PICK")
            {
                DocumentType documentType = commonContext.DocumentTypes.Where(doc => doc.Code == "781" && doc.Tenant == tenant).FirstOrDefault();
                if (documentType != null)
                {
                    DocumentTypeTemplate template = commonContext.DocumentTypeTemplates.Where(doc => doc.Id == documentType.DocumentTypeDefaultReportTemplateId && doc.Tenant == tenant).FirstOrDefault();
                    if (template != null)
                    {
                        dataProvider.VerticalShift = template.VerticalShift != null ? template.VerticalShift.Value : 10;
                        dataProvider.HorizontalShift = template.HorizontalShift != null ? template.HorizontalShift.Value : 10;
                    }
                }
            }
        }
        private void MapChildEntityPackages()
        {
            List<ShipmentPickUpDeliveryPackage> packages = shipmentsContext.ShipmentPickUpDeliveryPackages.Where(d => d.ShipmentPickUpDeliveryId == childEntityId && d.Tenant == tenant).ToList();

            if (packages.Count > 0)
            {
                dataProvider.TotalNumberOfPackages = packages.Sum(s => s.Quantity);
                dataProvider.TotalGrossWeight = packages.Sum(s => s.Weight);
                dataProvider.TotalVolume = packages.Sum(s => s.Volume);

                foreach (ShipmentPickUpDeliveryPackage item in packages)
                {
                    PackageLine itemLine = new PackageLine()
                    {
                        PackageDescriptionOfGoods = DataProviders.General.GetFieldString(item.Description),
                        SealNumber = DataProviders.General.GetFieldString(item.ShipperSeal),
                        ContainerNumber = DataProviders.General.GetFieldString(item.ContainerNumber),
                        Width = DataProviders.General.GetFieldString(item.Width),
                        Height = DataProviders.General.GetFieldString(item.Height),
                        Length = DataProviders.General.GetFieldString(item.Length),
                        PackageQuantity = DataProviders.General.GetFieldString(item.Quantity),
                        PackageGrossWeight = DataProviders.General.GetFieldString(item.Weight, shipment.GrossWeightUnitCode),
                        PackageVolume = DataProviders.General.GetFieldString(item.Volume, shipment.VolumeUnitCode),
                        Dimensions = this.GetDimensions(item),
                        Make = item.Make,
                        Model = item.Model,
                        Year = item.Year,
                        Color = item.Color,
                        ChassisNumber = item.ChassisNumber,
                        RegistrationNumber = item.RegistrationNumber,
                    };

                    if (!string.IsNullOrEmpty(item.CountryId))
                    {
                        Country country = CountryRepository.GetSingleCountry(item.CountryId, tenant, true);
                        if (country != null)
                        {
                            itemLine.CountryName = country.EnglishName;
                        }
                    }

                    this.MapChildEntityPackageType(itemLine, item);
                    this.MapChildEntityPackageHSCode(itemLine, item);

                    ShipmentPackagePM shipmentPackagePM = shipment.ShipmentPackages.Where(d => d.ContainerNumber == item.ContainerNumber).FirstOrDefault();

                    if (shipmentPackagePM != null)
                    {
                        foreach (InsideShipmentPackagePM insideShipmentPackagePM in shipmentPackagePM.InsideShipmentPackages)
                        {
                            itemLine.InsidePackagesLines.Add(GetInsidePackageLine(insideShipmentPackagePM));
                        }
                    }

                    dataProvider.PackagesLines.Add(itemLine);
                }
            }
        }
        private void MapChildEntityPackageType(PackageLine itemLine, ShipmentPickUpDeliveryPackage item)
        {
            if (item.PackageTypeId != null)
            {
                PackageType packageType = (from d in commonContext.PackageTypes where d.Id == item.PackageTypeId select d).FirstOrDefault();

                if (packageType != null)
                {
                    itemLine.ContainerSize = DataProviders.General.GetFieldString(packageType.ContainerSize);
                    itemLine.PackageType = packageType.EnglishName != null ? packageType.EnglishName : "Package";
                    itemLine.IsContainer = packageType.IsContainer;
                }
            }
        }
        private void MapChildEntityPackageHSCode(PackageLine itemLine, ShipmentPickUpDeliveryPackage item)
        {
            if (item.IsMultiHarmonize)
            {
                List<PickUpDeliveryPackageHarmonize> allHarmonizes = shipmentsContext.PickUpDeliveryPackageHarmonizes.Where(d => d.PackageId == item.Id && d.Tenant == tenant).ToList();

                foreach (PickUpDeliveryPackageHarmonize itemHarmonize in allHarmonizes)
                {
                    if (string.IsNullOrEmpty(itemLine.HSCode))
                    {
                        itemLine.HSCode = itemHarmonize.Harmonize;
                    }

                    else
                    {
                        itemLine.HSCode += "," + itemHarmonize.Harmonize;
                    }
                }
            }

            else
            {
                itemLine.HSCode = item.Harmonize;
            }
        }

        private Vessel GetVessel(string id)
        {
            Vessel output = null;

            if (!string.IsNullOrEmpty(id))
            {
                output = (from a in commonContext.Vessels where a.Id == id select a).FirstOrDefault();
            }

            return output;
        }
        private string GetEmptyContainer(string cardId)
        {
            string output = null;

            if (!string.IsNullOrEmpty(cardId))
            {
                Card card = CardRepository.GetSingleCard(cardId, tenant, true);
                if (card != null)
                {
                    output = card.EnglishName != null ? card.EnglishName : "";

                    Address address = addressRepository.GetSingleAddressByCardIdAndTypeId(card.Id, "M", tenant);

                    if (address != null)
                    {
                        if (address.IsLocalLanguage && !string.IsNullOrEmpty(card.LocalName))
                        {
                            output = card.LocalName;
                        }

                        output = output + Environment.NewLine + DataProviders.General.GetAddress(address);

                        if (address.PhoneNumber != null)
                        {
                            output = output + Environment.NewLine + "Phone No. : " + address.PhoneNumber;
                        }

                        if (address.FaxNumber != null)
                        {
                            output = output + "   Fax No. : " + address.FaxNumber;
                        }
                    }
                }
            }

            return output;
        }
        private InsidePackageLine GetInsidePackageLine(InsideShipmentPackagePM item)
        {
            InsidePackageLine itemLine = new InsidePackageLine()
            {
                Quantity = item.Quantity,
                Volume = item.Volume,
                VolumetricWeight = item.VolumetricWeight,
                Weight = item.Weight,
                Description = item.Description,
                Make = item.Make,
                Model = item.Model,
                Year = item.Year,
                Color = item.Color,
                ChassisNumber = item.ChassisNumber,
                RegistrationNumber = item.RegistrationNumber
            };

            if (!string.IsNullOrEmpty(item.PackageTypeId))
            {
                PackageType packageType = (from pa in commonContext.PackageTypes where pa.Id == item.PackageTypeId select pa).FirstOrDefault();
                if (packageType != null)
                {
                    itemLine.PackageType = packageType.EnglishName;
                }
            }

            if (item.Length != null && item.Width != null && item.Height != null)
            {
                itemLine.Dimensions = item.Length + "x" + item.Width + "x" + item.Height;
            }

            if (!string.IsNullOrEmpty(item.CountryId))
            {
                Country country = CountryRepository.GetSingleCountry(item.CountryId, tenant, true);
                if (country != null)
                {
                    itemLine.CountryName = country.EnglishName;
                }
            }

            return itemLine;
        }
        private string GetDimensions(ShipmentPickUpDeliveryPackage item)
        {
            string output = "";

            if (item.Width != null && item.Height != null && item.Length != null)
            {
                output = item.Length + " x " + item.Width + " x " + item.Height + " " + shipment.DimensionsUnitCode;
            }

            return output;
        }
    }
}
