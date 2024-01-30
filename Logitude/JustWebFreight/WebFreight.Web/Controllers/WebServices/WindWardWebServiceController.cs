using TrackedShipmentsAPI.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Http;
using System.Net.Http;
using System;
using System.Net;
using WebFreight.Web.Helpers;
using Simplog.Server.Infrastructure;
using System.IO;
using System.Text;
using System.Xml;
using WebFreight.Web;
using System.Security.AccessControl;
using System.Web.Mvc;
using System.Globalization;
using System.Configuration;
using System.Linq;
using Intuit.Ipp.Data;

namespace WebFreight.Web.Controllers.WebServices
{
    public class WindWardWebServiceController : ApiController
	{

		[System.Web.Http.HttpPost]
		public HttpResponseMessage shipmentUpdate([FromBody] JObject webhook)
		{
			try
			{
				WriteLogMe("shipmentUpdate ENTER POST: ", webhook, "shipmentUpdate");
				
				var webhookService = new TrackedShipmentsAPI.Services.WebhookService();
				dynamic webhookObject = null;
				using (JsonReader reader = new JsonTextReader(new StringReader(webhook.ToString())))
				{
					reader.DateParseHandling = DateParseHandling.None;
					 webhookObject = JObject.Load(reader);
				}

				dynamic webhookObjectData = webhookObject?.data != null ? webhookObject?.data : webhookObject;
				
				string sentAt = webhookObjectData?.metadata?.sentAt;

				var enrichedData = webhookService.AddDataToJSON(webhookObjectData, sentAt);
				var result = webhookService.JsonToXML(enrichedData);

				var xmlString = result.OuterXml;
				WriteData(webhook, xmlString);
				var ContainerPushPage = new ContainerPush();
				WriteLogMe("shipmentUpdate AnalyzeContainerStatus: " , null, "shipmentUpdate");

				ContainerPushPage.AnalyzeContainerStatus(xmlString);

				string xmlStr = xmlString.ToString();
				WriteLogMe("shipmentUpdate After POST: "+ xmlStr, null, "shipmentUpdate");

				return Request.CreateResponse(HttpStatusCode.OK, xmlStr);
			}
			catch (Exception ex)
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
			}
		}
		private void WriteData(dynamic webhook, dynamic xmlString)
		{
			try
			{
				string WindWardSettings = LogitudeSettings.WindWardSettings;
				var WindWardSettingsArray = WindWardSettings?.Split(',');
				string IsWriteData = (WindWardSettingsArray!=null && WindWardSettingsArray.Count() > 3) ? WindWardSettingsArray[3] : "0";
				if (IsWriteData == "1")
				{				
				   string id = webhook.data?.metadata?.jobNumber;
				   string containerNumber = webhook.data?.shipment?.identifiers?.containerNumber;
					string BLNumber = webhook.data?.shipment?.identifiers?.bolNumber;
					string filename =string.Format("{0}_{1}_{2}_{3}.json", containerNumber, BLNumber, id ,DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss-fff"));
				   
				   
				   if (!System.IO.Directory.Exists("c:\\temp\\windward"))
				   {
				   	System.IO.Directory.CreateDirectory("c:\\temp\\windward");
				   }
				   System.IO.File.WriteAllText(Path.Combine("c:\\temp\\windward", filename), JsonConvert.SerializeObject(xmlString));
				}
			}
			catch (Exception ex)
			{

				//throw;
			}
		}
		public void WriteLogMe(string subject, object obj, string funcName)
		{
			DateTime stopLogAt = DateTime.MinValue;
			try
			{
				string UntilDateyyyyMMdd = ConfigurationManager.AppSettings["20230718T104900.LogUntilDateyyyyMMdd"];
				if (!string.IsNullOrWhiteSpace(UntilDateyyyyMMdd))
				{
					stopLogAt = DateTime.ParseExact(UntilDateyyyyMMdd,
														"yyyyMMdd",
														CultureInfo.InvariantCulture,
														DateTimeStyles.None);
				}
				var logData = "";

				if (obj != null)
				{
					Newtonsoft.Json.JsonConvert.SerializeObject(obj);

				}
				LogitudeSettings.HandleLogMe(subject + logData, false, funcName, stopLogAt);
			}
			catch (Exception ex)
			{

			}
		}

	}
}