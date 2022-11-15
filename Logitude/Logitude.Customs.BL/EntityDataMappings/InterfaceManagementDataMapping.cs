
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Server.Tools; 
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Def.EntityPMs; 
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data.Repsitories;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class InterfaceManagementDataMapping: IMapping<InterfaceManagementPM, InterfaceManagement>
   {

        public void CustomPMToPOCO(InterfaceManagementPM entityPM, InterfaceManagement entityPOCO)
        {

            entityPOCO.Code = entityPM.Code;
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
            entityPM.SendTime = null;
            

            
        }
        private static void BuildSearchFields(InterfaceManagementPM entityPM, InterfaceManagement poco, bool isNewEntity)
        {
            string result = "";

            result = entityPM.Code 
                + "," + entityPM.DcaPrefixName + ","
                + "," + entityPM.DcaPrefixName2 + ","
                //256*3 + 32 
                //+ "," + entityPM.DcaPrefixName3 + ","
                //+ "," + entityPM.DcaPrefixName4 + "," 
                + entityPM.Description;           

            entityPM.SearchFields = result.ToLower();
            poco.SearchFields = entityPM.SearchFields;
        }


        public void CustomPOCOToPM(InterfaceManagementPM entityPM, InterfaceManagement entityPOCO)
        {
            InterfaceTenantDefinitionRepository definitionRep = new InterfaceTenantDefinitionRepository(entityPM.Tenant);
            InterfaceTenantDefinition definition = definitionRep.GetSingleDefinitionByCode(entityPM.Code, entityPM.Tenant);

            if (definition != null)
            {
                entityPM.Tenant = definition.Tenant;
                entityPM.TenantPriority = definition.TenantPriority;
                entityPM.TenantSendOptionsCode = definition.TenantSendOptionsCode;
                entityPM.SendTime = definition.SendTime;

                entityPM.DcaRenameFileEnable = definition.DcaRenameFileEnable;
                entityPM.DcaRenameFilePrefix = definition.DcaRenameFilePrefix;

            }

        }
   }


}
   