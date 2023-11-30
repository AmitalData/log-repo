using Logitude.BL.InfrastructureModel.EntityPMs;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.DataContracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.BL.InfrastructureModel.EntityQueries
{
    public class ChildEntitiesCustomFieldQuery
    {
        ChildEntitiesCustomFieldRepository repository;

        public ChildEntitiesCustomFieldQuery()
        {
            repository = new ChildEntitiesCustomFieldRepository();
        }

        public ChildEntitiesCustomFieldQuery(int tenant)
        {
            repository = new ChildEntitiesCustomFieldRepository(tenant);
        }

        public ChildEntitiesCustomFieldQuery(ChildEntitiesCustomFieldRepository ChildEntitiesCustomFieldRepository)
        {
            repository = ChildEntitiesCustomFieldRepository;
        }

        public ChildEntitiesCustomField GetSingle(string childEntityId, string childObjectTableId, int tenant)
        {
            ChildEntitiesCustomField ChildEntitiesCustomField = (from a in repository.context.ChildEntitiesCustomFields
                                                                 where a.Tenant == tenant && a.ChildEntityId == childEntityId && a.ChildObjectTableId == childObjectTableId
                                                                 select a).FirstOrDefault();

            return ChildEntitiesCustomField;
        }
    }
}
