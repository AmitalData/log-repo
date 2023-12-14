using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityKeys;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Server.Tools;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceModel.Activation;
using System.Transactions;
using System.Web.Http;
using WebFreight.Web.Security;

namespace WebFreight.Web.WcfApi
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class DeclarationStatusWcfService : IDeclarationStatusWcfService
    {

        public Response Upsert(List<DeclarationStatusPM> entityPM, bool batch)
        {
            Response response = new Response();
            return response;
        }
        public Response BuildDeclarationStatusesList(int tenant, string customFileNo, List<DeclarationStatusPM> DeclarationStatusesList)
        {
            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("DeclarationStatus", "UPDATE", tenant);//UPDATE//READ
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {

                    DeclarationStatusesList = DeclarationStatusesList.OrderBy(e => e.StatusDate).ToList();

                    ICustomContext objectContext = CustomContext.GetContext(tenant);


                    DeclarationRepository declarationRepository = new DeclarationRepository(objectContext);
                    DeclarationStatusRepository declarationStatusRepository = new DeclarationStatusRepository(objectContext);
                    DeclarationStatusQueryService declarationStatusQueryService = new DeclarationStatusQueryService(objectContext);
                    DeclarationStatusUpdateService declarationStatusUpdateService = new DeclarationStatusUpdateService(objectContext, new Dictionary<string, IContext>(), tenant);
                    StatusCodeRepository StatusCodeRepository = new StatusCodeRepository(objectContext);


                    Declaration entityPoco = declarationRepository.GetByCustomFileNo(customFileNo, tenant);
                    if (entityPoco != null)
                    {
                        //var oldDeclarationStatusesIds = declarationStatusRepository.GetIdsByDeclarationIdAndTenant(tenant, entityPoco.Id);
                         declarationStatusRepository.DeleteByIdAndTenant(entityPoco.Id,tenant); // delete all old

                        foreach (DeclarationStatusPM declarationStatus in DeclarationStatusesList)
                        {
                            var statusCode = StatusCodeRepository.GetSingleByCode(declarationStatus.StatusID,tenant);
                            if (statusCode != null)
                            {
                                var DeclarationStatusPM = new DeclarationStatusPM();
                                DeclarationStatusPM.StatusID = statusCode.Id;
                                DeclarationStatusPM.UnfSequenceNumeric = declarationStatus.UnfSequenceNumeric;
                                DeclarationStatusPM.Tenant = tenant;
                                DeclarationStatusPM.DeclarationId = entityPoco.Id;
                                DeclarationStatusPM.StatusDate = declarationStatus.StatusDate;
                                DeclarationStatusPM.LineNumber = declarationStatus.LineNumber;
                                DeclarationStatusPM.StatusRemarks = declarationStatus.StatusRemarks;
                                DeclarationStatusPM.ChangeSetOp = ChangeSetOperation.Insert;
                                declarationStatusUpdateService.Update(DeclarationStatusPM, true);
                            }
                        }
                        if (!response.HasError)
                        {
                        }
                    }
                    else
                    {
                        response.HasError = true;
                        response.ErrorMessage = "Declaration doesn't exist!";
                    }
                    
                    scope.Complete();
                    return response;
                }
            }

            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);

                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                return response;
            }


        }

    }
}
