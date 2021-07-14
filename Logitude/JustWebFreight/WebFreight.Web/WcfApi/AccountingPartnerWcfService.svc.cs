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
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "AccountingPartnerWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select AccountingPartnerWcfService.svc or AccountingPartnerWcfService.svc.cs at the Solution Explorer and start debugging.
     [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class AccountingPartnerWcfService : IAccountingPartnerWcfService
    {
        public Response Upsert(AccountingPartnerPM entityPM, bool batch)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
                SecurityUtility.CheckContactFeature("AccountingPartner", "UPDATE", entityPM.Tenant);//UPDATE//READ
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    
                    ClassLevelValidator validationClass = new ClassLevelValidator("AccountingPartner", entityPM.Tenant) { IsHybrid = true };
                    if (!validationClass.IsValid(entityPM, entityPM, null))
                    {
                        response.HasError = true;
                        response.ErrorMessage = validationClass.GetErrorMessage(entityPM, null);
                        return response;
                    }
                   
                    ICommonDataContext objectContext = CommonDataContext.GetContext(entityPM.Tenant);

                    PaymentTermRepository paymentTermRepository = new PaymentTermRepository(entityPM.Tenant);
                    AccountingPartnerRepository AccountingPartnerRepository = new AccountingPartnerRepository(objectContext);
                    AccountingPartnerService service = new AccountingPartnerService(objectContext, entityPM.Tenant); 
                    UserRepository userReporistory = new UserRepository(objectContext);
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
                            response.ErrorMessage = "PrimaryContactId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                    if (entityPM.PaymentTermId != null)
                    {
                        var paymentTerm = paymentTermRepository.GetSinglePaymentTermByCode(entityPM.PaymentTermId, entityPM.Tenant);
                        if (paymentTerm != null)
                        {
                            entityPM.PaymentTermId = paymentTerm.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "PaymentTermId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }

                   response = MapCollectedIdField(entityPM, userReporistory);
                    if (response.HasError)
                    {
                        return response;
                    }

                    entityPM.IsHybrid = true;
                    AccountingPartner entity = AccountingPartnerRepository.GetSingleAccountingPartnerByCode(entityPM.Code, entityPM.Tenant);
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

        private Response MapCollectedIdField(AccountingPartnerPM entityPM, UserRepository userReporistory)
        {
            Response response = new Response();
            if (entityPM.CollectorId != null)
            {
                User user = userReporistory.GetSingleUserByCode(entityPM.CollectorId, entityPM.Tenant, true);
                if (user != null)
                {
                    entityPM.CollectorId = user.Id;
                    
                }
                else
                {
                    response = this.GetNotExistErrorMessage("CollectedId", response);
                }
            }

            return response;
        }
         

        private Response GetNotExistErrorMessage(string fieldName, Response response)
        {
            response.HasError = true;
            response.ErrorMessage = fieldName+" field doesn't exist in the database,Upsert this entity before using it.";
            return response; 
        }
     

        public AccountingPartnerPM GetAccountingPartnerPM(DataContracts.AccountingPartnerApiFilters filters, int tenant, ref Response response)
        { 
            try
            {
                AccountingPartnerPM entityPM = null;
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("AccountingPartner", "UPDATE", tenant);//UPDATE//READ
                if (CacheManager.CacheWrapper == null)
                {
                    CacheManager.CacheWrapper = new MockCacheWrapper();
                }

                ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);

                UserRepository userReporistory = new UserRepository(objectContext);
                CardRepository cardRepository = new CardRepository(objectContext);
                ContactRepository contactRepository = new ContactRepository(objectContext);
                CountryRepository countryRepository = new CountryRepository(objectContext);
                RankRepository rankRepository = new RankRepository(objectContext);
                PaymentTermRepository paymentTermRepository = new PaymentTermRepository(objectContext); 
                AccountingPartnerQuery query = new AccountingPartnerQuery(tenant);

                if (filters.ById)
                {
                    entityPM = query.GetSinglePMForHybrid(filters.SearchCode, tenant);
                }
                if (filters.ByCode)
                {
                    entityPM = query.GetSinglePMByCodeForHybrid(filters.SearchCode, tenant, false);
                }
                if (filters.ByVatNumber)
                {
                    entityPM = query.GetSinglePMByVatNumberForHybrid(filters.SearchCode, tenant, false);
                }

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
