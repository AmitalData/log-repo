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

using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityLists;

namespace Logitude.Accounting.Data.EntityListQueryServices
{ 

    public partial class InterestBasesTypeListQueryService
    {
	    private IQueryable<InterestBasesTypeList> GetIqueryableList(IQueryable<InterestBasesType> iQueryable)
        {
		IQueryable<InterestBasesTypeList> query = (from a in iQueryable
                                            select new InterestBasesTypeList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          CreateDate = a.CreateDate,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdateDate = a.UpdateDate,
					
					                          UpdatedByUserId = a.UpdatedByUserId,
					
					                          SearchFields = a.SearchFields,
					
					                          Code = a.Code,
					
					                          LocalName = a.LocalName,
					
					                          EnglishName = a.EnglishName,
					
					                          Description = a.Description,
					
					                          InActive = a.InActive,
					
		                    	            });
            return query;
		}

		private IQueryable<InterestBasesType> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<InterestBasesType> iQueryable, int tenant)
        {
            return iQueryable;
        }
				private IQueryable<InterestBasesType> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<InterestBasesType> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	