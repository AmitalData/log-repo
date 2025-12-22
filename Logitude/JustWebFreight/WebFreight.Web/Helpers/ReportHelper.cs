

using Logitude.Accounting.BL.DataContract;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.ExternalService;
using Logitude.Server.Tools;
using Logitude.Server.Tools.Counters;
using Logitude.Server.Tools.Helpers;
using Logitude.Server.Tools.QueueService;
using Logitude.Server.Tools.StorageService;
using Logitude.SystemLogs;
using Microsoft.Practices.Unity;
using Newtonsoft.Json.Linq;
using NPOI.SS.UserModel;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; 
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.Helpers;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.DataContracts;
using Stimulsoft.Base;
using Stimulsoft.Base.Json;
using Stimulsoft.Report;
using Stimulsoft.Report.Dictionary;
using Stimulsoft.Report.Export;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Web.UI.WebControls;
using System.Xml.Serialization;
using WebFreight.Web.CommonDataModel.DomainServices;
using WebFreight.Web.DataProviders;
using WebFreight.Web.Helpers.DataProviderHelpers;
using WebFreight.Web.Helpers.ExcelReport;
using WebFreight.Web.ReportsWebServices;
using WebFreight.Web.ReportsWebServices.LogitudeReports;
using WebFreight.Web.ReportsWebServices.LogitudeReports.Accounting;
using WebFreight.Web.ReportsWebServices.LogitudeReports.Bluesnap;
using WebFreight.Web.ReportsWebServices.LogitudeReports.CRM;
using WebFreight.Web.ReportsWebServices.LogitudeReports.CRM.LogitudeCRMReport;
using WebFreight.Web.ReportsWebServices.LogitudeReports.Customs;
using WebFreight.Web.ReportsWebServices.LogitudeReports.Operational;
using WebFreight.Web.ReportsWebServices.LogitudeReports.Quotes.SpotRate;
using WebFreight.Web.ReportsWebServices.LogitudeReports.TimeManagement;
using WebFreight.Web.ShipmentPackageModel;
using WebFreight.Web.TaxesApprovalModel;
using WebFreight.Web.WebServices;
using System.Reflection;
using WebFreight.Web.DataContracts;
using Microsoft.VisualStudio.Services.Common;
using static Microsoft.VisualStudio.PlatformUI.SearchFilterDataSource;


namespace WebFreight.Web.Helpers
{
	public class ReportHelper
	{

		public void CopyReports(int? tenantNumber = null)
		{
			ReportRepository reportRepository = new ReportRepository(0);
			ReportsTemplateRepository reportsTemplateRepository = new ReportsTemplateRepository(0);
			ReportsTemplatesVersionRepository reportsTemplatesVersionRepository = new ReportsTemplatesVersionRepository(0);
			DocumentRepository documentRepository = new DocumentRepository(0);
			ReportModificationRepository reportModificationRep = new ReportModificationRepository(0);


			ContactRepository contactRepository = new ContactRepository(0);
			List<Report> reportList = reportRepository.GetReports(0).ToList();
			TenantQuery tenantQuery = new TenantQuery(0);
			List<TenantList> tenantList = new List<TenantList>();
			string proessType = "all Tenant";

			if (tenantNumber != null)
			{
				proessType = "Sign up";
				tenantList = tenantQuery.GetAllTenantLists().Where(d => d.Id == (int)tenantNumber).ToList();
			}
			else tenantList = tenantQuery.GetAllTenantLists().ToList();

			foreach (TenantList tenant in tenantList)
			{
				try
				{
					List<Report> myReports = new List<Report>();
					#region Report
					if (tenant.Id != 0)
					{
						myReports = reportRepository.GetReports(tenant.Id).ToList();

						foreach (Report report in reportList)
						{
							Report repo = myReports.Where(d => d.Code == report.Code && d.Tenant == tenant.Id).FirstOrDefault();
							if (repo == null)
							{
								Report newReport = new Report()
								{
									Id = IdCounter.GetNumber("Report", tenant.Id).ToString(),
									Tenant = tenant.Id,
									Name = report.Name,
									LocalName = report.LocalName,
									SearchFields = report.SearchFields,
									Code = report.Code,
									Description = report.Description,
									FilterControlName = report.FilterControlName,
									FilterHtmlComponentUrl = report.FilterHtmlComponentUrl,
									InActive = report.InActive,
									ReportGroupId = report.ReportGroupId,
									FeatureId = report.FeatureId,
									FeatureUniqeCode = report.FeatureUniqeCode,
									AvailableForScheduling = report.AvailableForScheduling,
									DisablePreview = report.DisablePreview,

								};
								reportRepository.Add(newReport);
								myReports.Add(newReport);

							}

						}

						reportRepository.SubmitChanges();

					}
					else
					{
						myReports = reportList;

					}
					#endregion

					#region Template and Version 

					if (reportsTemplateRepository.GetReportsTemplates(tenant.Id).FirstOrDefault() == null)
					{
						string userId = contactRepository.GetConactIdByemail("system@tenant" + tenant.Id.ToString() + ".com", tenant.Id);

						List<ReportModification> modifications = reportModificationRep.GetReportModifications(tenant.Id).ToList();
						List<Document> documentLists = new List<Document>();

						#region Full Documents Lists
						List<string> documentIds = new List<string>();
						foreach (Report myReport in myReports)
						{
							Report report = reportList.Where(D => D.Code == myReport.Code).FirstOrDefault();
							if (report == null) report = myReport;

							if (!string.IsNullOrEmpty(report.ReportDocumentId) && !documentIds.Contains(report.ReportDocumentId))
							{
								documentIds.Add(report.ReportDocumentId);
							}

							ReportModification modification = modifications.Where(d => d.ReportId == report.Id).FirstOrDefault();
							if (modification != null)
							{
								if (!string.IsNullOrEmpty(modification.ReportDocumentId) && !documentIds.Contains(modification.ReportDocumentId))
								{
									documentIds.Add(modification.ReportDocumentId);
								}
							}
						}
						if (documentIds.Count > 0)
						{
							documentLists = documentRepository.GetDocumentsByIds(documentIds);
						}


						#endregion

						if (documentLists.Count > 0)
						{
							List<ReportsTemplate> reportsTemplates = new List<ReportsTemplate>();
							foreach (Report myReport in myReports)
							{
								Report report = reportList.Where(D => D.Code == myReport.Code).FirstOrDefault();
								if (report == null) report = myReport;

								reportsTemplates = new List<ReportsTemplate>();
								#region Document

								string defaultTemplateId = "";

								//Tenant 0
								bool IsHaveReportTemplateInTenantZero = false;
								if (!string.IsNullOrEmpty(report.ReportDocumentId))
								{
									string documentId = AddDocument(documentRepository, report.ReportDocumentId, report.Tenant, tenant.Id, documentLists);
									if (!string.IsNullOrEmpty(documentId))
									{
										IsHaveReportTemplateInTenantZero = true;

										defaultTemplateId = AddReportTemplate(myReport.Id, myReport.Name, userId, documentId, tenant.Id, reportsTemplateRepository, reportsTemplatesVersionRepository, reportsTemplates, true, "R");
									}
								}

								//My Tenant
								ReportModification modification = modifications.Where(d => d.ReportId == report.Id).FirstOrDefault();
								if (modification != null)
								{
									string documentId = AddDocument(documentRepository, modification.ReportDocumentId, tenant.Id, tenant.Id, documentLists);
									if (!string.IsNullOrEmpty(documentId))
									{
										string description = myReport.Name;

										if (IsHaveReportTemplateInTenantZero)
										{
											ReportsTemplate reportsTemplate = reportsTemplates.Where(d => d.ReportId == myReport.Id && d.Tenant == myReport.Tenant).FirstOrDefault();
											if (reportsTemplate != null) reportsTemplate.Description += " (1)";
											description += " (2)";
										}

										defaultTemplateId = AddReportTemplate(myReport.Id, description, userId, documentId, tenant.Id, reportsTemplateRepository, reportsTemplatesVersionRepository, reportsTemplates, false, "R");
									}
								}

								if (!string.IsNullOrEmpty(defaultTemplateId))
								{
									myReport.DefaultTemplateId = defaultTemplateId;
									reportRepository.Update(myReport);
								}

								#endregion


							}

							documentRepository.SubmitChanges();
							reportsTemplateRepository.SubmitChanges();
							reportsTemplatesVersionRepository.SubmitChanges();
							reportRepository.SubmitChanges();
						}

					}
					#endregion
				}
				catch (Exception ex)
				{

					string data = "@ Tenant : " + tenant.Id + "@ Exception : " + ex.Message;
					AzureLog.SaveLogsInStorage(data, "P", DateTime.Now, "", "", 0, "", "Copy Report in " + proessType, null);
				}
			}

		}


		//private bool IsUsingFileStreamAndTiffImage(int tenant)
		// {
		//     return (tenant == 1526 || tenant == 1) ? true : false;
		// }

		public string GetSpecificPageFromStimulReportAsBase64(ReportFliter reportFliter)
		{
			string url = "";
			string extension = "tiff"; //IsUsingFileStreamAndTiffImage(reportFliter.tenant) ? "tiff" : "mdc";
			IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
			BlobFileInfo fileInfo = GetNewBlobFileInfo((reportFliter.ReportKey + "@" + reportFliter.ReportName + extension), extension, reportFliter.tenant);
			byte[] result = storageservice.Read(fileInfo);
			if (result != null)
			{
				url = GetSpecificPageFromTiffImageAsBase64(reportFliter,ref result);

			}

			storageservice.Dispose();

			return url;
		}






        public string AddReportTemplate(string reportId, string description, string userId, string documentId, int tenant, ReportsTemplateRepository reportsTemplateRepository, ReportsTemplatesVersionRepository reportsTemplatesVersionRepository, List<ReportsTemplate> reportsTemplates, bool isSystem, string templateType, string entityId = null, string objectTableId = null, string subject = null, string originalTemplateId = null, bool useStimul = false)
		{
            NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"Creating report template for report: {reportId}, template type: {templateType}, tenant: {tenant}, description: {description}");

			#region ReportsTemplate

			ReportsTemplate reportsTemplate = new ReportsTemplate()
			{
				Id = IdCounter.GetNumber("ReportsTemplate", tenant).ToString(),
				Tenant = tenant,
				CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
				UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
				Description = description,
				ReportId = reportId,
				CreatedByUserId = userId,
				UpdatedByUserId = userId,
				IsSystem = isSystem,
				InActive = false,
				CurrentVersion = 1,
				TemplateType = templateType,
				EntityId = entityId,
				ObjectTableId = objectTableId,
                OriginalTemplateId = originalTemplateId,
                Subject = subject,
				UseStimul = useStimul
			};
			reportsTemplateRepository.Add(reportsTemplate);

			if (reportsTemplates != null)
			{
				reportsTemplates.Add(reportsTemplate);
			}
			#endregion

			#region ReportsTemplatesVersion

			ReportsTemplatesVersion reportsTemplatesVersion = new ReportsTemplatesVersion()
			{
				Id = IdCounter.GetNumber("ReportsTemplatesVersion", tenant).ToString(),
				Tenant = tenant,
				CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
				UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
				ReportId = reportId,
				CreatedByUserId = userId,
				UpdatedByUserId = userId,
				TemplateId = reportsTemplate.Id,
				Version = 1,
				ReportDocumentId = !string.IsNullOrEmpty(documentId) ? documentId : null,
			};

			reportsTemplatesVersionRepository.Add(reportsTemplatesVersion);

			#endregion

			return reportsTemplate.Id;
		}

		public string AddDocument(DocumentRepository documentRepository, string documentId, int tenant, int mytenant, List<Document> documentLists)
		{
			Document newDocument = null;
			string result = "";
			Document document = documentLists.Where(d => d.Id == documentId && d.Tenant == tenant).FirstOrDefault();
			if (document != null)
			{
				IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
				BlobFileInfo fileInfo = new BlobFileInfo()
				{
					FileName = document.Id,
					FolderName = "reports",
					Extension = document.Extension,
					Tenant = document.Tenant,

				};
				byte[] fileData = storageservice.Read(fileInfo);

				if (fileData != null)
				{

					newDocument = new Document
					{
						Id = IdCounter.GetNumber("Document", mytenant).ToString(),
						Tenant = mytenant,
						CreateDate = TenantServerConfigration.GetCurrentDateTime(mytenant),
						FileName = document.FileName,
						FileSize = document.FileSize,
						Extension = document.Extension,
						HasFile = document.HasFile,
						CalculatedFileName = document.CalculatedFileName,
						Folder = document.Folder,

					};
					documentRepository.Add(newDocument);



					fileInfo = new BlobFileInfo()
					{
						FileName = newDocument.Id,
						FolderName = newDocument.Folder,
						Extension = newDocument.Extension,
						Tenant = mytenant,
						FileSize = newDocument.FileSize,
						IsEncrypted = true,
					};


					storageservice.Write(fileData, fileInfo);
				}
			}

			if (newDocument != null)
			{
				result = newDocument.Id;
			}


			return result;

		}

