using System;
using System.Collections.Generic;
using System.Linq;
using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Data.Security;
using AmitalCloud.Infrastructure.Domain.EntityPMs;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using AmitalCloud.Infrastructure.Domain.Interfaces;

namespace AmitalCloud.Infrastructure.Data.Queries
{
    public class TranslationQuery
    {
        readonly Repository<Translation> repository;
        readonly IAmitalCloudContext context;

        public TranslationQuery(int tenant)
        {
            context = AmitalCloudContext.GetContext(tenant);
            repository = new Repository<Translation>(context);
        }

        public List<Translation> GetTenantTranslations(int tenant)
        {
            List<Translation> AllTranslations = new Repository<Translation>(AmitalCloudContext.GetContext(tenant)).GetMulti(a => a.Tenant == tenant);
            
            Repository<Tenant> tenantRepo = new Repository<Tenant>(context);
            Tenant tenantPoco = tenantRepo.GetMulti(a => a.Id == tenant).FirstOrDefault();

            ModifyTranslationByPrivateLabel(tenant, AllTranslations, tenantPoco?.Language);
            return AllTranslations;
        }

        public List<Translation> GetTenantLanguageTranslations(int tenant, string language)
        {
            List<Translation> AllTranslations = repository.GetMulti(a => a.Tenant == 0 && a.TranslationHeaderCode == language);

            ModifyTranslationByPrivateLabel(tenant, AllTranslations, language);
            return AllTranslations;
        }

        private void ModifyTranslationByPrivateLabel(int tenant, List<Translation> AllTranslations, string language)
        {
            TenantManagmentPrivateLabelsPM privatelabel = null;
            var url = AmitalCloudSecurityUtility.getLoggedDomain();
            if (!url.Contains("system.logbox.co.il") && !url.Contains("cloud.amital.co.il"))
            {
                TenantManagmentPrivateLabelsQuery query = new TenantManagmentPrivateLabelsQuery(tenant);
                privatelabel = query.GetSingleActivePMByUrl_Cache(url);
            }
            if (privatelabel != null)
            {
                Repository<TextCode> textCodeRepo = new Repository<TextCode>(context);
                TextCode textCode = textCodeRepo.GetMulti(a => a.Code == "General.MH.Importers" && a.Tenant == tenant, "").FirstOrDefault();
                Translation tra = AllTranslations.FirstOrDefault(t => t.TextCodeCode == textCode.Code);
                if (tra != null)
                {
                    tra.TranslatedText = privatelabel.PrivateLabelName;
                }
                else
                {
                    tra = new Translation()
                    {
                        Id = Guid.NewGuid().ToString(),
                        TextCodeId = textCode.Id,
                        TextCode = textCode,
                        Tenant = tenant,
                        TranslatedText = privatelabel.PrivateLabelName,
                        TranslationHeaderCode = language,
                        TextCodeCode = textCode.Code,
                    };

                    AllTranslations.Add(tra);
                }
                textCode = textCodeRepo.GetMulti(a => a.Code == "General.MH.ActivationWizard" && a.Tenant == tenant, "").FirstOrDefault();
                tra = AllTranslations.FirstOrDefault(t => t.TextCodeCode == textCode.Code);
                if (tra != null)
                {
                    tra.TranslatedText = privatelabel.PrivateLabelShortName + " Services";
                }
                else
                {
                    tra = new Translation()
                    {
                        Id = Guid.NewGuid().ToString(),
                        TextCodeId = textCode.Id,
                        TextCode = textCode,
                        Tenant = tenant,
                        TranslatedText = privatelabel.PrivateLabelShortName + " Services",
                        TranslationHeaderCode = language,
                        TextCodeCode = textCode.Code,
                    };

                    AllTranslations.Add(tra);
                }
            }
        }
    }
}