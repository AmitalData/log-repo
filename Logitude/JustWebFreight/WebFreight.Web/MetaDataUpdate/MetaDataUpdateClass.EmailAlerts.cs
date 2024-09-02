using Logitude.BL.InfrastructureModel.EntityPMs;
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
        public void LoadEmailAlertSettings()
        {
            IWebFreightContext ObjectContext = WebFreightContext.GetContext(0);
            EmailAlertSettingRepository = new EmailAlertSettingRepository(ObjectContext);
            if (objectTabelQuery == null)
                objectTabelQuery = new Logitude.BL.InfrastructureModel.EntityQueries.ObjectTableQuery(0);
            List<ObjectTablePM> objectTables = objectTabelQuery.GetObjectPMsByTenant(0).ToList();

            #region object tables
            ObjectTablePM ActivityObjectTable = objectTables.Where(d => d.Name == "Activity").FirstOrDefault();
            ObjectTablePM OpportunityObjectTable = objectTables.Where(d => d.Name == "Opportunity").FirstOrDefault();
            ObjectTablePM CustomerObjectTable = objectTables.Where(d => d.Name == "Customer").FirstOrDefault();
            ObjectTablePM QuoteObjectTable = objectTables.Where(d => d.Name == "Quote").FirstOrDefault();
            ObjectTablePM BookingObjectTable = objectTables.Where(d => d.Name == "Booking").FirstOrDefault();
            #endregion

            Dictionary<string, EmailAlertSetting> tenantAlerts = EmailAlertSettingRepository.GetEmailAlertSettings(0).GroupBy(d => d.Code).ToDictionary(g => g.Key, a => a.FirstOrDefault());

            AddEmailAlertSettings.AddEmailAlertSetting(new EmailAlertSettingDetails() { Code = "OOPA", SettingLevelCode = "OWNR", Description = "Opportunity Assign", Tenant = 0, ObjectTableId = OpportunityObjectTable.Id, InActive = true, IndexOrder = 0 }, EmailAlertSettingRepository, tenantAlerts);
            AddEmailAlertSettings.AddEmailAlertSetting(new EmailAlertSettingDetails() { Code = "OOPS", SettingLevelCode = "OWNR", Description = "Opportunity Stage Update", Tenant = 0, ObjectTableId = OpportunityObjectTable.Id, InActive = true, IndexOrder = 1 }, EmailAlertSettingRepository, tenantAlerts);
            AddEmailAlertSettings.AddEmailAlertSetting(new EmailAlertSettingDetails() { Code = "OACA", SettingLevelCode = "OWNR", Description = "Appointment Assign", Tenant = 0, ObjectTableId = ActivityObjectTable.Id, InActive = true, IndexOrder = 2 }, EmailAlertSettingRepository, tenantAlerts);
            AddEmailAlertSettings.AddEmailAlertSetting(new EmailAlertSettingDetails() { Code = "OACT", SettingLevelCode = "OWNR", Description = "Task Assign", Tenant = 0, ObjectTableId = ActivityObjectTable.Id, InActive = true, IndexOrder = 3 }, EmailAlertSettingRepository, tenantAlerts);
            AddEmailAlertSettings.AddEmailAlertSetting(new EmailAlertSettingDetails() { Code = "OACP", SettingLevelCode = "OWNR", Description = "Phone Call Assign", Tenant = 0, ObjectTableId = ActivityObjectTable.Id, InActive = true, IndexOrder = 4 }, EmailAlertSettingRepository, tenantAlerts);
            AddEmailAlertSettings.AddEmailAlertSetting(new EmailAlertSettingDetails() { Code = "OQTA", SettingLevelCode = "OWNR", Description = "Quote  Assign", Tenant = 0, ObjectTableId = QuoteObjectTable.Id, InActive = true, IndexOrder = 5 }, EmailAlertSettingRepository, tenantAlerts);
            AddEmailAlertSettings.AddEmailAlertSetting(new EmailAlertSettingDetails() { Code = "OMER", SettingLevelCode = "OWNR", Description = "Message Received", Tenant = 0, InActive = true, IndexOrder = 6 }, EmailAlertSettingRepository, tenantAlerts);
            AddEmailAlertSettings.AddEmailAlertSetting(new EmailAlertSettingDetails() { Code = "GNOP", SettingLevelCode = "GNRL", Description = "New Opportunity", Tenant = 0, ObjectTableId = OpportunityObjectTable.Id, InActive = true, IndexOrder = 6 }, EmailAlertSettingRepository, tenantAlerts);
            AddEmailAlertSettings.AddEmailAlertSetting(new EmailAlertSettingDetails() { Code = "GOPS", SettingLevelCode = "GNRL", Description = "Opportunity Stage Update", Tenant = 0, ObjectTableId = OpportunityObjectTable.Id, InActive = true, IndexOrder = 7 }, EmailAlertSettingRepository, tenantAlerts);
            AddEmailAlertSettings.AddEmailAlertSetting(new EmailAlertSettingDetails() { Code = "GOCW", SettingLevelCode = "GNRL", Description = "Opportunity Close Won", Tenant = 0, ObjectTableId = OpportunityObjectTable.Id, InActive = true, IndexOrder = 8 }, EmailAlertSettingRepository, tenantAlerts);
            AddEmailAlertSettings.AddEmailAlertSetting(new EmailAlertSettingDetails() { Code = "GOCL", SettingLevelCode = "GNRL", Description = "Opportunity Close Lost", Tenant = 0, ObjectTableId = OpportunityObjectTable.Id, InActive = true, IndexOrder = 9 }, EmailAlertSettingRepository, tenantAlerts);
            AddEmailAlertSettings.AddEmailAlertSetting(new EmailAlertSettingDetails() { Code = "GANA", SettingLevelCode = "GNRL", Description = "New Appointment", Tenant = 0, ObjectTableId = ActivityObjectTable.Id, InActive = true, IndexOrder = 10 }, EmailAlertSettingRepository, tenantAlerts);
            AddEmailAlertSettings.AddEmailAlertSetting(new EmailAlertSettingDetails() { Code = "GANT", SettingLevelCode = "GNRL", Description = "New Task", Tenant = 0, ObjectTableId = ActivityObjectTable.Id, InActive = true, IndexOrder = 11 }, EmailAlertSettingRepository, tenantAlerts);
            AddEmailAlertSettings.AddEmailAlertSetting(new EmailAlertSettingDetails() { Code = "GANP", SettingLevelCode = "GNRL", Description = "New Phone Call", Tenant = 0, ObjectTableId = ActivityObjectTable.Id, InActive = true, IndexOrder = 12 }, EmailAlertSettingRepository, tenantAlerts);
            AddEmailAlertSettings.AddEmailAlertSetting(new EmailAlertSettingDetails() { Code = "GAAC", SettingLevelCode = "GNRL", Description = "Appointment Closed", Tenant = 0, ObjectTableId = ActivityObjectTable.Id, InActive = true, IndexOrder = 13 }, EmailAlertSettingRepository, tenantAlerts);
            AddEmailAlertSettings.AddEmailAlertSetting(new EmailAlertSettingDetails() { Code = "GATC", SettingLevelCode = "GNRL", Description = "Task Closed", Tenant = 0, ObjectTableId = ActivityObjectTable.Id, InActive = true, IndexOrder = 14 }, EmailAlertSettingRepository, tenantAlerts);
            AddEmailAlertSettings.AddEmailAlertSetting(new EmailAlertSettingDetails() { Code = "GAPC", SettingLevelCode = "GNRL", Description = "Phone Call Closed", Tenant = 0, ObjectTableId = ActivityObjectTable.Id, InActive = true, IndexOrder = 15 }, EmailAlertSettingRepository, tenantAlerts);
            AddEmailAlertSettings.AddEmailAlertSetting(new EmailAlertSettingDetails() { Code = "GNQT", SettingLevelCode = "GNRL", Description = "New Quote", Tenant = 0, ObjectTableId = QuoteObjectTable.Id, InActive = true, IndexOrder = 16 }, EmailAlertSettingRepository, tenantAlerts);
            AddEmailAlertSettings.AddEmailAlertSetting(new EmailAlertSettingDetails() { Code = "GQTA", SettingLevelCode = "GNRL", Description = "Quote Accepted", Tenant = 0, ObjectTableId = QuoteObjectTable.Id, InActive = true, IndexOrder = 17 }, EmailAlertSettingRepository, tenantAlerts);
            AddEmailAlertSettings.AddEmailAlertSetting(new EmailAlertSettingDetails() { Code = "GQTD", SettingLevelCode = "GNRL", Description = "Quote Declined", Tenant = 0, ObjectTableId = QuoteObjectTable.Id, InActive = true, IndexOrder = 18 }, EmailAlertSettingRepository, tenantAlerts);
            AddEmailAlertSettings.AddEmailAlertSetting(new EmailAlertSettingDetails() { Code = "GCAC", SettingLevelCode = "GNRL", Description = "Customer to be activated", Tenant = 0, ObjectTableId = CustomerObjectTable.Id, InActive = true, IndexOrder = 19 }, EmailAlertSettingRepository, tenantAlerts);
            AddEmailAlertSettings.AddEmailAlertSetting(new EmailAlertSettingDetails() { Code = "GCFS", SettingLevelCode = "GNRL", Description = "Customer First Shipment", Tenant = 0, ObjectTableId = CustomerObjectTable.Id, InActive = true, IndexOrder = 20 }, EmailAlertSettingRepository, tenantAlerts);
            AddEmailAlertSettings.AddEmailAlertSetting(new EmailAlertSettingDetails() { Code = "GCFI", SettingLevelCode = "GNRL", Description = "Customer First Invoice", Tenant = 0, ObjectTableId = CustomerObjectTable.Id, InActive = true, IndexOrder = 21 }, EmailAlertSettingRepository, tenantAlerts);
            AddEmailAlertSettings.AddEmailAlertSetting(new EmailAlertSettingDetails() { Code = "BOKC", SettingLevelCode = "GNRL", Description = "Booking Confirmed", Tenant = 0, ObjectTableId = BookingObjectTable.Id, InActive = false, IndexOrder = 22 }, EmailAlertSettingRepository, tenantAlerts);

            EmailAlertSettingRepository.SubmitChanges();
        }
    }
}