using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InvoiceModel.APIDataContract.ApiV1;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InvoiceModel;
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

namespace WebFreight.Web.ExternalAPIs.V1
{
    public class ARInvoiceController: ApiController
    {

        public HttpResponseMessage GetSingleARInvoice(string id, string number)
        {
            try
            {

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
				SecurityUtility.AuthenticateAPICall(authToken.Tenant);
 

                ARInvoiceQueryService Service = new ARInvoiceQueryService(tenant);
                ServiceResponse response = new ServiceResponse();
                var Result = new ARInvoice();
                if (!string.IsNullOrEmpty(id))
                {
                    Result = Service.GetARInvoiceById(id, tenant);
                }
                else if (!string.IsNullOrEmpty(number))
                {
                    Result = Service.GetARInvoiceByInvoiceNumber(number, tenant);
                }

                string xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(Result);
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage Post(ARInvoice entity)
        {
            ARInvoice oldEntity = entity;

            if (ModelState.IsValid)
            {
                try
                {

                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        int tenant = entity.Tenant;

						SecurityUtility.AuthenticateAPICall(authToken.Tenant);

						if (entity != null)
                        {
                            oldEntity = LogitudeXmlSerializer.DeserializeObject<ARInvoice>(LogitudeXmlSerializer.SerializeObjectToXmlString(entity));
                        }

                        IInvoiceContext MyContext = InvoiceContext.GetContext(entity.Tenant);
                        ARInvoiceQueryService mappingService = new ARInvoiceQueryService(entity.Tenant);
                        ARInvoicePM entityPM = mappingService.ARInvoiceDataMappingAndValidatin(entity, entity.Tenant);
                        mappingService.SetInvoiceLinesEntityId(entityPM, entity.Tenant);
                        mappingService.ValidateAccountingExternalEntityId(entity);
                        mappingService.SetBillToGLAccountId(entityPM);
                        entityPM.IsExternalEntity = true;
                        entityPM.IsExternalAPI = true;
                        entityPM.Tenant = entity.Tenant;
                        entityPM.IsGeneralInvoice = true;
                        if (entity.IsDraft)
                        {
                            entityPM.SetApproved = false;
                        }
                        else entityPM.SetApproved = true;


                        if(entityPM.StatusCode == "AD")
                        {
                            entityPM.SetApproved = true;
                        }

                        #region Computing Invoice Lines Fields
                        ICommonDataContext CommonContext = CommonDataContext.GetContext(tenant);
                        Tenant MyTenant = (from d in CommonContext.Tenants where d.Id == tenant select d).FirstOrDefault();
                        RatesTableQuery MyRatesTableQuery = new RatesTableQuery(tenant);
                        string LocalCurrencyId = MyTenant.CurrencyId;
                        DateTime todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;

                        VatTypeRepository vatTypeRepository = new VatTypeRepository(CommonContext);
                        VatTypePercentageRepository vatTypePercentageRepository = new VatTypePercentageRepository(CommonContext);
                        VatTypePercentageQuery myVatTypePercentageQuery = new VatTypePercentageQuery(vatTypePercentageRepository);
                        List<VatType> allVatTypes = vatTypeRepository.GetVatTypes(tenant).ToList();
                        List<VatTypePercentagePM> allVatPercentages = myVatTypePercentageQuery.GetVatTypePercentagePMByDate(tenant, todayDate);

                        foreach (ARInvoiceLinePM itemPM in entityPM.InvoiceLines)
                        {
                            if (itemPM.ForiegnCurrencyId != null)
                            {
                                if (itemPM.ForiegnExchangeRate == null)
                                {
                                    if (itemPM.ForiegnCurrencyId == LocalCurrencyId)
                                    {
                                        itemPM.ForiegnExchangeRate = 1;
                                    }

                                    else
                                    {
                                        LastRate lastRate = MyRatesTableQuery.GetLastRecordByValueDate(tenant, itemPM.ForiegnCurrencyId, LocalCurrencyId, todayDate);
                                        if (lastRate != null)
                                        {
                                            itemPM.ForiegnExchangeRate = MethodHelper.Round(lastRate.Rate, 5);
                                        }
                                    }
                                }
                            }

                            if (itemPM.VatTypeId != null)
                            {
                                if (itemPM.VatPercentage == null)
                                {
                                    VatType MyVatType = allVatTypes.Where(d => d.Id == itemPM.VatTypeId).FirstOrDefault();
                                    if (MyVatType != null)
                                    {
                                        if (!MyVatType.IsMultiPercentage)
                                        {
                                            VatTypePercentagePM myPercentagePM = allVatPercentages.Where(d => d.VatTypeId == itemPM.VatTypeId).FirstOrDefault();
                                            if (myPercentagePM != null)
                                            {
                                                itemPM.VatPercentage = myPercentagePM.Percentage;
                                            }
                                        }
                                    }
                                }
                            }

                            if (itemPM.LocalCurrencyAmount == null)
                            {
                                if (itemPM.ForiegnCurrencyAmount != null && itemPM.ForiegnExchangeRate != null)
                                {
                                    itemPM.LocalCurrencyAmount = MethodHelper.Round(itemPM.ForiegnCurrencyAmount * itemPM.ForiegnExchangeRate, 2);
                                }
                            }
                        }
                        #endregion

                        #region Computing Invoice Number
                        if (entityPM.InvoiceNumber == null)
                        {
                            if (!entityPM.IsInvoiceNumberManuallySet)
                            {                               
                                if (entityPM.IsConstituentInvoice)
                                {
                                    entityPM.InvoiceNumber = TableCounter.GetNumber(tenant, "CNST", "CNS", null);
                                }

                                else if (entityPM.IsConsolidationInvoice)
                                {
                                    entityPM.InvoiceNumber = TableCounter.GetNumber(tenant, "INVC", "CON", null);
                                }

                                else
                                {
                                    entityPM.InvoiceNumber = TableCounter.GetNumber(tenant, "INVC", entityPM.ARInvoiceTypeCode, null);
                                }
                            }
                        }
                        #endregion

                        ARInvoiceService service = new ARInvoiceService(MyContext, entity.Tenant);

                        ARInvoicePM invoice = mappingService.UpdateCreditInvoice(entityPM, tenant);
                        if (!string.IsNullOrEmpty(entity.ComputingPartnerCode))
                        {
                            ComputingPartnerQuery computingPartnerQuery = new ComputingPartnerQuery(tenant);
                            var partner = computingPartnerQuery.GetSinglePMByCodeAndCheckTenantZero(entity.ComputingPartnerCode, tenant);
                            entityPM.CreatedByPartner = (partner != null ? partner.Name : null);

                        }
                        service.Create(entityPM);
                        if (invoice != null)
                        {
                            service.Update(invoice, true);
                        }

                        entity = mappingService.ARInvoiceDataMappingAndValidatin(entityPM, entity.Tenant);
                        APIHelper.AddCommunicationLog("D", oldEntity, entity, "ARInvoice", entityPM.Id, "ARInvoice API", entity.Tenant);

                        scope.Complete();


                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }

                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    APIHelper.AddCommunicationLog("F", oldEntity, apiExceptionResult.Exception, "ARInvoice", null, "ARInvoice API");
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }

            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", oldEntity, apiExceptionResult.Exception, "ARInvoice", null, "ARInvoice API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

        public HttpResponseMessage Put(ARInvoice entity)
        {
            ARInvoice oldEntity = entity;

            if (ModelState.IsValid)
            {
                try
                {
                    using (TransactionScope scope = TransactionFactory.GetTransaction())
                    {
                        
                        string token = HttpContext.Current.Request.Headers["Token"];
                        AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                        SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                        int tenant = authToken.Tenant;
						SecurityUtility.AuthenticateAPICall(authToken.Tenant);
						if (entity != null)
                        {
                            oldEntity = LogitudeXmlSerializer.DeserializeObject<ARInvoice>(LogitudeXmlSerializer.SerializeObjectToXmlString(entity));
                        }
                       
                        IInvoiceContext MyContext = InvoiceContext.GetContext(tenant);
                        ARInvoiceQueryService mappingService = new ARInvoiceQueryService(tenant);
                        ARInvoicePM entityPM = mappingService.ARInvoiceDataMappingAndValidatin(entity, tenant);
                        mappingService.SetBillToGLAccountId(entityPM);
                        //  mappingService.UpdateCreditInvoice(entityPM, tenant);
                        entityPM.IsExternalAPI = true;
                        entityPM.IsExternalEntity = true;
                        if (!string.IsNullOrEmpty(entity.ComputingPartnerCode))
                        {
                            ComputingPartnerQuery computingPartnerQuery = new ComputingPartnerQuery(tenant);
                            var partner = computingPartnerQuery.GetSinglePMByCodeAndCheckTenantZero(entity.ComputingPartnerCode, tenant);
                            entityPM.CreatedByPartner = (partner != null ? partner.Name : null);

                        }
                        ARInvoiceService service = new ARInvoiceService(MyContext, tenant);
                        service.Update(entityPM, true);

                        APIHelper.AddCommunicationLog("D",  oldEntity, entity, "ARInvoice", entityPM.Id, "ARInvoice API", authToken.Tenant);

                        scope.Complete();

                        
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }

                catch (Exception ex)
                {
                    var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                    APIHelper.AddCommunicationLog("F",  oldEntity, apiExceptionResult.Exception, "ARInvoice", null, "ARInvoice API");
                    return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
                }
            }
            else
            {
                var apiExceptionResult = ApiExceptionHandler.HandleModelException(ModelState);
                APIHelper.AddCommunicationLog("F", oldEntity, apiExceptionResult.Exception, "ARInvoice", null, "ARInvoice API");
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }
    }
}