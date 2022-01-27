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

    public partial class CustomsEnvironmentSettingListQueryService
    {
	    private IQueryable<CustomsEnvironmentSettingList> GetIqueryableList(IQueryable<CustomsEnvironmentSetting> iQueryable)
        {
		IQueryable<CustomsEnvironmentSettingList> query = (from a in iQueryable
                                            select new CustomsEnvironmentSettingList()
											{
                     
					                          Id = a.Id,
					
					                          EnvironmentCode = a.EnvironmentCode,
					
					                          UseRabbitMQ = a.UseRabbitMQ,
					
					                          RabbitHost = a.RabbitHost,
					
					                          RabbitUserName = a.RabbitUserName,
					
		                    	            });
            return query;
		}

		private IQueryable<CustomsEnvironmentSetting> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CustomsEnvironmentSetting> iQueryable)
        {
			throw new NotImplementedException();
		}
				private IQueryable<CustomsEnvironmentSetting> ApplyBusinessUnitFilters(QueryOperations queryOperations,IQueryable<CustomsEnvironmentSetting> iQueryable)
        {
			return iQueryable;
		}
		
			}


}
	