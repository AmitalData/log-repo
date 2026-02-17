using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using WebFreight.Web.Security;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure;
using System.Web;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Net;
using System.Net.Http;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.Accounting.Data;
using Logitude.Accounting.BL.EntityUpdateServices;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.BL.Validators;

namespace WebFreight.Web.AccountingModel.Controllers
{
    public class Journ2alControllerClass
    {

        public static HttpResponseMessage JournalPost(JournalPM entityPM, HttpRequestMessage Request)
        {
            using (var scope = TransactionFactory.GetTransaction())
            {
                try
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                    SecurityUtility.CheckContactFeature("Journal", "NEW", authToken.Tenant);

                    IAccountingContext MyContext = AccountingContext.GetContext(entityPM.Tenant);
                    System.ComponentModel.DataAnnotations.ValidationContext validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(entityPM);
                    ValidationResult validationResult = JournalValidator.IsJournalValid(entityPM, validationContext);
                    if (validationResult == null)
                    {
                        JournalUpdateService service = new JournalUpdateService(MyContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                        entityPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;


                        service.Update(entityPM, true);

                        ObjectTableRepository objectTabelRepository = new ObjectTableRepository(entityPM.Tenant);
                        ObjectTable objectTable = objectTabelRepository.GetObjectTableByName("Journal", 0, true);
                        string email = HttpContext.Current.User.Identity.Name;
                        ContactRepository contactRepository = new ContactRepository(entityPM.Tenant);
                        Contact loggedContact = contactRepository.GetSingleContactByEmail(email, entityPM.Tenant);
                        if (loggedContact != null)
                        {
                            //ActivityLog.AddAcitivityLog(entityPM.Id, objectTable.Id, entityPM.Tenant, "U", loggedContact.Id);
                        }

                        scope.Complete();

                        return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                    }
                    else
                    {
                        string errorText = validationResult.ErrorMessage + ", Number=" + entityPM.ExternalNo;
                        throw new Exception(errorText);
                    }
                }

                catch (Exception ex)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }
            }
        }
    }
}