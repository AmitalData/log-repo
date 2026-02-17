using Logitude.SystemLogs;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using Simplog.Server.Infrastructure.Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFreight.Web.Monitoring
{
    public partial class StorageBackup : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Clear();
            Response.ContentType = "text/xml";
            //Response.Write("<?xml version='1.0' encoding='UTF-8'?>");
            Response.Write("<pingdom_http_custom_check>");

            if (IsStorageBackupUp())
            {
                Response.Write("<status>OK</status>");
            }
            else
            {
                Response.Write("<status>Fail</status>");
            }
            int ResponseTime_Millisecond = HttpContext.Current.Timestamp.Millisecond;
            String ResponseTime = "<response_time>" + ResponseTime_Millisecond + "</response_time>";
            Response.Write(ResponseTime);
            Response.Write("</pingdom_http_custom_check>");
            Response.End();
        }

        private bool IsStorageBackupUp()
        {

            string backupcontainer = "backupresult";
            bool exist = false;

            try
            {
                if (Request != null)
                {
                    string account = Request.QueryString["account"];
                    switch (account)
                    {
                        case "Amital":
                            {
                                string AmitalAccountName = "";//"";otherbackup
                                string AmitalAccountKey =  "";

                                string yesterday = DateTime.Now.AddDays(-1).DayOfWeek.ToString();
                                switch (yesterday)
                                {

                                    case "Sunday":
                                        AmitalAccountName = "amitalbackupsunday";
                                        AmitalAccountKey = "5p8Sc1senk9dl8Z8LF7SaIctUf380QPqCVmTzkPfavJII0BVkBalOVBH4qXMAQttmTmVBv3Ze2CISIf3QYU2ug==";
                                        break;

                                    case "Monday":
                                        AmitalAccountName = "amitalbackupmonday";
                                        AmitalAccountKey = "OiA7/mmhgcvVlOhi5rtABgs1qfgsQvEfJCTZbPndgngJczI6iAmbt407/kaNmxfD23NJczHqXiCf0tBKFGR5pQ==";
                                        break;

                                    case "Tuesday":
                                        AmitalAccountName = "amitalbackuptuesday";
                                        AmitalAccountKey = "1PmTnvSJ4dkkadTPwvr1xMcM4SAacElRvsY0IVri3jMGEFUgCBJO4W+Mwd+ZKb1APKWtyy5cv5xD0teC+sY0hg==";
                                        break;

                                    case "Wednesday":
                                        AmitalAccountName = "amitalbackupwednesday";
                                        AmitalAccountKey = "tSytPeRguCzm+1jD3u99M+fbZCvOr1iOPT1eEgQc2y93AAIixKfYpeHh50Cht6VCdixzcvrJdO6xax6881Hs+Q==";
                                        break;

                                    case "Thursday":
                                        AmitalAccountName = "amitalbackupthursday";
                                        AmitalAccountKey = "gjAo+cDDMl1h+ONGnsslMsgng0cav6QE7JQupwbmPMjnYkk6754ZO2uveR9T5oWVUI53AHCkgGHnINIKPBcY2Q==";
                                        break;

                                    case "Friday":
                                        AmitalAccountName = "amitalbackupfriday";
                                        AmitalAccountKey = "ST7oxkV6XD69YXJF0PPyBuZQVg6D9unbtuVC42pGudzJx3rijbU9YWcPLO8yWGt/8wCfN+5Dc74fr7yE8UNJHA==";
                                        break;

                                    case "Saturday":
                                        AmitalAccountName = "amitalbackupsaturday";
                                        AmitalAccountKey = "ctuJ4/iXR4ZY3dduVrt7terln064FNQWt9sqjO0q8zk7Be52Q69hlm5K4JYmPry2oVRp1kZOXb8Rg4zJPqwhzg==";
                                        break;


                                }

                                exist = CheckBackupBlob(AmitalAccountName, AmitalAccountKey, backupcontainer);

                                if (!exist)
                                    return false;
                                break;
                            }

                        case "Logitude":
                            {
                                string LogitudeAccountName = "";
                                string LogitudeAccountKey = "";

                                string yesterday = DateTime.Now.AddDays(-1).DayOfWeek.ToString();
                                switch (yesterday)
                                {
                                    case "Sunday":
                                        LogitudeAccountName = "logitudebackupsunday";
                                        LogitudeAccountKey = "blcp9DsFPQ+Od0GMqzOBLy3V/+sdxxh16uuIDaY96xn7g2tD/rpZ7eKoZUkJAvsx9mWD+qZ56k1xEXKZB78bzQ==";
                                        break;

                                    case "Monday":
                                        LogitudeAccountName = "logitudebackupmonday";
                                        LogitudeAccountKey = "e0lTI+h/31UOrO03IAe8gI7eR5sBmMDdmlpgqHn5nzp3jqUP60xMsrm5nkxlntCPyc+cnnjDApZjKwett7XXuw==";
                                        break;

                                    case "Tuesday":
                                        LogitudeAccountName = "logitudebackuptuesday";
                                        LogitudeAccountKey = "4Fm7CHGiw52I6w+QxV7lY35aqzIFiASxKTn6Qkq8jomUvIwQS3TIHRzuDFZ1pdv3J0fKKYm8xnEEsgesfRwQOQ==";
                                        break;

                                    case "Wednesday":
                                        LogitudeAccountName = "logitudebackupwednesday";
                                        LogitudeAccountKey = "J6k3KIIGosRGKZPUZmjkCfgxniZZz8iqe8pgW2e3xjuVtSLlMjyKHTGTMpitAD8MZBYmbTM/DQDeyGf8dFLg5A==";
                                        break;

                                    case "Thursday":
                                        LogitudeAccountName = "logitudebackupthursday";
                                        LogitudeAccountKey = "L3WmubNECcQF/VUcGmTWiCghKToTTZyU0yzKHJSc2SEWirsXGxqO7AKMdVEa5LjNPIdBsFaxN93zMWu7SbItxA==";
                                        break;

                                    case "Friday":
                                        LogitudeAccountName = "logitudebackupfriday";
                                        LogitudeAccountKey = "HmMtGkOJdS+WaOFAACWAIggH2ACATlHHvYNf+hJCkoQ14UgdduhdoayWbExFJe2sVnMkJE5yc43peXycSu2MKw==";
                                        break;

                                    case "Saturday":
                                        LogitudeAccountName = "logitudebackupsaturday";
                                        LogitudeAccountKey = "cB9oGFft3ZJeF75I5Jr2T0iQszM/iRtufcvxxpvFbLygnedNW3VddnTS6zWa1V0N7i8t4NjVDuUCzAGS2xjpZQ==";
                                        break;
                                }

                                exist = CheckBackupBlob(LogitudeAccountName, LogitudeAccountKey, backupcontainer);

                                if (!exist)
                                    return false;
                                break;
                            }


                        default:
                            return false;
                    }
                }
            }
            catch (Exception e)
            {
                ExceptionHandler.HandleException(e, DateTime.Now, 0, "", "StorageUp", "Bug in IsStorageUp Method : Write to a Blob", null);
                return false;
            }

            return true;
        }

        private bool CheckBackupBlob(string BackupAccountName, string BackupAccountKey, string backupcontainer)
        {
            CloudBlobContainer blobContainer;
            CloudBlockBlob resultblob;

            CloudStorageAccount CloudStorageAccount = new CloudStorageAccount(new StorageCredentials(BackupAccountName, BackupAccountKey),
new Uri(@"http://" + BackupAccountName + ".blob.core.windows.net/"),
new Uri(@"http://" + BackupAccountName + ".queue.core.windows.net/"),
new Uri(@"http://" + BackupAccountName + ".table.core.windows.net/"), null);

            CloudBlobClient CloudBlobClient = CloudStorageAccount.CreateCloudBlobClient();
            blobContainer = CloudBlobClient.GetContainerReference(backupcontainer);
            string blobName = string.Format(BackupAccountName.ToLower() + "-{0:yyyy-MM-dd}.txt", (DateTime.Today.AddDays(-1))).ToLower();
            resultblob = blobContainer.GetBlockBlobReference(blobName);

            return resultblob.Exists();

        }
    }
}