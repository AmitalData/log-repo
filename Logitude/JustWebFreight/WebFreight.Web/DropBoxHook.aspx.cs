using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Newtonsoft.Json;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using WebFreight.Web.Security;

namespace WebFreight.Web
{
    public partial class DropBoxHook : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                //var signature = Request.Headers.Get("X-Dropbox-Signature");
                //for account in json.loads(Request.data)['list_folder']['accounts']:
                var signatureHeader = Request.Headers.GetValues("X-Dropbox-Signature");
                if (signatureHeader == null || !signatureHeader.Any())
                {
                    var tempo = Request.QueryString["challenge"];
                    if (tempo != null)
                    {
                        Response.Write(tempo);
                        Response.End();
                    }
                    return;
                    //return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }

                // Get the signature value
                string signature = signatureHeader.FirstOrDefault();

                // Extract the raw body of the request
                string body = null;
                TenantAdditionalDataRepository Repo = new TenantAdditionalDataRepository(0);
                using (StreamReader reader = new StreamReader(Request.InputStream))
                {
                    body = reader.ReadToEnd();
                }

                // Check that the signature is good
                //string appSecret = ConfigurationManager.AppSettings["Dropbox_AppSecret"];
                //using (HMACSHA256 hmac = new HMACSHA256(Encoding.UTF8.GetBytes(appSecret)))
                //{
                //    if (!VerifySha256Hash(hmac, body, signature))
                //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                //}
                if (body != null)
                {
                    dynamic jsonObj = JsonConvert.DeserializeObject(body);
                    if (jsonObj != null && jsonObj.delta != null && jsonObj.delta.users != null)
                    {
                        foreach (var obj in jsonObj.delta.users)
                        {
                            TenantAdditionalData currentTenant = Repo.GetSingleTenantAdditionalDataByUID(obj.Value.ToString());
                            if (currentTenant != null)
                            {
                                IQueueService queueservice = new DbQueueService();
                                queueservice.InitializeQueue("DrobBoxQueue", 0);
                                queueservice.Send(new Dictionary<string, string>() { { "Tenant", currentTenant.Tenant.ToString() } }, null, null);
                            }
                        }
                    }
                }

                var temp = Request.QueryString["challenge"];
                if (temp != null)
                {
                    Response.Write(temp);
                    Response.End();
                }
                
            }
            catch (Exception ex)
            {
                ExceptionHandler.HandleException(ex, DateTime.Now, 0, "", "DropBoxHook", "", null);
            }
        }

        private string GetSha256Hash(HMACSHA256 sha256Hash, string input)
        {
            // Convert the input string to a byte array and compute the hash. 
            byte[] data = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes 
            // and create a string.
            var stringBuilder = new StringBuilder();

            // Loop through each byte of the hashed data  
            // and format each one as a hexadecimal string. 
            foreach (byte t in data)
            {
                stringBuilder.Append(t.ToString("x2"));
            }

            // Return the hexadecimal string. 
            return stringBuilder.ToString();
        }

        private bool VerifySha256Hash(HMACSHA256 sha256Hash, string input, string hash)
        {
            // Hash the input. 
            string hashOfInput = GetSha256Hash(sha256Hash, input);

            if (String.Compare(hashOfInput, hash, StringComparison.OrdinalIgnoreCase) == 0)
                return true;

            return false;
        }


    }
}