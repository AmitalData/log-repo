	using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;

namespace Logitude.Accounting.Data.EntityListQueryServices
{

    public partial class ReconcileExternalPageLineListQueryService
    {
        public IQueryable<ReconcileExternalPageLineList> GetIqueryableList(IQueryable<ReconcileExternalPageLine> iQueryable)
        {
            IQueryable<ReconcileExternalPageLineList> query = (from a in iQueryable
                                                               select new ReconcileExternalPageLineList()
                                                               {

                                                                   LineNumber = a.LineNumber,

                                                                   Tenant = a.Tenant,

                                                                   ReferenceDate = a.ReferenceDate,

                                                                   Reference = a.Reference,

                                                                   IsReconciled = a.IsReconciled,

                                                                   SearchFields = a.SearchFields,

                                                                   Amount = a.Amount,

                                                                   Notes = a.Notes,

                                                                   ReconcileExternalPageId = a.ReconcileExternalPageId,

                                                                   Id = a.Id,

                                                               });
            return query;
        }

        public List<ReconcileExternalPageLineList> GetPageLinesByIds(List<string> ids)
        {
            IQueryable<ReconcileExternalPageLine> pageLineQuery = (from a in context.ReconcileExternalPageLines
                                                                    where ids.Contains(a.Id)
                                                                    select a);

            IQueryable<ReconcileExternalPageLineList> pageLineListQuery = GetIqueryableList(pageLineQuery);
            var myList = pageLineListQuery.ToList();
            return myList;
        }

        public IQueryable<ReconcileExternalPageLine> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<ReconcileExternalPageLine> iQueryable, int tenant)
        {
            return iQueryable;
        }
        public IQueryable<ReconcileExternalPageLine> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<ReconcileExternalPageLine> iQueryable, int tenant)
        {
            return iQueryable;
        }

    }


}
	