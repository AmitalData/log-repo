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

    public partial class QuoteOPTemplateTextCodeListQueryService
    {
	    private IQueryable<QuoteOPTemplateTextCodeList> GetIqueryableList(IQueryable<QuoteOPTemplateTextCode> iQueryable)
        {
		IQueryable<QuoteOPTemplateTextCodeList> query = (from a in iQueryable
                                            select new QuoteOPTemplateTextCodeList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          TextCode = a.TextCode,
					
					                          EnglishName = a.EnglishName,
					
					                          LocalName = a.LocalName,
					
					                          QuoteTemplateId = a.QuoteTemplateId,
					
					                          Area = a.Area,
					
					                          OriginalEnglishName = a.OriginalEnglishName,
					
					                          OriginalLocalName = a.OriginalLocalName,
					
		                    	            });
            return query;
		}

		private IQueryable<QuoteOPTemplateTextCode> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<QuoteOPTemplateTextCode> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<QuoteOPTemplateTextCode> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<QuoteOPTemplateTextCode> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	