		public ReportsTemplate CopyReportsTemplate(string reportsTemplateId, string userId, int tenant)
		{
			ReportsTemplateRepository reportsTemplateRepository = new ReportsTemplateRepository(tenant);
			ReportsTemplate originalReportsTemplate = reportsTemplateRepository.GetSingleReportsTemplate(reportsTemplateId, tenant);
			ReportsTemplate reportsTemplate = null;
			if (originalReportsTemplate != null)
			{
				reportsTemplate = new ReportsTemplate()
				{
					Id = IdCounter.GetNumber("ReportsTemplate", tenant).ToString(),
					Tenant = tenant,
					CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
					UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
					Description = originalReportsTemplate.Description,
					ReportId = originalReportsTemplate.ReportId,
					CreatedByUserId = userId,
					UpdatedByUserId = userId,
					IsSystem = false,
					InActive = false,
					CurrentVersion = 1,
					TemplateType = originalReportsTemplate.TemplateType,
				};
				reportsTemplateRepository.Add(reportsTemplate);

				ReportsTemplatesVersionRepository reportsTemplatesVersionRepository = new ReportsTemplatesVersionRepository(tenant);
				ReportsTemplatesVersion originalReportsTemplatesVersion = reportsTemplatesVersionRepository.GetLastReportsTemplatesVersionByReportsTemplateId(originalReportsTemplate.Id, tenant);

				#region CreateDocument
				string documentId = "";
				DocumentRepository documentRepository = new DocumentRepository(tenant);
				if (originalReportsTemplatesVersion != null && !string.IsNullOrEmpty(originalReportsTemplatesVersion.ReportDocumentId))
				{

					Document document = documentRepository.GetSingleDocument(originalReportsTemplatesVersion.Tenant, originalReportsTemplatesVersion.ReportDocumentId);
					if (document != null)
					{
						List<Document> documentLists = new List<Document>();
						documentLists.Add(document);
						documentId = AddDocument(documentRepository, document.Id, tenant, tenant, documentLists);

					}
				}
				#endregion

				ReportsTemplatesVersion reportsTemplatesVersion = new ReportsTemplatesVersion()
				{
					Id = IdCounter.GetNumber("ReportsTemplatesVersion", tenant).ToString(),
					Tenant = tenant,
					CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
					UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
					ReportId = originalReportsTemplate.ReportId,
					CreatedByUserId = userId,
					UpdatedByUserId = userId,
					TemplateId = reportsTemplate.Id,
					Version = 1,
					ReportDocumentId = !string.IsNullOrEmpty(documentId) ? documentId : null,
				};
				reportsTemplatesVersionRepository.Add(reportsTemplatesVersion);


				documentRepository.SubmitChanges();
				reportsTemplateRepository.SubmitChanges();
				reportsTemplatesVersionRepository.SubmitChanges();

			}


			return reportsTemplate;
		}

		public void CopyModifiedSystemReportsTemplates()
		{
			new SystemReportsTemplatesService().CopyModifiedByUsersTemplates();
		}
		public ReportsTemplatesVersion RestoreReportsTemplatesVersion(string reportsTemplatesVersionId, string userId, int tenant)
		{
			ReportsTemplatesVersion reportsTemplatesVersion = null;
			ReportsTemplatesVersionRepository reportsTemplatesVersionRepository = new ReportsTemplatesVersionRepository(tenant);
			ReportsTemplatesVersion currentReportsTemplatesVersion = reportsTemplatesVersionRepository.GetSingleReportsTemplatesVersionWithOutInclude(reportsTemplatesVersionId, tenant);
			if (currentReportsTemplatesVersion != null)
			{
				ReportsTemplateRepository reportsTemplateRepository = new ReportsTemplateRepository(tenant);
				ReportsTemplate reportsTemplate = reportsTemplateRepository.GetSingleReportsTemplate(currentReportsTemplatesVersion.TemplateId, tenant);

				#region CreateDocument
				string documentId = "";
				DocumentRepository documentRepository = new DocumentRepository(tenant);
				if (currentReportsTemplatesVersion != null && !string.IsNullOrEmpty(currentReportsTemplatesVersion.ReportDocumentId))
				{

					Document document = documentRepository.GetSingleDocument(currentReportsTemplatesVersion.Tenant, currentReportsTemplatesVersion.ReportDocumentId);
					if (document != null)
					{
						List<Document> documentLists = new List<Document>();
						documentLists.Add(document);
						documentId = AddDocument(documentRepository, document.Id, tenant, tenant, documentLists);
						documentRepository.SubmitChanges();

					}
				}
				#endregion

				#region reportsTemplatesVersion
				reportsTemplatesVersion = new ReportsTemplatesVersion()
				{
					Id = IdCounter.GetNumber("ReportsTemplatesVersion", tenant).ToString(),
					Tenant = tenant,
					CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
					UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
					ReportId = currentReportsTemplatesVersion.ReportId,
					CreatedByUserId = userId,
					UpdatedByUserId = userId,
					TemplateId = reportsTemplate.Id,
					Version = (reportsTemplate.CurrentVersion + 1),
					ReportDocumentId = !string.IsNullOrEmpty(documentId) ? documentId : null,
					IsRestored = true,
				};
				reportsTemplatesVersionRepository.Add(reportsTemplatesVersion);

				reportsTemplatesVersionRepository.SubmitChanges();
				#endregion


				#region reportsTemplates
				reportsTemplate.CurrentVersion = reportsTemplatesVersion.Version;
				reportsTemplate.UpdatedByUserId = userId;
				reportsTemplate.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
				reportsTemplateRepository.Update(reportsTemplate);
				reportsTemplateRepository.SubmitChanges();
				#endregion

			}
			return reportsTemplatesVersion;
		}

		public Document CreateDocumentAndWriteOnStorage(DocumentFile documentFile)
		{
			#region Create Document and Write on Storage
			DocumentRepository documentRepository = new DocumentRepository(documentFile.Tenant);

			Document newDocument = new Document
			{
				Id = IdCounter.GetNumber("Document", documentFile.Tenant).ToString(),
				Tenant = documentFile.Tenant,
				CreateDate = TenantServerConfigration.GetCurrentDateTime(documentFile.Tenant),
				FileName = documentFile.FileName,
				FileSize = documentFile.FileData.Length,
				Extension = documentFile.Extension,
				HasFile = true,
				CalculatedFileName = documentFile.FileName,
				Folder = documentFile.Folder,

			};

			documentRepository.Add(newDocument);
			documentRepository.SubmitChanges();

			IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;

			BlobFileInfo fileInfo = new BlobFileInfo()
			{
				FileName = newDocument.Id,
				FolderName = documentFile.Folder,
				Extension = documentFile.Extension,
				Tenant = documentFile.Tenant,
				FileSize = newDocument.FileSize,

			};

			storageservice.Write(documentFile.FileData, fileInfo);

			#endregion

			return newDocument;
		}


		public void StimulReportSaved(string processType, string reportTemplateId, byte[] fileData, string userId, int tenant)
		{
			if (fileData != null)
			{
				DocumentRepository documentRepository = new DocumentRepository(tenant);
				ReportsTemplateRepository reportsTemplateRepository = new ReportsTemplateRepository(tenant);
				ReportsTemplate reportsTemplate = reportsTemplateRepository.GetSingleReportsTemplate(reportTemplateId, tenant);

				if (processType == "SaveOnSameDocument")
				{
					ReportsTemplatesVersionRepository reportsTemplatesVersionRepository = new ReportsTemplatesVersionRepository(tenant);
					ReportsTemplatesVersion reportsTemplatesVersion = reportsTemplatesVersionRepository.GetLastReportsTemplatesVersionByReportsTemplateId(reportTemplateId, tenant);
					if (reportsTemplatesVersion != null)
					{
						IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
						BlobFileInfo fileInfo = new BlobFileInfo()
						{
							FileName = reportsTemplatesVersion.ReportDocumentId,
							FolderName = "reports",
							Extension = "mrt",
							Tenant = tenant,
							FileSize = fileData.Length,

						};
						storageservice.Write(fileData, fileInfo);
					}
				}
				else
				{

					ReportHelper reportHelper = new ReportHelper();
					DocumentFile documentFile = new DocumentFile() { FileName = reportsTemplate.Description, FileData = fileData, Extension = "mrt", Folder = "reports", Tenant = tenant };
					Document newDocument = reportHelper.CreateDocumentAndWriteOnStorage(documentFile);

					#region Create Reports Templates Version 
					ReportsTemplatesVersionRepository reportsTemplatesVersionRepository = new ReportsTemplatesVersionRepository(tenant);
					ReportsTemplatesVersion reportsTemplatesVersion = new ReportsTemplatesVersion()
					{
						Id = IdCounter.GetNumber("ReportsTemplatesVersion", tenant).ToString(),
						Tenant = tenant,
						CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
						UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
						ReportId = reportsTemplate.ReportId,
						CreatedByUserId = userId,
						UpdatedByUserId = userId,
						TemplateId = reportsTemplate.Id,
						Version = (reportsTemplate.CurrentVersion + 1),
						ReportDocumentId = newDocument.Id,
					};
					reportsTemplatesVersionRepository.Add(reportsTemplatesVersion);
					reportsTemplatesVersionRepository.SubmitChanges();
					#endregion

					#region Update Reports Template
					reportsTemplate.CurrentVersion = reportsTemplatesVersion.Version;
					reportsTemplate.UpdatedByUserId = userId;
					reportsTemplate.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
					reportsTemplateRepository.Update(reportsTemplate);
					reportsTemplateRepository.SubmitChanges();
					#endregion
				}

			}


		}
		public byte[] LoadDataToStimulReport(string processType, string reportTemplateId, int tenant, string reportsTemplateId, string templateType)
		{

			if (templateType == "E")
			{
				ReportsTemplateRepository reportsTemplateRepository = new ReportsTemplateRepository(tenant);
				ReportsTemplate reportsTemplate = reportsTemplateRepository.GetSingleReportsTemplate(reportsTemplateId, tenant);
				var documentId = processType == "ReportPreview" ? reportTemplateId : null;
				return new ExcelReportMrtBuilder(tenant).Build(reportsTemplate, documentId, processType == "ReportPreview");
			}

			Document document = null;
			DocumentRepository documentRepository = new DocumentRepository(tenant);
			if (processType == "ReportPreview")
			{
				document = documentRepository.GetSingleDocument(tenant, reportTemplateId);
			}
			else
			{
				ReportsTemplatesVersionQuery reportsTemplatesVersionQuery = new ReportsTemplatesVersionQuery(tenant);
				ReportsTemplatesVersionPM reportsTemplatesVersionPM = reportsTemplatesVersionQuery.GetLastReportsTemplatesVersionPMByReportsTemplateId(reportTemplateId, tenant);
				if (reportsTemplatesVersionPM != null)
				{
					document = documentRepository.GetSingleDocument(reportsTemplatesVersionPM.Tenant, reportsTemplatesVersionPM.ReportDocumentId);
				}
			}

			if (document == null)
			{
				return null;
			}

			return ReadFieldDataFromDocument(document);
		}

		private byte[] ReadFieldDataFromDocument(Document document)
		{

			IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
			BlobFileInfo fileInfo = new BlobFileInfo()
			{
				FileName = document.Id,
				FolderName = document.Folder,
				Extension = document.Extension,
				Tenant = document.Tenant,

			};
			byte[] fileData = storageservice.Read(fileInfo);
			return fileData;

		}

		public void UpdateReportLocalNames()
		{
			ReportRepository reportRepository = new ReportRepository(0);
			TenantQuery tenantQuery = new TenantQuery(0);
			List<TenantList> tenantList = new List<TenantList>();
			ContactRepository contactRepository = new ContactRepository(0);

			try
			{

				/// method  1
				List<Report> tenant0ReportListWithLocalNames = reportRepository.GetReportsWithLocalNamesTenant0().ToList();
				List<Report> nonTenant0ReportList = reportRepository.GetReportsExceptTenant0().ToList();

				foreach (Report report in tenant0ReportListWithLocalNames)
				{
					if (report != null)
					{
						List<Report> relatedReports = nonTenant0ReportList.Where(d => d.Code == report.Code).ToList();
						if (relatedReports != null)
						{
							relatedReports.ForEach((elem) =>
							{
								elem.LocalName = report.LocalName;
								reportRepository.Update(elem);
							});
							reportRepository.SubmitChanges();
						}
					}
				}


				/// method 2
				//List<Report> tenant0ReportList = reportRepository.GetReports(0).ToList();
				//List<Report> nonTenant0ReportList = reportRepository.GetReportsExceptTenant0().ToList();


				//foreach (Report report in nonTenant0ReportList)
				//{
				//    if (report != null)
				//    {
				//        Report t0Report = tenant0ReportList.Find(d => d.Code == report.Code);
				//        if (t0Report != null)
				//        {
				//            report.LocalName = t0Report.LocalName;
				//            reportRepository.Update(report);
				//            reportRepository.SubmitChanges();
				//        }
				//    }
				//}
			}
			catch (Exception ex)
			{
				throw new ApplicationException("RPTERR001:" + ex.Message);
			}
		}

		#region StimualReport


		public string GetReportDateTimeFormat(ReportFliter reportFliter)
		{
			ICommonDataContext context = CommonDataContext.GetContext(reportFliter.tenant);
			CommonDataDomainService commonService = new CommonDataDomainService();
			Tenant currentTenant = context.Tenants.Where(t => t.Id == reportFliter.tenant).FirstOrDefault();
			string datetimeformat = @"dd\/MM\/yyyy";
			if (!string.IsNullOrEmpty(currentTenant.DateTimeFormat)) datetimeformat = currentTenant.DateTimeFormat;
			return datetimeformat;


		}

