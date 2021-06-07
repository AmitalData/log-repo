using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Web;
using System.Web.Http;
using System.Web.Security;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using WebFreight.Web.DataContracts;
using WebFreight.Web.GlobalModel;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Logitude.BL.ShipmentsModel.EntityLists;
using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.InfrastructureModel.EntityPMs;
using Logitude.BL.InfrastructureModel.EntityQueries;
using Simplog.Server.Infrastructure.Azure;
using Microsoft.WindowsAzure.Storage.Blob;
using Logitude.SystemLogs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Logitude.Server.Tools.StorageService;
using Logitude.Server.Tools;
using Microsoft.Practices.Unity;
using Logitude.Infrastructure.Data.Repsitories;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Server.Tools.Helpers;
using System.Runtime.Remoting.Messaging;
using Logitude.Infrastructure.BL.EntityQueryServices;

namespace WebFreight.Web
{
    public class CommonDataController : ApiController
    {

        public List<GlobalContact> GetTenants(string email)
        {

             
            GlobalContactRepository globalContactsRepository = new GlobalContactRepository();
            List<GlobalContact> globalContacts = globalContactsRepository.GetContactByEmail(email).Where(c=>c.IsUser == true || c.InternetAccess==true).ToList();
            GlobalContact cc = new GlobalContact();
           
            return globalContacts;
        }

        public string GetTenantLogoUri(int companyId)     
        {
            try
            {
                SharedLogisticsSettingQueryService sharedLogisticsSettingQueryService = new SharedLogisticsSettingQueryService(companyId);
                string uri = sharedLogisticsSettingQueryService.GetTenantLogoUri(companyId, true);

                return uri;
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, companyId, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "Uploader : DownloadFile Method", null);
                return null;
            }
        }

