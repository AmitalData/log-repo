using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.DataContracts;
using Logitude.BL.InvoiceModel.EntityPMs;
using Logitude.BL.InvoiceModel.EntityQueries;
using Logitude.BL.InvoiceModel.Tools;
using Logitude.BL.InvoiceModel.Tools.EntityService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Profact.TimbraCFDI;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InvoiceModel;
using Simplog.Data.InvoiceModel.EntityPOCOs;
using Simplog.Data.InvoiceModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace CommunicationWorkerRole.Services.SAT
{
	public class SATProfact40Service
	{
		ICommonDataContext context;
		IQueueService queueservice;
		private bool isConcurrencyToggleEnabled;
		private IInvoiceContext invoiceContext;
		private ARInvoiceQuery invoiceQuery;
		private ARPaymentQuery paymentQuery;
		public SATProfact40Service(ICommonDataContext context, IQueueService queueservice)
        {
			this.context = context;
			this.queueservice = queueservice;
		}
		public void SendRequest(SATProfact40ServiceArgs args)
		{
			CommunicationLog waitingCommLog = args.WaitingCommLog;
			CommunicationLogRepository communicationLogRep = args.CommunicationLogRep;
			byte[] datainByte = args.DatainByte;
			Simplog.Data.InvoiceModel.EntityPOCOs.SATInterfaceSetting satSetting = args.SatSetting;

			SATInterfaceHelper sATInterfaceHelper = new SATInterfaceHelper();

			bool isProduction = satSetting.Token != "mvpNUXmQfK8=";

			Profact.TimbraCFDI40.Conector conector = new Profact.TimbraCFDI40.Conector(isProduction);
			Profact.TimbraCFDI40.Comprobante comprobante = LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI40.Comprobante>(datainByte);

			//Establecemos las credenciales para el permiso de conexión

			conector.EstableceCredenciales(satSetting.Token);
			//Timbramos el CFDI por medio del conector y guardamos resultado
			ResultadoTimbre resultadoTimbre = conector.TimbraCFDI(comprobante);

			invoiceContext = InvoiceContext.GetContext(waitingCommLog.Tenant);
			paymentQuery = new ARPaymentQuery(waitingCommLog.Tenant);
			invoiceQuery = new ARInvoiceQuery(waitingCommLog.Tenant);
			ARInvoiceRepository arinvoiceRep = new ARInvoiceRepository(invoiceContext);
			ARPaymentRepository arpaymentRep = new ARPaymentRepository(invoiceContext);
			isConcurrencyToggleEnabled = FeatureToggleHelper.HasFeatureToggle("INU", waitingCommLog.Tenant);

			//Verificamos el resultado
			if (resultadoTimbre.Exitoso)
			{
				if (waitingCommLog.Subject == "Payment SAT Interface")
				{
					ARPaymentPM payment = paymentQuery.GetSinglePM(waitingCommLog.EntityId, waitingCommLog.Tenant);
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
							payment.SATApprovalDate = GetSATApprovalDateFromComplemento(waitingCommLog, comprobante.Complemento.Any);
							payment.SATAdditionalFieldsXML = LogitudeXmlSerializer.SerializeObjectToXmlString(additional);
							payment.SATXML = resultadoTimbre.Xml;
							payment.SATTransferStatusCode = "TD";
							payment.TransmissionError = null;
							ARPaymentService paymentService = new ARPaymentService(invoiceContext, waitingCommLog.Tenant);
							paymentService.Update(payment);
						}

						else
						{ 
							ARPayment paymentPOCO = arpaymentRep.GetSingleARPayment(waitingCommLog.EntityId, waitingCommLog.Tenant);
							if (paymentPOCO != null)
							{
								paymentPOCO.SATApprovalDate = GetSATApprovalDateFromComplemento(waitingCommLog, comprobante.Complemento.Any);
								paymentPOCO.SATAdditionalFieldsXML = LogitudeXmlSerializer.SerializeObjectToXmlString(additional);
								paymentPOCO.SATXML = resultadoTimbre.Xml;
								paymentPOCO.SATTransferStatusCode = "TD";
								paymentPOCO.TransmissionError = null;
								arpaymentRep.Update(paymentPOCO);
								arpaymentRep.SubmitChanges();
							}
						}

						sATInterfaceHelper.UpdatePaymentInvoicesSATStatus(payment.Id, payment.Tenant, comprobante.Complemento.Any[0], arinvoiceRep, arpaymentRep);

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
					ARInvoicePM invoice = invoiceQuery.GetSinglePM(waitingCommLog.EntityId, waitingCommLog.Tenant);
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

						Profact.TimbraCFDI40.Comprobante resultComprobante = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI40.Comprobante>(resultadoTimbre.Xml);

						if (isConcurrencyToggleEnabled)
						{
							invoice.IsUpdatedBySAT = true;
							invoice.SATApprovalDate = GetSATApprovalDateFromComplemento(waitingCommLog, resultComprobante.Complemento.Any);
							invoice.SATXML = resultadoTimbre.Xml;
							invoice.SATTransferStatusCode = "TD";
							invoice.TransmissionError = null;
							invoice.SATInvoiceStatusCode = "OP";
							invoice.SATAdditionalFieldsXML = LogitudeXmlSerializer.SerializeObjectToXmlString(additional);
							ARInvoiceService invoiceService = new ARInvoiceService(invoiceContext, waitingCommLog.Tenant);
							invoiceService.Update(invoice);
						}

						else
						{
							ARInvoice invoicePOCO = arinvoiceRep.GetSingleARInvoice(waitingCommLog.EntityId, waitingCommLog.Tenant);
							if (invoicePOCO != null)
							{
								invoicePOCO.SATApprovalDate = GetSATApprovalDateFromComplemento(waitingCommLog, resultComprobante.Complemento.Any);
								invoicePOCO.SATXML = resultadoTimbre.Xml;
								invoicePOCO.SATTransferStatusCode = "TD";
								invoicePOCO.TransmissionError = null;
								invoicePOCO.SATInvoiceStatusCode = "OP";
								invoicePOCO.SATAdditionalFieldsXML = LogitudeXmlSerializer.SerializeObjectToXmlString(additional);
								arinvoiceRep.Update(invoicePOCO);
								arinvoiceRep.SubmitChanges();
							}
						}

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
						HandlePaymentError(new SATErrorArgs
						{
							WaitingCommLog = waitingCommLog,
							ArpaymentRep = arpaymentRep,
							CommunicationLogRep = communicationLogRep,
							TransError = transError,
							ResultadoTimbre = resultadoTimbre
						});
					}
					else
					{
						Simplog.Data.InvoiceModel.EntityPOCOs.ARInvoice invoice = arinvoiceRep.GetSingleInvoice(waitingCommLog.EntityId);
						HandleInvoiceError(new SATErrorArgs
						{
							WaitingCommLog = waitingCommLog,
							ArinvoiceRep = arinvoiceRep,
							CommunicationLogRep = communicationLogRep,
							TransError = transError,
							Invoice = invoice,
							ResultadoTimbre = resultadoTimbre 
						});
					}
				}
			}
		}

		private void GetProfactSentXML(CommunicationLog waitingCommLog, CommunicationLogRepository communicationLogRep, Profact.TimbraCFDI40.Conector conector, ResultadoTimbre resultadoTimbre, Simplog.Data.InvoiceModel.Repositories.ARInvoiceRepository arinvoiceRep, Simplog.Data.InvoiceModel.Repositories.ARPaymentRepository arpaymentRep)
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
					ARPaymentPM payment = paymentQuery.GetSinglePM(waitingCommLog.EntityId, waitingCommLog.Tenant);

					if (payment != null && resultadoConsulta.Exitoso)
					{
						Profact.TimbraCFDI40.Comprobante paymentComprobante = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI40.Comprobante>(resultadoConsulta.Xml);

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
							payment.SATApprovalDate = GetSATApprovalDateFromComplemento(waitingCommLog, paymentComprobante.Complemento.Any);
							payment.SATAdditionalFieldsXML = LogitudeXmlSerializer.SerializeObjectToXmlString(additional);
							payment.SATXML = resultadoConsulta.Xml;
							payment.SATTransferStatusCode = "TD";
							payment.TransmissionError = null;
							ARPaymentService paymentService = new ARPaymentService(invoiceContext, waitingCommLog.Tenant);
							paymentService.Update(payment);
						}

						else
						{
							ARPayment paymentPOCO = arpaymentRep.GetSingleARPayment(waitingCommLog.EntityId, waitingCommLog.Tenant);
							if (paymentPOCO != null)
							{
								paymentPOCO.SATApprovalDate = GetSATApprovalDateFromComplemento(waitingCommLog, paymentComprobante.Complemento.Any);
								paymentPOCO.SATAdditionalFieldsXML = LogitudeXmlSerializer.SerializeObjectToXmlString(additional);
								paymentPOCO.SATXML = resultadoConsulta.Xml;
								paymentPOCO.SATTransferStatusCode = "TD";
								paymentPOCO.TransmissionError = null;
								arpaymentRep.Update(paymentPOCO);
								arpaymentRep.SubmitChanges();
							}
						}

						sATInterfaceHelper.UpdatePaymentInvoicesSATStatus(payment.Id, waitingCommLog.Tenant, paymentComprobante.Complemento.Any[0], arinvoiceRep, arpaymentRep);

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
						HandlePaymentError(new SATErrorArgs 
						{ 
							WaitingCommLog = waitingCommLog, 
							ArpaymentRep = arpaymentRep, 
							CommunicationLogRep = communicationLogRep, 
							TransError = transError, 
							ResultadoTimbre = resultadoTimbre 
						});
					}
				}
				else
				{
					HandlePaymentError(new SATErrorArgs 
					{ 
						WaitingCommLog = waitingCommLog, 
						ArpaymentRep = arpaymentRep, 
						CommunicationLogRep = communicationLogRep, 
						TransError = transError, 
						ResultadoTimbre = resultadoTimbre 
					});
				}
			}
			else
			{
				ARInvoicePM invoice = invoiceQuery.GetSinglePM(waitingCommLog.EntityId, waitingCommLog.Tenant);
				ARInvoice invoicePOCO = arinvoiceRep.GetSingleARInvoice(waitingCommLog.EntityId, waitingCommLog.Tenant);

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

						Profact.TimbraCFDI40.Comprobante invoiceComprobante = Logitude.Server.Tools.LogitudeXmlSerializer.DeserializeObject<Profact.TimbraCFDI40.Comprobante>(resultadoConsulta.Xml);

						if (isConcurrencyToggleEnabled)
						{
							invoice.IsUpdatedBySAT = true;
							invoice.SATApprovalDate = GetSATApprovalDateFromComplemento(waitingCommLog, invoiceComprobante.Complemento.Any);
							invoice.SATXML = resultadoConsulta.Xml;
							invoice.SATAdditionalFieldsXML = LogitudeXmlSerializer.SerializeObjectToXmlString<SATAdditionalFields>(additional);
							invoice.SATTransferStatusCode = "TD";
							invoice.TransmissionError = null;
							ARInvoiceService invoiceService = new ARInvoiceService(invoiceContext, waitingCommLog.Tenant);
							invoiceService.Update(invoice);
						}

						else
						{
							if (invoicePOCO != null)
							{
								invoicePOCO.SATApprovalDate = GetSATApprovalDateFromComplemento(waitingCommLog, invoiceComprobante.Complemento.Any);
								invoicePOCO.SATXML = resultadoConsulta.Xml;
								invoicePOCO.SATAdditionalFieldsXML = LogitudeXmlSerializer.SerializeObjectToXmlString<SATAdditionalFields>(additional);
								invoicePOCO.SATTransferStatusCode = "TD";
								invoicePOCO.TransmissionError = null;
								arinvoiceRep.Update(invoicePOCO);
								arinvoiceRep.SubmitChanges();
							}
						}

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
						HandleInvoiceError(new SATErrorArgs
						{
							WaitingCommLog = waitingCommLog,
							ArinvoiceRep = arinvoiceRep,
							CommunicationLogRep = communicationLogRep,
							TransError = transError,
							Invoice = invoicePOCO,
							InvoicePM = invoice,
							ResultadoTimbre = resultadoTimbre
						});
					}
				}
				else
				{
					HandleInvoiceError(new SATErrorArgs
					{
						WaitingCommLog = waitingCommLog,
						ArinvoiceRep = arinvoiceRep,
						CommunicationLogRep = communicationLogRep,
						TransError = transError,
						Invoice = invoicePOCO,
						InvoicePM = invoice,
						ResultadoTimbre = resultadoTimbre
					});
				}
			}
		}

		private DateTime GetSATApprovalDateFromComplemento(CommunicationLog waitingCommLog, XmlElement[] anycomplemento)
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

		private void HandlePaymentError(SATErrorArgs sATErrorArgs)
		{
			ARPaymentPM payment = paymentQuery.GetSinglePM(sATErrorArgs.WaitingCommLog.EntityId, sATErrorArgs.WaitingCommLog.Tenant);
			ARPayment paymentPOCO = sATErrorArgs.ArpaymentRep.GetSingleARPayment(sATErrorArgs.WaitingCommLog.EntityId, sATErrorArgs.WaitingCommLog.Tenant);

			if (sATErrorArgs.TransError != payment.TransmissionError || payment.SATTransferStatusCode != "TE")
			{
				if (isConcurrencyToggleEnabled)
				{
					if (payment != null)
					{
						payment.IsUpdatedBySAT = true;
						payment.SATTransferStatusCode = "TE";
						payment.TransmissionError = sATErrorArgs.TransError;
						ARPaymentService aRPaymentService = new ARPaymentService(invoiceContext, payment.Tenant);
						aRPaymentService.Update(payment);
					}
				}

				else
				{
					if (paymentPOCO != null)
					{
						paymentPOCO.SATTransferStatusCode = "TE";
						paymentPOCO.TransmissionError = sATErrorArgs.TransError;
						sATErrorArgs.ArpaymentRep.Update(paymentPOCO);
						sATErrorArgs.ArpaymentRep.SubmitChanges();

					}
				}
			}

			SaveCommunicationLogAsDoneWithSATError(sATErrorArgs);
		}

		private void HandleInvoiceError(SATErrorArgs sATErrorArgs)
		{
			sATErrorArgs.InvoicePM = invoiceQuery.GetSinglePM(sATErrorArgs.WaitingCommLog.EntityId, sATErrorArgs.WaitingCommLog.Tenant);
			if (sATErrorArgs.TransError != sATErrorArgs.Invoice.TransmissionError || sATErrorArgs.Invoice.SATTransferStatusCode != "TE")
			{
				if (isConcurrencyToggleEnabled)
				{
					sATErrorArgs.InvoicePM.IsUpdatedBySAT = true;
					sATErrorArgs.InvoicePM.SATTransferStatusCode = "TE";
					sATErrorArgs.InvoicePM.TransmissionError = sATErrorArgs.TransError;
					ARInvoiceService aRInvoiceService = new ARInvoiceService(invoiceContext, sATErrorArgs.InvoicePM.Tenant);
					aRInvoiceService.Update(sATErrorArgs.InvoicePM);
				}

				else
				{
					sATErrorArgs.Invoice.SATTransferStatusCode = "TE";
					sATErrorArgs.Invoice.TransmissionError = sATErrorArgs.TransError;
					sATErrorArgs.ArinvoiceRep.Update(sATErrorArgs.Invoice);
					sATErrorArgs.ArinvoiceRep.SubmitChanges();
				}
			}

			SaveCommunicationLogAsDoneWithSATError(sATErrorArgs);
		}

		private void SaveCommunicationLogAsDoneWithSATError(SATErrorArgs sATErrorArgs)
		{
			string exceptionMessage = "Done, with SAT Error: " + sATErrorArgs.TransError + "\nMore Details: " + sATErrorArgs.ResultadoTimbre.Detalle;
			sATErrorArgs.WaitingCommLog.CommunicationStatusTypeCode = "D";
			sATErrorArgs.WaitingCommLog.ExceptionMessage = StringHelper.TruncateLongString(exceptionMessage, 7000);
			sATErrorArgs.WaitingCommLog.LastStatusDate = TenantServerConfigration.GetCurrentDateTime(sATErrorArgs.WaitingCommLog.Tenant);

			if (context != null)
			{
				sATErrorArgs.CommunicationLogRep.Update(sATErrorArgs.WaitingCommLog);
				sATErrorArgs.CommunicationLogRep.SubmitChanges();
			}

			queueservice.Complete();
		}

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
				EntityId = payment.Id,
				ObjectTableId = arObjectTable.Id,
				EntityNumber = payment.PaymentNo,
			};

			documentsService.Create(extDocPM, fileData, systemUser.Id, false);
		}

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
				EntityId = string.IsNullOrEmpty(invoice.MainEntityId) ? invoice.Id : invoice.MainEntityId,
				EntityNumber = string.IsNullOrEmpty(invoice.MainEntityReference) ? invoice.InvoiceNumber : invoice.MainEntityReference,
				ChildEntityId = invoice.Id,
				ChildObjectTableId = arObjectTable.Id,
				ChildEntityReference = invoice.InvoiceNumber,

			};
			extDocPM.ObjectTableId = !invoice.IsConsolidationInvoice ? entityObjectTable.Id : arObjectTable.Id;

			documentsService.Create(extDocPM, fileData, systemUser.Id, false);

		}
	}

	public class SATProfact40ServiceArgs
    {
		public CommunicationLog WaitingCommLog { get; set; }
		public CommunicationLogRepository CommunicationLogRep { get; set; }
		public byte[] DatainByte { get; set; }
		public Simplog.Data.InvoiceModel.EntityPOCOs.SATInterfaceSetting SatSetting { get; set; }
	}

	public class SATErrorArgs
	{
		public CommunicationLog WaitingCommLog { get; set; }
		public Simplog.Data.InvoiceModel.Repositories.ARPaymentRepository ArpaymentRep { get; set; }
		public Simplog.Data.InvoiceModel.Repositories.ARInvoiceRepository ArinvoiceRep { get; set; }
		public Simplog.Data.InvoiceModel.EntityPOCOs.ARInvoice Invoice { get; set; }
		public ARInvoicePM InvoicePM { get; set; }
		public CommunicationLogRepository CommunicationLogRep { get; set; }
		public string TransError { get; set; }
		public ResultadoTimbre ResultadoTimbre { get; set; }
	}
}
