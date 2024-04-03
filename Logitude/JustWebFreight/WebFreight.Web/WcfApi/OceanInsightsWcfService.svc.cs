using Logitude.BL.ShipmentsModel.EntityPMs;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.XSD.Simulators;
using Microsoft.VisualStudio.OLE.Interop;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Asn1.X509;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.Repositories;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IdentityModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Xml;
using Unifreight.ContainerTasks;
using WebFreight.Web.Security;
using WWApi.Models;
using static Dropbox.Api.Sharing.ListFileMembersIndividualResult;

namespace WebFreight.Web.WcfApi
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "OceanInsightsWcfService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select OceanInsightsWcfService.svc or OceanInsightsWcfService.svc.cs at the Solution Explorer and start debugging.
    //  ss
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class OceanInsightsWcfService : IOceanInsightsWcfService
    {
        public Response Insert(int Tenant, string ScacCode, string ReferenceNo, string Type)
        {
			try
			{
				string data = "Tenant: " + Tenant.ToString() + "ScacCode: " + ScacCode?.ToString() + "ReferenceNo: " + ReferenceNo?.ToString() + "Type: " + Type?.ToString();
				WriteLogMe("Insert: " + data,null , "UpsertTrackedShipments");
				return UnitedRequest(Tenant, ScacCode, ReferenceNo, Type)?.Result;
			}
			catch(Exception ex) 
			{
				WriteLogMe("Insert Exception: " + ex.Message.ToString(), null, "UpsertTrackedShipments");

				return null;
			}

		}
		public async Task<Response> UnitedRequest(int Tenant, string ScacCode, string ReferenceNo, string Type)
		{
			try
			{
				string Bol = "";
				if (Type == "c_id")
				{
					var arrayReferenceNo = ReferenceNo?.Split(',');
					ReferenceNo = arrayReferenceNo != null && arrayReferenceNo?.Count() > 0 ? arrayReferenceNo[0] : ReferenceNo;
					Bol = arrayReferenceNo != null && arrayReferenceNo?.Count() > 1 ? arrayReferenceNo[1] : "";

				}
				string WindWardSettings = LogitudeSettings.WindWardSettings;
				var WindWardSettingsArray = WindWardSettings?.Split(',');
				var WWtenant = WindWardSettingsArray != null && WindWardSettingsArray?.Count() > 2 ? WindWardSettingsArray[2] : "";
				if (!string.IsNullOrEmpty(WWtenant))
				{
					WriteLogMe("UnitedRequest Insert WindWard: "+ WWtenant, null, "UpsertTrackedShipments");
					var Res = await WindWard(Convert.ToInt32(WWtenant), ScacCode, ReferenceNo, Type, Bol);
					WriteLogMe("UnitedRequest After WindWard: " + WWtenant, Res, "UpsertTrackedShipments");

				}				
				return OceanInsight(Tenant, ScacCode, ReferenceNo, Type);
			}
			catch (Exception ex)
			{
				WriteLogMe("UnitedRequest Exception WindWard: " + ex.Message.ToString(), null, "UpsertTrackedShipments");

				return null;
			}

		}
        public Response OceanInsight(int Tenant, string ScacCode, string ReferenceNo, string Type)
        {
			//ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
			Response response = new Response();
			try
			{				
				SecurityUtility.AuthenticationOnTenant(Tenant);
				string OIToken = LogitudeSettings.OceanInsightsToken;
				//SecurityUtility.CheckContactFeature("Shipment", "UPDATE", entityPM.Tenant);//UPDATE//READ
				using (TransactionScope scope = TransactionFactory.GetTransaction())
				{

					IShipmentsContext objectContext = ShipmentsContext.GetContext(Tenant);
					OceanInsightsRequestRepository oceanInsightsRequestRepository = new OceanInsightsRequestRepository(objectContext);


					if (string.IsNullOrEmpty(ScacCode))
					{
						response.HasError = true;
						response.ErrorMessage = "ScacCode must have value";
						return response;
					}
					if (string.IsNullOrEmpty(ReferenceNo))
					{
						response.HasError = true;
						response.ErrorMessage = "ReferenceNo # must have value";
						return response;
					}
					if (response.HasError)
					{
						return response;
					}
					OceanInsightsRequestQuery query = new OceanInsightsRequestQuery(Tenant);
					OceanInsightsRequestPM OceanInsightsRequestPm;// = new OceanInsightsRequestPM();
					if (Type == "c_id")
					{
						OceanInsightsRequestPm = query.GetSinglePMByOceanInsightsByScacCodeContainerNoTenant(ScacCode, ReferenceNo, Tenant);
					}
					else
					{
						OceanInsightsRequestPm = query.GetSinglePMByOceanInsightsByCareierScacBLNoTenant(ScacCode, ReferenceNo, Tenant);
						if (OceanInsightsRequestPm == null)
						{
							OceanInsightsRequestPm = query.GetSinglePMByOceanInsightsByScacCodeContainerNoTenant(ScacCode, ReferenceNo, Tenant);
						}
					}

					//Hashtable Table = new Hashtable();
					//Table.Add("CONTAINER_NO", ContainerNo);
					//Table.Add("CARRIER_SCAC", ScacCode);
					//Table.Add("TOKEN", "a020db8267898a2502414e8479215ed32de41106");
					//Table.Add("REQ_ID","142707");
					bool UseOIV2 = FeatureToggleHelper.HasFeatureToggle("OI2", 0);
					ContainerTasks Task = new ContainerTasks(UseOIV2);
					string Result;
					string Status;
					string Errors;
					object Temp = null;//STARTMONITOR
					if (OceanInsightsRequestPm == null)
					{
						OceanInsightsRequestPm = new OceanInsightsRequestPM();
						if (Type == "c_id")
						{
							Task.StartMonitor(ScacCode, ReferenceNo, OIToken, out Result, out Status, out Errors);//ActivateOperation("STARTMONITOR", ref Table, ref Temp, out Result, out Status, out Errors);

						}
						else
						{
							Task.StartMonitor(ScacCode, ReferenceNo, OIToken, out Result, out Status, out Errors, false);
						}
						if (!string.IsNullOrEmpty(Errors) || !string.IsNullOrWhiteSpace(Errors))
						{
							string SearchErrors;
							Task.StartMonitorForExistedRequest(ReferenceNo, OIToken, out Result, out Status, out SearchErrors);
							if (!string.IsNullOrEmpty(SearchErrors) || !string.IsNullOrWhiteSpace(SearchErrors))
							{
								response.HasError = true;
								response.ErrorMessage = SearchErrors;
								return response;
							}
							else
							{
								XmlDocument xmldoc = new XmlDocument();
								xmldoc.LoadXml(Result);
								XmlNodeList nodeList = xmldoc.GetElementsByTagName("shipmentsubscription_id");
								string Id = string.Empty;
								foreach (XmlNode item in nodeList)
								{
									Id = item.InnerText;
								}
								OceanInsightsRequestService service = new OceanInsightsRequestService(objectContext, Tenant);
								if (Type == "c_id")
								{
									OceanInsightsRequestPm.ContainerNumber = ReferenceNo;
								}
								else
								{
									OceanInsightsRequestPm.BLNumber = ReferenceNo;
								}
								OceanInsightsRequestPm.SCACCode = ScacCode;
								OceanInsightsRequestPm.Tenant = Tenant;
								OceanInsightsRequestPm.OceanInsigntId = Id;
								OceanInsightsRequestPm.Type = Type;


								service.Create(OceanInsightsRequestPm);
							}
						}
						else
						{
							XmlDocument xmldoc = new XmlDocument();
							xmldoc.LoadXml(Result);
							XmlNodeList nodeList = xmldoc.GetElementsByTagName("id");
							string Id = string.Empty;
							foreach (XmlNode item in nodeList)
							{
								Id = item.InnerText;
							}
							OceanInsightsRequestService service = new OceanInsightsRequestService(objectContext, Tenant);
							if (Type == "c_id")
							{
								OceanInsightsRequestPm.ContainerNumber = ReferenceNo;
							}
							else
							{
								OceanInsightsRequestPm.BLNumber = ReferenceNo;
							}
							OceanInsightsRequestPm.SCACCode = ScacCode;
							OceanInsightsRequestPm.Tenant = Tenant;
							OceanInsightsRequestPm.OceanInsigntId = Id;
							OceanInsightsRequestPm.Type = Type;

							service.Create(OceanInsightsRequestPm);
						}
					}
					if (string.IsNullOrEmpty(OceanInsightsRequestPm.OceanInsigntId))
					{
						Task.StartMonitorForExistedRequest(ReferenceNo, OIToken, out Result, out Status, out Errors);
						if (!string.IsNullOrEmpty(Errors) || !string.IsNullOrWhiteSpace(Errors))
						{
							response.HasError = true;
							response.ErrorMessage = Errors;
							return response;
						}
						else
						{
							XmlDocument xmldoc = new XmlDocument();
							xmldoc.LoadXml(Result);
							XmlNodeList nodeList = xmldoc.GetElementsByTagName("shipmentsubscription_id");
							string Id = string.Empty;
							//foreach (XmlNode item in nodeList)
							//{
							Id = nodeList.Item(0).InnerText;//item.InnerText;
															//}
							OceanInsightsRequestService service = new OceanInsightsRequestService(objectContext, Tenant);
							OceanInsightsRequestPm.OceanInsigntId = Id;

							service.Update(OceanInsightsRequestPm);
						}
					}
					response.Result = OceanInsightsRequestPm.OceanInsigntId;
					scope.Complete();
					return response;
				}
			}
			catch (System.Data.Entity.Validation.DbEntityValidationException e)
			{
				string Error = "";
				foreach (var eve in e.EntityValidationErrors)
				{
					Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
						eve.Entry.Entity.GetType().Name, eve.Entry.State);
					foreach (var ve in eve.ValidationErrors)
					{
						Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
							ve.PropertyName, ve.ErrorMessage);

						Error += "- Property:" + ve.PropertyName + ", Error:" + ve.ErrorMessage + Environment.NewLine;
					}
				}
				response.HasError = true;
				response.ErrorMessage = Error;
				return response;

			}

			catch (Exception ex)
			{
				response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
				response.HasError = true;
				response.ErrorMessage = ex.Message;
				response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);

				if (!string.IsNullOrEmpty(ex.StackTrace))
				{
					response.ErrorMessage += Environment.NewLine + ex.StackTrace;
				}
				return response;
			}
		}
		private async Task<Response> WindWard(int WWtenant, string ScacCode, string ReferenceNo, string Type,string Bol = "")
        {
			WriteLogMe("UnitedRequest ENTER WindWard: " + WWtenant, null, "UpsertTrackedShipments");

			ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3 | SecurityProtocolType.Tls12 | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls;
			Response response = new Response();
			try
			{
				//SecurityUtility.AuthenticationOnTenant(WWtenant);
				
				using (TransactionScope scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
				{

					IShipmentsContext objectContext = ShipmentsContext.GetContext(WWtenant);

					if (string.IsNullOrEmpty(ScacCode))
					{
						response.HasError = true;
						response.ErrorMessage = "ScacCode must have value";
						return response;
					}
					if (string.IsNullOrEmpty(ReferenceNo))
					{
						response.HasError = true;
						response.ErrorMessage = "ReferenceNo # must have value";
						return response;
					}
				
					OceanInsightsRequestQuery query = new OceanInsightsRequestQuery(WWtenant);
					OceanInsightsRequestPM OceanInsightsRequestPm;// = new OceanInsightsRequestPM();
					if (Type == "c_id")
					{
						OceanInsightsRequestPm = query.GetSinglePMByOceanInsightsByScacCodeContainerNoTenant(ScacCode, ReferenceNo, WWtenant);
					}
					else
					{
						OceanInsightsRequestPm = query.GetSinglePMByOceanInsightsByCareierScacBLNoTenant(ScacCode, ReferenceNo, WWtenant);
						if (OceanInsightsRequestPm == null)
						{
							OceanInsightsRequestPm = query.GetSinglePMByOceanInsightsByScacCodeContainerNoTenant(ScacCode, ReferenceNo, WWtenant);
						}
					}
					string Result = "";
					string Status = "";
					string Errors = "";
					string Id = "";
					if (OceanInsightsRequestPm == null)
					{
						string JobNumber = GetJobNumber(WWtenant, ScacCode, ReferenceNo, Type);
						var WWResult = await UpsertTrackedShipments(ScacCode, ReferenceNo, Type, JobNumber, Bol);

						 Result = WWResult.v_result;
						 Status = WWResult.status;
						 Errors = WWResult.err_message;

						if (!string.IsNullOrEmpty(Errors) || !string.IsNullOrWhiteSpace(Errors))
						{
							response.HasError = true;
							response.ErrorMessage = Errors;
							return response;
						}
						else
						{
							 Id = Result;
							OceanInsightsRequestService service = new OceanInsightsRequestService(objectContext, WWtenant);
							
							OceanInsightsRequestPm = new OceanInsightsRequestPM();


							if (Type == "c_id")
							{
								OceanInsightsRequestPm.ContainerNumber = ReferenceNo;
							}
							else
							{
								OceanInsightsRequestPm.BLNumber = ReferenceNo;
							}
							OceanInsightsRequestPm.SCACCode = ScacCode;
							OceanInsightsRequestPm.Tenant = WWtenant;
							OceanInsightsRequestPm.OceanInsigntId = Id;
							OceanInsightsRequestPm.Type = Type;

							service.Create(OceanInsightsRequestPm);								
						}
					}					
					if (string.IsNullOrEmpty(OceanInsightsRequestPm.OceanInsigntId))
					{
						string JobNumber = GetJobNumber(WWtenant, ScacCode, ReferenceNo, Type);
						var WWResult = await UpsertTrackedShipments(ScacCode, ReferenceNo, Type, JobNumber, Bol);

						Result = WWResult.v_result;
						Status = WWResult.status;
						Errors = WWResult.err_message;

						if (!string.IsNullOrEmpty(Errors) || !string.IsNullOrWhiteSpace(Errors))
						{
							response.HasError = true;
							response.ErrorMessage = Errors;
							return response;
						}
						else
						{
							 Id = Result;
							OceanInsightsRequestService service = new OceanInsightsRequestService(objectContext, WWtenant);
							OceanInsightsRequestPm.OceanInsigntId = Id;

							service.Update(OceanInsightsRequestPm);
						}					
					}
					
                    response.Result = OceanInsightsRequestPm.OceanInsigntId;
					scope.Complete();
					//scope.Dispose();
					
				}
				return response;
			}
			catch (System.Data.Entity.Validation.DbEntityValidationException e)
			{
				string Error = "";
				foreach (var eve in e.EntityValidationErrors)
				{
					Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
						eve.Entry.Entity.GetType().Name, eve.Entry.State);
					foreach (var ve in eve.ValidationErrors)
					{
						Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
							ve.PropertyName, ve.ErrorMessage);

						Error += "- Property:" + ve.PropertyName + ", Error:" + ve.ErrorMessage + Environment.NewLine;
					}
				}
				response.HasError = true;
				response.ErrorMessage = Error;

				return response;

			}

			catch (Exception ex)
			{
				response.IsAuthenticationError = ex.GetType() == typeof(AutenticationException);
				response.HasError = true;
				response.ErrorMessage = ex.Message;
				response.InnerErrorMessage = (ex.InnerException != null ? (ex.InnerException.InnerException != null ? ex.InnerException.InnerException.Message : ex.InnerException.Message) : null);

				if (!string.IsNullOrEmpty(ex.StackTrace))
				{
					response.ErrorMessage += Environment.NewLine + ex.StackTrace;
				}
				return response;
			}
		}
		class Result
		{
			public string v_result { get; set; }

			public string status { get; set; }

			public string err_message { get; set; }	
		}
		private string GetJobNumber(int WWtenant, string ScacCode, string ReferenceNo, string Type)
		{
			string JobNumber = "";
			try
			{
				OceanInsightsRequestQuery query = new OceanInsightsRequestQuery(WWtenant);
				OceanInsightsRequestPM OceanInsightsRequestPm;// = new OceanInsightsRequestPM();
				if (Type == "c_id")
				{
					OceanInsightsRequestPm = query.GetSinglePMByOceanInsightsByScacCodeContainerNo(ScacCode, ReferenceNo);
				}
				else
				{
					OceanInsightsRequestPm = query.GetSinglePMByOceanInsightsByCareierScacBLNo(ScacCode, ReferenceNo);
					if (OceanInsightsRequestPm == null)
					{
						OceanInsightsRequestPm = query.GetSinglePMByOceanInsightsByScacCodeContainerNo(ScacCode, ReferenceNo);
					}
				}
				int num = 0;
				if (OceanInsightsRequestPm != null && int.TryParse(OceanInsightsRequestPm.OceanInsigntId, out num) && num < 0)
				{
					JobNumber = OceanInsightsRequestPm.OceanInsigntId;
				}
				else
				{
					JobNumber = "-" + CodeCounter.GetNumber("OceanInsightsRequest", 0).ToString();
				}
				return JobNumber;

			}
			catch (Exception ex)
			{
				throw new Exception("jobNumber is empty");
			}
		}

		private async Task<Result> UpsertTrackedShipments(string carrierSCAC, string ReferenceNo, string Type, string JobNumber,string Bol = "")
		{
			Result Result=new Result();
		
			try
			{
				if (string.IsNullOrEmpty(carrierSCAC) || string.IsNullOrWhiteSpace(carrierSCAC))
				{
					Result.err_message = "Parameter 'CarrierSCAC' (Standard Carrier Alpha Code) is missing !!!";
					Result.status = "-1";
					return Result;
				}
				if (string.IsNullOrEmpty(ReferenceNo) || string.IsNullOrWhiteSpace(ReferenceNo))
				{
					Result.err_message = "Parameter 'ContainerNo' is missing !!!";
					Result.status = "-1";
					return Result;
				}
				List<TrackedShipmentModel> shipments = new List<TrackedShipmentModel>();
				TrackedShipmentModel trackedShipmentModel = new TrackedShipmentModel();
				if (Type == "c_id")
				{
					trackedShipmentModel.containerNumber = ReferenceNo;
					trackedShipmentModel.bol = !string.IsNullOrEmpty(Bol)? Bol:null;
				}
				else
				{
					trackedShipmentModel.bol = ReferenceNo;
				}
				trackedShipmentModel.scac = carrierSCAC;
				trackedShipmentModel.carrierBookingReference = "";
                trackedShipmentModel.metadata.jobNumber = JobNumber;
				shipments.Add(trackedShipmentModel);

				List<Dictionary<string, object>> shipmentDictionaries = shipments.Select(shipment => new Dictionary<string, object>
				{
					{ "containerNumber", shipment?.containerNumber },
					{ "scac", shipment?.scac },
					{ "bol", shipment?.bol },
					{ "carrierBookingReference", shipment?.carrierBookingReference },
					{ "metadata", new Dictionary<string, object>
						{
							{ "jobNumber", shipment?.metadata?.jobNumber ?? Guid.NewGuid().ToString() }
						}
					}
				}).ToList();
				var trackedShipmentsService = new TrackedShipmentsAPI.Services.TrackedShipmentsService();
				var result = await trackedShipmentsService.UpsertTrackedShipments(shipmentDictionaries);
				dynamic data = JObject.Parse(result);
				WriteLogMe("UpsertTrackedShipments AFTER POST: ", data, "UpsertTrackedShipments");

				Result.v_result = data?.data?.upsertTrackedShipments[0]?.metadata?.jobNumber;
                return Result;
			}
			catch (WebException ex)
			{
				string message = "";
				var res = ((HttpWebResponse)ex.Response);
				if (res != null)
				{
					message = res.StatusCode + ": " + res.StatusDescription;
					message += Environment.NewLine +
						new StreamReader(res.GetResponseStream()).ReadToEnd();
				}
				Result.err_message = message;
				Result.status = "-1";
				return Result;

			}
			catch (Exception ex)
			{
				Result.err_message = ex.ToString();
				Result.status = "-1";
				return Result;
			}
			
		}

		public void WriteLogMe(string subject ,object obj,string funcName)
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

				if(obj!= null)
				{
					Newtonsoft.Json.JsonConvert.SerializeObject(obj);

				}
				LogitudeSettings.HandleLogMe(subject + logData, false, funcName, stopLogAt);
			}
			catch (Exception ex)
			{

			}
		}
		public Response GetStatus(string RequestId, string Type)
        {
            Response response = new Response();
            bool UseOIV2 = FeatureToggleHelper.HasFeatureToggle("OI2", 0);
            ContainerTasks Task = new ContainerTasks(UseOIV2);
            string Result;
            string Status;
            string Errors;
            string OIToken = LogitudeSettings.OceanInsightsToken;
            Task.GetStatus(RequestId, OIToken, out Result, out Status, out Errors, Type);//ActivateOperation("STARTMONITOR", ref Table, ref Temp, out Result, out Status, out Errors);
            if (!string.IsNullOrEmpty(Errors) || !string.IsNullOrWhiteSpace(Errors))
            {
                response.HasError = true;
                response.ErrorMessage = Errors;
                return response;
            }
            else
            {
                response.HasError = false;
                response.Result = Result;
                return response;
            }
        }
    }
}
