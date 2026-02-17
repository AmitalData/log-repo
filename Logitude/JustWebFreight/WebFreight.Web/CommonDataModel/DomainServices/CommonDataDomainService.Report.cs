using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceModel.DomainServices.Server;
using System.Web;
using System.Xml.Serialization;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Data.InfrastructureModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using WebFreight.Web.DataContracts;
using WebFreight.Web.Helpers;
using WebFreight.Web.Security;
using Simplog.Server.Infrastructure.DataContracts;
using Logitude.Server.Tools.Counters;
using Logitude.BL.CommonDataModel.EntityLists;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityQueries;
using Logitude.BL.CommonDataModel.Tools.EntityService;
using Logitude.BL.Helpers;
using Logitude.Server.Tools;
using Logitude.Server.Tools.StorageService;
using Microsoft.Practices.Unity;

namespace WebFreight.Web.CommonDataModel.DomainServices
{
    public partial class CommonDataDomainService
    {
      
        public void UpdateReportList(ReportList currentEntity)
        {
        }

        public IQueryable<Report> GetReports(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Report", "READ", tenant);

            reportRepository = new ReportRepository(tenant);
            return reportRepository.GetReports(0);
        }

        public IQueryable<ReportPM> GetReportsByTenant(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Report", "READ", tenant);

            reportQuery = new ReportQuery(tenant);
            IQueryable<ReportPM> reports = reportQuery.GetReportPMsByTenant(tenant);
            return reports;
        }

        public ReportPM GetSingleReport(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Report", "READ", tenant);

            commonContext = CommonDataContext.GetContext(tenant);
            reportRepository = new ReportRepository(commonContext);
            reportQuery = new ReportQuery(reportRepository);
            return reportQuery.GetSinglePM(id, tenant);
        }

        public ReportPM GetReportById(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Report", "READ", tenant);
   
            reportQuery = new ReportQuery(tenant);
            return reportQuery.GetSinglePM(id, tenant);        
        }

        public ReportList GetSingleReportList(string id, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Report", "READ", tenant);

            reportRepository = new ReportRepository(tenant);
            reportQuery = new ReportQuery(reportRepository);
            ReportList reportList = null;
            Report report = reportRepository.GetSingleReport(id, tenant);

            if (report != null)
            {
                List<Report> singleEntityList = new List<Report>();
                singleEntityList.Add(report);

                IQueryable<Report> iQueryable = singleEntityList.AsQueryable();
                IQueryable<ReportList> iQueryableEntityList = reportQuery.GetIQueryableEntityList(iQueryable);
                reportList = iQueryableEntityList.FirstOrDefault();
            }
            return reportList;
        }

        public List<ReportList> GetReportLists(int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Report", "READ", tenant);

            reportRepository = new ReportRepository(tenant);
            reportQuery = new ReportQuery(reportRepository);
            List<ReportList> query2 = reportQuery.GetReportsWithModifications(tenant);
            return query2;
        }

