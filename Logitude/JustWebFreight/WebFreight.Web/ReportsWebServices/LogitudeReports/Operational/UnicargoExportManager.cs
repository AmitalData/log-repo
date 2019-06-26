using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Helpers;
using Logitude.Server.Tools.Helpers;
using Logitude.TimeManagement.Data;
using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.Data.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using WebFreight.Web.DataProviders;

namespace WebFreight.Web.ReportsWebServices.LogitudeReports.TimeManagement
{
    public class UnicargoExportManager
    {
        private int tenant;
        private IShipmentsContext shipmentsContext;
       private  ICommonDataContext commonDataContext;
        private EntityStatusRepository entityStatusRepository;
        private IWebFreightContext webFreightContext;
        private TransportModeRepository transportModeRepository;
        private ContactRepository ContactRepository;
        private CustomFieldResolver customFieldResolver;
        private CardRepository cardRepository;

        public UnicargoExportManager(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;
            customFieldResolver = new CustomFieldResolver();
            shipmentsContext = ShipmentsContext.GetContext(tenant);
            ContactRepository =new ContactRepository( CommonDataContext.GetContext(tenant));
            commonDataContext = CommonDataContext.GetContext(tenant);
            webFreightContext = WebFreightContext.GetContext(tenant);
            cardRepository = new CardRepository(tenant);
            entityStatusRepository = new EntityStatusRepository(tenant);
            transportModeRepository = new TransportModeRepository(tenant);
            MemoryStream memoryStream = new MemoryStream(xmlFilters);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations myQueryOperations = (QueryOperations)xmlSerializer.Deserialize(memoryStream);
        }

        public byte[] GetData()
        {
            UnicargoExportDataProvider myDataProvider = this.LoadDataProvider();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(UnicargoExportDataProvider));
            MemoryStream memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream, myDataProvider);
            memoryStream.Seek(0, SeekOrigin.Begin);

