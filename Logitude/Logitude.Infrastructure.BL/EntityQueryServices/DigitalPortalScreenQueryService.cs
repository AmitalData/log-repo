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
    public partial class DigitalPortalScreenQueryService
    {
        public List<DigitalPortalScreenList> GetDigitalPortalScreensQuery(int tenant, string objectTableId, string screenCode = "", string profileCode = "")
        {
            var digitalPortalScreenRepository = new DigitalPortalScreenRepository(tenant);

            var digitalPortalScreens = digitalPortalScreenRepository.GetDigitalPortalScreens(tenant, objectTableId, screenCode, profileCode)
                                                                    .Select(x => new DigitalPortalScreenList
                                                                    {
                                                                        Id = x.Id,
                                                                        Tenant = x.Tenant,
                                                                        Name = x.Name,
                                                                        CreateDate = x.CreateDate,
                                                                        UpdateDate = x.UpdateDate,
                                                                        Content = x.Content,
                                                                        DraftContent = x.DraftContent,
                                                                        ScreenCode = x.ScreenCode,
                                                                        ObjectTableId = x.ObjectTableId,
                                                                        ProfileId = x.ProfileId,
                                                                        IsList = x.IsList
                                                                    })
                                                                    .ToList();
            return digitalPortalScreens;
        }
        
        public List<DigitalPortalScreenList> GetDigitalPortalScreenNamesQuery(int tenant, string profileCode)
        {
            var digitalPortalScreenRepository = new DigitalPortalScreenRepository(tenant);

            var digitalPortalScreens = digitalPortalScreenRepository.GetDigitalPortalScreenNames(tenant, profileCode)
                                                                    .Select(x => new DigitalPortalScreenList
                                                                    {
                                                                        Id = x.Id,
                                                                        Tenant = x.Tenant,
                                                                        Name = x.Name,
                                                                        CreateDate = x.CreateDate,
                                                                        UpdateDate = x.UpdateDate,
                                                                        ScreenCode = x.ScreenCode,
                                                                        ObjectTableId = x.ObjectTableId,
                                                                        ProfileId = x.ProfileId,
                                                                        Content = x.Content,
                                                                        DraftContent = x.DraftContent,
                                                                        IsList = x.IsList                                                                        
                                                                    })
                                                                    .ToList();
            return digitalPortalScreens;
        }
        
        public List<DigitalPortalScreenList> GetDigitalPortalScreenNamesTenant0()
        {
            var digitalPortalScreenRepository = new DigitalPortalScreenRepository(0);
            var digitalPortalScreens = digitalPortalScreenRepository.GetDigitalPortalScreenNamesTenant0()
                                                                    .Select(x => new DigitalPortalScreenList
                                                                    {
                                                                        Id = x.Id,
                                                                        Tenant = x.Tenant,
                                                                        Name = x.Name,
                                                                        CreateDate = x.CreateDate,
                                                                        UpdateDate = x.UpdateDate,
                                                                        ScreenCode = x.ScreenCode,
                                                                        ObjectTableId = x.ObjectTableId,
                                                                        ProfileId = x.ProfileId,
                                                                        Content = x.Content,
                                                                        DraftContent = x.DraftContent,
                                                                        ProfileCode = x.DigitalProfile.Code,
                                                                        IsList = x.IsList
                                                                    })
                                                                    .ToList();
            return digitalPortalScreens;
        }
        
        public void UpdateDigitalPortalScreen(DigitalPortalScreenList digitalPortalScreenUpdateObject)
        {
            if (string.IsNullOrEmpty(digitalPortalScreenUpdateObject.Id))
            {
                var entityPm = new DigitalPortalScreenPM
                {
                    Name = digitalPortalScreenUpdateObject.Name,
                    ObjectTableId = digitalPortalScreenUpdateObject.ObjectTableId,
                    Tenant = digitalPortalScreenUpdateObject.Tenant,
                    ScreenCode = digitalPortalScreenUpdateObject.ScreenCode,
                    Content = digitalPortalScreenUpdateObject.Content,
                    DraftContent = digitalPortalScreenUpdateObject.DraftContent,
                    CreateDate = digitalPortalScreenUpdateObject.CreateDate,
                    UpdateDate = digitalPortalScreenUpdateObject.UpdateDate,
                    ProfileId = digitalPortalScreenUpdateObject.ProfileId,
                    IsList = digitalPortalScreenUpdateObject.IsList
                };

                entityPm.ChangeSetOp = ChangeSetOperation.Insert;
                var contextData = InfrastructureContext.GetContext(entityPm.Tenant);
                var service = new DigitalPortalScreenUpdateService(contextData, new Dictionary<string, IContext>(), entityPm.Tenant);
                service.Update(entityPm, true);
            }
            else
            {
                var entityPm = new DigitalPortalScreenPM
                {
                    Id = digitalPortalScreenUpdateObject.Id,
                    Name = digitalPortalScreenUpdateObject.Name,
                    ObjectTableId = digitalPortalScreenUpdateObject.ObjectTableId,
                    Tenant = digitalPortalScreenUpdateObject.Tenant,
                    Content = digitalPortalScreenUpdateObject.Content,
                    DraftContent = digitalPortalScreenUpdateObject.DraftContent,               
                    ScreenCode = digitalPortalScreenUpdateObject.ScreenCode,
                    CreateDate = digitalPortalScreenUpdateObject.CreateDate,
                    UpdateDate = digitalPortalScreenUpdateObject.UpdateDate,
                    ProfileId = digitalPortalScreenUpdateObject.ProfileId,
                    IsList = digitalPortalScreenUpdateObject.IsList
                };

                var contextData = InfrastructureContext.GetContext(entityPm.Tenant);
                var service = new DigitalPortalScreenUpdateService(contextData, new Dictionary<string, IContext>(), entityPm.Tenant);
                entityPm.ChangeSetOp = ChangeSetOperation.Update;
                service.Update(entityPm, true);
            }
        }
    }
}
