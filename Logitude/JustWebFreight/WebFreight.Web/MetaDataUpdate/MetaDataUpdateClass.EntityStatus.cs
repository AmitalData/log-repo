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