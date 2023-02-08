using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure.AnalyticTableServices;
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
