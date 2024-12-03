using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.SystemLogs;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Transactions;
using System.Web;
//using WebFreight.Web.WebServices;

namespace Logitude.BL.Helpers
{
    public class AmitalCloudActivityLog
    {
        private static string GetConnection(int tenant)
        {
            GlobalDBRepository globalDbRep;
            GlobalDB currentDb;
            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                //GlobalDBRep = new GlobalDBRepository();
                currentDb = GlobalDBRepository.GetGlobalDBByTenant(tenant);

            }

            string dbConnectionInfo = currentDb.DBConnection;
            string dbSeconderyConnectionInfo = currentDb.SecondaryAzureDBConnection;

            DbConnection connection = DatabaseInitializer.GetConnection(dbConnectionInfo, dbSeconderyConnectionInfo);
            WebFreightContext context = new WebFreightContext(connection);

            return context.Database.Connection.ConnectionString;// entityBuilder.ConnectionString;
        }
  
        public static void AddAcitivityLog(object entityId,string objectTableId,int tenant,string activityTypeCode,string userId)
        {
            try
            {
                string strConnString = GetConnection(tenant);

                if (entityId != null)
                {
                    string query = "INSERT INTO EntityLastActivities (Id, ActivityDate, ActivityTypeCode, EntityId, ObjectTableId, Tenant,UserId) " +
                  "VALUES (@Id, @ActivityDate, @ActivityTypeCode, @EntityId, @ObjectTableId, @Tenant, @UserId) ";

                    using (SqlConnection cn = new SqlConnection(strConnString))
                    {
                        SqlCommand cmd = new SqlCommand(query, cn);
                        cmd.Parameters.Add("@Id", SqlDbType.VarChar, 50).Value = IdCounter.GetNumber("EntityLastActivity", tenant);
                        cmd.Parameters.Add("@ActivityDate", SqlDbType.DateTime).Value = TenantServerConfigration.GetCurrentDateTime(tenant);
                        cmd.Parameters.Add("@ActivityTypeCode", SqlDbType.VarChar, 50).Value = activityTypeCode;
                        cmd.Parameters.Add("@EntityId", SqlDbType.VarChar, 50).Value = entityId.ToString();
                        cmd.Parameters.Add("@ObjectTableId", SqlDbType.VarChar, 50).Value = objectTableId;
                        cmd.Parameters.Add("@Tenant", SqlDbType.Int).Value = tenant;
                        cmd.Parameters.Add("@UserId", SqlDbType.VarChar, 50).Value = userId;
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 5;
                        cn.Open();
                        var output = cmd.ExecuteNonQuery();
                        cn.Close();
                    }

                }
            }
            catch { }
        }

        public static void SendTotangoContactActivity(string email, string module, string activity, int tenant, bool isSharedLogisticsContact, string cardId, string via)
        {
            try
            {
                Contact loggedContact = null;
                User loggedUser = null;
                ICommonDataContext commonDataContext;
                var isDemoTenant = false;
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())//TransactionFactory.GetNewTransaction())
                {
                    commonDataContext = CommonDataContext.GetContext(tenant);
                    loggedContact = commonDataContext.Contacts.Where(c => c.Email == email && c.Tenant == 0).FirstOrDefault();
                    if (loggedContact != null)
                    {
                        loggedUser = commonDataContext.Users.Where(c => c.Id == loggedContact.Id && c.Tenant == 0).FirstOrDefault();
                    }

                   
                    scope.Complete();
                }

                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    SettingRepository mySettingRepository = new SettingRepository();
                    isDemoTenant = mySettingRepository.IsDemoTenant(tenant.ToString());
                    scope.Complete();
                }

                commonDataContext = CommonDataContext.GetContext(tenant);
                if (loggedContact == null)
                {

                    loggedContact = commonDataContext.Contacts.Where(c => c.Email == email && c.Tenant == tenant).FirstOrDefault();
                    loggedUser = commonDataContext.Users.Where(c => c.Id == loggedContact.Id && c.Tenant == tenant).FirstOrDefault();
                }
                TenantRepository rep = new TenantRepository(commonDataContext);
                TenantQuery tenantQuery = new TenantQuery(rep);
                TenantPM currentTenant = tenantQuery.GetSinglePM(tenant);
                //TotangoService service = new TotangoService();
                string orgDisplayName = currentTenant.Company + (currentTenant.CountryName != null ? ("-" + currentTenant.CountryName.Trim()) : "");
                string organizationId = tenant.ToString();
                if (isDemoTenant || tenant == 153)
                {
                    orgDisplayName = loggedUser.Notes;
                    organizationId = loggedUser.Id;
                }

