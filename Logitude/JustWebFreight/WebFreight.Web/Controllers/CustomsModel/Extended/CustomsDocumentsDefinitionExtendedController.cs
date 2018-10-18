using Logitude.BL.Security;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.Data;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.Helpers;

namespace WebFreight.Web.Controllers.CustomsModel.Extended
{
    public class CustomsDocumentsDefinitionExtendedController: ApiController
    {
        public HttpResponseMessage Delete(string id)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        int tenant = authToken.Tenant;

                        ICustomContext MyContext = CustomContext.GetContext(tenant);
                        CustomsDocumentsDefinitionQueryService queryService = new CustomsDocumentsDefinitionQueryService(MyContext);
                        CustomsDocumentsDefinitionPM entityPM = queryService.GetSingle(id, false, false);
                        CustomsDocumentsDefinitionUpdateService service = new CustomsDocumentsDefinitionUpdateService(MyContext, new Dictionary<string, IContext>(), tenant);
                        entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                        service.Update(entityPM, true);

                        scope.Complete();
                        return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                    }
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildModelException(ModelState));
            }
        }


        public HttpResponseMessage GetCustomsDocumentsDefinitionsForDeclaration(string declarationId)
        {

            List<CustomsDocumentsDefinitionPM> listCustomsDocumentsDefinition = new List<CustomsDocumentsDefinitionPM>();
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    ICustomContext MyContext = CustomContext.GetContext(tenant);
                    var myDeclarationQueryService = new DeclarationQueryService(1);
                    var myDeclaration = myDeclarationQueryService.GetSingle(declarationId, true, false);
                    if (myDeclaration != null 
                        //&& 
                        )
                    {
                        string CargoTypeCode = null;
                        if (myDeclaration.Consignments != null && myDeclaration.Consignments.Count() > 0) {
                            CargoTypeCode = myDeclaration.Consignments[0].CargoTypeCode;
                        }
                        var myCustomsDocumentsDefinitionQueryService = new CustomsDocumentsDefinitionQueryService(tenant);
                        listCustomsDocumentsDefinition = myCustomsDocumentsDefinitionQueryService.GetCustomsDocumentsDefinitionsForDeclaration(CargoTypeCode, myDeclaration.ProcedureCurrentCode, myDeclaration.TransportModeId, 1);
                    }

                    
                    return Request.CreateResponse(HttpStatusCode.OK, listCustomsDocumentsDefinition);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }

        }

    }
}