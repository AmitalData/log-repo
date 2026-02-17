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

    public partial class ProceduralFaultsConnEntityListQueryService
    {
	    private IQueryable<ProceduralFaultsConnEntityList> GetIqueryableList(IQueryable<ProceduralFaultsConnEntity> iQueryable)
        {
            IQueryable<ProceduralFaultsConnEntityList> query = (from a in iQueryable
                                                                     select new ProceduralFaultsConnEntityList()
                                                     {
                                                         Id = a.Id,
                                                         Tenant = a.Tenant,
                                                         EntityIdKey1 = a.EntityIdKey1,
                                                         EntityIdKey2 = a.EntityIdKey2,
                                                         EntityIdKey3 = a.EntityIdKey3,
                                                         EntityType = a.EntityType,
                                                         ProceduralFaultId = a.ProceduralFaultId,
                                                        
                                                     });
            return query;
		}

        private IQueryable<ProceduralFaultsConnEntity> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<ProceduralFaultsConnEntity> iQueryable, int tenant)
        {
            return iQueryable;
		}
	}


}
	