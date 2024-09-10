using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;

using Logitude.SystemLogs;
using Logitude.SystemLogs.POCOs;
using Logitude.SystemLogs.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;

using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using System.Data.SqlClient;
using System.Data;
using Simplog.Data.InfrastructureModel;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System.Data.Common;
using System.Transactions;

namespace Logitude.Server.Tools.Helpers
{
    public class TotangoActivityLogger
    {
        public static void SendUserActivity(string organizationId, string orgDisplayName, string userName, string module, string activity, string email, int tenant, bool isSharedLogisticsContact, string cardId, string partnerTypeId)
        {
            if (!SettingUtil.DeploymentStage.IsDBStage(SettingUtil.DeploymentStage.Development) && !LogitudeSettings.IsCostomsDeploy)
            {
                ContactRepository contactRepository = new ContactRepository(tenant);
                Contact contact = contactRepository.GetSingleContactByEmail(email, tenant);


                try
                {
                    UserRepository userRepository = new UserRepository(tenant);
                    User user = userRepository.GetSingleUserByEmail(email, tenant, false);

                    bool iscustomerCare = (user.Tenant == 0 && !user.IsDistributor);
                    if (user != null && !iscustomerCare)
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
                            string currentIP = null;
                            if (HttpContext.Current != null && HttpContext.Current.Request != null)
                            {
                                currentIP = HttpContext.Current.Request.Headers["X-Real-IP"];
                                if (string.IsNullOrEmpty(currentIP))
                                {
                                    currentIP = HttpContext.Current.Request.UserHostAddress;
                                }
                            }
                            
                            if (currentIP == null || !authenticatedIPs.Contains(currentIP))
                            {
                                wc.DownloadString(new Uri(sRequest));
                            }
                        }
                    }

                    AddContactActivityLog(cardId, partnerTypeId, contact?.Id, module, activity, tenant, isSharedLogisticsContact);

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
                    ExceptionHandler.HandleException(ex, DateTime.Now, 0, contact != null ? contact.Email : "", contact != null ? contact.Email : "", "SendUserActivity", ip);
                }
            }


            // return wc.DownloadString(new Uri(sRequest));
        }

        private static DbConnection GetLogsDBConnection()
        {
			var ConfigConnectionString = string.Empty;

			if (LogitudeSettings.DatabaseManagementSystem == "oracle")
			{
				ConfigConnectionString = ConfigurationManager.ConnectionStrings["Oracle_SystemLogsStr"].ConnectionString; ;
			}
			else
			{
				ConfigConnectionString = ConfigurationManager.ConnectionStrings["SystemLogsStr"].ConnectionString;
			}	
            return DatabaseInitializer.GetConnection(ConfigConnectionString);
        }
        public static void AddContactActivityLog(string cardId, string partnerTypeId, string contactId, string module, string activity, int tenant, bool isSharedLogisticsContact, string via = "")
        {
            ContactRepository contactRepository = new ContactRepository(tenant);
            Contact contact = contactRepository.GetSingleContact(contactId, tenant);
            SqlTransaction transaction = null;
            string strConnString = GetLogsDBConnection().ConnectionString;
            using (var scope = TransactionFactory.GetNewReadCommittedTransaction())
            {
                using (SqlConnection cn = new SqlConnection(strConnString))
                {
                    try
                    {
                        string query = "INSERT INTO ContactActivityLogs " +
                        "(Id, ContactId, Module, Activity, LogDateTime, GMTLogDateTime,Tenant,IsSharedLogisticsContact,CardId,PartnerTypeId,Via) " +
                        "VALUES (@Id, @ContactId, @Module, @Activity, @LogDateTime, @GMTLogDateTime, @Tenant, @IsSharedLogisticsContact, @CardId, @PartnerTypeId, @Via) ";
                        SqlCommand cmd = new SqlCommand(query, cn);
                        cmd.Parameters.Add("@Id", SqlDbType.VarChar, 50).Value = Guid.NewGuid().ToString();
                        if (contactId != null)
                        {
                            cmd.Parameters.Add("@ContactId", SqlDbType.VarChar, 50).Value = contactId;
                        }
                        else
                        {
                            cmd.Parameters.Add("@ContactId", SqlDbType.VarChar, 50).Value = DBNull.Value;
                        }
                        cmd.Parameters.Add("@Module", SqlDbType.VarChar, 50).Value = module;
                        cmd.Parameters.Add("@Activity", SqlDbType.VarChar, 50).Value = activity;
                        cmd.Parameters.Add("@LogDateTime", SqlDbType.DateTime).Value = TenantServerConfigration.GetCurrentDateTime(tenant);
                        cmd.Parameters.Add("@GMTLogDateTime", SqlDbType.DateTime).Value = DateTime.Now;
                        cmd.Parameters.Add("@Tenant", SqlDbType.Int).Value = tenant;
                        cmd.Parameters.Add("@IsSharedLogisticsContact", SqlDbType.Bit).Value = isSharedLogisticsContact;
                        if (cardId != null)
                        {
                            cmd.Parameters.Add("@CardId", SqlDbType.VarChar, 50).Value = cardId;
                        }
                        else
                        {
                            cmd.Parameters.Add("@CardId", SqlDbType.VarChar, 50).Value = DBNull.Value;
                        }
                        if (partnerTypeId != null)
                        {
                            cmd.Parameters.Add("@PartnerTypeId", SqlDbType.VarChar, 50).Value = partnerTypeId;
                        }
                        else
                        {
                            cmd.Parameters.Add("@PartnerTypeId", SqlDbType.VarChar, 50).Value = DBNull.Value;
                        }
                        if (via != null)
                        {
                            cmd.Parameters.Add("@Via", SqlDbType.VarChar, 50).Value = via;
                        }
                        else
                        {
                            cmd.Parameters.Add("@Via", SqlDbType.VarChar, 50).Value = DBNull.Value;
                        }
                        cmd.CommandType = CommandType.Text;
                        cmd.CommandTimeout = 5;
                        //transaction = cn.BeginTransaction(System.Data.IsolationLevel.Snapshot);
                        cn.Open();
                        //cmd.Transaction = transaction;
                        var output = cmd.ExecuteNonQuery();
                        //transaction.Commit();
                        cn.Close();
                    }
                    #region commited
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
                    //};

                    //contactActivityLogRepository.Add(log);
                    //contactActivityLogRepository.SubmitChanges();
                    #endregion
                    catch (Exception ex)
                    {
                        if (transaction != null)
                        {
                            transaction.Rollback();
                        }
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
                        ExceptionHandler.HandleException(ex, DateTime.Now, 0, contact != null ? contact.Email : "", contact != null ? contact.Email : "", "SendUserActivity", ip);
                    }
                }
                scope.Complete();
            }


        }
    }
}
