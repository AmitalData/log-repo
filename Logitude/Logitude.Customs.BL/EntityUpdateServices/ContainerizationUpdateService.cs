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
            var declarationQueryService = new DeclarationQueryService(entityPM.Tenant);
            var declarationUpdateService = new DeclarationUpdateService(this.MainContext, new Dictionary<string, IContext>(), entityPM.Tenant);
            List<DeclarationPM> AllPms=new List<DeclarationPM>();
            if (!String.IsNullOrWhiteSpace(entityPM.ConnectedDeclarations))
            {
                var connectedDeclarations = entityPM.ConnectedDeclarations.Split(',').ToList();
                var pms = declarationQueryService.GetDeclarationsByIds(connectedDeclarations, entityPM.Tenant);
                AllPms.AddRange(pms);
                if (pms.Count == 0)
                {
                    //throw new Exception("Why ??"); 
                }
                else
                {
                    foreach (var declaration in pms)
                    {
                        if (declaration.ExportContainerizationID != entityPM.Id)
                        {
                            declaration.ExportContainerizationID = entityPM.Id;
                            declaration.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                        }
                    }
                }

            }
            if (!String.IsNullOrWhiteSpace(entityPM.NotConnectedDeclarations))
            {
                var disConnectedDeclarations = entityPM.NotConnectedDeclarations.Split(',').ToList();
                var pms=declarationQueryService.GetDeclarationsByIds(disConnectedDeclarations, entityPM.Tenant);
                AllPms.AddRange(pms);
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
                }

            }
            if (!String.IsNullOrWhiteSpace(entityPM.NotConnectedDeclarations) || !String.IsNullOrWhiteSpace(entityPM.ConnectedDeclarations)) {
                declarationUpdateService.UpdateMulti(AllPms, new List<DeclarationPM>(), entityPM, false); 
            }
            base.UpdateComposition(entityPM);
        }

        protected override void OnCreating(ContainerizationPM entityPM, EntityPM entityParentPM)
        {
            entityPM.ContainerizationDate = DateTime.Now;
            ContainerizationQueryService containerizationQueryService = new ContainerizationQueryService(entityPM.Tenant);
            if(containerizationQueryService != null)
            {
                entityPM.ContainerizationNumber = containerizationQueryService.GetContainerizationNumber(entityPM.Tenant).ToString();
            }
            entityPM.OperationMode = "1";

        }
        protected override void OnUpdating(ContainerizationPM entityPM, Containerization entityPOCO)
        {
            if (String.IsNullOrWhiteSpace(entityPM.ConnectedDeclarations))
            {
                entityPM.IsChange = false;
                entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
            }
            base.OnUpdating(entityPM, entityPOCO);
        }
    }
}
