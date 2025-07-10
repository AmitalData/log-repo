using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.TariffModule.BL.EntityPMs;
using Logitude.TariffModule.Data;
using Logitude.TariffModule.Data.EntityPOCOs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.TariffModule.BL.EntityUpdateServices
{
    public partial class TariffProductUpdateService
    {
        protected override void OnCreating(TariffProductPM entityPM, EntityPM entityParentPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                this.ValidateTariffProductCode(entityPM, true);
            }
        }

        protected override void OnUpdating(TariffProductPM entityPM)
        {
            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                this.ValidateTariffProductCode(entityPM, false);
            }
        }

        private void ValidateTariffProductCode(TariffProductPM entityPM, bool isNewEntity)
        {
            bool exist = false;
            ITariffModuleContext iContext = TariffModuleContext.GetContext(entityPM.Tenant);

            if (isNewEntity)
            {
                exist = (from a in iContext.TariffProducts
                         where a.Code.ToLower() == entityPM.Code.ToLower() && a.Tenant == entityPM.Tenant
                         select a).Any();
            }

            else
            {
                exist = (from a in iContext.TariffProducts
                         where a.Code.ToLower() == entityPM.Code.ToLower()
                         && a.Id != entityPM.Id
                         && a.Tenant == entityPM.Tenant
                         select a).Any();
            }

            if (exist)
            {
                throw new ApplicationException("Code already exists");
            }
        }

        protected override void Trace(TariffProductPM entityPM, TariffProduct entityPOCO, string changesXml)
        {
            ICommonDataContext commonContext = CommonDataContext.GetContext(entityPM.Tenant);
            string loggedContactId = null;
            ContactRepository contactRepository = new ContactRepository(commonContext);
            Contact contact = contactRepository.GetSingleContactByEmail(AuthenticationUtil.GetAuthenticatedUser(), entityPM.Tenant);

            if (contact != null)
            {
                loggedContactId = contact.Id;
            }

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Insert)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "CREV",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "TariffProduct",
                    Notes = changesXml
                });
            }

            if (entityPM.ChangeSetOp == Simplog.Server.Infrastructure.ChangeSetOperation.Update)
            {
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = entityPM.Tenant,
                    EventTypeCode = "UPEV",
                    UserId = loggedContactId,
                    EntityId = entityPM.Id,
                    ObjectTableName = "TariffProduct",
                    Notes = changesXml
                });

                if (entityPM.Inactive && !entityPOCO.Inactive)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "ACTP",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = "TariffProduct",
                        Notes = changesXml
                    });
                }

                if (!entityPM.Inactive && entityPOCO.Inactive)
                {
                    EventTracer.CreateTraceEvent(new EventTracerArgs()
                    {
                        Tenant = entityPM.Tenant,
                        EventTypeCode = "RETP",
                        UserId = loggedContactId,
                        EntityId = entityPM.Id,
                        ObjectTableName = "TariffProduct",
                        Notes = changesXml
                    });
                }
            }
            
        }
    }
}