		public void ReportAuthentication(ReportFliter reportFliter, int tenant)
		{
			if (tenant != reportFliter.tenant)
			{
				throw new Exception("Sorry you’re not authenticated to view this report");
			}

			if (!string.IsNullOrEmpty(reportFliter.UserId))
			{
				UserQuery userQuery = new UserQuery(tenant);
				bool isExist = userQuery.CheckIfUserExistInTenant(reportFliter.UserId, tenant);
				if (!isExist)
				{
					throw new Exception("Sorry you’re not authenticated to view this report");
				}
			}
		}
		public CustomerPotentialActualDataProvider BuildCustomerPotentialActualDataProvider(ReportFliter reportFliter)
		{
			CustomerPotentialActualDataProvider customerPotentialActualDataProvider = null;
			if (reportFliter != null)
			{
				byte[] filters = GetReportFilters(reportFliter.QueryFilterItemLists);
				byte[] reportDataProvider = BuildReportDataProvider(reportFliter, filters);
				if (reportDataProvider != null)
				{
					MemoryStream memorystream = new MemoryStream(reportDataProvider);
					XmlSerializer serializer = new XmlSerializer(typeof(CustomerPotentialActualDataProvider));
					customerPotentialActualDataProvider = (CustomerPotentialActualDataProvider)serializer.Deserialize(memorystream);
				}
			}
			return customerPotentialActualDataProvider;
		}
		public void CreateExcelOfReport(ReportFliter reportFliter)
		{
			AdvancedDateResolver advancedDateResolver = new AdvancedDateResolver();
			List<QueryFilterItem> reportFilterItems = advancedDateResolver.ResolveDateValues(reportFliter.QueryFilterItemLists);
			reportFliter.QueryFilterItemLists = reportFilterItems;


			ReportStimulDataProviderDetails reportStimulDataProviderDetails = null;
			if (reportFliter != null)
			{
				byte[] filters = GetReportFilters(reportFliter.QueryFilterItemLists);
				byte[] reportDataProvider = BuildReportDataProvider(reportFliter, filters);

				if (FeatureToggleHelper.HasFeatureToggle("DMS", reportFliter.tenant))
				{
					using (BufferedStream memorystream = new BufferedStream(new MemoryStream(reportDataProvider)))
					{
						reportStimulDataProviderDetails = GetReportStimulDataProviderDetails(memorystream, reportFliter);

					}
				}
				else
				{
					using (MemoryStream memorystream = new MemoryStream(reportDataProvider))
					{
						reportStimulDataProviderDetails = GetReportStimulDataProviderDetails(memorystream, reportFliter);
					}
				}

			}
            ExcelReportService reportsTemplateQuery = new ExcelReportService(reportFliter.tenant);
            ExportToExcelHelper exportToExcelHelper = new ExportToExcelHelper();

            IWorkbook workbook;

            if (reportFliter.DefaultExcelNoStimId == "DefExcelTempId")
			{
                 workbook = exportToExcelHelper.ExportToExcel(reportStimulDataProviderDetails.CurrentBusinessObject.BusinessObjectValue, reportStimulDataProviderDetails.CurrentBusinessObject.Name);
            }
            else
			{
                var selectedData = reportsTemplateQuery.GetSelectedDataProviderFields(reportFliter.ReportId, reportFliter.DefaultExcelNoStimId);
                var sortMap = new Dictionary<string, int>();
                var filteredData = FilterSelectedFieldsWithParent(reportStimulDataProviderDetails.CurrentBusinessObject.BusinessObjectValue, selectedData, null, sortMap);

                workbook = exportToExcelHelper.ExportToExcel(filteredData, reportStimulDataProviderDetails.CurrentBusinessObject.Name, sortMap);
            }


            MemoryStream memoryStream = new MemoryStream();
			workbook.Write(memoryStream);
            MemoryStream tempStream = new MemoryStream(memoryStream.ToArray());


            string tempFilePath = Path.GetTempFileName() + ".xlsx";
            using (FileStream fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
            {
                tempStream.Position = 0;
                tempStream.CopyTo(fileStream);
            }

            ReadFileFromStreamFileAndSaveOnStorgeByChunks(tempFilePath, reportFliter, ".xlsx");


        }


        private IDictionary<string, object> FilterSelectedFieldsWithParent(object source, List<DataProviderField> selectedFields, string parentName = null, Dictionary<string, int> sortMap = null)
        {
            var result = new Dictionary<string, object>();
            if (source == null || selectedFields == null)
                return result;

            foreach (var field in selectedFields)
            {
                var prop = source.GetType().GetProperty(field.Name);
                if (prop == null)
                    continue;

                var value = prop.GetValue(source);

                string columnName = !string.IsNullOrEmpty(field.Translation)
                    ? field.Translation
                    : (string.IsNullOrEmpty(parentName)
                        ? field.Name
                        : parentName + "_" + field.Name);


                if (field.Type == "List" && value is IEnumerable enumerable)
                {
                    var list = new List<object>();
                    foreach (var item in enumerable)
                    {
                        if (field.Fields?.Any() == true)
                        {
                            list.Add(FilterSelectedFieldsWithParent(item, field.Fields, field.Name, sortMap));
                        }
                        else
                        {
                            list.Add(item);
                        }
                    }
                    result[columnName] = list;
                }
                else
                {
                    sortMap?.TryAdd(columnName, field.Sort);
                    result[columnName] = value;
                }
            }

            return result;
        }

        public MemoryStream GetExcel(string reportKey,string fileName,int tenant)
        {
            string extension = ".xlsx";
             fileName = $"{reportKey}@{fileName}{extension}";

            BlobFileInfo fileInfo = GetNewBlobFileInfo(fileName, extension, tenant);

            IBlobService storageservice = ContainerAccessor.Container.Resolve(
                typeof(IBlobService),
                "StorageService",
                new ParameterOverride("", 1)
            ) as IBlobService;

            byte[] result = storageservice.Read(fileInfo); 

            if (result == null || result.Length == 0)
                return null;

            return new MemoryStream(result);
        }

        public string BuildStimulReport(ReportFliter reportFliter)
		{
			AdvancedDateResolver advancedDateResolver = new AdvancedDateResolver();
			List<QueryFilterItem> reportFilterItems = advancedDateResolver.ResolveDateValues(reportFliter.QueryFilterItemLists);
			reportFliter.QueryFilterItemLists = reportFilterItems;

			string result = string.Empty;
			using (StiReport stiReport = GetStimulReportByReportFilter(reportFliter))
			{
				if (stiReport != null)
				{
					result = WriteReportToStorage(reportFliter, stiReport);
				}
			}
			return result;
		}

		public StiReport GetStimulReportByReportFilter(ReportFliter reportFliter,Boolean getStimulReportForMail = false)
		{
			StiReport report = null;
			if (reportFliter != null)
			{
				byte[] filters = GetReportFilters(reportFliter.QueryFilterItemLists);
				byte[] reportDataProvider = BuildReportDataProvider(reportFliter, filters);
				if (reportDataProvider == null && reportFliter.IsSchedulerReport) return report;
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"reportDataProvider length: {reportDataProvider.Length}");


                byte[] template = GetReportByteByType(reportFliter);
				if (template == null) throw new Exception("Report Template is missing");
				else
				{
					if (FeatureToggleHelper.HasFeatureToggle("DMS", reportFliter.tenant))
					{
						using (BufferedStream memorystream = new BufferedStream(new MemoryStream(reportDataProvider)))
						{
							ReportStimulDataProviderDetails reportStimulDataProviderDetails = GetReportStimulDataProviderDetails(memorystream, reportFliter);
							report = GetStimulReportByTemplateAndProviderDetails(reportStimulDataProviderDetails, template);
						}
					}
					else
					{
						using (MemoryStream memorystream = new MemoryStream(reportDataProvider))
						{
							ReportStimulDataProviderDetails reportStimulDataProviderDetails = GetReportStimulDataProviderDetails(memorystream, reportFliter);
							report = GetStimulReportByTemplateAndProviderDetails(reportStimulDataProviderDetails, template, getStimulReportForMail);
						}
					}
				}
			}
			return report;
		}

		private byte[] GetReportByteByType(ReportFliter reportFliter)
		{

			ReportsTemplatesVersionRepository reportsTemplatesVersionRepository = new ReportsTemplatesVersionRepository(reportFliter.tenant);
			ReportsTemplateRepository reportsTemplateRepository = new ReportsTemplateRepository(reportFliter.tenant);

			string reportDocumentId = reportsTemplatesVersionRepository.GetReportDocumentIdByReportTemplateId(reportFliter.DefaultTemplateId, reportFliter.tenant);
			var reportsTemplate = reportsTemplateRepository.GetSingleReportsTemplate(reportFliter.DefaultTemplateId, reportFliter.tenant);

			if (reportsTemplate.TemplateType == "E")
			{
				return new ExcelReportMrtBuilder(reportFliter.tenant).Build(reportsTemplate, reportDocumentId, true);
			}

			ReportsTemplatesWebService reportsTemplatesWebService = new ReportsTemplatesWebService();
			try
			{
				return reportsTemplatesWebService.GetReportTemplate(reportDocumentId, reportFliter.tenant, false);

			}
			catch (Exception ex)
			{

				throw ex;
			}

		}

		public byte[] GetReportFilters(List<QueryFilterItem> queryFilterItemLists)
		{
			QueryOperations queryOperations = new QueryOperations();
			queryOperations.QueryFilterItems = new System.Collections.Generic.List<QueryFilterItem>();
			if (queryFilterItemLists != null)
			{
				foreach (QueryFilterItem filterItem in queryFilterItemLists)
				{
					if (filterItem.FieldDataType == "Date")
					{
						if (filterItem.FieldValue != null)
							filterItem.FieldValue = DateTime.Parse(filterItem.FieldValue.ToString());
					}
					queryOperations.QueryFilterItems.Add(filterItem);
				}
			}
			FilterSerializer filterSeriazlizer = new FilterSerializer();
			byte[] filters = filterSeriazlizer.SerializeFilterItems(queryOperations);

			return filters;
		}

