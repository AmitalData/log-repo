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
using Simplog.Data.ShipmentsModel.Repositories;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.DataProviders;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using WebFreight.Web.Helpers;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel;
using System.Text;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.Helpers;
using System.Text.RegularExpressions;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using WebFreight.Web.WebServices;

namespace WebFreight.Web.ReportsWebServices
{
    /// <summary>
    /// Summary description for PreAlertWebService
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.  
    // [System.Web.Script.Services.ScriptService]
    public class PreAlertWebService : System.Web.Services.WebService
    {
        int tenant;
        string shipmentid;
        IShipmentsContext shipmentsContext;
        PreAlertDataProvider prealertDataProvider;
        TenantPM tenantpm;
        ICommonDataContext commonContext;
        WebServiceHelper serviceHelper;
        CountryRepository countryRepository;
        AddressRepository addressRepository;
        PortRepository portRepository;
        CardQuery cardQuery;
        private ShipmentPM shipmentpm;
        private ShipmentRepository shipmentRepository;
        private WebServiceHelper servicHelper;
        [WebMethod]
        public byte[] GetPreAlertData(string shipmentid, int tenant, string documentTypeId)
        {
            servicHelper = new WebServiceHelper(tenant);
            PreAlertDataProvider prealertDataProvider = GetPreAlertDataProvider(shipmentid, tenant, documentTypeId);

            #region Serialize and remove null region

            try
            {
                Type prealerttype = prealertDataProvider.GetType();

                PropertyInfo[] properties = prealerttype.GetProperties();

                foreach (PropertyInfo pi in properties)
                {
                    Type piType = pi.PropertyType;

                    if (piType.Name != "Double" && piType.Name != "List`1")
                    {
                        if (pi.GetValue(prealertDataProvider, null) == null || pi.GetValue(prealertDataProvider, null).ToString() == "0" || pi.GetValue(prealertDataProvider, null).ToString() == "00.00")
                        {
                            pi.SetValue(prealertDataProvider, "", null);
                        }
                    }
                }
            }
            catch { }

            XmlSerializer serializer = new XmlSerializer(typeof(PreAlertDataProvider));
            MemoryStream memstream = new MemoryStream();
            serializer.Serialize(memstream, prealertDataProvider);
            memstream.Seek(0, SeekOrigin.Begin);
            var reader = new StreamReader(memstream);
            string content = reader.ReadToEnd();
            byte[] bytearray = memstream.ToArray();

            return bytearray;

            #endregion
        }