            StreamReader streamReader = new StreamReader(memoryStream);
            string content = streamReader.ReadToEnd();
            byte[] bytearray = memoryStream.ToArray();
            return bytearray;
        }

        private UnicargoExportDataProvider LoadDataProvider()
        {
            UnicargoExportDataProvider myDataProvider = new UnicargoExportDataProvider()
            {
             Shipments   = new List<UnicargoExport>()
            };

            List<Shipment> shipments = shipmentsContext.Shipments.Where(p=>p.Tenant==tenant && p.IsOperationalClosed==false && p.IsAccountingClosed==false && p.IsCancelled==false).ToList();
           Dictionary<string,string> incoterms= commonDataContext.Incoterms.Where(p => p.Tenant == tenant).ToDictionary(a => a.Id, b => b.Name);
            List<string> shipmentdelevriesIds = shipments.Select(d => d.Id).ToList();
            List<ShipmentMasterData> shipmentMasterDatas = shipmentsContext.ShipmentMasterDatas.Where(p => p.Tenant == tenant && shipmentdelevriesIds.Contains(p.Id)).ToList();

            Dictionary<string, string> ShipmentTypes = shipmentsContext.ShipmentTypes.ToDictionary(a => a.Id, b => b.Name);
            Dictionary<string, string> Shipmenttransportmodes = webFreightContext.TransportModes.ToDictionary(a => a.Id, b => b.Name);

            Dictionary<string, string> transportmodes = shipmentsContext.PickUpDeliveryTransportModes.ToDictionary(a => a.Code, b => b.Name);
            Dictionary<string, string> Directions = webFreightContext.Directions.ToDictionary(a => a.Id, b => b.Name);
            Dictionary<string, string> ShipmentLevels = shipmentsContext.ShipmentLevels.ToDictionary(a => a.Code, b => b.Name);
            Dictionary<string, string> departments = commonDataContext.Departments.Where(p => p.Tenant == tenant).ToDictionary(a => a.Id, b => b.EnglishName);
            Dictionary<string, string> branches = commonDataContext.Branches.Where(p => p.Tenant == tenant).ToDictionary(a => a.Id, b => b.EnglishName);
            Dictionary<string, string> SpeicalServices = shipmentsContext.SpecialServicesTypes.Where(p => p.Tenant == tenant).ToDictionary(a => a.Id, b => b.EnglishName);
            Dictionary<string, string> ShipmentPackagesTypes = commonDataContext. PackageTypes.Where(p => p.Tenant == tenant).ToDictionary(a => a.Id, b => b.EnglishName);
            List<ShipmentPickUpDelivery> shipmentPickUpDeliveriesLists = (from d in shipmentsContext.ShipmentPickUpDeliveries where shipmentdelevriesIds.Contains(d.ShipmentId) select d).ToList();
            List<ShipmentPackage> shipmentPackages = (from d in shipmentsContext.ShipmentPackages where shipmentdelevriesIds.Contains(d.ShipmentId) select d).ToList();
            List<string> FromPartnerCardIds = (from d in shipmentsContext.ShipmentPickUpDeliveries where shipmentdelevriesIds.Contains(d.ShipmentId) select d.FromPartnerCardId).ToList();
            List<string> ConsgineeCardIds = (from d in shipmentsContext.Shipments where shipmentdelevriesIds.Contains(d.Id) select d.ConsigneeId).ToList();
            List<Address> FromPartnerAddressLists = (from a in commonDataContext.Addresses.Include("Country").Include("State") where a.Tenant == tenant && FromPartnerCardIds.Contains(a.CardId) && a.AddressTypeId.ToUpper() == "M" select a).ToList();
            List<Address> ConsgineeAddressLists = (from a in commonDataContext.Addresses.Include("Country").Include("State") where a.Tenant == tenant && ConsgineeCardIds.Contains(a.CardId) && a.AddressTypeId.ToUpper() == "M" select a).ToList();

            List<string> FromAddressCountryIds = (from d in shipmentsContext.ShipmentPickUpDeliveries where shipmentdelevriesIds.Contains(d.ShipmentId) select d.FromAddressCountryId).ToList();
            List<string> ToPartnerCardIds = (from d in shipmentsContext.ShipmentPickUpDeliveries where shipmentdelevriesIds.Contains(d.ShipmentId) select d.ToPartnerCardId).ToList();
            Dictionary<string, string> Vessels = commonDataContext.Vessels.Where(p => p.Tenant == tenant ).ToDictionary(a => a.Id, b => b.EnglishName);



            List<Country> FromAddressCountryLists = (from record in commonDataContext.Countries.Include("GlobalZone") where FromAddressCountryIds.Contains(record.Id) && record.Tenant == tenant select record).ToList();
            List<Address> ToPartnerAddressLists = (from a in commonDataContext.Addresses.Include("Country").Include("State") where a.Tenant == tenant && ToPartnerCardIds.Contains(a.CardId) && a.AddressTypeId.ToUpper() == "M" select a).ToList();




            shipments.ForEach(item =>
            {
                UnicargoExport Shipment = new UnicargoExport();

                #region  General Section
                Shipment.House = item.House;
                Shipment.FileNumber = item.ShipmentNumber;
                if (!string.IsNullOrEmpty(item.IncotermId))
                {
                    Shipment.Incoterms = incoterms.ContainsKey(item.IncotermId)?incoterms[item.IncotermId]!=null? incoterms[item.IncotermId]:null:null;
                }

                Shipment.MainHarmonize = item.MainHarmonize;
                if (!string.IsNullOrEmpty(item.ShipmentTypeId))
                {
                    Shipment.Type = ShipmentTypes.ContainsKey(item.ShipmentTypeId) ? ShipmentTypes[item.ShipmentTypeId] != null ? ShipmentTypes[item.ShipmentTypeId] : null:null;
                }
                else
                {
                    Shipment.Type = "Air";
                }

                if (!string.IsNullOrEmpty(item.StatusId))
                {
                    EntityStatus status= entityStatusRepository.GetSingleEntityStatus(item.StatusId, tenant);
                    if (status != null)
                    {
                        Shipment.Status = status.Name;
                    }
                }


                if (!string.IsNullOrEmpty(item.TransportModeId))
                {
                    Shipment.TransportMode = Shipmenttransportmodes.ContainsKey(item.TransportModeId) ? Shipmenttransportmodes[item.TransportModeId] != null ? Shipmenttransportmodes[item.TransportModeId] : null:null;
                }

                if (!string.IsNullOrEmpty(item.DirectionId))
                {
                    Shipment.Direction = Directions.ContainsKey(item.DirectionId) ?Directions[item.DirectionId] != null ? Directions[item.DirectionId] : null:null;
                }


                if (!string.IsNullOrEmpty(item.ShipmentLevelCode))
                {
                    Shipment.ShipmentLevel = ShipmentLevels.ContainsKey(item.ShipmentLevelCode) ?ShipmentLevels[item.ShipmentLevelCode] != null ? ShipmentLevels[item.ShipmentLevelCode] : null:null;
                }

                if(!string.IsNullOrEmpty(item.CreatedByUserId))
                {
                    Contact contact=ContactRepository.GetSingleContactByIdAndTenant(item.CreatedByUserId, tenant, true);
                    if (contact != null)
                    {
                        Shipment.Openedby = contact.EnglishName;
                    }
                }


                if (!string.IsNullOrEmpty(item.SalesmanUserId))
                {
                    Contact contact = ContactRepository.GetSingleContactByIdAndTenant(item.SalesmanUserId, tenant, true);
                    if (contact != null)
                    {
                        Shipment.Salesman = contact.EnglishName;
                    }
                }

                if (!string.IsNullOrEmpty(item.DepartmentId))
                {
                    Shipment.Department = departments.ContainsKey(item.DepartmentId) ?departments[item.DepartmentId] != null ? departments[item.DepartmentId] : null:null;
                }

                if (!string.IsNullOrEmpty(item.BranchId))
                {
                    Shipment.Branch = branches.ContainsKey(item.BranchId) ?branches[item.BranchId] != null ? branches[item.BranchId] : null:null;
                }


                if (!string.IsNullOrEmpty(item.AccountManagerUserId))
                {
                    Contact contact = ContactRepository.GetSingleContactByIdAndTenant(item.AccountManagerUserId, tenant, true);
                    if (contact != null)
                    {
                        Shipment.AccountManager = contact.EnglishName;
                    }
                }

                if (!string.IsNullOrEmpty(item.SpecialServicesTypeId))
                {
                    Shipment.SpecialServiceType = SpeicalServices.ContainsKey(item.SpecialServicesTypeId) ?SpeicalServices[item.SpecialServicesTypeId] != null ? SpeicalServices[item.SpecialServicesTypeId] : null:null;
                }
                Shipment.CreateDate = item.CreateDateTime;
                Shipment.ValueofGoods = item.ValueOfGoods;
                customFieldResolver.SetDataProviderCustomFieldsValues("Shipment", tenant, item, Shipment);


                #endregion


                #region Partners Section
                Shipment.Shipper = item.ShipperName;
                Shipment.Consignee = item.ConsigneeName;
                if (!string.IsNullOrEmpty(item.ConsigneeId))
                {
                    Address ConsigneeAddress = ConsgineeAddressLists.Where(d => d.CardId == item.ConsigneeId).FirstOrDefault();
                    if (ConsigneeAddress != null)
                    {
                        Shipment.ConsigneeAddress = DataProviders.General.GetAddress(ConsigneeAddress);// myPartnerAddress.Country == null ? "" : myPartnerAddress.Country.EnglishName;
                        Shipment.ConsigneePhone = ConsigneeAddress.PhoneNumber;

                    }
               


                }

                if (!string.IsNullOrEmpty(item.Notify1Id))
                {
                    Card card = cardRepository.GetSingleCardByIdAndTenant(item.Notify1Id, tenant, true);
                    if (card != null)
                    {
                        Shipment.Notify1 = card.EnglishName;
                    }
                }

                if (!string.IsNullOrEmpty(item.Notify2Id))
                {
                    Card card = cardRepository.GetSingleCardByIdAndTenant(item.Notify2Id, tenant, true);
                    if (card != null)
                    {
                        Shipment.Notify2 = card.EnglishName;
                    }
                }


                if (!string.IsNullOrEmpty(item.ColoaderId))
                {
                    Card card = cardRepository.GetSingleCardByIdAndTenant(item.ColoaderId, tenant, true);
                    if (card != null)
                    {
                        Shipment.CoLoader = card.EnglishName;
                    }
                }

                if (!string.IsNullOrEmpty(item.FreightForwarderId))
                {
                    Card card = cardRepository.GetSingleCardByIdAndTenant(item.FreightForwarderId, tenant, true);
                    if (card != null)
                    {
                        Shipment.FreightForwarder = card.EnglishName;
                    }
                }


                if (!string.IsNullOrEmpty(item.ConsolidatorId))
                {
                    Card card = cardRepository.GetSingleCardByIdAndTenant(item.ConsolidatorId, tenant, true);
                    if (card != null)
                    {
                        Shipment.Consolidator = card.EnglishName;
                    }
                }



                if (!string.IsNullOrEmpty(item.CustomerId))
                {
                    Card card = cardRepository.GetSingleCardByIdAndTenant(item.CustomerId, tenant, true);
                    if (card != null)
                    {
                        Shipment.Customer = card.EnglishName;
                    }
                }

                if (!string.IsNullOrEmpty(item.ShipperNotExporterId))
                {
                    Card card = cardRepository.GetSingleCardByIdAndTenant(item.ShipperNotExporterId, tenant, true);
                    if (card != null)
                    {
                        Shipment.ShippernotExporter = card.EnglishName;
                    }
                }

                if (!string.IsNullOrEmpty(item.AgentId))
                {
                    Card card = cardRepository.GetSingleCardByIdAndTenant(item.AgentId, tenant, true);
                    if (card != null)
                    {
                        Shipment.Agent = card.EnglishName;
                    }
                }


                if (!string.IsNullOrEmpty(item.ReleasingAgentId))
                {
                    Card card = cardRepository.GetSingleCardByIdAndTenant(item.ReleasingAgentId, tenant, true);
                    if (card != null)
                    {
                        Shipment.ReleasingAgent = card.EnglishName;
                    }
                }


                #endregion

                #region Packages Section
                string packageType = "";
                string shipperSeal = "";
                string containerNumber = "";

                shipmentPackages.Where(p=>p.ShipmentId==item.Id).ToList().ForEach(package =>
                {

                    if (!string.IsNullOrEmpty(package.PackageTypeId))
                    {
                        string type = ShipmentPackagesTypes.ContainsKey(package.PackageTypeId) ? ShipmentPackagesTypes[package.PackageTypeId] != null ? ShipmentPackagesTypes[package.PackageTypeId] : null : null;
                        if (!string.IsNullOrEmpty(type))
                        {
                            packageType += type+",";
                        }
                    }


                    if (!string.IsNullOrEmpty(package.ShipperSeal))
                    {
                        shipperSeal += package.ShipperSeal + ",";
                    }

                    if (!string.IsNullOrEmpty(package.ContainerNumber))
                    {
                        containerNumber += package.ContainerNumber + ",";
                    }

                });

                if (packageType.EndsWith(","))
                    packageType = packageType.Substring(0, packageType.Length - 1);


                if (shipperSeal.EndsWith(","))
                    shipperSeal = shipperSeal.Substring(0, shipperSeal.Length - 1);


                if (containerNumber.EndsWith(","))
                    containerNumber = containerNumber.Substring(0, containerNumber.Length - 1);

                Shipment.PackageType = packageType; 
                Shipment.ShipperSeal = shipperSeal; 
                Shipment.ContinerNumber = containerNumber;

                int numberofpackages = item.NumberOfPackages != null ? item.NumberOfPackages.Value : 0;
                int numberofcontainers = item.NumberOfContainers != null ? item.NumberOfContainers.Value : 0;

                Shipment.TotalPieces = MethodHelper.IsLCLEntity(item.TransportModeId, item.ShipmentTypeId) ? numberofpackages.ToString() : numberofcontainers.ToString();


                Shipment.Volume = item.Volume;// shipmentPackages.Sum(p => p.Volume);
                Shipment.GrossWeight = item.GrossWeight;// shipmentPackages.Sum(p => p.Weight);
                Shipment.VolumetricWeight = item.VolumetricWeight;// shipmentPackages.Sum(p => p.VolumetricWeight);
                Shipment.Ratio = item.Ratio;
                Shipment.DescriptionofGoods = item.DescriptionOfGoods;
                #endregion


                #region Routing Section

                ShipmentPickUpDelivery myLastDelivery = shipmentPickUpDeliveriesLists.Where(d => d.ShipmentId == item.Id && d.PickUpDeliveryTypeCode == "DELV").OrderByDescending(s => s.PickUpDeliveryNumber).FirstOrDefault();
                ShipmentPickUpDelivery myFirstPickup = shipmentPickUpDeliveriesLists.Where(d => d.ShipmentId == item.Id && d.PickUpDeliveryTypeCode == "PICK").OrderBy(s => s.PickUpDeliveryNumber).FirstOrDefault();
                if (myFirstPickup != null) {

                        if(myFirstPickup.PickUpDeliveryFromTypeCode== "PART")
                        {
                                    if (!string.IsNullOrEmpty(myFirstPickup.FromPartnerCardId))
                                    {
                                        Address myPartnerAddress = FromPartnerAddressLists.Where(d => d.CardId == myFirstPickup.FromPartnerCardId).FirstOrDefault();
                                        if (myPartnerAddress != null)
                                        {
                                Shipment.PickupFromPartnerAddress = DataProviders.General.GetAddress(myPartnerAddress);// myPartnerAddress.Country == null ? "" : myPartnerAddress.Country.EnglishName;
                                        }

                                Card card = cardRepository.GetSingleCardByIdAndTenant(myFirstPickup.FromPartnerCardId, tenant, true);

                                if (card != null)
                                {
                                    Shipment.PickupFromPartner = card.EnglishName;
                                }
                            }
                        }

                        if (myFirstPickup.PickUpDeliveryToTypeCode == "PART")
                        {
                            if (!string.IsNullOrEmpty(myFirstPickup.ToPartnerCardId))
                            {
                                Address myPartnerAddress = ToPartnerAddressLists.Where(d => d.CardId == myFirstPickup.ToPartnerCardId).FirstOrDefault();
                                if (myPartnerAddress != null)
                                {
                                Shipment.PickupToPartnerAddress = DataProviders.General.GetAddress(myPartnerAddress);// myPartnerAddress.Country == null ? "" : myPartnerAddress.Country.EnglishName;
                                }

                                Card card = cardRepository.GetSingleCardByIdAndTenant(myFirstPickup.ToPartnerCardId, tenant, true);

                                if (card != null)
                                {
                                    Shipment.PickupToPartner = card.EnglishName;
                                }
                            }
                        }

                        if (myFirstPickup.PickUpDeliveryToTypeCode == "PORT")
                        {
                            if (!string.IsNullOrEmpty(myFirstPickup.ToPortId))
                            {
                                PortPM myPort = PortQuery.GetSinglePort(tenant, myFirstPickup.ToPortId, true);
                                if (myPort != null)
                                {
                                Shipment.PickupToPort = myPort.Code + " " + myPort.EnglishName;// myPort.StateName+" , "+ myPort.CountryName;
                                }
                            }
                        }

                        Shipment.PickupExpectedDeparture = myFirstPickup.ETD;
                        Shipment.PickupExpectedArrival = myFirstPickup.ETA;
                        Shipment.PickupActualDeparture = myFirstPickup.ATD;
                        Shipment.PickupActualArrival = myFirstPickup.ATA;

                    }

                ShipmentMasterData MasterData = shipmentMasterDatas.Where(p => p.Id == item.Id).FirstOrDefault();
                if (MasterData != null)
                {


                    if (!string.IsNullOrEmpty(MasterData.MainCarriageFromPortId))
                    {

                        PortPM myPort = PortQuery.GetSinglePort(tenant, MasterData.MainCarriageFromPortId, true);
                        if (myPort != null)
                        {
                            Shipment.MainCarriageLeg1LoadingPort = myPort.Code + " " + myPort.EnglishName;
                        }

                    }

                    if (!string.IsNullOrEmpty(MasterData.Transshipment1FromPortId))
                    {

                        PortPM myPort = PortQuery.GetSinglePort(tenant, MasterData.Transshipment1FromPortId, true);
                        if (myPort != null)
                        {
                            Shipment.MainCarriageLeg1ViaPort1 = myPort.Code + " " + myPort.EnglishName;
                        }

                    }

                    if (!string.IsNullOrEmpty(MasterData.Transshipment2FromPortId))
                    {

                        PortPM myPort = PortQuery.GetSinglePort(tenant, MasterData.Transshipment2FromPortId, true);
                        if (myPort != null)
                        {
                            Shipment.MainCarriageLeg1ViaPort2 = myPort.Code + " " + myPort.EnglishName;
                        }

                    }


                    if (!string.IsNullOrEmpty(MasterData.Transshipment3FromPortId))
                    {

                        PortPM myPort = PortQuery.GetSinglePort(tenant, MasterData.Transshipment3FromPortId, true);
                        if (myPort != null)
                        {
                            Shipment.MainMainCarriageLeg1ViaPort3 = myPort.Code + " " + myPort.EnglishName;
                        }

                    }



                    if (!string.IsNullOrEmpty(MasterData.MainCarriageToPortId))
                    {

                        PortPM myPort = PortQuery.GetSinglePort(tenant, MasterData.MainCarriageToPortId, true);
                        if (myPort != null)
                        {
                            Shipment.MainCarriageLeg1DischargePort = myPort.Code + " " + myPort.EnglishName;
                        }

                    }


                    if (!string.IsNullOrEmpty(MasterData.MainCarriageToPortId))
                    {

                        PortPM myPort = PortQuery.GetSinglePort(tenant, MasterData.MainCarriageToPortId, true);
                        if (myPort != null)
                        {
                            Shipment.MainCarriageLeg1DischargePort = myPort.Code + " " + myPort.EnglishName;
                        }

                    }


                    if (!string.IsNullOrEmpty(MasterData.MainCarriageCarrierId))
                    {
                        Card card = cardRepository.GetSingleCardByIdAndTenant(MasterData.MainCarriageCarrierId, tenant, true);
                        if (card != null)
                        {
                            Shipment.MainCarriageLeg1ShippingLine = card.EnglishName;
                        }
                    }



                    if (!string.IsNullOrEmpty(MasterData.MainCarriageCarrierNumber))
                    {
                        Shipment.MainCarriageLeg1VoyageNo = MasterData.MainCarriageCarrierNumber;
                    }

                    if (!string.IsNullOrEmpty(MasterData.Master))
                    {
                        Shipment.MainCarriageLeg1OBL = MasterData.Master;
                    }

                    if (MasterData.MAWBOBLDate != null)
                    {
                        Shipment.MainCarriageLeg1OBLDate = MasterData.MAWBOBLDate;
                    }

                    if (item.CutoffDate != null)
                    {
                        Shipment.MainCarriageLeg1CutoffDate = item.CutoffDate;

                    }

                    if (!string.IsNullOrEmpty(MasterData.MainCarriageVesselId))
                    {
                        string vessel = Vessels.ContainsKey(MasterData.MainCarriageVesselId) ? Vessels[MasterData.MainCarriageVesselId] != null ? Vessels[MasterData.MainCarriageVesselId] : null : null;

                        Shipment.MainCarriageLeg1Vessel = vessel;

                    }


                    if (MasterData.MainCarriageETD != null)
                    {
                        Shipment.MainCarriageLeg1ETD = MasterData.MainCarriageETD;
                    }

                    if (MasterData.MainCarriageETA != null)
                    {
                        Shipment.MainCarriageLeg1ETA = MasterData.MainCarriageETA;
                    }

                    if (MasterData.MainCarriageATD != null)
                    {
                        Shipment.MainCarriageLeg1ATD = MasterData.MainCarriageATD;
                    }

                    if (MasterData.MainCarriageATA != null)
                    {
                        Shipment.MainCarriageLeg1ATA = MasterData.MainCarriageATA;
                    }

                    if (!string.IsNullOrEmpty(MasterData.Transshipment1CarrierId))
                    {
                        Card card = cardRepository.GetSingleCardByIdAndTenant(MasterData.Transshipment1CarrierId, tenant, true);

                        if (card != null)
                        {
                            Shipment.Transshipment1ShippingLine = card.EnglishName;
                        }
                    }

                    if (!string.IsNullOrEmpty(MasterData.Transshipment1CarrierNumber))
                    {
                        Shipment.Transshipment1VoyageNo = MasterData.Transshipment1CarrierNumber;
                    }

                    if (!string.IsNullOrEmpty(MasterData.Transshipment1AdditionalMAWBOBLBL))
                    {
                        Shipment.Transshipment1OBL = MasterData.Transshipment1AdditionalMAWBOBLBL;
                    }

                    if (!string.IsNullOrEmpty(MasterData.Transshipment1VesselId))
                    {
                        string vessel = Vessels.ContainsKey(MasterData.Transshipment1VesselId) ? Vessels[MasterData.Transshipment1VesselId] != null ? Vessels[MasterData.Transshipment1VesselId] : null : null;

                        Shipment.Transshipment1Vessel = vessel;
                    }

                    if (MasterData.Transshipment1ETD != null)
                    {
                        Shipment.Transshipment1ETD = MasterData.Transshipment1ETD;
                    }

                    if (MasterData.Transshipment1ETA != null)
                    {
                        Shipment.Transshipment1ETA = MasterData.Transshipment1ETA;
                    }

                    if (MasterData.Transshipment1ATD != null)
                    {
                        Shipment.Transshipment1ATD = MasterData.Transshipment1ATD;
                    }

                    if (MasterData.Transshipment1ATA != null)
                    {
                        Shipment.Transshipment1ATA = MasterData.Transshipment1ATA;
                    }

                    if (myLastDelivery != null)
                    {
                        if (myLastDelivery.PickUpDeliveryToTypeCode == "PART")
                        {
                            if (!string.IsNullOrEmpty(myLastDelivery.ToPartnerCardId))
                            {
                                Address myPartnerAddress = ToPartnerAddressLists.Where(d => d.CardId == myLastDelivery.ToPartnerCardId).FirstOrDefault();
                                if (myPartnerAddress != null)
                                {
                                    Shipment.DeliveryToPatnerAddress = DataProviders.General.GetAddress(myPartnerAddress);// myPartnerAddress.Country == null ? "" : myPartnerAddress.Country.EnglishName;
                                }

                                Card card = cardRepository.GetSingleCardByIdAndTenant(myLastDelivery.ToPartnerCardId, tenant, true);

                                if (card != null)
                                {
                                    Shipment.DeliveryToPartner = card.EnglishName;
                                }
                            }




                        }


                        if (myLastDelivery.PickUpDeliveryFromTypeCode == "PART")

                            if (!string.IsNullOrEmpty(myLastDelivery.FromPartnerCardId))
                            {

                                Card card = cardRepository.GetSingleCardByIdAndTenant(myLastDelivery.FromPartnerCardId, tenant, true);

                                if (card != null)
                                {
                                    Shipment.DeliveryFromPartner = card.EnglishName;
                                }
                            }


                        if (myLastDelivery.PickUpDeliveryFromTypeCode == "PORT")
                        {
                            if (!string.IsNullOrEmpty(myLastDelivery.FromPortId))
                            {
                                PortPM myPort = PortQuery.GetSinglePort(tenant, myLastDelivery.FromPortId, true);
                                if (myPort != null)
                                {
                                    Shipment.DeliveryFromPort = myPort.Code + " " + myPort.EnglishName;
                                }
                            }
                        }

                    

                









                        if (!string.IsNullOrEmpty(myLastDelivery.TransportModeCode))
                        {
                            Shipment.DeliveryTransportMode = transportmodes.ContainsKey(myLastDelivery.TransportModeCode) ? transportmodes[myLastDelivery.TransportModeCode] != null ? transportmodes[myLastDelivery.TransportModeCode] : null : null;
                        }


                        if (myLastDelivery.ETD != null)
                        {
                            Shipment.DeliveryExpectedDeparture = myLastDelivery.ETD;
                        }

                        if (myLastDelivery.ETA != null)
                        {
                            Shipment.DeliveryExpectedArrival = myLastDelivery.ETA;
                        }

                        if (myLastDelivery.ATA != null)
                        {
                            Shipment.DeliveryActualArrival = myLastDelivery.ATA;
                        }

                        if (myLastDelivery.ATD != null)
                        {
                            Shipment.DeliveryActualDeparture = myLastDelivery.ATD;
                        }
                    }
                    

                }

               
                myDataProvider.Shipments.Add(Shipment);
                #endregion


            });

            return myDataProvider;
        }

    }
}