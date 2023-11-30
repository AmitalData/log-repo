using Logitude.Infrastructure.BL.EntityPMs;
using Logitude.Infrastructure.BL.EntityUpdateServices;
using Logitude.Infrastructure.Data;
using Logitude.Infrastructure.Data.EntityLists;
using Logitude.Infrastructure.Data.Repsitories;
using Simplog.Server.Infrastructure;
using System.Collections.Generic;
using System.Linq;

namespace Logitude.Infrastructure.BL.EntityQueryServices
{
    public partial class DigitalPreDefinedComponentQueryService
    {
        public List<DigitalPreDefinedComponentList> GetDigitalPreDefinedComponentQuery(int tenant, string objectTableId, string name = "")
        {
            var digitalPreDefinedComponentRepository = new DigitalPreDefinedComponentRepository(tenant);
            var digitalPortalScreens = digitalPreDefinedComponentRepository.GetDigitalPreDefinedComponents(tenant, objectTableId, name)
                                                                           .Select(x => new DigitalPreDefinedComponentList
                                                                           {
                                                                               Id = x.Id,
                                                                               Tenant = x.Tenant,
                                                                               Name = x.Name,
                                                                               CreateDate = x.CreateDate,
                                                                               UpdateDate = x.UpdateDate,
                                                                               ObjectTableId = x.ObjectTableId,
                                                                               Content = x.Content
                                                                           })
                                                                           .ToList();
            return digitalPortalScreens;
        }


        public void UpdateDigitalPreDefinedComponent(DigitalPreDefinedComponentList digitalPreDefinedComponentUpdateObject)
        {
            if (string.IsNullOrEmpty(digitalPreDefinedComponentUpdateObject.Id))
            {
                var entityPm = new DigitalPreDefinedComponentPM
                {
                    ObjectTableId = digitalPreDefinedComponentUpdateObject.ObjectTableId,
                    Tenant = digitalPreDefinedComponentUpdateObject.Tenant,
                    Name = digitalPreDefinedComponentUpdateObject.Name,
                    Content = digitalPreDefinedComponentUpdateObject.Content,
                    CreateDate = digitalPreDefinedComponentUpdateObject.CreateDate,
                    UpdateDate = digitalPreDefinedComponentUpdateObject.UpdateDate
                };

                entityPm.ChangeSetOp = ChangeSetOperation.Insert;
                var contextData = InfrastructureContext.GetContext(entityPm.Tenant);
                DigitalPreDefinedComponentUpdateService service = new DigitalPreDefinedComponentUpdateService(contextData, new Dictionary<string, IContext>(), entityPm.Tenant);
                service.Update(entityPm, true);
            }
            else
            {
                var entityPm = new DigitalPreDefinedComponentPM
                {
                    Id = digitalPreDefinedComponentUpdateObject.Id,
                    ObjectTableId = digitalPreDefinedComponentUpdateObject.ObjectTableId,
                    Tenant = digitalPreDefinedComponentUpdateObject.Tenant,
                    Content = digitalPreDefinedComponentUpdateObject.Content,
                    Name = digitalPreDefinedComponentUpdateObject.Name,
                    CreateDate = digitalPreDefinedComponentUpdateObject.CreateDate,
                    UpdateDate = digitalPreDefinedComponentUpdateObject.UpdateDate
                };

                var contextData = InfrastructureContext.GetContext(entityPm.Tenant);
                DigitalPreDefinedComponentUpdateService service = new DigitalPreDefinedComponentUpdateService(contextData, new Dictionary<string, IContext>(), entityPm.Tenant);
                entityPm.ChangeSetOp = ChangeSetOperation.Update;
                service.Update(entityPm, true);
            }
        }
    }
}
