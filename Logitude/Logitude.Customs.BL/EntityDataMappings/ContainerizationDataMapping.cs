
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
using Logitude.Customs.Data.EntityKeys;

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
            var items = declarations.ToList();
            foreach(Declaration dec in items)
            {
                entityPM.ConnectedDeclarations= entityPM.ConnectedDeclarations + dec.Id + ",";
            }
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
            var ContainerizationStatusQueryService = new ContainerizationHataraStatusQueryService(entityPOCO.Tenant);
            entityPM.HataraStatusName = ContainerizationStatusQueryService.GetSingle(entityPOCO.HataraStatus,false,true)?.Name;

        }
        private static void BuildSearchFields(ContainerizationPM entityPM, Containerization poco, bool isNewEntity)
        {
            string result = "";
            if (!string.IsNullOrEmpty(entityPM.ConnectedDeclarations))
            {
                var declarationQueryService = new DeclarationQueryService(entityPM.Tenant);
                var ConnectedDeclarations = entityPM.ConnectedDeclarations.Split(',').ToList();
                var pms = declarationQueryService.GetDeclarationsByIds(ConnectedDeclarations, entityPM.Tenant);
                foreach (var declaration in pms)
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
                    if (!string.IsNullOrEmpty(declaration.CustomerName))
                    {
                            result = string.IsNullOrEmpty(result) ? declaration.CustomerName : result + "," + declaration.CustomerName;
                    }
                }
            }
            if (!string.IsNullOrEmpty(entityPM.ContainerizationNumber))
            {
                result = string.IsNullOrEmpty(result) ? entityPM.ContainerizationNumber : result + "," + entityPM.ContainerizationNumber;
            }
            var splittedResult = result.Split(',');
            var distinctResult = splittedResult.Distinct().ToList();
            result = "";
            foreach (var item in distinctResult)
            {
                result = string.IsNullOrEmpty(result) ? item : result + "," + item;
            }
            entityPM.SearchFields = result.ToLower();
            poco.SearchFields = entityPM.SearchFields;
        }
    }
}
