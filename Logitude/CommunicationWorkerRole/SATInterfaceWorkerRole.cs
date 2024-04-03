using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.QueueService;
using Logitude.SystemLogs;
using Microsoft.WindowsAzure.ServiceRuntime;
using Profact.TimbraCFDI;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using System.Xml;
using Simplog.Data.InvoiceModel;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.InvoiceModel.Tools;
using CommunicationWorkerRole.Services.SAT;
using Logitude.BL.InvoiceModel.Tools.EntityService;

namespace CommunicationWorkerRole
{
	public class SATInterfaceWorkerRole : WorkerEntryPoint
	{
		private bool isConcurrencyToggleEnabled = false;
		private IInvoiceContext invoiceContext;
		private ARInvoiceRepository arInvoiceRepository;
		private ARPaymentRepository arPaymentRepository;
		private ARInvoiceQuery arInvoiceQuery;		
		private ARPaymentQuery arPaymentQuery;
		private CommunicationLog communicationLog;
		private CommunicationLogRepository communicationLogRep;
		private ICommonDataContext commonContext;
		private int tenant = 0;
		public override void Run()
		{
			while (IsRunning)
			{
				if (!General.IsUpdating())
				{
					try
					{
						queueservice = new DbQueueService();
						queueservice.InitializeQueue("SATInterface", 0);

						var response = queueservice.Receive(new TimeSpan(0, 0, 0, 10));
						LastActivity = DateTime.UtcNow;

						if (response.MessageId != null)
						{

							string communicationLogId = response.MessageValues["CommunicationLogId"].ToString();
							int.TryParse(response.MessageValues["Tenant"].ToString(), out tenant);
							commonContext = CommonDataContext.GetContext(tenant);
							communicationLogRep = new CommunicationLogRepository(commonContext);
							communicationLog = communicationLogRep.GetSingleCommunicationLog(communicationLogId, tenant);
							isConcurrencyToggleEnabled = FeatureToggleHelper.HasFeatureToggle("INU", tenant);							
							this.Initialize();

							bool processEnebled = true;

							if (processEnebled)
							{
								if (communicationLog != null)
								{
									if (communicationLog.CommunicationStatusTypeCode == "D")
									{
										queueservice.Complete();
									}
									else
									{
										SendCommunicationLog();
										queueservice.Complete();
										LogDoneItemInMemory();
									}
								}
								else
								{
									if (response.RetryNumber <= 11)
									{
										if (response.RetryNumber < 3)
										{
											queueservice.Delay(new TimeSpan(0, 0, 0, 1));
										}

										if (response.RetryNumber >= 3 && response.RetryNumber <= 5)
										{
											queueservice.Delay(new TimeSpan(0, 0, 0, 5));
										}

										if (response.RetryNumber > 5 && response.RetryNumber <= 10)
										{
											queueservice.Delay(new TimeSpan(0, 0, 0, 10));
											AzureLog.SaveLogsInStorage("couldn't find communication log: " + communicationLogId + " ,tenant:" + tenant + ",retry number(DeliveryCount):" + response.RetryNumber
												+ ",at utc time:" + DateTime.UtcNow + ",at SATInterface worker role.", "L", DateTime.UtcNow, "", "", 0, null, null, null);
											Thread.Sleep(3000);
										}
										if (response.RetryNumber == 11)
										{
											queueservice.Delay(new TimeSpan(0, 0, 2, 0));
											AzureLog.SaveLogsInStorage("couldn't find communication log: " + communicationLogId + " ,tenant:" + tenant + ",retry number(DeliveryCount):" + response.RetryNumber
											+ ",at utc time:" + DateTime.UtcNow + ",at SATInterface worker role.", "L", DateTime.UtcNow, "", "", 0, null, null, null);
											Thread.Sleep(10000);
										}
									}
									else
									{
										queueservice.Complete();
										AzureLog.SaveLogsInStorage("couldn't find communication log and the message is completed: " + communicationLogId + " ,tenant:" + tenant + ",retry number(DeliveryCount):" + response.RetryNumber
											+ ",at utc time:" + DateTime.UtcNow + ",at SATInterface worker role.", "L", DateTime.UtcNow, "", "", 0, null, null, null);
									}
								}
							}
						}
					}
					catch (Exception ex)
					{
						ConnectClient();
						ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "SATInterface worker role start", null, null);
						Thread.Sleep(10000);
					}

				}
				else
				{
					Thread.Sleep(60000);
				}
			}
		}
		private void Initialize()
		{
			invoiceContext = InvoiceContext.GetContext(tenant);
			arInvoiceRepository = new ARInvoiceRepository(invoiceContext);
			arPaymentRepository = new ARPaymentRepository(invoiceContext);
			arInvoiceQuery = new ARInvoiceQuery(arInvoiceRepository);
			arPaymentQuery = new ARPaymentQuery(arPaymentRepository);
		}

		private void SetNextTryDateTime()
		{
			DateTime date = TenantServerConfigration.GetCurrentDateTime(tenant);
			DateTime dateUtc = DateTime.UtcNow;
			switch (communicationLog.Retries)
			{
				case 1:
				case 2:
					{
						communicationLog.NextTryDateTime = date.AddSeconds(1);
						communicationLog.NextTryDateTimeUTC = dateUtc.AddSeconds(1);
						queueservice.Delay(new TimeSpan(0, 0, 0, 1));

						break;
					}
				case 3:
				case 4:
					{
						communicationLog.NextTryDateTime = date.AddSeconds(5);
						communicationLog.NextTryDateTimeUTC = dateUtc.AddSeconds(5);
						queueservice.Delay(new TimeSpan(0, 0, 0, 5));
						break;
					}
				case 5:
					{
						communicationLog.NextTryDateTime = date.AddMinutes(1);
						communicationLog.NextTryDateTimeUTC = dateUtc.AddMinutes(1);
						queueservice.Delay(new TimeSpan(0, 0, 1));
						break;
					}
				case 6:
				case 7:
				case 8:
					{
						communicationLog.NextTryDateTime = date.AddMinutes(2);
						communicationLog.NextTryDateTimeUTC = dateUtc.AddMinutes(2);
						queueservice.Delay(new TimeSpan(0, 0, 2));
						break;
					}
				case 9:
					{
						communicationLog.NextTryDateTime = date.AddMinutes(5);
						communicationLog.NextTryDateTimeUTC = dateUtc.AddMinutes(5);
						queueservice.Delay(new TimeSpan(0, 0, 5));
						break;
					}
				case 10:
					{
						communicationLog.NextTryDateTime = date.AddMinutes(10);
						communicationLog.NextTryDateTimeUTC = dateUtc.AddMinutes(10);
						queueservice.Delay(new TimeSpan(0, 0, 10));
						break;
					}
				default:
					{
						communicationLog.NextTryDateTime = date.AddMinutes(20);
						communicationLog.NextTryDateTimeUTC = dateUtc.AddMinutes(20);
						queueservice.Delay(new TimeSpan(0, 0, 20));
						break;
					}
			}
			communicationLog.Logs += Environment.NewLine + "Retry #" + communicationLog.Retries + " Next Retry: " + communicationLog.NextTryDateTimeUTC.ToString();
		}

