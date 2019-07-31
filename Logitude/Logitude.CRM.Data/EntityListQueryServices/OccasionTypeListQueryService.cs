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

    public partial class OccasionTypeListQueryService
    {
	    private IQueryable<OccasionTypeList> GetIqueryableList(IQueryable<OccasionType> iQueryable)
        {
		IQueryable<OccasionTypeList> query = (from a in iQueryable
                                            select new OccasionTypeList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdateDate = a.UpdateDate,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          SearchFields = a.SearchFields,
					
					                          Code = a.Code,
					
					                          Name = a.Name,
					
					                          AddedManually = a.AddedManually,
					
		                    	            });
            return query;
		}

		private IQueryable<OccasionType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<OccasionType> iQueryable, int tenant)
        {
            return iQueryable;
        }
				private IQueryable<OccasionType> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<OccasionType> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	