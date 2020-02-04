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

    public partial class CargoSealListQueryService
    {
	    private IQueryable<CargoSealList> GetIqueryableList(IQueryable<CargoSeal> iQueryable)
        {
		IQueryable<CargoSealList> query = (from a in iQueryable
                                            select new CargoSealList()
											{
                     
					                          CargoSealIdentifierId = a.CargoSealIdentifierId,
					
					                          Tenant = a.Tenant,
					
					                          SealNumber = a.SealNumber,
					
					                          Remarks = a.Remarks,
					
					                          SealCompletenessStateCode = a.SealCompletenessStateCode,
					
					                          SealTypeCode = a.SealTypeCode,
					
					                          UpdateReasonCode = a.UpdateReasonCode,
					
					                          UpdateTypeCode = a.UpdateTypeCode,
					
		                    	            });
            return query;
		}

		private IQueryable<CargoSeal> ApplyCustomFilters(QueryOperations queryOperations,IQueryable<CargoSeal> iQueryable, int tenant)
        {
			return iQueryable;
		}
	}


}
	