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
                    queueservice.Send(new Dictionary<string, string>() { { "AnalyzeQueueId", analyzeQueue.Id } });
                    queueservice.Complete();
                }

                scope2.Complete();
            }


        }
    }
}