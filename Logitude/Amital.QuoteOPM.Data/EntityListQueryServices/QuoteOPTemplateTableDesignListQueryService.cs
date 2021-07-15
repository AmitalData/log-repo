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

    public partial class QuoteOPTemplateTableDesignListQueryService
    {
	    private IQueryable<QuoteOPTemplateTableDesignList> GetIqueryableList(IQueryable<QuoteOPTemplateTableDesign> iQueryable)
        {
		IQueryable<QuoteOPTemplateTableDesignList> query = (from a in iQueryable
                                            select new QuoteOPTemplateTableDesignList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          BorderTypeCode = a.BorderTypeCode,
					
					                          BorderColor = a.BorderColor,
					
					                          BorderThickness = a.BorderThickness,
					
					                          HeaderDesignId = a.HeaderDesignId,
					
					                          LinesDesignId = a.LinesDesignId,
					
					                          GroupByDesignId = a.GroupByDesignId,
					
		                    	            });
            return query;
		}

		private IQueryable<QuoteOPTemplateTableDesign> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<QuoteOPTemplateTableDesign> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<QuoteOPTemplateTableDesign> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<QuoteOPTemplateTableDesign> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	