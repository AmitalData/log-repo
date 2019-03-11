using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
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

namespace WebFreight.Web.ReportsWebServices.LogitudeReports
{
    public class VehiclesManager
    {
        private int tenant;

        private DateTime? FromDate = null;
        private DateTime? ToDate = null;
        private string CustomerId = null;
        private string TransportMode = null;
        private string Direction = null;
        private string Packagetype = null;
        private bool IsByCreateDate = true;


        public VehiclesManager(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;

            MemoryStream memoryStream = new MemoryStream(xmlFilters);
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations myQueryOperations = (QueryOperations)xmlSerializer.Deserialize(memoryStream);

            QueryFilterItem filterItem_FromDate = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "FromDate").FirstOrDefault();
            QueryFilterItem filterItem_ToDate = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "ToDate").FirstOrDefault();
            QueryFilterItem filterItem_CustomerId = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "BillToId").FirstOrDefault();
            QueryFilterItem filterItem_TransportMode = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "TransportMode").FirstOrDefault();
            QueryFilterItem filterItem_Direction = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "Direction").FirstOrDefault();
            QueryFilterItem filterItem_Packagetype = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "Packagetype").FirstOrDefault();
            QueryFilterItem filterItem_IsByCreateDate = myQueryOperations.QueryFilterItems.Where(d => d.FieldName == "IsByCreateDate").FirstOrDefault();

            DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
            DateTime myStartDate = todayDate.AddMonths(-1);
            DateTime fromDate = new DateTime(myStartDate.Year, myStartDate.Month, 1);
            DateTime toDate = new DateTime(todayDate.Year, todayDate.Month, DateTime.DaysInMonth(todayDate.Year, todayDate.Month));

            if (filterItem_IsByCreateDate != null)
            {
                if (filterItem_IsByCreateDate.FieldValue != null)
                {
                    IsByCreateDate = (bool)filterItem_IsByCreateDate.FieldValue;
                }
            }

            if (filterItem_FromDate != null)
            {
                DateTime date;
                bool isValid = DateTime.TryParse(filterItem_FromDate.FieldValue.ToString(), out date);
                if (isValid)
                {
                    this.FromDate = date;
                }
            }

            if (filterItem_ToDate != null)
            {
                DateTime date;
                bool isValid = DateTime.TryParse(filterItem_ToDate.FieldValue.ToString(), out date);
                if (isValid)
                {
                    this.ToDate = date;
                }
            }

            if (filterItem_CustomerId != null)
            {
                if (filterItem_CustomerId.FieldValue != null)
                {
                    CustomerId = filterItem_CustomerId.FieldValue.ToString();
                }
            }


            if (filterItem_TransportMode != null)
            {
                if (filterItem_TransportMode.FieldValue != null)
                {
                    TransportMode = filterItem_TransportMode.FieldValue.ToString();
                }
            }

            if (filterItem_Direction != null)
            {
                if (filterItem_Direction.FieldValue != null)
                {
                    Direction = filterItem_Direction.FieldValue.ToString();
                }
            }

            if (filterItem_Packagetype != null)
            {
                if (filterItem_Packagetype.FieldValue != null)
                {
                    Packagetype = filterItem_Packagetype.FieldValue.ToString();
                }
            }


        }

        public byte[] GetData()
        {
            VehiclesDataProvider myDataProvider = new VehiclesDataProvider();

            myDataProvider = this.LoadDataProvider();

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(VehiclesDataProvider));
            MemoryStream memoryStream = new MemoryStream();
            xmlSerializer.Serialize(memoryStream, myDataProvider);
            memoryStream.Seek(0, SeekOrigin.Begin);
            StreamReader streamReader = new StreamReader(memoryStream);
            string content = streamReader.ReadToEnd();
            byte[] bytearray = memoryStream.ToArray();
            return bytearray;
        }

        private VehiclesDataProvider LoadDataProvider()
        {
            VehiclesDataProvider myDataProvider = new VehiclesDataProvider();
            myDataProvider.ShipmentPackages = new List<VehiclePackageRecord>();


            IShipmentsContext context = ShipmentsContext.GetContext(tenant);
            ICommonDataContext Commoncontext = CommonDataContext.GetContext(tenant);
            IWebFreightContext webFreightContext = WebFreightContext.GetContext(tenant);
            ShipmentRepository shipmentRepository = new ShipmentRepository(context);
            CardRepository cardRepository = new CardRepository(Commoncontext);
            EntityStatusRepository entityRepository = new EntityStatusRepository(webFreightContext);
            PortRepository portRepository = new PortRepository(Commoncontext);
            CountryRepository countryRepository = new CountryRepository(Commoncontext);
            IQueryable<Shipment> iQueryable_shipments = (from d in context.Shipments where d.Tenant == tenant select d).AsQueryable();
            if (!string.IsNullOrEmpty(Direction) && Direction != "All")
            {
                iQueryable_shipments = iQueryable_shipments.Where(d => d.DirectionId == Direction);
            }

            if (!string.IsNullOrEmpty(TransportMode) && TransportMode != "All")
            {
                iQueryable_shipments = iQueryable_shipments.Where(d => d.TransportModeId == TransportMode);
            }

            if (IsByCreateDate)
            {
                if (FromDate != null)
                {
                    iQueryable_shipments = iQueryable_shipments.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) >= System.Data.Entity.DbFunctions.TruncateTime(FromDate));
                }

                if (ToDate != null)
                {
                    iQueryable_shipments = iQueryable_shipments.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.CreateDateTime) <= System.Data.Entity.DbFunctions.TruncateTime(ToDate));
                }
            }
            else
            {
                if (FromDate != null)
                {
                    iQueryable_shipments = iQueryable_shipments.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.OperationalDate) >= System.Data.Entity.DbFunctions.TruncateTime(FromDate));
                }

                if (ToDate != null)
                {
                    iQueryable_shipments = iQueryable_shipments.Where(d => System.Data.Entity.DbFunctions.TruncateTime(d.OperationalDate) <= System.Data.Entity.DbFunctions.TruncateTime(ToDate));
                }
            }

            if (!string.IsNullOrEmpty(CustomerId))
            {
                iQueryable_shipments = iQueryable_shipments.Where(d => d.CustomerId == CustomerId);
            }




            IQueryable<ShipmentPackage> shipmentPackages;
            IQueryable<InsideShipmentPackage> InsideShipmentPackages;

                shipmentPackages = (from d in context.ShipmentPackages.Include("PackageType") where d.Tenant == tenant select d);
                InsideShipmentPackages = (from d in context.InsideShipmentPackages.Include("PackageType") where d.Tenant == tenant select d);
            List< ShipmentPackageData> ShipmentPackagesAndInside = new List<ShipmentPackageData>();
            if (!string.IsNullOrEmpty(Packagetype))
            {
                     ShipmentPackagesAndInside = (from shipment in iQueryable_shipments
                                 join package in shipmentPackages on shipment.Id equals package.ShipmentId into shipmentpackage
                                 from dept in shipmentpackage.DefaultIfEmpty()
                                 join insidepackage in InsideShipmentPackages on dept.Id equals insidepackage.ShipmentPackageId into insideshipmentpackage
                                 from insidepackages in insideshipmentpackage.DefaultIfEmpty()
                                 join masterDatas in context.ShipmentMasterDatas on shipment.MasterShipmentDataId equals masterDatas.Id into masterShipment
                                 from master in masterShipment.DefaultIfEmpty()
                                 where (dept.PackageType.IsVehicle || insidepackages.PackageType.IsVehicle) && (dept.PackageTypeId== Packagetype || insidepackages.PackageTypeId== Packagetype)
                                 select new ShipmentPackageData()
                                 {

                                     ShipmentNumber = shipment.ShipmentNumber,
                                     CustomerId = shipment.CustomerId,
                                     StatusId = shipment.StatusId,
                                     POL = shipment.ShipmentLevelCode == "H" ? shipment.FromPortId : master.MainCarriageFromPortId,
                                     POD = shipment.ShipmentLevelCode == "H" ? shipment.ToPortId : master.MainCarriageFinalDestinationPortId,
                                     DepartualDate = master.MainCarriageATD != null ? master.MainCarriageATD : master.MainCarriageETD,
                                     DepartualDateIndication = master.MainCarriageATD != null ? "Actual" : "Expected",
                                     ArrivalDate = master.MainCarriageATA != null ? master.MainCarriageATA : master.MainCarriageETA,
                                     ArrivalDateIndication = master.MainCarriageATA != null ? "Actual" : "Expected",
                                     TransportModeId = shipment.TransportModeId,
                                     Carrier = master.MainCarriageCarrierId,
                                     VesselId = master.MainCarriageVesselId,
                                     CarrierNumber = master.MainCarriageCarrierNumber,
                                     CarrierPrefix = master.MainCarriageCarrierPrefix,
                                     MasterNumber = shipment.TransportModeId == "A" ? master.AirlinePrefix + master.Master : master.Master,
                                     HouseNumber = shipment.House,
                                     Make = shipment.ShipmentTypeId == "FCLD" || shipment.ShipmentTypeId == "FTL" ? (insidepackages != null ? insidepackages.Make : null) : (dept != null ? dept.Make : null),
                                     Model = shipment.ShipmentTypeId == "FCLD" || shipment.ShipmentTypeId == "FTL" ? (insidepackages != null ? insidepackages.Model : null) : (dept != null ? dept.Model : null),
                                     Year = shipment.ShipmentTypeId == "FCLD" || shipment.ShipmentTypeId == "FTL" ? (insidepackages != null ? insidepackages.Year : null) : (dept != null ? dept.Year : null),
                                     Color = shipment.ShipmentTypeId == "FCLD" || shipment.ShipmentTypeId == "FTL" ? (insidepackages != null ? insidepackages.Color : null) : (dept != null ? dept.Color : null),
                                     ChassisNumber = shipment.ShipmentTypeId == "FCLD" || shipment.ShipmentTypeId == "FTL" ? (insidepackages != null ? insidepackages.ChassisNumber : null) : (dept != null ? dept.ChassisNumber : null),
                                     RegistrationNumber = shipment.ShipmentTypeId == "FCLD" || shipment.ShipmentTypeId == "FTL" ? (insidepackages != null ? insidepackages.RegistrationNumber : null) : (dept != null ? dept.RegistrationNumber : null),
                                     CountryofManufacture = shipment.ShipmentTypeId == "FCLD" || shipment.ShipmentTypeId == "FTL" ? (insidepackages != null ? insidepackages.CountryId : null) : (dept != null ? dept.CountryId : null),
                                     ContainerNumber = dept != null ? dept.ContainerNumber : null,
                                     ContainerType = shipment.ShipmentTypeId == "FCLD" || shipment.ShipmentTypeId == "FTL" ? (insidepackages != null ? insidepackages.PackageType.Code : null) : (dept != null ? dept.PackageType.Code : null),
                                     VehicleType = shipment.ShipmentTypeId == "FCLD" || shipment.ShipmentTypeId == "FTL" ? (insidepackages != null ? insidepackages.PackageType.EnglishName : null) : (dept != null ? dept.PackageType.EnglishName : null),
                                 }).ToList();

            }
            else
            {
                 ShipmentPackagesAndInside = (from shipment in iQueryable_shipments
                                                 join package in shipmentPackages on shipment.Id equals package.ShipmentId into shipmentpackage
                                                 from dept in shipmentpackage.DefaultIfEmpty()
                                                 join insidepackage in InsideShipmentPackages on dept.Id equals insidepackage.ShipmentPackageId into insideshipmentpackage
                                                 from insidepackages in insideshipmentpackage.DefaultIfEmpty()
                                                 join masterDatas in context.ShipmentMasterDatas on shipment.MasterShipmentDataId equals masterDatas.Id into masterShipment
                                                 from master in masterShipment.DefaultIfEmpty()
                                                 where (dept.PackageType.IsVehicle || insidepackages.PackageType.IsVehicle) 
                                                 select new ShipmentPackageData()

                                                 {

                                                     ShipmentNumber = shipment.ShipmentNumber,
                                                     CustomerId = shipment.CustomerId,
                                                     StatusId = shipment.StatusId,
                                                     POL = shipment.ShipmentLevelCode == "H" ? shipment.FromPortId : master.MainCarriageFromPortId,
                                                     POD = shipment.ShipmentLevelCode == "H" ? shipment.ToPortId : master.MainCarriageFinalDestinationPortId,
                                                     DepartualDate = master.MainCarriageATD != null ? master.MainCarriageATD : master.MainCarriageETD,
                                                     DepartualDateIndication = master.MainCarriageATD != null ? "Actual" : "Expected",
                                                     ArrivalDate = master.MainCarriageATA != null ? master.MainCarriageATA : master.MainCarriageETA,
                                                     ArrivalDateIndication = master.MainCarriageATA != null ? "Actual" : "Expected",
                                                     TransportModeId = shipment.TransportModeId,
                                                     Carrier = master.MainCarriageCarrierId,
                                                     VesselId = master.MainCarriageVesselId,
                                                     CarrierNumber = master.MainCarriageCarrierNumber,
                                                     CarrierPrefix = master.MainCarriageCarrierPrefix,
                                                     MasterNumber = shipment.TransportModeId == "A" ? master.AirlinePrefix + master.Master : master.Master,
                                                     HouseNumber = shipment.House,
                                                     Make = shipment.ShipmentTypeId == "FCLD" || shipment.ShipmentTypeId == "FTL" ? (insidepackages != null ? insidepackages.Make : null) : (dept != null ? dept.Make : null),
                                                     Model = shipment.ShipmentTypeId == "FCLD" || shipment.ShipmentTypeId == "FTL" ? (insidepackages != null ? insidepackages.Model : null) : (dept != null ? dept.Model : null),
                                                     Year = shipment.ShipmentTypeId == "FCLD" || shipment.ShipmentTypeId == "FTL" ? (insidepackages != null ? insidepackages.Year : null) : (dept != null ? dept.Year : null),
                                                     Color = shipment.ShipmentTypeId == "FCLD" || shipment.ShipmentTypeId == "FTL" ? (insidepackages != null ? insidepackages.Color : null) : (dept != null ? dept.Color : null),
                                                     ChassisNumber = shipment.ShipmentTypeId == "FCLD" || shipment.ShipmentTypeId == "FTL" ? (insidepackages != null ? insidepackages.ChassisNumber : null) : (dept != null ? dept.ChassisNumber : null),
                                                     RegistrationNumber = shipment.ShipmentTypeId == "FCLD" || shipment.ShipmentTypeId == "FTL" ? (insidepackages != null ? insidepackages.RegistrationNumber : null) : (dept != null ? dept.RegistrationNumber : null),
                                                     CountryofManufacture = shipment.ShipmentTypeId == "FCLD" || shipment.ShipmentTypeId == "FTL" ? (insidepackages != null ? insidepackages.CountryId : null) : (dept != null ? dept.CountryId : null),
                                                     ContainerNumber = dept != null ? dept.ContainerNumber : null,
                                                     ContainerType = shipment.ShipmentTypeId == "FCLD" || shipment.ShipmentTypeId == "FTL" ? (insidepackages != null ? insidepackages.PackageType.Code : null) : (dept != null ? dept.PackageType.Code : null),
                                                     VehicleType = shipment.ShipmentTypeId == "FCLD" || shipment.ShipmentTypeId == "FTL" ? (insidepackages != null ? insidepackages.PackageType.EnglishName : null) : (dept != null ? dept.PackageType.EnglishName : null),
                                                 }).ToList();

            }
                 


            
            

            ShipmentPackagesAndInside.ForEach(item =>
            {
                VehiclePackageRecord package = new VehiclePackageRecord();
                package.ShipmentNumber = item.ShipmentNumber;
                package.MasterNumber = item.MasterNumber;
                package.HouseNumber = item.HouseNumber;
                package.Make = item.Make;
                package.Model = item.Model;
                package.Year = item.Year;
                package.Color = item.Color;
                package.ChassisNumber = item.ChassisNumber;
                package.RegistrationNumber = item.RegistrationNumber;
                package.ContainerNumber = item.ContainerNumber;
                package.ContainerType = item.ContainerType;
                package.VehicleType = item.VehicleType;

                if (item.CustomerId!=null)
                {
                    Card Customer = cardRepository.GetSingleCard(item.CustomerId, tenant);
                    if (Customer != null)
                    {
                        package.Customer = Customer.EnglishName;
                    }

                }


                if (item.StatusId != null)
                {
                    EntityStatus Status = entityRepository.GetSingleEntityStatus(item.StatusId, tenant);
                    if (Status != null)
                    {
                        package.Status = Status.Name;
                    }

                }

                if (item.POL != null)
                {
                    Port port = portRepository.GetSinglePort(item.POL, tenant);
                    if (port != null)
                    {
                        package.POL = port.EnglishName;
                    }

                }

                if (item.POD != null)
                {
                    Port port = portRepository.GetSinglePort(item.POD, tenant);
                    if (port != null)
                    {
                        package.POD = port.EnglishName;
                    }
                }



                if (item.DepartualDate != null)
                {
                    package.DepartureDate = item.DepartualDate;
                    package.DepartualDateIndication= item.DepartualDateIndication;                   
                }
                else
                {
                    package.DepartualDateIndication = null;
                }

                if (item.ArrivalDate != null)
                {
                    package.ArrivalDate = item.ArrivalDate;
                    package.ArrivalDateIndication = item.ArrivalDateIndication;
                }
                else
                {
                    package.ArrivalDateIndication = null;
                }

                if (item.CountryofManufacture != null)
                {
                    Country country = countryRepository.GetSingleCountry(item.CountryofManufacture, tenant);
                    if (country != null)
                    {
                        package.CountryofManufacture = country.EnglishName;
                    }
                }


                if (item.Carrier != null)
                {
                    Card card = cardRepository.GetSingleCard(item.Carrier, tenant);
                    if (card != null)
                    {
                        package.Carrier = card.EnglishName;
                    }
                }

                
                if (item.TransportModeId=="O")
                {

                    Vessel vessel = (from a in Commoncontext.Vessels where a.Id == item.VesselId && a.Tenant == tenant select a).FirstOrDefault();
                     package.CarrierNumber =( vessel!=null?( vessel.EnglishName + " / "):"")+(item.CarrierPrefix!=null? item.CarrierPrefix :"" )+ (item.CarrierNumber!=null? item.CarrierNumber:"");
                    
                }
                else
                {
                    package.CarrierNumber = item.CarrierPrefix+item.CarrierNumber;
                }
                myDataProvider.ShipmentPackages.Add(package);

            });





            return myDataProvider;
        }
    }

    public class ShipmentPackageData
    {
        public string ShipmentNumber { get; set; }
        public string CustomerId { get; set; }
        public string StatusId { get; set; }
        public string POL { get; set; }
        public string POD { get; set; }
        public DateTime? DepartualDate { get; set; }
        public string DepartualDateIndication { get; set; }
        public DateTime? ArrivalDate { get; set; }
        public string ArrivalDateIndication { get; set; }
        public string TransportModeId { get; set; }
        public string Carrier { get; set; }
        public string VesselId { get; set; }
        public string CarrierNumber { get; set; }
        public string CarrierPrefix { get; set; }
        public string MasterNumber { get; set; }
        public string HouseNumber { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public string Year { get; set; }
        public string Color { get; set; }
        public string ChassisNumber { get; set; }
        public string RegistrationNumber { get; set; }
        public string CountryofManufacture { get; set; }
        public string ContainerNumber { get; set; }
        public string ContainerType { get; set; }
        public string VehicleType { get; set; }
     
    }
}