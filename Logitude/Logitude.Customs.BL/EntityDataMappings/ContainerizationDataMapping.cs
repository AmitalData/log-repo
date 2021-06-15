
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
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.BL.EntityQueryServices;
using Simplog.Server.Infrastructure;

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class ContainerizationDataMapping: IMapping<ContainerizationPM, Containerization>
   {

        public void CustomPMToPOCO(ContainerizationPM entityPM, Containerization entityPOCO)
        {
            entityPOCO.Id = entityPM.Id;
            BuildSearchFields(entityPM, entityPOCO, entityPM.ChangeSetOp == ChangeSetOperation.Insert);
            entityPOCO.SearchFields = entityPM.SearchFields;
        }

        public void CustomPOCOToPM(ContainerizationPM entityPM, Containerization entityPOCO)
        {
            //throw new NotImplementedException();
            this.CustomMappedPMProperties.Add(PMPropertyNames.ImporterName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ContainerizationStatusName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.HataraStatusName);
            var declarationRepository = new DeclarationRepository(entityPOCO.Tenant);
            var declarations=declarationRepository.GetByExportContainerizationID(entityPOCO.Id, entityPOCO.Tenant);
           /* var items = declarations.ToList();
            foreach(Declaration dec in items)
            {
                entityPM.ConnectedDeclarations= entityPM.ConnectedDeclarations + dec.Id + ",";
            }*/
            var declaration= declarations.FirstOrDefault();
            if (declaration != null)
            {
                var customerCard = CardRepository.GetSingleCard(declaration.CustomerId, entityPOCO.Tenant, true);
                if (customerCard!=null)
                {
                    entityPM.ImporterName = customerCard.LocalName;
                }
                
            }
            var containerizationStatusCodeQueryService = new ContainerizationStatusCodeQueryService(entityPOCO.Tenant);
            entityPM.ContainerizationStatusName= containerizationStatusCodeQueryService.GetSingle(entityPOCO.ContainerizationStatus,false,true)?.Name;
            var declarationStatusTypeQueryService = new DeclarationStatusTypeQueryService(entityPOCO.Tenant);
            entityPM.HataraStatusName = declarationStatusTypeQueryService.GetSingle(entityPOCO.HataraStatus,false,true)?.LocalName;

        }
        private static void BuildSearchFields(ContainerizationPM entityPM, Containerization poco, bool isNewEntity)
        {
            string result = "";
            entityPM.SearchFields = result.ToLower();
            var declarationRepository = new DeclarationRepository(entityPM.Tenant);
            var declarations = declarationRepository.GetByExportContainerizationID(entityPM.Id, entityPM.Tenant);
            var declaration = declarations.FirstOrDefault();
            if (declaration != null)
            {
                ConsignmentQueryService cosigmentQuery = new ConsignmentQueryService(poco.Tenant);
                ConsignmentPM consignment = cosigmentQuery.GetSingle(declaration.Id, 1, false, false);
                if (consignment != null)
                {
                    if (!string.IsNullOrEmpty(consignment.ManifestNumber))
                    {
                        result = string.IsNullOrEmpty(result) ? consignment.ManifestNumber : result + "," + consignment.ManifestNumber;
                    }
                    if (!string.IsNullOrEmpty(consignment.SecondCargoID))
                    {
                        result = string.IsNullOrEmpty(result) ? consignment.SecondCargoID : result + "," + consignment.SecondCargoID;
                    }
                    if (!string.IsNullOrEmpty(consignment.ThirdCargoID))
                    {
                        result = string.IsNullOrEmpty(result) ? consignment.ThirdCargoID : result + "," + consignment.ThirdCargoID;
                    }
                }
                if (!string.IsNullOrEmpty(declaration.ExportFile))
                {
                    result = string.IsNullOrEmpty(result) ? declaration.ExportFile : result + "," + declaration.ExportFile;
                }
            }
            poco.SearchFields = entityPM.SearchFields;
        }
    }
}
