using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.Data;
using Logitude.CustomsMessaging.Common.RequestParams;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
 using UnifreightIIG.Common.ContainerizationMessageServiceReference;
using Logitude.Customs.Data.Repsitories;

namespace Logitude.CustomsMessaging.RequestServices
{
    public class SaveCC_MSG2450_ContainerizationMessageRequestService
        : RequestServiceBase<AV_MSG2_ContainerizationMessage, GenericRequestParams>
    {
        public override AV_MSG2_ContainerizationMessage GetRequest(GenericRequestParams requestParams)
        {
            var req = new AV_MSG2_ContainerizationMessage();
            ICustomContext customContext = CustomContext.GetContext(requestParams.Tenant);

            ContainerizationQueryService containerizationQueryService = new ContainerizationQueryService(customContext);
            CustomsSettingQueryService customsSettingQueryService = new CustomsSettingQueryService(customContext);
            DeclarationQueryService declarationQueryService = new DeclarationQueryService(customContext);
            var settings = customsSettingQueryService.GetSingleByTenant(requestParams.Tenant);
                        var containerization = containerizationQueryService.GetSingle(requestParams.AppicationId, true, false);

            var declarationRepository = new DeclarationRepository(requestParams.Tenant);
            var declarations = declarationRepository.GetByExportContainerizationID(requestParams.AppicationId, requestParams.Tenant);

          

            req.ContainerizationDetails = new AV_MSG2_ContainerizationMessageContainerizationDetails();


            req.ContainerizationDetails.operationCode =Convert.ToInt32( containerization.OperationMode);
            req.ContainerizationDetails.operationType = 2;
            req.ContainerizationDetails.CustomsAgentID = Convert.ToInt32(settings.CustomsAgentId);
            req.ContainerizationDetails.AgentDeclaration =true ;
            req.ContainerizationDetails.containerizationDate = containerization.ContainerizationDate;

               List<AV_MSG2_ContainerizationMessageDeclaration> connectedDeclarations = new List<AV_MSG2_ContainerizationMessageDeclaration>();

            req.ContainerCargo = new AV_MSG2_ContainerizationMessageContainerCargo();
            req.ContainerCargo.cargoIdentifier = new cargoIdentifier();

            var cons = declarationQueryService.GetConsignmentListPMByDeclarationId(declarations.ToList()[0].Id, requestParams.Tenant);

            req.ContainerCargo.cargoIdentifier.cargoIdentifierType = Convert.ToInt32(cons[0].CargoTypeCode);
            req.ContainerCargo.cargoIdentifier.cargoIdentifierKey1 =  cons[0].ManifestNumber;
            req.ContainerCargo.cargoIdentifier.cargoIdentifierKey2 = cons[0].SecondCargoID;
            req.ContainerCargo.cargoIdentifier.cargoIdentifierKey3 = cons[0].ThirdCargoID;


            foreach (var dec in declarations)
            {
                AV_MSG2_ContainerizationMessageDeclaration aV_MSG2_ContainerizationMessageDeclaration = new AV_MSG2_ContainerizationMessageDeclaration();
                aV_MSG2_ContainerizationMessageDeclaration.DeclarationID = dec.Id;
                aV_MSG2_ContainerizationMessageDeclaration.LeadDocumentType = Convert.ToInt32(dec.DeclarationTypeCode);
                connectedDeclarations.Add(aV_MSG2_ContainerizationMessageDeclaration);
             }


            req.Declaration = connectedDeclarations.ToArray();

            return req;
        }

     }
}
