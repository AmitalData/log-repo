using System;
using System.IO;
using System.Net;
using System.Text;

namespace Logitude.Server.Tools.WebHook
{
	public class WebHookService
	{
		private string url = null;
		WebRequest webHookWebRequest;
		WebHookAuthorization webHookAuthorization;
		public WebHookService(string uRL, WebHookAuthorization webHookAuth)
		{
			url = uRL;
			webHookAuthorization = webHookAuth;
		}

		public string Upload(string remoteFile, byte[] fileData)
		{
			string responseMessage = "";
			try
            {
                webHookWebRequest = WebRequest.Create(url);
				if(webHookAuthorization.AuthorizationType == "BASICAUTHENTICATION")
                    AddBasicAuthenticationToWebRequestHeaders();
                FillWebRequestParameters(fileData);
                Stream dataStream = webHookWebRequest.GetRequestStream();
                dataStream.Write(fileData, 0, fileData.Length);
                dataStream.Close();

                responseMessage = CheckIfFileWasUploadedSuccessfully(remoteFile, dataStream);
            }
            catch (WebException ex)
			{
				WebHookServiceExceptionThrower.Throw(ex, remoteFile, "upload");
			}
			catch (Exception ex)
			{
				throw ex;
			}

			return responseMessage;
		}

        private void AddBasicAuthenticationToWebRequestHeaders()
        {
            string basicAuthCredidentials = webHookAuthorization.BasicUserName + ":" + webHookAuthorization.BasicPassword;
            string base64Credidentials = Convert.ToBase64String(Encoding.Default.GetBytes(basicAuthCredidentials));
            webHookWebRequest.Headers["Authorization"] = "Basic " + base64Credidentials;
        }

        private void FillWebRequestParameters(byte[] fileData)
        {
            webHookWebRequest.Method = "POST";
            webHookWebRequest.ContentType = "application/x-www-form-urlencoded";
            webHookWebRequest.ContentLength = fileData.Length;
        }

        private string CheckIfFileWasUploadedSuccessfully(string remoteFile, Stream dataStream)
        {
			WebResponse response = webHookWebRequest.GetResponse();
			string responseFromServer;
			using (dataStream = response.GetResponseStream())
			{
				StreamReader reader = new StreamReader(dataStream);
				responseFromServer = reader.ReadToEnd();
			}
			response.Close();
			webHookWebRequest = null;

			string responseMessage;
            if (((HttpWebResponse)response).StatusDescription == "OK" && responseFromServer == "Accepted")
                responseMessage = "File '" + remoteFile + "' was successfully uploaded to '" + GetURLDomain();
            else
                throw new Exception("File '" + remoteFile + "' was unsuccessfully uploaded to '" + GetURLDomain());
            return responseMessage;
        }

        private string GetURLDomain()
		{
			string urlDomain = url;
			urlDomain = urlDomain.Replace("https://", "");
			urlDomain = urlDomain.Replace("http://", "");
			if (urlDomain.IndexOf('/') > -1)
			{
				urlDomain = urlDomain.Substring(0, urlDomain.IndexOf('/'));
			}
			return urlDomain;
		}
	}

	public class WebHookAuthorization
	{
		public string AuthorizationType { get; set; }
		public string BasicUserName { get; set; }
		public string BasicPassword { get; set; }
	}
}