		#region SendCommunicationLog

		
		private void SendCommunicationLog()
		{
			try
			{
				if (communicationLog.Retries < 5)
				{
					SendWaitingCommunicationLog();
				}

				else
				{
					if (commonContext != null)
					{
						SetRelatedEntityTransferStatusToTransferError(communicationLog);

						communicationLog.CommunicationStatusTypeCode = "F";
						communicationLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant);
						communicationLogRep.Update(communicationLog);
						communicationLogRep.SubmitChanges();
						queueservice.Complete();
					}
				}
			}

			catch (Exception exc)
			{
				ExceptionHandler.HandleException(exc, DateTime.Now, tenant, "", "WorkerRole", "", null);

				//Change number of retries

				communicationLog.Retries++;
				communicationLog.ExceptionMessage = exc.Message;
				if (exc.InnerException != null)
				{
					communicationLog.ExceptionMessage = communicationLog.ExceptionMessage + Environment.NewLine + exc.InnerException;
				}
				if (exc.StackTrace != null)
				{
					communicationLog.ExceptionMessage = communicationLog.ExceptionMessage + Environment.NewLine + "Stack trace: " + exc.StackTrace;
				}

				communicationLog.ExceptionMessage = StringHelper.TruncateLongString(communicationLog.ExceptionMessage, 7000);
				SetNextTryDateTime();
				if (commonContext != null)
				{
					communicationLogRep.Update(communicationLog);
					communicationLogRep.SubmitChanges();
				}
				throw;

			}
		}

		private void SetRelatedEntityTransferStatusToTransferError(CommunicationLog waitingCommLog)
		{
			if (waitingCommLog.Subject == "Payment SAT Interface")
			{
				if (isConcurrencyToggleEnabled)
				{
					ARPaymentPM payment = arPaymentQuery.GetSinglePM(waitingCommLog.EntityId, waitingCommLog.Tenant);
					if (payment != null && payment.SATTransferStatusCode != "TE")
					{
						payment.IsUpdatedBySAT = true;
						payment.SATTransferStatusCode = "TE";
						this.SaveARPayment_UpdateService(payment);
					}
				}

				else
                {
					ARPayment payment = arPaymentRepository.GetSingleARPayment(waitingCommLog.EntityId, waitingCommLog.Tenant);
					if (payment != null && payment.SATTransferStatusCode != "TE")
					{
						payment.SATTransferStatusCode = "TE";
						this.SaveARPayment_Repository(payment);
					}
				}
			}
			else
			{
				if (isConcurrencyToggleEnabled)
				{
					ARInvoicePM invoice = arInvoiceQuery.GetSinglePM(waitingCommLog.EntityId, waitingCommLog.Tenant);
					if (invoice != null && invoice.SATTransferStatusCode != "TE")
					{
						invoice.IsUpdatedBySAT = true;
						invoice.SATTransferStatusCode = "TE";
						this.SaveARInvoice_UpdateService(invoice);
					}
				}

				else
                {
					ARInvoice invoice = arInvoiceRepository.GetSingleARInvoice(waitingCommLog.EntityId, waitingCommLog.Tenant);
					if (invoice != null && invoice.SATTransferStatusCode != "TE")
					{
						invoice.SATTransferStatusCode = "TE";
						this.SaveARInvoice_Repository(invoice);
					}
				}
			}
		}

		private void SendWaitingCommunicationLog()
		{
			switch (communicationLog.Subject)
			{
				case "SAT Interface Cancellation Request":
					SendProfactCancellationRequest();
					break;

				case "Payment SAT Interface Cancellation":
					SendPaymentProfactCancellationRequest();
					break;

				default:
					SendProfactRequest();
					break;
			}
		}

		#endregion

		#region SendProfactRequest

		private void SendProfactRequest()
		{
			if (communicationLog.Document != null)
			{
				string filename = communicationLog.DocumentId + "." + communicationLog.Document.Extension;

				Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
				{
					FileName = communicationLog.Document.Id,
					FolderName = communicationLog.Document.Folder,
					Extension = communicationLog.Document.Extension,
					Tenant = communicationLog.Document.Tenant,
					FileSize = communicationLog.Document.FileSize,
				};

				Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new Microsoft.Practices.Unity.ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
				byte[] datainByte = storageservice.Read(fileInfo);

				if (datainByte != null)
				{
					SATInterfaceSettingRepository sATInterfaceSettingRepository = new SATInterfaceSettingRepository(tenant);
					SATInterfaceSetting satSetting = sATInterfaceSettingRepository.GetSingleSATInterfaceSetting(tenant);
					if (satSetting != null)
					{
						if (satSetting.SATInterfaceCode == "PROF40")
						{
							SATProfact40Service sATProfact40Service = new SATProfact40Service(commonContext, queueservice);
							sATProfact40Service.SendRequest(new SATProfact40ServiceArgs { WaitingCommLog = communicationLog, CommunicationLogRep = communicationLogRep, DatainByte = datainByte, SatSetting = satSetting });
						}
						else
							SendProfact33Request(datainByte, satSetting);
					}
				}
			}
		}

		private void SendProfact33Request(byte[] datainByte, SATInterfaceSetting satSetting)
		{
			SATInterfaceHelper sATInterfaceHelper = new SATInterfaceHelper();

			bool isProduction = satSetting.Token != "mvpNUXmQfK8=";

			Profact.TimbraCFDI33.Conector conector = new Profact.TimbraCFDI33.Conector(isProduction);
			Profact.TimbraCFDI33.Comprobante comprobante = LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI33.Comprobante>(datainByte);

			//Establecemos las credenciales para el permiso de conexión

			conector.EstableceCredenciales(satSetting.Token);
			//Timbramos el CFDI por medio del conector y guardamos resultado
			ResultadoTimbre resultadoTimbre = conector.TimbraCFDI(comprobante);

			//Verificamos el resultado
			if (resultadoTimbre.Exitoso)
			{
				if (communicationLog.Subject == "Payment SAT Interface")
				{
					ARPaymentPM payment = arPaymentQuery.GetSinglePM(communicationLog.EntityId, tenant);
					if (payment != null)
					{
						SATAdditionalFields additional = new SATAdditionalFields()
						{
							CadenaOriginal = resultadoTimbre.CadenaTimbre,
						};

						if (resultadoTimbre.CodigoBidimensional != null)
						{
							additional.QRImage = Convert.ToBase64String(resultadoTimbre.CodigoBidimensional);//imagedetail.Id;//
						}

						if (isConcurrencyToggleEnabled)
						{
							payment.IsUpdatedBySAT = true;
							payment.SATApprovalDate = GetSATApprovalDateFromComplemento(comprobante.Complemento.Any);
							payment.SATAdditionalFieldsXML = LogitudeXmlSerializer.SerializeObjectToXmlString(additional);
							payment.SATXML = resultadoTimbre.Xml;
							payment.SATTransferStatusCode = "TD";
							payment.TransmissionError = null;
							this.SaveARPayment_UpdateService(payment);
						}

						else
                        {
							ARPayment paymentPOCO = arPaymentRepository.GetSingleARPayment(communicationLog.EntityId, tenant);
							if(paymentPOCO != null)
                            {
								paymentPOCO.SATApprovalDate = GetSATApprovalDateFromComplemento(comprobante.Complemento.Any);
								paymentPOCO.SATAdditionalFieldsXML = LogitudeXmlSerializer.SerializeObjectToXmlString(additional);
								paymentPOCO.SATXML = resultadoTimbre.Xml;
								paymentPOCO.SATTransferStatusCode = "TD";
								paymentPOCO.TransmissionError = null;
								this.SaveARPayment_Repository(paymentPOCO);
							}
						}

						sATInterfaceHelper.UpdatePaymentInvoicesSATStatus(payment.Id, tenant, comprobante.Complemento.Any[0], arInvoiceRepository, arPaymentRepository);

						Encoding encoding = Encoding.UTF8;
						byte[] xmlfile = encoding.GetBytes(resultadoTimbre.Xml);
						CreateSATPaymentDocument(payment, xmlfile, true);

						EventTracer.CreateTraceEvent(new EventTracerArgs()
						{
							EntityId = communicationLog.EntityId,
							ObjectTableName = communicationLog.ObjectTable.Name,
							Tenant = tenant,
							UserId = communicationLog.CreatedByUserId,
							EventTypeCode = "PAAS",
						});
					}
				}
				else
				{
					ARInvoicePM invoice = arInvoiceQuery.GetSinglePM(communicationLog.EntityId, tenant);
					if (invoice != null)
					{
						SATAdditionalFields additional = new SATAdditionalFields()
						{
							CadenaOriginal = resultadoTimbre.CadenaTimbre,
						};

						if (resultadoTimbre.CodigoBidimensional != null)
						{
							additional.QRImage = Convert.ToBase64String(resultadoTimbre.CodigoBidimensional);//imagedetail.Id;//
						}

						Profact.TimbraCFDI33.Comprobante resultComprobante = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI33.Comprobante>(resultadoTimbre.Xml);

						if (isConcurrencyToggleEnabled)
						{
							invoice.SATApprovalDate = GetSATApprovalDateFromComplemento(resultComprobante.Complemento.Any);
							invoice.IsUpdatedBySAT = true;
							invoice.SATXML = resultadoTimbre.Xml;
							invoice.SATTransferStatusCode = "TD";
							invoice.TransmissionError = null;
							invoice.SATInvoiceStatusCode = "OP";
							invoice.SATAdditionalFieldsXML = LogitudeXmlSerializer.SerializeObjectToXmlString(additional);
							this.SaveARInvoice_UpdateService(invoice);
						}

                        else 
						{
							ARInvoice invoicePOCO = arInvoiceRepository.GetSingleARInvoice(communicationLog.EntityId, tenant);
							if(invoicePOCO != null)
                            {
								invoicePOCO.SATApprovalDate = GetSATApprovalDateFromComplemento(resultComprobante.Complemento.Any);
								invoicePOCO.SATXML = resultadoTimbre.Xml;
								invoicePOCO.SATTransferStatusCode = "TD";
								invoicePOCO.TransmissionError = null;
								invoicePOCO.SATInvoiceStatusCode = "OP";
								invoicePOCO.SATAdditionalFieldsXML = LogitudeXmlSerializer.SerializeObjectToXmlString(additional);
								this.SaveARInvoice_Repository(invoicePOCO);
							}
						}

						Encoding encoding = Encoding.UTF8;
						byte[] xmlfile = encoding.GetBytes(resultadoTimbre.Xml);

						CreateSATDocument(invoice, xmlfile);

						EventTracer.CreateTraceEvent(new EventTracerArgs()
						{
							EntityId = communicationLog.EntityId,
							ObjectTableName = communicationLog.ObjectTable.Name,
							Tenant = tenant,
							UserId = communicationLog.CreatedByUserId,
							EventTypeCode = "INAS",
						});
					}
				}

				communicationLog.CommunicationStatusTypeCode = "D";
				communicationLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(tenant);
				communicationLog.DoneDateUTC = DateTime.UtcNow;
				communicationLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant);
				communicationLog.LastStatusDateUTC = DateTime.UtcNow;
				communicationLogRep.Update(communicationLog);
				communicationLogRep.SubmitChanges();

			}
			else
			{

				if (resultadoTimbre.Descripcion != null && resultadoTimbre.Descripcion.Contains("Este CFDI ya ha sido timbrado con UUID"))
				{
					GetProfactSentXML(conector, resultadoTimbre);
				}
				else
				{
					string transError = resultadoTimbre.Descripcion;					
					if (!string.IsNullOrEmpty(resultadoTimbre.Descripcion))
					{
						transError = resultadoTimbre.Descripcion.Replace("Error en la validación de estructura xsd:", "").ToString().Trim();
						if (!string.IsNullOrEmpty(resultadoTimbre.DescripcionInterna))
						{
							transError += Environment.NewLine + resultadoTimbre.DescripcionInterna;
						}
					}

					if (communicationLog.Subject == "Payment SAT Interface")
					{						
							HandlePaymentError(transError);						
					}
					else
					{
						ARInvoicePM invoice = arInvoiceQuery.GetSinglePM(communicationLog.EntityId, tenant);						
						HandleInvoiceError(transError, invoice);
					}
				}
			}
		}

		private void GetProfactSentXML(Profact.TimbraCFDI33.Conector conector, ResultadoTimbre resultadoTimbre)
		{
			SATInterfaceHelper sATInterfaceHelper = new SATInterfaceHelper();
			string transError = ResolveTransmisionError(resultadoTimbre);
			string[] descrip = resultadoTimbre.Descripcion.Split(':');
			ICommonDataContext commonContext = CommonDataContext.GetContext(tenant);
			TenantRepository tenantRepository = new TenantRepository(commonContext);
			Tenant currentTenant = tenantRepository.GetSingleTenant(tenant);

			if (communicationLog.Subject == "Payment SAT Interface")
			{
				if (descrip.Length > 1)
				{
					string folioFiscal = descrip[descrip.Length - 1];
					string rfcEmisor = currentTenant.VatNumber;
					ResultadoConsulta resultadoConsulta = conector.ObtieneCFDI(rfcEmisor, folioFiscal);
					ARPaymentPM payment = arPaymentQuery.GetSinglePM(communicationLog.EntityId, tenant);

					if (payment != null && resultadoConsulta.Exitoso)
					{
						Profact.TimbraCFDI33.Comprobante paymentComprobante = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI33.Comprobante>(resultadoConsulta.Xml);

						SATAdditionalFields additional = new SATAdditionalFields()
						{
							CadenaOriginal = resultadoConsulta.CadenaTimbre,
						};

						if (resultadoConsulta.CodigoBidimensional != null)
						{
							additional.QRImage = Convert.ToBase64String(resultadoConsulta.CodigoBidimensional);//imagedetail.Id;//
						}

						if (isConcurrencyToggleEnabled)
						{
							payment.IsUpdatedBySAT = true;
							payment.SATApprovalDate = GetSATApprovalDateFromComplemento(paymentComprobante.Complemento.Any);
							payment.SATAdditionalFieldsXML = LogitudeXmlSerializer.SerializeObjectToXmlString(additional);
							payment.SATXML = resultadoConsulta.Xml;
							payment.SATTransferStatusCode = "TD";
							payment.TransmissionError = null;
							this.SaveARPayment_UpdateService(payment);
						}

						else
                        {
							ARPayment paymentPOCO = arPaymentRepository.GetSingleARPayment(communicationLog.EntityId, tenant);
							if(paymentPOCO != null)
                            {
								paymentPOCO.SATApprovalDate = GetSATApprovalDateFromComplemento(paymentComprobante.Complemento.Any);
								paymentPOCO.SATAdditionalFieldsXML = LogitudeXmlSerializer.SerializeObjectToXmlString(additional);
								paymentPOCO.SATXML = resultadoConsulta.Xml;
								paymentPOCO.SATTransferStatusCode = "TD";
								paymentPOCO.TransmissionError = null;
								this.SaveARPayment_Repository(paymentPOCO);
							}
						}

						sATInterfaceHelper.UpdatePaymentInvoicesSATStatus(payment.Id, tenant, paymentComprobante.Complemento.Any[0], arInvoiceRepository, arPaymentRepository);

						Encoding encoding = Encoding.UTF8;
						byte[] xmlfile = encoding.GetBytes(resultadoConsulta.Xml);

						CreateSATPaymentDocument(payment, xmlfile, true);

						communicationLog.CommunicationStatusTypeCode = "D";
						communicationLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(tenant);
						communicationLog.DoneDateUTC = DateTime.UtcNow;
						communicationLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant);
						communicationLog.LastStatusDateUTC = DateTime.UtcNow;
						communicationLogRep.Update(communicationLog);
						communicationLogRep.SubmitChanges();

						EventTracer.CreateTraceEvent(new EventTracerArgs()
						{
							EntityId = communicationLog.EntityId,
							ObjectTableName = communicationLog.ObjectTable.Name,
							Tenant = tenant,
							UserId = communicationLog.CreatedByUserId,
							EventTypeCode = "PAAS",
						});
					}
					else
					{
						HandlePaymentError(transError);
					}
				}
				else
				{
					HandlePaymentError(transError);
				}
			}
			else
			{
				//Este CFDI ya ha sido timbrado con UUID: { 0}
				//Este CFDI ya ha sido timbrado con UUID: 09acd5a4 - c544 - 45af - b897 - f64b13f60606

				ARInvoicePM invoice = arInvoiceQuery.GetSinglePM(communicationLog.EntityId, tenant);
				descrip = resultadoTimbre.Descripcion.Split(':');
				if (descrip.Length > 1)
				{
					string folioFiscal = descrip[descrip.Length - 1];
					string rfcEmisor = currentTenant.VatNumber;
					ResultadoConsulta resultadoConsulta = conector.ObtieneCFDI(rfcEmisor, folioFiscal);
					if (resultadoConsulta.Exitoso)
					{
						SATAdditionalFields additional = new SATAdditionalFields()
						{
							CadenaOriginal = resultadoConsulta.CadenaTimbre,
						};

						if (resultadoConsulta.CodigoBidimensional != null)
						{
							additional.QRImage = Convert.ToBase64String(resultadoConsulta.CodigoBidimensional);//imagedetail.Id;//
						}

						Profact.TimbraCFDI33.Comprobante invoiceComprobante = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI33.Comprobante>(resultadoConsulta.Xml);

						if (isConcurrencyToggleEnabled)
						{
							invoice.IsUpdatedBySAT = true;
							invoice.SATApprovalDate = GetSATApprovalDateFromComplemento(invoiceComprobante.Complemento.Any);
							invoice.SATXML = resultadoConsulta.Xml;
							invoice.SATAdditionalFieldsXML = LogitudeXmlSerializer.SerializeObjectToXmlString<SATAdditionalFields>(additional);
							invoice.SATTransferStatusCode = "TD";
							invoice.TransmissionError = null;
							this.SaveARInvoice_UpdateService(invoice);
						}

						else
                        {
							ARInvoice invoicePOCO = arInvoiceRepository.GetSingleARInvoice(communicationLog.EntityId, tenant);
							if(invoicePOCO != null)
                            {
								invoicePOCO.SATApprovalDate = GetSATApprovalDateFromComplemento(invoiceComprobante.Complemento.Any);
								invoicePOCO.SATXML = resultadoConsulta.Xml;
								invoicePOCO.SATAdditionalFieldsXML = LogitudeXmlSerializer.SerializeObjectToXmlString<SATAdditionalFields>(additional);
								invoicePOCO.SATTransferStatusCode = "TD";
								invoicePOCO.TransmissionError = null;
								this.SaveARInvoice_Repository(invoicePOCO);
							}
						}

						Encoding encoding = Encoding.UTF8;
						byte[] xmlfile = encoding.GetBytes(resultadoConsulta.Xml);

						CreateSATDocument(invoice, xmlfile, true);

						communicationLog.CommunicationStatusTypeCode = "D";
						communicationLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(tenant);
						communicationLog.DoneDateUTC = DateTime.UtcNow;
						communicationLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant);
						communicationLog.LastStatusDateUTC = DateTime.UtcNow;
						communicationLogRep.Update(communicationLog);
						communicationLogRep.SubmitChanges();

						EventTracer.CreateTraceEvent(new EventTracerArgs()
						{
							EntityId = communicationLog.EntityId,
							ObjectTableName = communicationLog.ObjectTable.Name,
							Tenant = tenant,
							UserId = communicationLog.CreatedByUserId,
							EventTypeCode = "INAS",
						});

					}
					else
					{
						HandleInvoiceError(transError, invoice);
					}
				}
				else
				{
					HandleInvoiceError(transError, invoice);
				}
			}
		}

		private DateTime GetSATApprovalDateFromComplemento(XmlElement[] anycomplemento)
		{
			if (anycomplemento != null)
			{
				List<System.Xml.XmlElement> myLXmlComplementos = anycomplemento.ToList<System.Xml.XmlElement>();
				var timbreFiscalDigitalElement = myLXmlComplementos.Where(el => el.Name == "tfd:TimbreFiscalDigital").FirstOrDefault();
				if (timbreFiscalDigitalElement != null)
				{
					Profact.TimbraCFDI.TimbreFiscalDigital digitalTi = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(timbreFiscalDigitalElement.OuterXml);

					if (digitalTi.FechaTimbrado != null && !digitalTi.FechaTimbrado.Equals(DateTime.MinValue))
					{
						return digitalTi.FechaTimbrado;
					}
					else
					{
						return TenantServerConfigration.GetCurrentDateTime(tenant);
					}
				}
				else
				{
					return TenantServerConfigration.GetCurrentDateTime(tenant);
				}
			}
			else
			{
				return TenantServerConfigration.GetCurrentDateTime(tenant);
			}
		}

		private string ResolveTransmisionError(ResultadoTimbre resultadoTimbre)
		{
			string transError = resultadoTimbre.Descripcion;
			if (!string.IsNullOrEmpty(resultadoTimbre.Descripcion))
			{
				transError = resultadoTimbre.Descripcion.Replace("Error en la validación de estructura xsd:", "").ToString().Trim();
				if (!string.IsNullOrEmpty(resultadoTimbre.DescripcionInterna))
				{
					transError += Environment.NewLine + resultadoTimbre.DescripcionInterna;
				}

			}

			return transError;
		}

		private void HandlePaymentError(string transError)
		{
			ARPaymentPM payment = arPaymentQuery.GetSinglePM(communicationLog.EntityId, tenant);
			if (payment != null)
			{
				if (transError != payment.TransmissionError || payment.SATTransferStatusCode != "TE")
				{
					if (isConcurrencyToggleEnabled)
					{
						payment.IsUpdatedBySAT = true;
						payment.SATTransferStatusCode = "TE";
						payment.TransmissionError = transError;
						SaveARPayment_UpdateService(payment);
					}

					else
                    {
						ARPayment paymentPOCO = arPaymentRepository.GetSingleARPayment(communicationLog.EntityId, tenant);
						if(paymentPOCO != null)
                        {
							paymentPOCO.SATTransferStatusCode = "TE";
							paymentPOCO.TransmissionError = transError;
							SaveARPayment_Repository(paymentPOCO);
						}
					}
				}
			}

			SaveCommunicationLogAsDoneWithSATError(transError);
		}

		private void HandleInvoiceError(string transError, ARInvoicePM invoice)
		{
			if (transError != invoice.TransmissionError || invoice.SATTransferStatusCode != "TE")
			{
				if (isConcurrencyToggleEnabled)
				{
					invoice.IsUpdatedBySAT = true;
					invoice.SATTransferStatusCode = "TE";
					invoice.TransmissionError = transError;
					SaveARInvoice_UpdateService(invoice);
				}

                else
                {
					ARInvoice invoicePOCO = arInvoiceRepository.GetSingleARInvoice(communicationLog.EntityId, tenant);
					if(invoicePOCO != null)
                    {
						invoicePOCO.SATTransferStatusCode = "TE";
						invoicePOCO.TransmissionError = transError;
						SaveARInvoice_Repository(invoicePOCO);
					}
				}
			}

			SaveCommunicationLogAsDoneWithSATError(transError);
		}

		private void SaveCommunicationLogAsDoneWithSATError(string transError)
		{
			string exceptionMessage = "Done, with SAT Error: " + transError;
			communicationLog.CommunicationStatusTypeCode = "D";
			communicationLog.ExceptionMessage = StringHelper.TruncateLongString(exceptionMessage, 7000);
			communicationLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant);

			if (commonContext != null)
			{
				communicationLogRep.Update(communicationLog);
				communicationLogRep.SubmitChanges();
			}

			queueservice.Complete();
		}
		#endregion

		#region CreateSATDocument
		private void CreateSATDocument(ARInvoicePM invoice, byte[] fileData, bool checkIfExists = false)
		{
			int tenant = invoice.Tenant;
			ICommonDataContext objectContext = CommonDataContext.GetContext(invoice.Tenant);
			DocumentTypeRepository documentTypeRep = new DocumentTypeRepository(objectContext);
			DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(invoice.Tenant);
			Simplog.Data.InfrastructureModel.Repositories.ObjectTableRepository tableRepository = new Simplog.Data.InfrastructureModel.Repositories.ObjectTableRepository(invoice.Tenant);
			Simplog.Data.InfrastructureModel.EntityPOCOs.ObjectTable arObjectTable = tableRepository.GetObjectTableByName("ARInvoice", 0, false);
			Simplog.Data.InfrastructureModel.EntityPOCOs.ObjectTable shipmentObjectTable = tableRepository.GetObjectTableByName("Shipment", 0, false);
			Simplog.Data.InfrastructureModel.EntityPOCOs.ObjectTable masterObjectTable = tableRepository.GetObjectTableByName("Master", 0, false);

			ObjectTable entityObjectTable = shipmentObjectTable;
			ARInvoicePM invoicePM = arInvoiceQuery.GetSinglePM(invoice.Id, invoice.Tenant);
			ContactRepository contactRepository = new ContactRepository(tenant);
			Contact systemUser = contactRepository.GetSingleContactByEmail("system@tenant" + tenant + ".com", tenant, true);
			DocumentType docType = documentTypeRep.GetDocumentTypeByCode("SATXML", invoice.Tenant);

			if (docType == null)
			{
				throw new Exception("SATXML document type couldn't be found!");
			}

			if (invoicePM.InvoiceEntities.Count() > 1)
			{
				entityObjectTable = masterObjectTable;
			}

			else if (invoicePM.InvoiceEntities.Count() == 1)
			{
				entityObjectTable = shipmentObjectTable;
				var currentInvoiceEntity = invoicePM.InvoiceEntities.Where(a => a.ObjectTableId == entityObjectTable.Id).FirstOrDefault();
				if (currentInvoiceEntity == null)
				{
					entityObjectTable = masterObjectTable;
				}
			}

			if (checkIfExists)
			{
				DocumentsFilingPM documentFiling = documentsFilingQuery.GetDocumentsFilingPMsByEntityIdAndObjectTable(invoice.MainEntityId, invoice.Id, entityObjectTable.Id, "I", tenant).Where(f => f.DocumentTypeId == docType.Id).FirstOrDefault();
				if (documentFiling != null && documentFiling.HasFile)
				{
					return;
				}
			}

			Logitude.BL.CommonDataModel.Tools.EntityService.DocumentsFilingService documentsService = new Logitude.BL.CommonDataModel.Tools.EntityService.DocumentsFilingService(objectContext, invoice.Tenant);

			Logitude.BL.CommonDataModel.EntityPMs.DocumentsFilingPM extDocPM = new Logitude.BL.CommonDataModel.EntityPMs.DocumentsFilingPM()
			{
				Id = Logitude.Server.Tools.Counters.IdCounter.GetNumber("DocumentsFiling", tenant).ToString(),
				DirectionCode = "I",
				Tenant = invoice.Tenant,
				DocumentTypeId = docType.Id,
				CreatedByUserId = systemUser.Id,
				CreateDate = DateTime.Now,
				OwnerId = systemUser.Id,
				UpdatedByUserId = systemUser.Id,
				ExternalEntityName = "ARInvoice",
				EntityReference = invoice.InvoiceNumber,
				FileExtension = "xml",
				FileSize = fileData.Length,
				Folder = "docsin",
				HasFile = true,
				FileName = "SAT XML",
				Received = true,
				ReceivedDate = DateTime.Now,
				ReceivedByUserId = systemUser.Id,
				Description = "SAT XML",
				IsFromUnifreightPodMobile = true,
				EntityId = string.IsNullOrEmpty(invoice.MainEntityId) ? invoice.Id : invoice.MainEntityId,
				EntityNumber = string.IsNullOrEmpty(invoice.MainEntityReference) ? invoice.InvoiceNumber : invoice.MainEntityReference,
				ChildEntityId = invoice.Id,
				ChildObjectTableId = arObjectTable.Id,
				ChildEntityReference = invoice.InvoiceNumber,
			};

			extDocPM.ObjectTableId = !invoice.IsConsolidationInvoice ? entityObjectTable.Id : arObjectTable.Id;
			documentsService.Create(extDocPM, fileData, systemUser.Id, false);
		}
		#endregion

		#region SendProfactCancellationRequest
		private void SendProfactCancellationRequest()
		{
			ARInvoice invoicePOCO = arInvoiceRepository.GetSingleInvoice(communicationLog.EntityId);
			ARInvoicePM invoice = arInvoiceQuery.GetSinglePM(communicationLog.EntityId, tenant);
			if (!string.IsNullOrEmpty(invoice.SATXML))
			{
				SATInterfaceSettingRepository sATInterfaceSettingRepository = new SATInterfaceSettingRepository(tenant);
				SATInterfaceSetting satSetting = sATInterfaceSettingRepository.GetSingleSATInterfaceSetting(tenant);
				if (satSetting != null)
				{
					if (satSetting.SATInterfaceCode == "PROF40")
						SATInvoiceProfact40CancellationService.SendRequest(new SATInvoiceProfact40CancellationServiceArgs { WaitingCommLog = communicationLog, CommunicationLogRep = communicationLogRep, ARInvoice = invoicePOCO, ARInvoicePM = invoice, ARInvoiceRep = arInvoiceRepository, SatSetting = satSetting });
					else
						SendProfactCancellation33(invoice, satSetting);
				}
			}
		}


		private void SendProfactCancellation33(ARInvoicePM invoice, SATInterfaceSetting satSetting)
		{
			bool isProduction = satSetting.Token != "mvpNUXmQfK8=";
			Profact.TimbraCFDI33.Conector conector = new Profact.TimbraCFDI33.Conector(isProduction);
			//Establecemos las credenciales para el permiso de conexión
			conector.EstableceCredenciales(satSetting.Token);

			ComprobanteDetails comprobanteDetails = GetcomprobanteDetails(invoice.SATXML);
			ARInvoice invoicePOCO = arInvoiceRepository.GetSingleARInvoice(invoice.Id, tenant);

			if (comprobanteDetails.ComplementoAny != null && invoicePOCO != null)
			{
				List<System.Xml.XmlElement> myLXmlComplementos = comprobanteDetails.ComplementoAny.ToList<System.Xml.XmlElement>();
				var timbreFiscalDigitalElement = myLXmlComplementos.Where(el => el.Name == "tfd:TimbreFiscalDigital").FirstOrDefault();
				if (timbreFiscalDigitalElement != null)
				{
					Profact.TimbraCFDI.TimbreFiscalDigital digitalTi = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(timbreFiscalDigitalElement.OuterXml);


					//Rfc Emisor
					string rfcEmisor = comprobanteDetails.RfcEmisor;

					//Folio Fiscal - UUID
					string folioFiscal = digitalTi.UUID.Trim();

					//Obtenemos el xml por medio del conector y guardamos resultado
					ResultadoCancelacion resultadoCancelacion = conector.CancelaCFDI(rfcEmisor, folioFiscal);

					//Verificamos el resultado
					if (resultadoCancelacion.Exitoso)
					{
						//El comprobante fue cancelado exitosamente
						//MessageBox.Show("Cancelación exitosa " + resultadoCancelacion.Descripcion);
						communicationLog.CommunicationStatusTypeCode = "D";
						communicationLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(tenant);
						communicationLog.DoneDateUTC = DateTime.UtcNow;
						communicationLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant);
						communicationLog.LastStatusDateUTC = DateTime.UtcNow;
						communicationLogRep.Update(communicationLog);
						communicationLogRep.SubmitChanges();

						if (isConcurrencyToggleEnabled)
						{
							invoice.IsUpdatedBySAT = true;
							invoice.SATTransferStatusCode = "TD";
							invoice.TransmissionError = null;
							SaveARInvoice_UpdateService(invoice);
						}

						else
                        {
							
							if(invoicePOCO != null)
                            {
								invoicePOCO.SATTransferStatusCode = "TD";
								invoicePOCO.TransmissionError = null;
								SaveARInvoice_Repository(invoicePOCO);
							}
                        }
					}
					else
					{
						//No se pudo cancelar, mostramos respuesta
						//MessageBox.Show(resultadoCancelacion.Descripcion);
						string transError = resultadoCancelacion.Descripcion;
						if ((transError == "Comprobante ya está en proceso de cancelación" && resultadoCancelacion.TipoExcepcion == "EstatusSat") || transError == "El comprobante será cancelado")

						{
							communicationLog.CommunicationStatusTypeCode = "D";
							communicationLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(tenant);
							communicationLog.DoneDateUTC = DateTime.UtcNow;
							communicationLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant);
							communicationLog.LastStatusDateUTC = DateTime.UtcNow;
							communicationLogRep.Update(communicationLog);
							communicationLogRep.SubmitChanges();

							if (isConcurrencyToggleEnabled)
							{
								invoice.IsUpdatedBySAT = true;
								invoice.SATTransferStatusCode = "CS";
								SaveARInvoice_UpdateService(invoice);
							}

							else
							{
								invoicePOCO.SATTransferStatusCode = "CS";
								SaveARInvoice_Repository(invoicePOCO);
							}
						}
						else
						{
							if (communicationLog.Retries == 4)
							{
								if (!string.IsNullOrEmpty(resultadoCancelacion.Descripcion) && invoice != null)
								{
									transError = resultadoCancelacion.Descripcion.Replace("Error en la validación de estructura xsd:", "").ToString().Trim();
									if (!string.IsNullOrEmpty(resultadoCancelacion.TipoExcepcion))
									{
										transError += Environment.NewLine + resultadoCancelacion.TipoExcepcion;
									}
									if (transError != invoice.TransmissionError || invoice.SATTransferStatusCode != "TE")
									{
										if (isConcurrencyToggleEnabled)
										{
											invoice.IsUpdatedBySAT = true;
											invoice.SATTransferStatusCode = "TE";
											invoice.TransmissionError = transError;
											SaveARInvoice_UpdateService(invoice);
										}

										else
										{
											invoicePOCO.SATTransferStatusCode = "CS";
											SaveARInvoice_Repository(invoicePOCO);
										}
									}
								}
							}

							throw new Exception("Failed," + transError);
						}
					}
				}
			}
		}

		public static ComprobanteDetails GetcomprobanteDetails(string sATXML)
		{
			try
			{
				Profact.TimbraCFDI33.Comprobante comprobante = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI33.Comprobante>(sATXML);
				return new ComprobanteDetails
				{
					ComplementoAny = comprobante.Complemento.Any,
					RfcEmisor = comprobante.Emisor.Rfc.Trim()
				};
			}
			catch (Exception ex)
			{
				Profact.TimbraCFDI40.Comprobante comprobante = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI40.Comprobante>(sATXML);
				return new ComprobanteDetails
				{
					ComplementoAny = comprobante.Complemento.Any,
					RfcEmisor = comprobante.Emisor.Rfc.Trim()
				};
			}
		}

		#endregion

		#region SendPaymentProfactCancellationRequest
		private void SendPaymentProfactCancellationRequest()
		{
			ARPayment paymentPOCO = arPaymentRepository.GetSingleARPayment(communicationLog.EntityId);
			ARPaymentPM payment = arPaymentQuery.GetSinglePM(communicationLog.EntityId, tenant);
			if (!string.IsNullOrEmpty(payment.SATXML))
			{
				SATInterfaceSettingRepository sATInterfaceSettingRepository = new SATInterfaceSettingRepository(tenant);
				SATInterfaceSetting satSetting = sATInterfaceSettingRepository.GetSingleSATInterfaceSetting(tenant);
				if (satSetting != null)
				{
					if (satSetting.SATInterfaceCode == "PROF40")
						SATPaymentProfact40CancellationService.SendRequest(new SATPaymentProfact40CancellationServiceArgs { WaitingCommLog = communicationLog, CommunicationLogRep = communicationLogRep, ARPaymentRep = arPaymentRepository, ARPayment = paymentPOCO, ARPaymentPM =  payment,  ARInvoiceRep = arInvoiceRepository, SatSetting = satSetting });
					else
						SendPaymentProfactCancellation33(payment, satSetting);
				}
			}
		}


		private void SendPaymentProfactCancellation33(ARPaymentPM payment, SATInterfaceSetting satSetting)
		{
			SATInterfaceHelper sATInterfaceHelper = new SATInterfaceHelper();


			bool isProduction = satSetting.Token != "mvpNUXmQfK8=";
			Profact.TimbraCFDI33.Conector conector = new Profact.TimbraCFDI33.Conector(isProduction);
			//Establecemos las credenciales para el permiso de conexión
			conector.EstableceCredenciales(satSetting.Token);

			ComprobanteDetails comprobanteDetails = GetcomprobanteDetails(payment.SATXML);
			ARPayment paymentPOCO = arPaymentRepository.GetSingleARPayment(payment.Id, tenant);
			if (comprobanteDetails.ComplementoAny != null && paymentPOCO != null)
			{
				List<System.Xml.XmlElement> myLXmlComplementos = comprobanteDetails.ComplementoAny.ToList<System.Xml.XmlElement>();
				var timbreFiscalDigitalElement = myLXmlComplementos.Where(el => el.Name == "tfd:TimbreFiscalDigital").FirstOrDefault();
				if (timbreFiscalDigitalElement != null)
				{
					Profact.TimbraCFDI.TimbreFiscalDigital digitalTi = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(timbreFiscalDigitalElement.OuterXml);


					//Rfc Emisor
					string rfcEmisor = comprobanteDetails.RfcEmisor;

					//Folio Fiscal - UUID
					string folioFiscal = digitalTi.UUID.Trim();

					//Obtenemos el xml por medio del conector y guardamos resultado
					ResultadoCancelacion resultadoCancelacion = conector.CancelaCFDI(rfcEmisor, folioFiscal);

					//Verificamos el resultado
					if (resultadoCancelacion.Exitoso)
					{
						//El comprobante fue cancelado exitosamente
						//MessageBox.Show("Cancelación exitosa " + resultadoCancelacion.Descripcion);
						communicationLog.CommunicationStatusTypeCode = "D";
						communicationLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(tenant);
						communicationLog.DoneDateUTC = DateTime.UtcNow;
						communicationLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant);
						communicationLog.LastStatusDateUTC = DateTime.UtcNow;
						communicationLogRep.Update(communicationLog);
						communicationLogRep.SubmitChanges();

						if (isConcurrencyToggleEnabled)
						{
							payment.IsUpdatedBySAT = true;
							payment.SATXML = null;
							payment.SATTransferStatusCode = "TD";
							payment.TransmissionError = null;
							this.SaveARPayment_UpdateService(payment);
						}

						else
                        {
							paymentPOCO.SATXML = null;
							paymentPOCO.SATTransferStatusCode = "TD";
							paymentPOCO.TransmissionError = null;
							this.SaveARPayment_Repository(paymentPOCO);
						}

						sATInterfaceHelper.UpdatePaymentInvoicesSATStatus(payment.Id, tenant, comprobanteDetails.ComplementoAny[0], arInvoiceRepository, arPaymentRepository);

					}
					else
					{
						string transError = resultadoCancelacion.Descripcion;
						if ((transError == "Comprobante ya está en proceso de cancelación" && resultadoCancelacion.TipoExcepcion == "EstatusSat") || transError == "El comprobante será cancelado")
						{
							communicationLog.CommunicationStatusTypeCode = "D";
							communicationLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(tenant);
							communicationLog.DoneDateUTC = DateTime.UtcNow;
							communicationLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(tenant);
							communicationLog.LastStatusDateUTC = DateTime.UtcNow;
							communicationLogRep.Update(communicationLog);
							communicationLogRep.SubmitChanges();

							if (isConcurrencyToggleEnabled)
							{
								payment.IsUpdatedBySAT = true;
								payment.SATTransferStatusCode = "CS";
								this.SaveARPayment_UpdateService(payment);
							}

							else
                            {
								paymentPOCO.SATTransferStatusCode = "CS";
								this.SaveARPayment_Repository(paymentPOCO);
							}
						}
						else
						{
							if (communicationLog.Retries == 4)
							{
								//No se pudo cancelar, mostramos respuesta
								//MessageBox.Show(resultadoCancelacion.Descripcion);
								if (!string.IsNullOrEmpty(resultadoCancelacion.Descripcion) && payment != null)
								{
									transError = resultadoCancelacion.Descripcion.Replace("Error en la validación de estructura xsd:", "").ToString().Trim();
									if (!string.IsNullOrEmpty(resultadoCancelacion.TipoExcepcion))
									{
										transError += Environment.NewLine + resultadoCancelacion.TipoExcepcion;
									}
									if (transError != payment.TransmissionError || payment.SATTransferStatusCode != "TE")
									{
										if (isConcurrencyToggleEnabled)
										{
											payment.SATTransferStatusCode = "TE";
											payment.TransmissionError = transError;
											payment.IsUpdatedBySAT = true;
											this.SaveARPayment_UpdateService(payment);
										}

										else
										{
											paymentPOCO.SATTransferStatusCode = "TE";
											paymentPOCO.TransmissionError = transError;
											this.SaveARPayment_Repository(paymentPOCO);
										}
									}
								}
							}

							throw new Exception("Failed," + transError);
						}
					}
				}
			}
		}

		#endregion


		#region CreateSATPaymentDocument
		private void CreateSATPaymentDocument(ARPaymentPM payment, byte[] fileData, bool checkIfExists)
		{

			int tenant = payment.Tenant;
			ICommonDataContext objectContext = CommonDataContext.GetContext(payment.Tenant);
			DocumentTypeRepository documentTypeRep = new DocumentTypeRepository(objectContext);
			DocumentRepository documentRepostory = new DocumentRepository(objectContext);
			Logitude.BL.CommonDataModel.Tools.EntityService.DocumentsFilingService documentsService = new Logitude.BL.CommonDataModel.Tools.EntityService.DocumentsFilingService(objectContext, payment.Tenant);

			Simplog.Data.InfrastructureModel.Repositories.ObjectTableRepository tableRepository = new Simplog.Data.InfrastructureModel.Repositories.ObjectTableRepository(payment.Tenant);
			Simplog.Data.InfrastructureModel.EntityPOCOs.ObjectTable arObjectTable = tableRepository.GetObjectTableByName("ARPayment", 0, false);
			Simplog.Data.InfrastructureModel.EntityPOCOs.ObjectTable shipmentObjectTable = tableRepository.GetObjectTableByName("Shipment", 0, false);
			Simplog.Data.InfrastructureModel.EntityPOCOs.ObjectTable masterObjectTable = tableRepository.GetObjectTableByName("Master", 0, false);
			DocumentsFilingQuery documentsFilingQuery = new DocumentsFilingQuery(payment.Tenant);

			ObjectTable entityObjectTable = shipmentObjectTable;
			ContactRepository contactRepository = new ContactRepository(tenant);
			Contact systemUser = contactRepository.GetSingleContactByEmail("system@tenant" + tenant + ".com", tenant, true);


			DocumentType docType = documentTypeRep.GetDocumentTypeByCode("PSATXML", payment.Tenant);
			if (docType == null)
			{
				throw new Exception("PSATXML document type couldn't be found!");
			}

			bool exists = false;
			if (checkIfExists)
			{
				DocumentsFilingPM documentFiling = documentsFilingQuery.GetDocumentsFilingPMsByEntityIdAndObjectTable(payment.Id, null, arObjectTable.Id, "I", tenant).Where(f => f.DocumentTypeId == docType.Id).FirstOrDefault();
				if (documentFiling != null && documentFiling.HasFile && !string.IsNullOrEmpty(documentFiling.DocumentId))
				{
					documentFiling.FileSize = fileData.Length;
					documentsService.Update(documentFiling, fileData);
					exists = true;
				}
			}

			if (exists)
				return;



			Logitude.BL.CommonDataModel.EntityPMs.DocumentsFilingPM extDocPM = new Logitude.BL.CommonDataModel.EntityPMs.DocumentsFilingPM()
			{
				Id = Logitude.Server.Tools.Counters.IdCounter.GetNumber("DocumentsFiling", tenant).ToString(),
				DirectionCode = "I",
				Tenant = payment.Tenant,
				DocumentTypeId = docType.Id,
				CreatedByUserId = systemUser.Id,
				CreateDate = DateTime.Now,
				OwnerId = systemUser.Id,
				UpdatedByUserId = systemUser.Id,
				ExternalEntityName = "ARPayment",
				EntityReference = payment.PaymentNo,
				FileExtension = "xml",
				FileSize = fileData.Length,
				Folder = "docsin",
				HasFile = true,
				FileName = "SAT XML",
				Received = true,
				ReceivedDate = DateTime.Now,
				ReceivedByUserId = systemUser.Id,

				Description = "SAT XML",
				IsFromUnifreightPodMobile = true,

				//EntityId = invoice.MainEntityId,
				//ObjectTableId = entityObjectTable.Id,
				//EntityNumber = invoice.MainEntityReference,

				EntityId = payment.Id,
				ObjectTableId = arObjectTable.Id,
				EntityNumber = payment.PaymentNo,

			};

			documentsService.Create(extDocPM, fileData, systemUser.Id, false);

		}

		#endregion


		public override bool OnStart()
		{

			ConnectClient(); // mohammad to try reconnect in case of disconnected client. 23-7-15
							 // Set the maximum number of concurrent connections 
			ServicePointManager.DefaultConnectionLimit = 12;
			//ThreadId = Thread.CurrentThread.ManagedThreadId.ToString();


			ThreadId = Guid.NewGuid().ToString();
			BatchServiceCode = "SATInterface";
			DoneItemsInRange = new Dictionary<DateTime, int>();

			//DiagnosticMonitor.Start("DiagnosticsConnectionString");

			// For information on handling configuration changes
			// see the MSDN topic at http://go.microsoft.com/fwlink/?LinkId=166357.
			RoleEnvironment.Changing += RoleEnvironmentChanging;

			return base.OnStart();
		}
		IQueueService queueservice;
		public void ConnectClient()
		{
			try
			{


				queueservice = new DbQueueService();
				queueservice.InitializeQueue("SATInterface", 0);

			}
			catch (Exception ex)
			{
				ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "Connect client method", null, null);
			}
		}

		private void RoleEnvironmentChanging(object sender, RoleEnvironmentChangingEventArgs e)
		{

			// If a configuration setting is changing
			if (e.Changes.Any(change => change is RoleEnvironmentConfigurationSettingChange))
			{

				// Set e.Cancel to true to restart this role instance
				e.Cancel = true;
			}
		}

		private void SaveARInvoice_UpdateService(ARInvoicePM invoice)
		{
			ARInvoiceService invoiceService = new ARInvoiceService(invoiceContext, invoice.Tenant);
			invoiceService.Update(invoice);
		}
		private void SaveARPayment_UpdateService(ARPaymentPM payment)
		{
			ARPaymentService paymentService = new ARPaymentService(invoiceContext, payment.Tenant);
			paymentService.Update(payment);
		}

		private void SaveARInvoice_Repository(ARInvoice invoice)
		{
			arInvoiceRepository.Update(invoice);
			arInvoiceRepository.SubmitChanges();
		}
		private void SaveARPayment_Repository(ARPayment payment)
		{
			arPaymentRepository.Update(payment);
			arPaymentRepository.SubmitChanges();
		}
	}

	public class ComprobanteDetails
	{
		public XmlElement[] ComplementoAny { get; set; }
		public string RfcEmisor { get; set; }
	}

}
