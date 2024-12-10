	using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
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

    public partial class LogisticPermitListQueryService
    {
	    private IQueryable<LogisticPermitList> GetIqueryableList(IQueryable<LogisticPermit> iQueryable)
        {
		IQueryable<LogisticPermitList> query = (from a in iQueryable
                                            select new LogisticPermitList()
											{
                     
					                          Id = a.Id,
					
					                          Tenant = a.Tenant,
					
					                          TransmitDate = a.TransmitDate,
					
					                          ActionCode = a.ActionCode,
					
					                          CargoIdentifierType = a.CargoIdentifierType,
					
					                          CargoIdentifierKey1 = a.CargoIdentifierKey1,
					
					                          CargoIdentifierKey2 = a.CargoIdentifierKey2,
					
					                          SearchFields = a.SearchFields,
					
		                    	            });
            return query;
		}

		private IQueryable<LogisticPermit> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<LogisticPermit> iQueryable, int tenant)
        {
			throw new NotImplementedException();
		}
			}


}
	