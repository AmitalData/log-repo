using Logitude.BL.CommonDataModel.EntityPMs;
using Simplog.Data.CommonDataModel.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;
using Logitude.BL.CommonDataModel.EntityLists;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;

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
                     CreatedByUserName = a.CreatedByUser == null ? null : (a.CreatedByUser.Contact == null ? null : a.CreatedByUser.Contact.EnglishName),
                     UpdatedByUserName = a.UpdatedByUser == null ? null : (a.UpdatedByUser.Contact == null ? null : a.UpdatedByUser.Contact.EnglishName),
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
                     CreatedByUserName = a.CreatedByUser == null ? null : (a.CreatedByUser.Contact == null ? null : a.CreatedByUser.Contact.EnglishName),
                     UpdatedByUserName = a.UpdatedByUser == null ? null : (a.UpdatedByUser.Contact == null ? null : a.UpdatedByUser.Contact.EnglishName),
                     SearchFields = a.SearchFields,
                 }).ToList();

            return myResult;
        }
        
        public IQueryable<TariffCarrierTranslationList> GetIQueryableEntityList(IQueryable<TariffCarrierTranslation> entityPocos)
        {
            IQueryable<TariffCarrierTranslationList> myList =
                (from a in entityPocos
                 select new TariffCarrierTranslationList()
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
                     SearchFields = a.SearchFields,
                 });

            return myList;
        }
    }
}
