using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System.Linq;
 

namespace Logitude.Accounting.Data.EntityListQueryServices
{
    public partial class JournalExternalReconcileListQueryService
    {

        private IQueryable<JournalExternalReconcileList> GetIqueryableList(IQueryable<JournalExternalReconcile> iQueryable)
        {
            IQueryable<JournalExternalReconcileList> query = (from a in iQueryable
                                                              select new JournalExternalReconcileList()
                                                              {
                                                                  
                                                              });
            return query;
        }

        private IQueryable<JournalExternalReconcile> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<JournalExternalReconcile> iQueryable, int tenant)
        {
            return iQueryable;
        }
        private IQueryable<JournalExternalReconcile> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<JournalExternalReconcile> iQueryable, int tenant)
        {
            return iQueryable;

        }
    }
}