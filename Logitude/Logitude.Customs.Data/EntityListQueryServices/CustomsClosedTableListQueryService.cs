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

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class CustomsClosedTableListQueryService
    {
	    private IQueryable<CustomsClosedTableList> GetIqueryableList(IQueryable<CustomsClosedTable> iQueryable)
        {
            IQueryable<CustomsClosedTableList> query = (from a in iQueryable.Include("ClosedTableStatus").Include("ObjectTable")
                                                        select new CustomsClosedTableList()
                                                       {
                                                           Id = a.Id,
                                                           CustomsLocalName = a.CustomsLocalName,
                                                           CustomsName = a.CustomsName,
                                                           DbName = a.DbName,
                                                           LastUpdateDate = a.LastUpdateDate,
                                                           StatusCode = a.StatusCode,
                                                           StatusName = a.ClosedTableStatus.LocalName,
                                                           SearchFields=a.SearchFields,
                                                           ObjectTableId=a.ObjectTableId,
                                                           ObjectTableName=a.ObjectTable.Name,
                                                           Existed=a.Existed,
                                                           RetreiveDateTime = a.RetreiveDateTime,
                                                       });
            return query;
		}

		private IQueryable<CustomsClosedTable> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CustomsClosedTable> iQueryable)
        {
            return iQueryable;
		}
	}


}
	