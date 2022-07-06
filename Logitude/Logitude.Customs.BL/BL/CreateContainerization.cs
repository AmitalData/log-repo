using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Logitude.Customs.Data.Repsitories.ContainerizationRepository;

namespace Logitude.Customs.BL.BL
{
    public class CreateContainerization
    {
        public List<ContainerizationDetails> CreateContainerizations(ContainerizationPM entityPM)
        {

          

            var containerizationList = new List<ContainerizationPM>();
            var connectedDeclarations=entityPM.ConnectedDeclarations.Split(',').ToList();
            var context = CustomContext.GetContext(entityPM.Tenant);
            DeclarationRepository declarationRepository = new DeclarationRepository(context);
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(declarationRepository);
          
            var list = declarationQueryService.GetContainerizationUniqueConsignment(connectedDeclarations);
            int counter = 0;
           
            var containerizationListKeys = new List<string>();
            ContainerizationRepository containerizationRepository = new ContainerizationRepository(context);
            ContainerizationQueryService containerizationQueryService = new ContainerizationQueryService(containerizationRepository);
            if (list.Count == 0)
            {
                return null;
            }
            foreach (var dec in list)
            {
               

                   if (containerizationList.Count > 0 && containerizationList[counter] != null && containerizationList[counter].CargoTypeCode == dec.CargoTypeCode && containerizationList[counter].ManifestNumber == dec.ManifestNumber && containerizationList[counter].SecondCargoID == dec.SecondCargoId && containerizationList[counter].ThirdCargoID == dec.ThirdCargoId)
                   {
                       containerizationList[counter].ConnectedDeclarations = containerizationList[counter].ConnectedDeclarations + "," + dec.DeclarationId;
                   }
                   else
                   {
                       if (containerizationList.Count > 0) 
                       {
                           counter++;
                       }
                       var ContainerizationPM = new ContainerizationPM();
                      
                        
                       ContainerizationPM.AgentDeclaration = entityPM.AgentDeclaration;
                       ContainerizationPM.Tenant = entityPM.Tenant;
                       ContainerizationPM.OperationMode = entityPM.OperationMode;
                       ContainerizationPM.IsChange = entityPM.IsChange;
                       ContainerizationPM.ConnectedDeclarations = dec.DeclarationId;
                       ContainerizationPM.CargoTypeCode = dec.CargoTypeCode;
                       ContainerizationPM.ManifestNumber = dec.ManifestNumber;
                       ContainerizationPM.SecondCargoID = dec.SecondCargoId;
                       ContainerizationPM.ThirdCargoID = dec.ThirdCargoId;
                       if (dec.IsNew)
                       {
                           ContainerizationPM.ChangeSetOp = ChangeSetOperation.Insert;
                       }
                       else
                       {
                        var conById = declarationQueryService.GetContainerizationByID(dec.Id);
                          ContainerizationPM.Id = dec.Id;
                          ContainerizationPM.ContainerizationDate = conById.ContainerizationDate;
                          ContainerizationPM.ContainerizationNumber = conById.ContainerizationNumber;
                          ContainerizationPM.ContainerizationStatus = conById.ContainerizationStatus;
                          ContainerizationPM.HataraStatus = conById.HataraStatus;
                          ContainerizationPM.IsMultiCustomers = conById.IsMultiCustomers;
                          ContainerizationPM.ChangeSetOp = ChangeSetOperation.Update;
                       }
                       containerizationList.Add(ContainerizationPM);
                       containerizationListKeys.Add((dec.CargoTypeCode.ToLower() + dec.ManifestNumber.ToLower() + dec.SecondCargoId.ToLower() + dec.ThirdCargoId.ToLower()).ToString());
                   }
               
            }
            ContainerizationUpdateService containerizationUpdateService = new ContainerizationUpdateService(context, new Dictionary<string, IContext>(), entityPM.Tenant);
            foreach (var containerization in containerizationList)
            {
                   containerizationUpdateService.Update(containerization, true);
            }
           
          var listCon =  containerizationQueryService.GetContainerizationByKeys(entityPM.Tenant, containerizationListKeys);

            return listCon;
        }



    }
}
