using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.TariffModule.BL.EntityPMs;
using Logitude.TariffModule.Data;
using Logitude.TariffModule.Data.EntityPOCOs;
using Logitude.TariffModule.Data.Repositories;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TariffModule.BL.EntityUpdateServices
{
    public partial class TariffUpdateService
    {
        protected override void OnCreating(TariffPM entityPM, Server.Tools.EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                entityPM.Id = IdCounter.GetNumber("Tariff", entityPM.Tenant);
                entityPM.TariffNumber = CodeCounter.GetNumber("Tariff", entityPM.Tenant).ToString();
                entityPM.CreateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
                entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

                if (entityPM.PriceSteps == null)
                {
                    ITariffModuleContext iContext= TariffModuleContext.GetContext(entityPM.Tenant);
                    TariffSetting iTariffSetting = (from d in iContext.TariffSettings where d.Tenant == entityPM.Tenant select d).FirstOrDefault();
                    if (iTariffSetting != null)
                    {
                        entityPM.PriceSteps = iTariffSetting.DefaultPriceSteps;
                    }
                }
            }
        }

        protected override void OnUpdating(TariffPM entityPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                entityPM.UpdateDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);
            }

            //DateTime myDate = TenantServerConfigration.GetCurrentDateTime(entityPM.Tenant);

            ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
            ContactRepository contactRep = new ContactRepository(commonContext);
            string email = "";
            if (AuthenticationUtil.IsAuthenticatedUserExists())
            {
                email = AuthenticationUtil.GetAuthenticatedUser();
            }

            else
            {
                email = "system@tenant" + entityPM.Tenant + ".com";
            }

            string myLoggedUserId = null;
            Contact contact = contactRep.GetSingleContactByEmail(email, entityPM.Tenant);
            if (contact != null)
            {
                myLoggedUserId = contact.Id;
            }

            entityPM.UpdatedByUserId = myLoggedUserId;

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                if (entityPM.CreatedByUserId == null)
                {
                    entityPM.CreatedByUserId = myLoggedUserId;
                }

                this.CreateTariffVersion(entityPM);
            }
        }

        protected override void UpdateComposition(TariffPM entityPM)
        {
            TariffLineUpdateService tariffLineUpdateService = new TariffLineUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            tariffLineUpdateService.UpdateMulti(entityPM.TariffLines, entityPM.DeletedTariffLines, entityPM, false);
        }

        protected override void Trace(TariffPM entityPM, Tariff entityPOCO, string changesXml)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
            ContactRepository contactRep = new ContactRepository(commonContext);
            Contact contact = contactRep.GetSingleContactByEmail(AuthenticationUtil.GetAuthenticatedUser(), entityPM.Tenant);
            if(entityPM.TariffLines.Where(p=>p.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert).Count() > 0)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "TLAD",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Tariff",
                    Notes = changesXml
                });
            }
            if (entityPM.SetAsInActive)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "SAIN",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Tariff",
                    Notes = changesXml
                });
            }



            if (entityPM.SetAsReActive)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "REAC",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Tariff",
                    Notes = changesXml
                });
            }


            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPEV",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Tariff",
                    Notes = changesXml
                });

            }

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CREV",
                    UserId = contact.Id,
                    EntityId = entityPM.Id,
                    ObjectTableName = "Tariff",
                    Notes = changesXml
                });
            }

        }

        private void CreateTariffVersion(TariffPM entityPM)
        {
            entityPM.LastVersion += 1;

            TariffVersionPM tariffVersionPM = new TariffVersionPM()
            {
                TariffId = entityPM.Id,
                Tenant = entityPM.Tenant,
                CreateDate = entityPM.CreateDate,
                CreatedByUserId = entityPM.CreatedByUserId,
                StartDate = entityPM.StartDate,
                ExpirationDate = entityPM.ExpirationDate,
                Version = entityPM.LastVersion,
                SearchFields = entityPM.LastVersion.ToString(),
                ChangeSetOp = ChangeSetOperation.Insert,
                IsDraft = true,
            };

            TariffVersionUpdateService tariffVersionUpdateService = new TariffVersionUpdateService(MainContext, new Dictionary<string, IContext>(), Tenant);
            tariffVersionUpdateService.Update(tariffVersionPM, true);
        }
    }
}
