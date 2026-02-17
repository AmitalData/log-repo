using System;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.Tools.DataMapping;
using Logitude.BL.CommonDataModel.Tools.TraceEvents;
using Logitude.BL.CommonDataModel.Tools.Validating;
using Logitude.BL.Helpers;
using Logitude.Server.Tools.Counters;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Logitude.Server.Tools.Helpers;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.Security;

namespace Logitude.BL.CommonDataModel.Tools.EntityService
{
    public class ReportService
    {
        bool isNewEntity;
        private int tenant;
        public Report Poco { get; set; }

        public ICommonDataContext ObjectContext
        {
            get { return objectContext; }
            set { objectContext = value; }
        }

        private ReportPM entityPm;
        private ICommonDataContext objectContext;
        private ReportRepository entityRepository;
        public ReportService(ICommonDataContext objectContext, int tenant)
        {
            this.tenant = tenant;
            this.ObjectContext = objectContext;
            this.entityRepository = new ReportRepository(objectContext);
        }

        public void Create(ReportPM entityPM)
        {
            this.isNewEntity = true;
            this.entityPm = entityPM;
            this.entityPm.Id = IdCounter.GetNumber("Report", tenant).ToString();
            this.Poco = new Report();
            this.Poco.Id = this.entityPm.Id;

            ReportValidating.Validate(entityPM, this.ObjectContext, this.isNewEntity);
            ReportTracing.Trace(entityPM, Poco, isNewEntity);
            ReportMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Add(Poco);
            entityRepository.SubmitChanges();
        }

        public void Update(ReportPM entityPM)
        {
            this.isNewEntity = false;
            this.entityPm = entityPM;
            this.Poco = entityRepository.GetSingleReport(entityPM.Id, entityPm.Tenant);
            
            ReportValidating.Validate(entityPM, this.ObjectContext, this.isNewEntity);
            ReportTracing.Trace(entityPM, Poco, isNewEntity);
            ReportMapping.MapEntity(entityPM, Poco, isNewEntity);
            entityRepository.Update(Poco);
            entityRepository.SubmitChanges();
        }

        public string CreateDocumentForReport(string reportId, int tenant, long fileSize)
        {            
            ReportRepository reportRepository = new ReportRepository(ObjectContext);
            Report entity = reportRepository.GetSingleReport(reportId);
            DocumentRepository documentRepository = new DocumentRepository(ObjectContext);

            int size = Convert.ToInt32(fileSize);
            Document document = new Document()
            {
                CreateDate = DateTime.Now,
                Extension = "mrt",
                FileSize = size,
                Tenant = Convert.ToInt32(tenant),
                Id = IdCounter.GetNumber("Document", tenant),
                HasFile = true,
                Folder="reports",
            };

            documentRepository.Add(document);
            documentRepository.SubmitChanges();

            if (tenant == 0 || entity.Tenant == tenant)
            {
                entity.ReportDocumentId = document.Id;
                reportRepository.Update(entity);
                reportRepository.SubmitChanges();
            }
            else if(entity.Tenant == 0)
            {
                ReportModificationRepository reportModificationRepository = new ReportModificationRepository(ObjectContext);
                ReportModification modification = reportModificationRepository.GetSingleReportModification(reportId, tenant);
                if (modification != null)
                {
                    modification.ReportDocumentId = document.Id;
                    reportModificationRepository.Update(modification);
                }
                else
                {
                    modification = new ReportModification()
                    {
                        ReportId = reportId,
                        Tenant = tenant,
                        ReportDocumentId = document.Id,
                    };
                    reportModificationRepository.Add(modification);
                }
                reportModificationRepository.SubmitChanges();
            }

            if (!string.IsNullOrEmpty(entity.ReportDocumentId))
            {
                ContactPM loggedContact = new ContactQuery(tenant).GetContactByEmailOnly(SecurityUtility.GetAuthenticatedUser(), tenant);
                EventTracer.CreateTraceEvent(new EventTracerArgs()
                {
                    Tenant = tenant,
                    EventTypeCode = "TEPU",
                    UserId = loggedContact.Id,
                    EntityId = reportId,
                    ObjectTableName = "Report",
                });
            }

            return document.Id;
        }
    }
}
