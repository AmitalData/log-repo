
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

namespace Logitude.Customs.BL.EntityDataMappings
{
   
   public partial class ContainerizationDataMapping: IMapping<ContainerizationPM, Containerization>
   {

        public void CustomPMToPOCO(ContainerizationPM entityPM, Containerization entityPOCO)
        {
            //throw new NotImplementedException();
            

        }

        public void CustomPOCOToPM(ContainerizationPM entityPM, Containerization entityPOCO)
        {
            //throw new NotImplementedException();
            this.CustomMappedPMProperties.Add(PMPropertyNames.ImporterName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.ContainerizationStatusName);
            this.CustomMappedPMProperties.Add(PMPropertyNames.HataraStatusName);
            
            var declarationRepository = new DeclarationRepository(entityPOCO.Tenant);
            var declarations=declarationRepository.GetByExportContainerizationID(entityPOCO.Id, entityPOCO.Tenant);
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
   }


}
   