		public byte[] BuildReportDataProvider(ReportFliter reportFliter, byte[] filters)
		{
			LogitudeReportsWebService logitudeReportsWebService = new LogitudeReportsWebService();
			byte[] dataProvider = null;
			switch (reportFliter.ReportCode)
			{
				#region
				case "ATRE":
					{
						dataProvider = logitudeReportsWebService.LoadAutomationTestReportDataProvider(filters, reportFliter.tenant);
						break;
					}

				case "SHEL":
					{
						dataProvider = LoadShipmentsEventsListDataProvider(filters, reportFliter.tenant);
						break;
					}

				case "SHST":
					{
						dataProvider = logitudeReportsWebService.LoadShipmentsStocksData(filters, reportFliter.tenant);
						break;
					}
				case "SHRR":
					{
						ShipperReturnsManager myDataManager = new ShipperReturnsManager(filters, reportFliter.tenant);
						dataProvider = myDataManager.GetData();
						break;
					}
				case "ERLR":
					{
						ExternalReconciliationLinesReportLoader ExternalReconciliationManager = new ExternalReconciliationLinesReportLoader(filters, reportFliter.tenant);
						dataProvider = ExternalReconciliationManager.GetData();
						break;
					}
				case "URDR":
					{
						UserDefinedReportManager UserDefinedReportManager = new UserDefinedReportManager(filters, reportFliter.tenant);
						dataProvider = UserDefinedReportManager.GetData();
						break;
					}
				case "UPTR":
					{
						dataProvider = logitudeReportsWebService.LoadUsersByTenantData(filters, reportFliter.tenant);
						break;
					}

				case "INVN":
					{
						dataProvider = logitudeReportsWebService.LoadInventoryData(filters, reportFliter.tenant);
						break;
					}

				case "RACL":
					{
						AccountingLedgerManager myDataManager = new AccountingLedgerManager(filters, reportFliter.tenant);
						dataProvider = myDataManager.GetData();
						break;
					}

				case "ASDB":
					{
						DashBoardWebService dashBoardWebService = new DashBoardWebService();
						dataProvider = dashBoardWebService.LoadActivityStatusDetailsData(filters, reportFliter.tenant);
						break;
					}

				case "RAAR":
					{
						dataProvider = logitudeReportsWebService.LoadAgedAccountsReceivableData(filters, reportFliter.tenant);
						break;
					}

				case "EBRP":
					{
						dataProvider = logitudeReportsWebService.LoadBookingsData(filters, reportFliter.tenant);
						break;
					}

				case "RALS":
					{
						dataProvider = logitudeReportsWebService.LoadAirlineStatisticsData(filters, reportFliter.IncludeOperationalyClosed, reportFliter.tenant);
						break;
					}

				case "RSLS":
					{
						dataProvider = logitudeReportsWebService.LoadShippingLineStatisticsData(filters, reportFliter.IncludeOperationalyClosed, reportFliter.tenant);
						break;
					}

				case "CODT":
					{
						dataProvider = logitudeReportsWebService.LoadContainerDetailsVoyageData(filters, reportFliter.tenant);
						break;
					}

				case "COTR":
					{

						dataProvider = logitudeReportsWebService.LoadContainerTruckingData(filters, reportFliter.tenant);
						break;
					}

				case "CUAD":
					{
						dataProvider = logitudeReportsWebService.LoadCustomerAdditionalServicesData(filters, reportFliter.tenant);
						break;
					}

				case "CUPA":
					{
						dataProvider = logitudeReportsWebService.LoadCustomerPotentialActualData(filters, reportFliter.tenant);
						break;
					}

				case "EWRP":
					{
						dataProvider = logitudeReportsWebService.LoadEAWBsData(filters, reportFliter.tenant);
						break;
					}

				case "EXIN":
					{
						dataProvider = logitudeReportsWebService.LoadExpectedIncomeData(filters, reportFliter.tenant);
						break;
					}

				case "FBRP":
					{
						dataProvider = logitudeReportsWebService.LoadFlightBookingData(filters, reportFliter.tenant);
						break;
					}

				case "RITS":
					{
						dataProvider = logitudeReportsWebService.LoadIATAStatisticsData(filters, reportFliter.IncludeOperationalyClosed, reportFliter.tenant);
						break;
					}

				case "RIBP":
					{
						dataProvider = logitudeReportsWebService.LoadInvoiceByPartnerData(filters, reportFliter.CurrentCurrencyCodeType, reportFliter.DateType, reportFliter.tenant);
						break;
					}

				case "RINV":
					{
						dataProvider = logitudeReportsWebService.LoadInvoicesData(filters, reportFliter.tenant);
						break;
					}

				case "RAPI":
					{
						dataProvider = logitudeReportsWebService.LoadAPInvoicesData(filters, reportFliter.tenant);
						break;
					}

				case "MCOR":
					{
						MonthlyConversionManager myDataManager = new MonthlyConversionManager(filters, reportFliter.tenant);
						dataProvider = myDataManager.GetData();
						break;
					}

				case "OCRP":
					{
						RegisterShipmentPackageWebService registerShipmentPackageWebService = new RegisterShipmentPackageWebService();
						dataProvider = registerShipmentPackageWebService.RegisterShipments(filters, reportFliter.tenant, reportFliter.CustomerId);
						break;
					}

				case "PUAC":
					{
						dataProvider = logitudeReportsWebService.LoadParticipantsUsersActivitiesData(filters, reportFliter.tenant);
						break;
					}

				case "RPRS":
					{
						dataProvider = logitudeReportsWebService.LoadProfitByShipmentData(filters, reportFliter.CurrentCurrencyCodeType, reportFliter.IncludeOperationalyClosed, reportFliter.tenant);
						break;
					}

				case "RQUO":
					{
						dataProvider = logitudeReportsWebService.LoadQuotesData(filters, reportFliter.QuoteCustomerTypeCode, reportFliter.tenant);
						break;
					}

				case "SCHT":
				case "SCHA":
					{
						dataProvider = logitudeReportsWebService.LoadShipmentChargesAnalysisData(filters, reportFliter.tenant);
						break;
					}

				case "OPSC":
					{
						dataProvider = logitudeReportsWebService.LoadOpportunityStageChangingData(filters, reportFliter.tenant);
						break;
					}

				case "RSID":
					{
						dataProvider = logitudeReportsWebService.LoadStatementByInvoiceDateData(filters, reportFliter.tenant);
						break;
					}

				case "RSTA":
					{
						StatementReportManager manager = new StatementReportManager(filters, reportFliter.tenant);
						dataProvider = manager.GetData();
						break;
					}

				case "RSAS":
					{
						dataProvider = logitudeReportsWebService.LoadStatementAgingData(filters, reportFliter.tenant);
						break;
					}

				case "SBAG":
					{
						dataProvider = logitudeReportsWebService.LoadStatisticsByAgentData(filters, reportFliter.tenant);
						break;
					}

				case "RCLS":
					{
						StatisticsByCustomerManager manager = new StatisticsByCustomerManager(filters, reportFliter.tenant);
						dataProvider = manager.GetData();
						break;
					}

				case "ARID":
					{
						AccountingDepositReportManager accountingDepositReportManager = new AccountingDepositReportManager(filters, reportFliter.tenant);
						dataProvider = accountingDepositReportManager.GetData();
						break;
					}

				case "CASS":
					{
						dataProvider = logitudeReportsWebService.LoadCASSData(filters, reportFliter.tenant);
						break;
					}

				case "SPQS":
					{
						ShipmentProfitVSQuoteEstimateManager myDataManager = new ShipmentProfitVSQuoteEstimateManager(filters, reportFliter.tenant);
						dataProvider = myDataManager.GetData();
						break;
					}

				case "AREX":
					{
						ArchivoExportadoManager myDataManager = new ArchivoExportadoManager(filters, reportFliter.tenant);
						dataProvider = myDataManager.GetData();
						break;
					}

				case "DSCA":
					{
						DetailedShipmentChargesManager myDataManager = new DetailedShipmentChargesManager(filters, reportFliter.tenant);
						dataProvider = myDataManager.GetData();
						break;
					}

				case "INVR":
					{
						dataProvider = logitudeReportsWebService.LoadInvoicesVatAndRoutingData(filters, reportFliter.tenant);
						break;
					}

				case "EMTS":
					{
						//dataProvider = logitudeReportsWebService.LoadEmployeeTimeSheetData(filters, reportFliter.tenant);

						EmployeeTimeSheetManager myDataManager = new EmployeeTimeSheetManager(filters, reportFliter.tenant);
						dataProvider = myDataManager.GetData();

						break;
					}

				case "WDTS":
					{
						WorkPerDaysProjectManager myDataManager = new WorkPerDaysProjectManager(filters, reportFliter.tenant);
						dataProvider = myDataManager.GetData();

						//var isUsingNewCode = false;
						//if (isUsingNewCode)
						//{
						//    WorkPerDaysProjectManager myDataManager = new WorkPerDaysProjectManager(filters, reportFliter.tenant);
						//    dataProvider = myDataManager.GetData();
						//}

						//else
						//{
						//    dataProvider = logitudeReportsWebService.LoadWorkPerDaysProjectData(filters, reportFliter.tenant);
						//}

						break;
					}
				case "WGTS":
					{
						WorkPerDaysCategoryManager myDataManager = new WorkPerDaysCategoryManager(filters, reportFliter.tenant);
						dataProvider = myDataManager.GetData();
						break;
					}
				case "TPTS":
					{
						dataProvider = logitudeReportsWebService.LoadTasksWithoutProjectsData(filters, reportFliter.tenant);
						break;
					}

                case "AGER":
                    {
                        dataProvider = logitudeReportsWebService.LoadAccountingAgingDataProvider(filters, reportFliter.tenant);
                        break;
                    }
				case "NAGR":
					{
						dataProvider = logitudeReportsWebService.LoadAccountingNewAgingDataProvider(filters, reportFliter.tenant);
						break;
					}
				case "OSBC":
                    {
                        dataProvider = logitudeReportsWebService.LoadOpenShipmentsByCustomerDataProvider(filters, reportFliter, reportFliter.tenant);
                        break;
                    }

				case "PTVC":
					{
						dataProvider = logitudeReportsWebService.LoadParentVsChildTenantsDataProvider(filters, reportFliter.tenant);
						break;
					}

				case "REXR":
					{
						dataProvider = logitudeReportsWebService.LoadRevenueExpenseDataProvider(filters, reportFliter.tenant);
						break;
					}

				case "TRBR":
					{
						dataProvider = logitudeReportsWebService.LoadTrailBalanceDataProvider(filters, reportFliter.tenant);
						break;
					}

				case "LICM":
					{
						dataProvider = logitudeReportsWebService.LoadLicenseManagementDataProvider(filters, reportFliter.tenant);
						break;
					}

				case "SHID":
					{
						dataProvider = logitudeReportsWebService.LoadShipmentDetailsDataProvider(filters, reportFliter, reportFliter.tenant);
						break;
					}

				case "VDK":
					{
						dataProvider = logitudeReportsWebService.LoadVDKDataProvider(filters, reportFliter.tenant);
						break;
					}
				case "VEHI":
					{

						VehiclesManager myDataManager = new VehiclesManager(filters, reportFliter.tenant);
						dataProvider = myDataManager.GetData();
						break;
					}
				case "VDCA":
					{
						VendorChargesAnalysisManager myDataManager = new VendorChargesAnalysisManager(filters, reportFliter.tenant);
						dataProvider = myDataManager.GetData();
						break;
					}

				case "LTRP":
					{
						dataProvider = logitudeReportsWebService.LoadLedgerTransactionDataProvider(filters, reportFliter, reportFliter.tenant);
						break;
					}

				case "UNER":
					{
						UnicargoExportManager myDataManager = new UnicargoExportManager(filters, reportFliter.tenant);
						dataProvider = myDataManager.GetData();
						break;
					}

				case "FLBM":
					{
						FlightBookingsManifestManager myDataManager = new FlightBookingsManifestManager(filters, reportFliter.tenant);
						dataProvider = myDataManager.GetData();
						break;
					}

				case "RCRF":
					{
						RacingQuoteManager myDataManager = new RacingQuoteManager(filters, reportFliter.tenant);
						dataProvider = myDataManager.GetData();
						break;
					}

				case "BSPR":
					{
						BluesnapPaymentsReportManager myDataManager = new BluesnapPaymentsReportManager(filters, reportFliter.tenant);
						dataProvider = myDataManager.GetData();
						break;
					}
				case "CSSR":
					{
						dataProvider = logitudeReportsWebService.LoadCustomerStatusDataProvider(filters, reportFliter.tenant);
						break;
					}
				case "LOCR":
					{
						LogitudeCRMReportDataProviderService myDataService = new LogitudeCRMReportDataProviderService(filters, reportFliter.tenant);
						dataProvider = myDataService.Load();
						break;
					}
				case "PRVR":
					{
						TaxDeductionReportPerVendorService dataService = new TaxDeductionReportPerVendorService(filters, reportFliter.tenant);
						dataProvider = dataService.GetData();
						break;
					}
				case "ARIS":
					{
						ARinvoiceSequencesService aRinvoiceSequencesService = new ARinvoiceSequencesService(filters, reportFliter.tenant);
						dataProvider = aRinvoiceSequencesService.GetData();
						break;
					}
				case "SRQR":
					{
						SpotRateQuoteReportDataProviderService spotRateQuoteReportDataProviderService = new SpotRateQuoteReportDataProviderService(filters, reportFliter.tenant);
						dataProvider = spotRateQuoteReportDataProviderService.Load();
						break;
					}
				case "RCIL":
					{
						ControlForInvoiceLinesLoader controlForInvoiceLinesLoader = new ControlForInvoiceLinesLoader(filters, reportFliter.tenant);
						dataProvider = controlForInvoiceLinesLoader.GetData();
						break;
					}
				case "MBBR":
					{
						MonthlyBalancesLinesLoader controlForInvoiceLinesLoader = new MonthlyBalancesLinesLoader(filters, reportFliter.tenant);
						dataProvider = controlForInvoiceLinesLoader.GetData();
						break;
					}
				case "EXDE":
					{
						ExportDeclarationLoader myDataManager = new ExportDeclarationLoader(filters, reportFliter.tenant);
						dataProvider = myDataManager.GetData();
						break;
					}
                case "SHTO":
                    {
                        ShipmentFormLoader myDataManager = new ShipmentFormLoader(filters, reportFliter.tenant);
                        dataProvider = myDataManager.GetData();
                        break;
                    }
                case "ECCR":
					{
						CustomsCollateralLoader myDataManager = new CustomsCollateralLoader(filters, reportFliter.tenant);
						dataProvider = myDataManager.GetData();
						break;
					}
                case "COO":
					{
						CertificateOfOriginLoader myDataManager = new CertificateOfOriginLoader(filters, reportFliter.tenant);
						dataProvider = myDataManager.GetData();
						break;
					}
				case "COOC":
					{
						CertificateOfOriginCountLoader myDataManager = new CertificateOfOriginCountLoader(filters, reportFliter.tenant);
						dataProvider = myDataManager.GetData();
						break;
					}
                case "NTRP":
                    {
                        dataProvider = logitudeReportsWebService.LoadNewLedgerTransactionDataProvider(filters, reportFliter, reportFliter.tenant);
                        break;
                    }
                    #endregion
            }
			return dataProvider;
		}
		public object dataprovider;
		public Dictionary<string, dynamic> myProperties;
		public List<ISlvLeaf> mylist ;

