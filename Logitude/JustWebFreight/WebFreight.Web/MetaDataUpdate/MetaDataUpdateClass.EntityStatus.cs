using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebFreight.Web.MetaDataUpdate.AddClasses;
using WebFreight.Web.MetaDataUpdate.DetailClasses;

namespace WebFreight.Web.MetaDataUpdate
{
    public partial class MetaDataUpdateClass
    {
        public void LoadEntityStatus()
        {
            ObjectContext = WebFreightContext.GetContext(0);
            EntityStatusRepository = new EntityStatusRepository(ObjectContext);

            ObjectTablePM shipmentObject = ObjectTableQuery.GetObjectTableByCode("Shipment", 0);
            ObjectTablePM customerObject = ObjectTableQuery.GetObjectTableByCode("Customer", 0);
            ObjectTablePM containerObject = ObjectTableQuery.GetObjectTableByCode("Container", 0);

            Dictionary<string, EntityStatus> tenantEntityStatus = EntityStatusRepository.GetEntityStatusByTenant(0).ToDictionary(d => d.Code, a => a);

            #region Shipment Status
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "SHOR", StatusWeight = 0, Name = "Order", ObjectTableId = shipmentObject.Id, Tenant = 0, DisplayName = "Order" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "SHPK", StatusWeight = 10, Name = "Pick Up", ObjectTableId = shipmentObject.Id, Tenant = 0, DisplayName = "Picked Up" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "SHP2", StatusWeight = 10, Name = "Pick Up", ObjectTableId = shipmentObject.Id, Tenant = 0, DisplayName = "Pick Up Arranged" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "SHON", StatusWeight = 20, Name = "On Hand", ObjectTableId = shipmentObject.Id, Tenant = 0, DisplayName = "On Hand" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "ECCL", StatusWeight = 30, Name = "Export Custom Clearance", ObjectTableId = shipmentObject.Id, Tenant = 0, DisplayName = "Export Custom Clearance" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "ETD", StatusWeight = 40, Name = "ETD", ObjectTableId = shipmentObject.Id, Tenant = 0, DisplayName = "ETD" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "ETA", StatusWeight = 50, Name = "ETA", ObjectTableId = shipmentObject.Id, Tenant = 0, DisplayName = "ETA" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "SDE5", StatusWeight = 60, Name = "Departed", ObjectTableId = shipmentObject.Id, Tenant = 0, DisplayName = "Pre Carriage Departed" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "PRCA", StatusWeight = 70, Name = "Arrived", ObjectTableId = shipmentObject.Id, Tenant = 0, DisplayName = "Pre Carriage Arrived" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "SDEP", StatusWeight = 80, Name = "Departed", ObjectTableId = shipmentObject.Id, Tenant = 0, DisplayName = "Departed" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "SARR", StatusWeight = 90, Name = "Arrived", ObjectTableId = shipmentObject.Id, Tenant = 0, DisplayName = "Arrived at Destination" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "SDE2", StatusWeight = 100, Name = "Departed", ObjectTableId = shipmentObject.Id, Tenant = 0, DisplayName = "Leg 1 Departed" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "SAR2", StatusWeight = 110, Name = "Arrived", ObjectTableId = shipmentObject.Id, Tenant = 0, DisplayName = "Leg 1 Arrived" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "SDE3", StatusWeight = 120, Name = "Departed", ObjectTableId = shipmentObject.Id, Tenant = 0, DisplayName = "Leg 2 Departed" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "SAR3", StatusWeight = 130, Name = "Arrived", ObjectTableId = shipmentObject.Id, Tenant = 0, DisplayName = "Leg 2 Arrived" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "SDE4", StatusWeight = 140, Name = "Departed", ObjectTableId = shipmentObject.Id, Tenant = 0, DisplayName = "Leg 3 Departed" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "SAR4", StatusWeight = 150, Name = "Arrived", ObjectTableId = shipmentObject.Id, Tenant = 0, DisplayName = "Leg 3 Arrived" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "ONCD", StatusWeight = 160, Name = "Departed", ObjectTableId = shipmentObject.Id, Tenant = 0, DisplayName = "On Carriage Departed" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "SAR5", StatusWeight = 170, Name = "Arrived", ObjectTableId = shipmentObject.Id, Tenant = 0, DisplayName = "On Carriage Arrived" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "CERT", StatusWeight = 180, Name = "Pending Import Formalities", ObjectTableId = shipmentObject.Id, Tenant = 0, DisplayName = "Pending Import Formalities" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "DTCA", StatusWeight = 190, Name = "Delivered to Customs Agent", ObjectTableId = shipmentObject.Id, Tenant = 0, DisplayName = "Delivered to Customs Agent" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "ICCL", StatusWeight = 200, Name = "Import Custom Clearance", ObjectTableId = shipmentObject.Id, Tenant = 0,DisplayName = "Import Custom Clearance" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "SHCL", StatusWeight = 200, Name = "Cleared", ObjectTableId = shipmentObject.Id, Tenant = 0,DisplayName = "Cleared" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "SAFD", StatusWeight = 210, Name = "Available for Delivery", ObjectTableId = shipmentObject.Id, Tenant = 0, DisplayName = "Available for Delivery" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "SDLY", StatusWeight = 230, Name = "Delivery", ObjectTableId = shipmentObject.Id, Tenant = 0, DisplayName = "Delivery Departed" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "SDL2", StatusWeight = 230, Name = "Delivery", ObjectTableId = shipmentObject.Id, Tenant = 0, DisplayName = "Delivery Arranged" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "SDLD", StatusWeight = 240, Name = "Delivered", ObjectTableId = shipmentObject.Id, Tenant = 0, DisplayName = "Delivered" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "INPS", StatusWeight = 250, Name = "In Progress", ObjectTableId = shipmentObject.Id, Tenant = 0, DisplayName = "In Progress" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "DDAP", StatusWeight = 250, Name = "Declaration Data Approved", ObjectTableId = shipmentObject.Id, Tenant = 0, DisplayName = "Declaration Data Approved" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "DDDE", StatusWeight = 250, Name = "Declaration Data Denied", ObjectTableId = shipmentObject.Id, Tenant = 0, DisplayName = "Declaration Data Denied" }, EntityStatusRepository, tenantEntityStatus);

            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "INWH", StatusWeight = 205, Name = "Storage Entry", ObjectTableId = shipmentObject.Id, Tenant = 0, DisplayName = "Storage Entry" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "SEEX", StatusWeight = 40, Name = "Storage Entry", ObjectTableId = shipmentObject.Id, Tenant = 0, DisplayName = "Storage Entry" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "SRIM", StatusWeight = 225, Name = "Storage Released", ObjectTableId = shipmentObject.Id, Tenant = 0, DisplayName = "Storage Released" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "SREX", StatusWeight = 50, Name = "Storage Released", ObjectTableId = shipmentObject.Id, Tenant = 0, DisplayName = "Storage Released" }, EntityStatusRepository, tenantEntityStatus);

            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "BKAR", StatusWeight = 5, Name = "Booking arrangement", ObjectTableId = shipmentObject.Id, Tenant = 0, DisplayName = "Booking arrangement" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "CRFP", StatusWeight = 10, Name = "Cargo Ready for Pickup", ObjectTableId = shipmentObject.Id, Tenant = 0, DisplayName = "Cargo Ready for Pickupt" }, EntityStatusRepository, tenantEntityStatus);

            #endregion

            #region Customer Status
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "CSCR", Name = "Created", ObjectTableId = customerObject.Id, StatusWeight = 0, Tenant = 0, DisplayName = "Created" }, EntityStatusRepository, tenantEntityStatus);
            #endregion

            #region Container
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "EMPS", StatusWeight = 10, Name = "Empty to Shipper", ObjectTableId = containerObject.Id, Tenant = 0, DisplayName = "Empty to Shipper" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "PICS", StatusWeight = 20, Name = "Picked up at Shipper", ObjectTableId = containerObject.Id, Tenant = 0, DisplayName = "Picked up at Shipper" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "GTIN", StatusWeight = 30, Name = "Gate In", ObjectTableId = containerObject.Id, Tenant = 0, DisplayName = "Gate In" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "PCDP", StatusWeight = 40, Name = "Pre Carriage Departed", ObjectTableId = containerObject.Id, Tenant = 0, DisplayName = "Pre Carriage Departed" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "PCAV", StatusWeight = 50, Name = "Pre Carriage Arrived", ObjectTableId = containerObject.Id, Tenant = 0, DisplayName = "Pre Carriage Arrived" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "POLD", StatusWeight = 60, Name = "POL Departed", ObjectTableId = containerObject.Id, Tenant = 0, DisplayName = "POL Departed" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "T1AV", StatusWeight = 70, Name = "Transshipment  1 Arrived", ObjectTableId = containerObject.Id, Tenant = 0, DisplayName = "Transshipment  1 Arrived" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "T1DT", StatusWeight = 80, Name = "Transshipment1 Departed", ObjectTableId = containerObject.Id, Tenant = 0, DisplayName = "Transshipment1 Departed" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "T2AV", StatusWeight = 90, Name = "Transshipment 2 Arrived", ObjectTableId = containerObject.Id, Tenant = 0, DisplayName = "Transshipment 2 Arrived" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "T2DT", StatusWeight = 100, Name = "Transshipment 2 Departed", ObjectTableId = containerObject.Id, Tenant = 0, DisplayName = "Transshipment 2 Departed" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "T3AV", StatusWeight = 110, Name = "Transshipment 3 Arrived", ObjectTableId = containerObject.Id, Tenant = 0, DisplayName = "Transshipment 3 Arrived" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "T3DT", StatusWeight = 120, Name = "Transshipment 3 Departed", ObjectTableId = containerObject.Id, Tenant = 0, DisplayName = "Transshipment 3 Departed" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "ARPD", StatusWeight = 130, Name = "Arrived at POD", ObjectTableId = containerObject.Id, Tenant = 0, DisplayName = "Arrived at POD" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "DSCH", StatusWeight = 140, Name = "Discharged", ObjectTableId = containerObject.Id, Tenant = 0, DisplayName = "Discharged" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "APAR", StatusWeight = 150, Name = "Appointment Arranged", ObjectTableId = containerObject.Id, Tenant = 0, DisplayName = "Appointment Arranged" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "GTOT", StatusWeight = 160, Name = "Gate Out", ObjectTableId = containerObject.Id, Tenant = 0, DisplayName = "Gate Out" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "ARWH", StatusWeight = 170, Name = "Arrived to Warehouse", ObjectTableId = containerObject.Id, Tenant = 0, DisplayName = "Arrived to Warehouse" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "AVDL", StatusWeight = 180, Name = "Available for Delivery", ObjectTableId = containerObject.Id, Tenant = 0, DisplayName = "Available for Delivery" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "DPWH", StatusWeight = 190, Name = "Departed from Warehouse", ObjectTableId = containerObject.Id, Tenant = 0, DisplayName = "Departed from Warehouse" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "DLCO", StatusWeight = 200, Name = "Delivered to Consignee", ObjectTableId = containerObject.Id, Tenant = 0, DisplayName = "Delivered to Consignee" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "UNDS", StatusWeight = 210, Name = "Unloaded at Destination", ObjectTableId = containerObject.Id, Tenant = 0, DisplayName = "Unloaded at Destination" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "PODC", StatusWeight = 220, Name = "POD", ObjectTableId = containerObject.Id, Tenant = 0, DisplayName = "POD" }, EntityStatusRepository, tenantEntityStatus);
            AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "EMRT", StatusWeight = 230, Name = "Empty Return", ObjectTableId = containerObject.Id, Tenant = 0, DisplayName = "Empty Return" }, EntityStatusRepository, tenantEntityStatus);

            #endregion

            #region Just for Testing
            if (Testing.General.IsTesting)
            {
                AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "TTTT", Name = "teset", ObjectTableId = shipmentObject.Id, StatusWeight = 5, Tenant = 0, DisplayName = "teset" }, EntityStatusRepository, tenantEntityStatus);
                //Update
                //AddEntityStatus.AddEntityStatu(new EntityStatusDetails() { Code = "QTNA", Name = "Update No Answer", IndexOrder = 4, ObjectTableId = shipmentObject.Id, StatusWeight = 5, Tenant = 0, }, EntityStatusRepository, tenantEntityStatus);
            }
            #endregion

            EntityStatusRepository.SubmitChanges();
        }
    }
}