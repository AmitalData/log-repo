using Logitude.TimeManagement.Data;
using Logitude.TimeManagement.Data.EntityPOCOs;
using Logitude.TimeManagement.Data.Repositories;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
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

        public UnicargoExportManager(byte[] xmlFilters, int tenant)
        {
            this.tenant = tenant;
            shipmentsContext = ShipmentsContext.GetContext(tenant);
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

            ShipmentRepository shipmentRepository = new ShipmentRepository(shipmentsContext);
            List<ShipmentDataView> shipments = shipmentRepository.GetShipmentViewsByTenant(tenant).ToList();
            shipments.ForEach(item =>
            {
                UnicargoExport Shipment = new UnicargoExport();

                #region  General Section
                Shipment.House = item.House;
                Shipment.Incoterms = item.IncotermCode; // Check
                Shipment.MainHarmonize = item.MainHarmonize;
                Shipment.Type = item.ShipmentType;
                Shipment.Status = item.ShipmentStatusName;
                Shipment.TransportMode = item.TransportModeName;
                Shipment.Direction = item.DirectionName;
                Shipment.FileNumber = item.CustomFileNumber; // Check
                Shipment.ShipmentLevel = item.ShipmentLevelName;
                // Shipment.AdditionalAirwayBill = item.AdditionalAirwayBill; // Check
                //  Shipment.OpStatus = item.OpStatus; // Check
                // Shipment.CargoReadyDate = item.CargoReadyDate; // check
                Shipment.LFD = item.LastFinalDestination; // Check
                Shipment.AvailableDate = item.TerminalAvailable; //Check
                Shipment.Openedby = item.CreatedByUserName; // Check
                Shipment.Salesman = item.SalesmanUserName; //check
                Shipment.AccountManager = item.AccountManagerUserName; // check
                Shipment.Department = item.DepartmentId; // Set Name
                Shipment.Branch = item.BranchName;
                Shipment.SpecialServiceType = item.SpecialServicesTypeName;
                Shipment.ValueofGoods = item.ValueOfGoods;
                Shipment.Comments = item.Notes; //check
                // Shipment.PaymentStatus = item.PaymentStatus; //status
                // Shipment.LeadType = item.LeadType; //check
                // Shipment.Handler = item.Handler; //check
                Shipment.CreateDate = item.CreateDateTime;
                //Shipment.PickupRef = item.ShipmentPickUpIndex; //check
                #endregion

                #region Partners Section
                Shipment.Shipper = item.ShipperName;
                Shipment.Consignee = item.ConsigneeName;
                Shipment.Notify1 = item.Notify1Name;
                Shipment.Notify2 = item.Notify2Name;
                //Shipment.CoLoader = item.CoLoader; Add To Shipment Views
                Shipment.FreightForwarder = item.FreightForwarderName;
                Shipment.Consolidator = item.ConsolidatorName;
                Shipment.Customer = item.CustomerName;
                Shipment.ShippernotExporter = item.ShipperNotExporterName;
                Shipment.Agent = item.AgentName;
                Shipment.ReleasingAgent = item.ReleasingAgentName;

                #endregion

                #region Packages Section
                //Shipment.PackageType = item.PackageType; // Check
                // Shipment.TotalPieces = item.TotalPieces; // Add
                Shipment.Volume = item.Volume;
                Shipment.GrossWeight = item.GrossWeight;
                Shipment.VolumetricWeight = item.VolumetricWeight;
                Shipment.Ratio = item.Ratio;
                // Shipment.ShipperSeal = item.ShipperSeal; //Check
                Shipment.DescriptionofGoods = item.DescriptionOfGoods;
                // Shipment.ContinerNumber = item.NumberOfContainers; // Check  the array implode that you did for us (for the email template variable)
                #endregion


                #region Routing Section

                //Shipment.PickupFromPartner= 
                //Shipment.PickupFromPartnerAddress
                //Shipment.PickupToPartner
                //Shipment.PickupToPartnerAddress
                Shipment.PickupExpectedDeparture = item.FirstPickupETD; //Check
                Shipment.PickupExpectedArrival = item.FirstPickupETA; //Check
                //Shipment.PickupActualDeparture
                //Shipment.PickupToPort
                //Shipment.PickupActualArrival
                Shipment.MainCarriageLeg1LoadingPort = item.MainCarriageCarrierCode; // Check
               // Shipment.MainCarriageLeg1ViaPort1 = item.maincarriagecarrier


                #endregion


            });

            return myDataProvider;
        }

    }
}