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
            var connectedDeclarations = entityPM.ConnectedDeclarations.Split(',').ToList();
            var context = CustomContext.GetContext(entityPM.Tenant);
            DeclarationRepository declarationRepository = new DeclarationRepository(context);
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(declarationRepository);

            var list = declarationQueryService.GetContainerizationUniqueConsignment(connectedDeclarations);
            if (entityPM.ContainerizationStatus == "3" && !string.IsNullOrEmpty(entityPM.ExistInCustoms))
            {
                var IsExist = list.Any(x =>
                     ((!string.IsNullOrEmpty(x.CargoTypeCode) ? (x.CargoTypeCode) : "") + "-" +
                              (!string.IsNullOrEmpty(x.ManifestNumber) ? (x.ManifestNumber) : "") + "-" +
                              (!string.IsNullOrEmpty(x.SecondCargoId) ? (x.SecondCargoId) : "") + "-" +
                              (!string.IsNullOrEmpty(x.ThirdCargoId) ? (x.ThirdCargoId) : "")) == entityPM.ExistInCustoms

                 );

                if (!IsExist)
                {
                    List<ContainerizationDetails> listCD = new List<ContainerizationDetails>();
                    ContainerizationDetails CD = new ContainerizationDetails();
                    CD.Id = "1";
                    CD.ContainerizationNumber = "1";
                    CD.Tenant = 0;
                    listCD.Add(CD);
                    return listCD;
                }
            }
            int counter = 0;

            var containerizationListKeys = new List<string>();
            ContainerizationRepository containerizationRepository = new ContainerizationRepository(context);
            ContainerizationQueryService containerizationQueryService = new ContainerizationQueryService(containerizationRepository);

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

                    if (dec.IsNew && entityPM.ContainerizationStatus!="3")
                    {
                        var ContainerizationPM = new ContainerizationPM();
                        
                        ContainerizationPM.AgentDeclaration = entityPM.AgentDeclaration;
                        ContainerizationPM.Tenant = entityPM.Tenant;
                        ContainerizationPM.OperationMode = "1";
                        ContainerizationPM.IsChange = entityPM.IsChange;
                        ContainerizationPM.ConnectedDeclarations = dec.DeclarationId;
                        ContainerizationPM.CargoTypeCode = dec.CargoTypeCode;
                        ContainerizationPM.ManifestNumber = dec.ManifestNumber;
                        ContainerizationPM.SecondCargoID = dec.SecondCargoId;
                        ContainerizationPM.ThirdCargoID = dec.ThirdCargoId;
                        ContainerizationPM.TransportModeId = dec.TransportModeId;
                        ContainerizationPM.ChangeSetOp = ChangeSetOperation.Insert;
                        containerizationList.Add(ContainerizationPM);
                    }
                    else
                    {

                        var ContainerizationPMById=new ContainerizationPM();


                        if (dec.IsNew && entityPM.ContainerizationStatus == "3" )
                        {
                             ContainerizationPMById = containerizationQueryService.GetSingle(entityPM.Id, false, true);

                            ContainerizationPMById.CargoTypeCode = dec.CargoTypeCode;
                            ContainerizationPMById.ManifestNumber = dec.ManifestNumber;
                            ContainerizationPMById.SecondCargoID = dec.SecondCargoId;
                            ContainerizationPMById.ThirdCargoID = dec.ThirdCargoId;
                            ContainerizationPMById.TransportModeId = dec.TransportModeId;
                            ContainerizationPMById.ContainerizationStatus = null;
                          
                        }
                        else
                        {
                             ContainerizationPMById = containerizationQueryService.GetSingle(dec.Id, false, true);
                         
                        }

                        ContainerizationPMById.OperationMode = "2";
                        ContainerizationPMById.ConnectedDeclarations = dec.DeclarationId;
                        ContainerizationPMById.ChangeSetOp = ChangeSetOperation.Update;
                        containerizationList.Add(ContainerizationPMById);
                    }
                 
                    containerizationListKeys.Add((dec.CargoTypeCode?.ToLower() + dec.ManifestNumber?.ToLower() + dec.SecondCargoId?.ToLower() + dec.ThirdCargoId?.ToLower()).ToString());
                }

            }

            if (entityPM.ContainerizationStatus == "3" && (containerizationList.Count() > 1 || list.Any(x => !x.IsNew)))
            {
                List<ContainerizationDetails> listCD = new List<ContainerizationDetails>();
                ContainerizationDetails CD = new ContainerizationDetails();
                CD.Id = "0";
                CD.ContainerizationNumber = "0";
                CD.Tenant = 0;
                listCD.Add(CD);
                return listCD;
            }
            ContainerizationUpdateService containerizationUpdateService = new ContainerizationUpdateService(context, new Dictionary<string, IContext>(), entityPM.Tenant);
            foreach (var containerization in containerizationList)
            {
                containerizationUpdateService.Update(containerization, true);
            }

            var listCon = containerizationQueryService.GetContainerizationByKeys(entityPM.Tenant, containerizationListKeys);

            return listCon;
        }



    }
}