        public List<ReportList> GetReportListsByGroupId(string groupId, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Report", "READ", tenant);

            reportRepository = new ReportRepository(tenant);
            reportQuery = new ReportQuery(reportRepository);
            List<ReportList> query2 = reportQuery.GetReportListsByGroupId(groupId ,tenant);
            List<ReportList> result = new List<ReportList>();
            foreach (ReportList report in query2)
            {
     
                if (CheckIfReportExistInMyTenantOrTenant0(report.ReportDocumentId, tenant))
                {
                    result.Add(report);
                }
            }
           
            return result;
        }
        private bool CheckIfReportExistInMyTenantOrTenant0(string reportId, int tenant)
        {
            byte[] theDatainByte = null;
            bool isExist = false;

            try
            {
                BlobFileInfo fileInfo = new BlobFileInfo()
                {
                    FileName = reportId,
                    FolderName = "reports",
                    Extension = "mrt",
                    Tenant = tenant,

                };
                IBlobService storageservice = ContainerAccessor.Container.Resolve(typeof(IBlobService), "StorageService", new ParameterOverride("", 1)) as IBlobService;
                theDatainByte = storageservice.Read(fileInfo);


                if (theDatainByte == null)
                {
                    fileInfo.Tenant = 0;
                    theDatainByte = storageservice.Read(fileInfo);

                }

                if (theDatainByte != null)
                {
                    isExist = true;
                }


                return isExist;//Request.CreateResponse(HttpStatusCode.OK, ""); 
            }
            catch (Exception e)
            {
                return false;//Request.CreateResponse(HttpStatusCode.BadRequest, ApiExceptionBuilder.BuildException(ex));
            }


        }
        public IQueryable<ReportList> GetReportFilters(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Report", "READ", tenant);

            reportRepository = new ReportRepository(tenant);
            reportQuery = new ReportQuery(reportRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<Report> reports = reportRepository.GetReports(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();

            reports = filter.GetFilteredQuery<Report>(nonListQueryOperation, reports);
            int skippedPorts = queryOperations.PageIndex;

            IQueryable<ReportList> query2 = reportQuery.GetIQueryableEntityList(reports);

            query2 = filter.GetFilteredQuery<ReportList>(listQueryOperation, query2);

            if (!string.IsNullOrEmpty(queryOperations.SortByColumnName) && !string.IsNullOrEmpty(queryOperations.SortDirectin))
            {
                PropertyInfo propInfo = typeof(ReportList).GetProperty(queryOperations.SortByColumnName);
                List<ObjectField> reportObjectFields = ObjectFieldRepository.GetObjectFieldsByObjectTableName("Report", tenant).ToList();

                ObjectField objectField = (from a in reportObjectFields
                                           where a.FieldName == queryOperations.SortByColumnName
                                           select a).FirstOrDefault();

                if (objectField != null)
                {
                    switch (objectField.DataTypeCode.ToLower())
                    {
                        case "text":
                            {
                                query2 = sortClass.GetSorterQuery<ReportList, string>(queryOperations, query2);
                                break;
                            }
                        case "double":
                            {
                                query2 = sortClass.GetSorterQuery<ReportList, double>(queryOperations, query2);
                                break;
                            }
                        case "datetime":
                            {
                                query2 = sortClass.GetSorterQuery<ReportList, DateTime>(queryOperations, query2);
                                break;
                            }
                        case "integer":
                            {
                                query2 = sortClass.GetSorterQuery<ReportList, int>(queryOperations, query2);
                                break;
                            }
                        case "boolean":
                            {
                                query2 = sortClass.GetSorterQuery<ReportList, bool>(queryOperations, query2);
                                break;
                            }
                        default:
                            {
                                query2 = query2.OrderByDescending(d => d.Name);
                                break;
                            }
                    }
                }
            }
            else
            {
                query2 = query2.OrderByDescending(d => d.Name);
            }
            //--------------------------------------------------------------------------------------------------

            query2 = query2.Skip(skippedPorts);
            query2 = query2.Take(queryOperations.PageSize);
            return query2;
        }

        public int GetReportFiltersCount(byte[] xmlFilters, int tenant)
        {
            SecurityUtility.AuthenticationOnTenant(tenant);
            SecurityUtility.CheckContactFeature("Report", "READ", tenant);

            reportRepository = new ReportRepository(tenant);
            reportQuery = new ReportQuery(reportRepository);
            MemoryStream memorystream = new MemoryStream(xmlFilters);
            XmlSerializer serializer = new XmlSerializer(typeof(QueryOperations));
            QueryOperations queryOperations = (QueryOperations)serializer.Deserialize(memorystream);
            GenericFilter filter = new GenericFilter();
            GenericSort sortClass = new GenericSort();
            IQueryable<Report> reports = reportRepository.GetReports(tenant);

            QueryOperations nonListQueryOperation = new QueryOperations();
            nonListQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == false).ToList();
            QueryOperations listQueryOperation = new QueryOperations();
            listQueryOperation.QueryFilterItems = queryOperations.QueryFilterItems.Where(d => d.DisplayInList == true).ToList();
            reports = filter.GetFilteredQuery<Report>(nonListQueryOperation, reports);

            IQueryable<ReportList> query2 = reportQuery.GetIQueryableEntityList(reports);

            query2 = filter.GetFilteredQuery<ReportList>(listQueryOperation, query2);
            int count = query2.Count();
            return count;
        }

        public void InsertReport(ReportPM report)
        {
            SecurityUtility.CheckContactFeature("Report", "NEW", report.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(report.Tenant);
            }

            ReportService service = new ReportService(objectContext, report.Tenant);
            service.Create(report);
            
            TableLastUpdateClass.UpdateTableHistory(report.Tenant, "Report");
        }

        public void UpdateReport(ReportPM currentreport)
        {
            SecurityUtility.CheckContactFeature("Report", "UPDATE", currentreport.Tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(currentreport.Tenant);
            }


            reportRepository = new ReportRepository(objectContext);
          
            ReportService service = new ReportService(objectContext, currentreport.Tenant);
            service.Update(currentreport);
            TableLastUpdateClass.UpdateTableHistory(currentreport.Tenant, "Report");
        }

        public void DeleteReport(ReportPM report)
        {           
            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(report.Tenant);
            }
            reportRepository = new ReportRepository(objectContext);
            Report entity = reportRepository.GetSingleReport(report.Id, report.Tenant);
            reportRepository.Remove(entity);
        }

        [Invoke]
        public string CreateDocumentForReport(string reportId,int tenant,long fileSize)
        {
            SecurityUtility.CheckContactFeature("Report", "UPDATE", tenant);

            if (objectContext == null)
            {
                objectContext = CommonDataContext.GetContext(tenant);
            }

            ReportService reportService = new ReportService(objectContext, tenant);
            return reportService.CreateDocumentForReport(reportId, tenant, fileSize);          
        }

        public bool DoesReportCodeExist(string code, int tenant)
        {
            reportRepository = new ReportRepository(tenant);
            return (reportRepository.GetReports(tenant).Where(d => d.Code == code && d.Tenant == tenant)).Any();
        }
    }
}