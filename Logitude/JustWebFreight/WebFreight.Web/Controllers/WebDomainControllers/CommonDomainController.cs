
using Dropbox.Api;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using Logitude.Accounting.Data.EntityLists;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.DataContracts;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Logitude.BL.QuoteModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BookingLib.Data.EntityLists;
using Logitude.CRM.Data.EntityLists;
using Logitude.Customs.Data;
using Logitude.Customs.Data.EntityListQueryServices;
using Logitude.Customs.Data.EntityLists;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.StorageService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure.LogitudeCacheManager;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Xml;
using WebFreight.Web.AccountingModel.DomainServices;
using WebFreight.Web.BookingModel.DomainServices;
using WebFreight.Web.CommonDataModel.DomainServices;
using WebFreight.Web.Controllers.CommonDataModel.Extended;
using WebFreight.Web.CRMModel.DomainServices;
using WebFreight.Web.CustomModel.DomainServices;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Helpers.APIHelpers;
using WebFreight.Web.QuoteModel.DomainServices;
using WebFreight.Web.Security;
using WebFreight.Web.ShipmentsModel.DomainServices;

namespace WebFreight.Web.Controllers.WebDomainControllers
{
    public class CommonDomainController : ApiController
    {
        public HttpResponseMessage GetUpdateAutoDisplay(string myChargeTypeId, string myPropertyTypeCode, bool isAutoDisplay)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;
                    string loggedUserEmail = authToken.Email;

                    SecurityUtility.AuthenticationOnTenant(tenant);
                    SecurityUtility.CheckContactFeature("ChargesType", "UPDATE", tenant);

                    ChargesTypeRepository chargesTypeRepository = new ChargesTypeRepository(tenant);
                    ChargesType myChargeType = chargesTypeRepository.GetSingleChargesType(myChargeTypeId, tenant);

                    if (myChargeType != null)
                    {
                        switch (myPropertyTypeCode)
                        {
                            case "S":
                                {
                                    myChargeType.IsAutoDisplayInShipment = isAutoDisplay;
                                    break;
                                }

                            case "C":
                                {
                                    myChargeType.IsAutoDisplayInConsolidation = isAutoDisplay;
                                    break;
                                }
                        }

                        chargesTypeRepository.Update(myChargeType);
                        chargesTypeRepository.SubmitChanges();
                    }

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, "OK");
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetTranslationHeadersByTenant(int Id)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = Id;
                string loggedUserEmail = authToken.Email;

                SecurityUtility.AuthenticationOnTenant(tenant);

                TranslationHeaderRepository repository = new TranslationHeaderRepository(tenant);
                List<TranslationHeader> myResult = repository.GetTranslationHeadersByTenant(Id).ToList();

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetCopyCurrencyToTenant(string CurrencyId, double CurrencyRate, DateTime RateDate)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(tenant);
                    SecurityUtility.CheckContactFeature("Currency", "READ", tenant);

                    CommonDataDomainService commonDomain = new CommonDataDomainService();
                    CurrencyList myResult = commonDomain.CopyCurrencyToTenant(CurrencyId, tenant, CurrencyRate, RateDate);
                    //CurrencyList myResult = new CurrencyList();

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetBlueSnapToken(string VaultedShopperId,string countryname)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    BluesnapParameters bluesnapParameters = new BluesnapParameters();

