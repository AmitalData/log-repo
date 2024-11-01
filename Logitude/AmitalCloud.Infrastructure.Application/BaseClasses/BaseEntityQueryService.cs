using AmitalCloud.Infrastructure.Data.Helpers;
using AmitalCloud.Infrastructure.Domain.BaseClasses;
using AmitalCloud.Infrastructure.Domain.Interfaces;
using System.Collections.Generic;

namespace AmitalCloud.Infrastructure.Application.BaseClasses
{
    public abstract class BaseEntityQueryService<TContext,TEntityPOCO, TEntityKeys, TEntityPM, TEntityList, TkeyType> : IBaseEntityQueryService<TEntityPM> where TEntityPOCO : BaseEntity, new()
    where TEntityPM : IEntityPM, new()
    where TEntityKeys : IEntityKeyFields<TEntityPOCO, TkeyType>, new()
    where TEntityList : class, new()
    where TContext : class , IContext
    {
        protected int Tenant;
        protected TEntityPOCO EntityPOCO { get; set; }
        protected IMapping<TEntityPM, TEntityPOCO, TEntityList> mapping;
        protected TEntityPM EntityPM;
        protected IRepository< TEntityPOCO> Repository;
        protected IContext MainContext;
        //protected TEntityParentPM EntityParentPM;
        protected TEntityKeys EntityKeys;
        //protected TEntityKeys EntityParentKeys;
        //protected TEntityParentKeys EntityParentKeys; vladi TODO - check if this is needed
        public BaseEntityQueryService()
        {

        }
        public BaseEntityQueryService( IRepository< TEntityPOCO > repository, IMapping<TEntityPM, TEntityPOCO, TEntityList> mapping)
        {
            //this.MainContext = mainContext;
            this.Repository = repository;
            this.mapping = mapping;
            this.InitializeSettings();
        }
        public TEntityPM GetSingle(TEntityKeys entityKeys, bool getComposition, bool getFromCache)
        {
            if (getFromCache && (CacheManager.CacheWrapper != null))
            {
                string cacheKey = $"TEntityPMGetSingle_({entityKeys.GetEntityPMName()}_{entityKeys.GetFullKey()}_{getComposition})";
                var cacheObj = CacheManager.CacheWrapper.Get(cacheKey);
                if (cacheObj != null)
                {
                    EntityPM = (TEntityPM)cacheObj;
                }
                else
                {
                    EntityPOCO = Repository.GetSingle(entityKeys);
                    if (EntityPOCO != null)
                    {
                        EntityPM = GetEntityPM(EntityPOCO, getComposition, entityKeys);
                    }
                    if (EntityPM != null)
                    {
                        CacheManager.CacheWrapper.Insert(cacheKey, EntityPM);
                    }
                    else
                    {
                        CacheManager.CacheWrapper.Insert(cacheKey, new NullCache());
                    }
                }
            }
            else
            {
                EntityPM = new TEntityPM();
                EntityPOCO = Repository.GetSingle(entityKeys);
                if (EntityPOCO != null)
                {
                    EntityPM = GetEntityPM(EntityPOCO, getComposition, entityKeys);
                }
            }
            return EntityPM;
        }
        public TEntityPM GetSingle(IEnumerable<KeyValuePair<string, string>> paramList, bool getComposition, bool getFromCache)
        {
            var keys = new TEntityKeys();
            keys.Initialize(paramList);
            return GetSingle(keys, getComposition, getFromCache);
        }
        public TEntityPM GetEntityPM(TEntityPOCO entityPOCO, bool getComposition = false, IEntityKeyFields<TEntityPOCO, TkeyType> entityKeys = null)
        {
            var entityPM = new TEntityPM();
            if (entityPOCO == null)
            {
                return default(TEntityPM);
            }
            mapping.POCOToPM(entityPM, entityPOCO);
            if (getComposition)
            {
                GetComposition(entityKeys, entityPM);
            }
            mapping.CustomPOCOToPM(entityPM, entityPOCO);
            return entityPM;
        }
        public virtual void GetComposition(IEntityKeyFields<TEntityPOCO, TkeyType> entityKeys, TEntityPM entityPM)
        {
        }
        protected abstract IEntityKeyFields<TEntityPOCO, TkeyType> GetKeys(TEntityPOCO entityPOCO);
        public virtual void InitializeSettings()
        {

        }
    }
}
