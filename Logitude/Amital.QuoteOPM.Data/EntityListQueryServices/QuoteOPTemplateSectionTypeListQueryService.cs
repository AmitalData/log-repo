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

    public partial class QuoteOPTemplateSectionTypeListQueryService
    {
	    private IQueryable<QuoteOPTemplateSectionTypeList> GetIqueryableList(IQueryable<QuoteOPTemplateSectionType> iQueryable)
        {
		IQueryable<QuoteOPTemplateSectionTypeList> query = (from a in iQueryable
                                            select new QuoteOPTemplateSectionTypeList()
											{
                     
					                          Code = a.Code,
					
					                          Name = a.Name,
					
		                    	            });
            return query;
		}

		private IQueryable<QuoteOPTemplateSectionType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<QuoteOPTemplateSectionType> iQueryable)
        {
			throw new NotImplementedException();
		}
				private IQueryable<QuoteOPTemplateSectionType> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<QuoteOPTemplateSectionType> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	