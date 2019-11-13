using Intuit.Ipp.Core;
using Intuit.Ipp.QueryFilter;
using Intuit.Ipp.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Intuit.Ipp.LinqExtender;
using WebFreight.Web.Helpers;
using Intuit.Ipp.Data;
using Logitude.BL.InvoiceModel.EntityPMs;
using Intuit.Ipp.DataService;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.InvoiceModel;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Simplog.Data.CommonDataModel.Repositories;
using WebFreight.Web.Security;
using Logitude.BL.InvoiceModel.Tools;
using WebFreight.Web.Helpers.Quickbooks;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.SystemLogs;

namespace WebFreight.Web.Controllers.WebDomainControllers
{
    public class QuickbooksDomainController : ApiController
    {


        public HttpResponseMessage GetQuickBooksQueries(string CardName,string SearchField,string SearchText,bool ReceivableCard,bool PayableCard,string LogitudeCardName)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string token = HttpContext.Current.Request.Headers["Token"];
                Simplog.Data.CommonDataModel.EntityPOCOs.AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                QuickbooksHelper helper = new QuickbooksHelper(authToken.Tenant + "");
                if (string.IsNullOrEmpty(SearchText))
                    SearchText = "";
                string sql = helper.QBOSQL(CardName, SearchField, SearchText, ReceivableCard, PayableCard, LogitudeCardName);
                if (!string.IsNullOrEmpty(sql))
                {
                    if (ReceivableCard)
                    {
                        var myResult = helper.GetQuickBooksOnlineCustomersByText(sql);
                        return Request.CreateResponse(HttpStatusCode.OK, myResult);

                    }

                    else if (PayableCard)
                    {
                        var myResult = helper.GetQuickBooksOnlineVendorByText(sql);
                        return Request.CreateResponse(HttpStatusCode.OK, myResult);
                    }

                    else if (LogitudeCardName == "VatType")
                    {
                        var myResult = helper.GetQuickBooksOnlineVatTypesByText(sql);
                        return Request.CreateResponse(HttpStatusCode.OK, myResult);
                    }



                    else if (LogitudeCardName == "PaymentTerm")
                    {
                        var myResult = helper.GetQuickBooksOnlinePaymentTermsByText(sql);
                        return Request.CreateResponse(HttpStatusCode.OK, myResult);
                    }


                    else if (LogitudeCardName == "Currency")
                    {
                        var myResult = helper.GetQuickBooksOnlineCurrenciesByText(sql);
                        return Request.CreateResponse(HttpStatusCode.OK, myResult);
                    }


                    else if (LogitudeCardName == "ChargesType" && CardName == "Item")
                    {

                        var myResult = helper.GetQuickBooksOnlineReceivableChargesTypesByText(sql);
                        return Request.CreateResponse(HttpStatusCode.OK, myResult);

                    }
                    else if (CardName == "Account" || CardName == "AccountBankCredit")
                    {
                        var myResult = helper.GetQuickBooksOnlinePayablesChargesTypesByText(sql);
                        return Request.CreateResponse(HttpStatusCode.OK, myResult);
                    }


                    else if (CardName == "PaymentMethod")
                    {
                        var myResult = helper.GetQuickBooksOnlinePaymentMethodsByText(sql);
                        return Request.CreateResponse(HttpStatusCode.OK, myResult);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, new List<string>());
            
            }

