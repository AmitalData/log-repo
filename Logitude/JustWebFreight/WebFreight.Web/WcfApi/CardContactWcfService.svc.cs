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

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CardContactWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CardContactWcfService.svc or CardContactWcfService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CardContactWcfService : ICardContactWcfService
    {
        public Response Upsert(CardContactPM entityPM, bool batch)
        {

            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {

                    ClassLevelValidator validationClass = new ClassLevelValidator("CardContact", entityPM.Tenant) { IsHybrid = true };
                    if (!validationClass.IsValid(entityPM, entityPM, null))
                    {
                        response.HasError = true;
                        response.ErrorMessage = validationClass.GetErrorMessage(entityPM, null);
                        return response;
                    }
                    ICommonDataContext objectContext = CommonDataContext.GetContext(entityPM.Tenant);
                    ContactRepository ContactRepository = new ContactRepository(objectContext);
                    CardRepository cardRepository = new CardRepository(objectContext);
                    CardContactRepository cardContactRepository = new CardContactRepository(objectContext);
                    CardContactService service = new CardContactService(objectContext, entityPM.Tenant);

                    entityPM.IsHybrid = true;

                    if (entityPM.IsAll)
                    {
                        if (entityPM.IsAirExport || entityPM.IsAirImport || entityPM.IsInlandExport || entityPM.IsInlandImport || entityPM.IsOceanExport || entityPM.IsOceanImport)
                        {
                            response.HasError = true;
                            response.ErrorMessage = "The field IsAll must be false to enable other product Types!";
                            return response;
                        }
                    }
                    Contact contact = ContactRepository.GetSingleContactByExternalId(entityPM.ContactId, entityPM.Tenant);
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
                    if (contact != null)
                    {
                        entityPM.ContactId = contact.Id;
                    }
                    else
                    {
                        response.HasError = true;
                        response.ErrorMessage = "ContactId field doesn't exist in the database,Upsert this entity before using it.";
                        return response;
                    }


                    CardContact entity = cardContactRepository.GetSingleCardContact(contact.Id, card.Id, entityPM.Tenant);

                    foreach (CardContactProductPM product in entityPM.CardContactProducts)
                    {
                        if (!string.IsNullOrEmpty(product.ProductTypeCode))
                        {
                            product.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;

                            ProductTypeRepository productTypeRepository = new ProductTypeRepository(objectContext);
                            ProductType type = productTypeRepository.GetSingleProductType(product.ProductTypeCode, entityPM.Tenant);
                            if (type == null)
                            {

                                response.HasError = true;
                                response.ErrorMessage = "ProductTypeCode field doesn't exist in the database,Upsert this entity before using it.";
                                return response;
                            }

                        }
                        else
                        {
                            response.HasError = true;
                            response.ErrorMessage = "ProductTypeCode field is required";
                            return response;
                        }
                    }

                    if (entity == null)
                    {
                        service.SetChangeSet(entityPM.CardContactProducts);
                        service.Create(entityPM);
                    }
                    else
                    {

                        CardContactProductQuery productTypeQuery = new CardContactProductQuery(new CardContactProductRepository(objectContext));
                        List<CardContactProductPM> oldProducts = productTypeQuery.GetCardContactProductPMsByCardContactId(entity.Id, entity.Tenant);
                        foreach (CardContactProductPM product in oldProducts)
                        {
                            product.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Delete;
                            entityPM.CardContactProducts.Add(product);
                        }

                        service.SetChangeSet(entityPM.CardContactProducts);

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


        public Response Delete(string contactExternalId, string cardCode, int tenant, bool batch)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }
            
            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
        
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                   
                    ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);
                    CardContactRepository cardContactRepository = new CardContactRepository(objectContext);

                    CardContact entity = cardContactRepository.GetSingleCardContactByExternal(contactExternalId, cardCode, tenant);

                    if (entity != null)
                    {
                        cardContactRepository.Remove(entity);
                        cardContactRepository.SubmitChanges();
                    }
                    else
                    {
                        response.HasError = true;
                        response.ErrorMessage = "CardContact  doesn't exist in the database";
                        return response;
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


        public CardContactPM GetCardContactPM(string contactExternalId, string cardCode, int tenant, ref Response response)
        {
            
         

            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(tenant);
                 
                     
                    ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);
                    ContactRepository ContactRepository = new ContactRepository(objectContext);
                    CardRepository cardRepository = new CardRepository(objectContext);
                    CardContactRepository cardContactRepository = new CardContactRepository(objectContext);
                   // CardContactService service = new CardContactService(objectContext, tenant);



                    //Contact contact = ContactRepository.GetSingleContactByExternalId(contactExternalId, tenant);
                    //Card card = cardRepository.GetSingleCardByCode(cardCode, tenant, false);

                    CardContactQuery cardContactQuery = new CardContactQuery(cardContactRepository);
                    var entity = cardContactQuery.GetSinglePMByCardCodeContactExternalId(contactExternalId, cardCode, tenant);

                   
                    //CardContact entity = cardContactRepository.GetSingleCardContact(contact.Id, card.Id, tenant);

                   

                    //if (entity == null)
                    //{
                    //    service.Create(entityPM);
                    //}
                    //else
                    //{
                    //    entityPM.Id = entity.Id;
                    //    service.Update(entityPM);

                    //}

                   return entity;
                  
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
                return null;
            }
        }
    }
}
