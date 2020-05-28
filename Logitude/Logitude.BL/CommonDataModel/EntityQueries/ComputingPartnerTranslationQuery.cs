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

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class ComputingPartnerTranslationQuery
    {
        ComputingPartnerTranslationRepository repository;

        public ComputingPartnerTranslationQuery()
        {
            repository = new ComputingPartnerTranslationRepository();
        }

        public ComputingPartnerTranslationQuery(int tenant)
        {
            repository = new ComputingPartnerTranslationRepository(tenant);
        }

        public ComputingPartnerTranslationQuery(ComputingPartnerTranslationRepository myRepository)
        {
            repository = myRepository;
        }

        public ComputingPartnerTranslationPM GetSinglePM(string id, int tenant)
        {
            ComputingPartnerTranslationPM entityPM =
                (from a in repository.Context.ComputingPartnerTranslations.Include("ObjectTable").Include("ComputingPartner").Include("CreatedByUser").Include("UpdatedByUser")
                 where a.Id == id && a.Tenant == tenant
                 select new ComputingPartnerTranslationPM()
                 {
                     Id = a.Id,
                     Tenant = a.Tenant,
                     OurCode = a.OurCode,
                     PartnerCode = a.PartnerCode,
                     CreateDate = a.CreateDate,
                     UpdateDate = a.UpdateDate,
                     CreatedByUserId = a.CreatedByUserId,
                     UpdatedByUserId = a.UpdatedByUserId,
                     ObjectTableId = a.ObjectTableId,
                     ComputingPartnerId = a.ComputingPartnerId,
                     ObjectTableName = a.ObjectTable == null ? "" : a.ObjectTable.Name,
                     ComputingPartnerName = a.ComputingPartner == null ? "" : a.ComputingPartner.Name,
                     CreatedByUserName = a.CreatedByUser == null ? "" : (a.CreatedByUser.Contact == null ? "" : a.CreatedByUser.Contact.EnglishName),
                     UpdatedByUserName = a.UpdatedByUser == null ? "" : (a.UpdatedByUser.Contact == null ? "" : a.UpdatedByUser.Contact.EnglishName),
                 }).FirstOrDefault();

            return entityPM;
        }

        public IQueryable<ComputingPartnerTranslationPM> GetTranslationsByPartnerAndTableId(string computingPartnerId, string objectTableId, int tenant)
        {
            IQueryable<ComputingPartnerTranslationPM> myResult =
                (from a in repository.Context.ComputingPartnerTranslations.Include("ObjectTable").Include("ComputingPartner").Include("CreatedByUser").Include("UpdatedByUser")
                 where a.ComputingPartnerId == computingPartnerId && a.ObjectTableId == objectTableId && a.Tenant == tenant
                 select new ComputingPartnerTranslationPM()
                 {
                     Id = a.Id,
                     Tenant = a.Tenant,
                     OurCode = a.OurCode,
                     PartnerCode = a.PartnerCode,
                     CreateDate = a.CreateDate,
                     UpdateDate = a.UpdateDate,
                     CreatedByUserId = a.CreatedByUserId,
                     UpdatedByUserId = a.UpdatedByUserId,
                     ObjectTableId = a.ObjectTableId,
                     ComputingPartnerId = a.ComputingPartnerId,
                     ObjectTableName = a.ObjectTable == null ? "" : a.ObjectTable.Name,
                     ComputingPartnerName = a.ComputingPartner == null ? "" : a.ComputingPartner.Name,
                     CreatedByUserName = a.CreatedByUser == null ? "" : (a.CreatedByUser.Contact == null ? "" : a.CreatedByUser.Contact.EnglishName),
                     UpdatedByUserName = a.UpdatedByUser == null ? "" : (a.UpdatedByUser.Contact == null ? "" : a.UpdatedByUser.Contact.EnglishName),
                 });

            return myResult;
        }


        public IQueryable<ComputingPartnerTranslationPM> GetTranslationsByPartnerAndTableIdAndCode(string computingPartnerId, string objectTableId,string Code, int tenant)
        {
            IQueryable<ComputingPartnerTranslationPM> myResult =
                (from a in repository.Context.ComputingPartnerTranslations.Include("ObjectTable").Include("ComputingPartner").Include("CreatedByUser").Include("UpdatedByUser")
                 where a.ComputingPartnerId == computingPartnerId && a.ObjectTableId == objectTableId && a.Tenant == tenant && a.OurCode==Code
                 select new ComputingPartnerTranslationPM()
                 {
                     Id = a.Id,
                     Tenant = a.Tenant,
                     OurCode = a.OurCode,
                     PartnerCode = a.PartnerCode,
                     CreateDate = a.CreateDate,
                     UpdateDate = a.UpdateDate,
                     CreatedByUserId = a.CreatedByUserId,
                     UpdatedByUserId = a.UpdatedByUserId,
                     ObjectTableId = a.ObjectTableId,
                     ComputingPartnerId = a.ComputingPartnerId,
                     ObjectTableName = a.ObjectTable == null ? "" : a.ObjectTable.Name,
                     ComputingPartnerName = a.ComputingPartner == null ? "" : a.ComputingPartner.Name,
                     CreatedByUserName = a.CreatedByUser == null ? "" : (a.CreatedByUser.Contact == null ? "" : a.CreatedByUser.Contact.EnglishName),
                     UpdatedByUserName = a.UpdatedByUser == null ? "" : (a.UpdatedByUser.Contact == null ? "" : a.UpdatedByUser.Contact.EnglishName),
                 });

            return myResult;
        }


        public IQueryable<ComputingPartnerTranslationList> GetIQueryableEntityList(IQueryable<ComputingPartnerTranslation> iQueryable, string computingPartnerId, string objectTableId, int tenant)
        {
            IQueryable<ComputingPartnerTranslationList> myResult = from a in iQueryable.Include("CreatedByUser").Include("UpdatedByUser").Include("CreatedByUser.Contact").Include("UpdatedByUser.Contact").Include("ComputingPartner").Include("ObjectTable")
                                                                   where a.ComputingPartnerId == computingPartnerId && a.ObjectTableId == objectTableId && a.Tenant == tenant
                                                                   select new ComputingPartnerTranslationList()
                                                        {
                                                            Id = a.Id,
                                                            CreateDate = a.CreateDate,
                                                            UpdateDate = a.UpdateDate,
                                                            CreatedByUserId = a.CreatedByUserId,
                                                            UpdatedByUserId = a.UpdatedByUserId,
                                                            ComputingPartnerName = a.ComputingPartner != null ? a.ComputingPartner.Name : null,
                                                            CreatedByUserName = a.CreatedByUser == null ? "" : (a.CreatedByUser.Contact == null ? "" : a.CreatedByUser.Contact.EnglishName),
                                                            UpdatedByUserName = a.UpdatedByUser == null ? "" : (a.UpdatedByUser.Contact == null ? "" : a.UpdatedByUser.Contact.EnglishName),
                                                            Tenant = tenant,
                                                            ComputingPartnerId=a.ComputingPartnerId,
                                                            ObjectTableId=a.ObjectTableId,
                                                            OurCode=a.OurCode,
                                                            PartnerCode=a.PartnerCode,
                                                            ObjectTableName=a.ObjectTable!=null?a.ObjectTable.Name:null,
                                                            SearchFields=a.SearchFields,
                                                        };
            return myResult;
        }

        public string GetPartnerCodeTranslation(string logitudeCode, string computingPartnerId, string objectTableId, int tenant)
        {
            string partnerCode = (from a in repository.Context.ComputingPartnerTranslations
                                  where a.ComputingPartnerId == computingPartnerId && a.ObjectTableId == objectTableId && a.Tenant == tenant
                                  && a.OurCode == logitudeCode
                                  select a.PartnerCode).FirstOrDefault();
            if (string.IsNullOrEmpty(partnerCode) || string.IsNullOrWhiteSpace(partnerCode))
            {
                partnerCode = (from a in repository.Context.ComputingPartnerTranslations
                               where a.ComputingPartnerId == computingPartnerId && a.ObjectTableId == objectTableId && a.Tenant == 0
                               && a.OurCode == logitudeCode
                               select a.PartnerCode).FirstOrDefault();
            }

            return partnerCode;
        }

        public string GetLogitudeCodeTranslation(string PartnerCode, string computingPartnerId, string objectTableId, int tenant)
        {
            string LogitudeCode = (from a in repository.Context.ComputingPartnerTranslations
                                  where a.ComputingPartnerId == computingPartnerId && a.ObjectTableId == objectTableId && a.Tenant == tenant
                                  && a.PartnerCode == PartnerCode
                                  select a.OurCode).FirstOrDefault();
            if (LogitudeCode == null)
            {
                LogitudeCode = (from a in repository.Context.ComputingPartnerTranslations
                               where a.ComputingPartnerId == computingPartnerId && a.ObjectTableId == objectTableId && a.Tenant == 0
                               && a.PartnerCode == PartnerCode
                               select a.OurCode).FirstOrDefault();
            }

            return LogitudeCode;
        }

        public List<ComputingPartnerTranslationPM> GetAllByComputingPartner(string computingPartner, int tenant)
        {
            List<ComputingPartnerTranslationPM> entityPMs =
                (from a in repository.Context.ComputingPartnerTranslations.Include("ObjectTable").Include("ComputingPartner").Include("CreatedByUser").Include("UpdatedByUser")
                 where  a.Tenant == tenant && a.ComputingPartnerId == computingPartner
                 select new ComputingPartnerTranslationPM()
                 {
                     Id = a.Id,
                     Tenant = a.Tenant,
                     OurCode = a.OurCode,
                     PartnerCode = a.PartnerCode,
                     CreateDate = a.CreateDate,
                     UpdateDate = a.UpdateDate,
                     CreatedByUserId = a.CreatedByUserId,
                     UpdatedByUserId = a.UpdatedByUserId,
                     ObjectTableId = a.ObjectTableId,
                     ComputingPartnerId = a.ComputingPartnerId,
                     ObjectTableName = a.ObjectTable == null ? "" : a.ObjectTable.Name,
                     ComputingPartnerName = a.ComputingPartner == null ? "" : a.ComputingPartner.Name,
                     CreatedByUserName = a.CreatedByUser == null ? "" : (a.CreatedByUser.Contact == null ? "" : a.CreatedByUser.Contact.EnglishName),
                     UpdatedByUserName = a.UpdatedByUser == null ? "" : (a.UpdatedByUser.Contact == null ? "" : a.UpdatedByUser.Contact.EnglishName),
                 }).ToList();

            return entityPMs;
        }
    }
}