using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.Messaging.U2L.ImportDeclaration;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Reflection;
using System.Web.UI.WebControls;
using static Logitude.BL.InfrastructureModel.EntityQueries.ObjectFieldQuery;

namespace WebFreight.Web.CustomWebServices.Testers
{
    public partial class IzikTester : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //try
            //{
            Response.Clear();
            SupplierInvoiceFreightAmountPM pm = new SupplierInvoiceFreightAmountPM()
            {
                DeclarationId = "1-100934",
                InvoiceCounterKey = 1,
                CurrencyTypeCode = "USD"
            };

            int tenant = 1;
            //bool isValid = CheckForgienKeyClosedTable(pm, tenant);
            //bool isValid2 = CheckForgienKey<SupplierInvoiceFreightAmount>(pm, tenant);

            DeclarationPM declarationPm = new DeclarationQueryService(tenant).GetSingle("1-100934", true, false);

            //var p = declarationPm;
            //var z = p.GetType().GetProperty("Consignments")?.GetValue(p, null);

            //bool isList =
            //    z.GetType().FullName.StartsWith("System.Collections.Generic.List") &&
            //    z.GetType().GetGenericTypeDefinition().IsAssignableFrom(typeof(List<>));

            //if (isList)
            //{
            //    var val = ((IEnumerable<dynamic>)z).FirstOrDefault();

            //    if (val != null && val.GetType().Name.EndsWith("PM"))
            //        CheckForgienKeyClosedTable(val, 0);
            //}
            declarationPm.SupplierInvoices[0].IncotermCode = "C&F";


            ClearWrongValues(declarationPm, tenant);

            bool isValid2 = ForiegnKeyCheck.CheckClosedTable(declarationPm, tenant);
            //bool isValid2 = CheckForgienKeyClosedTable(declarationPm, 0);
            string a = declarationPm.SupplierInvoices[0].IncotermCode;
            Response.Write(declarationPm);
            //}
            //catch (Exception eee)
            //{
            //    Response.Clear();
            //    Response.Write(eee.ToString());
            //}
        }
        private void ClearWrongValues(DeclarationPM declarationPm, int tenant)
        {            
            ForiegnKeyCheck.CheckClosedTable(declarationPm, tenant);
            ForiegnKeyCheck.Check<Declaration>(declarationPm, tenant);

            declarationPm.Consignments.ForEach(x =>
            {
                ForiegnKeyCheck.CheckClosedTable(x, tenant);
                ForiegnKeyCheck.Check<Consignment>(x, tenant);
            });

            declarationPm.SupplierInvoices.ForEach(x =>
            {
                ForiegnKeyCheck.CheckClosedTable(x, tenant);
                ForiegnKeyCheck.Check<SupplierInvoice>(x, tenant);
            });

            declarationPm.DeclarationTaxes.ForEach(x =>
            {
                ForiegnKeyCheck.CheckClosedTable(x, tenant);
                ForiegnKeyCheck.Check<DeclarationTax>(x, tenant);
            });

            declarationPm.DeclarationConstraints.ForEach(x =>
            {
                ForiegnKeyCheck.CheckClosedTable(x, tenant);
                ForiegnKeyCheck.Check<DeclarationConstraint>(x, tenant);
            });

            declarationPm.DeclarationConsAcceptances.ForEach(x =>
            {
                ForiegnKeyCheck.CheckClosedTable(x, tenant);
                ForiegnKeyCheck.Check<DeclarationConsAcceptancePM>(x, tenant);
            });

            declarationPm.DecDangersContacts.ForEach(x =>
            {
                ForiegnKeyCheck.CheckClosedTable(x, tenant);
                ForiegnKeyCheck.Check<DecDangersContact>(x, tenant);
            });

            declarationPm.DeclarationExportRecipients.ForEach(x =>
            {
                ForiegnKeyCheck.CheckClosedTable(x, tenant);
                ForiegnKeyCheck.Check<DeclarationExportRecipient>(x, tenant);
            });
        }

    }
}