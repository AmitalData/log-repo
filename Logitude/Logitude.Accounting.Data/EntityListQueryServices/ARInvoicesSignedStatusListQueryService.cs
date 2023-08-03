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

    public partial class ARInvoicesSignedStatusListQueryService
    {
	    private IQueryable<ARInvoicesSignedStatusList> GetIqueryableList(IQueryable<ARInvoicesSignedStatus> iQueryable)
        {
		IQueryable<ARInvoicesSignedStatusList> query = (from a in iQueryable
                                            select new ARInvoicesSignedStatusList()
											{
                     
					                          Code = a.Code,
					
					                          LocalName = a.LocalName,
					
					                          EnglishName = a.EnglishName,
					
		                    	            });
            return query;
		}

		private IQueryable<ARInvoicesSignedStatus> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ARInvoicesSignedStatus> iQueryable)
        {
			throw new NotImplementedException();
		}
				private IQueryable<ARInvoicesSignedStatus> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<ARInvoicesSignedStatus> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	