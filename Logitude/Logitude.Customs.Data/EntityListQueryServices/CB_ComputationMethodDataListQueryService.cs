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

    public partial class CB_ComputationMethodDataListQueryService
    {
	    private IQueryable<CB_ComputationMethodDataList> GetIqueryableList(IQueryable<CB_ComputationMethodData> iQueryable)
        {
		IQueryable<CB_ComputationMethodDataList> query = (from a in iQueryable
                                            select new CB_ComputationMethodDataList()
											{
                     
		                    	            });
            return query;
		}

		private IQueryable<CB_ComputationMethodData> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CB_ComputationMethodData> iQueryable)
        {
			throw new NotImplementedException();
		}
			}


}
	