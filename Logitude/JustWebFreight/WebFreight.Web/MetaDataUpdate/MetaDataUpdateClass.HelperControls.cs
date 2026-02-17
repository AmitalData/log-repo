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
        public void LoadObjectTableHelperControls()
        {
            ObjectContext = WebFreightContext.GetContext(0);
            ObjectTableHelperControlsRepository = new ObjectTableHelperControlRepository(ObjectContext);

            ObjectTable ShipmentTable = ObjectContext.ObjectTables.Where(f => f.Name == "Shipment" && f.Tenant == 0).FirstOrDefault();
            ObjectTable MasterTable = ObjectContext.ObjectTables.Where(f => f.Name == "Master" && f.Tenant == 0).FirstOrDefault();
            ObjectTable QuoteTable = ObjectContext.ObjectTables.Where(f => f.Name == "Quote" && f.Tenant == 0).FirstOrDefault();

            ObjectTable ARInvoiceTable = ObjectContext.ObjectTables.Where(f => f.Name == "ARInvoice" && f.Tenant == 0).FirstOrDefault();
            ObjectTable APInvoiceTable = ObjectContext.ObjectTables.Where(f => f.Name == "APInvoice" && f.Tenant == 0).FirstOrDefault();
            ObjectTable ARPaymentTable = ObjectContext.ObjectTables.Where(f => f.Name == "ARPayment" && f.Tenant == 0).FirstOrDefault();
            ObjectTable APPaymentTable = ObjectContext.ObjectTables.Where(f => f.Name == "APPayment" && f.Tenant == 0).FirstOrDefault();
            ObjectTable CustomerTable = ObjectContext.ObjectTables.Where(f => f.Name == "Customer" && f.Tenant == 0).FirstOrDefault();

            Dictionary<string, ObjectTableHelperControl> TenantHelpers = ObjectTableHelperControlsRepository.GetObjectTableHelperControlsByTenant(0).ToDictionary(d => d.Code, a => a);

            AddObjectTableHelperControls.AddObjectTableHelperControl(new ObjectTableHelperControlDetails() { Code = "SHHC", ControlPath = "Simplog.ShipmentLib.Views.Helpers.HelperControl", ObjectTableId = ShipmentTable.Id, Tenant = 0 }, ObjectTableHelperControlsRepository, TenantHelpers);
            AddObjectTableHelperControls.AddObjectTableHelperControl(new ObjectTableHelperControlDetails() { Code = "JHHC", ControlPath = "Simplog.ShipmentLib.Views.Helpers.HelperControl", ObjectTableId = MasterTable.Id, Tenant = 0 }, ObjectTableHelperControlsRepository, TenantHelpers);
            AddObjectTableHelperControls.AddObjectTableHelperControl(new ObjectTableHelperControlDetails() { Code = "QTHC", ControlPath = "Simplog.QuoteLib.Views.Helper.HelperControl", ObjectTableId = QuoteTable.Id, Tenant = 0 }, ObjectTableHelperControlsRepository, TenantHelpers);
            AddObjectTableHelperControls.AddObjectTableHelperControl(new ObjectTableHelperControlDetails() { Code = "INHC", ControlPath = "Simplog.InvoiceLib.Views.Helper.ARInvoiceHelperControl", ObjectTableId = ARInvoiceTable.Id, Tenant = 0 }, ObjectTableHelperControlsRepository, TenantHelpers);
            AddObjectTableHelperControls.AddObjectTableHelperControl(new ObjectTableHelperControlDetails() { Code = "APIH", ControlPath = "Simplog.InvoiceLib.Views.Helper.APInvoiceHelperControl", ObjectTableId = APInvoiceTable.Id, Tenant = 0 }, ObjectTableHelperControlsRepository, TenantHelpers);
            AddObjectTableHelperControls.AddObjectTableHelperControl(new ObjectTableHelperControlDetails() { Code = "ARPH", ControlPath = "Simplog.InvoiceLib.Views.Helper.ARPaymentHelperControl", ObjectTableId = ARPaymentTable.Id, Tenant = 0 }, ObjectTableHelperControlsRepository, TenantHelpers);
            AddObjectTableHelperControls.AddObjectTableHelperControl(new ObjectTableHelperControlDetails() { Code = "APPH", ControlPath = "Simplog.InvoiceLib.Views.Helper.APPaymentHelperControl", ObjectTableId = APPaymentTable.Id, Tenant = 0 }, ObjectTableHelperControlsRepository, TenantHelpers);
            AddObjectTableHelperControls.AddObjectTableHelperControl(new ObjectTableHelperControlDetails() { Code = "CSTH", ControlPath = "Simplog.FreightLib.Views.Helper.CustomerHelperControl", ObjectTableId = CustomerTable.Id, Tenant = 0 }, ObjectTableHelperControlsRepository, TenantHelpers);

            #region Just For Testing 
            if (Testing.General.IsTesting)
            {
                AddObjectTableHelperControls.AddObjectTableHelperControl(new ObjectTableHelperControlDetails() { Code = "TTTT", ControlPath = "Simplog.InvoiceLib.HelperControls.HelperControl", ObjectTableId = ARInvoiceTable.Id, Tenant = 0 }, ObjectTableHelperControlsRepository, TenantHelpers);
                //Update
                AddObjectTableHelperControls.AddObjectTableHelperControl(new ObjectTableHelperControlDetails() { Code = "INHC", ControlPath = "Simplog.InvoiceLib.HelperControls.HelperControl123", ObjectTableId = ARInvoiceTable.Id, Tenant = 0 }, ObjectTableHelperControlsRepository, TenantHelpers);
            }
            #endregion

            ObjectContext.SaveChanges();
        }
    }
}