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

namespace Logitude.Server.Tools.Helpers
{
   public class TotangoActivityLogger
    {
        public static void SendUserActivity(string organizationId, string orgDisplayName, string userName, string module, string activity, string contactId, int tenant, bool isSharedLogisticsContact, string cardId, string partnerTypeId)
        {
            if (LogitudeSettings.DeploymentStage != "Dev" && !LogitudeSettings.IsCostomsDeploy)
            {
                ContactRepository contactRepository = new ContactRepository(tenant);
                Contact contact = contactRepository.GetSingleContact(contactId, tenant);

                try
                {
                    UserRepository userRepository = new UserRepository(tenant);
                  
                    User user = userRepository.GetSingleUser(contactId, tenant, false);
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

                    AddContactActivityLog(cardId, partnerTypeId, contactId, module, activity, tenant, isSharedLogisticsContact);

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


        private static void AddContactActivityLog(string cardId, string partnerTypeId, string contactId, string module, string activity, int tenant, bool isSharedLogisticsContact)
        {
            ContactRepository contactRepository = new ContactRepository(tenant);
            Contact contact = contactRepository.GetSingleContact(contactId, tenant);

            try
            {
                ContactActivityLogRepository contactActivityLogRepository = new ContactActivityLogRepository();
                ContactActivityLog log = new ContactActivityLog()
                {
                    Id = Guid.NewGuid().ToString(),
                    ContactId = contactId,
                    Module = module,
                    Activity = activity,
                    LogDateTime = TenantServerConfigration.GetCurrentDateTime(tenant),
                    GMTLogDateTime = DateTime.Now,
                    Tenant = tenant,
                    IsSharedLogisticsContact = isSharedLogisticsContact,
                    CardId = cardId,
                    PartnerTypeId = partnerTypeId,
                };

                contactActivityLogRepository.Add(log);
                contactActivityLogRepository.SubmitChanges();
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
    }
}
