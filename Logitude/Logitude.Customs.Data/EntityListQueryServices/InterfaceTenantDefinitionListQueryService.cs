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

    public partial class InterfaceTenantDefinitionListQueryService
    {
	    private IQueryable<InterfaceTenantDefinitionList> GetIqueryableList(IQueryable<InterfaceTenantDefinition> iQueryable)
        {
            IQueryable<InterfaceTenantDefinitionList> query = (from a in iQueryable
                                                               select new InterfaceTenantDefinitionList()
                                                         {
                                                             Code = a.Code,
                                                            Active = a.Active,
                                                          
                                                            Id = a.Id,
                                                            Tenant = a.Tenant,
                                                            TenantPriority = a.TenantPriority,
                                                            TenantSendOptionsCode = a.TenantSendOptionsCode,


                                                         });
            return query;
		}

        private IQueryable<InterfaceTenantDefinition> ApplyCustomFilters(QueryOperations queryOperations, IQueryable<InterfaceTenantDefinition> iQueryable, int tenant)
        {
            return iQueryable;
		}
	}


}
	