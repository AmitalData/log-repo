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
using Intuit.Ipp.Core;
using Logitude.Server.Tools;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.CommonDataModel.EntityQueries;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AgentHypredService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select AgentHypredService.svc or AgentHypredService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AgentWcfService : IAgentWcfService
    {

        public Response Upsert(AgentPM entityPM, bool batch)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
                SecurityUtility.CheckContactFeature("Agent", "UPDATE", entityPM.Tenant);//UPDATE//READ
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {


                    ClassLevelValidator validationClass = new ClassLevelValidator("Agent", entityPM.Tenant) { IsHybrid = true };
                    if (!validationClass.IsValid(entityPM, entityPM, null))
                    {
                        response.HasError = true;
                        response.ErrorMessage = validationClass.GetErrorMessage(entityPM, null);
                        return response;
                    }

                    ICommonDataContext commoncontext = CommonDataContext.GetContext(entityPM.Tenant);

                    AgentRepository AgentRepository = new AgentRepository(commoncontext);
                    AgentService service = new AgentService(commoncontext, entityPM.Tenant);
                    CurrencyRepository currencyRepository = new CurrencyRepository(commoncontext);
                    VatTypeRepository vatTypeRepository = new VatTypeRepository(commoncontext);

                    if (entityPM.InvoiceCurrencyId != null)
                    {
                        Currency currency = currencyRepository.GetSingleCurrencyByCode(entityPM.InvoiceCurrencyId, entityPM.Tenant);
                        if (currency != null)
                        {
                            entityPM.InvoiceCurrencyId = currency.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "InvoiceCurrencyId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                    if (entityPM.VatTypeId != null)
                    {
                        VatType vattype = vatTypeRepository.GetSingleVatTypeByCode(entityPM.VatTypeId, entityPM.Tenant);
                        if (vattype != null)
                        {
                            entityPM.VatTypeId = vattype.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "VatTypeId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                    string paymentTermCode = entityPM.PaymentTermId;
                    entityPM.PaymentTermId = GetPaymentTermId(entityPM, commoncontext);
                    if (!string.IsNullOrEmpty(paymentTermCode) && string.IsNullOrEmpty(entityPM.PaymentTermId))
                    {
                        response.HasError = true;
                        response.ErrorMessage = "PaymentTermId field doesn't exist in the database,Upsert this entity before using it.";
                        return response;
                    }
                    
                    if (entityPM.PrimaryContactId != null)
                    {
                        ContactRepository contactRepository = new ContactRepository(commoncontext);
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
                    Agent entity = AgentRepository.GetSingleAgentByCode(entityPM.Code, entityPM.Tenant);
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

        private string GetPaymentTermId(AgentPM entityPM, ICommonDataContext commoncontext)
        {
            if (string.IsNullOrEmpty(entityPM.PaymentTermId)) return null;
            PaymentTermRepository paymentTermRepository = new PaymentTermRepository(commoncontext);
            PaymentTerm paymentTerm = paymentTermRepository.GetSinglePaymentTermByCode(entityPM.PaymentTermId, entityPM.Tenant);
            if (paymentTerm == null) return null;
            return paymentTerm.Id;
        }

        public AgentPM GetAgentPM(string code, int tenant, ref Response response)
        {

            try
            {
                AgentPM entityPM = null;
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Agent", "READ", tenant);//UPDATE//READ
                if (CacheManager.CacheWrapper == null)
                {
                    CacheManager.CacheWrapper = new MockCacheWrapper();
                }

                ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);

                ContactRepository contactRepository = new ContactRepository(objectContext);
                PaymentTermRepository paymentTermRepository = new PaymentTermRepository(objectContext);
                AgentQuery query = new AgentQuery(tenant);

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
