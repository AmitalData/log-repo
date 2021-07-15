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

    public partial class QuoteOPTemplateListQueryService
    {
	    private IQueryable<QuoteOPTemplateList> GetIqueryableList(IQueryable<QuoteOPTemplate> iQueryable)
        {
		IQueryable<QuoteOPTemplateList> query = (from a in iQueryable
                                            select new QuoteOPTemplateList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          HeaderDocId = a.HeaderDocId,
					
					                          FooterDocId = a.FooterDocId,
					
					                          QuoteOPTemplateSettingId = a.QuoteOPTemplateSettingId,
					
					                          Name = a.Name,
					
					                          IsTemplate = a.IsTemplate,
					
					                          OriginalQuoteOPTemplateId = a.OriginalQuoteOPTemplateId,
					
					                          CreateDate = a.CreateDate,
					
					                          UpdateDate = a.UpdateDate,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          SearchFields = a.SearchFields,
					
					                          TemplateTypeCode = a.TemplateTypeCode,
					
					                          IsDefault = a.IsDefault,
					
					                          InActive = a.InActive,
					
					                          IsCopiedAtSignup = a.IsCopiedAtSignup,
					
					                          IsEnabledForCustomers = a.IsEnabledForCustomers,
					
		                    	            });
            return query;
		}

		private IQueryable<QuoteOPTemplate> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<QuoteOPTemplate> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<QuoteOPTemplate> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<QuoteOPTemplate> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	