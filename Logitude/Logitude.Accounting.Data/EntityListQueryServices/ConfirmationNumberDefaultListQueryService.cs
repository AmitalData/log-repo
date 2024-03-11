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

    public partial class ConfirmationNumberDefaultListQueryService
    {
	    private IQueryable<ConfirmationNumberDefaultList> GetIqueryableList(IQueryable<ConfirmationNumberDefault> iQueryable)
        {
		IQueryable<ConfirmationNumberDefaultList> query = (from a in iQueryable
                                            select new ConfirmationNumberDefaultList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          FromDate = a.FromDate,
					
					                          SearchFields = a.SearchFields,
					
					                          AmountForConfirmationNumber = a.AmountForConfirmationNumber,
					
		                    	            });
            return query;
		}

		private IQueryable<ConfirmationNumberDefault> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<ConfirmationNumberDefault> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<ConfirmationNumberDefault> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<ConfirmationNumberDefault> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	