                    if (VaultedShopperId == "undefined")
                        VaultedShopperId = null;
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;
                    int tenant = authToken.Tenant;
                    string myResult=null;
                    SecurityUtility.AuthenticationOnTenant(tenant);
                    if(!String.IsNullOrEmpty(VaultedShopperId))
                    {                 
                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri("https://ws.bluesnap.com/services/2/tools/auth-token");
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/xml"));

                        string apicreditionals = "API_15408257301181065689979";
                        var request = WebRequest.Create("https://ws.bluesnap.com/services/2/tools/auth-token?shopperId=" + VaultedShopperId + "&expirationInMinutes=120");
                        if ((LogitudeSettings.DeploymentStage == "logitudepreproduction" || LogitudeSettings.DeploymentStage == "Dev") && countryname != "Israel")
                        {
                            apicreditionals = "API_1516630314047705132569";
                            bluesnapParameters.ContractId = "2261197";
                            bluesnapParameters.Storeid = "8439";
                            request = WebRequest.Create("https://sandbox.bluesnap.com/services/2/tools/auth-token?shopperId=" + VaultedShopperId + "&expirationInMinutes=120");

                        }


                        string authInfo = apicreditionals + ":" + "BlueSand123";
                        authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));

                        request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(apicreditionals+":BlueSand123"));
                        try
                        {
                            var response2 = request.GetResponse();

                            string strResponse = "";
                            using (var sr = new StreamReader(response2.GetResponseStream()))
                            {
                                strResponse = sr.ReadToEnd();

                            }

                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(strResponse);


                            myResult = doc.InnerText;
                            bluesnapParameters.Token = myResult;

                        }
                        catch (Exception EX1)
                        {

                             authInfo = "API_15416735830591484092606" + ":" + "BlueSand123";
                            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));

                            //like this:
                             request = WebRequest.Create("https://ws.bluesnap.com/services/2/tools/auth-token?shopperId=" + VaultedShopperId + "&expirationInMinutes=120");
                            request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("API_15416735830591484092606:BlueSand123"));
                            try
                            {
                                var response2 = request.GetResponse();

                                string strResponse = "";
                                using (var sr = new StreamReader(response2.GetResponseStream()))
                                {
                                    strResponse = sr.ReadToEnd();

                                }

                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(strResponse);


                               myResult = doc.InnerText;
                                bluesnapParameters.Token = myResult;

                            }
                            catch (Exception EX2)
                            {
                                myResult = null;
                                bluesnapParameters.Token = myResult;

                            }

                        }

                    }



                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, bluesnapParameters);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }



        


        public HttpResponseMessage GetBlueSnapSecretToken(string VaultedShopperId,string countryname)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    if (VaultedShopperId == "undefined")
                        VaultedShopperId = null;
                    BluesnapParameters bluesnapParameters = new BluesnapParameters();
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;
                    int tenant = authToken.Tenant;
                    string myResult = null;
                    SecurityUtility.AuthenticationOnTenant(tenant);
                    if (!String.IsNullOrEmpty(VaultedShopperId))
                    {
                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri("https://ws.bluesnap.com/services/2/tools/auth-token");
                        ServicePointManager.Expect100Continue = true;
                        ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                        client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/xml"));
                        string apicreditionals = "API_15408257301181065689979";
                        var request = WebRequest.Create("https://bluesnap.com/services/2/tools/param-encryption");
                        if ((LogitudeSettings.DeploymentStage == "logitudepreproduction" || LogitudeSettings.DeploymentStage == "Dev") && countryname != "Israel")
                        {
                            apicreditionals = "API_1516630314047705132569";
                            bluesnapParameters.ContractId = "2261197";
                            bluesnapParameters.Storeid = "8439";
                            request = WebRequest.Create("https://sandbox.bluesnap.com/services/2/tools/param-encryption");
                        }

                        string authInfo = apicreditionals + ":" + "BlueSand123";
                        authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));



                        string xml = @"<param-encryption xmlns='http://ws.plimus.com'><parameters><parameter><param-key>shopperId</param-key><param-value>"+VaultedShopperId+"</param-value></parameter><parameter><param-key>expirationInMinutes</param-key><param-value>300</param-value></parameter><parameter><param-key>pageName</param-key><param-value>AUTO_LOGIN_PAGE</param-value></parameter></parameters></param-encryption>";
                        //like this:
                        request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes(apicreditionals+":BlueSand123"));

                        byte[] bytes;
                        bytes = System.Text.Encoding.ASCII.GetBytes(xml);
                        request.ContentType = "application/xml";
                        request.ContentLength = bytes.Length;

                        request.Method = "POST";
                        Stream requestStream = request.GetRequestStream();
                        requestStream.Write(bytes, 0, bytes.Length);
                        requestStream.Close();
                        try
                        {
                            var response2 = request.GetResponse();

                            string strResponse = "";
                            using (var sr = new StreamReader(response2.GetResponseStream()))
                            {
                                strResponse = sr.ReadToEnd();

                            }

                            XmlDocument doc = new XmlDocument();
                            doc.LoadXml(strResponse);


                            myResult = doc.InnerText;
                            bluesnapParameters.Token = myResult;

                        }
                        catch (Exception EX1)
                        {
                            authInfo = "API_15416735830591484092606" + ":" + "BlueSand123";
                            authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));

                            //like this:
                             request = WebRequest.Create("https://bluesnap.com/services/2/tools/param-encryption");
                            request.Headers["Authorization"] = "Basic " + Convert.ToBase64String(Encoding.Default.GetBytes("API_15416735830591484092606:BlueSand123"));

                            bytes =null;
                            bytes = System.Text.Encoding.ASCII.GetBytes(xml);
                            request.ContentType = "application/xml";
                            request.ContentLength = bytes.Length;

                            request.Method = "POST";
                             requestStream = request.GetRequestStream();
                            requestStream.Write(bytes, 0, bytes.Length);
                            requestStream.Close();

                            try
                            {
                                var response2 = request.GetResponse();

                                string strResponse = "";
                                using (var sr = new StreamReader(response2.GetResponseStream()))
                                {
                                    strResponse = sr.ReadToEnd();

                                }

                                XmlDocument doc = new XmlDocument();
                                doc.LoadXml(strResponse);


                                myResult = doc.InnerText;
                                bluesnapParameters.Token = myResult;

                            }
                            catch (Exception EX2)
                            {
                                myResult = null;
                                bluesnapParameters.Token = myResult;

                            }

                        }

                    }



                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, bluesnapParameters);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }



        public HttpResponseMessage GetPortCopyToCurrentTenant(string entityId)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    string loggedUserEmail = authToken.Email;
                    int tenant = authToken.Tenant;

                    CommonDataDomainService commonDomain = new CommonDataDomainService();
                    PortList myResult = commonDomain.GetPortCopyToCurrentTenant(entityId, tenant);

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetCopyCommodityToTenant(string entityId)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(tenant);
                    SecurityUtility.CheckContactFeature("Commodity", "NEW", tenant);

                    CommodityRepository commodityRepository = new CommodityRepository(0);
                    Commodity myTenantZeroCommodity = commodityRepository.GetSingleCommodity(entityId, 0);

                    CommodityPM entityPM = null;

                    if (myTenantZeroCommodity != null)
                    {
                        bool isCommodityExists = commodityRepository.IsCommodityExists(myTenantZeroCommodity.Code, tenant);

                        if (!isCommodityExists)
                        {
                            entityPM = new CommodityPM()
                            {
                                Tenant = tenant,
                                Code = myTenantZeroCommodity.Code,
                                Name = myTenantZeroCommodity.Name,
                                AirlineId = myTenantZeroCommodity.AirlineId,
                                InActive = myTenantZeroCommodity.InActive,
                                SearchFields = myTenantZeroCommodity.SearchFields,
                            };

                            ICommonDataContext objectContext = CommonDataContext.GetContext(tenant);

                            CommodityService service = new CommodityService(objectContext, tenant);
                            service.Create(entityPM);

                            objectContext.SaveChanges();
                        }
                    }

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, entityPM);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetQuickSearch(string ObjectTableName, string SearchFields)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);

                //object myResult = null;

                if (!string.IsNullOrEmpty(ObjectTableName))
                {
                    bool isFullTextSearch = false;

                    //using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    //{
                    //    SettingRepository mySettingRepository = new SettingRepository();
                    //    Setting mySetting = mySettingRepository.GetSingleSetting("1");

                    //    if (mySetting != null)
                    //    {
                    //        if (mySetting.DeploymentStage != null)
                    //        {
                    //            if (mySetting.DeploymentStage.ToLower() == "amitalstorage" || mySetting.DeploymentStage.ToLower() == "dev")
                    //            {
                    //                isFullTextSearch = true;
                    //            }
                    //        }
                    //    }
                    //    if (isFullTextSearch == false)
                    //    {
                    //        string loggedUserEmail = authToken.Email;
                    //        string loggedContactId = null;
                    //        ContactQuery contactQuery = new ContactQuery(tenant);
                    //        ContactPM loggedContact = contactQuery.GetContactByEmailOnly(loggedUserEmail, tenant);
                    //        if (loggedContact != null)
                    //        {
                    //            loggedContactId = loggedContact.Id;
                    //        }
                    //        if (loggedContactId == "1-23905" || loggedContactId == "1-60232" || loggedContactId == "1-16354")//Ayman,Ihab and Rabaia
                    //        {
                    //            isFullTextSearch = true;
                    //        }
                    //    }

                    //}

                    //if (isFullTextSearch == false)
                    //{
                    //    string loggedUserEmail = authToken.Email;
                    //    string loggedContactId = null;
                    //    ContactQuery contactQuery = new ContactQuery(tenant);
                    //    ContactPM loggedContact = contactQuery.GetContactByEmailOnly(loggedUserEmail, tenant);
                    //    if (loggedContact != null)
                    //    {
                    //        loggedContactId = loggedContact.Id;
                    //    }
                    //    if (loggedContactId == "1-23905" || loggedContactId == "1-60232" || loggedContactId == "1-16354")//Ayman,Ihab and Rabaia
                    //    {
                    //        isFullTextSearch = true;
                    //    }
                    //}
                    TenantRepository myTenantRepository = new TenantRepository(tenant);
                    Tenant myTenant = myTenantRepository.GetSingleTenant(tenant);
                    if (myTenant != null)
                    {
                        isFullTextSearch = myTenant.IsFullTextSearchEnabled;
                    }
                    QueryOperations myQueryOperations = new QueryOperations();
                    myQueryOperations.PageIndex = 0;
                    myQueryOperations.PageSize = 10;
                    if (ObjectTableName == "Airline")
                    {
                        myQueryOperations.PageSize = 8;
                    }

                    if (!string.IsNullOrEmpty(SearchFields))
                    {
                        myQueryOperations.SetFilter("SearchFields", SearchFields, false, "Contains", null, false);
                    }

                    FilterSerializer serializer = new FilterSerializer();
                    byte[] arrayOfBytes = serializer.SerializeFilterItems(myQueryOperations);

                    switch (ObjectTableName)
                    {
                        case "Shipment":
                            {
                                ShipmentsDomainService myDomainService = new ShipmentsDomainService();
                                if (isFullTextSearch)
                                {
                                    IQueryable<ShipmentList> myResult = myDomainService.GetShipmentFullTextSearch(arrayOfBytes, tenant);
                                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                                }

                                else
                                {
                                    //IQueryable<ShipmentList> myResult = myDomainService.GetShipmentFilters(arrayOfBytes, tenant);

                                    IShipmentsContext iContext = ShipmentsContext.GetContext(tenant);
                                    IQueryable<Shipment> iQueryable = (from d in iContext.Shipments where d.Tenant == tenant select d);

                                    if (!string.IsNullOrEmpty(SearchFields))
                                    {
                                        iQueryable = iQueryable.Where(d => d.SearchFields != null);
                                        iQueryable = iQueryable.Where(d => d.SearchFields.ToLower().Contains(SearchFields.ToLower()));
                                    }

                                    iQueryable = iQueryable.OrderByDescending(d => d.CreateDateTime);
                                    iQueryable = System.Data.Entity.QueryableExtensions.Skip(iQueryable, () => 0);
                                    iQueryable = System.Data.Entity.QueryableExtensions.Take(iQueryable, () => 10);

                                    List<ShipmentList> myResult = (from x in iQueryable.Include("Direction").Include("TransportMode").Include("CustomerCard")
                                                                   select new ShipmentList()
                                                                   {
                                                                       Id = x.Id,
                                                                       Tenant = x.Tenant,
                                                                       ShipmentNumber = x.ShipmentNumber,
                                                                       DirectionId = x.DirectionId,
                                                                       TransportModeId = x.TransportModeId,
                                                                       CustomerId = x.CustomerId,
                                                                       DirectionName = x.Direction == null ? null : x.Direction.Name,
                                                                       TransportModeName = x.TransportMode == null ? null : x.TransportMode.Name,
                                                                       CustomerName = x.CustomerCard == null ? null : x.CustomerCard.EnglishName,
                                                                   }).ToList();

                                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                                }

                                //break;
                            }

                        case "Booking":
                            {
                                BookingsDomainService myDomainService = new BookingsDomainService();
                                List<BookingList> myResult = myDomainService.GetBookingFilters(arrayOfBytes, tenant);
                                return Request.CreateResponse(HttpStatusCode.OK, myResult);
                                //break;
                            }

                        case "User":
                            {
                                ContactDomainService myDomainService = new ContactDomainService();
                                List<UserList> myResult = myDomainService.GetUserFilters(arrayOfBytes, tenant);
                                return Request.CreateResponse(HttpStatusCode.OK, myResult);
                                //break;
                            }

                        case "Ticket":
                            {
                                CRMDomainService myDomainService = new CRMDomainService();
                                List<TicketList> myResult = myDomainService.GetTicketFilters(arrayOfBytes, tenant).ToList();
                                return Request.CreateResponse(HttpStatusCode.OK, myResult);
                                //break;
                            }

                        case "Quote":
                            {
                                QuotesDomainService myDomainService = new QuotesDomainService();
                                List<QuoteList> myResult = myDomainService.GetQuoteFilters(arrayOfBytes, tenant).ToList();
                                return Request.CreateResponse(HttpStatusCode.OK, myResult);
                                //break;
                            }

                        case "CardGLAccount":
                            {
                                // Set Type Filter
                                myQueryOperations.SetFilter("AccountTypeCode", "1", false, "Equals", null, false);
                                arrayOfBytes = serializer.SerializeFilterItems(myQueryOperations);

                                // Get the list
                                AccountingDomainService myDomainService = new AccountingDomainService();
                                List<GLAccountList> myResult = myDomainService.GetGLAccountFilters(arrayOfBytes, tenant);
                                return Request.CreateResponse(HttpStatusCode.OK, myResult);
                                //break;
                            }

                        case "CustomerGLAccount":
                            {
                                // Set Type Filter
                                myQueryOperations.SetFilter("AccountTypeCode", "2", false, "Equals", null, false);
                                arrayOfBytes = serializer.SerializeFilterItems(myQueryOperations);

                                // Get the list
                                AccountingDomainService myDomainService = new AccountingDomainService();
                                List<GLAccountList> myResult = myDomainService.GetGLAccountFilters(arrayOfBytes, tenant);
                                return Request.CreateResponse(HttpStatusCode.OK, myResult);
                                //break;
                            }

                        case "VendorGLAccount":
                            {
                                // Set Type Filter
                                myQueryOperations.SetFilter("AccountTypeCode", "3", false, "Equals", null, false);
                                arrayOfBytes = serializer.SerializeFilterItems(myQueryOperations);

                                // Get the list
                                AccountingDomainService myDomainService = new AccountingDomainService();
                                List<GLAccountList> myResult = myDomainService.GetGLAccountFilters(arrayOfBytes, tenant);
                                return Request.CreateResponse(HttpStatusCode.OK, myResult);
                                //break;
                            }

                        case "Journal":
                            {
                                AccountingDomainService myDomainService = new AccountingDomainService();
                                List<JournalList> myResult = myDomainService.GetJournalFilters(arrayOfBytes, tenant);
                                return Request.CreateResponse(HttpStatusCode.OK, myResult);
                                //break;
                            }

                        case "CashBook":
                            {
                                AccountingDomainService myDomainService = new AccountingDomainService();
                                List<CashBookList> myResult = myDomainService.GetCashBookFilters(arrayOfBytes, tenant);
                                return Request.CreateResponse(HttpStatusCode.OK, myResult);
                                //break;
                            }

                        case "BankDeposit":
                            {
                                AccountingDomainService myDomainService = new AccountingDomainService();
                                List<BankDepositList> myResult = myDomainService.GetBankDepositFilters(arrayOfBytes, tenant);
                                return Request.CreateResponse(HttpStatusCode.OK, myResult);
                                //break;
                            }

                        case "Customer":
                            {

                                PartnersDomainService myDomainService = new PartnersDomainService();
                                List<CustomerList> myResult = myDomainService.GetCustomerFilters(arrayOfBytes, tenant);
                                return Request.CreateResponse(HttpStatusCode.OK, myResult);


                                //partnerTypeId
                            }

                        case "Contact":
                            {
                                ContactDomainService myDomainService = new ContactDomainService();
                                List<ContactList> myResult = myDomainService.GetContactFilters(arrayOfBytes, tenant).ToList();
                                return Request.CreateResponse(HttpStatusCode.OK, myResult);
                            }
                        case "Activity":
                            {
                                CRMDomainService myDomainService = new CRMDomainService();
                                List<ActivityList> myResult = myDomainService.GetActivityFilters(arrayOfBytes, tenant).ToList();
                                return Request.CreateResponse(HttpStatusCode.OK, myResult);
                            }
                        case "Opportunity":
                            {
                                CRMDomainService myDomainService = new CRMDomainService();
                                List<OpportunityList> myResult = myDomainService.GetOpportunityFilters(arrayOfBytes, tenant).ToList();
                                return Request.CreateResponse(HttpStatusCode.OK, myResult);
                            }

                        case "AgentSharedManifest":
                            {
                                ContactDomainService myDomainService = new ContactDomainService();
                                List<AgentSharedManifestList> myResult = myDomainService.GetAgentSharedManifestFilters(arrayOfBytes, tenant).ToList();
                                return Request.CreateResponse(HttpStatusCode.OK, myResult);

                            }

                        case "Airline":
                            {
                                myQueryOperations.SetFilter("IsAllowedInAirlinesRestriction", false, false, "Equals", null, false);
                                arrayOfBytes = serializer.SerializeFilterItems(myQueryOperations);

                                PartnersDomainService myDomainService = new PartnersDomainService();
                                List<AirlineList> myResult = myDomainService.GetAirlineFilters(arrayOfBytes, tenant).ToList();
                                return Request.CreateResponse(HttpStatusCode.OK, myResult);
                            }

                        //
                        //Customs
                        case "Customs.Client":
                            {

                                CustomDomainService myDomainService = new CustomDomainService();
                                List<ClientList> myResult = myDomainService.GetClientFilters(arrayOfBytes, tenant);
                                return Request.CreateResponse(HttpStatusCode.OK, myResult);
                            }
                        case "Customs.CustomsItems":
                            {
                                ICustomContext ctx = CustomContext.GetContext(tenant);
                                CustomsItemListQueryService query = new CustomsItemListQueryService(ctx);

                                QueryOperations QO = new QueryOperations();
                                QO.PageIndex = 0;
                                QO.PageSize = 100;
                                QO.SetFilter("FullClassification", SearchFields, false, "Contains", null, false);
                                QO.SetFilter("CustomsItemCategoryID", "2,3", false, "InListInt", null, false);

                                List<CustomsItemList> myResult = query.GetList(QO, tenant);

                                return Request.CreateResponse(HttpStatusCode.OK, myResult);
                            }
                        case "Customs.CouriersVat":
                            {

                                ICustomContext ctx = CustomContext.GetContext(tenant);
                                CouriersVatListQueryService query = new CouriersVatListQueryService(ctx);

                                QueryOperations QO = new QueryOperations();
                                QO.PageIndex = 0;
                                QO.PageSize = 100;
                                QO.SetFilter("SearchFields", SearchFields, false, "Contains", null, false);
                                QO.SetFilter("InActive", false, false, "Equals", null, false);

                                List<CouriersVatList> myResult = query.GetList(QO, tenant);

                                return Request.CreateResponse(HttpStatusCode.OK, myResult);
                            }

                        case "Customs.CustomsAirline":
                            {

                                ICustomContext ctx = CustomContext.GetContext(tenant);
                                CustomsAirlineListQueryService query = new CustomsAirlineListQueryService(ctx);

                                QueryOperations QO = new QueryOperations();
                                QO.PageIndex = 0;
                                QO.PageSize = 100;
                                QO.SetFilter("SearchFields", SearchFields, false, "Contains", null, false);
                                QO.SetFilter("InActive", false, false, "Equals", null, false);

                                List<CustomsAirlineList> myResult = query.GetList(QO, tenant);

                                return Request.CreateResponse(HttpStatusCode.OK, myResult);
                            }

                        default:
                            {
                                object myResult = null;
                                return Request.CreateResponse(HttpStatusCode.OK, myResult);
                            }
                    }
                }

                else
                {
                    object myResult = null;
                    return Request.CreateResponse(HttpStatusCode.OK, myResult);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetDashboardSpotlightCounts(int tenant)
        {

            try
            {

                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                CommonDataDomainService commonDomain = new CommonDataDomainService();
                DailySpotlightClass myResult = commonDomain.GetDashboardSpotlightCounts(tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);

            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }
        [System.Web.Http.Route("olderthan/{loadingDate}")]
        public HttpResponseMessage GetVatTypePercentagePMByDate(string dateString)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                CommonDataDomainService commonDomain = new CommonDataDomainService();

                DateTime? loadingDate = DateHelper.GetDate(dateString);
                if (loadingDate == null)
                {
                    loadingDate = TenantServerConfigration.GetCurrentDateTime(tenant);
                }

                List<VatTypePercentagePM> myResult = commonDomain.GetVatTypePercentagePMByDate(tenant, loadingDate);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetContactsCounts()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                ContactSummary summaryClass = new ContactSummary() { Id = tenant };
                ContactDomainService service = new ContactDomainService();
                summaryClass = service.GetContactsSummary(tenant);

                return Request.CreateResponse(HttpStatusCode.OK, summaryClass);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetGettingStartedData()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                CommonDataDomainService commonDomain = new CommonDataDomainService();
                var result = commonDomain.GetGettingStartedData(tenant);
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetUserGettingStartedData(bool displayGettingStarted)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string email = authToken.Email;
                //ICommonDataContext context = CommonDataContext.GetContext(tenant);
                //ContactRepository repo = new ContactRepository(context);
                //Contact contact = repo.GetSingleContactByEmail(email, tenant);
                //contact.DisplayGettingStarted = displayGettingStarted;
                //repo.Update(contact);
                //repo.SubmitChanges();

                ContactQuery query = new ContactQuery(tenant);
                ContactPM contact = query.GetSingleContact(email, tenant);
                contact.DisplayGettingStarted = displayGettingStarted;
                ICommonDataContext MyContext = CommonDataContext.GetContext(tenant);
                ContactService service = new ContactService(MyContext, tenant);
                service.Update(contact);

                return Request.CreateResponse(HttpStatusCode.OK, "");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetDropBoxAuthURI(int tenant)
        {
            try
            {
                var tempstate = Guid.NewGuid().ToString("N");
                TenantAdditionalDataRepository Repo = new TenantAdditionalDataRepository(tenant);
                var currentTenant = Repo.GetSingleTenantAdditionalData(tenant);
                if (currentTenant != null)
                {
                    currentTenant.DropBoxState = tempstate;
                    Repo.Update(currentTenant);
                }
                else
                {
                    currentTenant = new TenantAdditionalData()
                    {
                        Tenant = tenant,
                        DropBoxState = tempstate
                    };
                    Repo.Add(currentTenant);
                }
                var URI = LogitudeSettings.LogitudeURL.Replace("http", "https");
                if (URI.Contains("logitudepre.cloudapp.net"))
                {
                    URI = "https://test.logitudeworld.com/Preproduction/";
                }
                Repo.SubmitChanges();
                var redirect = DropboxOAuth2Helper.GetAuthorizeUri(
               OAuthResponseType.Code,
               LogitudeSettings.DropboxAppKey,
               URI + "/DropBoxAuth.aspx",
               tempstate);
                //string token = HttpContext.Current.Request.Headers["Token"];
                //AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                //int tenant = authToken.Tenant;
                //string email = authToken.Email;
                ////ICommonDataContext context = CommonDataContext.GetContext(tenant);
                ////ContactRepository repo = new ContactRepository(context);
                ////Contact contact = repo.GetSingleContactByEmail(email, tenant);
                ////contact.DisplayGettingStarted = displayGettingStarted;
                ////repo.Update(contact);
                ////repo.SubmitChanges();

                //ContactQuery query = new ContactQuery(tenant);
                //ContactPM contact = query.GetSingleContact(email, tenant);
                //contact.DisplayGettingStarted = displayGettingStarted;
                //ICommonDataContext MyContext = CommonDataContext.GetContext(tenant);
                //ContactService service = new ContactService(MyContext, tenant);
                //service.Update(contact);

                return Request.CreateResponse(HttpStatusCode.OK, redirect);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetDropBoxComLog(int tenant)
        {
            try
            {
                DropBoxActionsHelper helper = new DropBoxActionsHelper(tenant);
                List<QueueTask> tasks = new List<QueueTask>();
                tasks.Add(new QueueTask() { Action = "NewImporterShipment", Parameters = new List<Logitude.Server.Tools.Parameter>() { new Logitude.Server.Tools.Parameter { Name = "ImporterShipment", Value = "sssssssssssss" } } });
                var ByteData = LogitudeXmlSerializer.SerializeObject(tasks);
                var commLog = helper.CreateDropBoxCommunicationLog(tenant, "ARInvoice", ByteData, "test.xml", "ARInvoices");
                return Request.CreateResponse(HttpStatusCode.OK, commLog);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetDropBoxAccessTocken(int tenant)
        {
            try
            {
                TenantAdditionalDataRepository Repo = new TenantAdditionalDataRepository(tenant);
                var currentTenant = Repo.GetSingleTenantAdditionalData(tenant);

                return Request.CreateResponse(HttpStatusCode.OK, currentTenant);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetRedOfDropBoxAccessTocken(int tenant)
        {
            try
            {
                TenantAdditionalDataRepository Repo = new TenantAdditionalDataRepository(tenant);
                var currentTenant = Repo.GetSingleTenantAdditionalData(tenant);
                if (currentTenant != null)
                {
                    Repo.Remove(currentTenant);
                    Repo.SubmitChanges();
                }
                return Request.CreateResponse(HttpStatusCode.OK, currentTenant);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public async Task<HttpResponseMessage> GetDropBoxConnectionTest(int tenant)
        {
            try
            {
                DropBoxActionsHelper helper = new DropBoxActionsHelper(tenant);
                List<string> tasks = new List<string>();
                tasks.Add(Guid.NewGuid().ToString());
                var ByteData = LogitudeXmlSerializer.SerializeObject(tasks);
                //string path = "/Test";
                var temp = await helper.GetCurrentAccount();

                return Request.CreateResponse(HttpStatusCode.OK, "Success");
            }

            catch (Exception ex)
            {
                if (ex.Message.Contains("invalid_access_token"))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, "invalid_access_token");
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
                }

            }
        }
        public HttpResponseMessage GetDropBoxComLogTestFile(int tenant, string FileName, string FolderName, string FullText, string ObjectTableId)
        {
            try
            {
                DropBoxActionsHelper helper = new DropBoxActionsHelper(tenant);
                var ByteData = Encoding.ASCII.GetBytes(FullText); //LogitudeXmlSerializer.SerializeObject(FullText);
                if (!FileName.Contains("."))
                {
                    FileName = FileName + ".txt";
                }
                var commLog = helper.CreateDropBoxCommunicationLog(tenant, ObjectTableId, ByteData, FileName, FolderName);
                return Request.CreateResponse(HttpStatusCode.OK, commLog);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetUpdateCustomerActualData(string entityId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                PartnersDomainService commonDomain = new PartnersDomainService();
                commonDomain.UpdateCustomerActualData(entityId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, "");
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetAllVatTypesGroups()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                VATTypesGroupQuery entityQuery = new VATTypesGroupQuery(tenant);
                List<VATTypesGroupPM> myResult = entityQuery.GetVATTypesGroups(tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetContactsByEmails(string emails)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                List<ContactList> myResult = new List<ContactList>();
                if (!string.IsNullOrEmpty(emails))
                {
                    List<string> emailsList = emails.ToLower().Split(';').Select(p => p.Trim()).ToList().Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                    if (emailsList.Count() > 0)
                    {
                        ICommonDataContext context = CommonDataContext.GetContext(tenant);

                        myResult = (from d in context.Contacts
                                    where d.Tenant == tenant
                                    && d.Email != null
                                    && emailsList.Contains(d.Email.ToLower())
                                    select new ContactList()
                                    {
                                        Id = d.Id,
                                        Tenant = d.Tenant,
                                        Email = d.Email,
                                        EnglishName = d.EnglishName,
                                        SearchFields = d.SearchFields,
                                    }).ToList();
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetUsersByEmails(string emails)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                List<UserList> myResult = new List<UserList>();
                UserQuery query = new UserQuery(tenant);
                myResult = query.GetUserListsByEmailsString(emails, tenant);


                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetUserListsByidsString(string ids)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                List<UserList> myResult = new List<UserList>();
                UserQuery query = new UserQuery(tenant);
                myResult = query.GetUserListsByidsString(ids, tenant);


                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage PutUserLicenses(UserLicenseUpdateHelper args)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    string token = HttpContext.Current.Request.Headers["Token"];
                    AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                    int tenant = authToken.Tenant;

                    SecurityUtility.AuthenticationOnTenant(tenant);

                    if (args != null)
                    {
                        if (args.Items.Count > 0)
                        {
                            ContactDomainService service = new ContactDomainService();

                            foreach (UserLicensePM entity in args.Items)
                            {
                                if (string.IsNullOrEmpty(entity.Id))
                                {
                                    service.InsertUserLicense(entity);
                                }

                                else
                                {
                                    service.DeleteUserLicense(entity);
                                }
                            }
                        }
                    }

                    scope.Complete();
                    return Request.CreateResponse(HttpStatusCode.OK, args);
                }
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetActivityStatusByMessagesLogs(int lastDays, string showType)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("Airline", "READ", authToken.Tenant);
                CommonDataDomainService service = new CommonDataDomainService();
                List<ChartingDataClass> myResult = service.GetActivityStatusByMessagesLogs(lastDays, showType, authToken.Tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetAirlineDashboardSpotlightCounts(int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("Airline", "READ", authToken.Tenant);
                CommonDataDomainService service = new CommonDataDomainService();
                DailySpotlightClass myResult = service.GetAirlineDashboardSpotlightCounts(tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetDashBoardBookings(int tenant)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("Airline", "READ", authToken.Tenant);
                CommonDataDomainService service = new CommonDataDomainService();
                List<ChartingDataClass> myResult = service.GetDashBoardBookings(tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetTopParticipantsDashBoard(int lastDays)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                string loggedUserEmail = authToken.Email;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                SecurityUtility.CheckContactFeature("Airline", "READ", authToken.Tenant);
                CommonDataDomainService service = new CommonDataDomainService();
                List<ChartingDataClass> myResult = service.GetTopParticipantsDashBoard(lastDays, authToken.Tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetComputingPartnerTranslationsByPartnerAndTableId(string ComputingPartnerId, string ObjectTableId, int tenant)
        {
            try
            {
                CommonDataDomainService commonDomain = new CommonDataDomainService();
                List<ComputingPartnerTranslationPM> myResult = commonDomain.GetComputingPartnerTranslationsByPartnerAndTableId(ComputingPartnerId, ObjectTableId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetCustomerTenantAccessCard(string CustomerId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("CustomerTenantAccess", "READ", tenant);
                CommonDataDomainService service = new CommonDataDomainService();
                CustomerTenantAccessCardPM myResult = service.GetCustomerTenantAccessCard(CustomerId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetSingleCustomerTenantAccess(string CustomerId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("CustomerTenantAccess", "READ", tenant);
                CommonDataDomainService service = new CommonDataDomainService();
                CustomerTenantAccessPM myResult = service.GetSingleCustomerTenantAccess(CustomerId, tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetCustomerTenantAccessCardsBatchPMsByCustomerIdCustomerTenantAccessId(string CustomerId, string CustomerTenantAccessId)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("CustomerTenantAccess", "READ", tenant);
                CommonDataDomainService service = new CommonDataDomainService();
                List<CustomerTenantAccessCardsBatchPM> myResult = service.GetCustomerTenantAccessCardsBatchPMsByCustomerIdCustomerTenantAccessId(CustomerId, CustomerTenantAccessId, tenant).OrderByDescending(d=>d.CreateDateTime).Skip(0).Take(100).ToList();
                foreach (var item in myResult)
                {
                    var queueMessageMoreDetailsQuery = new QueueMessageMoreDetailsQuery(tenant);
                    var AllQueues = queueMessageMoreDetailsQuery.GetIQueryableQueueMessageMoreDetailsPMByField1Field2(item.CustomerId,item.BatchNumber).Where(a => a.QueueDefinitionCode == "ImportersShipmentsBatchQueue");
                    item.TotalFailed = AllQueues.Where(a => a.Status == -1).Count();
                    item.Totalsucceeded = AllQueues.Where(a => a.Status == 1).Count();
                    item.TotalShipment = AllQueues.Count();
                    if (item.TotalFailed > 0)
                    {
                        item.Status = "Failed";
                    }
                    else if(item.TotalShipment == (item.TotalFailed + item.Totalsucceeded))
                    {
                        item.Status = "Done";
                    }
                    else if (item.TotalShipment != (item.TotalFailed + item.Totalsucceeded))
                    {
                        item.Status = "In Progress";
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
       
        public HttpResponseMessage GetSingleVatTypeByCode(string Code)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("VatType", "READ", tenant);

                VatTypeQuery vatTypeQuery = new VatTypeQuery(tenant);
                VatTypePM myResult = vatTypeQuery.GetSinglePMByCode(Code, tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetOnCreatingMexicanTenant()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string tenantString = tenant.ToString();


                DateTime? todayDate = TenantServerConfigration.GetCurrentDateTime(tenant).Date;
                ICommonDataContext myContext = CommonDataContext.GetContext(tenant);

                AccountingSettingRepository myAccountingSettingRepository = new AccountingSettingRepository(myContext);
                AccountingSetting myAccountingSetting = myAccountingSettingRepository.GetSingleAccountingSetting(tenant);
                if (myAccountingSetting != null)
                {
                    myAccountingSetting.EnableMultiPercentageVATTypes = true;
                    myAccountingSettingRepository.Update(myAccountingSetting);
                    myAccountingSettingRepository.SubmitChanges();
                }


                VatTypeRepository myVatTypeRepository = new VatTypeRepository(myContext);
                VatType vatType_STD = myVatTypeRepository.GetSingleVatTypeByCode("STD", tenant);
                VatType vatType_RET = myVatTypeRepository.GetSingleVatTypeByCode("RET", tenant);
                VatType vatType_REIVA = myVatTypeRepository.GetSingleVatTypeByCode("REIVA", tenant);

                if (vatType_RET == null || vatType_REIVA == null)
                {
                    VatTypeService vatTypeService = new VatTypeService(myContext, tenant);

                    string vatType_STD_Id = vatType_STD == null ? null : vatType_STD.Id;
                    string vatType_RET_Id = vatType_RET == null ? null : vatType_RET.Id;

                    if (vatType_RET == null)
                    {
                        VatTypePM vatType_RET_PM = new VatTypePM()
                        {
                            Code = "RET",
                            Tenant = tenant,
                            EnglishName = "Retention",
                            NewEntityPercentage = -4,
                            NewEntityPercentageDate = todayDate,
                        };

                        vatTypeService.Create(vatType_RET_PM);

                        vatType_RET_Id = vatType_RET_PM.Id;
                    }

                    if (vatType_REIVA == null)
                    {
                        VatTypePM vatType_REIVA_PM = new VatTypePM()
                        {
                            Code = "REIVA",
                            Tenant = tenant,
                            EnglishName = "IVA + RET",
                            IsMultiPercentage = true,
                        };

                        if (vatType_STD != null)
                        {
                            VATTypesGroupPM vatGroup_STD = new VATTypesGroupPM()
                            {
                                Tenant = tenant,
                                SingleVATTypeId = vatType_STD.Id,
                            };

                            vatType_REIVA_PM.VatTypeGroups.Add(vatGroup_STD);
                        }

                        VATTypesGroupPM vatGroup_RET = new VATTypesGroupPM()
                        {
                            Tenant = tenant,
                            SingleVATTypeId = vatType_RET_Id,
                        };

                        vatType_REIVA_PM.VatTypeGroups.Add(vatGroup_RET);
                        vatTypeService.Create(vatType_REIVA_PM);
                    }
                }

                bool myResult = true;
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetOnCreatingUSTenant()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string tenantString = tenant.ToString();

                ICommonDataContext myContext = CommonDataContext.GetContext(tenant);
                CustomsInterfaceSettingRepository myCustomsInterfaceSettingRepository = new CustomsInterfaceSettingRepository(myContext);
                CustomsInterfaceSetting myCustomsInterfaceSetting = myCustomsInterfaceSettingRepository.GetSingleCustomsInterfaceSetting(tenant, tenant);

                if (myCustomsInterfaceSetting != null)
                {
                    myCustomsInterfaceSetting.ActivateCustomsManagementInShipments = true;
                    myCustomsInterfaceSettingRepository.Update(myCustomsInterfaceSetting);
                    myCustomsInterfaceSettingRepository.SubmitChanges();
                }

                // Ayman: We need this to update the Cached Tenant & TenantPM
                TenantQuery tenantQuery = new TenantQuery(tenant);
                TenantPM tenantPM = tenantQuery.GetSinglePM(tenant);
                if (tenantPM != null)
                {
                    tenantPM.DateTimeFormat = @"MM\/dd\/yyyy";
                    tenantPM.TenantVATManagement = false;

                    CommonDataDomainService domain = new CommonDataDomainService();
                    domain.UpdateTenantPM(tenantPM);
                }

                string myZeroVatId = null;
                VatTypeRepository myVatTypeRepository = new VatTypeRepository(myContext);
                VatType myVatType = myVatTypeRepository.GetSingleVatTypeByCode("ZERO", tenant);
                if (myVatType != null)
                {
                    myZeroVatId = myVatType.Id;
                }

                ChargesTypeRepository myChargesTypeRepository = new ChargesTypeRepository(myContext);
                List<ChargesType> myChargesTypes = myChargesTypeRepository.GetChargesTypes(tenant).ToList();
                foreach (ChargesType item in myChargesTypes)
                {
                    item.VatTypeId = myZeroVatId;
                    myChargesTypeRepository.Update(item);
                }

                myChargesTypeRepository.SubmitChanges();

                bool myResult = true;
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetOnCreatingMoroccoTenant()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                string tenantString = tenant.ToString();

                ICommonDataContext myContext = CommonDataContext.GetContext(tenant);
                CustomsInterfaceSettingRepository myCustomsInterfaceSettingRepository = new CustomsInterfaceSettingRepository(myContext);
                CustomsInterfaceSetting myCustomsInterfaceSetting = myCustomsInterfaceSettingRepository.GetSingleCustomsInterfaceSetting(tenant, tenant);

                if (myCustomsInterfaceSetting != null)
                {
                    myCustomsInterfaceSetting.ActivateCustomsManagementInShipments = true;
                    myCustomsInterfaceSettingRepository.Update(myCustomsInterfaceSetting);
                    myCustomsInterfaceSettingRepository.SubmitChanges();
                }
                                
                bool myResult = true;
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetOnCreatingIsraelTenant()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                AccountingSettingPM myResult = null;

                ICommonDataContext myContext = CommonDataContext.GetContext(tenant);

                AccountingSettingRepository iAccountingSettingRepository = new AccountingSettingRepository(myContext);
                AccountingSetting iAccountingSetting = iAccountingSettingRepository.GetSingleAccountSetting(tenant);
                if (iAccountingSetting != null)
                {
                    iAccountingSetting.AllowVoidAPI = false;
                    iAccountingSetting.AllowVoidAPP = false;
                    iAccountingSetting.AllowVoidARI = false;
                    iAccountingSetting.AllowVoidARP = false;
                    iAccountingSetting.AllowManualInvoiceNumber = false;
                    iAccountingSetting.IsARInvoiceChronologicalDates = true;
                    iAccountingSetting.IsARPaymentChronologicalDates = true;
                    iAccountingSetting.IsVatNumberMandatoryInAP = true;
                    iAccountingSetting.IsVatNumberMandatoryInAR = true;
                    iAccountingSettingRepository.Update(iAccountingSetting);
                    iAccountingSettingRepository.SubmitChanges();

                    AccountingSettingQuery accountingSettingQuery = new AccountingSettingQuery(iAccountingSettingRepository);
                    myResult = accountingSettingQuery.GetSinglePM(tenant);
                }

                List<string> iCodes = new List<string>();
                iCodes.Add("999S");
                iCodes.Add("999C");
                iCodes.Add("999M");
                iCodes.Add("999CI");
                iCodes.Add("999MP");
                iCodes.Add("999P");
                DocumentTypeRepository iDocumentTypeRepository = new DocumentTypeRepository(myContext);
                List<DocumentType> iDocumentTypes = iDocumentTypeRepository.GetDocumentTypesByCodeLists(iCodes, tenant);
                if (iDocumentTypes.Count > 0)
                {
                    foreach (DocumentType iDocumentType in iDocumentTypes)
                    {
                        List<DocumentTypeCopy> iDocumentTypeCopies = (from d in myContext.DocumentTypeCopies
                                                                      where d.Tenant == tenant && d.DocumentTypeId == iDocumentType.Id
                                                                      select d).ToList();

                        if (iDocumentTypeCopies.Count > 0)
                        {
                            DocumentTypeCopy iDocumentTypeCopy = iDocumentTypeCopies.Where(d => d.Name.ToLower() == "original").FirstOrDefault();
                            if (iDocumentTypeCopy == null)
                            {
                                iDocumentTypeCopy = iDocumentTypeCopies.Where(d => d.Code.ToLower() == iDocumentType.Code.ToLower()).FirstOrDefault();
                            }


                            if (iDocumentTypeCopy != null)
                            {
                                iDocumentType.LimitedPrintCopyId = iDocumentTypeCopy.Id;
                                iDocumentType.IsDocumentOneTimePrintLimited = true;
                                iDocumentTypeRepository.Update(iDocumentType);
                            }
                        }
                    }

                    iDocumentTypeRepository.SubmitChanges();
                }

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetNoneZeroTenantTranslation(string ComputingPartnerId, string ObjectTableId, string Code)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                ComputingPartnerTranslationQuery computingPartnerTranslationQuery = new ComputingPartnerTranslationQuery(authToken.Tenant);
                ComputingPartnerTranslationPM computingPartnerTranslationPM = computingPartnerTranslationQuery.GetTranslationsByPartnerAndTableIdAndCode(ComputingPartnerId, ObjectTableId, Code, authToken.Tenant).FirstOrDefault();
                ComputingPartnerTranslationQuery computingPartnerTranslationZeroQuery = new ComputingPartnerTranslationQuery(0);
                ComputingPartnerTranslationPM computingPartnerTranslationZeroPM = computingPartnerTranslationZeroQuery.GetTranslationsByPartnerAndTableIdAndCode(ComputingPartnerId, ObjectTableId, Code, 0).FirstOrDefault();
                TranslateItemClass item = new TranslateItemClass()
                {
                    Tenant = authToken.Tenant,
                    OurCode = computingPartnerTranslationPM != null ? computingPartnerTranslationPM.OurCode : null,
                    ComputingPartnerId = computingPartnerTranslationPM != null ? computingPartnerTranslationPM.ComputingPartnerId : null,
                    ComputingPartnerName = computingPartnerTranslationPM != null ? computingPartnerTranslationPM.ComputingPartnerName : null,
                    ObjectTableId = computingPartnerTranslationPM != null ? computingPartnerTranslationPM.ObjectTableId : null,
                    ObjectTableName = computingPartnerTranslationPM != null ? computingPartnerTranslationPM.ObjectTableName : null,
                    CreateDate = computingPartnerTranslationPM != null ? computingPartnerTranslationPM.CreateDate : null,
                    UpdateDate = computingPartnerTranslationPM != null ? computingPartnerTranslationPM.UpdateDate : null,
                    CreatedByUserId = computingPartnerTranslationPM != null ? computingPartnerTranslationPM.CreatedByUserId : null,
                    UpdatedByUserId = computingPartnerTranslationPM != null ? computingPartnerTranslationPM.UpdatedByUserId : null,
                    SearchFields = computingPartnerTranslationPM != null ? computingPartnerTranslationPM.SearchFields : null,
                    DefaultTranslationCode = computingPartnerTranslationZeroPM != null ? computingPartnerTranslationZeroPM.PartnerCode : null,
                    PartnerCode = computingPartnerTranslationPM != null ? computingPartnerTranslationPM.PartnerCode : null,
                    Id = computingPartnerTranslationPM != null ? computingPartnerTranslationPM.Id : computingPartnerTranslationZeroPM != null ? computingPartnerTranslationZeroPM.Id : null
                };



                PerformanceLogger.AddServerExecutionTimeHeader(logKey);

                return Request.CreateResponse(HttpStatusCode.OK, item);

            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetHybridTenantThresholdByIdTenant()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(tenant);
                SecurityUtility.CheckContactFeature("HybridTenantThreshold", "READ", tenant);
                CommonDataDomainService service = new CommonDataDomainService();
                HybridTenantThresholdPM myResult = service.GetHybridTenantThresholdByIdTenant(tenant);
                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetDeafaultMyWarehouse()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                var result = "";

                WarehouseQuery warehouseQuery = new WarehouseQuery(tenant);
                var list = warehouseQuery.GetWarehousePMsByTenant(tenant);
                if (list != null)
                {
                    var myWarehouselist = list.Where(a => a.MyWarehouse == true).ToList();
                    if (myWarehouselist != null && myWarehouselist.Count == 1)
                    {
                        result = myWarehouselist.FirstOrDefault().Id;
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetCustomsInterfaceListByTenant()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                CustomsInterfaceSettingQuery query = new CustomsInterfaceSettingQuery(tenant);
                List<CustomsInterfaceSettingList> result = new List<CustomsInterfaceSettingList>();
                var list = query.GetIQueryableEntityListByTenant(tenant);
                if (list != null)
                {
                    result = list.ToList();
                }
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetFilingInboxes([FromUri] ApiQueryFilters filters,string userId, bool isShowDeleted)
        {
            try
            {
                string logKey = PerformanceLogger.LogCurrentTime();
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);


                if (userId == "null" || userId == "undefined") userId = null;
                if (!string.IsNullOrEmpty(userId))
                {
                    UserQuery userQuery = new UserQuery(authToken.Tenant);
                    bool isExist = userQuery.CheckIfUserExistInTenant(userId, authToken.Tenant);
                    if (!isExist)
                    {
                        throw new Exception("Sorry you’re not authenticated to view these documents");
                    }
                }

                QueryOperations queryOperations = new QueryOperations()
                {
                    ObjectTableName = "FilingInbox",
                    PageIndex = filters.PageIndex,
                    PageSize = filters.PageSize,
                    QuerySection = "FilingInboxs",
                    SortByColumnName = filters.SortBy,
                    SortDirectin = filters.SortDirection,
                    GetAll = filters.GetAll,
                };

                List<ObjectField> AirlineObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("FilingInbox", tenant);
                List<PropertyInfo> filterProperties = filters.GetType().GetProperties().ToList();
                for (int i = 1; i <= 10; i++)
                {
                    object filterNameProp = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Name")).GetValue(filters);
                    object filterValue1 = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Value")).GetValue(filters);
                    object filterOperatorProp = filterProperties.FirstOrDefault(f => f.Name == ("Filter" + i + "Operator")).GetValue(filters);
                    object filterValue2 = null;

                    if (filterNameProp != null)
                    {
                        string filterName = filterNameProp.ToString();
                        string filterOperator = filterOperatorProp != null ? filterOperatorProp.ToString() : "Equals";

                        ObjectField field = AirlineObjectFields.FirstOrDefault(f => f.FieldName == filterName);
                        if (field != null)
                        {
                            string valuestring1 = filterValue1 != null ? filterValue1.ToString() : null;
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = filterValue2 != null ? filterValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            queryOperations.SetFilter(filterName, value1, field.IsCustomFilter, filterOperator, value2, field.DisplayInList);
                        }

                        else
                        {
                            queryOperations.SetFilter(filterName, filterValue1, false, filterOperator, filterValue2, true);
                        }
                    }
                }

                if (!string.IsNullOrEmpty(filters.AdditionalFilters))
                {
                    JavaScriptSerializer JsonConvert = new JavaScriptSerializer();
                    var filters_list = JsonConvert.Deserialize<List<QueryFilterItem>>(filters.AdditionalFilters);

                    foreach (QueryFilterItem filter in filters_list)
                    {
                        ObjectField field = AirlineObjectFields.FirstOrDefault(f => f.FieldName == filter.FieldName);
                        if (field != null)
                        {
                            string valuestring1 = filter.FieldValue != null ? filter.FieldValue.ToString() : null;
                            object value1 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring1);

                            string valuestring2 = filter.FieldValue2 != null ? filter.FieldValue2.ToString() : null;
                            object value2 = Logitude.Server.Tools.Helpers.FieldValueResolver.GetFieldDataValue(field, valuestring2);

                            queryOperations.SetFilter(filter.FieldName, value1, field.IsCustomFilter, filter.Operator, value2, field.DisplayInList);
                        }
                        else
                        {
                            queryOperations.SetFilter(filter.FieldName, filter.FieldValue, filter.IsCustom, filter.Operator, filter.FieldValue2, filter.DisplayInList);
                        }
                    }
                }
                ServiceResponse response = new ServiceResponse();
                FilingInboxQuery query = new FilingInboxQuery(tenant);
                List<FilingInboxPM> result = new List<FilingInboxPM>();
                IQueryable<FilingInboxPM> list = query.GetFilingInboxes_LastTwoMonths(userId, isShowDeleted, tenant);
                response.Count = list.Count();

                int skippedEntities = queryOperations.PageIndex;
                list = list.Skip(skippedEntities);
                list = list.Take(queryOperations.PageSize);
                response.Result = list;
                HttpResponseMessage reponseMessage = Request.CreateResponse(HttpStatusCode.OK, response);
                PerformanceLogger.AddServerExecutionTimeHeader(logKey);
                return reponseMessage;
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage GetFilingAttachPdfReport(string documentId)
        {
            try
            {
                byte[] result = null;
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                DocumentRepository documentRepository = new DocumentRepository(tenant);
                Document document = documentRepository.GetSingleDocument(tenant, documentId);
                if (document != null)
                {
                    IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                    BlobFileInfo fileInfo = new BlobFileInfo()
                    {
                        FileName = document.Id,
                        FolderName = document.Folder,
                        Extension = document.Extension,
                        Tenant = tenant,
                        FileSize = document.FileSize,
                    };
                    result = storageservice.Read(fileInfo);
                }
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        public HttpResponseMessage PutFilingInboxLogs(FilingInboxSummary summary)
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);

                ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
                DocumentTypeRepository docRep = new DocumentTypeRepository(tenant);

                // Update FilingInbox
                FilingInboxRepository myFilingInboxRepository = new FilingInboxRepository(commonContext);
                FilingInbox myFilingInbox = myFilingInboxRepository.GetSingleFilingInbox(summary.FilingId, tenant);
                myFilingInbox.IsDeleted = summary.IsDeleted;
                myFilingInboxRepository.Update(myFilingInbox);

                FilingInboxAttachmentQuery myFilingInboxAttachmentQuery = new FilingInboxAttachmentQuery(tenant);
                if (summary.Attaches != null && summary.Attaches.Count() > 0)
                {
                    FilingInboxAttachmentLogRepository logRepository = new FilingInboxAttachmentLogRepository(commonContext);
                    string updatedByUserId = summary.UserId;
                    if (updatedByUserId == "null" || updatedByUserId == null)
                    {
                        ContactRepository contactRepository = new ContactRepository(tenant);
                        Contact contact = contactRepository.GetContactByUserTypeAndTenant("S", tenant);
                        updatedByUserId = contact.Id;
                    }

                    ObjectTableRepository objectTabletRepository = new ObjectTableRepository(tenant);
                    var objectTable = objectTabletRepository.GetObjectTableByName(summary.ObjectTableName, tenant, false);

                    DocumentRepository myDocumentRepository = new DocumentRepository(tenant);
                    Document myDocument = new Document();

                    foreach (var item in summary.Attaches)
                    {
                        myDocument = myDocumentRepository.GetSingleDocument(tenant, item.DocumentId);

                        int len = (int)myDocument.FileSize;
                        Byte[] mybytearray = new Byte[len];

                        IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                        BlobFileInfo fileInfo = new BlobFileInfo()
                        {
                            FileName = myDocument.Id,
                            FolderName = myDocument.Folder,
                            Extension = myDocument.Extension,
                            Tenant = tenant,
                            FileSize = myDocument.FileSize,
                        };
                        byte[] buffer = storageservice.Read(fileInfo);
                        Stream stream = new MemoryStream(buffer);
                        stream.Read(mybytearray, 0, len);

                        var entityId = summary.EntityId;
                        var entityNumber = summary.EntityNumber;

                        if (!string.IsNullOrEmpty(item.House))
                        {
                            entityId = item.House;
                            entityNumber = item.HouseNumber;
                        }
                        string loggedUserEmail = null;
                        if (item.IsDigitallySign)
                        {
                            loggedUserEmail = authToken.Email;
                        }
                        DocumentsFilingPM documentInPM = new DocumentsFilingPM()
                        {
                            DirectionCode = "I",
                            Tenant = tenant,
                            EntityId = entityId,
                            EntityNumber = entityNumber,
                            DocumentTypeId = item.DocumentType,
                            ObjectTableId = objectTable.Id,
                            CreatedByUserId = updatedByUserId,
                            CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                            OwnerId = updatedByUserId,
                            UpdatedByUserId = updatedByUserId,
                            UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
                            FileSize = (int)myDocument.FileSize,
                            Folder = "docsin",
                            HasFile = true,
                            FileName = item.FileName.Contains('.') == false ? item.FileName : item.FileName.Split('.')[0],
                            FileExtension = item.FileName.Contains('.') == false ? "" : item.FileName.Split('.')[1],
                            FileData = mybytearray,
                            Description = item.Description,
                            IsSharedWithForwarder = item.IsSharedWithAgent,
                            SignRequestByUserEmail = loggedUserEmail,
                            DontAddToQueue = false,
                        };
                        if (documentInPM.FileExtension != null && documentInPM.FileExtension.ToLower() == "pdf")
                        {
                            List<Dictionary<string, string>> signatures = new List<Dictionary<string, string>>();
                            string message = "";
                            bool success = this.GetSignatureMetadata(item.FileName, mybytearray, out signatures, out message);
                            if (success)
                            {
                                string Signerslist = "";
                                if (signatures.Count > 0)
                                {
                                    for (int i = 0; i < signatures.Count; i++)
                                    {
                                        try
                                        {
                                            Signerslist += signatures[i]["CN"] + ',';
                                            Signerslist += "Vat: " + signatures[i]["O"] + ',';
                                        }
                                        catch (Exception exc)
                                        {

                                        }

                                    }
                                    documentInPM.SignersList = Signerslist.TrimEnd(',');
                                    documentInPM.IsDigitallySigned = true;
                                }
                            }
                        }
                        ShipmentRepository ShipmentRepo = new ShipmentRepository(tenant);
                        var myshipment = ShipmentRepo.GetSingleShipment(documentInPM.EntityId, tenant);
                        if (myshipment != null && string.IsNullOrEmpty(myshipment.ForwarderShipmentNumber))
                        {
                            DocumentsMetaDataTypeRepository DocumentsMetaDataTypeRepo = new DocumentsMetaDataTypeRepository(tenant);
                            var LBO = DocumentsMetaDataTypeRepo.GetSingleDocumentsMetaDataTypeByCode("LBO", tenant);
                            var LBF = DocumentsMetaDataTypeRepo.GetSingleDocumentsMetaDataTypeByCode("LBF", tenant);
                            if (LBF != null)
                            {
                                DocumentsFilingMetaDataValuePM value1 = new DocumentsFilingMetaDataValuePM();
                                value1.ChangeSetOp = ChangeSetOperation.Insert;
                                value1.DocumentsFilingId = documentInPM.Id;
                                value1.MetaDataValue = myshipment.TransportModeId + myshipment.ShipmentNumber;
                                value1.DocumentsMetaDataTypeId = LBF.Id;
                                value1.Tenant = documentInPM.Tenant;
                                documentInPM.DocumentsFilingMetaDataValues.Add(value1);
                            }
                            if (LBO != null)
                            {
                                DocumentsFilingMetaDataValuePM value2 = new DocumentsFilingMetaDataValuePM();
                                value2.ChangeSetOp = ChangeSetOperation.Insert;
                                value2.DocumentsFilingId = documentInPM.Id;
                                value2.MetaDataValue = myshipment.CustomerReference1;
                                value2.DocumentsMetaDataTypeId = LBO.Id;
                                value2.Tenant = documentInPM.Tenant;
                                documentInPM.DocumentsFilingMetaDataValues.Add(value2);
                            }
                        }
                       
                        DocumentsFilingService service = new DocumentsFilingService(commonContext, tenant);
                        service.Create(documentInPM, mybytearray, updatedByUserId, false, myDocument.Id);
                        myDocument.FileName = documentInPM.FileName;
                        myDocumentRepository.Update(myDocument);
                        myDocumentRepository.SubmitChanges();
                        FilingInboxAttachmentLog myFilingInboxAttachmentLog = new FilingInboxAttachmentLog()
                        {
                            Id = IdCounter.GetNumber("FilingInboxAttachmentLog", tenant).ToString(),
                            Tenant = tenant,
                            DocumentsFilingId = documentInPM.Id,
                            FilingInboxAttachmentId = item.EntityId,
                        };
                        logRepository.Add(myFilingInboxAttachmentLog);
                    }

                    logRepository.SubmitChanges();
                }
                return Request.CreateResponse(HttpStatusCode.OK, "");
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }
        private bool GetSignatureMetadata(string filename, byte[] filedata, out List<Dictionary<string, string>> signatures, out string message)
        {
            signatures = new List<Dictionary<string, string>>();
            message = "";
            try
            {
                using (PdfReader reader = new PdfReader(filedata))
                {
                    AcroFields af = reader.AcroFields;
                    var names = af.GetSignatureNames();
                    for (int i = 0; i < names.Count; ++i)
                    {
                        Dictionary<string, string> metadata = new Dictionary<string, string>();
                        String name = (string)names[i];
                        PdfPKCS7 pk = af.VerifySignature(name);
                        metadata.Add("SignDate", pk.SignDate.ToString("dd.MM.yyyy HH:mi.ss"));
                        var subjectFields = CertificateInfo.GetSubjectFields(pk.SigningCertificate);
                        List<string> mdlist = new List<string>() { "C", "CN", "SN", "T", "OU", "O", "GIVENNAME", "SURNAME", };
                        if (subjectFields != null)
                        {
                            foreach (var md in mdlist)
                            {
                                string value = subjectFields.GetField(md);
                                if (!string.IsNullOrEmpty(value))
                                    metadata.Add(md, value);
                            }
                        }
                        signatures.Add(metadata);
                    }
                }
                return (true);
            }
            catch (Exception ex)
            {
                message = "Failed to get metadata from pdf file '" + filename + "'" + Environment.NewLine + ex.ToString();
                return (false);
            }
        }

        public HttpResponseMessage GetLoggedTenantDB()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                int tenant = authToken.Tenant;

                TenantRepository entityRepository = new TenantRepository(tenant);
                TenantQuery entityQuery = new TenantQuery(entityRepository);
                TenantPM myResult = entityQuery.GetTenantFromDB(tenant);

                return Request.CreateResponse(HttpStatusCode.OK, myResult);
            }

            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetSignRequestReceived()
        {
            try
            {
                string token = HttpContext.Current.Request.Headers["Token"];
                AuthenticationToken authToken = AuthenticationTokenRepository.GetSingleTokenFromCache(token);
                SecurityUtility.AuthenticationOnTenant(authToken.Tenant);
                string result = null;
                if (LogitudeCacheManager.ServerCache != null)
                {
                    var CachedData = LogitudeCacheManager.ServerCache.GetFromCache("ClientAppStatus_" + authToken.Email);
                    if (CachedData != null)
                    {
                        var data = JsonConvert.DeserializeObject<StatusData>(CachedData);
                        if (data != null && data.IsActive == true && data.IsLogged == true && data.IsValidCert == true)
                        {

                        }
                        else // if (data == null || !data.IsActive || !data.IsLogged || !data.IsValidCert)
                        {
                            result = "error";
                        }
                    }
                }
                
                return Request.CreateResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }
        }

        public HttpResponseMessage GetTenantLogoUri(int tenant)
        {
            try
            {

                string result = "";
                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = "logo" + tenant,
                    FolderName = "logos",
                    Extension = "jpg",
                    Tenant = tenant,
                };

                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                byte[] datainByte = storageservice.Read(fileInfo);


                if (datainByte != null)
                {
                    int height = LogitudeSettings.WorkEnvironment != "cloud" && tenant == 1245 ? 170 : 114;

                    datainByte = ResizeImage(datainByte, 290, height, "jpg");
                    string base64String = System.Convert.ToBase64String(datainByte, 0, datainByte.Length);
                    result = "data:image/jpg;base64," + base64String;
                    return Request.CreateResponse(HttpStatusCode.OK, result); 
                }

                else return null;


            }

            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, tenant, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "BrandingController : GetTenantLogoUri Method", null);
                return null;
            }


        }

        #region ResizeTenantImage
        private byte[] ResizeImage(byte[] image, int width, int height, string extension)
        {
            using (var stream = new System.IO.MemoryStream(image))
            {
                var img = Image.FromStream(stream);

                float nPercentW = 1;
                float nPercentH = 1;

                if (img.Height > height) nPercentH = ((float)height / (float)img.Height);

                if (img.Width > width) nPercentW = ((float)width / (float)img.Width);

                if (nPercentH != 1 && nPercentW != 1)
                {
                    var thumbnail = FixedSize(img, width, height);

                    System.Drawing.Imaging.EncoderParameters param = new System.Drawing.Imaging.EncoderParameters(1);
                    ImageCodecInfo myImageCodecInfo;

                    var Quality = 90L;
                    param.Param[0] = new System.Drawing.Imaging.EncoderParameter(System.Drawing.Imaging.Encoder.Quality, Quality);


                    using (var thumbStream = new System.IO.MemoryStream())
                    {
                        if (extension == "jpg")
                        {
                            myImageCodecInfo = GetEncoderInfo("image/jpeg");
                            thumbnail.Save(thumbStream, myImageCodecInfo, param); //thumbnail.Save(thumbStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                        }
                        else if (extension == "png")
                        {
                            myImageCodecInfo = GetEncoderInfo("image/png");
                            thumbnail.Save(thumbStream, myImageCodecInfo, param);


                            // thumbnail.Save(thumbStream, System.Drawing.Imaging.ImageFormat.Png);
                        }

                        return thumbStream.GetBuffer();
                    }
                }
                else
                {
                    return image;
                }


            }

        }
        private Image FixedSize(Image imgPhoto, int Width, int Height)
        {

            try
            {
                int sourceWidth = imgPhoto.Width;
                int sourceHeight = imgPhoto.Height;
                int sourceX = 0;
                int sourceY = 0;
                int destX = 0;
                int destY = 0;

                float nPercent = 0;
                float nPercentW = 1;
                float nPercentH = 1;



                if (sourceHeight > Height) nPercentH = ((float)Height / (float)sourceHeight);

                if (sourceWidth > Width) nPercentW = ((float)Width / (float)sourceWidth);

                if (nPercentH != 1 && nPercentW != 1)
                {
                    if (nPercentH < nPercentW)
                    {
                        nPercent = nPercentW;
                        destX = System.Convert.ToInt16((Width -
                                      (sourceWidth * nPercent)) / 2);
                    }
                    else
                    {
                        nPercent = nPercentH;
                        destY = System.Convert.ToInt16((Height -
                                      (sourceHeight * nPercent)) / 2);
                    }
                }
                else
                {

                    return imgPhoto;
                    //nPercent = nPercentW;
                    //destX = System.Convert.ToInt16((Width -
                    //              (sourceWidth * nPercent)) / 2);

                    //nPercent = nPercentH;
                    //destY = System.Convert.ToInt16((Height -
                    //              (sourceHeight * nPercent)) / 2);
                }



                if (destX < 0) destX = 0;
                if (destY < 0) destY = 0;
                int destWidth = (int)(sourceWidth * nPercentW);
                int destHeight = (int)(sourceHeight * nPercentH);

                Bitmap bmPhoto = new Bitmap(Width, Height,
                                  PixelFormat.Format24bppRgb);
                bmPhoto.SetResolution(imgPhoto.HorizontalResolution,
                                 imgPhoto.VerticalResolution);

                Graphics grPhoto = Graphics.FromImage(bmPhoto);
                grPhoto.Clear(Color.White);
                grPhoto.InterpolationMode =
                        InterpolationMode.HighQualityBicubic;

                grPhoto.DrawImage(imgPhoto,
                    new Rectangle(destX, -((destWidth - Height) / 2), destWidth, destWidth),
                    new Rectangle(sourceX, sourceY, sourceWidth, sourceHeight),
                    GraphicsUnit.Pixel);

                grPhoto.Dispose();
                return bmPhoto;
            }
            catch (Exception ex)
            {
                return imgPhoto;
            }
        }

        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        public byte[] CreateImageThumbnail(byte[] image, int width = 50, int height = 50)
        {
            using (var stream = new System.IO.MemoryStream(image))
            {
                var img = Image.FromStream(stream);
                var thumbnail = img.GetThumbnailImage(width, height, () => false, IntPtr.Zero);

                using (var thumbStream = new System.IO.MemoryStream())
                {
                    thumbnail.Save(thumbStream, System.Drawing.Imaging.ImageFormat.Jpeg);
                    return thumbStream.GetBuffer();
                }
            }
        }
        #endregion


    }
}

public class StatusData
{
    public int Tenant { get; set; }
    public bool IsActive { get; set; }
    public DateTime LastStatusDate { get; set; }
    public bool IsLogged { get; set; }
    public string LoggedByUserEmail { get; set; }
    public bool IsValidCert { get; set; }
}


public class BluesnapParameters
{
    public string Token { get; set; }
    public string Storeid { get; set; }
    public string ContractId { get; set; }

}


public class FilingInboxSummary
{
    public string EntityId { get; set; }
    public string EntityNumber { get; set; }
    public string FilingId { get; set; }
    public string DocumentTypeId { get; set; }
    public string ObjectTableName { get; set; }
    public string UserId { get; set; }
    public bool IsDeleted { get; set; }
    public FilingInboxAttachItem[] Attaches { get; set; }
}

public class FilingInboxAttachItem
{
    public string EntityId { get; set; }
    public string DocumentType { get; set; }
    public string House { get; set; }
    public string HouseNumber { get; set; }
    public string DocumentId { get; set; }
    public string FileName { get; set; }
    public string Description { get; set; }
    public bool IsSharedWithAgent { get; set; }
    public bool IsDigitallySign { get; set; }
}


