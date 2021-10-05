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

    public partial class QuoteOPDocumentVersionListQueryService
    {
	    private IQueryable<QuoteOPDocumentVersionList> GetIqueryableList(IQueryable<QuoteOPDocumentVersion> iQueryable)
        {
		IQueryable<QuoteOPDocumentVersionList> query = (from a in iQueryable
                                            select new QuoteOPDocumentVersionList()
											{
                     
					                          QuoteOPId = a.QuoteOPId,
					
					                          Tenant = a.Tenant,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
		                    	            });
            return query;
		}

		private IQueryable<QuoteOPDocumentVersion> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<QuoteOPDocumentVersion> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
				private IQueryable<QuoteOPDocumentVersion> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<QuoteOPDocumentVersion> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	