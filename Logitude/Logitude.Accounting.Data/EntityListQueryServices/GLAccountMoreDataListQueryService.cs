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

    public partial class GLAccountMoreDataListQueryService
    {
        private IQueryable<GLAccountMoreDataList> GetIqueryableList(IQueryable<GLAccountMoreData> iQueryable)
        {
            IQueryable<GLAccountMoreDataList> query = (from a in iQueryable
                                                       select new GLAccountMoreDataList()
                                                       {
                                                           Tenant = a.Tenant,
                                                           AccountId = a.AccountId,
                                                           BalanceInLocalCurrency = a.BalanceInLocalCurrency,
                                                           LocalBalanceInDue = a.LocalBalanceInDue,
                                                           NextDueDate = a.NextDueDate,
                                                           TotalOpenChequesInLocalCur = a.TotalOpenChequesInLocalCur,
                                                           TotFutureOpenChequesInLocalCur = a.TotFutureOpenChequesInLocalCur,
                                                       });
            return query;
        }

        private IQueryable<GLAccountMoreData> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<GLAccountMoreData> iQueryable, int tenant)
        {
            return iQueryable;
        }
        private IQueryable<GLAccountMoreData> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<GLAccountMoreData> iQueryable, int tenant)
        {
            return iQueryable;
        }

    }


}
	