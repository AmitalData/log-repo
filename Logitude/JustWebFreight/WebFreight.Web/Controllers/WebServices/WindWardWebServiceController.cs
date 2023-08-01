using TrackedShipmentsAPI.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Web.Http;
using System.Net.Http;
using System;
using System.Text.Json;
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

				dynamic webhookObject = webhook  ; //= JObject.Parse(webhook.GetRawText());

				string sentAt = webhookObject?.data?.metadata?.sentAt;

				var enrichedData = webhookService.AddDataToJSON(webhookObject?.data, sentAt);
				var result = webhookService.JsonToXML(enrichedData);

				var xmlString = result.OuterXml;
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