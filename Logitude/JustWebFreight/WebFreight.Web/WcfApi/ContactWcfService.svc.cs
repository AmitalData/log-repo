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
using Logitude.BL.CommonDataModel.EntityLists;
using Microsoft.TeamFoundation.Common;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ContactWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ContactWcfService.svc or ContactWcfService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ContactWcfService : IContactWcfService
    {
        public Response Upsert(ContactPM entityPM, bool batch)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
                SecurityUtility.CheckContactFeature("Contact", "UPDATE", entityPM.Tenant);//UPDATE//READ
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {

                    ClassLevelValidator validationClass = new ClassLevelValidator("Contact", entityPM.Tenant) { IsHybrid = true };
                    if (!validationClass.IsValid(entityPM, entityPM, null))
                    {
                        response.HasError = true;
                        response.ErrorMessage = validationClass.GetErrorMessage(entityPM, null);
                        return response;
                    }

                    ICommonDataContext objectContext = CommonDataContext.GetContext(entityPM.Tenant);
                    ContactRepository ContactRepository = new ContactRepository(objectContext);
                    CardRepository cardRepository = new CardRepository(objectContext);
                    GlobalContactRepository globalContactsRepository=new GlobalContactRepository();
                    ContactService service = new ContactService(objectContext, entityPM.Tenant);

                    entityPM.IsHybrid = true;
                    entityPM.DontShowLocal = true;
                    if (string.IsNullOrEmpty(entityPM.ExternalId))
                    {
                        response.HasError = true;
                        response.ErrorMessage = "ExternalId field is required";
                        return response;
                    }
                    if (entityPM.Tenant != 0)
                    {
                        Contact contact = ContactRepository.GetSingleContactByEmailSpecificTenant(entityPM.Email, 0);
                        GlobalContact globalContact = globalContactsRepository.GetGlobalContactByEmailAndTenant(entityPM.Email, 0);
                         if (contact != null)
                        {
                            if (!contact.InActive)
                            {
                                response.HasError = true;
                                response.ErrorMessage = "Email field exist in the database Contact in tenant 0.";
                                return response;
                            }
                        }
                        if (globalContact != null)
                        {
                            if (!globalContact.InActive)
                            {
                                response.HasError = true;
                                response.ErrorMessage = "Email field exist in the database GlobalContact in tenant 0.";
                                return response;
                            }
                        }
                    }
                    if (entityPM.CardId != null)
                    {
                        Card card = cardRepository.GetSingleCardByCode(entityPM.CardId, entityPM.Tenant, false);
                        if (card != null)
                        {
                            entityPM.CardId = card.Id;
                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "CardId field doesn't exist in the database,Upsert this entity before using it.";
                            return response;
                        }
                    }


                    Contact entity = ContactRepository.GetSingleContactByExternalId(entityPM.ExternalId, entityPM.Tenant);

                    if (entity == null && !string.IsNullOrEmpty(entityPM.Email))
                    {
                        entity = ContactRepository.GetSingleContactByEmail(entityPM.Email, entityPM.Tenant);
                    }

                    if (entity == null)
                    {
                        service.Create(entityPM);
                    }
                    else
                    {
                        entityPM.Id = entity.Id;
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



        public ContactPM GetContactPMByEmail(string email, int tenant, ref Response response)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Contact", "READ", tenant);//UPDATE//READ

                if (!string.IsNullOrEmpty(email))
                {
                    ContactQuery contactQuery = new ContactQuery(tenant);

                    return contactQuery.GetSingleContact(email, tenant);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                response = new Response();
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

        public ContactPM GetContactByExternalId(string externalId, int tenant, ref Response response)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }
            try
            {

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Contact", "READ", tenant);//UPDATE//READ

                ContactQuery contactQuery = new ContactQuery(tenant);
                return contactQuery.GetSingleContactByExternalId(externalId, tenant);
            }
            catch (Exception ex)
            {
                response = new Response();
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



        //this method can be used to get list by tenant or by customer and tenant.
        public List<ContactList> GetContactList(DataContracts.ContactApiFilters filters, int tenant, ref Response response)
        {
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("Contact", "READ", tenant);//UPDATE//READ
                if (CacheManager.CacheWrapper == null)
                {
                    CacheManager.CacheWrapper = new MockCacheWrapper();
                }

                IQueryable<ContactList> contacts = null;
                ICommonDataContext commoncontext = CommonDataContext.GetContext(tenant);
                if (filters.ByCustomer)
                {

                    ContactQuery contactQuery = new ContactQuery(new ContactRepository(commoncontext));
                    CustomerPM pm = null;
                    CustomerQuery query = new CustomerQuery(tenant);

                    if (filters.ById)
                    {
                        pm = query.GetSinglePMForHybrid(filters.SearchCode, tenant);
                    }
                    if (filters.ByCode)
                    {
                        pm = query.GetSinglePMByCodeForHybrid(filters.SearchCode, tenant, false);
                    }
                    if (filters.ByVatNumber)
                    {
                        pm = query.GetSinglePMByVatNumberForHybrid(filters.SearchCode, tenant, false);
                    }

                    if (pm != null)
                    {
                        contacts = contactQuery.GetContactListsbyCardId(pm.Id, tenant);
                    }
                }
                else
                {
                    ContactQuery contactQuery = new ContactQuery(tenant);

                    contacts = contactQuery.GetContactLists(tenant);
                }

                if (!string.IsNullOrEmpty(filters.SearchFields))
                {
                    contacts = contacts.Where(c => c.SearchFields.Contains(filters.SearchFields));
                }

                CardContactRepository cardContactRepository = new CardContactRepository(commoncontext);
                List<ContactList> contactLists = contacts.OrderBy(d => d.Id).Skip(filters.Skip).Take(filters.Take).ToList();
                foreach (ContactList contact in contactLists)
                {
                    List<Card> cards = cardContactRepository.GetCardContacts(tenant).Where(d => d.ContactId == contact.Id).Select(r => r.Card).ToList();
                    string str = String.Empty;
                    foreach (Card card in cards)
                    {
                        str = str + card.EnglishName + "; ";

                    }
                    if (!string.IsNullOrEmpty(str) && str.Length >= 2)
                    {
                        contact.Company = str.Remove(str.Length - 2);
                    }

                }


                return contactLists;
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
