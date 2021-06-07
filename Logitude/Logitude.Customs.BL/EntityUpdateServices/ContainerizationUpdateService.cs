using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class ContainerizationUpdateService : EntityUpdateService<Containerization, ContainerizationPM, EntityPM>
    {

        protected override void UpdateComposition(ContainerizationPM entityPM)
        {

            if (!String.IsNullOrWhiteSpace(entityPM.NotConnectedDeclarations))
            {
                var disConnectedDeclarations = entityPM.NotConnectedDeclarations.Split(',').ToList();
                var declarationQueryService = new DeclarationQueryService(entityPM.Tenant);
                var declarationUpdateService = new DeclarationUpdateService(this.MainContext , new Dictionary<string, IContext>(), entityPM.Tenant);
                var pms=declarationQueryService.GetDeclarationsByIds(disConnectedDeclarations, entityPM.Tenant);
                if (pms.Count == 0)
                {
                    //throw new Exception("Why ??"); 
                }
                else
                {
                    foreach (var declaration in pms)
                    {
                        declaration.ExportContainerizationID = null;
                        declaration.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;


                    }
                    declarationUpdateService.UpdateMulti(pms, new List<DeclarationPM>(), entityPM, false);
                }

            }
            base.UpdateComposition(entityPM);
        }
    }
}
