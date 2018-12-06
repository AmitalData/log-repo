using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Newtonsoft.Json;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

namespace WebFreight.Web
{
    public partial class ChampMessaging : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                using (TransactionScope scope = TransactionFactory.GetTransaction())
                {
                    Response.Clear();
                    Response.ContentType = "text/xml";

                    NameValueCollection parr = Request.QueryString;

                    bool iPasswordValid = false;

                    string iPassword = parr["password"];
                    if (iPassword == "logiutde")
                    {
                        iPasswordValid = true;
                    }

                    //http://localhost:9996/ChampMessaging.aspx?password=logiutde

                    if (iPasswordValid)
                    {
                        HttpRequest iRequest = this.Request;

                        if (iRequest != null)
                        {
                            string iString = "";

                            using (var reader = new StreamReader(Request.InputStream))
                            {
                                iString = reader.ReadToEnd();
                            }

                            //string jsonData = HttpUtility.UrlDecode(iString);

                            //XmlDocument doc = JsonConvert.DeserializeXmlNode("{\"Envelope\":" + jsonData, "Root");

                            //string xmlString = System.Xml.Linq.XElement.Parse(doc.OuterXml).ToString();

                            if (!string.IsNullOrEmpty(iString))
                            {
                                this.SaveMessageToAnalyzeQueue(iString);
                            }
                        }

                        Response.Write("<status>OK</status>");
                        Response.StatusCode = 200;
                        Response.End();
                    }

                    else
                    {
                        Response.Write("<status>Fail</status>");
                        Response.StatusCode = 401;
                        Response.End();
                    }

                    scope.Complete();
                }
            }

            catch (Exception ex)
            {
                if (ex.Message == "Thread was being aborted.")
                {

                }

                else
                {
                    ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "ChampMessaging Page", "Page_Load Method", null);

                    Response.Write("<status>Fail</status>");
                    Response.Write("<message>" + ex.Message + "</message>");
                    Response.StatusCode = 500;
                    Response.End();
                }
            }
        }
        protected override void Render(HtmlTextWriter writer)
        {
            base.Render(writer);
            Response.TrySkipIisCustomErrors = true;
        }

        private void SaveMessageToAnalyzeQueue(string xmlfileText)
        {
            AnalyzeQueue analyzeQueue = null;

            using (TransactionScope scope2 = TransactionFactory.GetNewTransaction())
            {
                AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();
                byte[] messageBytes = Encoding.ASCII.GetBytes(xmlfileText);

                analyzeQueue = new AnalyzeQueue()
                {
                    CreateDate = TenantServerConfigration.GetCurrentDateTime(0),
                    From = "Champ",
                    Id = IdCounter.GetNumber("AnalyzeQueue", 0),
                    MessageBody = messageBytes,
                    Status = "W",
                    Retries = 0,
                    ConnectedToEntity = false,
                    ConnectedToTenant = false,
                    FileSize = xmlfileText.Length,
                };

                analyzeQueue.SearchFields = analyzeQueue.From + ',' + analyzeQueue.Status;
                analyzeQueueReposiory.Add(analyzeQueue);
                analyzeQueueReposiory.SubmitChanges();

                scope2.Complete();
            }

            if (analyzeQueue != null)
            {
                DbQueueService queueservice = new DbQueueService();
                queueservice.InitializeQueue("ChampAnalyzer", 0);
                queueservice.Send(new Dictionary<string, string>() { { "AnalyzeQueueId", analyzeQueue.Id } });
                queueservice.Complete();
            }
        }
    }
}