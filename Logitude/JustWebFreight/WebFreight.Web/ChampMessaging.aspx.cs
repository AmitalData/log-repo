using Logitude.Server.Tools.Counters;
using Logitude.SystemLogs;
using Newtonsoft.Json;
using Simplog.Data.Helpers;
using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Global.Data.GlobalModel.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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
            }

            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "ChampMessaging Page", "ChampMessaging Method", null);
            }
        }

        private void SaveMessageToAnalyzeQueue(string messageData)
        {
            AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();
            byte[] messageBytes = Encoding.ASCII.GetBytes(messageData);

            AnalyzeQueue analyzeQueue = new AnalyzeQueue()
            {
                CreateDate = TenantServerConfigration.GetCurrentDateTime(0),
                From = "Champ",
                Id = IdCounter.GetNumber("AnalyzeQueue", 0),
                MessageBody = messageBytes,
                Status = "W",
                Retries = 0,
                ConnectedToEntity = false,
                ConnectedToTenant = false,
                FileSize = messageData.Length,
            };

            analyzeQueue.SearchFields = analyzeQueue.From + ',' + analyzeQueue.Status;
            analyzeQueueReposiory.Add(analyzeQueue);
            analyzeQueueReposiory.SubmitChanges();
        }
    }
}