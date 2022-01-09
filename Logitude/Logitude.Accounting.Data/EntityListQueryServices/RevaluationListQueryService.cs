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

    public partial class RevaluationListQueryService
    {
        private IQueryable<RevaluationList> GetIqueryableList(IQueryable<Revaluation> iQueryable)
        {
            IQueryable<RevaluationList> query = (from a in iQueryable
                                                 select new RevaluationList()
                                                 {
                                                     Id = a.Id,
                                                     Tenant = a.Tenant,
                                                     CreateDate = a.CreateDate,
                                                     CreatedByUserId = a.CreatedByUserId,
                                                     SearchFields = a.SearchFields,
                                                     RevaluationNumber = a.RevaluationNumber,
                                                     RevaluationDate = a.RevaluationDate,
                                                     ChartOfAccountsId = a.ChartOfAccountsId,
                                                     GLAccountId = a.GLAccountId,
                                                     ChartOfAccountsName = a.ChartOfAccount != null ? a.ChartOfAccount.LocalName : null,
                                                     GLAccountName = a.GLAccount != null ? a.GLAccount.LocalName : null,
                                                     GLAccountNumber = a.GLAccount != null ? a.GLAccount.DisplayNumber : null,
                                                     CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.LocalName : null,
                                                     RevaluationEnabled = a.RevaluationEnabled,
                                                     Message = a.Message,
                                                     Status = a.Status,
                                                     StatusName = a.RevaluationStatus != null ? a.RevaluationStatus.LocalName : a.RevaluationStatus.Name,
                                                     RevaluationsGLAccountId = a.RevaluationsGLAccountId,
                                                     RevaluationsGLAccountName = a.RevaluationGLAccount != null ? a.RevaluationGLAccount.LocalName : null,
                                                 });
            return query;
        }

        private IQueryable<Revaluation> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<Revaluation> iQueryable, int tenant)
        {
            return iQueryable;
        }

        private IQueryable<Revaluation> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<Revaluation> iQueryable, int tenant)
        {
            return iQueryable;
        }

        public List<RevaluationList> GetOpenRevaluationList(int tenant)
        {
            IQueryable<Revaluation> revaluationQuery = (from a in context.Revaluations
                                                    where a.Tenant == tenant && ((a.Status == "" || a.Status != "2") && (a.RevaluationDate != null || a.RevaluationDate != DateTime.MinValue))
                                                    select a);

            IQueryable<RevaluationList> revaluationListQuery = this.GetIqueryableList(revaluationQuery);
            List<RevaluationList> revaluationList = revaluationListQuery.ToList();

            return revaluationList;
        }

    }


}
	