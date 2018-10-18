using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using Simplog.Data.InvoiceModel.EntityPOCOs;

namespace Simplog.Data.InvoiceModel
{
    public interface IApInvoiceAgingReportContext
    {
        IDbSet<APAgingReportDataView> AgingReportInvoiceDataViews { get; }
        void SetAsModified(object entity);
        void DetectChanges();
        int SaveChanges();
    }
}