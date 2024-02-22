using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.Validators;
using System.Transactions;
using WebFreight.Web.Security;
using Logitude.Server.Tools;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Accounting.BL.Utils;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AirlineWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select AirlineWcfService.svc or AirlineWcfService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AirlineWcfService : IAirlineWcfService
    {
        public Response Upsert(AirlinePM entityPM, bool batch)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
                SecurityUtility.CheckContactFeature("Airline", "UPDATE", entityPM.Tenant);//UPDATE//READ
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                   
                    ClassLevelValidator validationClass = new ClassLevelValidator("Airline", entityPM.Tenant) { IsHybrid = true };
                    if (!validationClass.IsValid(entityPM, entityPM, null))
                    {
                        response.HasError = true;
                        response.ErrorMessage = validationClass.GetErrorMessage(entityPM, null);
                        return response;
                    }

                  

                    ICommonDataContext objectContext = CommonDataContext.GetContext(entityPM.Tenant);

                    AirlineRepository AirlineRepository = new AirlineRepository(objectContext);
                    AirlineService service = new AirlineService(objectContext, entityPM.Tenant);

                    if (entityPM.PrimaryContactId != null)
                    {
                        ContactRepository contactRepository = new ContactRepository(objectContext);
                        Contact contact = contactRepository.GetSingleContactByExternalId(entityPM.PrimaryContactId, entityPM.Tenant);
                        if (contact != null)
                        {
                            entityPM.PrimaryContactId = contact.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "InvoiceCurrencyId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                    entityPM.IsHybrid = true;
                    Airline entity = AirlineRepository.GetSingleAirlineByCode(entityPM.Code, entityPM.Tenant);
                    if (entity == null)
                    {
                        service.Create(entityPM);
                    }
                    else
                    {
                        entityPM.Id = entity.Id;
                        service.SetChangeSet(entityPM.CardExternalCodeByCurrencies);
                        service.Update(entityPM);
                    }
					CardGLAccountConnectBatch CardGLAccountConnectBatch = new CardGLAccountConnectBatch();
					CardGLAccountConnectBatch.ConnectSingleCardToGLAccountInBatch(entityPM.Tenant, entityPM.Id);
					response.Result = entityPM.Id;
                    scope.Complete();

                    return response;
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException e)
            {
                string Error = "";
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);

                        Error += "- Property:" + ve.PropertyName + ", Error:" + ve.ErrorMessage + Environment.NewLine;
                    }
                }
                response.HasError = true;
                response.ErrorMessage = Error;

                return response;
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


        public AirlinePM GetAirlinePM(string code, int tenant, ref Response response)
        {

            try
            {
                AirlinePM entityPM = null;
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Airline", "READ", tenant);//UPDATE//READ
                if (CacheManager.CacheWrapper == null)
                {
                    CacheManager.CacheWrapper = new MockCacheWrapper();
                }

                ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);

                ContactRepository contactRepository = new ContactRepository(objectContext);
                PaymentTermRepository paymentTermRepository = new PaymentTermRepository(objectContext);
                AirlineQuery query = new AirlineQuery(tenant);

                entityPM = query.GetSinglePMByCode(code, tenant);

                if (entityPM != null)
                {

                    if (entityPM.PrimaryContactId != null)
                    {
                        Contact contact = contactRepository.GetSingleContact(entityPM.PrimaryContactId, entityPM.Tenant);
                        if (contact != null && !string.IsNullOrEmpty(contact.ExternalId))
                        {
                            entityPM.PrimaryContactId = contact.ExternalId;
                            entityPM.PrimaryContactEmail = contact.Email;
                            entityPM.PrimaryContactName = contact.EnglishName;
                            entityPM.PrimaryContactPhone = contact.BusinessPhone;

                        }
                    }



                    if (entityPM.PaymentTermId != null)
                    {
                        PaymentTerm paymentTerm = paymentTermRepository.GetSinglePaymentTerm(entityPM.PaymentTermId, entityPM.Tenant);
                        if (paymentTerm != null)
                        {
                            entityPM.PaymentTermId = paymentTerm.Code;

                        }
                    }


                }

                return entityPM;
            }
            catch (Exception ex)
            {
                response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
                response.HasError = true;
                response.ErrorMessage = ex.Message;
                response.InnerErrorMessage = ex.InnerException != null ? ex.InnerException.Message : null;
                if (!string.IsNullOrEmpty(ex.StackTrace))
                {
                    response.ErrorMessage += Environment.NewLine + ex.StackTrace;
                }

                return null;

            }
        }
    }
}
