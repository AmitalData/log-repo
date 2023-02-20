using Logitude.Server.Tools.AnalyticTableServices;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using System.Data.Entity;


namespace Logitude.BL.AnalyticTableServices
{
    public class ARInvoiceAnalyticTableService : AnalyticTableService<ARInvoice, ARInvoiceAnalytic>
    {
        public ARInvoiceAnalyticTableService(DbContext context) : base(context)
        {

        }

        protected override void CustomMap(ARInvoice entity, ARInvoiceAnalytic analyticTable)
        {
         
        }
    }
}
