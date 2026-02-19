


using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.ShipmentsModel.EntityQueries;
using Logitude.BL.ShipmentsModel.EntityPMs;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class ReportsTemplatesVersionQuery
    {
        ReportsTemplatesVersionRepository repository;

        public ReportsTemplatesVersionQuery()
        {
            repository = new ReportsTemplatesVersionRepository();
        }

        public ReportsTemplatesVersionQuery(int tenant)
        {
            repository = new ReportsTemplatesVersionRepository(tenant);
        }

        public ReportsTemplatesVersionQuery(ReportsTemplatesVersionRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<ReportsTemplatesVersionList> GetIQueryableEntityList(IQueryable<ReportsTemplatesVersion> iQueryable)
        {
            IQueryable<ReportsTemplatesVersionList> result = from a in iQueryable
                                                     select new ReportsTemplatesVersionList()
                                                     {
                                                         Tenant = a.Tenant,
                                                         Id = a.Id,
                                                         ReportDocumentId = a.ReportDocumentId,
                                                         TemplateId = a.TemplateId,
                                                         CreateDate = a.CreateDate,
                                                         UpdateDate = a.UpdateDate,
                                                         UpdatedByUserId = a.UpdatedByUserId,
                                                         CreatedByUserId = a.CreatedByUserId,
                                                         Version = a.Version,
                                                         ReportId = a.ReportId,
                                                         IsRestored = a.IsRestored,
                                                     };


            return result;
        }

        public ReportsTemplatesVersionPM GetSinglePM(string id, int tenant)
        {
            ReportsTemplatesVersionPM entity = (from a in repository.context.ReportsTemplatesVersions
                                        where a.Tenant == tenant
                                        && a.Id == id
                                        select new ReportsTemplatesVersionPM()
                                        {
                                            Tenant = a.Tenant,
                                            Id = a.Id,
                                            ReportDocumentId = a.ReportDocumentId,
                                            TemplateId = a.TemplateId,
                                            CreateDate = a.CreateDate,
                                            UpdateDate = a.UpdateDate,
                                            UpdatedByUserId = a.UpdatedByUserId,
                                            CreatedByUserId = a.CreatedByUserId,
                                            Version = a.Version,
                                            ReportId = a.ReportId,
                                            IsRestored = a.IsRestored,
                                        }).FirstOrDefault();
            return entity;
        }

        public IQueryable<ReportsTemplatesVersionPM> GetReportsTemplatesVersionPMsByTenant(int tenant)
        {
            IQueryable<ReportsTemplatesVersionPM> ReportsTemplatesVersionPMs = from a in repository.context.ReportsTemplatesVersions
                                                               where a.Tenant == tenant
                                                               select new ReportsTemplatesVersionPM()
                                                               {
                                                                   Tenant = a.Tenant,
                                                                   Id = a.Id,
                                                                   ReportDocumentId = a.ReportDocumentId,
                                                                   TemplateId = a.TemplateId,
                                                                   CreateDate = a.CreateDate,
                                                                   UpdateDate = a.UpdateDate,
                                                                   UpdatedByUserId = a.UpdatedByUserId,
                                                                   CreatedByUserId = a.CreatedByUserId,
                                                                   Version = a.Version,
                                                                   ReportId = a.ReportId,
                                                                   IsRestored = a.IsRestored,
                                                               };
            return ReportsTemplatesVersionPMs;
        }

        public IQueryable<ReportsTemplatesVersionList> GetReportsTemplatesVersionListsByTenant(int tenant)
        {
            IQueryable<ReportsTemplatesVersionList> ReportsTemplatesVersionLists = from a in repository.context.ReportsTemplatesVersions
                                                                   where a.Tenant == tenant
                                                                   select new ReportsTemplatesVersionList()
                                                                   {
                                                                       Tenant = a.Tenant,
                                                                       Id = a.Id,
                                                                       ReportDocumentId = a.ReportDocumentId,
                                                                       TemplateId = a.TemplateId,
                                                                       CreateDate = a.CreateDate,
                                                                       UpdateDate = a.UpdateDate,
                                                                       UpdatedByUserId = a.UpdatedByUserId,
                                                                       CreatedByUserId = a.CreatedByUserId,
                                                                       Version = a.Version,
                                                                       ReportId = a.ReportId,
                                                                       IsRestored = a.IsRestored,

                                                                   };
            return ReportsTemplatesVersionLists;
        }

        public ReportsTemplatesVersionPM GetLastReportsTemplatesVersionPMByReportsTemplateId(string reportsTemplateId, int tenant)
        {
             ReportsTemplatesVersionPM entity = (from a in repository.context.ReportsTemplatesVersions
                                                where a.Tenant == tenant
                                                && a.TemplateId == reportsTemplateId
                                                 select new ReportsTemplatesVersionPM()
                                                {
                                                    Tenant = a.Tenant,
                                                    Id = a.Id,
                                                    ReportDocumentId = a.ReportDocumentId,
                                                    TemplateId = a.TemplateId,
                                                    CreateDate = a.CreateDate,
                                                    UpdateDate = a.UpdateDate,
                                                    UpdatedByUserId = a.UpdatedByUserId,
                                                    CreatedByUserId = a.CreatedByUserId,
                                                    Version = a.Version,
                                                    ReportId = a.ReportId,
                                                    IsRestored = a.IsRestored,
                                                 }).OrderByDescending(d => d.Version).FirstOrDefault();
            return entity;

           
        }

        public List<ReportsTemplatesVersionList> GetReportsTemplatesVersionListsByTenant(string reportsTemplateId , int tenant)
        {
            List<ReportsTemplatesVersionList> reportsTemplatesVersionLists = (from a in repository.context.ReportsTemplatesVersions
                                                                                   where a.Tenant == tenant && a.TemplateId == reportsTemplateId
                                                                                   select new ReportsTemplatesVersionList()
                                                                                   {
                                                                                       Tenant = a.Tenant,
                                                                                       Id = a.Id,
                                                                                       ReportDocumentId = a.ReportDocumentId,
                                                                                       TemplateId = a.TemplateId,
                                                                                       CreateDate = a.CreateDate,
                                                                                       UpdateDate = a.UpdateDate,
                                                                                       UpdatedByUserId = a.UpdatedByUserId,
                                                                                       CreatedByUserId = a.CreatedByUserId,
                                                                                       Version = a.Version,
                                                                                       ReportId = a.ReportId,
                                                                                       IsRestored = a.IsRestored,

                                                                                   }).ToList();


            #region Update By User Name

            List<string> contactIds = new List<string>();
            foreach (ReportsTemplatesVersionList item in reportsTemplatesVersionLists)
            {
                if (!string.IsNullOrEmpty(item.UpdatedByUserId))
                {
                    if (!contactIds.Contains(item.UpdatedByUserId)) contactIds.Add(item.UpdatedByUserId);
                }
            }

            List<ContactList> contactLists = null;
            if (contactIds.Count > 0)
            {
                ContactQuery contactQuery = new ContactQuery(tenant);
                contactLists = contactQuery.GetContactListsByListIds(contactIds, tenant).ToList();
            }

            if (contactLists != null && contactLists.Count > 0)
            {
                foreach (ReportsTemplatesVersionList item in reportsTemplatesVersionLists)
                {
                    if (!string.IsNullOrEmpty(item.UpdatedByUserId))
                    {
                        ContactList contactList = contactLists.Where(d => d.Id == item.UpdatedByUserId).FirstOrDefault();
                        if (contactList != null) item.UpdateByUserName = contactList.EnglishName; 

                    }
                }
            }
            #endregion
            return reportsTemplatesVersionLists;
        }


    }
}