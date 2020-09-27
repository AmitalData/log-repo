using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class DeclarationExportRecipientUpdateService : EntityUpdateService<DeclarationExportRecipient, DeclarationExportRecipientPM, DeclarationPM>
    {
        int? maxCounter;
        protected override void OnCreating(DeclarationExportRecipientPM entityPM, DeclarationPM entityParentPM)
        {
             entityPM.DeclarationId = entityParentPM.Id;
            entityPM.Tenant = entityParentPM.Tenant;
            //entityPM.LineNumber

            if (!this.maxCounter.HasValue)
            {
                ICustomContext _Context = MainContext as CustomContext;
                //var DeclarationExportQueryService = new DeclarationExportRecipientQueryService(_Context);

                this.maxCounter = (this.Repository as DeclarationExportRecipientRepository).GetMaxCounterKey(entityPM.DeclarationId, entityPM.Tenant) ?? 0;
            }
            maxCounter = entityPM.LineNumber = maxCounter.Value + 1;
        }
    }
}