        public string GetTenantCompanyLogoUri(int companyId, bool isSmalLogo)
        {
            try
            {
                SharedLogisticsSettingQueryService sharedLogisticsSettingQueryService = new SharedLogisticsSettingQueryService(companyId);
                string uri = sharedLogisticsSettingQueryService.GetTenantLogoUri(companyId, isSmalLogo);

                return uri;
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, companyId, User != null ? User.Identity.Name : "", User != null ? User.Identity.Name : "", "Uploader : DownloadFile Method", null);
                return null;
            }
        }

        //public static bool ValidateUser(string username, string password)
        //{
        //    var userRepository = new UserRepository();

        //    User user = userRepository.GetUserByUsername(username);
        //    if (user == null)
        //    {
        //        return false;
        //    }

        //    if (String.Compare(user.Password, password, false) == 0)
        //    {
        //        var authTicket = new FormsAuthenticationTicket(1, username, DateTime.Now,
        //                                                       DateTime.Now.AddMinutes(30), true, "");

        //        string cookieContents = FormsAuthentication.Encrypt(authTicket);
        //        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, cookieContents)
        //        {
        //            Expires = authTicket.Expiration,
        //            Path = FormsAuthentication.FormsCookiePath
        //        };
        //        if (HttpContext.Current != null)
        //        {
        //            HttpContext.Current.Response.Cookies.Add(cookie);
        //        }
        //        return true;
        //    }
        //    return false;
        //}
        //public IEnumerable<string> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        //// GET api/<controller>/5
        //public ShipmentPM Get(string id)
        //{

        //    return null;
        //}


        //// POST api/<controller>
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT api/<controller>/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/<controller>/5
        //public void Delete(int id)
        //{
        //}

        public List<TraceEventPM> GetEntityEvents(string entityId, string objectTableName, string partnerType, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);

            List<TraceEventPM> result = new List<TraceEventPM>();
           
            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName(objectTableName, 0, true);
            if (objectTable != null)
            {
                string objectTableId = objectTable.Id;

                TraceEventRepository traceEventsRepository = new TraceEventRepository(tenant);
                TraceEventQuery traceEventQuery = new TraceEventQuery(traceEventsRepository);

                List<TraceEventPM> data = traceEventQuery.GetTraceEventPMsByTenantByEntityId(tenant, entityId, objectTableId).ToList();

                if (partnerType == "AG")
                {
                    result = data.Where(d => d.IsAgentView).ToList();
                }

                else //if (partnerType == "CS") commented: if view from external link there will be no partner type and we want to show customer view events
                {
                    result = data.Where(d => d.IsCustomerView).ToList();
                }

            }

            return result.OrderByDescending(s => s.EventDateTime).ToList();
        }

        public List<TraceEventPM> GetShipmentEvents(string securitykey, string entityId, string objectTableName, string partnerType, int tenant)
        {
   
            List<TraceEventPM> result = new List<TraceEventPM>();

            ObjectTableRepository objectTabelRepository = new ObjectTableRepository(tenant);
            ObjectTable objectTable = objectTabelRepository.GetObjectTableByName(objectTableName, 0, true);
            if (objectTable != null)
            {
                string objectTableId = objectTable.Id;

                TraceEventRepository traceEventsRepository = new TraceEventRepository(tenant);
                TraceEventQuery traceEventQuery = new TraceEventQuery(traceEventsRepository);

                var traceEvents = traceEventQuery.GetTraceEventPMsByTenantAndEntityId_WithSecurityKey(tenant, entityId, objectTableId, securitykey);
                if (traceEvents != null)
                {
                    List<TraceEventPM> traceEventsList = traceEvents.ToList();
                    if (partnerType == "AG")
                    {
                        result = traceEventsList.Where(d => d.IsAgentView).ToList();
                    }

                    else
                    {
                        result = traceEventsList.Where(d => d.IsCustomerView).ToList();
                    }

                }
            }

            return result.OrderByDescending(s => s.EventDateTime).ToList();
        }
        public SharedLogisticLoggedData GetLoggingData(string email, int tenant, string cardId)
        {
            SharedLogisticLoggedData myResult = new SharedLogisticLoggedData();

            ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenant);
            CardRepository cardRepository = new CardRepository(commonDataContext);
            ContactRepository contactRepository = new ContactRepository(commonDataContext);
            TenantRepository tenantRepository = new TenantRepository(commonDataContext);
            CurrencyRepository currencyRepository = new CurrencyRepository(commonDataContext);

            Card card = cardRepository.GetSingleCard(cardId, tenant);
            Tenant myTenant = tenantRepository.GetSingleTenant(tenant);
            Contact myContact = contactRepository.GetSingleContactByEmail(email,tenant);

            if (card != null)
            {
                myResult.CardName = card.EnglishName;
            }

            if (myContact != null)
            {
                myResult.ContactName = myContact.EnglishName;
                myResult.ContactId = myContact.Id;
            }

            if (myTenant != null)
            {
                myResult.TenantCompany = myTenant.Company;
                myResult.LocalCurrencyCode = myTenant.Currency.Code;
                myResult.ProfitCurrencyCode = myTenant.ProfitCurrency.Code;
                myResult.TenantDateTimeFormat = myTenant.DateTimeFormat;
                myResult.DisplayDocumentsAndEvents = myTenant.DisplayDocumentsAndEvents;
                myResult.IsQuotesRequestsMenuEnabled = myTenant.IsQuotesRequestActivatedInShared;
            }

            SharedLogisticsSettingRepository sharedLogisticsSettingRepository = new SharedLogisticsSettingRepository(tenant);
            SharedLogisticsSetting sharedLogisticsSetting = sharedLogisticsSettingRepository.GetSingle(tenant.ToString(), tenant);
            if (sharedLogisticsSetting != null)
            {
                myResult.IsInvoicesMenuEnabled = sharedLogisticsSetting.IsInvoicesMenuEnabled;
                myResult.IsAgentShared = sharedLogisticsSetting.IsAgentShared;
                myResult.IsShipperShared = sharedLogisticsSetting.IsShipperShared;
                myResult.IsConsigneeShared = sharedLogisticsSetting.IsConsigneeShared;
            }

            return myResult;
        }

        public string GetSettingWorkEnvironment(string settingId)
        {
            string workEnvironment = null;

            SettingRepository mySettingRepository = new SettingRepository();
            Setting mySetting = mySettingRepository.GetSingleSetting(settingId);
            if (mySetting != null)
            {
                workEnvironment = mySetting.WorkEnvironment;
            }

            return workEnvironment;
        }
    }
}