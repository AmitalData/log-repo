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

    public partial class ConfirmationNumberStatusListQueryService
    {
	    private IQueryable<ConfirmationNumberStatusList> GetIqueryableList(IQueryable<ConfirmationNumberStatus> iQueryable)
        {
		IQueryable<ConfirmationNumberStatusList> query = (from a in iQueryable
                                            select new ConfirmationNumberStatusList()
											{
                     
					                          Code = a.Code,
					
					                          Name = a.Name,
					
					                          SearchFields = a.SearchFields,
					
					                          LocalName = a.LocalName,
					
					                          InActive = a.InActive,
					
		                    	            });
            return query;
		}

		private IQueryable<ConfirmationNumberStatus> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ConfirmationNumberStatus> iQueryable)
        {
			throw new NotImplementedException();
		}
				private IQueryable<ConfirmationNumberStatus> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<ConfirmationNumberStatus> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	