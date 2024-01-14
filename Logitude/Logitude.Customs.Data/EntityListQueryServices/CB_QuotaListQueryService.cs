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

    public partial class CB_QuotaListQueryService
    {
	    private IQueryable<CB_QuotaList> GetIqueryableList(IQueryable<CB_Quota> iQueryable)
        {
		IQueryable<CB_QuotaList> query = (from a in iQueryable
                                            select new CB_QuotaList()
											{
                     
		                    	            });
            return query;
		}

		private IQueryable<CB_Quota> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CB_Quota> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	