		public string GetDataProviderName(string code)
		{
			string dataProviderName = string.Empty;

			switch (code)
			{
				#region
				case "ATRE":
					{
						dataProviderName = "WebFreight.Web.Helpers.DataProviderHelpers.AutomationTestReportDataProvider";
						break;
					}

				case "SHEL":
					{
						dataProviderName = "WebFreight.Web.Helpers.DataProviderHelpers.ShipmentsEventsListDataProvider";
						break;
					}

				case "SHST":
					{
						dataProviderName = "WebFreight.Web.DataProviders.ShipmentsStocksDataProvider";

						break;
					}
				case "SHRR":
					{
						dataProviderName = "WebFreight.Web.DataProviders.ShipperReturnsDataProvider";

						break;
					}
				case "ERLR":
					{
						dataProviderName = "WebFreight.Web.DataProviders.ExternalReconciliationLinesReportDataProvider";
						break;
					}
				case "URDR":
					{
						dataProviderName = "WebFreight.Web.DataProviders.UserDefinedReportDataProvider";

						break;
					}
				case "UPTR":
					{
						dataProviderName = "WebFreight.Web.DataProviders.UsersByTenantDataProvider";


						break;
					}

				case "INVN":
					{
						dataProviderName = "WebFreight.Web.DataProviders.InventoryDataProvider";

						break;
					}

				case "RACL":
					{
						dataProviderName = "WebFreight.Web.DataProviders.AccountingLedgerDataProvider";

						break;
					}

				case "ASDB":
					{
						dataProviderName = "WebFreight.Web.DataProviders.DashBoardDataClass";

						break;
					}

				case "RAAR":
					{
						dataProviderName = "WebFreight.Web.DataProviders.AgedAccountsReceivableDataProvider";

						break;
					}

				case "EBRP":
					{
						dataProviderName = "WebFreight.Web.DataProviders.BookingsDataProvider";

						break;
					}

				case "RALS":
					{
						dataProviderName = "WebFreight.Web.DataProviders.AirlineStatisticsDataProvider";

						break;
					}

				case "RSLS":
					{
						dataProviderName = "WebFreight.Web.DataProviders.ShippingLineStatisticsDataProvider";

						break;
					}

				case "CODT":
					{
						dataProviderName = "WebFreight.Web.DataProviders.ContainerDetailsVoyageDataProvider";

						break;
					}

				case "COTR":
					{
						dataProviderName = "WebFreight.Web.DataProviders.ContainerTruckingDataProvider";

						break;
					}

				case "CUAD":
					{
						dataProviderName = "WebFreight.Web.DataProviders.CustomerAdditionalServicesDataProvider";

						break;
					}

				case "CUPA":
					{
						dataProviderName = "WebFreight.Web.DataProviders.CustomerPotentialActualDataProvider";

						break;
					}

				case "EWRP":
					{
						dataProviderName = "WebFreight.Web.DataProviders.EAWBsDataProvider";

						break;
					}

				case "EXIN":
					{
						dataProviderName = "WebFreight.Web.DataProviders.ExpectedIncomeDataProvider";

						break;
					}

				case "FBRP":
					{
						dataProviderName = "WebFreight.Web.DataProviders.FlightBookingDataProvider";

						break;
					}

				case "RITS":
					{
						dataProviderName = "WebFreight.Web.DataProviders.IATAStatisticsDataProvider";

						break;
					}

				case "RIBP":
					{
						dataProviderName = "WebFreight.Web.DataProviders.InvoicesByPartnerDataProvider";

						break;
					}

				case "RINV":
					{
						dataProviderName = "WebFreight.Web.DataProviders.InvoiceDataProvider";


						break;
					}

				case "RAPI":
					{
						dataProviderName = "WebFreight.Web.DataProviders.IATAStatisticsDataProvider";


						break;
					}

				case "MCOR":
					{
						dataProviderName = "WebFreight.Web.DataProviders.OpportunityMonthlyConversionDataProvider";

						break;
					}

				case "OCRP":
					{
						dataProviderName = "WebFreight.Web.ShipmentPackageModel.RegisterShipmentPackageDataProvider";

						break;
					}

				case "PUAC":
					{
						dataProviderName = "WebFreight.Web.DataProviders.ParticipantsUsersActivitiesDataProvider";

						break;
					}

				case "RPRS":
					{
						dataProviderName = "WebFreight.Web.DataProviders.ProfitByShipmentDataProvider";

						break;
					}

				case "RQUO":
					{
						dataProviderName = "WebFreight.Web.DataProviders.QuotesDataProvider";

						break;
					}

				case "SCHT":
				case "SCHA":
					{
						dataProviderName = "WebFreight.Web.DataProviders.ShipmentChargesAnalysisDataProvider";

						break;
					}

				case "OPSC":
					{
						dataProviderName = "WebFreight.Web.DataProviders.OpportunityStageChangingDataProvider";


						break;
					}

				case "RSID":
					{
						dataProviderName = "WebFreight.Web.DataProviders.StatementByInvoiceDateDataProvider";

						break;
					}

				case "RSTA":
					{
						dataProviderName = "WebFreight.Web.DataProviders.StatementDataProvider";

						break;
					}

				case "RSAS":
					{
						dataProviderName = "WebFreight.Web.DataProviders.StatementDataProvider";

						break;
					}

				case "SBAG":
					{
						dataProviderName = "WebFreight.Web.DataProviders.StatisticsByAgentDataProvider";

						break;
					}

				case "RCLS":
					{
						dataProviderName = "WebFreight.Web.DataProviders.StatisticsByClientDataProvider";

						break;
					}

				case "ARID":
					{
						dataProviderName = "WebFreight.Web.DataProviders.ARInvoiceDepositDataProvider";

						break;
					}

				case "CASS":
					{
						dataProviderName = "WebFreight.Web.DataProviders.CASSDataProvider";

						break;
					}

				case "SPQS":
					{
						dataProviderName = "WebFreight.Web.DataProviders.ShipmentProfitVSQuoteEstimateDataProvider";

						break;
					}

				case "AREX":
					{
						dataProviderName = "WebFreight.Web.DataProviders.ArchivoExportadoDataProvider";

						break;
					}

				case "DSCA":
					{
						dataProviderName = "WebFreight.Web.DataProviders.ArchivoExportadoDataProvider";

						break;
					}

				case "INVR":
					{
						dataProviderName = "WebFreight.Web.DataProviders.ARInvoiceIncludeVATRoutingsDataProvider";

						break;
					}

				case "EMTS":
					{
						//dataProvider = logitudeReportsWebService.LoadEmployeeTimeSheetData(filters, reportFliter.tenant);
						dataProviderName = "WebFreight.Web.DataProviders.EmployeeTimeSheetDataProvider";

						break;
					}

				case "WDTS":
					{
						dataProviderName = "WebFreight.Web.DataProviders.WorkDaysPerProjectDataProvider";



						var isUsingNewCode = false;
						if (isUsingNewCode)
						{
							dataProviderName = "WebFreight.Web.DataProviders.WorkDaysPerProjectDataProvider";

						}

						else
						{
							dataProviderName = "WebFreight.Web.DataProviders.WorkDaysPerProjectDataProvider";

						}

						break;
					}
				case "WGTS":
					{
						dataProviderName = "WebFreight.Web.DataProviders.WorkDaysPerCategoryDataProvider";


						break;
					}
				case "TPTS":
					{
						dataProviderName = "WebFreight.Web.DataProviders.TasksWithoutProjectsDataProvider";

						break;
					}

				case "AGER":
					{
						dataProviderName = "WebFreight.Web.DataProviders.AccountingAgingDataProvider";

                        break;
                    }
				case "NAGR":
					{
						dataProviderName = "WebFreight.Web.DataProviders.NewAccountingAgingDataProvider";

						break;
					}
				case "OSBC":
                    {
                        dataProviderName = "WebFreight.Web.DataProviders.OpenShipmentsByCustomerDataProvider";

						break;
					}

				case "PTVC":
					{
						dataProviderName = "WebFreight.Web.DataProviders.ParentVsChildTenantsDataProvider";
						break;
					}

				case "REXR":
					{
						dataProviderName = "WebFreight.Web.DataProviders.RevenueExpenseDataProvider";

						break;
					}

				case "TRBR":
					{
						dataProviderName = "WebFreight.Web.DataProviders.RevenueExpenseDataProvider";

						break;
					}

				case "LICM":
					{
						dataProviderName = "WebFreight.Web.DataProviders.LicenseManagementDataProvider";

						break;
					}

				case "SHID":
					{
						dataProviderName = "WebFreight.Web.DataProviders.ShipmentDetailsDataProvider";

						break;
					}

				case "VDK":
					{
						dataProviderName = "WebFreight.Web.DataProviders.VDKDataProvider";


						break;
					}
				case "VEHI":
					{
						dataProviderName = "WebFreight.Web.DataProviders.VehiclesDataProvider";


						break;
					}
				case "VDCA":
					{
						dataProviderName = "WebFreight.Web.DataProviders.VendorChargesAnalysisDataProvider";

						break;
					}

				case "LTRP":
					{
						dataProviderName = "WebFreight.Web.DataProviders.LedgerTransactionsDataProvider";

						break;
					}

				case "UNER":
					{
						dataProviderName = "WebFreight.Web.DataProviders.UnicargoExportDataProvider";

						break;
					}

				case "FLBM":
					{
						dataProviderName = "WebFreight.Web.DataProviders.FlightBookingsManifestDataProvider";

						break;
					}

				case "RCRF":
					{
						dataProviderName = "WebFreight.Web.DataProviders.RacingQuoteDataProvider";

						break;
					}

				case "BSPR":
					{
						dataProviderName = "WebFreight.Web.DataProviders.BluesnapPaymentsDataProvider";

						break;
					}
				case "CSSR":
					{
						dataProviderName = "WebFreight.Web.DataProviders.CustomerStatusDataProvider";

						break;
					}
				case "LOCR":
					{
						dataProviderName = "WebFreight.Web.DataProviders.LogitudeCRMReportDataProvider";

						break;
					}
				case "PRVR":
					{
						dataProviderName = "Logitude.Accounting.BL.DataContract.TaxDeductionReportData";

						break;
					}
				case "ARIS":
					{
						dataProviderName = "Logitude.Accounting.BL.DataContract.ARinvoiceSequencesReportData";

						break;
					}
				case "SRQR":
					{
						dataProviderName = "WebFreight.Web.DataProviders.SpotRateQuoteReportDataProvider";


						break;
					}
				case "RCIL":
					{
						dataProviderName = "Logitude.Accounting.BL.DataContract.ControlForInvoiceLinesDataProvider";


                        break;
                    }
                case "MBBR":
                    {
						dataProviderName = "WebFreight.Web.DataProviders.MonthlyBalancesReportDataProvider";


						break;
					}
                case "NTRP":
                    {
                        dataProviderName = "WebFreight.Web.DataProviders.NewLedgerTransactionDataProvider";

                        break;
                    }
                    #endregion
            }

			return dataProviderName;
		}

		public List<ISlvLeaf> BuildDataProviderJson(string code)
		{
			LogitudeReportsWebService logitudeReportsWebService = new LogitudeReportsWebService();
			XmlSerializer serializer = new XmlSerializer(typeof(string));
			MemoryStream memstream = new MemoryStream();
			string dataProviderName = GetDataProviderName(code);

			mylist = new List<ISlvLeaf>();
			mylist = GetPropertyNames(dataProviderName, mylist);

			return mylist;
		}


		public class ISlvLeaf
		{
			public  string content { get; set; } // Example: "<span>Child</span>"
			public bool expanded { get; set; }
			public  List<ISlvLeaf> children { get; set; }
			public Type type { get; set; }
		}
		public List<ISlvLeaf> GetPropertyNames(string dataProviderName, List<ISlvLeaf> mylist)
		{
			NetCommonHelper.Logger.DevLog.Instance.WriteDebug($"dataProviderName: {dataProviderName}");
        
            Type t = null;

            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                try
                {
                    var type = assembly.GetTypes().FirstOrDefault(x => x.FullName == dataProviderName);
                    if (type != null)
                    {
                        t = type;
                        break;
                    }
                }
                catch (ReflectionTypeLoadException ex)
                {
                    var loaderErrors = ex.LoaderExceptions.Select(e => e.Message);
                    NetCommonHelper.Logger.DevLog.Instance.WriteDebug("LoaderExceptions: " + string.Join(" | ", loaderErrors));
                }
                catch (Exception ex)
                {
                    NetCommonHelper.Logger.DevLog.Instance.WriteDebug("General Exception: " + ex.Message);
                }
            }

            NetCommonHelper.Logger.DevLog.Instance.WriteDebug(message: $"dataProvider type: {t?.FullName}");
			var properties1 = t.GetProperties();

