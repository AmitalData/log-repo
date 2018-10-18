using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel
{
    public interface IAgingReportInvoiceDataViewContext
    {
        IDbSet<AgingReportInvoiceDataView> AgingReportInvoiceDataViews { get; }
        void SetAsModified(object entity);
        void DetectChanges();
        int SaveChanges();
    }
}