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

    public partial class ExternalReconciliationListQueryService
    {
        private IQueryable<ExternalReconciliationList> GetIqueryableList(IQueryable<ExternalReconciliation> iQueryable)
        {
            IQueryable<ExternalReconciliationList> query = (from a in iQueryable
                                                            select new ExternalReconciliationList()
                                                            {
                                                                Id = a.Id,
                                                                GLAccountId = a.GLAccountId,
                                                                CreatedByUserId = a.CreatedByUserId,
                                                                ReconciliationNumber = a.ReconciliationNumber,
                                                                CreateDate = a.CreateDate,
                                                                Tenant = a.Tenant,
                                                                SearchFields = a.SearchFields,
                                                                CreatedByUserName = a.CreatedByUser != null ? a.CreatedByUser.Contact.LocalName : null,
                                                                AccountNumber = a.Account != null ? a.Account.DisplayNumber : null,
                                                                AccountName = a.Account != null ? a.Account.EnglishName : null,
                                                                AccountLocalName = a.Account != null ? a.Account.LocalName : null,
                                                                IsCancelled = a.IsCancelled,
                                                                CrossYearReconcile = a.CrossYearReconcile,

                                                            });
            return query;
        }

        private IQueryable<ExternalReconciliation> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<ExternalReconciliation> iQueryable, int tenant)
        {
            return iQueryable;
        }
        private IQueryable<ExternalReconciliation> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<ExternalReconciliation> iQueryable, int tenant)
        {
            return iQueryable;
        }

    }


}
	