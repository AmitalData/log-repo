using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Server.Infrastructure.AnalyticTableServices;
using System.Data.Entity;

namespace Logitude.BL.AnalyticTableServices
{
    public class APInvoiceAnalyticTableService : AnalyticTableService<APInvoice, APInvoiceAnalytic>
    {
        public APInvoiceAnalyticTableService(DbContext context) : base(context)
        {

        }

        protected override void CustomMap(APInvoice entity, APInvoiceAnalytic analyticTable)
        {
           
        }
    }
}
