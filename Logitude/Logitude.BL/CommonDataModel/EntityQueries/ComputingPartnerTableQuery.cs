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

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class ComputingPartnerTableQuery
    {
        ComputingPartnerTableRepository repository;



        public ComputingPartnerTableQuery(int tenant)
        {
            repository = new ComputingPartnerTableRepository(tenant);
        }

        public ComputingPartnerTableQuery(ComputingPartnerTableRepository myRepository)
        {
            repository = myRepository;
        }

        public ComputingPartnerTablePM GetSinglePM(string computingPartnerId, string objectTableId, int tenant)
        {
            ComputingPartnerTablePM entityPM =
                (from a in repository.Context.ComputingPartnerTables.Include("ObjectTable").Include("ComputingPartner").Include("CreatedByUser").Include("UpdatedByUser")
                 where a.ComputingPartnerId == computingPartnerId && a.ObjectTableId == objectTableId && (a.Tenant == tenant || a.Tenant == 0)
                 select new ComputingPartnerTablePM()
                 {
                     ComputingPartnerId = a.ComputingPartnerId,
                     ObjectTableId = a.ObjectTableId,
                     Tenant = a.Tenant,
                     Name = a.Name,
                     HasPartnerList = a.HasPartnerList,
                     MustUsePartnerList = a.MustUsePartnerList,
                     TransalationRequired = a.TransalationRequired,
                     TenantLevelTranslationBlocked = a.TenantLevelTranslationBlocked,
                     CreateDate = a.CreateDate,
                     UpdateDate = a.UpdateDate,
                     CreatedByUserId = a.CreatedByUserId,
                     UpdatedByUserId = a.UpdatedByUserId,
                     ObjectTableName = a.ObjectTable == null ? null : a.ObjectTable.Name,
                     ComputingPartnerName = a.ComputingPartner == null ? null : a.ComputingPartner.Name,
                     CreatedByUserName = a.CreatedByUser == null ? null : (a.CreatedByUser.Contact == null ? null : a.CreatedByUser.Contact.EnglishName),
                     UpdatedByUserName = a.UpdatedByUser == null ? null : (a.UpdatedByUser.Contact == null ? null : a.UpdatedByUser.Contact.EnglishName),
                 }).FirstOrDefault();

            return entityPM;
        }

        public IQueryable<ComputingPartnerTablePM> GetTablesByPartnerId(string computingPartnerId, int tenant)
        {
            IQueryable<ComputingPartnerTablePM> myResult =
                (from a in repository.Context.ComputingPartnerTables.Include("ObjectTable").Include("ComputingPartner").Include("CreatedByUser").Include("UpdatedByUser")
                 where a.ComputingPartnerId == computingPartnerId && (a.Tenant == tenant || a.Tenant == 0)
                 select new ComputingPartnerTablePM()
                 {
                     ComputingPartnerId = a.ComputingPartnerId,
                     ObjectTableId = a.ObjectTableId,
                     Tenant = a.Tenant,
                     Name = a.Name,
                     HasPartnerList = a.HasPartnerList,
                     MustUsePartnerList = a.MustUsePartnerList,
                     TransalationRequired = a.TransalationRequired,
                     TenantLevelTranslationBlocked = a.TenantLevelTranslationBlocked,
                     CreateDate = a.CreateDate,
                     UpdateDate = a.UpdateDate,
                     CreatedByUserId = a.CreatedByUserId,
                     UpdatedByUserId = a.UpdatedByUserId,
                     ObjectTableName = a.ObjectTable == null ? "" : a.ObjectTable.Name,
                     ComputingPartnerName = a.ComputingPartner == null ? "" : a.ComputingPartner.Name,
                     CreatedByUserName = a.CreatedByUser == null ? null : (a.CreatedByUser.Contact == null ? null : a.CreatedByUser.Contact.EnglishName),
                     UpdatedByUserName = a.UpdatedByUser == null ? null : (a.UpdatedByUser.Contact == null ? null : a.UpdatedByUser.Contact.EnglishName),
                 });

            return myResult;
        }




    }
}