        public PreAlertDataProvider GetPreAlertDataProvider(string shipmentid, int tenant, string documentTypeId)
        {
            this.tenant = tenant;
            this.shipmentid = shipmentid;
            prealertDataProvider = new PreAlertDataProvider();
            shipmentsContext = ShipmentsContext.GetContext(tenant);
            commonContext = CommonDataContext.GetContext(tenant);
            countryRepository = new CountryRepository(commonContext);
            cardQuery = new CardQuery(tenant);
            shipmentRepository = new ShipmentRepository(shipmentsContext);
            ShipmentQuery shipmentQuery = new ShipmentQuery(shipmentRepository);
            shipmentpm = shipmentQuery.GetSinglePM(shipmentid, tenant);

            TenantQuery tenantQuery = new TenantQuery(tenant);
            tenantpm = tenantQuery.GetSinglePM(tenant);
            serviceHelper = new WebServiceHelper(tenant);

            IWebFreightContext context = WebFreightContext.GetContext(tenant);

            if (shipmentpm != null)
            {
                this.MapMasterShipmentNumber();

                ContactRepository contactRepository = new ContactRepository(tenant);
                addressRepository = new AddressRepository(tenant);
                portRepository = new PortRepository(tenant);
                CardPM customer = cardQuery.GetSinglePM(shipmentpm.CustomerId, tenant);

                #region refences 
                prealertDataProvider.ShipperReference = shipmentpm.ShipperReference1;
                prealertDataProvider.CustomerReference = shipmentpm.CustomerReference1;
                prealertDataProvider.ConsigneeReference = shipmentpm.ConsigneeReference1;
                #endregion

                #region Customer + Customer's Contact
                if (!string.IsNullOrEmpty(shipmentpm.ConsigneeId))
                {
                    ContactQuery contactQuery = new ContactQuery(contactRepository);

                    CardPM consignee = cardQuery.GetSinglePM(shipmentpm.ConsigneeId, tenant);
                    if (consignee != null)
                    {
                        prealertDataProvider.Consignee = consignee.EnglishName;
                        ContactPM consigneeContact = contactQuery.GetSinglePM(shipmentpm.ConsigneeContactId, tenant);
                        if (consigneeContact != null)
                        {
                            prealertDataProvider.ConsigneeContactPhone = consigneeContact.BusinessPhone;
                            prealertDataProvider.ConsigneeContactName = consigneeContact.EnglishName;

                            Address consigneeContactAddress = addressRepository.GetSingleAddress(shipmentpm.ConsigneeAddressId, tenant);
                            if (consigneeContactAddress != null)
                            {
                                if (consigneeContactAddress.IsLocalLanguage)
                                {
                                    if (consignee != null && !string.IsNullOrEmpty(consignee.LocalName))
                                    {
                                        prealertDataProvider.Company = consignee.LocalName;
                                    }

                                    if (!string.IsNullOrEmpty(consigneeContact.LocalName))
                                    {
                                        prealertDataProvider.ConsigneeContactName = consigneeContact.LocalName;
                                    }
                                }
                                prealertDataProvider.ConsigneeAddress = DataProviders.General.GetAddress(consigneeContactAddress);
                            }
                        }
                    }

                    ContactPM customerContact = contactQuery.GetSinglePM(shipmentpm.CustomerContactId, tenant);
                    if (customerContact != null)
                    {
                        prealertDataProvider.ClientName = customerContact.EnglishName;

                        Address consigneeContactAddress = addressRepository.GetSingleAddress(shipmentpm.ConsigneeAddressId, tenant);
                        if (consigneeContactAddress != null)
                        {
                            if (consigneeContactAddress.IsLocalLanguage)
                            {
                                if (customer != null && !string.IsNullOrEmpty(customer.LocalName))
                                {
                                    prealertDataProvider.Company = customer.LocalName;
                                }

                                if (!string.IsNullOrEmpty(customerContact.LocalName))
                                {
                                    prealertDataProvider.ClientName = customerContact.LocalName;
                                }
                            }
                        }
                    }
                }
                #endregion

                if (customer != null)
                {
                    prealertDataProvider.Company = customer.EnglishName;
                    prealertDataProvider.IRSPlace = customer.IRSPlace;
                    prealertDataProvider.IRSNumber = customer.IRSNumber;
                }

                if (!string.IsNullOrEmpty(shipmentpm.BranchId))
                {
                    BranchRepository branchRepository = new BranchRepository(tenant);
                    Branch branch = branchRepository.GetSingleBranch(shipmentpm.BranchId, tenant);
                    if (branch != null)
                    {
                        prealertDataProvider.BranchSignature = branch.Signature;

                        if (!string.IsNullOrEmpty(branch.AddressId))
                        {
                            Address branchAddress = addressRepository.GetSingleAddress(branch.AddressId, tenant);
                            prealertDataProvider.BranchAddress = DataProviders.General.GetAddress(branchAddress);
                        }
                    }
                }

                prealertDataProvider.ProjectNumber = shipmentpm.ProjectNumber != null ? shipmentpm.ProjectNumber : "";
                Address customerAddress = addressRepository.GetSingleAddress(shipmentpm.CustomerAddressId, tenant);
                prealertDataProvider.ContactDetails = DataProviders.General.GetAddress(customerAddress);

                if (customerAddress != null)
                {
                    prealertDataProvider.CustomerAddress = DataProviders.General.GetAddress(customerAddress);

                    if (customerAddress.PhoneNumber != null || customerAddress.FaxNumber != null)
                    {
                        prealertDataProvider.CustomerAddress = prealertDataProvider.CustomerAddress + Environment.NewLine + (customerAddress.PhoneNumber != null ? "Tel: " + customerAddress.PhoneNumber + " " : "") + (customerAddress.FaxNumber != null ? "Fax: " + customerAddress.FaxNumber + " " : "");
                    }
                }
                prealertDataProvider.ChargeableWeightUnitCode = shipmentpm.ChargeableWeightUnitCode != null ? shipmentpm.ChargeableWeightUnitCode : "";
                prealertDataProvider.ChargeableWeight = shipmentpm.ChargeableWeight != null ? shipmentpm.ChargeableWeight != 0 ? (String.Format("{0:#,0.00}", shipmentpm.ChargeableWeight)) : "" : "";
                prealertDataProvider.MainIncoterm = shipmentpm.IncotermName;
                prealertDataProvider.OBLDate = shipmentpm.MAWBOBLDate;
                prealertDataProvider.ENSNumber = shipmentpm.ENSNumber;
                prealertDataProvider.ENSDate = shipmentpm.ENSDate;
                prealertDataProvider.FreightRelease = shipmentpm.FreightRelease;
                prealertDataProvider.TerminalAvailable = shipmentpm.TerminalAvailable;
                prealertDataProvider.ISFNumber = shipmentpm.ISFNumber;
                prealertDataProvider.ISFDate = shipmentpm.ISFDate;
                prealertDataProvider.ITNumber = shipmentpm.ITNumber;
                prealertDataProvider.ITDate = shipmentpm.ITDate;
                prealertDataProvider.BookingConfirmationNumber = shipmentpm.BookingConfirmationNumber;
                prealertDataProvider.IncotermCode = shipmentpm.IncotermCode;
                prealertDataProvider.ShipmentSubTypeName = shipmentpm.ShipmentSubTypeName;

                if (shipmentpm.DocumentsClosingDate != null)
                {
                    prealertDataProvider.DocumentsClosingDate = shipmentpm.DocumentsClosingDate;
                    prealertDataProvider.DocumentsClosingTime = shipmentpm.DocumentsClosingDate.Value.TimeOfDay;
                }

                if (!string.IsNullOrEmpty(shipmentpm.OBLTypeCode))
                {
                    OBLType type = shipmentsContext.OBLTypes.Where(d => d.Code == shipmentpm.OBLTypeCode).FirstOrDefault();

                    if (type != null)
                    {
                        prealertDataProvider.OBLType = type.Name;
                    }
                }

                #region Tenant Details
                string tenantAgent = null;

                if (tenantpm != null)
                {
                    prealertDataProvider.FMCNumber = tenantpm.FMCNumber;
                    tenantAgent = tenantpm.Company;
                    Address tenantAddress = addressRepository.GetSingleAddress(tenantpm.AddressId, tenant);

                    if (tenantAddress != null)
                    {
                        if (!string.IsNullOrEmpty(tenantAddress.City))
                        {
                            tenantAgent = tenantAgent + Environment.NewLine + tenantAddress.City;
                        }

                        if (tenantAddress.Country != null)
                        {
                            if (tenantAddress.IsLocalLanguage)
                            {
                                tenantAgent = tenantAgent + " " + tenantAddress.Country.Code + " " + tenantAddress.Country.LocalName;
                            }

                            else
                            {
                                tenantAgent = tenantAgent + " " + tenantAddress.Country.Code + " " + tenantAddress.Country.EnglishName;
                            }
                        }
                    }
                }
                #endregion

                #region Agent
                string shipmentAgent = null;
                if (!string.IsNullOrEmpty(shipmentpm.AgentId))
                {
                    CardPM agent = cardQuery.GetSinglePM(shipmentpm.AgentId, tenant);
                    if (agent != null)
                    {
                        shipmentAgent = agent.EnglishName;

                        Address agentAddress = addressRepository.GetSingleAddress(shipmentpm.AgentAddressId, tenant);

                        if (agentAddress != null)
                        {
                            if (agentAddress.IsLocalLanguage && !string.IsNullOrEmpty(agent.LocalName))
                            {
                                shipmentAgent = agent.LocalName;
                            }

                            shipmentAgent = shipmentAgent + Environment.NewLine + DataProviders.General.GetAddress(agentAddress);
                        }
                    }
                }
                #endregion

                if (shipmentpm.DirectionId == "E")
                {
                    prealertDataProvider.OriginAgent = tenantAgent;
                    prealertDataProvider.DestinationAgent = shipmentAgent;
                }

                else if (shipmentpm.DirectionId == "I")
                {
                    prealertDataProvider.OriginAgent = shipmentAgent;
                    prealertDataProvider.DestinationAgent = tenantAgent;
                }

                #region CustomAgent
                if (shipmentpm.DirectionId == "E" || shipmentpm.DirectionId == "D")
                {
                    if (!string.IsNullOrEmpty(shipmentpm.CustomAgentExportId))
                    {
                        CardPM customnAgent = cardQuery.GetSinglePM(shipmentpm.CustomAgentExportId, tenant);
                        if (customnAgent != null)
                        {
                            prealertDataProvider.CustomAgent = customnAgent.EnglishName;
                        }
                    }
                    else if (!string.IsNullOrEmpty(shipmentpm.CustomAgentImportId))
                    {
                        CardPM customnAgent = cardQuery.GetSinglePM(shipmentpm.CustomAgentImportId, tenant);
                        if (customnAgent != null)
                        {
                            prealertDataProvider.CustomAgent = customnAgent.EnglishName;
                        }
                    }
                    else
                    {
                        prealertDataProvider.CustomAgent = "";
                    }
                }
                else if (shipmentpm.DirectionId == "I")
                {
                    if (!string.IsNullOrEmpty(shipmentpm.CustomAgentImportId))
                    {
                        CardPM customnAgent = cardQuery.GetSinglePM(shipmentpm.CustomAgentImportId, tenant);
                        if (customnAgent != null)
                        {
                            prealertDataProvider.CustomAgent = customnAgent.EnglishName;
                        }
                    }
                    else if (!string.IsNullOrEmpty(shipmentpm.CustomAgentExportId))
                    {
                        CardPM customnAgent = cardQuery.GetSinglePM(shipmentpm.CustomAgentExportId, tenant);
                        if (customnAgent != null)
                        {
                            prealertDataProvider.CustomAgent = customnAgent.EnglishName;
                        }
                    }
                    else
                    {
                        prealertDataProvider.CustomAgent = "";
                    }
                }
                #endregion

                #region Shipper
                if (!string.IsNullOrEmpty(shipmentpm.ShipperId))
                {
                    CardPM shipper = cardQuery.GetSinglePM(shipmentpm.ShipperId, tenant);
                    if (shipper != null)
                    {
                        prealertDataProvider.Shipper = shipper.EnglishName;
                    }
                }
                #endregion                

                #region DeliveryDetails                
                ShipmentDeliveryQuery shipmentDeliveryQuery = new ShipmentDeliveryQuery(tenant);
                ShipmentDeliveryPM shipmentpickupdeliverypm = shipmentDeliveryQuery.GetShipmentDeliveryPMsByTenantAndShipment(shipmentid, tenant).Where(a => a.PickUpDeliveryNumber == shipmentpm.ShipmentNumber + "/" + shipmentpm.ShipmentDeliveryIndex).FirstOrDefault();

                if (shipmentpickupdeliverypm != null)
                {
                    if (!string.IsNullOrEmpty(shipmentpickupdeliverypm.ToAddressId))
                    {
                        Address DeliveryToAddress = addressRepository.GetSingleAddress(shipmentpickupdeliverypm.ToAddressId, tenant);
                        prealertDataProvider.DeliveryDetails = DataProviders.General.GetAddress(DeliveryToAddress);
                        prealertDataProvider.Destination = DataProviders.General.GetAddress(DeliveryToAddress);
                    }

                    else if (!string.IsNullOrEmpty(shipmentpickupdeliverypm.ToAddress))
                    {
                        prealertDataProvider.DeliveryDetails = shipmentpickupdeliverypm.ToAddress;
                        prealertDataProvider.Destination = shipmentpickupdeliverypm.ToAddress;
                    }

                    if (shipmentpickupdeliverypm.TransportModeCode != null)
                    {
                        PickUpDeliveryTransportMode myTransportMode = shipmentsContext.PickUpDeliveryTransportModes.Where(d => d.Code == shipmentpickupdeliverypm.TransportModeCode).FirstOrDefault();
                        if (myTransportMode != null)
                        {
                            prealertDataProvider.DeliveryTransportMode = myTransportMode.Name;
                        }
                    }
                }
                #endregion

                #region PickupDetails
                //get first pickup
                ShipmentPickUpQuery shipmentPickUpQuery = new ShipmentPickUpQuery(tenant);
                ShipmentPickUpPM shipmentpickupdeliverypm2 = shipmentPickUpQuery.GetShipmentPickUpPMsByTenantAndShipment(shipmentid, tenant).Where(a => a.PickUpDeliveryNumber == shipmentpm.ShipmentNumber + "/" + shipmentpm.ShipmentPickUpIndex).FirstOrDefault();

                if (shipmentpickupdeliverypm2 != null)
                {
                    if (!string.IsNullOrEmpty(shipmentpickupdeliverypm2.FromAddressId))
                    {
                        Address PickupToAddress = addressRepository.GetSingleAddress(shipmentpickupdeliverypm2.FromAddressId, tenant);
                        prealertDataProvider.PickupDetails = DataProviders.General.GetAddress(PickupToAddress);
                        prealertDataProvider.Origin = DataProviders.General.GetAddress(PickupToAddress);
                    }

                    else if (!string.IsNullOrEmpty(shipmentpickupdeliverypm2.FromAddress))
                    {
                        prealertDataProvider.PickupDetails = shipmentpickupdeliverypm2.FromAddress;
                        prealertDataProvider.Origin = shipmentpickupdeliverypm2.FromAddress;
                    }

                    //CuttOff 
                    prealertDataProvider.CutOffDate_DateTime = shipmentpm.CutoffDate;
                    if (shipmentpm.CutoffDate != null)
                    {
                        prealertDataProvider.CuttOffDateTime = String.Format("{0:dd MMM yyyy}", shipmentpm.CutoffDate);
                        prealertDataProvider.CuttOffTime = String.Format("{0:t}", shipmentpm.CutoffDate);
                    }

                    if (shipmentpickupdeliverypm2.TransportModeCode != null)
                    {
                        PickUpDeliveryTransportMode myTransportMode = shipmentsContext.PickUpDeliveryTransportModes.Where(d => d.Code == shipmentpickupdeliverypm2.TransportModeCode).FirstOrDefault();
                        if (myTransportMode != null)
                        {
                            prealertDataProvider.PickupTransportMode = myTransportMode.Name;
                        }
                    }
                }
                #endregion

                #region Shipment Fields

                string volumeUnitCode = shipmentpm.VolumeUnitCode != null ? shipmentpm.VolumeUnitCode : "";
                string grossWeightUnitCode = shipmentpm.GrossWeightUnitCode != null ? shipmentpm.GrossWeightUnitCode : "";

                prealertDataProvider.FileNumber = shipmentpm.ShipmentNumber;
                prealertDataProvider.OpenDate = String.Format("{0:dd MMM yyyy}", shipmentpm.CreateDateTime);
                prealertDataProvider.BookedBy = !string.IsNullOrEmpty(shipmentpm.BookingConfirmedBy) ? shipmentpm.BookingConfirmedBy : "";
                prealertDataProvider.Type = shipmentpm.TransportModeName + " " + shipmentpm.ShipmentTypeName;
                prealertDataProvider.Weight = shipmentpm.GrossWeight != null ? (shipmentpm.GrossWeight.ToString() + " " + grossWeightUnitCode) : "";
                prealertDataProvider.Volume = shipmentpm.Volume != null ? (shipmentpm.Volume.ToString() + " " + volumeUnitCode) : "";
                prealertDataProvider.MasterNumber = shipmentpm.Master != null ? shipmentpm.Master : "";
                prealertDataProvider.Carrier = shipmentpm.MainCarriageCarrierName != null ? shipmentpm.MainCarriageCarrierName : "";
                prealertDataProvider.FromPort = shipmentpm.FromPortName != null ? shipmentpm.FromPortName : "";
                prealertDataProvider.FinalPort = shipmentpm.ToPortName != null ? shipmentpm.ToPortName : "";
                prealertDataProvider.MainCarriageCarrierNumber = shipmentpm.MainCarriageCarrierNumber;
                prealertDataProvider.MainCarriageETA = shipmentpm.MainCarriageETA != null ? String.Format("{0:dd MMM yyyy}", shipmentpm.MainCarriageETA) : "";
                prealertDataProvider.MainCarriageETA_DateTime = shipmentpm.MainCarriageETA != null ? shipmentpm.MainCarriageETA : null;
                prealertDataProvider.MainCarriageETD = shipmentpm.MainCarriageETD != null ? String.Format("{0:dd MMM yyyy}", shipmentpm.MainCarriageETD) : "";
                prealertDataProvider.MainCarriageETD_DateTime = shipmentpm.MainCarriageETD != null ? shipmentpm.MainCarriageETD : null;
                prealertDataProvider.mainCarriageToPortCode = !string.IsNullOrEmpty(shipmentpm.MainCarriageToPortCode) ? shipmentpm.MainCarriageToPortCode : "";
                prealertDataProvider.mainCarriageToPortName = !string.IsNullOrEmpty(shipmentpm.MainCarriageToPortName) ? shipmentpm.MainCarriageToPortName : "";
                prealertDataProvider.GeneralDescriptionOfGoods = shipmentpm.DescriptionOfGoods != null ? shipmentpm.DescriptionOfGoods : "";
                prealertDataProvider.House = shipmentpm.House != null ? shipmentpm.House : "";

                if (shipmentpm.SpecialServicesTypeId != null)
                {
                    SpecialServicesTypeRepository specialServicesTypeRepository = new SpecialServicesTypeRepository(tenant);
                    SpecialServicesType specialServicesType = specialServicesTypeRepository.GetSingleSpecialServicesType(shipmentpm.SpecialServicesTypeId, tenant);
                    prealertDataProvider.SpecialServicesType = specialServicesType != null ? specialServicesType.EnglishName : "";
                }

                if (MethodHelper.IsLCLEntity(shipmentpm.TransportModeId, shipmentpm.ShipmentTypeId))
                {
                    if (shipmentpm.NumberOfPackages == null)
                    {
                        prealertDataProvider.PCS = "";
                    }

                    else
                    {
                        prealertDataProvider.PCS = shipmentpm.NumberOfPackages.Value.ToString() + " Packages ";
                    }
                }

                else
                {
                    prealertDataProvider.PCS = ((shipmentpm.NumberOfPackages != null ? (shipmentpm.NumberOfPackages.Value.ToString() + " Packages ") : "")) + ((shipmentpm.NumberOfContainers != null ? (shipmentpm.NumberOfContainers.Value.ToString() + " Containers") : ""));
                }

                PortQuery portQuery = new PortQuery(tenant);
                if (shipmentpm.ToPortId != null)
                {
                    PortPM toPortPortPM = portQuery.GetSinglePM(shipmentpm.ToPortId, tenant);
                    if (toPortPortPM != null)
                    {
                        prealertDataProvider.FinalPortCode = toPortPortPM.Code;
                        prealertDataProvider.FinalPort = toPortPortPM.EnglishName;
                    }
                }

                else if (shipmentpm.MainCarriageToPortId != null)
                {
                    PortPM mainCarriageToPort = portQuery.GetSinglePM(shipmentpm.MainCarriageToPortId, tenant);
                    if (mainCarriageToPort != null)
                    {
                        prealertDataProvider.FinalPortCode = mainCarriageToPort.Code;
                        prealertDataProvider.FinalPort = mainCarriageToPort.EnglishName;
                    }
                }

                if (shipmentpm.FromPortId != null)
                {
                    PortPM finalPortPortPM = portQuery.GetSinglePM(shipmentpm.FromPortId, tenant);
                    if (finalPortPortPM != null)
                    {
                        prealertDataProvider.FromPortCode = finalPortPortPM.Code;
                        prealertDataProvider.FromPort = finalPortPortPM.EnglishName;
                    }
                }
                else if (shipmentpm.MainCarriageFromPortId != null)
                {
                    PortPM mainCarriageFromPort = portQuery.GetSinglePM(shipmentpm.MainCarriageFromPortId, tenant);
                    if (mainCarriageFromPort != null)
                    {
                        prealertDataProvider.FromPortCode = mainCarriageFromPort.Code;
                        prealertDataProvider.FromPort = mainCarriageFromPort.EnglishName;
                    }
                }

                // Inland + Domestic
                if (shipmentpm.DirectionId == "D" && shipmentpm.TransportModeId == "I")
                {
                    InlandDomesticArgs args = new InlandDomesticArgs()
                    {
                        InlandDomesticFromTypeCode = shipmentpm.InlandDomesticFromTypeCode,
                        MainCarriageFromAddressId = shipmentpm.MainCarriageFromAddressId,
                        MainCarriageFromPortName = shipmentpm.MainCarriageFromPortName,
                        InlandDomesticFromCity = shipmentpm.InlandDomesticFromCity,
                        InlandDomesticFromCountryId = shipmentpm.InlandDomesticFromCountryId,
                        InlandDomesticToTypeCode = shipmentpm.InlandDomesticToTypeCode,
                        MainCarriageToAddressId = shipmentpm.MainCarriageToAddressId,
                        InlandDomesticToCity = shipmentpm.InlandDomesticToCity,
                        InlandDomesticToCountryId = shipmentpm.InlandDomesticToCountryId,
                        MainCarriageToPortName = shipmentpm.MainCarriageToPortName,
                    };
                    prealertDataProvider.FromLocation = servicHelper.GetInlandDomesticFromLocation(args);
                    prealertDataProvider.ToLocation = servicHelper.GetInlandDomesticToLocation(args);
                    prealertDataProvider.FinalLocation = prealertDataProvider.ToLocation;
                }
                else
                {
                    PortPM toPort = portQuery.GetSinglePM(shipmentpm.MainCarriageToPortId, tenant);
                    PortPM fromPort = portQuery.GetSinglePM(shipmentpm.MainCarriageFromPortId, tenant);

                    prealertDataProvider.FromLocation = fromPort != null ? fromPort.Code + " " + fromPort.EnglishName : "";
                    prealertDataProvider.ToLocation = toPort != null ? toPort.Code + " " + toPort.EnglishName : "";

                    Port finalDestination = (from a in commonContext.Ports
                                             where a.Id == shipmentpm.FinalDistenationPortId
                                             select a).FirstOrDefault();

                    prealertDataProvider.FinalLocation = finalDestination != null ? (finalDestination.Code + " " + finalDestination.EnglishName) : "";
                }

                //Transshipment1
                prealertDataProvider.Transshipment1CarrierNumber = shipmentpm.Transshipment1CarrierNumber != null ? shipmentpm.Transshipment1CarrierNumber : "";
                prealertDataProvider.Transshipment1CarrierName = shipmentpm.Transshipment1CarrierName != null ? shipmentpm.Transshipment1CarrierName : "";
                prealertDataProvider.Transshipment1Vessel = shipmentpm.Transshipment1VesselName;

                if (!string.IsNullOrEmpty(shipmentpm.Transshipment1VesselName))
                {                    
                    prealertDataProvider.Transshipment1CarrierNumber = shipmentpm.Transshipment1CarrierNumber != null ? (shipmentpm.Transshipment1VesselName + " / " + shipmentpm.Transshipment1CarrierNumber) : "";
                }

                prealertDataProvider.Transshipment1ETA = shipmentpm.Transshipment1ETA != null ? String.Format("{0:dd MMM yyyy}", shipmentpm.Transshipment1ETA) : "";
                prealertDataProvider.Transshipment1ETA_DateTime = shipmentpm.Transshipment1ETA != null ? shipmentpm.Transshipment1ETA : null;
                prealertDataProvider.Transshipment1ETD = shipmentpm.Transshipment1ETD != null ? String.Format("{0:dd MMM yyyy}", shipmentpm.Transshipment1ETD) : "";
                prealertDataProvider.Transshipment1ETD_DateTime = shipmentpm.Transshipment1ETD != null ? shipmentpm.Transshipment1ETD : null;
                prealertDataProvider.Transshipment1ToPortCode = shipmentpm.Transshipment1ToPortCode != null ? shipmentpm.Transshipment1ToPortCode : "";
                prealertDataProvider.Transshipment1ToPortName = shipmentpm.Transshipment1ToPortName != null ? shipmentpm.Transshipment1ToPortName : "";

                //Transshipment2
                prealertDataProvider.Transshipment2CarrierNumber = shipmentpm.Transshipment2CarrierNumber != null ? shipmentpm.Transshipment2CarrierNumber : "";
                prealertDataProvider.Transshipment2Vessel = shipmentpm.Transshipment2VesselName;

                if (!string.IsNullOrEmpty(shipmentpm.Transshipment2VesselName))
                {
                    prealertDataProvider.Transshipment2CarrierNumber = shipmentpm.Transshipment2CarrierNumber != null ? (shipmentpm.Transshipment2VesselName + " / " + shipmentpm.Transshipment2CarrierNumber) : "";
                }

                prealertDataProvider.Transshipment2ETA = shipmentpm.Transshipment2ETA != null ? String.Format("{0:dd MMM yyyy}", shipmentpm.Transshipment2ETA) : "";
                prealertDataProvider.Transshipment2ETA_DateTime = shipmentpm.Transshipment2ETA != null ? shipmentpm.Transshipment2ETA : null;
                prealertDataProvider.Transshipment2ETD = shipmentpm.Transshipment2ETD != null ? String.Format("{0:dd MMM yyyy}", shipmentpm.Transshipment2ETD) : "";
                prealertDataProvider.Transshipment2ETD_DateTime = shipmentpm.Transshipment2ETD != null ? shipmentpm.Transshipment2ETD : null;
                prealertDataProvider.Transshipment2ToPortCode = shipmentpm.Transshipment2ToPortCode != null ? shipmentpm.Transshipment2ToPortCode : "";
                prealertDataProvider.Transshipment2ToPortName = shipmentpm.Transshipment2ToPortName != null ? shipmentpm.Transshipment2ToPortName : "";

                //Transshipment3
                prealertDataProvider.Transshipment3CarrierNumber = shipmentpm.Transshipment3CarrierNumber != null ? shipmentpm.Transshipment3CarrierNumber : "";
                prealertDataProvider.Transshipment3Vessel = shipmentpm.Transshipment3VesselName;

                if (!string.IsNullOrEmpty(shipmentpm.Transshipment3VesselName))
                {                    
                    prealertDataProvider.Transshipment3CarrierNumber = shipmentpm.Transshipment3CarrierNumber != null ? (shipmentpm.Transshipment3VesselName + " / " + shipmentpm.Transshipment3CarrierNumber) : "";
                }

                prealertDataProvider.Transshipment3ETA = shipmentpm.Transshipment3ETA != null ? String.Format("{0:dd MMM yyyy}", shipmentpm.Transshipment3ETA) : "";
                prealertDataProvider.Transshipment3ETA_DateTime = shipmentpm.Transshipment3ETA != null ? shipmentpm.Transshipment3ETA : null;
                prealertDataProvider.Transshipment3ETD = shipmentpm.Transshipment3ETD != null ? String.Format("{0:dd MMM yyyy}", shipmentpm.Transshipment3ETD) : "";
                prealertDataProvider.Transshipment3ETD_DateTime = shipmentpm.Transshipment3ETD != null ? shipmentpm.Transshipment3ETD : null;
                prealertDataProvider.Transshipment3ToPortCode = shipmentpm.Transshipment3ToPortCode != null ? shipmentpm.Transshipment3ToPortCode : "";
                prealertDataProvider.Transshipment3ToPortName = shipmentpm.Transshipment3ToPortName != null ? shipmentpm.Transshipment3ToPortName : "";

                // Fill MainCarriageETA from last transhipment
                prealertDataProvider.FinalMainCarriageETA =
                    !string.IsNullOrEmpty(prealertDataProvider.Transshipment3ETA) ? prealertDataProvider.Transshipment3ETA :
                    (!string.IsNullOrEmpty(prealertDataProvider.Transshipment2ETA) ? prealertDataProvider.Transshipment2ETA :
                    (!string.IsNullOrEmpty(prealertDataProvider.Transshipment1ETA) ? prealertDataProvider.Transshipment1ETA : prealertDataProvider.MainCarriageETA)
                    );

                // Fill MainCarriageETD from first transhipment
                prealertDataProvider.FirstMainCarriageETD =
                    !string.IsNullOrEmpty(prealertDataProvider.Transshipment1ETD) ? prealertDataProvider.Transshipment1ETD :
                    (!string.IsNullOrEmpty(prealertDataProvider.Transshipment2ETD) ? prealertDataProvider.Transshipment2ETD :
                    (!string.IsNullOrEmpty(prealertDataProvider.Transshipment3ETD) ? prealertDataProvider.Transshipment3ETD : prealertDataProvider.MainCarriageETD)
                    );

                if (shipmentpm.TransportModeId == "A")
                {
                    //   prealertDataProvider.MasterNumber =  prealertDataProvider.MasterNumber 
                    prealertDataProvider.MasterNumber_Label = "M.A.W.B";
                    prealertDataProvider.Carrier_Label = "Airline";
                    prealertDataProvider.FromPort_Label = "Airport Of Departure";
                    prealertDataProvider.FinalPort_Label = "Airport Of Destination";
                    prealertDataProvider.MainCarriageCarrierNumber_Label = "Flight Number";
                    prealertDataProvider.ShippingDetails_FlightDetails = "Flight Details";

                    prealertDataProvider.MainCarriageCarrierNumber = shipmentpm.MainCarriageCarrierCode + prealertDataProvider.MainCarriageCarrierNumber;
                }

                else if (shipmentpm.TransportModeId == "O")
                {
                    prealertDataProvider.MasterNumber_Label = "O.B.L";
                    prealertDataProvider.Carrier_Label = "Shipping line";
                    prealertDataProvider.FromPort_Label = "Port Of Loading";
                    prealertDataProvider.FinalPort_Label = "Port Of Discharge";
                    prealertDataProvider.MainCarriageCarrierNumber_Label = "Vessel & Voyage";
                    prealertDataProvider.ShippingDetails_FlightDetails = "Shipping Details";

                    if (shipmentpm.MainCarriageVesselName != null)
                    {
                        prealertDataProvider.MainCarriageCarrierNumber = shipmentpm.MainCarriageVesselName + " / " + prealertDataProvider.MainCarriageCarrierNumber;

                    }
                }

                else if (shipmentpm.TransportModeId == "I")
                {
                    prealertDataProvider.MasterNumber_Label = "CMR/RWB#";
                    prealertDataProvider.Carrier_Label = "Trucker";

                    if (shipmentpm.DirectionId == "D")
                    {
                        prealertDataProvider.FromPort_Label = "Place of Loading";
                        prealertDataProvider.FinalPort_Label = "Place of Discharge";
                    }
                    else
                    {
                        prealertDataProvider.FromPort_Label = "Port of Loading";
                        prealertDataProvider.FinalPort_Label = "Port of Discharge";
                    }
                    prealertDataProvider.MainCarriageCarrierNumber_Label = "Carrier Number";
                    prealertDataProvider.ShippingDetails_FlightDetails = "Shipping Details";
                }
                #endregion

                #region Others

                string loggedUserEmail = AuthenticationUtil.GetLoggedUserEmail(tenant);
                if (!string.IsNullOrEmpty(loggedUserEmail))
                {
                    ContactQuery contactQuery = new ContactQuery(contactRepository);
                    ContactPM contactpm = contactQuery.GetContactByEmailOnly(loggedUserEmail, tenant);

                    if (contactpm != null)
                    {
                        prealertDataProvider.UserName = contactpm.EnglishName;
                        prealertDataProvider.Email = contactpm.Email != null ? contactpm.Email : "";
                    }
                }
                prealertDataProvider.TodayDate = String.Format("{0:dd MMM yyyy}", TenantServerConfigration.GetCurrentDateTime(tenant));
                #endregion

                #region Packages
                ShipmentPackageQuery shipmentpackageQuery = new ShipmentPackageQuery(tenant);
                List<ShipmentPackagePM> shipmentPackagesList = shipmentpackageQuery.GetShipmentPackages(shipmentid, shipmentpm.ShipmentNumber, tenant);
                List<Packages> packagesList = new List<Packages>();

                prealertDataProvider.Containers = "";

                string mySealNumber = "";
                string myContainerNumbers = "";
                string myTotalContainers = "";
                string alpha = "";
                string num = "";
                string alphaFormat = @"[^A-Za-z]*";
                string numericFormat = @"[^0-9]*";

                foreach (ShipmentPackagePM package in shipmentPackagesList)
                {
                    PackageType myPackageType = (from pa in commonContext.PackageTypes
                                                 where pa.Id == package.PackageTypeId
                                                 select pa).FirstOrDefault();

                    Packages masterpackage = new Packages();
                    masterpackage.Reference1 = package.Reference1;
                    masterpackage.Reference2 = package.Reference2;
                    masterpackage.Reference3 = package.Reference3;
                    masterpackage.CommodityNumber = package.CommodityNumber;
                    masterpackage.DescriptionOfGoods = package.Description;
                    masterpackage.NumberOfInsidePackages = package.NumberOfInsidePackages;
                    masterpackage.ContainerNumber = package.ContainerNumber;

                    if (package.IsDangerous)
                    {
                        masterpackage.DescriptionOfGoods +=
                            "CONTAINS DANGEROUS GOODS: " + Environment.NewLine +
                            (package.MaterialDescription != null ? package.MaterialDescription : "") +
                            " - Class: " + (package.ClassNumber != null ? package.ClassNumber : "") +
                            ", UN-N: " + (package.UnNumber != null ? package.UnNumber : "") +
                            ", PACKING GROUP " + (package.PackagingGroup != null ? package.PackagingGroup : "");
                    }

                    masterpackage.MarksAndNumbers = package.MarksAndNumbers;
                    masterpackage.PackageType = package.PackageTypeName;
                    masterpackage.Quantity = String.Format("{0:0}", package.Quantity);
                    masterpackage.Volume = String.Format("{0:#,0.00}", package.Volume);
                    masterpackage.Weight = String.Format("{0:#,0.00}", package.Weight);
                    masterpackage.WeightUnitCode = shipmentpm.GrossWeightUnitCode != null ? shipmentpm.GrossWeightUnitCode : "";
                    masterpackage.VolumeUnitCode = shipmentpm.VolumeUnitCode != null ? shipmentpm.VolumeUnitCode : "";

                    if (package.Width != null && package.Height != null && package.Length != null)
                    {
                        masterpackage.Dimensions = package.Length + " x " + package.Width + " x " + package.Height + " " + shipmentpm.DimensionsUnitCode;
                    }

                    if (!string.IsNullOrEmpty(package.ContainerNumber))
                    {
                        myContainerNumbers = string.IsNullOrEmpty(myContainerNumbers) ? package.ContainerNumber : myContainerNumbers + "," + package.ContainerNumber;
                        prealertDataProvider.Containers = prealertDataProvider.Containers + package.ContainerNumber + ",";
                    }

                    if (!string.IsNullOrEmpty(package.ShipperSeal))
                    {
                        mySealNumber = string.IsNullOrEmpty(mySealNumber) ? package.ShipperSeal : mySealNumber + "," + package.ShipperSeal;
                    }

                    if (myPackageType != null)
                    {
                        if (package.IsContainer)
                        {
                            alpha = Regex.Replace(myPackageType.Code, alphaFormat, string.Empty, RegexOptions.Compiled);
                            num = Regex.Replace(myPackageType.Code, numericFormat, string.Empty, RegexOptions.Compiled);
                            string itemText = package.Quantity.ToString() + " x " + num + "'" + alpha;
                            myTotalContainers = string.IsNullOrEmpty(myTotalContainers) ? itemText : myTotalContainers + ", " + itemText;
                        }
                    }

                    masterpackage.InsidePackagesLines = new List<InsidePackageLine>();
                    List<InsideShipmentPackage> insidePackages = shipmentsContext.InsideShipmentPackages.Where(d => d.ShipmentPackageId == package.Id && d.Tenant == package.Tenant).ToList();
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
                        insidePackage.Reference1 = insideItem.Reference1;
                        insidePackage.Reference2 = insideItem.Reference2;
                        insidePackage.Reference3 = insideItem.Reference3;
                        insidePackage.CommodityNumber = insideItem.CommodityNumber;

                        #region Car Details
                        insidePackage.Make = insideItem.Make;
                        insidePackage.Model = insideItem.Model;
                        insidePackage.Year = insideItem.Year;
                        insidePackage.Color = insideItem.Color;
                        insidePackage.ChassisNumber = insideItem.ChassisNumber;
                        insidePackage.RegistrationNumber = insideItem.RegistrationNumber;

                        if (!string.IsNullOrEmpty(insideItem.CountryId))
                        {
                            Country country = countryRepository.GetSingleCountry(insideItem.CountryId, tenant);
                            if (country != null)
                            {
                                insidePackage.CountryName = country.EnglishName;
                            }
                        }
                        #endregion

                        masterpackage.InsidePackagesLines.Add(insidePackage);
                    }

                    packagesList.Add(masterpackage);
                }

                prealertDataProvider.SealNumber = mySealNumber;
                prealertDataProvider.ContainerNumbers = myContainerNumbers;
                prealertDataProvider.TotalContainers = myTotalContainers;

                if (!string.IsNullOrEmpty(prealertDataProvider.Containers))
                {
                    prealertDataProvider.Containers = prealertDataProvider.Containers.Remove(prealertDataProvider.Containers.Length - 1, 1);
                }

                prealertDataProvider.PackagesList = packagesList;
                #endregion

                #region manifest details region
                DocumentTypeCustomFieldRepository documentTypeCustomFieldsRepository = new DocumentTypeCustomFieldRepository(tenant);
                FormCustomFieldRepository formCustomFieldRepository = new FormCustomFieldRepository(tenant);
                IncotermQuery incotermQuery = new IncotermQuery(tenant);
                List<ShipmentDataView> connectedShipments = shipmentRepository.GetShipmentViewsByTenantAndMasterId(shipmentid, tenant).ToList();
                List<FormCustomField> customfieldsList = formCustomFieldRepository.GetFormCustomFields(tenant).ToList();
                List<DocumentTypeCustomField> documentCustomfieldsList = documentTypeCustomFieldsRepository.GetDocumentTypeCustomFields(tenant).ToList();
                ShipmentPackageQuery shipmentPackagesQuery = new ShipmentPackageQuery(tenant);
                double grandTotalCollect = 0;
                double grandTotalPrepaid = 0;
                double totalWeight = 0;
                double totalVolume = 0;
                double totalPackagesQuantity = 0;
                double totalContainersQuantity = 0;

                foreach (ShipmentDataView shipmentView in connectedShipments)
                {
                    PreAlertManifestDetails detail = new PreAlertManifestDetails();

                    detail.FileNumber = shipmentView.ShipmentNumber;

                    string volume_UnitCode = shipmentView.VolumeUnitCode != null ? shipmentView.VolumeUnitCode : "";
                    detail.ChargeableWeightUnitCode = shipmentView.ChargeableWeightUnitCode != null ? shipmentView.ChargeableWeightUnitCode : "";

                    #region Shipper
                    CardPM shipper = cardQuery.GetSinglePM(shipmentView.ShipperId, tenant);

                    if (shipper != null)
                    {
                        detail.ShipperName = shipper.EnglishName;
                        Address shipperAdderss = addressRepository.GetSingleAddress(shipmentView.ShipperAddressId, tenant);
                        if (shipperAdderss != null)
                        {
                            if (shipperAdderss.IsLocalLanguage && !string.IsNullOrEmpty(shipper.LocalName))
                            {
                                detail.ShipperName = shipper.LocalName;
                            }

                            detail.ShipperAddress = DataProviders.General.GetAddress(shipperAdderss);
                        }
                    }
                    else
                    {
                        detail.ShipperName = "";
                        detail.ShipperAddress = "";
                    }
                    #endregion

                    #region Consignee
                    if (!string.IsNullOrEmpty(shipmentView.ConsigneeId))
                    {
                        CardPM consignee = cardQuery.GetSinglePM(shipmentView.ConsigneeId, tenant);

                        if (consignee != null)
                        {
                            detail.ConsigneeName = consignee.EnglishName;
                            Address consigneeAdderss = addressRepository.GetSingleAddress(shipmentView.ConsigneeAddressId, tenant);
                            if (consigneeAdderss != null)
                            {
                                if (consigneeAdderss.IsLocalLanguage && !string.IsNullOrEmpty(consignee.LocalName))
                                {
                                    detail.ConsigneeName = consignee.LocalName;
                                }

                                detail.ConsigneeAddress = DataProviders.General.GetAddress(consigneeAdderss);
                            }
                        }
                        else
                        {
                            detail.ConsigneeName = "";
                            detail.ConsigneeAddress = "";
                        }
                    }
                    else
                    {
                        detail.ConsigneeName = "";
                        detail.ConsigneeAddress = "";
                    }
                    #endregion

                    #region Notify
                    CardPM notify = cardQuery.GetSinglePM(shipmentView.Notify1Id, tenant);

                    if (notify != null)
                    {
                        detail.NotifyName = notify.EnglishName;
                        Address notifyAdderss = addressRepository.GetSingleAddress(shipmentView.Notify1AddressId, tenant);
                        if (notifyAdderss != null)
                        {
                            if (notifyAdderss.IsLocalLanguage && !string.IsNullOrEmpty(notify.LocalName))
                            {
                                detail.NotifyName = notify.LocalName;
                            }

                            detail.NotifyAddress = DataProviders.General.GetAddress(notifyAdderss);
                        }
                    }
                    else
                    {
                        detail.NotifyName = "";
                        detail.NotifyAddress = "";
                    }
                    #endregion

                    detail.Weight = shipmentView.GrossWeight != null ? shipmentView.GrossWeight != 0 ? (String.Format("{0:#,0.00}", shipmentView.GrossWeight) + " " + (shipmentView.GrossWeightUnitCode != null ? shipmentView.GrossWeightUnitCode : "")) : "" : "";
                    detail.ChargeableWeight = shipmentView.ChargeableWeight != null ? shipmentView.ChargeableWeight != 0 ? (String.Format("{0:#,0.00}", shipmentView.ChargeableWeight) + " " + (shipmentView.ChargeableWeightUnitCode != null ? shipmentView.ChargeableWeightUnitCode : "")) : "" : "";
                    detail.Volume = shipmentView.Volume != null ? shipmentView.Volume != 0 ? (shipmentView.Volume + " " + volume_UnitCode) : "" : "";

                    IncotermPM incoterm = incotermQuery.GetSinglePM(shipmentView.IncotermId, tenant);
                    detail.Incoterm = incoterm != null ? incoterm.Name : "";

                    detail.House = shipmentView.House != null ? shipmentView.House : "";

                    if (shipmentView.TransportModeId == "A")
                    {
                        double totalPrepaid = 0;
                        double totalCollect = 0;
                        this.GetOtherCharges(shipmentView.Id, tenant, ref totalPrepaid, ref totalCollect);
                        grandTotalCollect = grandTotalCollect + (shipmentView.AWBFreightAmountCollect != null ? shipmentView.AWBFreightAmountCollect.Value : 0) + (totalCollect);
                        grandTotalPrepaid = grandTotalPrepaid + (shipmentView.AWBFreightAmountPrepaid != null ? shipmentView.AWBFreightAmountPrepaid.Value : 0) + (totalPrepaid);
                        detail.HAWB = shipmentView.House;
                        detail.DestinationPortCode = shipmentView.MainCarriageFinalDestinationPortCode != null ? shipmentView.MainCarriageFinalDestinationPortCode : "";
                        detail.DestinationPortName = shipmentView.MainCarriageFinalDestinationPortName != null ? shipmentView.MainCarriageFinalDestinationPortName : "";
                        detail.Quantity = shipmentView.NumberOfPackages != null ? shipmentView.NumberOfPackages.ToString() : "";
                        totalPackagesQuantity = totalPackagesQuantity + (shipmentView.NumberOfPackages != null ? shipmentView.NumberOfPackages.Value : 0);

                        #region custom fields
                        //----Freight Cusotmfield---//
                        FormCustomField freightField = (from a in customfieldsList
                                                        where a.FieldCode == "IsFreight" && a.EntityId == shipmentid
                                                        select a).FirstOrDefault();
                        DocumentTypeCustomField freightDocumentCustom = (from a in documentCustomfieldsList
                                                                         where a.FieldCode == "IsFreight"
                                                                         select a).FirstOrDefault();

                        FormCustomField otherField = (from a in customfieldsList
                                                      where a.FieldCode == "IsOther" && a.EntityId == shipmentid
                                                      select a).FirstOrDefault();

                        DocumentTypeCustomField otherDocumentCustom = (from a in documentCustomfieldsList
                                                                       where a.FieldCode == "IsOther"
                                                                       select a).FirstOrDefault();

                        if (freightField != null)
                        {
                            if (freightField.Value == "True")
                            {
                                detail.Prepaid = "Freight: " + (shipmentView.AWBFreightAmountPrepaid != null ? shipmentView.AWBFreightAmountPrepaid.ToString() : "");
                                detail.Collect = "Freight: " + (shipmentView.AWBFreightAmountCollect != null ? shipmentView.AWBFreightAmountCollect.ToString() : "");
                                if (otherField != null)
                                {
                                    if (otherField.Value == "True")
                                    {
                                        detail.Prepaid = detail.Prepaid + Environment.NewLine + "Charges: " + totalPrepaid.ToString();
                                        detail.Collect = detail.Collect + Environment.NewLine + "Charges: " + totalCollect.ToString();
                                    }
                                    else
                                    {
                                        detail.Prepaid = detail.Prepaid + Environment.NewLine + "Charges: ";
                                        detail.Collect = detail.Collect + Environment.NewLine + "Charges: ";
                                    }
                                }
                                else
                                {
                                    detail.Prepaid = detail.Prepaid + Environment.NewLine + "Charges: ";
                                    detail.Collect = detail.Collect + Environment.NewLine + "Charges: ";
                                }
                            }
                            else
                            {
                                detail.Prepaid = "Freight: ";
                                detail.Collect = "Freight: ";
                            }
                        }
                        else if (freightDocumentCustom != null)
                        {
                            if (freightDocumentCustom.DefaultValue == "True")
                            {
                                detail.Prepaid = "Freight: " + (shipmentView.AWBFreightAmountPrepaid != null ? shipmentView.AWBFreightAmountPrepaid.ToString() : "");
                                detail.Collect = "Freight: " + (shipmentView.AWBFreightAmountCollect != null ? shipmentView.AWBFreightAmountCollect.ToString() : "");
                            }
                            else
                            {
                                detail.Prepaid = "Freight: ";
                                detail.Collect = "Freight: ";
                            }
                        }
                        #endregion
                    }
                    else
                    {
                        detail.PortOfDischarge = shipmentView.MainCarriageToPortCode;

                        totalPackagesQuantity = totalPackagesQuantity + (shipmentView.NumberOfPackages != null ? shipmentView.NumberOfPackages.Value : 0);
                        totalContainersQuantity = totalContainersQuantity + (shipmentView.NumberOfContainers != null ? shipmentView.NumberOfContainers.Value : 0);

                        # region packages region for kind and quantity
                        List<ShipmentPackagePM> packages = shipmentPackagesQuery.GetShipmentPackages(shipmentView.Id, shipmentView.ShipmentNumber, tenant);
                        string packageKinds = "";
                        string packageQty = "";
                        foreach (ShipmentPackagePM package in packages)
                        {
                            if (string.IsNullOrEmpty(packageKinds))
                            {
                                packageKinds = package.PackageTypeName;
                            }
                            else
                            {
                                packageKinds = packageKinds + Environment.NewLine + package.PackageTypeName;
                            }
                            if (string.IsNullOrEmpty(packageQty))
                            {
                                packageQty = (package.Quantity != null ? package.Quantity.ToString() : "");
                            }
                            else
                            {
                                packageQty = packageQty + Environment.NewLine + (package.Quantity != null ? package.Quantity.ToString() : "");
                            }
                        }

                        detail.PackageKind = packageKinds;
                        detail.PackageQuantity = packageQty;
                        #endregion
                    }

                    StringBuilder strGoods = new StringBuilder();
                    List<ShipmentPackagePM> PackagesList = shipmentPackagesQuery.GetShipmentPackages(shipmentView.Id, shipmentView.ShipmentNumber, tenant);
                    strGoods.Append(shipmentView.DescriptionOfGoods != null ? shipmentView.DescriptionOfGoods : "");

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

                    detail.DescriptionOfGoods = strGoods.ToString();

                    totalWeight = totalWeight + (shipmentView.GrossWeight != null ? shipmentView.GrossWeight.Value : 0);
                    totalVolume = totalVolume + (shipmentView.Volume != null ? shipmentView.Volume.Value : 0);

                    detail.PC = shipmentView.FreightPrepaidCollectId;

                    prealertDataProvider.PreAlertManifestDetails.Add(detail);
                }
                //prealertDataProvider.TotalCollect = grandTotalCollect.ToString();
                //prealertDataProvider.TotalPrepaid = grandTotalPrepaid.ToString();

                //if (totalPackagesQuantity != 0)
                //    prealertDataProvider.TotalQuantity = totalPackagesQuantity.ToString();// + " Pcs" + Environment.NewLine;

                //if (totalContainersQuantity != 0)
                //    prealertDataProvider.TotalQuantity += totalContainersQuantity.ToString();// +" Con";

                //prealertDataProvider.TotalVolume = totalVolume != 0 ? (String.Format("{0:#,0.00}", totalVolume) + " " + (prealertDataProvider.VolumeUnit)) : ""; //CBM
                //prealertDataProvider.TotalWeight = totalWeight != 0 ? (String.Format("{0:#,0.00}", totalWeight) + " " + (prealertDataProvider.WeightUnit)) : ""; //KGS 
                #endregion

                #region ReleasingAgent
                string myReleasingAgentId = shipmentpm.ReleasingAgentId;
                string myReleasingAgentAddressId = shipmentpm.ReleasingAgentAddressId;
                if (!string.IsNullOrEmpty(myReleasingAgentId))
                {
                    Card myPartnerCard = CardRepository.GetSingleCard(myReleasingAgentId, tenant, true);

                    if (myPartnerCard != null)
                    {
                        prealertDataProvider.ReleasingAgentAddress = myPartnerCard.EnglishName != null ? myPartnerCard.EnglishName + Environment.NewLine : "";
                        prealertDataProvider.ReleasingAgentName = myPartnerCard.EnglishName;
                        if (!string.IsNullOrEmpty(myReleasingAgentAddressId))
                        {
                            Address myPartnerAddress = addressRepository.GetSingleAddress(myReleasingAgentAddressId, tenant);

                            if (myPartnerAddress != null)
                            {
                                if (myPartnerAddress.IsLocalLanguage && !string.IsNullOrEmpty(myPartnerCard.LocalName))
                                {
                                    prealertDataProvider.ReleasingAgentAddress = myPartnerCard.LocalName + Environment.NewLine;
                                }

                                prealertDataProvider.ReleasingAgentAddress = prealertDataProvider.ReleasingAgentAddress + DataProviders.General.GetAddress(myPartnerAddress);

                                if (myPartnerAddress.PhoneNumber != null || myPartnerAddress.FaxNumber != null)
                                {
                                    prealertDataProvider.ReleasingAgentAddress = prealertDataProvider.ReleasingAgentAddress + Environment.NewLine + (myPartnerAddress.PhoneNumber != null ? "Tel: " + myPartnerAddress.PhoneNumber + " " : "") + (myPartnerAddress.FaxNumber != null ? "Fax: " + myPartnerAddress.FaxNumber + " " : "");
                                }
                            }
                        }
                    }
                }
                #endregion

                #region PlaceOfDelivery
                Port onCarriageToPort = null;
                Port onForwardingToPort = null;
                Port mainToPort = null;

                if (shipmentpm.OnCarriageToPortId != null)
                {
                    onCarriageToPort = (from a in commonContext.Ports where a.Id == shipmentpm.OnCarriageToPortId select a).FirstOrDefault();
                }

                if (shipmentpm.OnForwardingToPortId != null)
                {
                    onForwardingToPort = (from a in commonContext.Ports where a.Id == shipmentpm.OnForwardingToPortId select a).FirstOrDefault();
                }

                if (shipmentpm.MainCarriageToPortId != null)
                {
                    mainToPort = (from a in commonContext.Ports where a.Id == shipmentpm.MainCarriageToPortId select a).FirstOrDefault();
                }

                ShipmentPickUpDelivery myDelivery = (from d in shipmentsContext.ShipmentPickUpDeliveries
                                                     where d.ShipmentId == shipmentpm.Id && d.PickUpDeliveryTypeCode == "DELV"
                                                     select d).OrderBy(s => s.PickUpDeliveryNumber).FirstOrDefault();

                if (myDelivery != null)
                {
                    switch (myDelivery.PickUpDeliveryToTypeCode)
                    {
                        case "PART":
                            {
                                if (!string.IsNullOrEmpty(myDelivery.ToPartnerCardId))
                                {
                                    Address myPartnerAddress = addressRepository.GetMainAddressByCardId(myDelivery.ToPartnerCardId, tenant);
                                    if (myPartnerAddress != null)
                                    {
                                        prealertDataProvider.PlaceOfDelivery = myPartnerAddress.City;
                                    }
                                }

                                break;
                            }

                        case "PORT":
                            {
                                if (!string.IsNullOrEmpty(myDelivery.ToPortId))
                                {
                                    Port myPort = portRepository.GetSinglePort(tenant, myDelivery.ToPortId);
                                    if (myPort != null)
                                    {
                                        prealertDataProvider.PlaceOfDelivery = myPort.EnglishName;
                                    }
                                }

                                break;
                            }

                        case "CASL":
                            {
                                string myCity = myDelivery.ToAddressCity;
                                if (!string.IsNullOrEmpty(myCity))
                                {
                                    prealertDataProvider.PlaceOfDelivery = myCity;
                                }

                                break;
                            }
                    }
                }

                else if (onForwardingToPort != null)
                {
                    prealertDataProvider.PlaceOfDelivery = onForwardingToPort.EnglishName;
                }

                else if (onCarriageToPort != null)
                {
                    prealertDataProvider.PlaceOfDelivery = onCarriageToPort.EnglishName;
                }

                else
                {
                    if (shipmentpm.Transshipment3ToPortId != null)
                    {
                        prealertDataProvider.PlaceOfDelivery = shipmentpm.Transshipment3ToPortName;
                    }
                    else if (shipmentpm.Transshipment2ToPortId != null)
                    {
                        prealertDataProvider.PlaceOfDelivery = shipmentpm.Transshipment2ToPortName;
                    }
                    else if (shipmentpm.Transshipment1ToPortId != null)
                    {
                        prealertDataProvider.PlaceOfDelivery = shipmentpm.Transshipment1ToPortName;
                    }
                    else if (mainToPort != null)
                    {
                        prealertDataProvider.PlaceOfDelivery = mainToPort.EnglishName;
                    }
                }
                #endregion

                #region PickUpAddress
                Port preCarriageFromPort = null;
                Port preForwardingFromPort = null;
                Port mainFromPort = null;

                if (shipmentpm.PreCarriageFromPortId != null)
                {
                    preCarriageFromPort = (from a in commonContext.Ports where a.Id == shipmentpm.PreCarriageFromPortId select a).FirstOrDefault();
                }

                if (shipmentpm.PreForwardingFromPortId != null)
                {
                    preForwardingFromPort = (from a in commonContext.Ports where a.Id == shipmentpm.PreForwardingFromPortId select a).FirstOrDefault();
                }

                if (shipmentpm.MainCarriageFromPortId != null)
                {
                    mainFromPort = (from a in commonContext.Ports where a.Id == shipmentpm.MainCarriageFromPortId select a).FirstOrDefault();
                }

                ShipmentPickUpDelivery firstPickup =
                    (from d in shipmentsContext.ShipmentPickUpDeliveries
                     where d.ShipmentId == shipmentpm.Id && d.PickUpDeliveryTypeCode == "PICK"
                     select d).OrderBy(s => s.PickUpDeliveryNumber).FirstOrDefault();

                if (firstPickup != null)
                {
                    switch (firstPickup.PickUpDeliveryFromTypeCode)
                    {
                        case "PART":
                            {
                                if (!string.IsNullOrEmpty(firstPickup.FromPartnerCardId))
                                {
                                    Address myPartnerAddress = addressRepository.GetMainAddressByCardId(firstPickup.FromPartnerCardId, tenant);
                                    if (myPartnerAddress != null)
                                    {
                                        prealertDataProvider.PickUpAddress = myPartnerAddress.City;
                                        Country country = countryRepository.GetSingleCountry(myPartnerAddress.CountryId, tenant);
                                        if (country != null)
                                        {
                                            prealertDataProvider.PlaceOfReceiptCountryName = country.EnglishName;
                                        }
                                    }
                                }

                                break;
                            }

                        case "PORT":
                            {
                                if (!string.IsNullOrEmpty(firstPickup.FromPortId))
                                {
                                    Port myPort = portRepository.GetSinglePort(tenant, firstPickup.FromPortId);
                                    if (myPort != null)
                                    {
                                        prealertDataProvider.PickUpAddress = myPort.EnglishName;
                                        prealertDataProvider.PlaceOfReceiptCountryName = myPort.CountryName;

                                    }
                                }

                                break;
                            }

                        case "CASL":
                            {
                                string myCity = firstPickup.FromAddressCity;
                                if (!string.IsNullOrEmpty(myCity))
                                {
                                    prealertDataProvider.PickUpAddress = myCity;
                                    Country country = countryRepository.GetSingleCountry(firstPickup.FromAddressCountryId, tenant);
                                    if (country != null)
                                    {
                                        prealertDataProvider.PlaceOfReceiptCountryName = country.EnglishName;
                                    }
                                }

                                break;
                            }
                    }
                }

                else if (preForwardingFromPort != null)
                {
                    prealertDataProvider.PickUpAddress = preForwardingFromPort.EnglishName; 
                    prealertDataProvider.PlaceOfReceiptCountryName = preForwardingFromPort.CountryName;                    
                }

                else if (preCarriageFromPort != null)
                {
                    prealertDataProvider.PickUpAddress = preCarriageFromPort.EnglishName;                    
                    prealertDataProvider.PlaceOfReceiptCountryName = preCarriageFromPort.CountryName;                    
                }

                else if (mainFromPort != null)
                {
                    prealertDataProvider.PickUpAddress = mainFromPort.EnglishName;
                    Address myPartnerAddress = addressRepository.GetSingleAddress(shipmentpm.ShipperAddressId, tenant);
                    if (myPartnerAddress != null)
                    {
                        prealertDataProvider.PlaceOfReceiptCountryName = myPartnerAddress.Country == null ? null : myPartnerAddress.Country.EnglishName;
                    }
                }
                #endregion

                #region Assemblies
                if (shipmentpm.ShipmentAssemblies.Count > 0)
                {
                    prealertDataProvider.Assemblies = new List<ShipmentAssemblyLine>();

                    foreach (ShipmentAssemblyPM assembly in shipmentpm.ShipmentAssemblies)
                    {
                        prealertDataProvider.Assemblies.Add(new ShipmentAssemblyLine()
                        {
                            House = assembly.House,
                            ShipperName = assembly.ShipperName
                        });
                    }
                }
                #endregion

                #region Payables List
                GetShipmentPayaples();
                #endregion

                #region PickUpsAndDeliveries
                GetShipmentPickUpAndDeliveries();
                #endregion

                #region ProductItems
                ShipmentProductItemQuery shipmentProductItemQuery = new ShipmentProductItemQuery(tenant);
                List<ShipmentProductItemPM> shipmentProductItems = shipmentProductItemQuery.GetShipmentProductItems(shipmentid, tenant);                
                if (shipmentProductItems != null)
                {
                    List<ProductItemLine> productItemsLines = new List<ProductItemLine>();
                    ProductItemQuery productItemQuery = new ProductItemQuery(tenant);

                    foreach (ShipmentProductItemPM shipmentProductItem in shipmentProductItems)
                    {
                        ProductItemPM productItem = productItemQuery.GetSinglePM(shipmentProductItem.ProductItemId, tenant);
                        ProductItemLine productItemLine = new ProductItemLine();
              
                        if (productItem != null)
                        {
                            productItemLine.Name = productItem.Name;
                            productItemLine.Brand = productItem.Brand;
                            productItemLine.InActive = productItem.InActive;
                            productItemLine.Description = productItem.Description;
                            productItemLine.Tenant = productItem.Tenant;
                            productItemLine.SKU = productItem.SKU;
                            productItemLine.CustomerId = productItem.CustomerId;
                            productItemLine.ASIN = productItem.ASIN;
                            productItemLine.UPC = productItem.UPC;
                            productItemLine.OriginCountry = productItem.OriginCountryName;
                            productItemsLines.Add(productItemLine);
                        }                        
                    }

                    prealertDataProvider.ProductItemsLines = productItemsLines;
                }                
                #endregion

                prealertDataProvider.PreCarriageETD = shipmentpm.PreCarriageETD;
                prealertDataProvider.PreCarriageATD = shipmentpm.PreCarriageATD;
                prealertDataProvider.PreCarriageETA = shipmentpm.PreCarriageETA;
                prealertDataProvider.PreCarriageATA = shipmentpm.PreCarriageATA;
                prealertDataProvider.PreCarriageCarrierCode = shipmentpm.PreCarriageCarrierCode;
                prealertDataProvider.PreCarriageCarrierNumber = shipmentpm.PreCarriageCarrierNumber;
                prealertDataProvider.PreCarriageFrom = shipmentpm.PreCarriageFromPortName;
                prealertDataProvider.PreCarriageTo = shipmentpm.PreCarriageToPortName;
                prealertDataProvider.PreForwardingETD = shipmentpm.PreForwardingETD;
                prealertDataProvider.PreForwardingATD = shipmentpm.PreForwardingATD;
                prealertDataProvider.PreForwardingETA = shipmentpm.PreForwardingETA;
                prealertDataProvider.PreForwardingATA = shipmentpm.PreForwardingATA;
                prealertDataProvider.PreForwardingCarrierCode = shipmentpm.PreForwardingCarrierCode;
                prealertDataProvider.PreForwardingCarrierNumber = shipmentpm.PreForwardingCarrierNumber;
                prealertDataProvider.PreForwardingFrom = shipmentpm.PreForwardingFromPortName;
                prealertDataProvider.PreForwardingTo = shipmentpm.PreForwardingToPortName;
                prealertDataProvider.MainCarriageATD = shipmentpm.MainCarriageATD;
                prealertDataProvider.MainCarriageATA = shipmentpm.MainCarriageATA;
                prealertDataProvider.Transhipment1ATD = shipmentpm.Transshipment1ATD;
                prealertDataProvider.Transshipment1CarrierCode = shipmentpm.Transshipment1CarrierCode;
                prealertDataProvider.Transshipment1CarrierNumber_New = shipmentpm.Transshipment1CarrierNumber;
                prealertDataProvider.Transshipment1CarrierName = shipmentpm.Transshipment1CarrierName;
                prealertDataProvider.Transhipment2ATD = shipmentpm.Transshipment2ATD;
                prealertDataProvider.Transshipment2CarrierCode = shipmentpm.Transshipment2CarrierCode;
                prealertDataProvider.Transshipment2CarrierNumber_New = shipmentpm.Transshipment2CarrierNumber;
                prealertDataProvider.Transhipment3ATD = shipmentpm.Transshipment3ATD;
                prealertDataProvider.Transshipment3CarrierCode = shipmentpm.Transshipment3CarrierCode;
                prealertDataProvider.Transshipment3CarrierNumber_New = shipmentpm.Transshipment3CarrierNumber;
                prealertDataProvider.FullMaster = shipmentpm.LongMaster;
                prealertDataProvider.ShipmentNotes = shipmentpm.Notes;
                prealertDataProvider.TotalQuantity = shipmentpm.NumberOfPackages;
                prealertDataProvider.Salesman = shipmentpm.SalesmanUserName;
                prealertDataProvider.FullRoutings = this.GetFullRouting(shipmentpm, shipmentPickUpQuery, shipmentDeliveryQuery);
                prealertDataProvider.OnForwardingFrom = shipmentpm.OnForwardingFromPortName;
                prealertDataProvider.OnForwardingTo = shipmentpm.OnForwardingToPortName;
                prealertDataProvider.OnForwardingETD = shipmentpm.OnForwardingETD;
                prealertDataProvider.OnForwardingETA = shipmentpm.OnForwardingETA;
                prealertDataProvider.OnForwardingATD = shipmentpm.OnForwardingATD;
                prealertDataProvider.OnForwardingATA = shipmentpm.OnForwardingATA;
                prealertDataProvider.OnForwardingCarrierCode = shipmentpm.OnForwardingCarrierCode;
                prealertDataProvider.OnForwardingCarrierNumber = shipmentpm.OnForwardingCarrierNumber;
                prealertDataProvider.OnCarriageFrom = shipmentpm.OnCarriageFromPortName;
                prealertDataProvider.OnCarriageTo = shipmentpm.OnCarriageToPortName;
                prealertDataProvider.OnCarriageETD = shipmentpm.OnCarriageETD;
                prealertDataProvider.OnCarriageETA = shipmentpm.OnCarriageETA;
                prealertDataProvider.OnCarriageATD = shipmentpm.OnCarriageATD;
                prealertDataProvider.OnCarriageATA = shipmentpm.OnCarriageATA;
                prealertDataProvider.OnCarriageCarrierCode = shipmentpm.OnCarriageCarrierCode;
                prealertDataProvider.OnCarriageCarrierNumber = shipmentpm.OnCarriageCarrierNumber;

                ARInvoiceRepository invoiceRep = new ARInvoiceRepository(tenant);
                List<ARInvoice> invoices = invoiceRep.GetInvoicesByMainEntityId(shipmentpm.Id, tenant);
                if (invoices.Count > 0)
                {
                    string str = "";
                    foreach (ARInvoice item in invoices)
                    {
                        if (string.IsNullOrEmpty(str))
                        {
                            str = item.InvoiceNumber;
                        }
                        else
                        {
                            str = str + ", " + item.InvoiceNumber;
                        }
                    }

                    prealertDataProvider.InvoicesNumbers = str;
                }

                ShipmentPickUpDelivery myFirstPickup =
                    (from d in shipmentsContext.ShipmentPickUpDeliveries
                     where d.ShipmentId == shipmentpm.Id && d.PickUpDeliveryTypeCode == "PICK"
                     select d).OrderBy(s => s.PickUpDeliveryNumber).FirstOrDefault();

                if (myFirstPickup != null)
                {
                    prealertDataProvider.PickupETD = myFirstPickup.ETD;
                    prealertDataProvider.PickupATD = myFirstPickup.ATD;

                    PlaceOfReceiptData placeOfReceiptData = serviceHelper.GetPlaceOfReceiptData(myFirstPickup);
                }

                if (myFirstPickup != null)
                {
                    prealertDataProvider.OriginCountryName = this.GetPickUpDeliveryFromCityOrPortName(myFirstPickup);
                }

                else if (shipmentpm.PreForwardingFromPortId != null)
                {
                    prealertDataProvider.OriginCountryName = shipmentpm.PreForwardingFromPortCountryName;
                }

                else if (shipmentpm.PreCarriageFromPortId != null)
                {
                    prealertDataProvider.OriginCountryName = shipmentpm.PreCarriageFromPortCountryName;
                }

                else
                {
                    prealertDataProvider.OriginCountryName = shipmentpm.MainCarriageFromPortCountryName;
                }

                if (customer != null)
                {
                    if (!string.IsNullOrEmpty(customer.PrimaryContactId))
                    {
                        Contact contact = contactRepository.GetSingleContact(customer.PrimaryContactId, tenant);
                        if (contact != null)
                        {
                            prealertDataProvider.CustomerPrimaryContactName = contact.EnglishName;

                        }
                    }
                    prealertDataProvider.CustomerName = customer.EnglishName;
                }

                if (!string.IsNullOrEmpty(shipmentpm.AgentId))
                {
                    CardPM agent = cardQuery.GetSinglePM(shipmentpm.AgentId, tenant);
                    if (agent != null)
                    {
                        prealertDataProvider.AgentName = agent.EnglishName;

                        if (!string.IsNullOrEmpty(agent.PrimaryContactId))
                        {
                            Contact contact = contactRepository.GetSingleContact(agent.PrimaryContactId, tenant);
                            if (contact != null)
                            {
                                prealertDataProvider.AgentPrimaryContactName = contact.EnglishName;
                            }
                        }
                    }
                }

                prealertDataProvider.VoyageNumber = shipmentpm.MainCarriageCarrierNumber;
                prealertDataProvider.Vessel = shipmentpm.MainCarriageVesselName;

                if (!string.IsNullOrEmpty(shipmentpm.MoveTypeId))
                {
                    MoveType moveType = context.MoveTypes.Where(m => m.Id == shipmentpm.MoveTypeId).FirstOrDefault();

                    if (moveType != null)
                    {
                        prealertDataProvider.MoveTypeCode = moveType.Code;
                        prealertDataProvider.MoveTypeName = moveType.MoveTypeEnglishName;
                    }
                }

                //Custom fields
                DocumentTypeQuery documentTypeQuery = new DocumentTypeQuery(tenant);
                DocumentTypePM documentTypePM = documentTypeQuery.GetSingelDocumentTypeById(documentTypeId, tenant);
                FormCustomFieldQuery formCustomFieldQuery = new FormCustomFieldQuery(tenant);
                DocumentTypeCustomFieldQuery documentTypeCustomFieldQuery = new DocumentTypeCustomFieldQuery(tenant);
                if (documentTypePM != null)
                {
                    FormCustomFieldPM remarksFormCustomFieldPM = formCustomFieldQuery.GetFormCusotmFieldPMsByDocumentTypeId(documentTypePM.Id, tenant).Where(cu => cu.FieldCode == "Remarks").FirstOrDefault();
                    DocumentTypeCustomFieldPM remarksDocumentTypeCustomFieldPM = documentTypeCustomFieldQuery.GetDocumentTypeCusotmFieldPMsByDocumentTypeId(documentTypePM.Id, tenant).Where(cu => cu.FieldCode == "Remarks").FirstOrDefault();

                    prealertDataProvider.Remarks = remarksFormCustomFieldPM != null ? remarksFormCustomFieldPM.Value : (remarksDocumentTypeCustomFieldPM != null ? remarksDocumentTypeCustomFieldPM.DefaultValue : "");
                }

                prealertDataProvider.Logo = DataProviders.General.GetLogo(tenant);

                #region Warehouse Leg
                prealertDataProvider.WarehouseLegExpectedEntryDate = shipmentpm.WarehouseLegExpectedEntryDate;
                prealertDataProvider.WarehouseLegActualEntryDate = shipmentpm.WarehouseLegActualEntryDate;
                prealertDataProvider.WarehouseLegExpectedReleaseDate = shipmentpm.WarehouseLegExpectedReleaseDate;
                prealertDataProvider.WarehouseLegActualReleaseDate = shipmentpm.WarehouseLegActualReleaseDate;
                prealertDataProvider.WarehouseLegLastFreeDate = shipmentpm.WarehouseLegLastFreeDate;
                prealertDataProvider.WarehouseLegRemarks = shipmentpm.WarehouseLegRemarks;
                prealertDataProvider.WarehouseLegReference = shipmentpm.WarehouseLegReference;
                prealertDataProvider.WarehouseLegTerminalName = shipmentpm.WarehouseLegTerminalName;
                if (shipmentpm.WarehouseLegAddressId != null)
                {
                    Address warehouseAddress = addressRepository.GetSingleAddress(shipmentpm.WarehouseLegAddressId, tenant);
                    prealertDataProvider.WarehouseLegAddress = DataProviders.General.GetAddress(warehouseAddress);
                }
                prealertDataProvider.WarehouseLegEntryDate = shipmentpm.WarehouseLegEntryDate;
                prealertDataProvider.WarehouseLegReleaseDate = shipmentpm.WarehouseLegReleaseDate;
                prealertDataProvider.WarehouseLegTerminalCode = shipmentpm.WarehouseLegTerminalCode;
                #endregion

                CustomFieldResolver customFieldResolver = new CustomFieldResolver();
                customFieldResolver.SetDataProviderCustomFieldsValues("Shipment", tenant, shipmentpm, prealertDataProvider);
            }