            catch (Exception ex)
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                Simplog.Data.CommonDataModel.EntityPOCOs.AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                ExceptionHandler.HandleException(ex, DateTime.Now, authToken.Tenant, "", "Quickbooks "+ LogitudeCardName, "", null);

                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));

            }


        }




        public HttpResponseMessage GetQuickBooksOnlineCustomersById(String Id)
        {
            try
            {
                
                string token = HttpContext.Current.Request.Headers["Token"];
                Simplog.Data.CommonDataModel.EntityPOCOs.AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string sql = "Select DisplayName from Customer where Id='" + Id + "'";
                ARInvoiceHelper service = new ARInvoiceHelper();
                var myResult = service.GetQuickBooksOnlineCustomersByText(sql, authToken.Tenant+"");              
                return Request.CreateResponse(HttpStatusCode.OK, myResult);

            }

            catch (Exception ex) {

                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));

            }


        }

        public HttpResponseMessage GetQuickBooksOnlineVatTypesById(String Id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                Simplog.Data.CommonDataModel.EntityPOCOs.AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string sql = "Select Name from TaxCode where Id='" + Id + "'";
                ARInvoiceHelper service = new ARInvoiceHelper();
                var myResult = service.GetQuickBooksOnlineVatTypesByText(sql, authToken.Tenant+"");
                return Request.CreateResponse(HttpStatusCode.OK, myResult);

            }

            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));

            }


        }


        public HttpResponseMessage GetQuickBooksOnlineReceivableChargesTypesById(String Id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                Simplog.Data.CommonDataModel.EntityPOCOs.AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string sql = "Select * from Item where Id ='" + Id + "'";
                
               ARInvoiceHelper service = new ARInvoiceHelper();
                var myResult = service.GetQuickBooksOnlineReceivableChargesTypesByText(sql,authToken.Tenant+"");
                return Request.CreateResponse(HttpStatusCode.OK, myResult);

            }

            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));

            }


        }


        public HttpResponseMessage GetQuickBooksOnlinePayablesChargesTypesById(String Id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                Simplog.Data.CommonDataModel.EntityPOCOs.AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string sql = "Select * from Account where Id='" + Id + "'";

                ARInvoiceHelper service = new ARInvoiceHelper();
                var myResult = service.GetQuickBooksOnlinePayablesChargesTypesByText(sql, authToken.Tenant + "");
                return Request.CreateResponse(HttpStatusCode.OK, myResult);

            }

            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));

            }


        }



        public HttpResponseMessage GetQuickBooksOnlineCurrenciesById(String Id)
        {
            try
            {

                string token = HttpContext.Current.Request.Headers["Token"];
                Simplog.Data.CommonDataModel.EntityPOCOs.AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string sql ="Select * from CompanyCurrency where code ='" + Id + "'";


               ARInvoiceHelper service = new ARInvoiceHelper();
                var myResult = service.GetQuickBooksOnlineCurrenciesByText(sql,authToken.Tenant+"");
                return Request.CreateResponse(HttpStatusCode.OK, myResult);

            }

            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));

            }


        }



        public HttpResponseMessage GetQuickBooksOnlinePaymentTermsById(String Id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                Simplog.Data.CommonDataModel.EntityPOCOs.AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string sql = "Select Name from Term where Id='" + Id + "'";

                ARInvoiceHelper service = new ARInvoiceHelper();
                var myResult = service.GetQuickBooksOnlinePaymentTermsByText(sql, authToken.Tenant+"");
                return Request.CreateResponse(HttpStatusCode.OK, myResult);

            }

            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));

            }


        }


        public HttpResponseMessage GetQuickBooksOnlineVendorById(String Id)
        {
            try
            {

                string token = HttpContext.Current.Request.Headers["Token"];
                Simplog.Data.CommonDataModel.EntityPOCOs.AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string sql = "Select DisplayName from Vendor where Id='" + Id + "'";

                ARInvoiceHelper service = new ARInvoiceHelper();
                var myResult = service.GetQuickBooksOnlineVendorByText(sql, authToken.Tenant + "");
                return Request.CreateResponse(HttpStatusCode.OK, myResult);

            }

            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));

            }


        }


        public HttpResponseMessage GetQuickBooksOnlinePaymentMethodsById(String Id)
        {
            try
            {

                string token = HttpContext.Current.Request.Headers["Token"];
                Simplog.Data.CommonDataModel.EntityPOCOs.AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string sql = "Select Name from PaymentMethod where Id='" + Id + "'";

                ARInvoiceHelper service = new ARInvoiceHelper();
                var myResult = service.GetQuickBooksOnlinePaymentMethodByText(sql, authToken.Tenant + "");
                return Request.CreateResponse(HttpStatusCode.OK, myResult);

            }

            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));

            }


        }




        public HttpResponseMessage GetAccountingSettingsForTenant(string code)
        {
            try
            {
                AccountingSystemPM accountSystem = null;
               if (code != null)
               {
                    string token = HttpContext.Current.Request.Headers["Token"];
                Simplog.Data.CommonDataModel.EntityPOCOs.AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                AccountingSystemQuery accountSystems = new AccountingSystemQuery(authToken.Tenant);
                   accountSystem = accountSystems.GetSinglePM(code);
               }


               return Request.CreateResponse(HttpStatusCode.OK, accountSystem);



            }

            catch(Exception ex){

                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));

            }

        }


        public Boolean checkInvoiceNumber(string invoiceNumber)
        {
            ServiceContext serviceContext = getServiceContext("qyprd6uIZeM8IamAy4dVcmEfoQj7UybHB2En9UJj4T6JCJD5", "OAqYF6R9XqqgCepXuqknhobCYaVZNZXZoxq3ia1c", "123145721330079");
            QueryService<Invoice> invoiceQueryService = new QueryService<Invoice>(serviceContext);



            List<Invoice> myResult = invoiceQueryService.ExecuteIdsQuery("Select * from Invoice where DocNumber='" + invoiceNumber+"'").ToList();
            if (myResult.Count != 0)
                return false;
            return true;


        }




        public HttpResponseMessage GetInvoiceToQuickBooks(String Customerid,String invoiceId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                Simplog.Data.CommonDataModel.EntityPOCOs.AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);

                SecurityUtility.CheckContactFeature("ARInvoice", "READ", authToken.Tenant);
                ARInvoiceQuery aRInvoiceQuery = new ARInvoiceQuery(authToken.Tenant);
                ARInvoicePM invoice = aRInvoiceQuery.GetSinglePM(invoiceId, authToken.Tenant);


                ServiceContext serviceContext = getServiceContext("qyprd6uIZeM8IamAy4dVcmEfoQj7UybHB2En9UJj4T6JCJD5", "OAqYF6R9XqqgCepXuqknhobCYaVZNZXZoxq3ia1c", "123145721330079");
                QueryService<Customer> customerQueryService = new QueryService<Customer>(serviceContext);
                Invoice final = null;
                if (checkInvoiceNumber(invoice.InvoiceNumber))
                {
                    Intuit.Ipp.Data.Invoice QBOInvoice = new Invoice();
                    QBOInvoice.CustomerRef = new ReferenceType { Value = Customerid };
                    QBOInvoice.DocNumber = invoice.InvoiceNumber;
                    System.Collections.Generic.List<Line> lineList = new List<Line>();

                    for (int i = 0; i < invoice.InvoiceLines.Count; i++)
                    {
                        Line line = new Line();
                        line.Description = invoice.InvoiceLines[i].Description;
                        line.Amount = Decimal.Parse(invoice.InvoiceLines[i].InvoiceCurrencyAmount + "");
                        line.AmountSpecified = true;                       
                        line.DetailType = LineDetailTypeEnum.DescriptionOnly;
                        line.DetailTypeSpecified = true;

                        lineList.Add(line);
                    }


                    QBOInvoice.Line = lineList.ToArray();
                    QBOInvoice.ExchangeRate = decimal.Parse(invoice.InvoiceCurrencyExchangeRate+"");
                   // QBOInvoice.CurrencyRef = new ReferenceType {Value=invoice.InvoiceCurrencyCode };
                    QBOInvoice.TxnDate = invoice.InvoiceDate.Value;                  
                    DataService service = new DataService(serviceContext);
                     final = service.Add(QBOInvoice) as Invoice;
                     if (final!=null)
                     {
                         invoice.TransferStatusCode = "BL";

                         IInvoiceContext MyContext = InvoiceContext.GetContext(invoice.Tenant);
                         ARInvoiceService arInvoiceService = new ARInvoiceService(MyContext, invoice.Tenant);
                         arInvoiceService.Update(invoice);
                     }

                }
               

                    return Request.CreateResponse(HttpStatusCode.OK, final);

                

            }


            catch (Exception ex)
            {

                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));


            }




        }

        private static ServiceContext getServiceContext(String OAuthAccessToken, String OAuthAccessTokenSecret, String RealmId)
        {
            var consumerKey = "qyprdYKTUqQGAV8AudZJl40XhETBGd";
            var consumerSecret = "Wy8bD4Q5TgyZpqME8XPdSdvIqCOJcdIfPmcfbVaB";
            OAuthRequestValidator oauthValidator = new OAuthRequestValidator(OAuthAccessToken, OAuthAccessTokenSecret, consumerKey, consumerSecret);
            return new ServiceContext(RealmId, (IntuitServicesType)(1), oauthValidator);
        }


    }
}