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

    public partial class MagayaStatusListQueryService
    {
	    private IQueryable<MagayaStatusList> GetIqueryableList(IQueryable<MagayaStatus> iQueryable)
        {
		IQueryable<MagayaStatusList> query = (from a in iQueryable
                                            select new MagayaStatusList()
											{
                     
					                          StatusCode = a.StatusCode,
					
					                          StatusName = a.StatusName,
					
		                    	            });
            return query;
		}

		private IQueryable<MagayaStatus> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<MagayaStatus> iQueryable)
        {
			throw new NotImplementedException();
		}
				private IQueryable<MagayaStatus> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<MagayaStatus> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	