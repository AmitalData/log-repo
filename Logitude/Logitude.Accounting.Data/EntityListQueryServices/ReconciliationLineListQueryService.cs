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

    public partial class ReconciliationLineListQueryService
    {
	    private IQueryable<ReconciliationLineList> GetIqueryableList(IQueryable<ReconciliationLine> iQueryable)
        {
            IQueryable<ReconciliationLineList> query = (from a in iQueryable
                                                        select new ReconciliationLineList()
                                                    {
                                                        ReconciliationId = a.ReconciliationId,
                                                        TransactionId = a.TransactionId,
                                                        CurrencyId = a.CurrencyId,
                                                        CurrencyName = a.Currency != null ? a.Currency.EnglishName : null,
                                                        CurrencyCode = a.Currency != null ? a.Currency.Code : null,
                                                        Line = a.Line,
                                                        IsPartial = a.IsPartial,
                                                        Tenant = a.Tenant,
                                                        ReconciliationAmount = a.ReconciliationAmount,
                                                    });
            return query;
        }


		private IQueryable<ReconciliationLine> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ReconciliationLine> iQueryable,int tenant)
        {
            return iQueryable;
		}

		private IQueryable<ReconciliationLine> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<ReconciliationLine> iQueryable,int tenant)
        {
			return iQueryable;
		}


	}


}
	