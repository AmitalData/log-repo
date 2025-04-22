using System.Linq;
using AmitalCloud.Infrastructure.Data.Repositories;
using AmitalCloud.Infrastructure.Domain.EntityPOCOs;
using AmitalCloud.Infrastructure.Data.Context;
using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using System.Transactions;
using System;
using AmitalCloud.Infrastructure.Domain.DataContracts;

namespace AmitalCloud.Infrastructure.Data.Queries
{
    public class SystemMetadataLastUpdateQuery
    {
        private readonly Repository<SystemMetadataLastUpdate> repository;
        private int SystemMetadataLastUpdateDefaultId = 1;

        public SystemMetadataLastUpdateQuery(int tenant)
        {
            repository = new Repository<SystemMetadataLastUpdate>(GlobalContext.GetContext(tenant));
        }

        public MetaDataLastUpdateDates GetSystemMetadataLastUpdatesCacheHandle(int tenant)
        {
            string entityName = "SystemMetadataLastUpdates_" + tenant;
            MetaDataLastUpdateDates metadatalastUpdates;

            if (CacheManager.CacheWrapper != null && CacheManager.CacheWrapper.Get(entityName) != null)
            {
                metadatalastUpdates = (MetaDataLastUpdateDates)CacheManager.CacheWrapper.Get(entityName);
            }
            else
            {
                metadatalastUpdates = GetSystemMetadataLastUpdateFromDB(tenant);

                if (CacheManager.CacheWrapper != null && CacheManager.CacheWrapper.Get(entityName) == null && metadatalastUpdates != null)
                {
                    CacheManager.CacheWrapper.Insert(entityName, metadatalastUpdates, null, DateTime.UtcNow.AddMinutes(1), TimeSpan.Zero);
                }
            }
            return metadatalastUpdates;
        }

        private MetaDataLastUpdateDates GetSystemMetadataLastUpdateFromDB(int tenant)
        {
            MetaDataLastUpdateDates metadata = new MetaDataLastUpdateDates()
            {
                Id = SystemMetadataLastUpdateDefaultId,
            };

            using (TransactionScope scope = TransactionFactory.GetNewTransaction())
            {
                SystemMetadataLastUpdate update = repository.GetSingle(a => a.Id == SystemMetadataLastUpdateDefaultId.ToString());

                metadata.ObjectFieldsSystemUpdateDateGMT = (update != null ? update.ObjectFieldsUpdateDateGMT : DateTime.UtcNow);
                metadata.TranslationsSystemUpdateDateGMT = (update != null ? update.TranslationsUpdateDateGMT : DateTime.UtcNow);

                scope.Complete();
            }

            IAmitalCloudContext amitalCloudContext = AmitalCloudContext.GetContext(tenant);

            ObjectFieldRepository objectFieldsRepository = new ObjectFieldRepository(amitalCloudContext);
            ObjectFieldModification mod = objectFieldsRepository.GetLastObjectFieldModificationByTenant(tenant);
            metadata.ObjectFieldsTenantUpdateDateGMT = (mod != null ? mod.UpdateDateGMT.Value : new DateTime(2015, 1, 1));

            TranslationRepository translationRepository = new TranslationRepository(amitalCloudContext);
            Translation translation = translationRepository.GetLastTranslationsByTenant(tenant);
            metadata.TranslationsTenantUpdateDateGMT = (translation != null ? translation.UpdateDateGMT.Value : new DateTime(2015, 1, 1));
            return metadata;
        }
    }
}