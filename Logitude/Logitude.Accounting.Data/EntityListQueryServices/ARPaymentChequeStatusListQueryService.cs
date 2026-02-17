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

    public partial class ARPaymentChequeStatusListQueryService
    {
	    private IQueryable<ARPaymentChequeStatusList> GetIqueryableList(IQueryable<ARPaymentChequeStatus> iQueryable)
        {
		IQueryable<ARPaymentChequeStatusList> query = (from a in iQueryable
                                            select new ARPaymentChequeStatusList()
											{
                     
					                          Code = a.Code,
					
					                          SearchFields = a.SearchFields,
					
					                          LocalName = a.LocalName,
					
					                          EnglishName = a.EnglishName,
					
					                          Inactive = a.Inactive,
					
		                    	            });
            return query;
		}

		private IQueryable<ARPaymentChequeStatus> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ARPaymentChequeStatus> iQueryable)
        {
            return iQueryable;
		}
				private IQueryable<ARPaymentChequeStatus> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<ARPaymentChequeStatus> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	