            #region Serialize and remove null region

            try
            {
                Type prealerttype = prealertDataProvider.GetType();

                PropertyInfo[] properties = prealerttype.GetProperties();

                foreach (PropertyInfo pi in properties)
                {
                    Type piType = pi.PropertyType;

                    if (piType.Name != "Double" && piType.Name != "List`1")
                    {
                        if (pi.GetValue(prealertDataProvider, null) == null || pi.GetValue(prealertDataProvider, null).ToString() == "0" || pi.GetValue(prealertDataProvider, null).ToString() == "00.00")
                        {
                            pi.SetValue(prealertDataProvider, "", null);
                        }

                    }
                }
            }
            catch { }

            return prealertDataProvider;

            #endregion
        }

        private string GetFullRouting(ShipmentPM shipmentpm, ShipmentPickUpQuery shipmentPickUpQuery, ShipmentDeliveryQuery shipmentDeliveryQuery)
        {
            string routing = "";

            //pick ups
            List<ShipmentPickUpPM> myPickups = shipmentPickUpQuery.GetShipmentPickUpPMsByTenantAndShipment(shipmentid, tenant).Where(a => a.PickUpDeliveryToTypeCode == "PORT").ToList();
            if (myPickups.Count > 0)
            {
                foreach (ShipmentPickUpPM item in myPickups)
                {
                    if (string.IsNullOrEmpty(routing))
                    {
                        routing = item.FromPortCode + "-" + item.ToPortCode;
                    }
                    else
                    {
                        routing = routing + "-" + item.ToPortCode;
                    }
                }
            }

            //pre forwarding
            if (shipmentpm.PreForwardingFromPortId != null && shipmentpm.PreForwardingToPortId != null)
            {
                if (string.IsNullOrEmpty(routing))
                {
                    routing = shipmentpm.PreForwardingFromPortCode + "-" + shipmentpm.PreForwardingToPortCode;
                }
                else
                {
                    routing = routing + "-" + shipmentpm.PreForwardingToPortCode;
                }
            }

            //pre carriage
            if (shipmentpm.PreCarriageFromPortId != null && shipmentpm.PreCarriageToPortId != null)
            {
                if (string.IsNullOrEmpty(routing))
                {
                    routing = shipmentpm.PreCarriageFromPortCode + "-" + shipmentpm.PreCarriageToPortCode;
                }
                else
                {
                    routing = routing + "-" + shipmentpm.PreCarriageToPortCode;
                }
            }

            //main carriage
            if (string.IsNullOrEmpty(routing))
            {
                routing = shipmentpm.MainCarriageFromPortCode + "-" + shipmentpm.MainCarriageToPortCode;
            }
            else
            {
                routing = routing + "-" + shipmentpm.MainCarriageToPortCode;
            }

            //transshipment 1
            if (shipmentpm.Transshipment1FromPortId != null && shipmentpm.Transshipment1ToPortId != null)
            {
                if (string.IsNullOrEmpty(routing))
                {
                    routing = shipmentpm.Transshipment1ToPortCode;
                }
                else
                {
                    routing = routing + "-" + shipmentpm.Transshipment1ToPortCode;
                }
            }

            //transshipment 2
            if (shipmentpm.Transshipment2FromPortId != null && shipmentpm.Transshipment2ToPortId != null)
            {
                if (string.IsNullOrEmpty(routing))
                {
                    routing = shipmentpm.Transshipment2ToPortCode;
                }
                else
                {
                    routing = routing + "-" + shipmentpm.Transshipment2ToPortCode;
                }
            }

            //transshipment 3
            if (shipmentpm.Transshipment3FromPortId != null && shipmentpm.Transshipment3ToPortId != null)
            {
                if (string.IsNullOrEmpty(routing))
                {
                    routing = shipmentpm.Transshipment3ToPortCode;
                }
                else
                {
                    routing = routing + "-" + shipmentpm.Transshipment3ToPortCode;
                }
            }

            // on carriage
            if (shipmentpm.OnCarriageFromPortId != null && shipmentpm.OnCarriageToPortId != null)
            {
                if (string.IsNullOrEmpty(routing))
                {
                    routing = shipmentpm.OnCarriageToPortCode;
                }
                else
                {
                    routing = routing + "-" + shipmentpm.OnCarriageToPortCode;
                }
            }

            // on Forwarding
            if (shipmentpm.OnForwardingFromPortId != null && shipmentpm.OnForwardingToPortId != null)
            {
                if (string.IsNullOrEmpty(routing))
                {
                    routing = shipmentpm.OnForwardingToPortCode;
                }
                else
                {
                    routing = routing + "-" + shipmentpm.OnForwardingToPortCode;
                }
            }

            //deliveries
            List<ShipmentDeliveryPM> myDeliveries = shipmentDeliveryQuery.GetShipmentDeliveryPMsByTenantAndShipment(shipmentid, tenant).Where(a => a.PickUpDeliveryToTypeCode == "PORT").ToList();
            if (myDeliveries.Count > 0)
            {
                foreach (ShipmentDeliveryPM item in myDeliveries)
                {
                    if (string.IsNullOrEmpty(routing))
                    {
                        routing = item.ToPortCode;
                    }
                    else
                    {
                        routing = routing + "-" + item.ToPortCode;
                    }
                }
            }

            return routing;
        }

