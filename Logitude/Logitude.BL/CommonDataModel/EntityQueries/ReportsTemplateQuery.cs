



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
    public class ReportsTemplateQuery
    {
        ReportsTemplateRepository repository;

        public ReportsTemplateQuery()
        {
            repository = new ReportsTemplateRepository();
        }

        public ReportsTemplateQuery(int tenant)
        {
            repository = new ReportsTemplateRepository(tenant);
        }

        public ReportsTemplateQuery(ReportsTemplateRepository repository)
        {
            this.repository = repository;
        }

        public IQueryable<ReportsTemplateList> GetIQueryableEntityList(IQueryable<ReportsTemplate> iQueryable)
        {
            IQueryable<ReportsTemplateList> result = from a in iQueryable
                                                         select new ReportsTemplateList()
                                                         {
                                                             Tenant = a.Tenant,
                                                             Id = a.Id,
                                                             Description = a.Description,
                                                             CurrentVersion = a.CurrentVersion,
                                                             CreateDate = a.CreateDate,
                                                             UpdateDate = a.UpdateDate,
                                                             UpdatedByUserId = a.UpdatedByUserId,
                                                             CreatedByUserId = a.CreatedByUserId,
                                                             InActive = a.InActive,
                                                             IsSystem = a.IsSystem,
                                                             ReportId = a.ReportId,
                                                             CC = a.CC,
                                                             From = a.From,
                                                             Subject = a.Subject,
                                                             ReplyTo = a.ReplyTo,
                                                             ObjectTableId = a.ObjectTableId,
                                                             EntityId = a.EntityId
                                   
                                                         };


            return result;
        }


        public ReportsTemplatePM GetSinglePM(string id, int tenant)
        {
            ReportsTemplatePM entity = (from a in repository.context.ReportsTemplates
                                            where a.Tenant == tenant
                                            && a.Id == id
                                            select new ReportsTemplatePM()
                                            {
                                                Tenant = a.Tenant,
                                                Id = a.Id,
                                                Description = a.Description,
                                                CurrentVersion = a.CurrentVersion,
                                                CreateDate = a.CreateDate,
                                                UpdateDate = a.UpdateDate,
                                                UpdatedByUserId = a.UpdatedByUserId,
                                                CreatedByUserId = a.CreatedByUserId,
                                                InActive = a.InActive,
                                                IsSystem = a.IsSystem,
                                                ReportId = a.ReportId,
                                                TemplateType = a.TemplateType,
                                                CC = a.CC,
                                                From = a.From,
                                                Subject = a.Subject,
                                                ReplyTo = a.ReplyTo,
                                                ObjectTableId = a.ObjectTableId,
                                                EntityId = a.EntityId
                                            }).FirstOrDefault();


            if (entity != null)
            {
                ContactQuery contactQuery = new ContactQuery(tenant);
                ContactList contactLists = contactQuery.GetContactListsById(entity.UpdatedByUserId, tenant);
                if (contactLists != null)
                {
                    entity.UpdateByUserName = contactLists.EnglishName;
                }
            }

            return entity;
        }

        public IQueryable<ReportsTemplatePM> GetReportsTemplatePMsByTenant(int tenant)
        {
            IQueryable<ReportsTemplatePM> ReportsTemplatePMs = from a in repository.context.ReportsTemplates
                                                                       where a.Tenant == tenant
                                                                       select new ReportsTemplatePM()
                                                                       {
                                                                           Tenant = a.Tenant,
                                                                           Id = a.Id,
                                                                           Description = a.Description,
                                                                           CurrentVersion = a.CurrentVersion,
                                                                           CreateDate = a.CreateDate,
                                                                           UpdateDate = a.UpdateDate,
                                                                           UpdatedByUserId = a.UpdatedByUserId,
                                                                           CreatedByUserId = a.CreatedByUserId,
                                                                           InActive = a.InActive,
                                                                           IsSystem = a.IsSystem,
                                                                           ReportId = a.ReportId,
                                                                           TemplateType = a.TemplateType,
                                                                           CC = a.CC,
                                                                           From = a.From,
                                                                           Subject = a.Subject,
                                                                           ReplyTo = a.ReplyTo,
                                                                           ObjectTableId = a.ObjectTableId,
                                                                           EntityId = a.EntityId
                                                                       };
            return ReportsTemplatePMs;
        }

        public IQueryable<ReportsTemplateList> GetReportsTemplateListsByTenant(int tenant)
        {
            IQueryable<ReportsTemplateList> ReportsTemplateLists = from a in repository.context.ReportsTemplates
                                                                           where a.Tenant == tenant
                                                                           select new ReportsTemplateList()
                                                                           {
                                                                               Tenant = a.Tenant,
                                                                               Id = a.Id,
                                                                               Description = a.Description,
                                                                               CurrentVersion = a.CurrentVersion,
                                                                               CreateDate = a.CreateDate,
                                                                               UpdateDate = a.UpdateDate,
                                                                               UpdatedByUserId = a.UpdatedByUserId,
                                                                               CreatedByUserId = a.CreatedByUserId,
                                                                               InActive = a.InActive,
                                                                               IsSystem = a.IsSystem,
                                                                               ReportId = a.ReportId,
                                                                               TemplateType = a.TemplateType,
                                                                               CC = a.CC,
                                                                               From = a.From,
                                                                               Subject = a.Subject,
                                                                               ReplyTo = a.ReplyTo,
                                                                               ObjectTableId = a.ObjectTableId,
                                                                               EntityId = a.EntityId
                                                                           };
            return ReportsTemplateLists;
        }


        public IQueryable<ReportsTemplateList> GetReportsTemplateListsByReportId(string reportId, int tenant , string templateType = null)
        {
            IQueryable<ReportsTemplateList> reportsTemplateLists = from a in repository.context.ReportsTemplates
                                                                   where a.Tenant == tenant && a.ReportId == reportId && !a.InActive
                                                                   select new ReportsTemplateList()
                                                                   {
                                                                       Tenant = a.Tenant,
                                                                       Id = a.Id,
                                                                       Description = a.Description,
                                                                       CurrentVersion = a.CurrentVersion,
                                                                       CreateDate = a.CreateDate,
                                                                       UpdateDate = a.UpdateDate,
                                                                       UpdatedByUserId = a.UpdatedByUserId,
                                                                       CreatedByUserId = a.CreatedByUserId,
                                                                       InActive = a.InActive,
                                                                       IsSystem = a.IsSystem,
                                                                       ReportId = a.ReportId,
                                                                       TemplateType = a.TemplateType,
                                                                       CC = a.CC,
                                                                       From = a.From,
                                                                       Subject = a.Subject,
                                                                       ReplyTo = a.ReplyTo,
                                                                       ObjectTableId = a.ObjectTableId,
                                                                       EntityId = a.EntityId

                                                                   };
            if (!string.IsNullOrEmpty(templateType))
            {
                reportsTemplateLists = reportsTemplateLists.Where(d => d.TemplateType == templateType);
            }
            return reportsTemplateLists;
        }

        public List<ReportsTemplatePM> GetReportsTemplatePMsByReportId(string reportId, int tenant, string templateType = null)
        {
            IQueryable<ReportsTemplatePM> reportsTemplateLists = from a in repository.context.ReportsTemplates
                                                                  where a.Tenant == tenant && a.ReportId == reportId 
                                                                  select new ReportsTemplatePM()
                                                                  {
                                                                      Tenant = a.Tenant,
                                                                      Id = a.Id,
                                                                      Description = a.Description,
                                                                      CurrentVersion = a.CurrentVersion,
                                                                      CreateDate = a.CreateDate,
                                                                      UpdateDate = a.UpdateDate,
                                                                      UpdatedByUserId = a.UpdatedByUserId,
                                                                      CreatedByUserId = a.CreatedByUserId,
                                                                      InActive = a.InActive,
                                                                      IsSystem = a.IsSystem,
                                                                      ReportId = a.ReportId,
                                                                      TemplateType = a.TemplateType,
                                                                      CC = a.CC,
                                                                      From = a.From,
                                                                      Subject = a.Subject,
                                                                      ReplyTo = a.ReplyTo,
                                                                      ObjectTableId = a.ObjectTableId,
                                                                      IsCopiedAtSignup = a.IsCopiedAtSignup,
                                                                      EntityId = a.EntityId
                                                                  };

            if (!string.IsNullOrEmpty(templateType))
            {
                reportsTemplateLists = reportsTemplateLists.Where(d => d.TemplateType == templateType);
            }

            List<ReportsTemplatePM> restult = FullReportsTemplateProp(tenant, reportsTemplateLists.ToList()); 


            return restult;
        }


       

        private List<ReportsTemplatePM> FullReportsTemplateProp(int tenant, List<ReportsTemplatePM> reportsTemplateLists)
        {
            #region Update By User Name

            List<string> contactIds = new List<string>();
            foreach (ReportsTemplatePM item in reportsTemplateLists)
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
                foreach (ReportsTemplatePM item in reportsTemplateLists)
                {
                    if (!string.IsNullOrEmpty(item.UpdatedByUserId))
                    {
                        ContactList contactList = contactLists.Where(d => d.Id == item.UpdatedByUserId).FirstOrDefault();
                        if (contactList != null) item.UpdateByUserName = contactList.EnglishName;

                    }
                }
            }
            #endregion
            return reportsTemplateLists;
        }




    }
}