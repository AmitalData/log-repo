using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Microsoft.WindowsAzure.ServiceRuntime;
using Profact.TimbraCFDI;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Profact.TimbraCFDI33;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using System.Xml;
using Simplog.Data.InvoiceModel;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.Server.Tools.Helpers;
using Logitude.BL.InvoiceModel.Tools;

namespace CommunicationWorkerRole
{
	public class SATInterfaceWorkerRole : WorkerEntryPoint
	{

		List<CommunicationLog> waitingcommlogs;

		public List<CommunicationLog> WaitingCommLogs
		{
			get
			{
				if (waitingcommlogs == null)
				{
					waitingcommlogs = new List<CommunicationLog>();
				}
				return waitingcommlogs;
			}
			set { waitingcommlogs = value; }
		}
		public override void Run()
		{


			while (IsRunning)
			{

				if (!General.IsUpdating())
				{
					try
					{
						int tenant = 0;
						queueservice = new DbQueueService();
						queueservice.InitializeQueue("SATInterface", 0);

						var response = queueservice.Receive(new TimeSpan(0, 0, 0, 10));
						LastActivity = DateTime.UtcNow;

						if (response.MessageId != null)
						{

							string communicationLogId = response.MessageValues["CommunicationLogId"].ToString();
							int.TryParse(response.MessageValues["Tenant"].ToString(), out tenant);
							context = CommonDataContext.GetContext(tenant);
							CommunicationLogRepository communicationLogRep = new CommunicationLogRepository(context);
							CommunicationLog cl = communicationLogRep.GetSingleCommunicationLog(communicationLogId, tenant);

							bool processEnebled = true;

							if (processEnebled)
							{
								if (cl != null)
								{
									if (cl.CommunicationStatusTypeCode == "D")
									{
										queueservice.Complete();
									}
									else
									{
										SendCommunicationLog(communicationLogId, tenant, cl, communicationLogRep);
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

		public bool IsCommunicationLogProcessEnabled(string communicationLogId, int tenant)
		{
			bool isEnabled = true;
			context = CommonDataContext.GetContext(tenant);
			CommunicationLogRepository communicationLogRep = new CommunicationLogRepository(context);
			CommunicationLog cl = communicationLogRep.GetSingleCommunicationLog(communicationLogId, tenant);


			DateTime requestDate = DateTime.UtcNow;
			if (requestDate < cl.NextTryDateTimeUTC)
			{
				isEnabled = false;

			}

			return isEnabled;
		}
		private void SetNextTryDateTime(CommunicationLog cl)
		{
			string newLog = null;
			DateTime date = TenantServerConfigration.GetCurrentDateTime(cl.Tenant);
			DateTime dateUtc = DateTime.UtcNow;
			switch (cl.Retries)
			{
				case 1:
				case 2:
					{
						cl.NextTryDateTime = date.AddSeconds(1);
						cl.NextTryDateTimeUTC = dateUtc.AddSeconds(1);
						queueservice.Delay(new TimeSpan(0, 0, 0, 1));

						break;
					}
				case 3:
				case 4:
					{
						cl.NextTryDateTime = date.AddSeconds(5);
						cl.NextTryDateTimeUTC = dateUtc.AddSeconds(5);
						queueservice.Delay(new TimeSpan(0, 0, 0, 5));
						break;
					}
				case 5:
					{
						cl.NextTryDateTime = date.AddMinutes(1);
						cl.NextTryDateTimeUTC = dateUtc.AddMinutes(1);
						queueservice.Delay(new TimeSpan(0, 0, 1));
						break;
					}
				case 6:
				case 7:
				case 8:
					{
						cl.NextTryDateTime = date.AddMinutes(2);
						cl.NextTryDateTimeUTC = dateUtc.AddMinutes(2);
						queueservice.Delay(new TimeSpan(0, 0, 2));
						break;
					}
				case 9:
					{
						cl.NextTryDateTime = date.AddMinutes(5);
						cl.NextTryDateTimeUTC = dateUtc.AddMinutes(5);
						queueservice.Delay(new TimeSpan(0, 0, 5));
						break;
					}
				case 10:
					{
						cl.NextTryDateTime = date.AddMinutes(10);
						cl.NextTryDateTimeUTC = dateUtc.AddMinutes(10);
						queueservice.Delay(new TimeSpan(0, 0, 10));
						break;
					}
				default:
					{
						cl.NextTryDateTime = date.AddMinutes(20);
						cl.NextTryDateTimeUTC = dateUtc.AddMinutes(20);
						queueservice.Delay(new TimeSpan(0, 0, 20));
						break;
					}
			}
			cl.Logs += Environment.NewLine + "Retry #" + cl.Retries + " Next Retry: " + cl.NextTryDateTimeUTC.ToString();
		}

		#region SendCommunicationLog

		ICommonDataContext context;
		private void SendCommunicationLog(string communicationLogId, int tenant, CommunicationLog cl, CommunicationLogRepository communicationLogRep)
		{


			try
			{
				if (cl.Retries < 5)
				{
					SendWaitingCommunicationLog(cl, communicationLogRep);
				}

				else
				{
					if (context != null)
					{
						SetRelatedEntityTransferStatusToTransferError(cl);

						cl.CommunicationStatusTypeCode = "F";
						cl.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(cl.Tenant);
						communicationLogRep.Update(cl);
						communicationLogRep.SubmitChanges();
						queueservice.Complete();

					}

				}
			}

			catch (Exception exc)
			{
				ExceptionHandler.HandleException(exc, DateTime.Now, tenant, "", "WorkerRole", "", null);

				//Change number of retries

				cl.Retries++;
				cl.ExceptionMessage = exc.Message;
				if (exc.InnerException != null)
				{
					cl.ExceptionMessage = cl.ExceptionMessage + Environment.NewLine + exc.InnerException;
				}
				if (exc.StackTrace != null)
				{
					cl.ExceptionMessage = cl.ExceptionMessage + Environment.NewLine + "Stack trace: " + exc.StackTrace;
				}

				cl.ExceptionMessage = StringHelper.TruncateLongString(cl.ExceptionMessage, 7000);
				SetNextTryDateTime(cl);
				if (context != null)
				{
					communicationLogRep.Update(cl);
					communicationLogRep.SubmitChanges();
				}
				throw;

			}
		}

		private static void SetRelatedEntityTransferStatusToTransferError(CommunicationLog waitingCommLog)
		{
			if (waitingCommLog.Subject == "Payment SAT Interface")
			{
				Simplog.Data.InvoiceModel.Repositories.ARPaymentRepository arpaymentRep = new Simplog.Data.InvoiceModel.Repositories.ARPaymentRepository(waitingCommLog.Tenant);
				Simplog.Data.InvoiceModel.EntityPOCOs.ARPayment payment = arpaymentRep.GetSingleARPayment(waitingCommLog.EntityId);
				if (payment != null)
				{
					if (payment.SATTransferStatusCode != "TE")
					{
						payment.SATTransferStatusCode = "TE";
						//payment.TransmissionError = transError;
						arpaymentRep.Update(payment);
						arpaymentRep.SubmitChanges();
					}
				}
			}
			else
			{
				Simplog.Data.InvoiceModel.Repositories.ARInvoiceRepository arinvoiceRep = new Simplog.Data.InvoiceModel.Repositories.ARInvoiceRepository(waitingCommLog.Tenant);
				Simplog.Data.InvoiceModel.EntityPOCOs.ARInvoice invoice = arinvoiceRep.GetSingleInvoice(waitingCommLog.EntityId);
				if (invoice.SATTransferStatusCode != "TE")
				{
					invoice.SATTransferStatusCode = "TE";
					//invoice.TransmissionError = transError;
					arinvoiceRep.Update(invoice);
					arinvoiceRep.SubmitChanges();
				}
			}
		}

		private void SendWaitingCommunicationLog(CommunicationLog waitingCommLog, CommunicationLogRepository communicationLogRep)
		{
			switch (waitingCommLog.Subject)
			{
				case "SAT Interface Cancellation Request":
					SendProfactCancellationRequest(waitingCommLog, communicationLogRep);
					break;
				case "Payment SAT Interface Cancellation":
					SendPaymentProfactCancellationRequest(waitingCommLog, communicationLogRep);
					break;
				default:
					SendProfactRequest(waitingCommLog, communicationLogRep);
					break;

			}

		}

		#endregion

		#region SendProfactRequest

		private void SendProfactRequest(CommunicationLog waitingCommLog, CommunicationLogRepository communicationLogRep)
		{
			if (waitingCommLog.Document != null)
			{
				string filename = waitingCommLog.DocumentId + "." + waitingCommLog.Document.Extension;


				Logitude.Server.Tools.BlobFileInfo fileInfo = new Logitude.Server.Tools.BlobFileInfo()
				{
					FileName = waitingCommLog.Document.Id,
					FolderName = waitingCommLog.Document.Folder,
					Extension = waitingCommLog.Document.Extension,
					Tenant = waitingCommLog.Document.Tenant,
					FileSize = waitingCommLog.Document.FileSize,
				};
				Logitude.Server.Tools.StorageService.IBlobService storageservice = Logitude.Server.Tools.ContainerAccessor.Container.Resolve(typeof(Logitude.Server.Tools.StorageService.IBlobService), "StorageService", new Microsoft.Practices.Unity.ParameterOverride("", 1)) as Logitude.Server.Tools.StorageService.IBlobService;
				byte[] datainByte = storageservice.Read(fileInfo);


				if (datainByte != null)
				{




					Simplog.Data.InvoiceModel.Repositories.SATInterfaceSettingRepository sATInterfaceSettingRepository = new Simplog.Data.InvoiceModel.Repositories.SATInterfaceSettingRepository(waitingCommLog.Tenant);
					Simplog.Data.InvoiceModel.EntityPOCOs.SATInterfaceSetting satSetting = sATInterfaceSettingRepository.GetSingleSATInterfaceSetting(waitingCommLog.Tenant);
					if (satSetting != null)
					{

						SendProfact33Request(waitingCommLog, communicationLogRep, datainByte, satSetting);


					}

				}

			}
		}


		private void SendProfact33Request(CommunicationLog waitingCommLog, CommunicationLogRepository communicationLogRep, byte[] datainByte, Simplog.Data.InvoiceModel.EntityPOCOs.SATInterfaceSetting satSetting)
		{
			SATInterfaceHelper sATInterfaceHelper = new SATInterfaceHelper();

			bool isProduction = satSetting.Token != "mvpNUXmQfK8=";

			Profact.TimbraCFDI33.Conector conector = new Profact.TimbraCFDI33.Conector(isProduction);
			Profact.TimbraCFDI33.Comprobante comprobante = LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI33.Comprobante>(datainByte);

			//Establecemos las credenciales para el permiso de conexión

			conector.EstableceCredenciales(satSetting.Token);
			//Timbramos el CFDI por medio del conector y guardamos resultado
			ResultadoTimbre resultadoTimbre = conector.TimbraCFDI(comprobante);

			Simplog.Data.InvoiceModel.Repositories.ARInvoiceRepository arinvoiceRep = new Simplog.Data.InvoiceModel.Repositories.ARInvoiceRepository(waitingCommLog.Tenant);
			Simplog.Data.InvoiceModel.Repositories.ARPaymentRepository arpaymentRep = new Simplog.Data.InvoiceModel.Repositories.ARPaymentRepository(waitingCommLog.Tenant);



			//Verificamos el resultado
			if (resultadoTimbre.Exitoso)
			{
				if (waitingCommLog.Subject == "Payment SAT Interface")
				{
					Simplog.Data.InvoiceModel.EntityPOCOs.ARPayment payment = arpaymentRep.GetSingleARPayment(waitingCommLog.EntityId);
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

                        payment.SATApprovalDate = GetSATApprovalDateFromComplemento(waitingCommLog, comprobante.Complemento);
                         
						payment.SATAdditionalFieldsXML = LogitudeXmlSerializer.SerializeObjectToXmlString(additional);
						payment.SATXML = resultadoTimbre.Xml;
						payment.SATTransferStatusCode = "TD";
                        payment.TransmissionError = null;
                        arpaymentRep.Update(payment);
						arpaymentRep.SubmitChanges();
						sATInterfaceHelper.UpdatePaymentInvoicesSATStatus(payment, comprobante, arinvoiceRep, arpaymentRep);

						Encoding encoding = Encoding.UTF8;
						byte[] xmlfile = encoding.GetBytes(resultadoTimbre.Xml);
						CreateSATPaymentDocument(payment, xmlfile, true);

						EventTracer.CreateTraceEvent(new EventTracerArgs()
						{
							EntityId = waitingCommLog.EntityId,
							ObjectTableName = waitingCommLog.ObjectTable.Name,
							Tenant = waitingCommLog.Tenant,
							UserId = waitingCommLog.CreatedByUserId,
							EventTypeCode = "PAAS",
						});


					}
				}
				else
				{
					Simplog.Data.InvoiceModel.EntityPOCOs.ARInvoice invoice = arinvoiceRep.GetSingleInvoice(waitingCommLog.EntityId);

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
                        invoice.SATApprovalDate = GetSATApprovalDateFromComplemento(waitingCommLog, resultComprobante.Complemento);
                         

						invoice.SATXML = resultadoTimbre.Xml;
						invoice.SATTransferStatusCode = "TD";
                        invoice.TransmissionError = null;
                        invoice.SATInvoiceStatusCode = "OP";

						invoice.SATAdditionalFieldsXML = LogitudeXmlSerializer.SerializeObjectToXmlString(additional);

						arinvoiceRep.Update(invoice);
						arinvoiceRep.SubmitChanges();

						Encoding encoding = Encoding.UTF8;
						byte[] xmlfile = encoding.GetBytes(resultadoTimbre.Xml);

						CreateSATDocument(invoice, xmlfile);

						EventTracer.CreateTraceEvent(new EventTracerArgs()
						{
							EntityId = waitingCommLog.EntityId,
							ObjectTableName = waitingCommLog.ObjectTable.Name,
							Tenant = waitingCommLog.Tenant,
							UserId = waitingCommLog.CreatedByUserId,
							EventTypeCode = "INAS",
						});
					}
				}

				waitingCommLog.CommunicationStatusTypeCode = "D";
				waitingCommLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
				waitingCommLog.DoneDateUTC = DateTime.UtcNow;
				waitingCommLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
				waitingCommLog.LastStatusDateUTC = DateTime.UtcNow;
				communicationLogRep.Update(waitingCommLog);
				communicationLogRep.SubmitChanges();


			}
			else
			{

				if (resultadoTimbre.Descripcion != null && resultadoTimbre.Descripcion.Contains("Este CFDI ya ha sido timbrado con UUID"))
				{
					GetProfactSentXML(waitingCommLog, communicationLogRep, conector, resultadoTimbre, arinvoiceRep, arpaymentRep);
				}
				else
				{
					string transError = resultadoTimbre.Descripcion;
                    //if (waitingCommLog.Retries == 4)
                    //{

                    //}

                    if (!string.IsNullOrEmpty(resultadoTimbre.Descripcion))
                    {
                        transError = resultadoTimbre.Descripcion.Replace("Error en la validación de estructura xsd:", "").ToString().Trim();
                        if (!string.IsNullOrEmpty(resultadoTimbre.DescripcionInterna))
                        {
                            transError += Environment.NewLine + resultadoTimbre.DescripcionInterna;
                        }

                    }

                    if (waitingCommLog.Subject == "Payment SAT Interface")
                    {
                        Simplog.Data.InvoiceModel.EntityPOCOs.ARPayment payment = arpaymentRep.GetSingleARPayment(waitingCommLog.EntityId);
                        if (payment != null)
                        {
                            //if (transError != payment.TransmissionError || payment.SATTransferStatusCode != "TE")
                            //{
                            //    payment.SATTransferStatusCode = "TE";
                            //    payment.TransmissionError = transError;
                            //    arpaymentRep.Update(payment);
                            //    arpaymentRep.SubmitChanges();
                            //}

                            HandlePaymentError(waitingCommLog, arpaymentRep, communicationLogRep, transError);
                        }
                    }
                    else
                    {
                        Simplog.Data.InvoiceModel.EntityPOCOs.ARInvoice invoice = arinvoiceRep.GetSingleInvoice(waitingCommLog.EntityId);
                        //if (transError != invoice.TransmissionError || invoice.SATTransferStatusCode != "TE")
                        //{
                        //    invoice.SATTransferStatusCode = "TE";
                        //    invoice.TransmissionError = transError;

                        //    arinvoiceRep.Update(invoice);
                        //    arinvoiceRep.SubmitChanges();
                        //}

                        HandleInvoiceError(waitingCommLog, arinvoiceRep, communicationLogRep, transError, invoice);
                    }


                    //throw new Exception("Failed," + transError);
				}

			}
		}



		private void GetProfactSentXML(CommunicationLog waitingCommLog, CommunicationLogRepository communicationLogRep, Profact.TimbraCFDI33.Conector conector, ResultadoTimbre resultadoTimbre, Simplog.Data.InvoiceModel.Repositories.ARInvoiceRepository arinvoiceRep, Simplog.Data.InvoiceModel.Repositories.ARPaymentRepository arpaymentRep)
		{
			SATInterfaceHelper sATInterfaceHelper = new SATInterfaceHelper();
			string transError = ResolveTransmisionError(resultadoTimbre);
			string[] descrip = resultadoTimbre.Descripcion.Split(':');
			ICommonDataContext commonContext = CommonDataContext.GetContext(waitingCommLog.Tenant);
			TenantRepository tenantRepository = new TenantRepository(commonContext);
			Tenant currentTenant = tenantRepository.GetSingleTenant(waitingCommLog.Tenant);

			if (waitingCommLog.Subject == "Payment SAT Interface")
			{
				if (descrip.Length > 1)
				{
                    string folioFiscal = descrip[descrip.Length - 1];
					string rfcEmisor = currentTenant.VatNumber;
					ResultadoConsulta resultadoConsulta = conector.ObtieneCFDI(rfcEmisor, folioFiscal);
					Simplog.Data.InvoiceModel.EntityPOCOs.ARPayment payment = arpaymentRep.GetSingleARPayment(waitingCommLog.EntityId);

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

                        payment.SATApprovalDate = GetSATApprovalDateFromComplemento(waitingCommLog, paymentComprobante.Complemento);
                        payment.SATAdditionalFieldsXML = LogitudeXmlSerializer.SerializeObjectToXmlString(additional);
						payment.SATXML = resultadoConsulta.Xml;
						payment.SATTransferStatusCode = "TD";
                        payment.TransmissionError = null;
                        arpaymentRep.Update(payment);
						arpaymentRep.SubmitChanges();
						sATInterfaceHelper.UpdatePaymentInvoicesSATStatus(payment, paymentComprobante, arinvoiceRep, arpaymentRep);

						Encoding encoding = Encoding.UTF8;
						byte[] xmlfile = encoding.GetBytes(resultadoConsulta.Xml);

						CreateSATPaymentDocument(payment, xmlfile, true);

						waitingCommLog.CommunicationStatusTypeCode = "D";
						waitingCommLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
						waitingCommLog.DoneDateUTC = DateTime.UtcNow;
						waitingCommLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
						waitingCommLog.LastStatusDateUTC = DateTime.UtcNow;
						communicationLogRep.Update(waitingCommLog);
						communicationLogRep.SubmitChanges();
						
						EventTracer.CreateTraceEvent(new EventTracerArgs()
						{
							EntityId = waitingCommLog.EntityId,
							ObjectTableName = waitingCommLog.ObjectTable.Name,
							Tenant = waitingCommLog.Tenant,
							UserId = waitingCommLog.CreatedByUserId,
							EventTypeCode = "PAAS",
						});
					}
					else
					{
						HandlePaymentError(waitingCommLog, arpaymentRep, communicationLogRep, transError);
					}
				}
				else
				{
					HandlePaymentError(waitingCommLog, arpaymentRep, communicationLogRep, transError);
				}
			}
			else
			{
                //Este CFDI ya ha sido timbrado con UUID: { 0}
                //Este CFDI ya ha sido timbrado con UUID: 09acd5a4 - c544 - 45af - b897 - f64b13f60606

                Simplog.Data.InvoiceModel.EntityPOCOs.ARInvoice invoice = arinvoiceRep.GetSingleInvoice(waitingCommLog.EntityId);
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

                        invoice.SATApprovalDate  = GetSATApprovalDateFromComplemento(waitingCommLog, invoiceComprobante.Complemento);

                      

						string SATAdditionalFieldsXML = LogitudeXmlSerializer.SerializeObjectToXmlString<SATAdditionalFields>(additional);

						invoice.SATXML = resultadoConsulta.Xml;
						invoice.SATAdditionalFieldsXML = LogitudeXmlSerializer.SerializeObjectToXmlString<SATAdditionalFields>(additional);
						invoice.SATTransferStatusCode = "TD";
                        invoice.TransmissionError = null;
                        arinvoiceRep.Update(invoice);
						arinvoiceRep.SubmitChanges();

						Encoding encoding = Encoding.UTF8;
						byte[] xmlfile = encoding.GetBytes(resultadoConsulta.Xml);

						CreateSATDocument(invoice, xmlfile, true);


						waitingCommLog.CommunicationStatusTypeCode = "D";
						waitingCommLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
						waitingCommLog.DoneDateUTC = DateTime.UtcNow;
						waitingCommLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
						waitingCommLog.LastStatusDateUTC = DateTime.UtcNow;
						communicationLogRep.Update(waitingCommLog);
						communicationLogRep.SubmitChanges();


						EventTracer.CreateTraceEvent(new EventTracerArgs()
						{
							EntityId = waitingCommLog.EntityId,
							ObjectTableName = waitingCommLog.ObjectTable.Name,
							Tenant = waitingCommLog.Tenant,
							UserId = waitingCommLog.CreatedByUserId,
							EventTypeCode = "INAS",
						});

					}
					else
					{
						HandleInvoiceError(waitingCommLog, arinvoiceRep, communicationLogRep, transError, invoice);

					}
				}
				else
				{
					HandleInvoiceError(waitingCommLog, arinvoiceRep, communicationLogRep, transError, invoice);
				}

			}
		}

        private DateTime GetSATApprovalDateFromComplemento(CommunicationLog waitingCommLog, Profact.TimbraCFDI33.ComprobanteComplemento complemento)
        {
            if (complemento.Any != null)
            {
                List<System.Xml.XmlElement> myLXmlComplementos = complemento.Any.ToList<System.Xml.XmlElement>();
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
                       return TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
                    }
                }
                else
                {
                    return TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
                }
            }
            else
            {
                return TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
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

		private void HandlePaymentError(CommunicationLog waitingCommLog, Simplog.Data.InvoiceModel.Repositories.ARPaymentRepository arpaymentRep, CommunicationLogRepository communicationLogRep, string transError)
        {
            //if (waitingCommLog.Retries == 4)
            //{
                Simplog.Data.InvoiceModel.EntityPOCOs.ARPayment payment = arpaymentRep.GetSingleARPayment(waitingCommLog.EntityId);
                if (payment != null)
                {
                    if (transError != payment.TransmissionError || payment.SATTransferStatusCode != "TE")
                    {
                        payment.SATTransferStatusCode = "TE";
                        payment.TransmissionError = transError;
                        arpaymentRep.Update(payment);
                        arpaymentRep.SubmitChanges();
                    }
                }
			// }

			SaveCommunicationLogAsDoneWithSATError(waitingCommLog, communicationLogRep, transError);

        }

        private void HandleInvoiceError(CommunicationLog waitingCommLog, Simplog.Data.InvoiceModel.Repositories.ARInvoiceRepository arinvoiceRep, CommunicationLogRepository communicationLogRep, string transError, Simplog.Data.InvoiceModel.EntityPOCOs.ARInvoice invoice)
		{
			//if (waitingCommLog.Retries == 4)
			//{
				if (transError != invoice.TransmissionError || invoice.SATTransferStatusCode != "TE")
				{
					invoice.SATTransferStatusCode = "TE";
					invoice.TransmissionError = transError;

					arinvoiceRep.Update(invoice);
					arinvoiceRep.SubmitChanges();
				}
			//}

			SaveCommunicationLogAsDoneWithSATError(waitingCommLog, communicationLogRep, transError);
        }

		private void SaveCommunicationLogAsDoneWithSATError(CommunicationLog waitingCommLog, CommunicationLogRepository communicationLogRep, string transError)
		{
			string exceptionMessage = "Done, with SAT Error: " + transError;
			waitingCommLog.CommunicationStatusTypeCode = "D";
			waitingCommLog.ExceptionMessage = StringHelper.TruncateLongString(exceptionMessage, 7000);
			waitingCommLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);

			if (context != null)
			{
				communicationLogRep.Update(waitingCommLog);
				communicationLogRep.SubmitChanges();
			}

			queueservice.Complete();
		}
		private void SaveCommunicationLogAsFailed(CommunicationLog waitingCommLog, CommunicationLogRepository communicationLogRep, string transError)
        {
            string exceptionMessage = "Failed," + transError;
            waitingCommLog.CommunicationStatusTypeCode = "F";
            waitingCommLog.ExceptionMessage = StringHelper.TruncateLongString(exceptionMessage, 7000);
            waitingCommLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);

            if (context != null)
            {
                communicationLogRep.Update(waitingCommLog);
                communicationLogRep.SubmitChanges();
            }

            queueservice.Complete();
        }
        #endregion

        #region CreateSATDocument
        private void CreateSATDocument(Simplog.Data.InvoiceModel.EntityPOCOs.ARInvoice invoice, byte[] fileData, bool checkIfExists = false)
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


			ARInvoiceQuery arInvoiceQuery = new ARInvoiceQuery(tenant);
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

				//EntityId = invoice.Id,
				//ObjectTableId = arObjectTable.Id,
				//EntityNumber = invoice.InvoiceNumber,
				EntityId = string.IsNullOrEmpty(invoice.MainEntityId) ? invoice.Id : invoice.MainEntityId,
				//ObjectTableId = entityObjectTable.Id,
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
		private void SendProfactCancellationRequest(CommunicationLog waitingCommLog, CommunicationLogRepository communicationLogRep)
		{

			Simplog.Data.InvoiceModel.Repositories.ARInvoiceRepository arinvoiceRep = new Simplog.Data.InvoiceModel.Repositories.ARInvoiceRepository(waitingCommLog.Tenant);
			Simplog.Data.InvoiceModel.EntityPOCOs.ARInvoice invoice = arinvoiceRep.GetSingleInvoice(waitingCommLog.EntityId);
			if (!string.IsNullOrEmpty(invoice.SATXML))
			{
				Simplog.Data.InvoiceModel.Repositories.SATInterfaceSettingRepository sATInterfaceSettingRepository = new Simplog.Data.InvoiceModel.Repositories.SATInterfaceSettingRepository(waitingCommLog.Tenant);
				Simplog.Data.InvoiceModel.EntityPOCOs.SATInterfaceSetting satSetting = sATInterfaceSettingRepository.GetSingleSATInterfaceSetting(waitingCommLog.Tenant);
				if (satSetting != null)
				{

					SendProfactCancellation33(waitingCommLog, communicationLogRep, arinvoiceRep, invoice, satSetting);

				}
			}
		}


		private void SendProfactCancellation33(CommunicationLog waitingCommLog, CommunicationLogRepository communicationLogRep, Simplog.Data.InvoiceModel.Repositories.ARInvoiceRepository arinvoiceRep, Simplog.Data.InvoiceModel.EntityPOCOs.ARInvoice invoice, Simplog.Data.InvoiceModel.EntityPOCOs.SATInterfaceSetting satSetting)
		{
			Profact.TimbraCFDI33.Comprobante comprobante = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI33.Comprobante>(invoice.SATXML);
			bool isProduction = satSetting.Token != "mvpNUXmQfK8=";
			Profact.TimbraCFDI33.Conector conector = new Profact.TimbraCFDI33.Conector(isProduction);
			//Establecemos las credenciales para el permiso de conexión
			conector.EstableceCredenciales(satSetting.Token);

			if (comprobante.Complemento.Any != null)
			{
				List<System.Xml.XmlElement> myLXmlComplementos = comprobante.Complemento.Any.ToList<System.Xml.XmlElement>();
				var timbreFiscalDigitalElement = myLXmlComplementos.Where(el => el.Name == "tfd:TimbreFiscalDigital").FirstOrDefault();
				if (timbreFiscalDigitalElement != null)
				{
					Profact.TimbraCFDI.TimbreFiscalDigital digitalTi = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(timbreFiscalDigitalElement.OuterXml);


					//Rfc Emisor
					string rfcEmisor = comprobante.Emisor.Rfc.Trim();

					//Folio Fiscal - UUID
					string folioFiscal = digitalTi.UUID.Trim();

					//Obtenemos el xml por medio del conector y guardamos resultado
					ResultadoCancelacion resultadoCancelacion = conector.CancelaCFDI(rfcEmisor, folioFiscal);

					//Verificamos el resultado
					if (resultadoCancelacion.Exitoso)
					{
						//El comprobante fue cancelado exitosamente
						//MessageBox.Show("Cancelación exitosa " + resultadoCancelacion.Descripcion);
						waitingCommLog.CommunicationStatusTypeCode = "D";
						waitingCommLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
						waitingCommLog.DoneDateUTC = DateTime.UtcNow;
						waitingCommLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
						waitingCommLog.LastStatusDateUTC = DateTime.UtcNow;
						communicationLogRep.Update(waitingCommLog);
						communicationLogRep.SubmitChanges();

						invoice.SATTransferStatusCode = "TD";
                        invoice.TransmissionError = null;
                        arinvoiceRep.Update(invoice);
						arinvoiceRep.SubmitChanges();
					}
					else
					{
						//No se pudo cancelar, mostramos respuesta
						//MessageBox.Show(resultadoCancelacion.Descripcion);
						string transError = resultadoCancelacion.Descripcion;
						if ((transError == "Comprobante ya está en proceso de cancelación" && resultadoCancelacion.TipoExcepcion == "EstatusSat") || transError == "El comprobante será cancelado")

						{
							waitingCommLog.CommunicationStatusTypeCode = "D";
							waitingCommLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
							waitingCommLog.DoneDateUTC = DateTime.UtcNow;
							waitingCommLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
							waitingCommLog.LastStatusDateUTC = DateTime.UtcNow;
							communicationLogRep.Update(waitingCommLog);
							communicationLogRep.SubmitChanges();

							invoice.SATTransferStatusCode = "CS";
							arinvoiceRep.Update(invoice);
							arinvoiceRep.SubmitChanges();
						}
						else
						{
							if (waitingCommLog.Retries == 4)
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
										invoice.SATTransferStatusCode = "TE";
										invoice.TransmissionError = transError;
										arinvoiceRep.Update(invoice);
										arinvoiceRep.SubmitChanges();
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

		#region SendPaymentProfactCancellationRequest
		private void SendPaymentProfactCancellationRequest(CommunicationLog waitingCommLog, CommunicationLogRepository communicationLogRep)
		{

			Simplog.Data.InvoiceModel.Repositories.ARPaymentRepository arPaymentRep = new Simplog.Data.InvoiceModel.Repositories.ARPaymentRepository(waitingCommLog.Tenant);
			Simplog.Data.InvoiceModel.Repositories.ARInvoiceRepository arInvoiceRep = new Simplog.Data.InvoiceModel.Repositories.ARInvoiceRepository(waitingCommLog.Tenant);

			Simplog.Data.InvoiceModel.EntityPOCOs.ARPayment payment = arPaymentRep.GetSingleARPayment(waitingCommLog.EntityId);
			if (!string.IsNullOrEmpty(payment.SATXML))
			{
				Simplog.Data.InvoiceModel.Repositories.SATInterfaceSettingRepository sATInterfaceSettingRepository = new Simplog.Data.InvoiceModel.Repositories.SATInterfaceSettingRepository(waitingCommLog.Tenant);
				Simplog.Data.InvoiceModel.EntityPOCOs.SATInterfaceSetting satSetting = sATInterfaceSettingRepository.GetSingleSATInterfaceSetting(waitingCommLog.Tenant);
				if (satSetting != null)
				{
					SendPaymentProfactCancellation33(waitingCommLog, communicationLogRep, arPaymentRep, payment, satSetting, arInvoiceRep);
				}
			}
		}


		private void SendPaymentProfactCancellation33(CommunicationLog waitingCommLog, CommunicationLogRepository communicationLogRep, Simplog.Data.InvoiceModel.Repositories.ARPaymentRepository arPaymentRep, Simplog.Data.InvoiceModel.EntityPOCOs.ARPayment payment, Simplog.Data.InvoiceModel.EntityPOCOs.SATInterfaceSetting satSetting, ARInvoiceRepository arInvoiceRep)
		{
			SATInterfaceHelper sATInterfaceHelper = new SATInterfaceHelper();

			Profact.TimbraCFDI33.Comprobante comprobante = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI33.Comprobante>(payment.SATXML);
			bool isProduction = satSetting.Token != "mvpNUXmQfK8=";
			Profact.TimbraCFDI33.Conector conector = new Profact.TimbraCFDI33.Conector(isProduction);
			//Establecemos las credenciales para el permiso de conexión
			conector.EstableceCredenciales(satSetting.Token);

			if (comprobante.Complemento.Any != null)
			{
				List<System.Xml.XmlElement> myLXmlComplementos = comprobante.Complemento.Any.ToList<System.Xml.XmlElement>();
				var timbreFiscalDigitalElement = myLXmlComplementos.Where(el => el.Name == "tfd:TimbreFiscalDigital").FirstOrDefault();
				if (timbreFiscalDigitalElement != null)
				{
					Profact.TimbraCFDI.TimbreFiscalDigital digitalTi = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI.TimbreFiscalDigital>(timbreFiscalDigitalElement.OuterXml);


					//Rfc Emisor
					string rfcEmisor = comprobante.Emisor.Rfc.Trim();

					//Folio Fiscal - UUID
					string folioFiscal = digitalTi.UUID.Trim();

					//Obtenemos el xml por medio del conector y guardamos resultado
					ResultadoCancelacion resultadoCancelacion = conector.CancelaCFDI(rfcEmisor, folioFiscal);

					//Verificamos el resultado
					if (resultadoCancelacion.Exitoso)
					{
						//El comprobante fue cancelado exitosamente
						//MessageBox.Show("Cancelación exitosa " + resultadoCancelacion.Descripcion);
						waitingCommLog.CommunicationStatusTypeCode = "D";
						waitingCommLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
						waitingCommLog.DoneDateUTC = DateTime.UtcNow;
						waitingCommLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
						waitingCommLog.LastStatusDateUTC = DateTime.UtcNow;
						communicationLogRep.Update(waitingCommLog);
						communicationLogRep.SubmitChanges();

						payment.SATXML = null;
						payment.SATTransferStatusCode = "TD";
                        payment.TransmissionError = null;
                        arPaymentRep.Update(payment);
						arPaymentRep.SubmitChanges();

						sATInterfaceHelper.UpdatePaymentInvoicesSATStatus(payment, comprobante, arInvoiceRep, arPaymentRep);

					}
					else
					{
						string transError = resultadoCancelacion.Descripcion;
						if ((transError == "Comprobante ya está en proceso de cancelación" && resultadoCancelacion.TipoExcepcion == "EstatusSat") || transError == "El comprobante será cancelado")
						{
							waitingCommLog.CommunicationStatusTypeCode = "D";
							waitingCommLog.DoneDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
							waitingCommLog.DoneDateUTC = DateTime.UtcNow;
							waitingCommLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(waitingCommLog.Tenant);
							waitingCommLog.LastStatusDateUTC = DateTime.UtcNow;
							communicationLogRep.Update(waitingCommLog);
							communicationLogRep.SubmitChanges();

							payment.SATTransferStatusCode = "CS";
							arPaymentRep.Update(payment);
							arPaymentRep.SubmitChanges();
						}
						else
						{
							if (waitingCommLog.Retries == 4)
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
										payment.SATTransferStatusCode = "TE";
										payment.TransmissionError = transError;

										arPaymentRep.Update(payment);
										arPaymentRep.SubmitChanges();
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
		private void CreateSATPaymentDocument(Simplog.Data.InvoiceModel.EntityPOCOs.ARPayment payment, byte[] fileData, bool checkIfExists)
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



	}


}
