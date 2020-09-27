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

    public partial class CustomsGeneralListQueryService
    {
	    private IQueryable<CustomsGeneralList> GetIqueryableList(IQueryable<CustomsGeneral> iQueryable)
        {
		IQueryable<CustomsGeneralList> query = (from a in iQueryable
                                            select new CustomsGeneralList()
											{
                     
		                    	            });
            return query;
		}

		private IQueryable<CustomsGeneral> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CustomsGeneral> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<CustomsGeneral> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<CustomsGeneral> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	