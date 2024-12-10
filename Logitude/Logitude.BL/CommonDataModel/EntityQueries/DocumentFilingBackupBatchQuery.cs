using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.Helpers;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class DocumentFilingBackupBatchQuery
    {
        DocumentFilingBackupBatchRepository repository;

        public DocumentFilingBackupBatchQuery()
        {
            repository = new DocumentFilingBackupBatchRepository();
        }

        public DocumentFilingBackupBatchQuery(int tenant)
        {
            repository = new DocumentFilingBackupBatchRepository(tenant);
        }

        public DocumentFilingBackupBatchQuery(DocumentFilingBackupBatchRepository DocumentFilingBackupBatchRepository)
        {
            repository = DocumentFilingBackupBatchRepository;
        }

        public IQueryable<DocumentFilingBackupBatchList> GetIQueryableEntityList(IQueryable<DocumentFilingBackupBatch> iQueryable)
        {
            IQueryable<DocumentFilingBackupBatchList> entity = from a in iQueryable
                                                                    select new DocumentFilingBackupBatchList()
                                                                    {
                                                                        Id = a.Id,
                                                                        IncludeBackedUp = a.IncludeBackedUp,
                                                                        Tenant = a.Tenant,
                                                                        CreateDateTime = a.CreateDateTime,
                                                                         DoneDate= a.DoneDate,
                                                                        FromDatetime = a.FromDatetime,
                                                                        BatchNumber = a.BatchNumber,
                                                                        ToDatetime = a.ToDatetime,
                                                                        Status = a.Status,
                                                                        TotalFailed = a.TotalFailed,
                                                                        TotalDocuments = a.TotalDocuments,
                                                                        TotalSucceeded = a.TotalSucceeded

                                                                    };

            return entity;

        }

        public DocumentFilingBackupBatchPM GetSinglePM(string id,  int tenant)
        {
            DocumentFilingBackupBatchPM entity = (from a in repository.context.DocumentFilingBackupBatches
                                                       where a.Id == id &&  a.Tenant == tenant
                                                       select new DocumentFilingBackupBatchPM()
                                                 {
                                                           Id = a.Id,
                                                           IncludeBackedUp = a.IncludeBackedUp,
                                                           Tenant = a.Tenant,
                                                           CreateDateTime = a.CreateDateTime,
                                                           DoneDate = a.DoneDate,
                                                           FromDatetime = a.FromDatetime,
                                                           BatchNumber = a.BatchNumber,
                                                           ToDatetime = a.ToDatetime,
                                                           Status = a.Status,
                                                           TotalFailed = a.TotalFailed,
                                                           TotalDocuments = a.TotalDocuments,
                                                           TotalSucceeded = a.TotalSucceeded

                                                       }).FirstOrDefault();


            return entity;


        }



        public IQueryable<DocumentFilingBackupBatchPM> GetDocumentFilingBackupBatchPMs(int tenant)
        {
            var query = from a in repository.context.DocumentFilingBackupBatches
                        where a.Tenant == tenant
                        select new DocumentFilingBackupBatchPM()
                        {
                            Id = a.Id,
                            IncludeBackedUp = a.IncludeBackedUp,
                            Tenant = a.Tenant,
                            CreateDateTime = a.CreateDateTime,
                            DoneDate = a.DoneDate,
                            FromDatetime = a.FromDatetime,
                            BatchNumber = a.BatchNumber,
                            ToDatetime = a.ToDatetime,
                            Status = a.Status,
                            TotalFailed = a.TotalFailed,
                            TotalDocuments = a.TotalDocuments,
                            TotalSucceeded = a.TotalSucceeded
                        };

            return query;
        }




        public IQueryable<DocumentFilingBackupBatchPM> GetDocumentFilingBackupBatchPMsByBatchNumber(string batchNumber, int tenant)
        {
            var query = from a in repository.context.DocumentFilingBackupBatches
                        where a.Tenant == tenant && a.BatchNumber == batchNumber 
                        select new DocumentFilingBackupBatchPM()
                        {
                            Id = a.Id,
                            IncludeBackedUp = a.IncludeBackedUp,
                            Tenant = a.Tenant,
                            CreateDateTime = a.CreateDateTime,
                            DoneDate = a.DoneDate,
                            FromDatetime = a.FromDatetime,
                            BatchNumber = a.BatchNumber,
                            ToDatetime = a.ToDatetime,
                            Status = a.Status,
                            TotalFailed = a.TotalFailed,
                            TotalDocuments = a.TotalDocuments,
                            TotalSucceeded = a.TotalSucceeded
                        };

                return query;
        }

        public DocumentFilingBackupBatchPM GetOldestDocumentFilingBackupBatch(string batchNumber, int tenant)
        {


            var query = (from a in repository.context.DocumentFilingBackupBatches
                        where a.Tenant == tenant && a.BatchNumber == batchNumber 
                        orderby a.CreateDateTime ascending
                        select new DocumentFilingBackupBatchPM()
                        {
                            Id = a.Id,
                            IncludeBackedUp = a.IncludeBackedUp,
                            Tenant = a.Tenant,
                            CreateDateTime = a.CreateDateTime,
                            DoneDate = a.DoneDate,
                            FromDatetime = a.FromDatetime,
                            BatchNumber = a.BatchNumber,
                            ToDatetime = a.ToDatetime,
                            Status = a.Status,
                            TotalFailed = a.TotalFailed,
                            TotalDocuments = a.TotalDocuments,
                            TotalSucceeded = a.TotalSucceeded
                        }).FirstOrDefault();

            return query;
        }

     

     
    }
}
