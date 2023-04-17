using Logitude.Accounting.BL.APIDataContract.ApiV1;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.InvoiceModel.APIDataContract;
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
    public class ARInvoiceJournalLTController : ApiController
    {

        public HttpResponseMessage GetARInvoiceJournalLT(string id, string number)
        {
            try
            {

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticateAPICall(authToken.Tenant);
                SecurityUtility.AuthenticateAccessibleAPI("ARInvoice", authToken.Tenant);
                string xmlstring;
                ARInvoiceQueryService Service = new ARInvoiceQueryService(tenant);
                ServiceResponse response = new ServiceResponse();
                ARInvoiceLite aRInvoice = new ARInvoiceLite();
                if (!string.IsNullOrEmpty(id))
                {
                    //  aRInvoice = Service.GetARInvoiceById(id, tenant);
                    aRInvoice = Service.GetARInvoiceLiteById(id, tenant);
                }
                else if (!string.IsNullOrEmpty(number))
                {
               //   aRInvoice = Service.GetARInvoiceByInvoiceNumber(number, tenant);
                    aRInvoice = Service.GetARInvoiceLiteByInvoiceNumber(number, tenant);
                }
                ARInvoiceJournalLT journalLTResult_onlyInv = new ARInvoiceJournalLT()
                {
                    Id = aRInvoice != null ? aRInvoice.Id : "",
                    JournalId = "",
                    IsLedgerCreated = false,
                };

                ARInvoiceJournalLT Result;
                if (aRInvoice != null && !String.IsNullOrEmpty(aRInvoice.Id))
                {
                    JournalQueryService journalQueryService = new JournalQueryService(tenant);
                 // ARInvoiceJournalLT journalLTResult = journalQueryService.GetSingleJournalByAccountingEntity(aRInvoice.Id, "2", tenant);
                    ARInvoiceJournalLT journalLTResult = journalQueryService.GetSingleJournalLiteByAccountingEntity(aRInvoice.Id, "2", tenant);
                    if (journalLTResult == null || String.IsNullOrEmpty(journalLTResult.JournalId))
                    {
                        Result = journalLTResult_onlyInv;
                    }
                    else
                    {
                        Result = journalLTResult;
                    }
                }
                else
                {
                    Result = journalLTResult_onlyInv;
                }


                xmlstring = LogitudeXmlSerializer.SerializeObjectToXmlString(Result);
                return Request.CreateResponse(HttpStatusCode.OK, Result);
            }
            catch (Exception ex)
            {
                var apiExceptionResult = ApiExceptionHandler.HandleException(ex);
                return Request.CreateResponse(apiExceptionResult.StatusCode, apiExceptionResult.Exception);
            }
        }

    }
}