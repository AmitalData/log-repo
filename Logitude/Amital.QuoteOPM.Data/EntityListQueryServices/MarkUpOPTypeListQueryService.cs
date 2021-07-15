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

    public partial class MarkUpOPTypeListQueryService
    {
	    private IQueryable<MarkUpOPTypeList> GetIqueryableList(IQueryable<MarkUpOPType> iQueryable)
        {
		IQueryable<MarkUpOPTypeList> query = (from a in iQueryable
                                            select new MarkUpOPTypeList()
											{
                     
					                          SearchFields = a.SearchFields,
					
					                          Code = a.Code,
					
					                          Name = a.Name,
					
		                    	            });
            return query;
		}

		private IQueryable<MarkUpOPType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<MarkUpOPType> iQueryable)
        {
			throw new NotImplementedException();
		}
				private IQueryable<MarkUpOPType> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<MarkUpOPType> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	