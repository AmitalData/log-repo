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

using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityLists;

namespace Logitude.CRM.Data.EntityListQueryServices
{ 

    public partial class OccasionListQueryService
    {
	    private IQueryable<OccasionList> GetIqueryableList(IQueryable<Occasion> iQueryable)
        {
		IQueryable<OccasionList> query = (from a in iQueryable
                                            select new OccasionList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdateDate = a.UpdateDate,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          SearchFields = a.SearchFields,
					
					                          Name = a.Name,
					
					                          StartDateTime = a.StartDateTime,
					
					                          EndDateTime = a.EndDateTime,
					
					                          Goal = a.Goal,
					
					                          Location = a.Location,
					
					                          OwnerId = a.OwnerId,
					
					                          IndustryId = a.IndustryId,
					
					                          OccasionTypeId = a.OccasionTypeId,
					
					                          OccasionStatusId = a.OccasionStatusId,
					
		                    	            });
            return query;
		}

		private IQueryable<Occasion> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<Occasion> iQueryable, int tenant)
        {
            return iQueryable;
        }
				private IQueryable<Occasion> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<Occasion> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	