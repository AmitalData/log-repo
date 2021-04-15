 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Accounting.Data.Repositories
{
    public partial class ReconcileExternalPageLineRepository : IRepository<ReconcileExternalPageLine>
    {

        public List<ReconcileExternalPageLine> GetMulti(EntityKeyFields entityKeys)
        {
            ReconcileExternalPageKeys keys = entityKeys as ReconcileExternalPageKeys;

            return (from a in context.ReconcileExternalPageLines
                    where a.ReconcileExternalPageId == keys.Id
                    select a).ToList();
        }

        public List<ReconcileExternalPageLine> GetPageLinesByIdList(List<string> idList, int tenant)
        {
            List<ReconcileExternalPageLine> POCOs = (from a in context.ReconcileExternalPageLines
                                                     where idList.Contains(a.Id) && a.Tenant == tenant
                                                     select a).ToList();
            return POCOs;
        }
        public List<ReconcileExternalPageLine> GetPageLines(string pageId, int tenant)
        {
            IQueryable<ReconcileExternalPageLine> pageLineQuery = (from a in context.ReconcileExternalPageLines
                                                                   where a.ReconcileExternalPageId == pageId && a.Tenant == tenant
                                                                   select a);

            return pageLineQuery.ToList();
        }
    }
}