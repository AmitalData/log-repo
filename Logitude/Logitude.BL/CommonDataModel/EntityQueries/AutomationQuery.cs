using System;
using System.Linq;
using System.Web;
using System.Collections.Generic;
using Logitude.BL.Helpers;
using Logitude.BL.CommonDataModel.EntityPMs;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Data.CommonDataModel.Repositories;
using Simplog.Server.Infrastructure.Helpers;
using System.Transactions;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class AutomationQuery
    {
        AutomationRepository repository;
        public AutomationQuery()
        {
            repository = new AutomationRepository();
        }

        public AutomationQuery(int tenant)
        {
            repository = new AutomationRepository(tenant);
        }

        public AutomationQuery(AutomationRepository AutomationRepository)
        {
            repository = AutomationRepository;
        }


        public IQueryable<AutomationPM> GetAutomationPMsByTenant(int tenant)
        {
            IQueryable<AutomationPM> Automationes = from a in repository.context.Automations
                                                    where a.Tenant == tenant
                                                    select new AutomationPM()
                                                    {
                                                        Id = a.Id,
                                                        Name= a.Name,
                                                        CreateDate = a.CreateDate,
                                                        CreatedByUserId = a.CreatedByUserId,
                                                        Description = a.Description,
                                                        ResultCode = a.ResultCode,
                                                        Type = a.Type,
                                                        UpdateDate = a.UpdateDate,
                                                        UpdatedByUserId = a.UpdatedByUserId,
                                                        Inactive = a.Inactive,
                                                        Tenant = a.Tenant,
                                                        ObjectTableId = a.ObjectTableId,
                                                        From = a.From,
                                                        FromEmail = a.FromEmail,
                                                        Order = a.Order,
                                                        Code = a.Code,
                                                    };
            return Automationes;
        }

        public AutomationPM GetSinglePM(string id, int tenant)
        {
        

            var query = (from a in repository.context.Automations
                         where a.Tenant == tenant && a.Id == id
                         select new AutomationPM()
                         {
                             Id = a.Id,
                             Name = a.Name,
                             CreateDate = a.CreateDate,
                             CreatedByUserId = a.CreatedByUserId,
                             Description = a.Description,
                             ResultCode = a.ResultCode,
                             Type = a.Type,
                             UpdateDate = a.UpdateDate,
                             UpdatedByUserId = a.UpdatedByUserId,
                             Inactive = a.Inactive,
                             Tenant = a.Tenant,
                             ObjectTableId = a.ObjectTableId,
                             Version = a.Version,
                             AutomationXML = a.AutomationXML,
                             From = a.From,
                             FromEmail = a.FromEmail,
                             Order = a.Order,
                             Code = a.Code,
                         }).FirstOrDefault();
            return query;
        }


        public string GetAutomationXmalById(string id, int tenant)
        {

         
            string result = (from a in repository.context.Automations
                         where a.Tenant == tenant && a.Id == id
                        select a.AutomationXML).FirstOrDefault();

            return result;
        }






        public IQueryable<AutomationList> GetIQueryableEntityList(IQueryable<Automation> iQueryable)
        {
            IQueryable<AutomationList> result = from a in iQueryable
                                                select new AutomationList()
                                                {
                                                    Id = a.Id,
                                                    Name = a.Name,
                                                    CreateDate = a.CreateDate,
                                                    CreatedByUserId = a.CreatedByUserId,
                                                    Description = a.Description,
                                                    ResultCode = a.ResultCode,
                                                    Type = a.Type,
                                                    UpdateDate = a.UpdateDate,
                                                    UpdatedByUserId = a.UpdatedByUserId,
                                                    Inactive = a.Inactive,
                                                    Tenant = a.Tenant,
                                                    ObjectTableId = a.ObjectTableId,
                                                    Version = a.Version,
                                                    AutomationXML = a.AutomationXML,
                                                    From = a.From,
                                                    FromEmail = a.FromEmail,
                                                    Order = a.Order,
                                                    Code = a.Code,
                                                };
            return result;
        }

        public Automation GetFirstAutomationForTenant(int tenant)
        {
            return (from a in repository.context.Automations
                    where a.Tenant == tenant
                    select a).FirstOrDefault();
        }


        public List<AutomationPM> GetAutomationPMsByObjectTableId(string objectTableId, int tenant  , bool withOutXmal=false)
        {
            List<AutomationPM> automationlist = null;
            if (withOutXmal)
            {
                automationlist = (from a in repository.context.Automations.Include("CreatedByUser.Contact").Include("UpdatedByUser.Contact")
                                  where a.Tenant == tenant && a.ObjectTableId == objectTableId
                                  select new AutomationPM()
                                  {
                                      Id = a.Id,
                                      Name = a.Name,
                                      CreateDate = a.CreateDate,
                                      CreatedByUserId = a.CreatedByUserId,
                                      Description = a.Description,
                                      ResultCode = a.ResultCode,
                                      Type = a.Type,
                                      UpdateDate = a.UpdateDate,
                                      UpdatedByUserId = a.UpdatedByUserId,
                                      Inactive = a.Inactive,
                                      Tenant = a.Tenant,
                                      ObjectTableId = a.ObjectTableId,
                                      Version = a.Version,
                                      CreatedByUserName = a.CreatedByUser.Contact.EnglishName,
                                      UpdatedByUserName = a.UpdatedByUser.Contact.EnglishName,
                                      DocumentTypeId = a.DocumentTypeId,
                                      TemplateId = a.TemplateId,
                                      From = a.From,
                                      FromEmail = a.FromEmail,
                                      Order = a.Order,
                                      Code = a.Code,

                                  }).ToList();
            }
            else
            {
                automationlist = (from a in repository.context.Automations.Include("CreatedByUser.Contact").Include("UpdatedByUser.Contact")
                                  where a.Tenant == tenant && a.ObjectTableId == objectTableId
                                  select new AutomationPM()
                                  {
                                      Id = a.Id,
                                      Name = a.Name,
                                      CreateDate = a.CreateDate,
                                      CreatedByUserId = a.CreatedByUserId,
                                      Description = a.Description,
                                      ResultCode = a.ResultCode,
                                      Type = a.Type,
                                      UpdateDate = a.UpdateDate,
                                      UpdatedByUserId = a.UpdatedByUserId,
                                      Inactive = a.Inactive,
                                      Tenant = a.Tenant,
                                      ObjectTableId = a.ObjectTableId,
                                      Version = a.Version,
                                      CreatedByUserName = a.CreatedByUser.Contact.EnglishName,
                                      UpdatedByUserName = a.UpdatedByUser.Contact.EnglishName,
                                      DocumentTypeId = a.DocumentTypeId,
                                      TemplateId = a.TemplateId,
                                      From = a.From,
                                      FromEmail = a.FromEmail,
                                      Order = a.Order,
                                      AutomationXML = a.AutomationXML,
                                      Code = a.Code,
                                  }).ToList();
            }
            return automationlist;
        }


        public List<string> GetAutomationCodeLists(int tenant)
        {
            List<string> automationCodeLists = (from a in repository.context.Automations
                                                where a.Tenant == tenant
                                                select a.Code).ToList();
            return automationCodeLists;
        }


    }
}
