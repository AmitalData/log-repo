using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Model.EntityClasses;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Model.Interfaces;
using System.Collections.Generic;
using System.Linq;

namespace AmitalCloud.Infrastructure.APITools.Helpers
{
    public class ComputingPartnerTranslationHelper
    {
        IAmitalCloudContext context;
        ObjectTableRepository myObjectTabelRepository;
        int tenant = 0;
        public ComputingPartnerTranslationHelper(int tenant)
        {
            this.tenant = tenant;
            context = AmitalCloudContext.GetContext(tenant);
            myObjectTabelRepository = new ObjectTableRepository(tenant);
        }
        public string GetComputingPartnerCodeTranslation(string localCode, string computingPartner, string objectTableName)
        {
            string key = $"GetComputingPartnerCodeTranslation({localCode}, {computingPartner}, {objectTableName})";
            return CacheManager.GetOrInsertNewObject<string>(key, () =>
            {
                return GetComputingPartnerCodeTranslationReal(localCode, computingPartner, objectTableName);
            });
        }
        string GetComputingPartnerCodeTranslationReal(string localCode, string computingPartner, string objectTableName)
        {
            ObjectTable objectTable = myObjectTabelRepository.GetObjectTableByName(objectTableName, 0, true);
            ComputingPartnerPM partner = GetSinglePMByCode(computingPartner, tenant);
            if (partner == null)
            {
                partner = GetSinglePMByCode(computingPartner, 0);
            }
            string partnerCode = null;
            if (partner != null && objectTable != null)
            {
                partnerCode = GetPartnerCodeTranslation(localCode, partner.Id, objectTable.Id, tenant);
            }
            return partnerCode;
        }
        string GetPartnerCodeTranslation(string localCode, string computingPartnerId, string objectTableId, int tenant)
        {
            var result = new Repository<ComputingPartnerTranslation>(context).GetMulti(a => a.ComputingPartnerId == computingPartnerId && a.ObjectTableId == objectTableId && (a.Tenant == tenant || a.Tenant == 0) && a.OurCode == localCode, a => new { a.Tenant, a.PartnerCode });
            return result.Where(a => a.Tenant == tenant).Select(a => a.PartnerCode).FirstOrDefault() ?? result.Where(a => a.Tenant == 0).Select(a => a.PartnerCode).FirstOrDefault();
        }
        public string GetLocalCodeTranslation(string PartnerCode, string computingPartnerId, string objectTableId, int tenant)
        {
            var result = new Repository<ComputingPartnerTranslation>(context).GetMulti(a => a.ComputingPartnerId == computingPartnerId && a.ObjectTableId == objectTableId && (a.Tenant == tenant || a.Tenant == 0) && a.PartnerCode.ToUpper() == PartnerCode.ToUpper(), a => new { a.Tenant, a.OurCode });
            return result.Where(a => a.Tenant == tenant).Select(a => a.OurCode).FirstOrDefault() ?? result.Where(a => a.Tenant == 0).Select(a => a.OurCode).FirstOrDefault();
        }
        List<ComputingPartnerTranslationPM> GetAllByComputingPartner(string computingPartner, int tenant)
        =>
                new Repository<ComputingPartnerTranslation>(context).GetMulti(a => a.ComputingPartnerId == computingPartner && a.Tenant == tenant, a =>
                    new ComputingPartnerTranslationPM(a)).ToList();
        ComputingPartnerPM GetSinglePMByCode(string code, int tenant)
        {
            if (string.IsNullOrWhiteSpace(code)) return new ComputingPartnerPM();
            string key = $"GetSinglePMByCode({code},{tenant})";
            return CacheManager.GetOrInsertNewObject<ComputingPartnerPM>(key, () =>
            {
                return GetSinglePMByCodeSlow(code, tenant);
            });
        }
        ComputingPartnerPM GetSinglePMByCodeSlow(string code, int tenant)
        {
            return new Repository<ComputingPartner>(context).GetMulti(a => a.Code == code && (a.Tenant == tenant || a.Tenant == 0)
            , a => new ComputingPartnerPM()
            {
                Id = a.Id,
                CreateDate = a.CreateDate,
                UpdateDate = a.UpdateDate,
                CreatedByUserId = a.CreatedByUserId,
                UpdatedByUserId = a.UpdatedByUserId,
                Name = a.Name,
                Remarks = a.Remarks,
                SearchFields = a.SearchFields,
                Code = a.Code,
                InActive = a.InActive,
                Description = a.Description,
                Tenant = a.Tenant,
            }).FirstOrDefault();
        }
        public List<ComputingPartnerTranslationPM> GetComputingPartnerCodeTranslations(string computingPartner, int tenant)
        {
            List<ComputingPartnerTranslationPM> partnerTranslationPMs = null;

            ComputingPartnerPM partner = GetSinglePMByCode(computingPartner, tenant);
            if (partner == null)
            {
                partner = GetSinglePMByCode(computingPartner, 0);
            }


            if (partner != null)
            {
                partnerTranslationPMs = GetAllByComputingPartner(partner.Id, tenant).ToList();
            }


            return partnerTranslationPMs;
        }
        public string GetLocalCodeTranslation(string PartnerCode, string computingPartner, string objectTableName)
        {
            ObjectTable objectTable = myObjectTabelRepository.GetObjectTableByName(objectTableName, 0, true);
            ComputingPartnerPM partner = null;

            partner = GetSinglePMByCode(computingPartner, tenant);

            if (partner == null)
            {
                partner = GetSinglePMByCode(computingPartner, 0);
            }

            string MyCode = null;
            if (partner != null && objectTable != null)
            {
                MyCode = GetLocalCodeTranslation(PartnerCode, partner.Id, objectTable.Id, tenant);
            }

            return MyCode;
        }
    }

}
