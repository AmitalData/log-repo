using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
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

            ShipmentRepository shipmentRepository = new ShipmentRepository(context);

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


            IQueryable<ShipmentPackage> shipmentPackages = (from d in context.ShipmentPackages.Include("PackageType") where d.Tenant == tenant select d).AsQueryable();
            IQueryable<InsideShipmentPackage> InsideShipmentPackages = (from d in context.InsideShipmentPackages.Include("PackageType") where d.Tenant == tenant select d).AsQueryable();


            var ShipmentPackagesAndInside = (from shipment in iQueryable_shipments
                        join package in shipmentPackages on shipment.Id equals package.ShipmentId into shipmentpackage
                        from dept in shipmentpackage.DefaultIfEmpty()
                        join insidepackage in InsideShipmentPackages on dept.Id equals insidepackage.ShipmentPackageId into insideshipmentpackage
                        from insidepackages in insideshipmentpackage.DefaultIfEmpty()
                        select   new

                        {

                            ShipmentNumber=shipment.ShipmentNumber,

                            CustomerId = shipment.CustomerId,
                            ShipmentStatusId = shipment.StatusId,
                            POL = shipment.Origin,
                         //   POD = shipment.destinationport,
                           // DepartualDate = shipment.o,
                            ArrivalDate = shipment.FinalArrivalDate,
                            Carrier = shipment.OnCarriageCarrierId,
                            CarrierNumber = shipment.OnCarriageCarrierNumber,
                            MasterNumber = shipment.MasterShipmentDataId,
                            HouseNumber = shipment.House,
                            
                            val3 = shipment.FHLStatusCode,

                            val4 = dept.PackageTypeId,
                            val5 = dept.ContainerNumber,
                            val6 = insidepackages.ShipmentPackageId,
                            val7 = (int?)insidepackages.Tenant,


                        }).ToList();

            //var shipments = list.Where(a => a.val1 != null).ToList();
           // var shipmentpackages = list.Where(a => a.val4 != null).ToList();
            //var shipmentinsidepackages = list.Where(a => a.val6 != null || a.val7 != null).ToList();


            return myDataProvider;
        }
    }
}