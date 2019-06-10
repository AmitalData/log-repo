using Logitude.BL.Helpers;
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
            Dictionary<string, string> ShipmentTypes = shipmentsContext.ShipmentTypes.ToDictionary(a => a.Id, b => b.Name);
            Dictionary<string, string> transportmodes = webFreightContext.TransportModes.ToDictionary(a => a.Id, b => b.Name);
            Dictionary<string, string> Directions = webFreightContext.Directions.ToDictionary(a => a.Id, b => b.Name);
            Dictionary<string, string> ShipmentLevels = shipmentsContext.ShipmentLevels.ToDictionary(a => a.Code, b => b.Name);
            Dictionary<string, string> departments = commonDataContext.Departments.Where(p => p.Tenant == tenant).ToDictionary(a => a.Id, b => b.EnglishName);
            Dictionary<string, string> branches = commonDataContext.Branches.Where(p => p.Tenant == tenant).ToDictionary(a => a.Id, b => b.EnglishName);
            Dictionary<string, string> SpeicalServices = webFreightContext.SpecialServices.Where(p => p.Tenant == tenant).ToDictionary(a => a.Id, b => b.SpecialServiceEnglishName);
            Dictionary<string, string> ShipmentPackagesTypes = commonDataContext. PackageTypes.Where(p => p.Tenant == tenant).ToDictionary(a => a.Id, b => b.EnglishName);


            shipments.ForEach(item =>
            {
                UnicargoExport Shipment = new UnicargoExport();

                #region  General Section
                Shipment.House = item.House;
                if (!string.IsNullOrEmpty(item.IncotermId))
                {
                    Shipment.Incoterms = incoterms.ContainsKey(item.IncotermId)?incoterms[item.IncotermId]!=null? incoterms[item.IncotermId]:null:null;
                }

                Shipment.MainHarmonize = item.MainHarmonize;
                if (!string.IsNullOrEmpty(item.ShipmentTypeId))
                {
                    Shipment.Type = ShipmentTypes.ContainsKey(item.ShipmentTypeId) ? ShipmentTypes[item.ShipmentTypeId] != null ? ShipmentTypes[item.ShipmentTypeId] : null:null;
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
                    Shipment.TransportMode = transportmodes.ContainsKey(item.TransportModeId) ?transportmodes[item.TransportModeId] != null ? transportmodes[item.TransportModeId] : null:null;
                }

                if (!string.IsNullOrEmpty(item.DirectionId))
                {
                    Shipment.Direction = Directions.ContainsKey(item.DirectionId) ?Directions[item.DirectionId] != null ? Directions[item.DirectionId] : null:null;
                }

                Shipment.FileNumber = item.CustomFileNumber; // Check

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
                customFieldResolver.SetDataProviderCustomFieldsValues("Shipment", tenant, Shipment, item);
               
                #endregion


                #region Partners Section
                Shipment.Shipper = item.ShipperName;
                Shipment.Consignee = item.ConsigneeName;
                if (!string.IsNullOrEmpty(item.Notify1Id))
                {
                    Card card = cardRepository.GetSingleCardByCode(item.Notify1Id, tenant, true);
                    if (card != null)
                    {
                        Shipment.Notify1 = card.EnglishName;
                    }
                }

                if (!string.IsNullOrEmpty(item.Notify2Id))
                {
                    Card card = cardRepository.GetSingleCardByCode(item.Notify2Id, tenant, true);
                    if (card != null)
                    {
                        Shipment.Notify2 = card.EnglishName;
                    }
                }

                if (!string.IsNullOrEmpty(item.ColoaderId))
                {
                    Card card = cardRepository.GetSingleCardByCode(item.ColoaderId, tenant, true);
                    if (card != null)
                    {
                        Shipment.CoLoader = card.EnglishName;
                    }
                }

                if (!string.IsNullOrEmpty(item.FreightForwarderId))
                {
                    Card card = cardRepository.GetSingleCardByCode(item.FreightForwarderId, tenant, true);
                    if (card != null)
                    {
                        Shipment.FreightForwarder = card.EnglishName;
                    }
                }


                if (!string.IsNullOrEmpty(item.ConsolidatorId))
                {
                    Card card = cardRepository.GetSingleCardByCode(item.ConsolidatorId, tenant, true);
                    if (card != null)
                    {
                        Shipment.Consolidator = card.EnglishName;
                    }
                }



                if (!string.IsNullOrEmpty(item.CustomerId))
                {
                    Card card = cardRepository.GetSingleCardByCode(item.CustomerId, tenant, true);
                    if (card != null)
                    {
                        Shipment.Customer = card.EnglishName;
                    }
                }

                if (!string.IsNullOrEmpty(item.ShipperNotExporterId))
                {
                    Card card = cardRepository.GetSingleCardByCode(item.ShipperNotExporterId, tenant, true);
                    if (card != null)
                    {
                        Shipment.ShippernotExporter = card.EnglishName;
                    }
                }

                if (!string.IsNullOrEmpty(item.AgentId))
                {
                    Card card = cardRepository.GetSingleCardByCode(item.AgentId, tenant, true);
                    if (card != null)
                    {
                        Shipment.Agent = card.EnglishName;
                    }
                }


                if (!string.IsNullOrEmpty(item.ReleasingAgentId))
                {
                    Card card = cardRepository.GetSingleCardByCode(item.ReleasingAgentId, tenant, true);
                    if (card != null)
                    {
                        Shipment.ReleasingAgent = card.EnglishName;
                    }
                }


                #endregion

                #region Packages Section
                List<ShipmentPackage> shipmentPackages = (from d in shipmentsContext.ShipmentPackages where d.ShipmentId== item.Id select d).ToList();
                string packageType = "";
                string shipperSeal = "";
                string containerNumber = "";

                shipmentPackages.ForEach(package =>
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

                Shipment.TotalPieces = item.NumberOfPackages; 
                Shipment.Volume = shipmentPackages.Sum(p => p.Volume);
                Shipment.GrossWeight = shipmentPackages.Sum(p => p.Weight);
                Shipment.VolumetricWeight = shipmentPackages.Sum(p => p.VolumetricWeight);
                Shipment.Ratio = item.Ratio;
                Shipment.DescriptionofGoods = item.DescriptionOfGoods;
                #endregion


                #region Routing Section



                //ShipmentPickUpDelivery myFirstPickup
                //  = (from d in shipmentsContext.ShipmentPickUpDeliveries
                //     where d.ShipmentId == shipment.Id && d.PickUpDeliveryTypeCode == "PICK"
                //     select d).OrderBy(s => s.PickUpDeliveryNumber).FirstOrDefault();




                //Shipment.PickupFromPartner= 
                //Shipment.PickupFromPartnerAddress
                //Shipment.PickupToPartner
                //Shipment.PickupToPartnerAddress
                Shipment.PickupExpectedDeparture = item.FirstPickupETD; //Check
                Shipment.PickupExpectedArrival = item.FirstPickupETA; //Check
                                                                      //Shipment.PickupActualDeparture
                                                                      //Shipment.PickupToPort
                                                                      //Shipment.PickupActualArrival
                                                                      //    Shipment.MainCarriageLeg1LoadingPort = item.MainCarriageCarrierCode; // Check
                                                                      // Shipment.MainCarriageLeg1ViaPort1 = item.maincarriagecarrier

                myDataProvider.Shipments.Add(Shipment);
                #endregion


            });

            return myDataProvider;
        }

    }
}