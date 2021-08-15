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

using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Data.EntityLists;

namespace Amital.QuoteOPM.Data.EntityListQueryServices
{ 

    public partial class OPSpecialServicesTypeListQueryService
    {
	    private IQueryable<OPSpecialServicesTypeList> GetIqueryableList(IQueryable<OPSpecialServicesType> iQueryable)
        {
		IQueryable<OPSpecialServicesTypeList> query = (from a in iQueryable
                                            select new OPSpecialServicesTypeList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          Code = a.Code,
					
					                          EnglishName = a.EnglishName,
					
					                          LocalName = a.LocalName,
					
					                          SearchFields = a.SearchFields,
					
					                          InActive = a.InActive,
					
		                    	            });
            return query;
		}

		private IQueryable<OPSpecialServicesType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<OPSpecialServicesType> iQueryable, int tenant)
        {
			return iQueryable;
		}
				private IQueryable<OPSpecialServicesType> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<OPSpecialServicesType> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	