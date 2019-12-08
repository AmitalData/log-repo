using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.Repositories;
using Logitude.Accounting.Def.EntityPMs;
using Logitude.BL.CommonDataModel.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers.ExternalAPIHelpers;
using WebFreight.Web.Security;
using Currency = Simplog.Data.CommonDataModel.EntityPOCOs.Currency;

namespace WebFreight.Web.ExternalAPIs.V1
{
    public class VendorPartnerController : ApiController
    {
        public HttpResponseMessage GetSingleVendor(string id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
				SecurityUtility.AuthenticateAPICall(authToken.Tenant);
				VendorQueryService Service = new VendorQueryService(tenant);
                ServiceResponse response = new ServiceResponse();
                var Result = Service.GetVendorById(id, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }

            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage Post(Logitude.BL.CommonDataModel.APIDataContract.ApiV1.Vendor entity)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;
                    SecurityUtility.AuthenticationOnTenant(tenant);
					SecurityUtility.AuthenticateAPICall(tenant);
                  

                    ContactInfo loggedContactInfo = SecurityUtility.GetContactInfo(authToken.Email, tenant);
                    string computingPartnerCode = "";
                    if (!string.IsNullOrEmpty(entity.ComputingPartnerCode))
                    {
                        computingPartnerCode = entity.ComputingPartnerCode;//loggedContactInfo.ComputingPartnerCode;
                    }

                    ICommonDataContext MyContext = CommonDataContext.GetContext(tenant);
                    CardRepository cardRepository = new CardRepository(MyContext);

                    if (!string.IsNullOrEmpty(entity.Code))
                    {
                        bool exist = (from a in cardRepository.GetCards(tenant)
                                      where a.PartnerTypeId == "VD" && a.Code == entity.Code && a.Tenant == tenant
                                      select a).Any();

                        if (exist)
                        {
                            throw new Exception("Vendor with code " + entity.Code + " already exists");
                        }
                    }

                    VendorQueryService mappingService = new VendorQueryService(tenant);
                    VendorPM entityPM = mappingService.VendorCustomDataMappingAndValidating(entity, tenant, computingPartnerCode);

                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        if (entityPM.Addresses.Count == 0)
                        {
                            throw new ApplicationException("Missing Main Address");
                        }

                        else
                        {
                            if (entity.MainAddress.Country == null)
                            {
                                throw new ApplicationException("Main address country is required");
                            }

                            if (entity.MainAddress.City == null)
                            {
                                throw new ApplicationException("Main address city is required");
                            }
                        }
                        if (!string.IsNullOrEmpty(entity.ComputingPartnerCode))
                        {
                            ComputingPartnerQuery computingPartnerQuery = new ComputingPartnerQuery(authToken.Tenant);
                            var partner = computingPartnerQuery.GetSinglePMByCodeAndCheckTenantZero(entity.ComputingPartnerCode, authToken.Tenant);
                            entityPM.CreatedByPartner = (partner != null ? partner.Name : null);

                        }
                        VendorService service = new VendorService(MyContext, tenant);
                        service.Create(entityPM);

                        #region GLAccount
                        if (entity.GLAccount != null)
                        {
                            FullAccountingHelper fullAccountingHelper = new FullAccountingHelper();                            
                            Simplog.Data.CommonDataModel.EntityPOCOs.Card card = cardRepository.GetSingleCard(entityPM.Id, tenant);
                            if (card != null)
                            {
                                GLAccountPM gLAccountEntity = new GLAccountPM();
                                gLAccountEntity.Tenant = tenant;
                                gLAccountEntity.PassedFromAPI = true;
                                gLAccountEntity.ChartOfAccountsTypeCode = "4";
                                gLAccountEntity.RevenueExpenseType = "3";
                                gLAccountEntity.AccountTypeCode = "3";
                                gLAccountEntity.ChangeSetOp = Simplog.Server.Infrastructure.ChangeSetOperation.Insert;
                                gLAccountEntity.InternalNumber = entity.GLAccount.InternalNumber;
                                //DisplayNumber
                                if (string.IsNullOrEmpty(entity.GLAccount.DisplayNumber))
                                {
                                    gLAccountEntity.DisplayNumber = card.Code;
                                }
                                else
                                {
                                    gLAccountEntity.DisplayNumber = entity.GLAccount.DisplayNumber;
                                }

                                //EnglishName
                                if (string.IsNullOrEmpty(entity.GLAccount.EnglishName))
                                {
                                    gLAccountEntity.EnglishName = card.EnglishName;
                                }
                                else
                                {
                                    gLAccountEntity.EnglishName = entity.GLAccount.EnglishName;
                                }

                                //LocalName
                                if (string.IsNullOrEmpty(entity.GLAccount.LocalName))
                                {
                                    gLAccountEntity.LocalName = card.LocalName;
                                }
                                else
                                {
                                    gLAccountEntity.LocalName = entity.GLAccount.LocalName;
                                }

                                //ChartOfAccount
                                if (entity.GLAccount.ChartOfAccount == null)
                                {
                                    throw new ApplicationException("GL Account Chart of Account is required");
                                }
                                else
                                {
                                    ChartOfAccountRepository chartOfAccountRepository = new ChartOfAccountRepository(tenant);
                                    ChartOfAccount chartOfAccount = new ChartOfAccount();

                                    if (!string.IsNullOrEmpty(entity.GLAccount.ChartOfAccount.Id))
                                    {
                                        chartOfAccount = chartOfAccountRepository.GetSingle(entity.GLAccount.ChartOfAccount.Id, tenant);
                                    }
                                    else
                                    {
                                        chartOfAccount = chartOfAccountRepository.GetSingleByCode(entity.GLAccount.ChartOfAccount.Code, tenant);
                                    }

                                    if (chartOfAccount == null)
                                    {
                                        throw new ApplicationException("ChartOfAccount doesn't exist");
                                    }

                                    else
                                    {
                                        gLAccountEntity.ChartOfAccountsId = chartOfAccount.Id;
                                    }
                                }

                                //Currency
                                if (entity.GLAccount.IsMultiCurrency == true)
                                {
                                    gLAccountEntity.IsMultiCurrency = true;
                                    gLAccountEntity.ReconcileMethodCode = "0";
                                }

                                else
                                {
                                    if (entity.GLAccount.Currency == null)
                                    {
                                        throw new ApplicationException("GL Account Currency is required");
                                    }
                                    else
                                    {
                                        CurrencyRepository currencyRepository = new CurrencyRepository(MyContext);
                                        Currency currency = currencyRepository.GetSingleCurrencyByCode(entity.GLAccount.Currency.Code, tenant);
                                        if (currency == null)
                                        {
                                            throw new ApplicationException("Currency with Code " + entity.GLAccount.Currency.Code + " doesn't exist");
                                        }

                                        else
                                        {
                                            gLAccountEntity.CurrencyId = currency.Id;
                                        }
                                    }

                                    if (entity.GLAccount.ReconcileMethod == null)
                                    {
                                        throw new ApplicationException("GL Account Reconcile Method is required");
                                    }
                                    else
                                    {
                                        ReconcileMethodRepository reconcileMethodRepository = new ReconcileMethodRepository(tenant);
                                        ReconcileMethod reconcileMethod = reconcileMethodRepository.GetSingle(entity.GLAccount.ReconcileMethod.Code);
                                        if (reconcileMethod == null)
                                        {
                                            throw new ApplicationException("reconcile Method with Code " + entity.GLAccount.ReconcileMethod.Code + " doesn't exist");
                                        }

                                        else
                                        {
                                            gLAccountEntity.ReconcileMethodCode = reconcileMethod.Code;
                                        }
                                    }
                                }

                                string glAccountId = fullAccountingHelper.CreateGLAccount(gLAccountEntity);
                                if (!string.IsNullOrEmpty(glAccountId))
                                {
                                    card.GLAccountId = glAccountId;
                                    cardRepository.Update(card);
                                    cardRepository.SubmitChanges();
                                }
                            }
                        }
                        #endregion

                        scope.Complete();
                    }

                    var result = mappingService.GetVendorById(entityPM.Id, tenant);
                    APIHelper.AddCommunicationLog("D", entity, result, "Vendor", entityPM.Id, "Vendor API", tenant);
                    return Request.CreateResponse(HttpStatusCode.OK, result);
                }

                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Vendor", null, "Vendor API");
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }
            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Vendor", null, "Vendor API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage Put(Logitude.BL.CommonDataModel.APIDataContract.ApiV1.Vendor entity)
        {
            var apiExceptionResult = ApiExceptionHandler.HandleException(new Exception("Updates are not supported"));
            APIHelper.AddCommunicationLog("F", entity, apiExceptionResult.Exception, "Vendor", null, "Vendor API");
            return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
        }
    }
}