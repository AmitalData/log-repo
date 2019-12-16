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

    public partial class PriceStepsListQueryService
    {
	    private IQueryable<PriceStepsList> GetIqueryableList(IQueryable<PriceSteps> iQueryable)
        {
		IQueryable<PriceStepsList> query = (from a in iQueryable
                                            select new PriceStepsList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdateDate = a.UpdateDate,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          SearchFields = a.SearchFields,
					
					                          Name = a.Name,
					
					                          Inactive = a.Inactive,
					
					                          Steps = a.Steps,

                                              CreatedByUserName = a.CreatedByUser.Contact.EnglishName != null? a.CreatedByUser.Contact.EnglishName: "",

                                              UpdatedByUserName = a.UpdatedByUser.Contact.EnglishName != null ? a.UpdatedByUser.Contact.EnglishName :""
                                              
                                            });
            return query;
		}

		private IQueryable<PriceSteps> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<PriceSteps> iQueryable, int tenant)
        {
            return iQueryable;
        }
		private IQueryable<PriceSteps> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<PriceSteps> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
    }


}
	