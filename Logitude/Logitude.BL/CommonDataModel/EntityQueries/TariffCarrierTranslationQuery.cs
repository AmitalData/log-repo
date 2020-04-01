using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.BL.CommonDataModel.EntityQueries
{
    public class TariffCarrierTranslationQuery
    {
        TariffCarrierTranslationRepository repository;
        
        public TariffCarrierTranslationQuery(int tenant)
        {
            repository = new TariffCarrierTranslationRepository(tenant);
        }

        public TariffCarrierTranslationQuery(TariffCarrierTranslationRepository myRepository)
        {
            repository = myRepository;
        }

        public TariffCarrierTranslationPM GetSinglePM(string id, int tenant)
        {
            TariffCarrierTranslationPM entityPM =
                (from a in repository.Context.TariffCarrierTranslations.Include("Port").Include("Carrier").Include("CreatedByUser").Include("UpdatedByUser")
                 where a.Id == id && a.Tenant == tenant
                 select new TariffCarrierTranslationPM()
                 {
                     Id = a.Id,
                     Tenant = a.Tenant,
                     PartnerCode = a.PartnerCode,
                     CreateDate = a.CreateDate,
                     UpdateDate = a.UpdateDate,
                     CreatedByUserId = a.CreatedByUserId,
                     UpdatedByUserId = a.UpdatedByUserId,
                     PortId = a.PortId,
                     CarrierId = a.CarrierId,
                     PortCode = a.Port == null ? "" : a.Port.Code,
                     PortName = a.Port == null ? "" : a.Port.EnglishName,
                     CreatedByUserName = a.CreatedByUser == null ? "" : (a.CreatedByUser.Contact == null ? "" : a.CreatedByUser.Contact.EnglishName),
                     UpdatedByUserName = a.UpdatedByUser == null ? "" : (a.UpdatedByUser.Contact == null ? "" : a.UpdatedByUser.Contact.EnglishName),
                     SearchFields = a.SearchFields,
                 }).FirstOrDefault();

            return entityPM;
        }

        public List<TariffCarrierTranslationPM> GetTranslationsByCarrier(string carrierId, int tenant)
        {
            List<TariffCarrierTranslationPM> myResult =
                (from a in repository.Context.TariffCarrierTranslations.Include("Port").Include("Carrier").Include("CreatedByUser").Include("UpdatedByUser")
                 where a.CarrierId == carrierId && a.Tenant == tenant
                 select new TariffCarrierTranslationPM()
                 {
                     Id = a.Id,
                     Tenant = a.Tenant,
                     PartnerCode = a.PartnerCode,
                     CreateDate = a.CreateDate,
                     UpdateDate = a.UpdateDate,
                     CreatedByUserId = a.CreatedByUserId,
                     UpdatedByUserId = a.UpdatedByUserId,
                     PortId = a.PortId,
                     CarrierId = a.CarrierId,
                     PortCode = a.Port == null ? "" : a.Port.Code,
                     PortName = a.Port == null ? "" : a.Port.EnglishName,
                     CreatedByUserName = a.CreatedByUser == null ? "" : (a.CreatedByUser.Contact == null ? "" : a.CreatedByUser.Contact.EnglishName),
                     UpdatedByUserName = a.UpdatedByUser == null ? "" : (a.UpdatedByUser.Contact == null ? "" : a.UpdatedByUser.Contact.EnglishName),
                     SearchFields = a.SearchFields,
                 }).ToList();

            return myResult;
        }


        //public IQueryable<TariffCarrierTranslationPM> GetTranslationsByPartnerAndTableIdAndCode(string computingPartnerId, string objectTableId, string Code, int tenant)
        //{
        //    IQueryable<TariffCarrierTranslationPM> myResult =
        //        (from a in repository.Context.TariffCarrierTranslations.Include("ObjectTable").Include("ComputingPartner").Include("CreatedByUser").Include("UpdatedByUser")
        //         where a.ComputingPartnerId == computingPartnerId && a.ObjectTableId == objectTableId && a.Tenant == tenant && a.OurCode == Code
        //         select new TariffCarrierTranslationPM()
        //         {
        //             Id = a.Id,
        //             Tenant = a.Tenant,
        //             OurCode = a.OurCode,
        //             PartnerCode = a.PartnerCode,
        //             CreateDate = a.CreateDate,
        //             UpdateDate = a.UpdateDate,
        //             CreatedByUserId = a.CreatedByUserId,
        //             UpdatedByUserId = a.UpdatedByUserId,
        //             ObjectTableId = a.ObjectTableId,
        //             ComputingPartnerId = a.ComputingPartnerId,
        //             ObjectTableName = a.ObjectTable == null ? "" : a.ObjectTable.Name,
        //             ComputingPartnerName = a.ComputingPartner == null ? "" : a.ComputingPartner.Name,
        //             CreatedByUserName = a.CreatedByUser == null ? "" : (a.CreatedByUser.Contact == null ? "" : a.CreatedByUser.Contact.EnglishName),
        //             UpdatedByUserName = a.UpdatedByUser == null ? "" : (a.UpdatedByUser.Contact == null ? "" : a.UpdatedByUser.Contact.EnglishName),
        //         });

        //    return myResult;
        //}

        //public string GetPartnerCodeTranslation(string logitudeCode, string computingPartnerId, string objectTableId, int tenant)
        //{
        //    string partnerCode = (from a in repository.Context.TariffCarrierTranslations
        //                          where a.ComputingPartnerId == computingPartnerId && a.ObjectTableId == objectTableId && a.Tenant == tenant
        //                          && a.OurCode == logitudeCode
        //                          select a.PartnerCode).FirstOrDefault();
        //    if (string.IsNullOrEmpty(partnerCode) || string.IsNullOrWhiteSpace(partnerCode))
        //    {
        //        partnerCode = (from a in repository.Context.TariffCarrierTranslations
        //                       where a.ComputingPartnerId == computingPartnerId && a.ObjectTableId == objectTableId && a.Tenant == 0
        //                       && a.OurCode == logitudeCode
        //                       select a.PartnerCode).FirstOrDefault();
        //    }

        //    return partnerCode;
        //}

        //public string GetLogitudeCodeTranslation(string PartnerCode, string computingPartnerId, string objectTableId, int tenant)
        //{
        //    string LogitudeCode = (from a in repository.Context.TariffCarrierTranslations
        //                           where a.ComputingPartnerId == computingPartnerId && a.ObjectTableId == objectTableId && a.Tenant == tenant
        //                           && a.PartnerCode == PartnerCode
        //                           select a.OurCode).FirstOrDefault();
        //    if (LogitudeCode == null)
        //    {
        //        LogitudeCode = (from a in repository.Context.TariffCarrierTranslations
        //                        where a.ComputingPartnerId == computingPartnerId && a.ObjectTableId == objectTableId && a.Tenant == 0
        //                        && a.PartnerCode == PartnerCode
        //                        select a.OurCode).FirstOrDefault();
        //    }

        //    return LogitudeCode;
        //}

        //public List<TariffCarrierTranslationPM> GetAllByComputingPartner(string computingPartner, int tenant)
        //{
        //    List<TariffCarrierTranslationPM> entityPMs =
        //        (from a in repository.Context.TariffCarrierTranslations.Include("ObjectTable").Include("ComputingPartner").Include("CreatedByUser").Include("UpdatedByUser")
        //         where a.Tenant == tenant && a.ComputingPartnerId == computingPartner
        //         select new TariffCarrierTranslationPM()
        //         {
        //             Id = a.Id,
        //             Tenant = a.Tenant,
        //             OurCode = a.OurCode,
        //             PartnerCode = a.PartnerCode,
        //             CreateDate = a.CreateDate,
        //             UpdateDate = a.UpdateDate,
        //             CreatedByUserId = a.CreatedByUserId,
        //             UpdatedByUserId = a.UpdatedByUserId,
        //             ObjectTableId = a.ObjectTableId,
        //             ComputingPartnerId = a.ComputingPartnerId,
        //             ObjectTableName = a.ObjectTable == null ? "" : a.ObjectTable.Name,
        //             ComputingPartnerName = a.ComputingPartner == null ? "" : a.ComputingPartner.Name,
        //             CreatedByUserName = a.CreatedByUser == null ? "" : (a.CreatedByUser.Contact == null ? "" : a.CreatedByUser.Contact.EnglishName),
        //             UpdatedByUserName = a.UpdatedByUser == null ? "" : (a.UpdatedByUser.Contact == null ? "" : a.UpdatedByUser.Contact.EnglishName),
        //         }).ToList();

        //    return entityPMs;
        //}
    }
}
