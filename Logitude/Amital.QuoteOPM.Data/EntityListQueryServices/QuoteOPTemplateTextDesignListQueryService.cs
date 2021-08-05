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

    public partial class QuoteOPTemplateTextDesignListQueryService
    {
	    private IQueryable<QuoteOPTemplateTextDesignList> GetIqueryableList(IQueryable<QuoteOPTemplateTextDesign> iQueryable)
        {
		IQueryable<QuoteOPTemplateTextDesignList> query = (from a in iQueryable
                                            select new QuoteOPTemplateTextDesignList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          FontSize = a.FontSize,
					
					                          TextColor = a.TextColor,
					
					                          FontFamily = a.FontFamily,
					
					                          BackgroundColor = a.BackgroundColor,
					
					                          FontWeight = a.FontWeight,
					
					                          Italic = a.Italic,
					
					                          UnDerLine = a.UnDerLine,
					
					                          Alignment = a.Alignment,
					
		                    	            });
            return query;
		}

		private IQueryable<QuoteOPTemplateTextDesign> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<QuoteOPTemplateTextDesign> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<QuoteOPTemplateTextDesign> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<QuoteOPTemplateTextDesign> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	