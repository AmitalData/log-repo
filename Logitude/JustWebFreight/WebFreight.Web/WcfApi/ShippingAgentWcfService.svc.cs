using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Transactions;
using WebFreight.Web.Security;
using Logitude.BL.Validators;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.EntityService;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "ShippingAgentWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select ShippingAgentWcfService.svc or ShippingAgentWcfService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ShippingAgentWcfService : IShippingAgentWcfService
    {
        public Response Upsert(ShippingAgentPM entityPM, bool batch)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            Response response = new Response();
            try
            {
                SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
                SecurityUtility.CheckContactFeature("ShippingAgent", "UPDATE", entityPM.Tenant);//UPDATE//READ
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                   
                    ClassLevelValidator validationClass = new ClassLevelValidator("ShippingAgent", entityPM.Tenant) { IsHybrid = true };
                    if (!validationClass.IsValid(entityPM, entityPM, null))
                    {
                        response.HasError = true;
                        response.ErrorMessage = validationClass.GetErrorMessage(entityPM, null);
                        return response;
                    }

                    ICommonDataContext commoncontext = CommonDataContext.GetContext(entityPM.Tenant);

                    ShippingAgentRepository shippingAgentRepository = new ShippingAgentRepository(commoncontext);
                    ShippingAgentService service = new ShippingAgentService(commoncontext, entityPM.Tenant);
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
                    ShippingAgent entity = shippingAgentRepository.GetSingleShippingAgentByCode(entityPM.Code, entityPM.Tenant);
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
    }
}
