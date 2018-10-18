using Logitude.Server.Tools.Counters;
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
    public partial class EntityExternalUpdate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            byte[] data = getData(Request.InputStream);
            if (data != null)
            {
                this.SaveMessageToAnalyzeQueue(data);
            }
            
        }


        private void SaveMessageToAnalyzeQueue(byte[] messageData)
        {
            AnalyzeQueueRepository analyzeQueueReposiory = new AnalyzeQueueRepository();


            AnalyzeQueue analyzeQueue = new AnalyzeQueue()
            {
                CreateDate = TenantServerConfigration.GetCurrentDateTime(0),
                From = "EntityExternalUpdate",
                Id = IdCounter.GetNumber("AnalyzeQueue", 0),
                MessageBody = messageData,
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



        private byte[] getData(Stream str)
        {
            try
            {
                StreamReader stream = new StreamReader(str);
                string x = stream.ReadToEnd();
                if (string.IsNullOrEmpty(x) || string.IsNullOrWhiteSpace(x)) return (null);

                string jsonData = HttpUtility.UrlDecode(x);

                if (string.IsNullOrEmpty(jsonData) || string.IsNullOrWhiteSpace(jsonData)) return (null);

                byte[] array = Encoding.ASCII.GetBytes(jsonData);

                return (array);
            }
            catch (Exception ex)
            {
                WriteError("Failed to getData" + Environment.NewLine + ex.ToString());
                throw;
            }
        }

        private void WriteError(string data)
        {
            string tempfolder = Server.MapPath(@"~\Temp");
            string filename = System.IO.Path.Combine(tempfolder, "PushEvent_" + Path.GetFileNameWithoutExtension(Path.GetRandomFileName()) + ".Err.TXT");
            File.WriteAllText(filename, data);
        }

    }




}