
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
using Logitude.Server.Tools.Helpers;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.BL.EntityQueryServices;

namespace Logitude.Customs.BL.EntityDataMappings
{   
   public partial class LogisticActionRequestDataMapping: IMapping<LogisticActionRequestPM, LogisticActionRequest>
   {
        public void CustomPMToPOCO(LogisticActionRequestPM entityPM, LogisticActionRequest entityPOCO)
        {
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Id);
            CustomMappedPOCOProperties.Add(POCOPropertyNames.Tenant);

            if (entityPM.ChangeSetOp == ChangeSetOperation.Insert)
            {
                entityPOCO.Id = entityPM.Id;
                entityPOCO.Tenant = entityPM.Tenant;
            }

            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
        }


        public void CustomPOCOToPM(LogisticActionRequestPM entityPM, LogisticActionRequest entityPOCO)
        {
            entityPM.Tenant = entityPOCO.Tenant;
            if(entityPOCO.ResponseStatusCode != null)
                entityPM.RequestCancelStatus =  new LogisticActionResponseReqSRepository(entityPOCO.Tenant).GetSingle(entityPOCO.ResponseStatusCode).LocalName;
            if (entityPOCO.ExporterNumber != null)
            {
                ClientQueryService clientQueryService = new ClientQueryService(entityPOCO.Tenant);
                ClientPM client = clientQueryService.GetClientByCode(entityPOCO.ExporterNumber, entityPOCO.Tenant);
                if (client != null)
                {
                    entityPM.CalculatedExporterName = client.FullName;
                }
            }
        }
        

        private void BuildSearchFields(LogisticActionRequestPM entityPM, LogisticActionRequest entityPOCO, bool isNewEntity)
        {
            string mySearchFields = "";

            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ExportFileNo);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.ExporterNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.DeclarationNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.RequestNumber);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.CargoIdentifierKey1);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.CargoIdentifierKey2);
            MethodHelper.AddToSearchFields(ref mySearchFields, entityPM.CargoIdentifierKey3);

            mySearchFields = mySearchFields.ToLower();
            entityPM.SearchFields = mySearchFields;
            entityPOCO.SearchFields = mySearchFields;
        }
    }
}
   