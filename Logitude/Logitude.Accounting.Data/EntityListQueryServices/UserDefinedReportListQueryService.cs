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

    public partial class UserDefinedReportListQueryService
    {
	    private IQueryable<UserDefinedReportList> GetIqueryableList(IQueryable<UserDefinedReport> iQueryable)
        {
		IQueryable<UserDefinedReportList> query = (from a in iQueryable
                                            select new UserDefinedReportList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,

											  EnglishName = a.EnglishName,

											  LocalName = a.LocalName,

											  IsCancelled = a.IsCancelled,
					
					                          CreateDateTime = a.CreateDateTime,
					
					                          CreatedByUserId = a.CreatedByUserId,
					
					                          UpdatedDateTime = a.UpdatedDateTime,
					
					                          UpdatedByUserId = a.UpdatedByUserId,

											  CreatedByEnglishName = a.CreatedByUser == null ? null : a.CreatedByUser.Contact == null ? null : a.CreatedByUser.Contact.EnglishName,

											  CreatedByLocalName = a.CreatedByUser == null ? null : a.CreatedByUser.Contact == null ? null : a.CreatedByUser.Contact.LocalName,

											  UpdatedByEnglishName = a.UpdatedByUser == null ? null : a.UpdatedByUser.Contact == null ? null : a.UpdatedByUser.Contact.EnglishName,

											  UpdatedByLocalName = a.UpdatedByUser == null ? null : a.UpdatedByUser.Contact == null ? null : a.UpdatedByUser.Contact.LocalName,

											  CreatedByName = null,

											  UpdatedByName = null,
											});
            return query;
		}

		private IQueryable<UserDefinedReport> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<UserDefinedReport> iQueryable, int tenant)
        {
			return iQueryable;
		}
				private IQueryable<UserDefinedReport> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<UserDefinedReport> iQueryable, int tenant)
        {
			return iQueryable;
		}
		
			}


}
	