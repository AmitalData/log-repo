using Logitude.Server.Tools.AnalyticTableServices;
using Simplog.Data.InvoiceModel.EntityPOCOs;
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
