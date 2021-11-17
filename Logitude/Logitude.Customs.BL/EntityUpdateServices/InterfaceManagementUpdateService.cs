using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logitude.Customs.BL.EntityUpdateServices
{
    public partial class InterfaceManagementUpdateService : EntityUpdateService<InterfaceManagement, InterfaceManagementPM, EntityPM>
      {

        protected override void OnUpdating(InterfaceManagementPM entityPM)
        {
            if (entityPM.ChangeSetOp != ChangeSetOperation.Insert)
            {
                ICustomContext context = MainContext as CustomContext;
                InterfaceTenantDefinitionRepository definitionRep = new InterfaceTenantDefinitionRepository(context);
                InterfaceTenantDefinition definition = definitionRep.GetSingleDefinitionByCode(entityPM.Code, entityPM.Tenant);
                if (definition != null)
                {
                    MapInterface2TenantDef(entityPM, definition);
                    definitionRep.Update(definition);

                }
                else
                {
                    definition = new InterfaceTenantDefinition();
                    //definition.TenantSendOptionsCode = entityPM.TenantSendOptionsCode;
                    //definition.TenantPriority = entityPM.TenantPriority;
                    MapInterface2TenantDef(entityPM, definition);
                    definition.Id = IdCounter.GetNumber("Customs.InterfaceTenantDefinition", entityPM.Tenant);
                    definition.Code = entityPM.Code;
                    definition.Tenant = entityPM.Tenant;
                    

                    definitionRep.Add(definition);

                }

            }
        }

        private static void MapInterface2TenantDef(InterfaceManagementPM entityPM, InterfaceTenantDefinition definition)
        {
            definition.TenantSendOptionsCode = entityPM.TenantSendOptionsCode;
            definition.TenantPriority = entityPM.TenantPriority;
            definition.DcaRenameFileEnable = entityPM.DcaRenameFileEnable;
            definition.DcaRenameFilePrefix = entityPM.DcaRenameFilePrefix;
            definition.Active = entityPM.Active;//on the way fix bug ???(or make one ??)
        }
    }
}
