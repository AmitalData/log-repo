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

    public partial class QuoteOPTemplateSectionListQueryService
    {
	    private IQueryable<QuoteOPTemplateSectionList> GetIqueryableList(IQueryable<QuoteOPTemplateSection> iQueryable)
        {
		IQueryable<QuoteOPTemplateSectionList> query = (from a in iQueryable
                                            select new QuoteOPTemplateSectionList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          Name = a.Name,
					
					                          Description = a.Description,
					
					                          IsCancel = a.IsCancel,
					
					                          QuoteTemplateId = a.QuoteTemplateId,
					
					                          SectionDocId = a.SectionDocId,
					
					                          Order = a.Order,
					
					                          QuoteOPTemplateSectionTypeCode = a.QuoteOPTemplateSectionTypeCode,
					
		                    	            });
            return query;
		}

		private IQueryable<QuoteOPTemplateSection> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<QuoteOPTemplateSection> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<QuoteOPTemplateSection> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<QuoteOPTemplateSection> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	