        public string GetPickUpDeliveryFromCityOrPortName(ShipmentPickUpDelivery entity)
        {
            string myResult = "";
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
                                    myResult = myPartnerAddress.Country == null ? "" : myPartnerAddress.Country.EnglishName;
                                }
                            }

                            break;
                        }

                    case "PORT":
                        {
                            if (!string.IsNullOrEmpty(entity.FromPortId))
                            {

                                Port myPort = portRepository.GetSinglePort(tenant, entity.FromPortId);
                                if (myPort != null)
                                {
                                    myResult = myPort.EnglishName;
                                }
                            }

                            break;
                        }

                    case "CASL":
                        {
                            var countryName = "";
                            if (!string.IsNullOrEmpty(entity.FromAddressCountryId))
                            {
                                Country fromAddressCountry = CountryRepository.GetSingleCountry(entity.FromAddressCountryId, tenant, false);
                                if (fromAddressCountry != null)
                                {
                                    countryName = fromAddressCountry.EnglishName;
                                }
                            }
                            myResult = countryName;
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

        private void GetShipmentPayaples()
        {
            List<ShipmentPayable> payables = GetShipmentPayaplesList();
            prealertDataProvider.PayablesList = new List<PayableLine>();

            foreach (ShipmentPayable payableItem in payables)
            {
                PayableLine payableLine = new PayableLine()
                {
                    Id = payableItem.ChargesType == null ? null : payableItem.ChargesType.Code,
                    Name = payableItem.ChargesType == null ? null : payableItem.ChargesType.EnglishName,
                    LocalName = payableItem.ChargesType == null ? null : payableItem.ChargesType.LocalName,
                    CurrencyCode = payableItem.Currency == null ? null : payableItem.Currency.Code,
                    LocalCurrencyCode = GetCurrencyCode(tenantpm.CurrencyId),
                    ProfitCurrencyCode = GetCurrencyCode(tenantpm.ProfitCurrencyId),
                    OpenAmount = payableItem.OpenAmount,
                    OpenAmountInLocal = payableItem.OpenAmountInLocalCurrency,
                    OpenAmountInProfit = payableItem.OpenAmountInProfitCurrency,
                    ExpectedAmount = payableItem.ExpectedAmount,
                    ExpectedAmountInLocal = payableItem.ExpectedAmountLocal,
                    ExpectedAmountInProfit = payableItem.ExpectedAmountInProfitCurrency,
                    AccountedAmount = payableItem.AccountedAmount,
                    AccountedAmountInLocal = payableItem.AccountedAmountInLocalCurrency,
                    AccountedAmountInProfit = payableItem.AccountedAmountInProfitCurrency,
                    UnitPrice = payableItem.UnitPrice,
                    UOM = payableItem.Measurement == null ? null : payableItem.Measurement.Name,
                    Quantity = payableItem.Quantity,
                    VendorName = GetPayableVendor(payableItem.VendorId),
                };

                if (payableItem.Measurement != null)
                {
                    if (payableItem.Measurement.Code == "")
                    {
                        payableLine.UOMPercentage = "%";
                    }
                }

                prealertDataProvider.PayablesList.Add(payableLine);
            }

        }
        private List<ShipmentPayable> GetShipmentPayaplesList()
        {
            ShipmentPayableRepository payableRepository = new ShipmentPayableRepository(tenant);
            return payableRepository.GetShipemntPayablesByShipmentId(shipmentid, tenant);
        }

        private string GetCurrencyCode(string currencyId)
        {
            Currency currency = CurrencyRepository.GetSingleCurrency(currencyId, tenant, true);
            return currency.Code;
        }
        private string GetPayableVendor(string vendorId)
        {
            if (!string.IsNullOrEmpty(vendorId))
            {
                Card vendorCard = CardRepository.GetSingleCard(vendorId, tenant, true);
                if (vendorCard != null)
                {
                    return vendorCard.EnglishName;
                }
            }
            return "";
        }
        private void GetShipmentPickUpAndDeliveries()
        {
            List<ShipmentPickUpDelivery> shipmentDeliveriesAndPickUps = GetShipmentPickUpAndDeliveriesList();
            prealertDataProvider.PickUpsList = new List<PickUpDeliveryLine>();
            prealertDataProvider.DeliveriesList = new List<PickUpDeliveryLine>();
            foreach (ShipmentPickUpDelivery item in shipmentDeliveriesAndPickUps)
            {
                PickUpDeliveryLine pickUpDeliveryLine = BuildPickUpDeliveryLine(item);
                InsertItemToPickUpsAndDeliveriesList(pickUpDeliveryLine, item.PickUpDeliveryTypeCode);
            }
        }
        private List<ShipmentPickUpDelivery> GetShipmentPickUpAndDeliveriesList()
        {
            ShipmentPickUpDeliveryRepository shipmentPickUpDeliveryRepository = new ShipmentPickUpDeliveryRepository(shipmentsContext);
            return shipmentPickUpDeliveryRepository.GetShipmentPickUpDeliveryForShipment(shipmentid, tenant);
        }
        private PickUpDeliveryLine BuildPickUpDeliveryLine(ShipmentPickUpDelivery pickUpDeliveryItem)
        {
            WebServiceHelper serviceHelper = new WebServiceHelper(tenant);
            PickUpDeliveryLine item = new PickUpDeliveryLine();
            item.ETD = pickUpDeliveryItem.ETD;
            item.ETA = pickUpDeliveryItem.ETA;
            item.ATD = pickUpDeliveryItem.ATD;
            item.ATA = pickUpDeliveryItem.ATA;
            item.FromAddress = serviceHelper.GetDeliveryPickUpAddress(BuildPickUpAndDeliveriesArguments(pickUpDeliveryItem, true));
            item.ToAddress = serviceHelper.GetDeliveryPickUpAddress(BuildPickUpAndDeliveriesArguments(pickUpDeliveryItem, false));
            item.CarrierName = GetPickUpsAndDeliveriesCarrierName(pickUpDeliveryItem.CarrierId);

            #region Empty Container
            item.EmptyContainerReturnRef = pickUpDeliveryItem.EmptyDeliveryDepotReference;

            if (!string.IsNullOrEmpty(pickUpDeliveryItem.EmptyDeliveryContainerPartnerId))
            {
                CardPM cardPM = cardQuery.GetSinglePM(pickUpDeliveryItem.EmptyDeliveryContainerPartnerId, tenant);

                if (cardPM != null)
                {
                    string myEmptyContainer = null;
                    string myEmptyContainerName = null;
                    string myEmptyContainerAddress = null;

                    myEmptyContainer = cardPM.EnglishName != null ? cardPM.EnglishName : "";
                    myEmptyContainerName = cardPM.EnglishName != null ? cardPM.EnglishName : "";

                    if (cardPM.MainAddressId != null)
                    {
                        Address theAddress = addressRepository.GetSingleAddress(cardPM.MainAddressId, tenant);

                        if (theAddress != null)
                        {
                            if (theAddress.IsLocalLanguage && !string.IsNullOrEmpty(cardPM.LocalName))
                            {
                                myEmptyContainer = cardPM.LocalName;
                                myEmptyContainerName = cardPM.LocalName;
                            }

                            myEmptyContainer = myEmptyContainer + Environment.NewLine + DataProviders.General.GetAddress(theAddress);
                            myEmptyContainerAddress = DataProviders.General.GetAddress(theAddress);

                            if (theAddress.PhoneNumber != null)
                            {
                                myEmptyContainer = myEmptyContainer + Environment.NewLine + "Phone No. : " + theAddress.PhoneNumber;
                                myEmptyContainerAddress = myEmptyContainerAddress + Environment.NewLine + "Phone No. : " + theAddress.PhoneNumber;
                            }

                            if (theAddress.FaxNumber != null)
                            {
                                myEmptyContainer = myEmptyContainer + "   Fax No. : " + theAddress.FaxNumber;
                                myEmptyContainerAddress = myEmptyContainerAddress + "   Fax No. : " + theAddress.FaxNumber;
                            }
                        }
                    }

                    item.EmptyContainerReturn = myEmptyContainer;
                    item.EmptyContainerReturnName = myEmptyContainerName;
                    item.EmptyContainerReturnAddress = myEmptyContainerAddress;
                }
            }
            #endregion

            return item;
        }
        private string GetPickUpsAndDeliveriesCarrierName(string carrierId)
        {
            Card carrierCard = CardRepository.GetSingleCard(carrierId, tenant, true);
            string carrierName = "";
            if (carrierCard != null)
            {
                carrierName = carrierCard.EnglishName != null ? carrierCard.EnglishName : "";
            }
            return carrierName;
        }
        private void InsertItemToPickUpsAndDeliveriesList(PickUpDeliveryLine pickUpDeliveryLine, string pickUpDeliveryTypeCode)
        {
            if (pickUpDeliveryTypeCode == "DELV" || pickUpDeliveryTypeCode == "EMPT")
            {
                prealertDataProvider.DeliveriesList.Add(pickUpDeliveryLine);
            }
            else if (pickUpDeliveryTypeCode == "PICK")
            {
                prealertDataProvider.PickUpsList.Add(pickUpDeliveryLine);
            }
        }
        private PickUpAndDeliveriesArguments BuildPickUpAndDeliveriesArguments(ShipmentPickUpDelivery pickUpDeliveryItem, bool isFromAddress)
        {
            if (isFromAddress)
            {
                return new PickUpAndDeliveriesArguments()
                {
                    TypeCode = pickUpDeliveryItem.PickUpDeliveryFromTypeCode,
                    PartnerCardId = pickUpDeliveryItem.FromPartnerCardId,
                    AddressId = pickUpDeliveryItem.FromAddressId,
                    PortId = pickUpDeliveryItem.FromPortId,
                    AddressCountryId = pickUpDeliveryItem.FromAddressCountryId,
                    AddressCity = pickUpDeliveryItem.FromAddressCity,
                    AddressZipCode = pickUpDeliveryItem.FromAddressZipCode
                };
            }
            else
            {
                return new PickUpAndDeliveriesArguments()
                {
                    TypeCode = pickUpDeliveryItem.PickUpDeliveryToTypeCode,
                    PartnerCardId = pickUpDeliveryItem.ToPartnerCardId,
                    AddressId = pickUpDeliveryItem.ToAddressId,
                    PortId = pickUpDeliveryItem.ToPortId,
                    AddressCountryId = pickUpDeliveryItem.ToAddressCountryId,
                    AddressCity = pickUpDeliveryItem.ToAddressCity,
                    AddressZipCode = pickUpDeliveryItem.ToAddressZipCode
                };
            }
        }
        private void GetOtherCharges(string shipmentId, int tenant, ref double totalPrepaidString, ref double totalCollectString)
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
        private void MapMasterShipmentNumber()
        {
            if (shipmentpm.ShipmentLevelCode == "C")
            {
                prealertDataProvider.MasterShipmentNumber = shipmentpm.ShipmentNumber;
            }

            else if (shipmentpm.ShipmentLevelCode == "H" && !string.IsNullOrEmpty(shipmentpm.MasterShipmentDataId))
            {
                SetMasterShipmentNumberForConnectedHouse();
            }
        }
        private void SetMasterShipmentNumberForConnectedHouse()
        {
            Shipment masterData = shipmentRepository.GetSingleShipment(shipmentpm.MasterShipmentDataId, tenant);
            if (masterData != null)
            {
                prealertDataProvider.MasterShipmentNumber = masterData.ShipmentNumber;
            }
        }
    }
}
