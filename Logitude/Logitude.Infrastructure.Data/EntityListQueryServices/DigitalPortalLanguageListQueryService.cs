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

using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityLists;

namespace Logitude.Infrastructure.Data.EntityListQueryServices
{ 

    public partial class DigitalPortalLanguageListQueryService
    {
	    private IQueryable<DigitalPortalLanguageList> GetIqueryableList(IQueryable<DigitalPortalLanguage> iQueryable)
        {
		IQueryable<DigitalPortalLanguageList> query = (from a in iQueryable
                                            select new DigitalPortalLanguageList()
											{
                     
					                          Code = a.Code,
					
					                          Name = a.Name,
					
					                          SearchFields = a.SearchFields,
					
					                          DisplayText = a.DisplayText,
					
		                    	            });
            return query;
		}

		private IQueryable<DigitalPortalLanguage> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<DigitalPortalLanguage> iQueryable)
        {
			throw new NotImplementedException();
		}
				private IQueryable<DigitalPortalLanguage> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<DigitalPortalLanguage> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	