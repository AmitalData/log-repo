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

using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityLists;

namespace Logitude.Customs.Data.EntityListQueryServices
{ 

    public partial class CB_PreferenceListQueryService
    {
	    private IQueryable<CB_PreferenceList> GetIqueryableList(IQueryable<CB_Preference> iQueryable)
        {
		IQueryable<CB_PreferenceList> query = (from a in iQueryable
                                            select new CB_PreferenceList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          BackgroundColor = a.BackgroundColor,
					
					                          TextColor = a.TextColor,
					
					                          UserId = a.UserId,
					
		                    	            });
            return query;
		}

		private IQueryable<CB_Preference> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CB_Preference> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	