			foreach (var property in properties1)
			{
				try{				
					if (property.PropertyType.IsGenericType && property.PropertyType.GetGenericTypeDefinition() == typeof(List<>))
					{
						List<ISlvLeaf> childList = new List<ISlvLeaf>();
						ISlvLeaf iSlvLeaf = new ISlvLeaf()
						{
							content = property.Name,
							expanded = true,
							children = GetPropertyNames(property.PropertyType.GetGenericArguments()[0].FullName, childList)
						};
						mylist.Add(iSlvLeaf);

						//myProperties.Add(property.Name, dataProviderName);


					}
					else
					{
						ISlvLeaf iSlvLeaf = new ISlvLeaf()
						{
							content = property.Name,
							expanded = false,
							children = new List<ISlvLeaf>(),
							type = property.PropertyType
						};
						mylist.Add(iSlvLeaf);

					}
				}
				catch (Exception e)
				{
					return null;
				}
			}
			return mylist;

		}
		public ReportStimulDataProviderDetails GetReportStimulDataProviderDetails(Stream memorystream, ReportFliter reportFliter)
		{
			ReportStimulDataProviderDetails stimulReportDataProviderDetails = new ReportStimulDataProviderDetails();
			stimulReportDataProviderDetails.Tenant = reportFliter.tenant;

			var reportMap = new Dictionary<string, (Type type, string category)>
			{
				{ "RALS", (typeof(AirlineStatisticsDataProvider), "Airline Statistics") },
				{ "RSLS", (typeof(ShippingLineStatisticsDataProvider), "Shippingline Statistics") },
				{ "RPRS", (typeof(ProfitByShipmentDataProvider), "Profit By Shipment") },
				{ "RCLS", (typeof(StatisticsByClientDataProvider), "Statistics By Customer") },
				{ "PRVR", (typeof(TaxDeductionReportData), "PRVR") },
				{ "ARIS", (typeof(ARinvoiceSequencesReportData), "ARIS") },
				{ "RCIL", (typeof(ControlForInvoiceLinesDataProvider), "RCIL") },
				{ "MBBR", (typeof(MonthlyBalancesReportDataProvider), "MBBR") },
				{ "RSTA", (typeof(StatementDataProvider), "Statement") },
				{ "RIBP", (typeof(InvoicesByPartnerDataProvider), "InvoicesByPartner") },
				{ "RQUO", (typeof(QuotesDataProvider), "Quotes") },
				{ "RINV", (typeof(InvoiceDataProvider), "Invoices") },
				{ "RAPI", (typeof(InvoiceDataProvider), "Invoices") },
				{ "RAAR", (typeof(AgedAccountsReceivableDataProvider), "AgedAccountsReceivable") },
				{ "ROPF", (typeof(TaxesApproval), "TaxesApproval") },
				{ "RITS", (typeof(IATAStatisticsDataProvider), "IATA Statistics") },
				{ "RACL", (typeof(AccountingLedgerDataProvider), "Accounting Ledger") },
				{ "OCRP", (typeof(RegisterShipmentPackageDataProvider), "Shipment Packages") },
				{ "RSID", (typeof(StatementByInvoiceDateDataProvider), "StatementByInvoiceDate") },
				{ "OPSC", (typeof(OpportunityStageChangingDataProvider), "OpportunityStageChanging") },
				{ "MCOR", (typeof(OpportunityMonthlyConversionDataProvider), "OpportunityMonthlyConversion") },
				{ "EXIN", (typeof(ExpectedIncomeDataProvider), "ExpectedIncome") },
				{ "COTR", (typeof(ContainerTruckingDataProvider), "ContainerTrucking") },
				{ "CODT", (typeof(ContainerDetailsVoyageDataProvider), "ContainerDetailsVoyage") },
				{ "APOP", (typeof(ApprovedOpportunitiesDataProvider), "ApprovedOpportunities") },
				{ "CUAD", (typeof(CustomerAdditionalServicesDataProvider), "CustomerAdditionalServices") },
				{ "CUPA", (typeof(CustomerPotentialActualDataProvider), "CustomerPotentialActual") },
				{ "OPAS", (typeof(OpportunitiesAdditionalServicesDataProvider), "OpportunitiesAdditionalServices") },
				{ "SBAG", (typeof(StatisticsByAgentDataProvider), "Statistics By Agent") },
				{ "ASDB", (typeof(DashBoardDataClass), "DashBoardDataClass") },
				{ "EBRP", (typeof(BookingsDataProvider), "e-Booking") },
				{ "EWRP", (typeof(EAWBsDataProvider), "e-AWBs") },
				{ "FBRP", (typeof(FlightBookingDataProvider), "FlightBooking") },
				{ "SCHT", (typeof(ShipmentChargesAnalysisDataProvider), "ShipmentAnalysis") },
				{ "SCHA", (typeof(ShipmentChargesAnalysisDataProvider), "ShipmentAnalysis") },
				{ "PUAC", (typeof(ParticipantsUsersActivitiesDataProvider), "ParticipantsUsersActivities") },
				{ "ARID", (typeof(ARInvoiceDepositDataProvider), "Bank Deposit") },
				{ "CASS", (typeof(CASSDataProvider), "CASS") },
				{ "SPQS", (typeof(ShipmentProfitVSQuoteEstimateDataProvider), "Shipment Profit vs. Quote Estimate") },
				{ "DSCA", (typeof(ArchivoExportadoDataProvider), "Archivo Exportado") },
				{ "AREX", (typeof(ArchivoExportadoDataProvider), "Archivo Exportado") },
				{ "INVN", (typeof(InventoryDataProvider), "Inventory") },
				{ "INVR", (typeof(ARInvoiceIncludeVATRoutingsDataProvider), "ARInvoiceIncludeVATRoutings") },
				{ "EMTS", (typeof(EmployeeTimeSheetDataProvider), "EmployeeTimeSheet") },
				{ "WDTS", (typeof(WorkDaysPerProjectDataProvider), "WorkHoursPerProject") },
				{ "WGTS", (typeof(WorkDaysPerCategoryDataProvider), "WorkPerDaysCategoryManager") },
				{ "TPTS", (typeof(TasksWithoutProjectsDataProvider), "TasksWithoutProjects") },
				{ "AGER", (typeof(AccountingAgingDataProvider), "AGER") },
				{ "NAGR", (typeof(NewAccountingAgingDataProvider), "NAGR") },
				{ "LTRP", (typeof(LedgerTransactionsDataProvider), "LTRP") },
				{ "CSSR", (typeof(CustomerStatusDataProvider), "CSSR") },
				{ "OSBC", (typeof(OpenShipmentsByCustomerDataProvider), "OpenShipmentsByCustomer") },
				{ "PTVC", (typeof(ParentVsChildTenantsDataProvider), "ParentVsChildTenants") },
				{ "REXR", (typeof(RevenueExpenseDataProvider), "Accounting") },
				{ "TRBR", (typeof(RevenueExpenseDataProvider), "Accounting") },
				{ "UPTR", (typeof(UsersByTenantDataProvider), "UsersByTenant") },
				{ "LICM", (typeof(LicenseManagementDataProvider), "LicenseManagement") },
				{ "SHST", (typeof(ShipmentsStocksDataProvider), "Shipments Stocks") },
				{ "SHID", (typeof(ShipmentDetailsDataProvider), "ShipmentDetails") },
				{ "VDK",  (typeof(VDKDataProvider), "VDK") },
				{ "VEHI", (typeof(VehiclesDataProvider), "Vehicles") },
				{ "VDCA", (typeof(VendorChargesAnalysisDataProvider), "VendorChargesAnalysis") },
				{ "UNER", (typeof(UnicargoExportDataProvider), "UnicargoExport") },
				{ "SHEL", (typeof(ShipmentsEventsListDataProvider), "ShipmentsEventsList") },
				{ "SHRR", (typeof(ShipperReturnsDataProvider), "ShipperReturns") },
				{ "ERLR", (typeof(ExternalReconciliationLinesReportDataProvider), "ERLR") },
				{ "URDR", (typeof(UserDefinedReportDataProvider), "URDR") },
				{ "ATRE", (typeof(AutomationTestReportDataProvider), "AutomationTestReport") },
				{ "FLBM", (typeof(FlightBookingsManifestDataProvider), "FlightBookingsManifest") },
				{ "BSPR", (typeof(BluesnapPaymentsDataProvider), "BluesnapPayments") },
				{ "RCRF", (typeof(RacingQuoteDataProvider), "RacingQuote") },
				{ "LOCR", (typeof(LogitudeCRMReportDataProvider), "Logitude CRM Report") },
				{ "SRQR", (typeof(SpotRateQuoteReportDataProvider), "Spot Rate Quote Report") },
				{ "EXDE", (typeof(ExportDeclarationDataProvider), "EXDE") },
				{ "SHTO", (typeof(ShipmentFormDataProvider), "SHTO") },
				{ "ECCR", (typeof(CustomsCollateralDataProvider), "ECCR") },
				{ "COO",  (typeof(CertificateOfOriginDataProvider), "COO") },
				{ "COOC", (typeof(CertificateOfOriginCountDataProvider), "COOC") },
				{ "NTRP", (typeof(NewLedgerTransactionDataProvider), "NTRP") },
			};

            if (reportMap.TryGetValue(reportFliter.ReportCode, out var meta))
			{
                var serializer = new XmlSerializer(meta.type);
                var dataProvider = (dynamic)serializer.Deserialize(memorystream);

				try
				{
					BaseDataProviderService.FillBaseVariableFields(dataProvider, stimulReportDataProviderDetails.Tenant);
                    stimulReportDataProviderDetails.Logo = dataProvider.Logo;
                }
                catch (Exception ex)
				{
                    NetCommonHelper.Logger.DevLog.Instance.WriteError($"Failed to fill base variable fields for report: {reportFliter.ReportCode}, error: {ex.Message}");
                }

                stimulReportDataProviderDetails.CurrentBusinessObject = new StiBusinessObject
                {
                    Category = meta.category,
                    Name = meta.type.Name,
                    BusinessObjectValue = dataProvider
                };
            }
			else
			{
                throw new Exception($"Report code: {reportFliter.ReportCode} not found in report map");
            }
            return stimulReportDataProviderDetails;
        }

		private StiReport GetStimulReportByTemplateAndProviderDetails(ReportStimulDataProviderDetails reportStimulDataProviderDetails, byte[] reportTemplate, Boolean getStimulReportForMail = false)
		{
			StiReport report = new StiReport();
			ExportDocumentHelper exportDocumentHelper = new ExportDocumentHelper();



			string dllName = exportDocumentHelper.GetDllName(reportTemplate, report);

			//byte[] dllData = exportDocumentHelper.GetDllFromStorage(dllName, reportStimulDataProviderDetails.Tenant);

			if (false)
			{
				//if (dllData != null && dllData.Count() != 0)
				//{
				//    report = StiReport.GetReportFromAssembly(dllData);
				//    if (reportStimulDataProviderDetails.CurrentBusinessObject != null) exportDocumentHelper.RegBusinessObject(report, reportStimulDataProviderDetails.CurrentBusinessObject);
				//    report.NeedsCompiling = false;
				//    exportDocumentHelper.AddLogo(report, reportStimulDataProviderDetails.Logo);
				//}
				//else
				//{
				//    if (reportStimulDataProviderDetails.CurrentBusinessObject != null) exportDocumentHelper.RegBusinessObject(report, reportStimulDataProviderDetails.CurrentBusinessObject);
				//    report.Load(reportTemplate);

				//    exportDocumentHelper.AddLogo(report, reportStimulDataProviderDetails.Logo);

				//    exportDocumentHelper.SaveDllFileInStorage(reportTemplate, report, reportStimulDataProviderDetails.Tenant);
				//}
			}
			else
			{
				if (reportStimulDataProviderDetails.CurrentBusinessObject != null) exportDocumentHelper.RegBusinessObject(report, reportStimulDataProviderDetails.CurrentBusinessObject);
				report.Load(reportTemplate);
				exportDocumentHelper.AddLogo(report, reportStimulDataProviderDetails.Logo);
			}

			report.AutoLocalizeReportOnRun = true;

			if (LogitudeSettings.LogitudeURL != "http://localhost:9996"
				&& LogitudeSettings.LogitudeURL != "http://127.0.0.1:81" && !getStimulReportForMail)
			{
				report.ReportCacheMode = StiReportCacheMode.On;
				report.RenderedPages.CacheMode = true;
				report.RenderedPages.CanUseCacheMode = true;
			}
			//report.Culture = "he-IL"; // we can use report globalization to translate lables, google "Glabalization manager stimulsoft" for more
			report.Render(false);
			return report;
		}


		private string WriteReportToStorage(ReportFliter reportFliter, StiReport report)
		{
			SaveStimulReport(reportFliter, report);
			string url = "";
			if (!reportFliter.ReportsRunUsingWR)
			{
				url = ExportStimulaImage(report, reportFliter);
			}
			return url;
		}

		private void SaveStimulReport(ReportFliter reportFliter, StiReport report)
		{
			SaveStimulReportUsingFileStreamByFileType(reportFliter, report, "mdc");

			if (!reportFliter.DisablePreview)
			{
				SaveStimulReportUsingFileStreamByFileType(reportFliter, report, "tiff");
			}
		}


		private void SaveStimulReportUsingFileStreamByFileType(ReportFliter reportFliter, StiReport report, string fileType)
		{
			try
			{
				string tempFilePath = Path.Combine(Path.GetTempPath(), reportFliter.ReportKey + "." + fileType);
				using (var fileStream = new FileStream(tempFilePath, FileMode.Create, FileAccess.Write))
				{
					if (fileType == "mdc") report.SaveDocument(fileStream);
					else if (fileType == "tiff") report.ExportDocument(StiExportFormat.ImageTiff, fileStream, new StiTiffExportSettings() { PageRange = StiPagesRange.All, ImageResolution = 200 });
					else if (fileType == "xlsx") new StiExcel2007ExportService().ExportExcel(report, fileStream, new StiExcel2007ExportSettings() { UseOnePageHeaderAndFooter = true });
				}
				ReadFileFromStreamFileAndSaveOnStorgeByChunks(tempFilePath, reportFliter, fileType);
				SeveJsonDataProvider(report, reportFliter);
			}
			catch (Exception ex)
			{
				ExceptionHandler.HandleException(ex, DateTime.Now, 0, null, "SaveStimulReportUsingFileStream to " + fileType + "file", null, null);
				SaveStimulReportUsingMemoryStreamByFileType(reportFliter, report, fileType);
				SeveJsonDataProvider(report, reportFliter);
			}
		}

		private void SaveStimulReportUsingMemoryStreamByFileType(ReportFliter reportFliter, StiReport report, string fileType)
		{
			using (MemoryStream stream = new MemoryStream())
			{
				if (fileType == "mdc") report.SaveDocument(stream);
				else if (fileType == "tiff") report.ExportDocument(StiExportFormat.ImageTiff, stream, new StiTiffExportSettings() { PageRange = StiPagesRange.All, ImageResolution = 200 });
				else if (fileType == "xlsx") new StiExcel2007ExportService().ExportExcel(report, stream, new StiExcel2007ExportSettings() { UseOnePageHeaderAndFooter = true });
				if (stream != null)
				{
					byte[] reportData = stream.ToArray();
					if (reportData != null)
					{
						if (string.IsNullOrEmpty(reportFliter.ReportKey) || !reportFliter.ReportsRunUsingWR) reportFliter.ReportKey = Guid.NewGuid().ToString();
						BlobFileInfo fileInfo = GetNewBlobFileInfo((reportFliter.ReportKey + "@" + reportFliter.ReportName + fileType), fileType, reportFliter.tenant);
						fileInfo.FileSize = reportData.Length;

						IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
						storageservice.Write(reportData, fileInfo);
					}
				}
			}
		}

		private void SeveJsonDataProvider(StiReport report, ReportFliter reportFliter)
		{
			string json = JsonConvert.SerializeObject(report.BusinessObjectsStore[0].BusinessObjectValue);
			using (MemoryStream stream = new MemoryStream(Encoding.UTF8.GetBytes(json)))
			{
				byte[] reportData = stream.ToArray();
				BlobFileInfo fileInfo = GetNewBlobFileInfo(reportFliter.ReportKey + "@json", "json", reportFliter.tenant);
				fileInfo.FileSize = reportData.Length;
				IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
				storageservice.Write(reportData, fileInfo);
			}
		}

		private string GetJsonDataProvider(string reportKey, int tenant)
		{
			BlobFileInfo fileInfo = GetNewBlobFileInfo(reportKey + "@json", "json", tenant);
			IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
			byte[] bytes = storageservice.Read(fileInfo);
			string json = Encoding.UTF8.GetString(bytes);
			return json;
		}

		private string GetListFieldName(string reportCode)
		{
			switch (reportCode)
			{
				case "RINV":
					return "InvoicesReportList";

				default:
					throw new Exception("Report code not found");
			}
		}	

		private static void RemoveEmptyColumns(DataTable dataTable)
		{
			foreach (DataColumn column in dataTable.Columns.Cast<DataColumn>().ToList())
				if (dataTable.AsEnumerable().All(row => row.IsNull(column) || string.IsNullOrWhiteSpace(row[column].ToString())))
					dataTable.Columns.Remove(column);
		}

		private bool IsBase64Value(string value)
		{
			if (string.IsNullOrEmpty(value) || (value.Length % 4) != 0 ||
				!value.All(c => "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=".Contains(c)))
				return false;

			try
			{
				Convert.FromBase64String(value);
				return true;
			}
			catch
			{
				return false;
			}
		}

		private void AddSimpletoProperty(DataTable dataTable, JObject jObject)
		{
			List<JProperty> nonArrayProperties = jObject.Properties().Where(property => property.Value.Type != JTokenType.Array && property.Value.Type != JTokenType.Object).ToList();
			nonArrayProperties = nonArrayProperties.Where(prop => !IsBase64Value(prop.Value.ToString())).ToList();

			for (int i = 0; i < nonArrayProperties.Count; i++)
				if (dataTable.Columns.ToDynamicList().Any(dataColumn => dataColumn.ColumnName == nonArrayProperties[i].Name))
					nonArrayProperties[i] = new JProperty(nonArrayProperties[i].Name + "_1", nonArrayProperties[i].Value);

			nonArrayProperties.ForEach(column =>
			{
				Type columnType;
				switch (column.Value.Type)
				{
					case JTokenType.String:
						columnType = typeof(string);
						break;
					case JTokenType.Float:
					case JTokenType.Integer:
						columnType = typeof(decimal);
						break;
					case JTokenType.Date:
						columnType = typeof(DateTime);
						break;
					case JTokenType.Null:
						columnType = typeof(object);
						break;
					default:
						columnType = typeof(object);
						break;
				}
				dataTable.Columns.Add(column.Name, columnType);
			});

			foreach (DataRow row in dataTable.Rows)
				nonArrayProperties.ForEach(Columns => row[Columns.Name] = Columns.Value);
		}



        private void ReadFileFromStreamFileAndSaveOnStorgeByChunks(string tempFilePath, ReportFliter reportFliter, string extension)
        {
            BlobFileInfo fileInfo = GetNewBlobFileInfo((reportFliter.ReportKey + "@" + reportFliter.ReportName + extension), extension, reportFliter.tenant);
            IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
            List<string> blockIdsList = new List<string>();
			int bufferNumber = 0; long sendSize = 0;
            using (FileStream fileStream = new FileStream(tempFilePath, FileMode.Open, FileAccess.Read))
            {
                int bytesRead;
                fileInfo.FileSize = fileStream.Length;
                var buffer = new byte[GetChunkSize(fileStream.Length, sendSize)];
                while ((bytesRead = fileStream.Read(buffer, 0, buffer.Length)) > 0)
                {
					sendSize += buffer.Length;
                    var blockId = Convert.ToBase64String(Guid.NewGuid().ToByteArray());
                    blockIdsList.Add(blockId);
                    storageservice.WriteBlock(buffer, sendSize, blockIdsList.ToArray(), bufferNumber, fileInfo);
					bufferNumber += 1;
                    buffer = new byte[GetChunkSize(fileStream.Length, sendSize)];
                }
				fileStream.Close();
            }
            File.Delete(tempFilePath);
        }

        private long GetChunkSize(long fileSize, long sendSize)
		{
			long chunkSize = 1000000;
			if ((fileSize - sendSize) < chunkSize) chunkSize = fileSize - sendSize;

			return chunkSize;
		}

		public BlobFileInfo GetNewBlobFileInfo(string fileName, string extension, int tenant)
		{
			BlobFileInfo fileInfo = new BlobFileInfo()
			{
				FileName = fileName,
				FolderName = "others",
				Tenant = tenant,
				Extension = extension,
			};
			return fileInfo;
		}

		public string ExportStimulaImage(StiReport stiReport, ReportFliter reportFliter)
		{
			string url = "";
			using (MemoryStream memoryStream = new MemoryStream())
			{

				try
				{
					stiReport.ExportDocument(StiExportFormat.ImagePng, memoryStream, new StiPngExportSettings() { PageRange = new StiPagesRange(StiRangeType.Pages, reportFliter.NumberOfPage.ToString(), reportFliter.NumberOfPage), ImageResolution = 300, ImageFormat = StiImageFormat.Color });
				}
				catch (Exception ex)
				{
					stiReport.ExportDocument(StiExportFormat.ImagePng, memoryStream, new StiPngExportSettings() { PageRange = new StiPagesRange(StiRangeType.Pages, reportFliter.NumberOfPage.ToString(), reportFliter.NumberOfPage), ImageResolution = 200, ImageFormat = StiImageFormat.Color });
				}

				string base64String = System.Convert.ToBase64String(memoryStream.ToArray(), 0, memoryStream.ToArray().Length);
				reportFliter.PageCount = stiReport.RenderedPages.Count;
				url = "data:image/jpg;base64," + base64String;
			}
			return url;
		}

		private byte[] LoadShipmentsEventsListDataProvider(byte[] filters, int tenant)
		{

			LogitudeReportsWebService logitudeReportsWebService = new LogitudeReportsWebService();
			return logitudeReportsWebService.LoadShipmentsEventsListDataProvider(filters, tenant);
		}

        private bool IsHaveReport(string reportCode)
        {
            if (string.IsNullOrEmpty(reportCode))
            {
                switch (reportCode.ToLower())
                {
                    case "INVN":
                    case "RACL":
                    case "ASDB":
                    case "RAAR":
                    case "EBRP":
                    case "RALS":
                    case "RSLS":
                    case "CODT":
                    case "COTR":
                    case "CUAD":
                    case "CUPA":
                    case "EWRP":
                    case "EXIN":
                    case "FBRP":
                    case "RITS":
                    case "RIBP":
                    case "RINV":
                    case "RAPI":
                    case "MCOR":
                    case "OCRP":
                    case "PUAC":
                    case "RPRS":
                    case "RQUO":
                    case "SCHT":
                    case "SCHA":
                    case "OPSC":
                    case "RSID":
                    case "RSTA":
                    case "RSAS":
                    case "SBAG":
                    case "RCLS":
                    case "ARID":
                    case "CASS":
                    case "SPQS":
                    case "AREX":
                    case "INVR":
                    case "EMTS":
                    case "WDTS":
                    case "TPTS":
                    case "AGER":
                    case "OSBC":
                    case "PTVC":
                    case "LICM":
                    case "LTRP":
                    case "CSSR":
                    case "LOCR":
                    case "SRQR":
					case "NAGR":
					case "NTRP":
                        return true;

					default:
						return false;
				}
			}
			else return false;
		}


		public string GetSpecificPageFromTiffImageAsBase64(ReportFliter reportFliter,ref byte[] data)
		{
			string url = string.Empty;
			if (data != null)
			{
				using (MemoryStream byteStream = new MemoryStream())
				{
					System.Drawing.Bitmap bitmapReport = (Bitmap)System.Drawing.Image.FromStream(new MemoryStream(data));
					reportFliter.PageCount = bitmapReport.GetFrameCount(FrameDimension.Page);
					bitmapReport.SelectActiveFrame(FrameDimension.Page, reportFliter.NumberOfPage - 1);
					//MemoryStream byteStream = new MemoryStream();
					bitmapReport.Save(byteStream, ImageFormat.Jpeg);
					byte[] imagebyte = byteStream.ToArray();
					url = "data:image/jpg;base64," + System.Convert.ToBase64String(imagebyte, 0, imagebyte.Length);
				}
			}
			return url;
		}
		#endregion

		#region UpdateReport

        public void CopyFromTenant0(int tenant, int tenantToCopy, string reportCode = null)
        {
            ICommonDataContext commonDataContext = CommonDataContext.GetContext(tenantToCopy);
            reportsTemplateRepository = new ReportsTemplateRepository(commonDataContext);
            reportsTemplatesVersionRepository = new ReportsTemplatesVersionRepository(commonDataContext);
            documentRepository = new DocumentRepository(commonDataContext);

            ContactRepository contactRepository = new ContactRepository(commonDataContext);
            string userId = contactRepository.GetConactIdByemail("system@tenant" + tenantToCopy.ToString() + ".com", tenantToCopy);

            ReportGroupQuery reportGroupQuery = new ReportGroupQuery(tenantToCopy);
            string accountingReportGroupId = reportGroupQuery.GetReportGroupPMsByTenant(0).Where(a => a.Code == "RACC").Select(a => a.Id).FirstOrDefault();
            tenantZeroReportsTemplate = reportsTemplateRepository.GetReportsTemplates(0)
                .Where(d => d.IsCopiedAtSignup && d.Report.ReportGroupId == accountingReportGroupId && !d.InActive).ToList();
            tenantZeroReportsTemplatesVersionLists = reportsTemplatesVersionRepository.GetReportsTemplatesVersionsByReportsTemplateIds(tenantZeroReportsTemplate.Select(d => d.Id).ToList(), 0);
            documentLists = documentRepository.GetDocumentsByIds(tenantZeroReportsTemplatesVersionLists.Select(d => d.ReportDocumentId).ToList());
            List<Report> tenantZeroReports = tenantZeroReportsTemplate.Select(a => a.Report).Distinct().ToList();

            myTenantReportsTemplate = reportsTemplateRepository.GetReportsTemplatesWithOutInclude(tenantToCopy);
            myTenantReportsTemplatesVersion = reportsTemplatesVersionRepository.GetReportsTemplatesVersionsByReportsTemplateIds(myTenantReportsTemplate.Select(d => d.Id).ToList(), tenantToCopy);
            ReportRepository reportRepository = new ReportRepository(commonDataContext);
            List<Report> myReports = reportRepository.GetReports(tenantToCopy).ToList();

			if (!string.IsNullOrEmpty(reportCode))
			{
				tenantZeroReports = tenantZeroReports.Where(a => a.Code == reportCode).ToList();
            }

            tenantZeroReports.ForEach(report => CreateReportTemplates(report, tenantToCopy, userId, myReports));

            documentRepository.SubmitChanges();
            reportsTemplateRepository.SubmitChanges();
            reportsTemplatesVersionRepository.SubmitChanges();
            reportRepository.SubmitChanges();
        }

        private void CreateReportTemplates(Report report, int tenantToCopy, string userId, List<Report> myReports)
        {
            Report currentTenantReport = myReports.Where(d => d.Code == report.Code).FirstOrDefault();
            if (currentTenantReport == null)
            {
                NetCommonHelper.Logger.DevLog.Instance.WriteDebug("Report not found in tenant " + tenantToCopy + " for code " + report.Code);
                return;
            }

            HashSet<string> existingReportTemplates = myTenantReportsTemplate.Where(d => d.ReportId == currentTenantReport.Id).Select(a => a.OriginalTemplateId).ToHashSet();

            List<ReportsTemplate> reportsTemplateToAdd = tenantZeroReportsTemplate.Where(d => d.ReportId == report.Id && d.IsCopiedAtSignup && !existingReportTemplates.Contains(d.Id)).ToList();

            // create templates for report template type
            reportsTemplateToAdd.Where(d => d.TemplateType == "R").ToList().ForEach(reportTemplate => CreateNewReportsTemplate(new CopyReportTemplateArgs { tenant = tenantToCopy, userId = userId, report = report, systemReportTemplate = reportTemplate }, currentTenantReport));

            // create templates for excel template type
            reportsTemplateToAdd.Where(d => d.TemplateType == "E").ToList().ForEach(reportTemplate => AddExcelDocument(tenantToCopy, userId, report, reportTemplate, currentTenantReport));
        }


        private ReportsTemplateRepository reportsTemplateRepository;
		private ReportsTemplatesVersionRepository reportsTemplatesVersionRepository;
		private DocumentRepository documentRepository;
		private List<ReportsTemplate> tenantZeroReportsTemplate;
		private List<ReportsTemplate> myTenantReportsTemplate;
		private List<ReportsTemplatesVersion> tenantZeroReportsTemplatesVersionLists;
		private List<ReportsTemplatesVersion> myTenantReportsTemplatesVersion;
		private List<Document> documentLists;
		public void UpdateReports(int tenant)
		{

			ReportRepository reportRepository = new ReportRepository(tenant);
			reportsTemplateRepository = new ReportsTemplateRepository(tenant);
			reportsTemplatesVersionRepository = new ReportsTemplatesVersionRepository(tenant);
			documentRepository = new DocumentRepository(tenant);
			ContactRepository contactRepository = new ContactRepository(tenant);

			List<Report> reportList = reportRepository.GetReports(0).ToList();
			string userId = contactRepository.GetConactIdByemail("system@tenant" + tenant.ToString() + ".com", tenant);
			tenantZeroReportsTemplate = reportsTemplateRepository.GetReportsTemplatesWithOutInclude(0);
			myTenantReportsTemplate = reportsTemplateRepository.GetReportsTemplatesWithOutInclude(tenant);

			tenantZeroReportsTemplatesVersionLists = reportsTemplatesVersionRepository.GetReportsTemplatesVersionsByReportsTemplateIds(tenantZeroReportsTemplate.Select(d => d.Id).ToList(), 0);
			myTenantReportsTemplatesVersion = reportsTemplatesVersionRepository.GetReportsTemplatesVersionsByReportsTemplateIds(myTenantReportsTemplate.Select(d => d.Id).ToList(), tenant);
			documentLists = documentRepository.GetDocumentsByIds(tenantZeroReportsTemplatesVersionLists.Select(d => d.ReportDocumentId).ToList());
			List<Report> myReports = new List<Report>();

			if (tenant != 0)
			{
				myReports = reportRepository.GetReports(tenant).ToList();
				bool isChangeReport = false;
				foreach (Report report in reportList)
				{
					if (myReports.Where(d => d.Code == report.Code && d.Tenant == tenant).FirstOrDefault() == null)
					{
						Report newReport = new Report()
						{
							Id = IdCounter.GetNumber("Report", tenant).ToString(),
							Tenant = tenant,
							Name = report.Name,
							SearchFields = report.SearchFields,
							Code = report.Code,
							Description = report.Description,
							FilterControlName = report.FilterControlName,
							FilterHtmlComponentUrl = report.FilterHtmlComponentUrl,
							InActive = report.InActive,
							ReportGroupId = report.ReportGroupId,
							FeatureId = report.FeatureId,
							LocalName = report.LocalName,
							FeatureUniqeCode = report.FeatureUniqeCode,
							AvailableForScheduling = report.AvailableForScheduling,
							DisablePreview = report.DisablePreview,
							DefaultExcelTemplateId = report.DefaultExcelTemplateId,
							DefaultExcelNoStimId = report.DefaultExcelNoStimId

						};
						reportRepository.Add(newReport);
						myReports.Add(newReport);
						isChangeReport = true;
					}
				}

				if (isChangeReport) reportRepository.SubmitChanges();

                ReportGroupQuery reportGroupQuery = new ReportGroupQuery(tenant);
                string accountingReportGroupId = reportGroupQuery.GetReportGroupPMsByTenant(0).Where(a => a.Code == "RACC").Select(a => a.Id).FirstOrDefault();

				isChangeReport = false;
				foreach (Report report in reportList)
                {
                    if (report.ReportGroupId == accountingReportGroupId)
                    {
                        CreateReportTemplates(report, tenant, userId, myReports);
                    }
                    else
                    {


                        isChangeReport = UpdateExcelReports(tenant, userId, report, myReports) ? true : isChangeReport;
                        ReportsTemplate systemReportTemplate = tenantZeroReportsTemplate.Where(d => d.ReportId == report.Id && d.Id == report.DefaultTemplateId).FirstOrDefault();


                        if(systemReportTemplate != null)

                            isChangeReport = CopySystemReportTemplate(new CopyReportTemplateArgs { tenant = tenant, userId = userId, myReports = myReports, isChangeReport = isChangeReport, report = report, systemReportTemplate = systemReportTemplate });

                    }
                    ReportsTemplate systemEmailReportTemplate = tenantZeroReportsTemplate.Where(d => d.ReportId == report.Id && d.Id == report.DefaultMessageTemplateId).FirstOrDefault();
                    if (systemEmailReportTemplate != null)
						isChangeReport = CopySystemReportTemplate(new CopyReportTemplateArgs { tenant = tenant, userId = userId, myReports = myReports, isChangeReport = isChangeReport, report = report, systemReportTemplate = systemEmailReportTemplate });
				}


				if (isChangeReport)
				{
					documentRepository.SubmitChanges();
					reportsTemplateRepository.SubmitChanges();
					reportsTemplatesVersionRepository.SubmitChanges();
					reportRepository.SubmitChanges();
				}


			}

		}

		private bool CopySystemReportTemplate(CopyReportTemplateArgs copyReportTemplateArgs)
		{
			Report currentTenantReport = copyReportTemplateArgs.myReports.Where(d => d.Code == copyReportTemplateArgs.report.Code && d.Tenant == copyReportTemplateArgs.tenant).FirstOrDefault();
			if (currentTenantReport == null) return copyReportTemplateArgs.isChangeReport;
			ReportsTemplate currentTenantReportsTemplate = GetCurrentTenantReportsTemplate(currentTenantReport, copyReportTemplateArgs.systemReportTemplate.TemplateType);

			if (currentTenantReportsTemplate == null)
			{
				copyReportTemplateArgs.isChangeReport = CreateNewReportsTemplate(copyReportTemplateArgs, currentTenantReport);
			}
			else
			{
				copyReportTemplateArgs.isChangeReport = UpdateCurrentReportsTemplate(copyReportTemplateArgs, currentTenantReport, currentTenantReportsTemplate);
			}

			return copyReportTemplateArgs.isChangeReport;
		}

		private bool UpdateCurrentReportsTemplate(CopyReportTemplateArgs copyReportTemplateArgs, Report currentTenantReport, ReportsTemplate currentTenantReportsTemplate)
		{
			ReportsTemplatesVersion tenantZeroReportsTemplatesVersion = tenantZeroReportsTemplatesVersionLists.Where(d => d.ReportId == copyReportTemplateArgs.report.Id && d.TemplateId == copyReportTemplateArgs.systemReportTemplate.Id).FirstOrDefault();
			ReportsTemplatesVersion currentReportsTemplatesVersion = myTenantReportsTemplatesVersion.Where(d => d.ReportId == currentTenantReport.Id && d.TemplateId == currentTenantReportsTemplate.Id && d.Version == currentTenantReportsTemplate.CurrentVersion).FirstOrDefault();
			if (tenantZeroReportsTemplatesVersion == null || currentReportsTemplatesVersion == null) return copyReportTemplateArgs.isChangeReport;
			if (tenantZeroReportsTemplatesVersion.UpdateDate <= currentReportsTemplatesVersion.UpdateDate) return copyReportTemplateArgs.isChangeReport;

			string documentId = AddDocument(documentRepository, tenantZeroReportsTemplatesVersion.ReportDocumentId, tenantZeroReportsTemplatesVersion.Tenant, copyReportTemplateArgs.tenant, documentLists);
			ReportsTemplatesVersion reportsTemplatesVersion = GetNewInstanceReportsTemplatesVersion(copyReportTemplateArgs, currentTenantReportsTemplate, documentId);

			reportsTemplatesVersionRepository.Add(reportsTemplatesVersion);
			currentTenantReportsTemplate.IsSystem = true;
			currentTenantReportsTemplate.CurrentVersion = reportsTemplatesVersion.Version;
			currentTenantReportsTemplate.UpdateDate = TenantServerConfigration.GetCurrentDateTime(copyReportTemplateArgs.tenant);
			currentTenantReportsTemplate.UpdatedByUserId = copyReportTemplateArgs.userId;

			return true;
		}

		private static ReportsTemplatesVersion GetNewInstanceReportsTemplatesVersion(CopyReportTemplateArgs copyReportTemplateArgs, ReportsTemplate currentTenantReportsTemplate, string documentId)
		{
			return new ReportsTemplatesVersion()
			{
				Id = IdCounter.GetNumber("ReportsTemplatesVersion", copyReportTemplateArgs.tenant).ToString(),
				Tenant = copyReportTemplateArgs.tenant,
				CreateDate = TenantServerConfigration.GetCurrentDateTime(copyReportTemplateArgs.tenant),
				UpdateDate = TenantServerConfigration.GetCurrentDateTime(copyReportTemplateArgs.tenant),
				ReportId = currentTenantReportsTemplate.ReportId,
				CreatedByUserId = copyReportTemplateArgs.userId,
				UpdatedByUserId = copyReportTemplateArgs.userId,
				TemplateId = currentTenantReportsTemplate.Id,
				Version = currentTenantReportsTemplate.CurrentVersion + 1,
				ReportDocumentId = !string.IsNullOrEmpty(documentId) ? documentId : null,
			};
		}

		private bool CreateNewReportsTemplate(CopyReportTemplateArgs copyReportTemplateArgs, Report currentTenantReport)
		{
			ReportsTemplatesVersion tenantZeroReportsTemplatesVersion = tenantZeroReportsTemplatesVersionLists.Where(d => d.ReportId == copyReportTemplateArgs.report.Id && d.TemplateId == copyReportTemplateArgs.systemReportTemplate.Id).FirstOrDefault();
			if (tenantZeroReportsTemplatesVersion == null || (tenantZeroReportsTemplatesVersion != null && string.IsNullOrEmpty(tenantZeroReportsTemplatesVersion.ReportDocumentId))) return copyReportTemplateArgs.isChangeReport;

			string documentId = AddDocument(documentRepository, tenantZeroReportsTemplatesVersion.ReportDocumentId, copyReportTemplateArgs.report.Tenant, copyReportTemplateArgs.tenant, documentLists);
			if (string.IsNullOrEmpty(documentId)) return copyReportTemplateArgs.isChangeReport;

            string reportTemplateId = AddReportTemplate(currentTenantReport.Id, copyReportTemplateArgs.systemReportTemplate.Description, copyReportTemplateArgs.userId, documentId, copyReportTemplateArgs.tenant, reportsTemplateRepository, reportsTemplatesVersionRepository, null, true, copyReportTemplateArgs.systemReportTemplate.TemplateType, null, null, null, copyReportTemplateArgs.systemReportTemplate.Id);
			SetReportDefaultTemplates(copyReportTemplateArgs, currentTenantReport, reportTemplateId);
			return true;
		}

		private static void SetReportDefaultTemplates(CopyReportTemplateArgs copyReportTemplateArgs, Report currentTenantReport, string reportTemplateId)
		{
			if (copyReportTemplateArgs.systemReportTemplate.TemplateType == "R")
			{
				currentTenantReport.DefaultTemplateId = reportTemplateId;
			}
			else if (copyReportTemplateArgs.systemReportTemplate.TemplateType == "M")
			{
				currentTenantReport.DefaultMessageTemplateId = reportTemplateId;
			}
		}

		private ReportsTemplate GetCurrentTenantReportsTemplate(Report currentTenantReport, string systemReportTemplateType)
		{
			if(systemReportTemplateType == "M")
			{
				return myTenantReportsTemplate.Where(d => d.ReportId == currentTenantReport.Id && d.Id == currentTenantReport.DefaultMessageTemplateId).FirstOrDefault();
			}

			ReportsTemplate currentTenantReportsTemplate = myTenantReportsTemplate.Where(d => d.ReportId == currentTenantReport.Id && d.IsSystem).FirstOrDefault();
			if (currentTenantReportsTemplate == null)
			{
				currentTenantReportsTemplate = myTenantReportsTemplate.Where(d => d.ReportId == currentTenantReport.Id && d.Id == currentTenantReport.DefaultTemplateId).FirstOrDefault();
			}

			return currentTenantReportsTemplate;
		}

		private bool UpdateExcelReports(int tenant, string userId, Report reportTenantZero,List<Report> myReports)
		{
			ReportsTemplate systemExcelReportTemplate = tenantZeroReportsTemplate.Where(d => d.ReportId == reportTenantZero.Id && d.Id == reportTenantZero.DefaultExcelTemplateId).FirstOrDefault();
			if (systemExcelReportTemplate == null)
				return false;

			Report myReport = myReports.Where(d => d.Code == reportTenantZero.Code && d.Tenant == tenant).FirstOrDefault();
			if (myReport == null)
				return false;

			ReportsTemplate myReportsTemplate = myTenantReportsTemplate.Where(d => d.ReportId == myReport.Id && d.IsSystem).FirstOrDefault();
			if (myReportsTemplate == null)
			{
				myReportsTemplate = myTenantReportsTemplate.Where(d => d.ReportId == myReport.Id && d.Id == myReport.DefaultExcelTemplateId).FirstOrDefault();
			}

			// if template does not exist add it
			if (myReportsTemplate == null)
			{
				return AddExcelDocument(tenant, userId, reportTenantZero, systemExcelReportTemplate, myReport);
			}

			return AddExcelDocumentVersion(tenant, userId, reportTenantZero, systemExcelReportTemplate, myReport, myReportsTemplate);
		}

		private bool AddExcelDocumentVersion(int tenant, string userId, Report reportTenantZero, ReportsTemplate systemExcelReportTemplate, Report myReport, ReportsTemplate myReportsTemplate)
		{
			ReportsTemplatesVersion tenantZeroReportsTemplatesVersion = tenantZeroReportsTemplatesVersionLists.Where(d => d.ReportId == reportTenantZero.Id && d.TemplateId == systemExcelReportTemplate.Id).FirstOrDefault();
			ReportsTemplatesVersion myReportsTemplatesVersion = myTenantReportsTemplatesVersion.Where(d => d.ReportId == myReport.Id && d.TemplateId == myReportsTemplate.Id && d.Version == myReportsTemplate.CurrentVersion).FirstOrDefault();

			if (tenantZeroReportsTemplatesVersion == null || myReportsTemplatesVersion == null)
				return false;

			if (tenantZeroReportsTemplatesVersion.UpdateDate <= myReportsTemplatesVersion.UpdateDate)
				return false;

			string documentId = AddDocument(documentRepository, tenantZeroReportsTemplatesVersion.ReportDocumentId, tenantZeroReportsTemplatesVersion.Tenant, tenant, documentLists);
			ReportsTemplatesVersion reportsTemplatesVersion = GetReportsTemplatesVersion(tenant, userId, myReportsTemplate, documentId);

			reportsTemplatesVersionRepository.Add(reportsTemplatesVersion);
			myReportsTemplate.CurrentVersion = reportsTemplatesVersion.Version;
			myReportsTemplate.UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant);
			myReportsTemplate.UpdatedByUserId = userId;

			return true;
		}

		private ReportsTemplatesVersion GetReportsTemplatesVersion(int tenant, string userId, ReportsTemplate myReportsTemplate, string documentId)
		{
			return new ReportsTemplatesVersion()
			{
				Id = IdCounter.GetNumber("ReportsTemplatesVersion", tenant).ToString(),
				Tenant = tenant,
				CreateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
				UpdateDate = TenantServerConfigration.GetCurrentDateTime(tenant),
				ReportId = myReportsTemplate.ReportId,
				CreatedByUserId = userId,
				UpdatedByUserId = userId,
				TemplateId = myReportsTemplate.Id,
				Version = myReportsTemplate.CurrentVersion + 1,
				ReportDocumentId = !string.IsNullOrEmpty(documentId) ? documentId : null,
			};
		}

		private bool AddExcelDocument(int tenant, string userId, Report reportTenantZero, ReportsTemplate systemExcelReportTemplate, Report myReport)
		{

			ReportsTemplatesVersion tenantZeroReportsTemplatesVersion = tenantZeroReportsTemplatesVersionLists.FirstOrDefault(d => d.ReportId == reportTenantZero.Id && d.TemplateId == systemExcelReportTemplate.Id);

			if (tenantZeroReportsTemplatesVersion == null || string.IsNullOrEmpty(tenantZeroReportsTemplatesVersion.ReportDocumentId))
				return false;

			string documentId = AddDocument(documentRepository, tenantZeroReportsTemplatesVersion.ReportDocumentId, reportTenantZero.Tenant, tenant, documentLists);
			if (string.IsNullOrEmpty(documentId))
				return false;

            myReport.DefaultExcelTemplateId = AddReportTemplate(myReport.Id, systemExcelReportTemplate.Description, userId, documentId, tenant, reportsTemplateRepository, reportsTemplatesVersionRepository, null, true, "E", null, null, null, systemExcelReportTemplate.Id , systemExcelReportTemplate.UseStimul);
			return true;

		}
		#endregion

		public ReportFliter BuildReportDataViewWorkerRole(ReportFliter reportFliter)
		{

			ReportExecutionLogRepository reportExecutionLogRepository = new ReportExecutionLogRepository(reportFliter.tenant);
			reportFliter.ReportKey = Guid.NewGuid().ToString();

			ReportExecutionLog reportExecutionLog = new ReportExecutionLog()
			{
				Id = reportFliter.ReportKey,
				CreateDate = DateTime.Now,
				CreatedByUserId = reportFliter.UserId,
				ReportFilterXML = LogitudeXmlSerializer.SerializeObjectToXmlString(reportFliter),
				Tenant = reportFliter.tenant,
				StatusCode = "W",
				ReportId = reportFliter.ReportId,
                ReportTemplateId = reportFliter.ProcessType == "ExportToExcel"
					? (!string.IsNullOrEmpty(reportFliter.DefaultExcelNoStimId)
						? reportFliter.DefaultExcelNoStimId
						: null)
					: (!string.IsNullOrWhiteSpace(reportFliter.DefaultTemplateId)
						? reportFliter.DefaultTemplateId
						: null),
				DisablePreview = reportFliter.DisablePreview,
                NotDisplayInMenu = reportFliter.NotDisplayInMenu ,
			};

			reportExecutionLogRepository.Add(reportExecutionLog);
			reportExecutionLogRepository.SubmitChanges();

			IQueueService queueservice = new DbQueueService();
			string reportExecutionLogQueueCode = FeatureToggleHelper.HasFeatureToggle("RE2", reportExecutionLog.Tenant) ? "ReportExecutionLogV2Queue" : "ReportExecutionLogQueue";
			queueservice.InitializeQueue(reportExecutionLogQueueCode, reportExecutionLog.Tenant);
			queueservice.Send(new Dictionary<string, string>() { { "ReportExecutionLogId", reportExecutionLog.Id }, { "Tenant", reportExecutionLog.Tenant.ToString() } }, reportExecutionLog.Tenant, null, null, null, null);
			return reportFliter;

		}

		public static void AddStimulsoftLicenseKey()
		{
			StiLicense.Key = "6vJhGtLLLz2GNviWmUTrhSqnOItdDwjBylQzQcAOiHk5LQfMb0Dr1Ze4z6YRXSb7imTiay6/HzKYGUzkd/h3FMt5R7" +
"uunoM5lX8Vs2voVkSeT6Wv6WI6Jcy4xOeAjjPkTBhC+ivrrxidMQjLaebItqFcnJWqKXBUgoJa0WfmH3soi0IbfEmI" +
"fQ3ZmMq5BHsjsKoHSdnbzDUPWMXieYRTJZL6tsBC6QRy2ALPnYwg88ZJDGAWgAqMhZ+M0BVM17B3YJN9mu1MfAblN7" +
"rG1eWrSrR5B53af4aeWs0RmqVNatfenGL8sufvTgOiyEuQmC9J7sHOT6VoQpWOlZthrc7JOl4zbw+qduZHZrpLuK+1" +
"O3AB8EeDCQ6EgM8TcUesQBZZrUA4ZUFpxsCdvL0n4DQiB1tIof1TGHXCtZ62S1kAfU4XJzEGM/g3MYbKridAK5ckyc" +
"0xwsK2y46rm9W3EV0m49Na0pcJe+2ZScc6BP1o3tDS9ddHbfkt7hFZpUNTqOxn9BOP0YVoQul+dPckYle4PS4mzXVp" +
"tMrKV4En69rnW/z658axW0kQ2GxorKwW0IAR";
		}
	}


	public class BuildReportDataResult
	{
		public CustomerPotentialActualDataProvider CustomerPotentialActualDataProvider { get; set; }
		public string UrlImage { get; set; }
	}

	public class ReportStimulDataProviderDetails
	{
		public StiBusinessObject CurrentBusinessObject { get; set; }
		public byte[] Logo { get; set; }
		public int Tenant { get; set; }
	}

	public class CopyReportTemplateArgs
	{
		public int tenant { get; set; }
		public string userId { get; set; }
		public List<Report> myReports { get; set; }
		public bool isChangeReport { get; set; }
		public Report report { get; set; }
		public ReportsTemplate systemReportTemplate { get; set; }
	}
}