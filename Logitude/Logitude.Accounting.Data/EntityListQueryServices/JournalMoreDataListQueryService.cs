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

    public partial class JournalMoreDataListQueryService
    {
        private IQueryable<JournalMoreDataList> GetIqueryableList(IQueryable<JournalMoreData> iQueryable)
        {
            IQueryable<JournalMoreDataList> query = (from a in iQueryable
                                                     select new JournalMoreDataList()
                                                     {
                                                         IsLedgerCreated = a.IsLedgerCreated,
                                                         GeneralData = a.GeneralData,
                                                         JournalId = a.JournalId,
                                                         Line =a.Line,
                                                         Tenant =a.Tenant,
                                                     });
            return query;
        }

        private IQueryable<JournalMoreData> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<JournalMoreData> iQueryable, int tenant)
        {
            return iQueryable;
        }
        private IQueryable<JournalMoreData> ApplyBusinessUnitFilters(QueryOperations queryOperations, IQueryable<JournalMoreData> iQueryable, int tenant)
        {
            return iQueryable;
        }

    }


}