                if (isSharedLogisticsContact)
                {
                    Card card = commonDataContext.Cards.Where(d => d.Id == cardId & d.Tenant == tenant).FirstOrDefault();
                    AmitalCloudActivityLog.AddContactActivityWithTotango(organizationId, orgDisplayName, "External Contact", module, activity, loggedContact.Id, tenant, true, cardId, card.PartnerTypeId,via);
                }
                else
                {
                    AmitalCloudActivityLog.AddContactActivityWithTotango(organizationId, orgDisplayName, loggedContact.EnglishName, module, activity, loggedContact.Id, tenant, false, null, null,via);
                }
            }
            catch (Exception ex)
            {

            }
        }


        public static void AddContactActivityWithTotango(string organizationId, string orgDisplayName, string userName, string module, string activity, string contactId, int tenant, bool isSharedLogisticsContact, string cardId, string partnerTypeId, string via)
        {
            if (LogitudeSettings.DeploymentStage != "Dev" && !LogitudeSettings.IsCostomsDeploy)
            {
                try
                {
                    UserQuery userQuery = new UserQuery(tenant);
                    UserPM user = userQuery.GetSingleUserPM(contactId, tenant, false);
                    if (user != null && !user.IsCustomerCare)
                    {
                        orgDisplayName = !string.IsNullOrEmpty(orgDisplayName) ? orgDisplayName.Replace('&', '-') : "No Company";
                        activity = !string.IsNullOrEmpty(activity) ? activity.Replace('&', '-') : "No Activity";

                        string totangoService = LogitudeSettings.TotangoServiceId;//ConfigurationManager.ConnectionStrings["TotangoServiceId"].ConnectionString;//"SP-11460-01";//

                        if (!string.IsNullOrEmpty(totangoService))
                        {
                            if (!String.IsNullOrEmpty(orgDisplayName))
                            {
                                if (orgDisplayName.Length > 125)
                                {
                                    orgDisplayName = orgDisplayName.Take(125).ToString();
                                }
                            }

                            //test serviceId :SP-11460-01 test
                            //production ServiceId: SP-1146-01
                            //string serivceId = "SP-11460-01";

                            //if (!String.IsNullOrEmpty(totangoService))
                            //{
                            //    serivceId = totangoService;
                            //}
                            //http://sdr.totango.com/pixel.gif/?sdr_s=SP-XXXX-XX&sdr_o=ORGANIZATION&sdr_u=USERNAME&sdr_a=ACTIVITY&sdr_m=MODULE&sdr_odn=ORG_DISPLAY_NAME

                            WebClient wc = new WebClient();
                            string sRequest = "http://sdr.totango.com/pixel.gif/?sdr_s=" + totangoService + "&sdr_o=" + organizationId + "&sdr_u=" + userName + "&sdr_a=" + activity + "&sdr_m=" + module + "&sdr_odn=" + orgDisplayName;

                            string ipstring = LogitudeSettings.CustomerCareIP;//System.Configuration.ConfigurationManager.AppSettings.Get("CustomerCareIP");
                            string[] authenticatedIPs = ipstring.Split(',');
                            string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                            if (string.IsNullOrEmpty(currentIP))
                            {
                                currentIP = HttpContext.Current.Request.UserHostAddress;
                            }
                            if (!authenticatedIPs.Contains(currentIP))
                            {
                                wc.DownloadString(new Uri(sRequest));
                            }
                        }
                }

                     
                    
            }
            catch (Exception ex)
            {
                    string ip = "";
                    if (HttpContext.Current != null && HttpContext.Current.Request != null)
                    {
                        string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                        if (string.IsNullOrEmpty(currentIP))
                        {
                            currentIP = HttpContext.Current.Request.UserHostAddress;
                        }
                        ip = currentIP;
                    }
                    ExceptionHandler.HandleException(ex, DateTime.Now, 0, HttpContext.Current != null ? HttpContext.Current.User.Identity.Name : "", HttpContext.Current != null ? HttpContext.Current.User.Identity.Name : "", "SendUserActivity", ip);
                }
            }

            AddContactActivityLog(cardId, partnerTypeId, contactId, module, activity, tenant, isSharedLogisticsContact,via);


            }


        public static void AddContactActivityLog(string cardId, string partnerTypeId, string contactId, string module, string activity, int tenant, bool isSharedLogisticsContact , string via)
        {
            try
            {
                TotangoActivityLogger.AddContactActivityLog(cardId, partnerTypeId, contactId, module, activity, tenant, isSharedLogisticsContact,via);
               
                //ContactActivityLogRepository contactActivityLogRepository = new ContactActivityLogRepository();
                //ContactActivityLog log = new ContactActivityLog()
                //{
                //    Id = Guid.NewGuid().ToString(),
                //    ContactId = contactId,
                //    Module = module,
                //    Activity = activity,
                //    LogDateTime = TenantServerConfigration.GetCurrentDateTime(tenant),
                //    GMTLogDateTime = DateTime.Now,
                //    Tenant = tenant,
                //    IsSharedLogisticsContact = isSharedLogisticsContact,
                //    CardId = cardId,
                //    PartnerTypeId = partnerTypeId,
                //    Via = via,
                //};

                //contactActivityLogRepository.Add(log);
                //contactActivityLogRepository.SubmitChanges();
            }
            catch (Exception ex)
            {
                string ip = "";
                if (HttpContext.Current != null && HttpContext.Current.Request != null)
                {
                    string currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                    if (string.IsNullOrEmpty(currentIP))
                    {
                        currentIP = HttpContext.Current.Request.UserHostAddress;
                    }
                    ip = currentIP;
                }
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, HttpContext.Current != null ? HttpContext.Current.User.Identity.Name : "", HttpContext.Current != null ? HttpContext.Current.User.Identity.Name : "", "SendUserActivity", ip);
            }
        }  

    }
}