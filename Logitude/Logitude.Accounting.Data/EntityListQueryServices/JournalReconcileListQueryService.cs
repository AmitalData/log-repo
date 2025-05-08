using Logitude.Accounting.Data.EntityLists;
using Logitude.Accounting.Data.EntityPOCOs;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Accounting.Data.EntityListQueryServices
{
    public partial class JournalReconcileListQueryService
    {

        private IQueryable<JournalReconcileList> GetIqueryableList(IQueryable<JournalReconcile> iQueryable)
        {
            IQueryable<JournalReconcileList> query = (from a in iQueryable
                                                              select new JournalReconcileList()
                                                              {

                                                              });
            return query;
        }

        private IQueryable<JournalReconcile> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<JournalReconcile> iQueryable, int tenant)
        {
            return iQueryable;
        }
        private IQueryable<JournalReconcile> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<JournalReconcile> iQueryable, int tenant)
        {
            return iQueryable;

        }
    }
}
