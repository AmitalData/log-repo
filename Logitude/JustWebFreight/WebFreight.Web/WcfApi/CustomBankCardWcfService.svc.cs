using Logitude.BL.Validators;
using Logitude.Customs.Def.EntityPMs;
using Logitude.Customs.BL.EntityQueryServices;
using Logitude.Customs.BL.EntityUpdateServices;
using Logitude.Customs.BL.Validators;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.Repsitories;
using Logitude.Server.Tools;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure;
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

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "CustomBankCardWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select CustomBankCardWcfService.svc or CustomBankCardWcfService.svc.cs at the Solution Explorer and start debugging.
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CustomBankCardWcfService : ICustomBankCardWcfService
    {
        public Response Upsert(CustomBankPM entityPM, bool batch)
        {
            if (CacheManager.CacheWrapper == null)
            {
                CacheManager.CacheWrapper = new MockCacheWrapper();
            }

            Response response = new Response();

            try
            {
                SecurityUtility.AuthenticationOnTenant(entityPM.Tenant);
                SecurityUtility.CheckContactFeature("Customs.CustomBank", "UPDATE", entityPM.Tenant);
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {

                    CustomsClassLevelValidator validationClass = new CustomsClassLevelValidator("CustomBank", entityPM.Tenant);
                    if (!validationClass.IsValid(entityPM, entityPM, null))
                    {
                        response.HasError = true;
                        response.ErrorMessage = validationClass.GetErrorMessage(entityPM, null);
                        return response;
                    }

                    ICustomContext objectContext = CustomContext.GetContext(entityPM.Tenant);

                    CustomBankQueryService customBanksCardQueryService = new CustomBankQueryService(objectContext);
                    CustomBankUpdateService service = new CustomBankUpdateService(objectContext, new Dictionary<string, IContext>(), entityPM.Tenant);
                    CardRepository cardRep = new CardRepository(entityPM.Tenant);

                    CustomBankPM bankPM = customBanksCardQueryService.GetCustomBankByInternalCode(entityPM.InternalCode, entityPM.Tenant);
                    if (bankPM != null)
                    {
                        if (!string.IsNullOrEmpty(entityPM.CardId))
                        {
                            Card card = cardRep.GetSingleCardByCode(entityPM.CardId, entityPM.Tenant, false);
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

                        bankPM.AccountNumber = entityPM.AccountNumber;
                        bankPM.BankAddress = entityPM.BankAddress;
                        bankPM.InActive = entityPM.InActive;
                        bankPM.BankCode = entityPM.BankCode;
                        bankPM.BranchCode = entityPM.BranchCode;
                        bankPM.EnglishName = entityPM.EnglishName;
                        bankPM.InternalCode = entityPM.InternalCode;
                        bankPM.LocalName = entityPM.LocalName;
                        bankPM.PayerTypeCode = entityPM.PayerTypeCode;
                        bankPM.Tenant = entityPM.Tenant;
                        bankPM.CardId = entityPM.CardId;
                        bankPM.ChangeSetOp = ChangeSetOperation.Update;

                        List<string> cardIds = (from a in bankPM.CustomBanksCards
                                                select a.CardId).ToList();
                        foreach (CustomBanksCardPM item in entityPM.CustomBanksCards)
                        {
                            if (!string.IsNullOrEmpty(item.CardId))
                            {
                                Card card = cardRep.GetSingleCardByCode(item.CardId, entityPM.Tenant, false);
                                if (card != null)
                                {
                                    item.CardId = card.Id;
                                }
                                else
                                {
                                    response.HasError = true;
                                    response.ErrorMessage = "CardId field in CustomBankCard doesn't exist in the database,Upsert this entity before using it.";
                                    return response;
                                }
                            }

                            if (!cardIds.Contains(item.CardId))
                            {
                                item.CustomBankId = bankPM.Id;
                                item.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                                bankPM.CustomBanksCards.Add(item);
                            }
                        }

                        bankPM.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Update;
                    }

                    else
                    {
                        if (!string.IsNullOrEmpty(entityPM.CardId))
                        {
                            Card card = cardRep.GetSingleCardByCode(entityPM.CardId, entityPM.Tenant, false);
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

                        bankPM = new CustomBankPM()
                        {
                            AccountNumber = entityPM.AccountNumber,
                            BankAddress = entityPM.BankAddress,
                            InActive = entityPM.InActive,
                            BankCode = entityPM.BankCode,
                            BranchCode = entityPM.BranchCode,
                            EnglishName = entityPM.EnglishName,
                            InternalCode = entityPM.InternalCode,
                            LocalName = entityPM.LocalName,
                            PayerTypeCode = entityPM.PayerTypeCode,
                            Tenant = entityPM.Tenant,
                            CardId = entityPM.CardId,
                            ChangeSetOp = ChangeSetOperation.Insert,
                        };



                        foreach (CustomBanksCardPM item in entityPM.CustomBanksCards)
                        {
                            if (!string.IsNullOrEmpty(item.CardId))
                            {
                                Card card = cardRep.GetSingleCardByCode(item.CardId, entityPM.Tenant, false);
                                if (card != null)
                                {
                                    item.CardId = card.Id;
                                }
                                else
                                {
                                    response.HasError = true;
                                    response.ErrorMessage = "CardId field in CustomBankCard doesn't exist in the database,Upsert this entity before using it.";
                                    return response;
                                }
                            }

                            item.CustomBankId = entityPM.Id;
                            item.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                            bankPM.CustomBanksCards.Add(item);

                        }
                    }


                    service.Update(bankPM, true);

                    scope.Complete();
                    response.Result = bankPM.Id;
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

        public Response Delete(string bankId, string cardId, int tenant)
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
                    CustomBanksCardRepository repo = new CustomBanksCardRepository(tenant);

                    CustomBanksCard entity = repo.GetSingleCustomBanksCard(bankId, cardId, tenant);

                    if (entity != null)
                    {
                        repo.Remove(entity);
                        repo.SubmitChanges();
                    }
                    else
                    {
                        response.HasError = true;
                        response.ErrorMessage = "Custom Bank Card doesn't exist in the database";
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

    }
}
