using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Authentication;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebFreight.Web
{
    public partial class WebhooksReceiver : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            //helpMe();
            return;
            try
            {
                using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                {
                    var AccessKey = Request.Headers["AccessKey"];
                    if (string.IsNullOrEmpty(AccessKey))
                    {
                        AccessKey = Request.QueryString["AccessKey"];
                        if (string.IsNullOrEmpty(AccessKey))
                        {
                            //throw new HttpResponseException(HttpStatusCode.Unauthorized);
                            throw new AuthenticationException ("you are not authonticated to call this page.");
                        }
                    }
                    WebhookKeysRepository webhookKeysRepository = new WebhookKeysRepository();
                    var MyWebHookKey = webhookKeysRepository.GetSingleWebhookKeyByAccessKey(AccessKey);
                    if (MyWebHookKey == null)
                    {
                        //throw new HttpResponseException(HttpStatusCode.Unauthorized);
                        throw new AuthenticationException("you are not authonticated to call this page.");
                    }
                    string RecivedString = "";

                    using (var reader = new StreamReader(Request.InputStream,System.Text.Encoding.UTF8))
                    {
                        RecivedString = reader.ReadToEnd();
                    }

                    if (!string.IsNullOrEmpty(RecivedString))
                    {
                        this.SaveMessageToAnalyzeQueue(RecivedString, MyWebHookKey);
                    }

                    scope.Complete();
                }
            }
            catch (Exception ex)
            {
                Response.Write(ex.Message);
                //throw ex;
            }
           

        }

        private void SaveMessageToAnalyzeQueue(string xmlfileText, WebhookKeys MyWebHookKey)
        {
            AnalyzeQueue analyzeQueue = null;


            AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();
            byte[] messageBytes = Encoding.UTF8.GetBytes(xmlfileText);

            analyzeQueue = new AnalyzeQueue()
            {
                CreateDate = DateTime.Now,//TenantServerConfigration.GetCurrentDateTime(0),
                From = MyWebHookKey.PartnerName,
                Id = IdCounter.GetNumber("AnalyzeQueue", 0),
                MessageBody = messageBytes,
                Status = "W",
                Retries = 0,
                ConnectedToEntity = false,
                ConnectedToTenant = true,
                FileSize = xmlfileText.Length,
                Tenant = MyWebHookKey.Tenant,
                Subject = MyWebHookKey.Description,
            };

            analyzeQueue.SearchFields = analyzeQueue.From + ',' + analyzeQueue.Status;
            analyzeQueueReposiory.Add(analyzeQueue);
            analyzeQueueReposiory.SubmitChanges();
            using (TransactionScope scope2 = TransactionFactory.GetNewTransaction())
            {
                if (analyzeQueue != null)
                {
                    DbQueueService queueservice = new DbQueueService();
                    queueservice.InitializeQueue("GeneralWebHookAnalyzer", 0);
                    queueservice.Send(new Dictionary<string, string>() { { "AnalyzeQueueId", analyzeQueue.Id } }, analyzeQueue.Tenant);
                    queueservice.Complete();
                }

                scope2.Complete();
            }


        }


        private void helpMe()
        {
            //CSV structure from transzilla
            //אישור	סכום	שולם באמצעות	4 ספ' אחרונות	תוקף	ת'ז בעל הכרטיס	מספר שידור לשב'א	סוג תשלום	מספר אישור	סולק	סטטוס בדיקת CVV	סטטוס בדיקת ת'ז	מספר שובר שב'א	טוקן	איש קשר	טלפון	DCdisable
          //  0   1   2   3   4  5   6   7   8   9   10  11  12  13  14  15  16



            string[] fileData = File.ReadAllLines("C:\\Users\\Simon\\Desktop\\tranzila_transactions.csv");
           var sb=new StringBuilder();
            for ( int i=2;i< fileData.Length;i++)
            {
                var item = fileData[i];
                var dt = item.Split(',');
                string accKey = dt[13];
                string tempref = dt[0];
                string confCode = dt[8];
                string resCode = "000";
              
                string ccno = dt[3];
                string sum = dt[1];
                
                string expyear = dt[4].Substring(3);
                string expmonth = dt[4].Substring(0,2);
                //string bit = "bit";
                //string payment_method = "BIT";
                string contact =dt[14];
                string phone = dt[15];
                
                string myid = dt[5];
                string DCdisable = dt[16]+","+ dt[17];

                DCdisable = DCdisable.Substring(1, DCdisable.Length - 2);




                string lindex = dt[0];

                WebhookKeysRepository webhookKeysRepository = new WebhookKeysRepository();
                var MyWebHookKey = webhookKeysRepository.GetSingleWebhookKeyByAccessKey(accKey);
                string req = "";

                if (MyWebHookKey == null)
                {
                    MyWebHookKey = new WebhookKeys() { AccessKey = "123", PartnerName = "TranzilaPayment", Tenant = 49, Description = "TranzilaPayment" };
                }
                if (MyWebHookKey != null)
                {



                    // $"index={tempref}&ConfirmationCode={confCode}&notify_url=https://cloud.amital.co.il/WebhooksReceiver.aspx?AccessKey={accKey}&Response=000&processor_response_code={resCode}&supplier={ecommerce}&terminal_name={ecommerce}&ccno={ccno}&sum={sum}&currency={currency}&expyear={expyear}&expmonth={expmonth}&bit={bit}&payment_method={payment_method}&lang=il&pdesc=×ž×™×¡×™×\u009d&contact={contact}&phone={phone}&mycvv={mycvv}&myid={myid}&g-recaptcha-response=03AFcWeA6_y5B430c9R8Sp5Sv5NLGeUDI0VbgmuNU2w0qvlC4QY5i2PLkDr8zujMGSIvtyKTlAbOkzXbyQPbOwgFl9sldRwiHX9-SkvK3mn9qiSU1QRQ-VknQCcjSs8zObSgDmtm3t9P3QkSsClqN29o53VefYTXBelvngdHKlL6bXgDP7rGlmvBg9TI1VdGQpXuDlIev0DlIS-8bNIHdZIfXvZQTItX2LRpU1qVN-MEE8yi8c0Gy2F3N7w-8AST6DnXAZC1qALNA9F4OcXSbyYCnmoi6vWsd43yWI_mgjbecoa8wfmR_OHrtHXLgNUgki6yiL4-Cb3xvw1jiiSbvkhirMG6WZ3FHDvCogmx3ZYuP4AGR1meQwcAIiOml0P-iotQTuKMkSmI1XeFyQeQPsF2cVwTgf4GGPObCkInnOZUKdGtUxEyDZWTRfaHkDnfC8Prm7fgCLSwQ7lj62xoJmsuCFQDRxjQMoZwJWlZIdmVsYELApPfBqJLCZ2lEk2Y9yz09simDN6jRW0G0dj24xPegOTM9XAAiSj89PS3w7MIVPBFHTulweEEC-Bu9ywi_hoGNxtIafvBlPOsldcGlKuTCVZ0q6NrncHllYlOtpFdzNn3lV2BmUKrxnkFq4HrT_NSIByAWet6TbeUB5DJzMOdNe_1Wl3rkupg2aL9yscM6MPWjRGjgBPOvZB-sXU9bbiLfzjNzkvfxPUkHnR8KKvR_2nxV2hNTqzTc0oRnB3L6LOkM2liGPSrBLnidSA3oGaz7KHk8hm10hpsedeVR3kG5R4IfrYaeydsoDwEGLpVHWLHOYX8_56zHj3X-MTtYz-0Xjc-d9PtrIkitcPxFig4pV9G_esu27tMRe6EdOkBTLNCec1mXnPoY4y_1s_BGawOE8Tk7ldBP7&transaction_layout_bit=qr-sms&u71=1&op=1&DCdisable={DCdisable}&DclickTK={DclickTK}=&o_cred_type={o_cred_type}&nbit={nbit}&ccard={ccard}&failure_url_n={failure_url_n}";
                    req = $"Response=000&expmonth={expmonth}&contact={contact}&myid={myid}&currency=1&expyear={expyear.Substring(2)}&supplier=ecommerce&pdesc=מיסים&sum={sum}&json_purchase_data=[{{\"product_name\":\"מיסים\",\"product_quantity\":\"1\",\"product_price\":\"{sum}\"}}]&o_cred_type=&lang=il&ccard=&phone={phone}&transaction_layout_bit=qr-sms&thtk=&ccno={ccno}&DCdisable={DCdisable}&DclickTK=edj3wgr2wf1&u71=1&g-recaptcha-response=03AFcWeA5cRayMk6zrkngTaJCpXeY2Ui63ody5a_1pTTKdnaytOLBF1XPUfHJMHaVJ1pHM576C1tTtjFfUfV0JQ3UQKpkmgZrRkByXZRKzGdkWcGsL_-FKi3PY5-77fybOd4e41rlBgep7dEVV-j_gHEJBCasbs0t8AwImbPO9ITF5bE04PnaPiuAzm8YwPDKPHvj3hUffkpETCDHcAsVMM3vLlW2ufaBIOhORgdBYrg9sNqKabrWZEjSsYZfV0yeEO3RmSloHjo7J6GBpZ8nzpchR0WX1Xl8Gjjvii5Kpy6GGCAECZmqG6cmGUXNRZ53IVaWoiigryGCOK8TnqLekhiZRHmhR_fIcAu4AstX5zIRqPXoy4k1UZeTifXG3K4uXRpyWWRt8Sz_EAyan0_tbSOEq5Yf6qgfHOgNKEj2C6d1f2a_t50XG9qyTKmSLMKBzt3ovI0ldm1lbcz73JrRRJ9KAvJEaQ4oU9chQPBm0Y2zpJs-VXxzvSGn9wQxYhrg1sZ7ViJUt4iB6r8Tb_e4XjsnPUXCCGyg3Lx6ptk_0ac8wKe4i40ktqsNp6xH2-Nr4g9SiN-oM_lGBtLDaWEms2hSSo0Y-dKDPeJu-mAmRVmno9qq_-6DkVIxdBINf4V5ZtdXbkPDm-V9E1qytiZilab8zQWfOUeLehx3C_goX8KdOxoNaqtTcbPXmfh3xpfFLugmhwlogR2SjSEJTWLv0nTxv_6fFfrr46GsN2geFx4U6psSKA_01-RylrnQbI6eol-CCUaGBO5cu6SxHpGtnhKkRJUeRJMUiawR50RJbjdfqmeGwufAvJQ-5PQLOj11eWCVg-u0Nwc3gQc32K3GahUaa1P57SBVmlQ&op=1&ConfirmationCode={confCode}&index={lindex}&Responsesource=1&Responsecvv=1&Responseid=0&Tempref={tempref}&DBFIsForeign=0&DBFcard=2&cardtype=2&DBFcardtype=6&cardissuer=6&DBFsolek=1&cardaquirer=1&tz_parent=ecommerce&cred_type_shva=1";
                    SaveMessageToAnalyzeQueue(req, MyWebHookKey);
                }
                else
                {
                    sb.AppendLine(req);
                }

            }

            File.WriteAllText("C:\\Users\\Simon\\Desktop\\tranzila_transactions_errorlog.csv", sb.ToString());



          
        
        }
    }
}