using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Logitude.Server.Tools.Counters;
using System.Data.Entity;
using Logitude.Customs.Data;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System.Reflection;

namespace Logitude.Server.Tools
{
    public abstract class EntityQueryService<TEntityPOCO, TEntityKeys,TEntityPM, TEntityParentPM,TEntityParentKeys>
        where TEntityPOCO : class , new()
        where TEntityPM : EntityPM , new()
        where TEntityKeys : EntityKeyFields,new()
        where TEntityParentKeys : EntityKeyFields
   
    {
        
        protected int Tenant;
        protected TEntityPOCO EntityPOCO { get; set; }
        protected IMapping<TEntityPM, TEntityPOCO> mapping;
        protected TEntityPM EntityPM;
        protected IRepository<TEntityPOCO> Repository;
        protected IContext MainContext;
        protected TEntityParentPM EntityParentPM;
        protected TEntityKeys EntityKeys;
        protected TEntityParentKeys EntityParentKeys;

        public EntityQueryService()
        {

        }

        public EntityQueryService(IContext mainContext, int tenant)
        {
            this.Tenant = tenant;
            this.MainContext = mainContext;
        }

        public List<TEntityPM> GetMulti(TEntityParentKeys entityParentKeys, bool getFromCache,bool getComposition=true)
        {
            List<TEntityPOCO> entityPOCOs = Repository.GetMulti(entityParentKeys);
            List<TEntityPM> entityPMs = new List<TEntityPM>();
            foreach (TEntityPOCO entityPOCO in entityPOCOs)
            {
                TEntityPM entityPM = new TEntityPM();
                EntityKeyFields entityKeys = GetKeys(entityPOCO);
                if (entityKeys != null && getComposition)
                {
                    GetComposition(entityKeys,entityPM);
                }
                mapping.CustomPOCOToPM(entityPM, entityPOCO);
                mapping.POCOToPM(entityPM, entityPOCO);
                entityPMs.Add(entityPM);
            }
            return entityPMs;
        }

        protected TEntityPM GetSingle(TEntityKeys entityKeys,bool getComposition, bool getFromCache)
        {
            if (getFromCache && (CacheManager.CacheWrapper != null))
            {
//#if CrashIfRecorcdNotFound 
//                string entityKeyString = entityKeys.GetEntityPMName() + "_" + entityKeys.GetFullKey();
//                EntityPM = CacheManager.CacheWrapper.Get(entityKeyString) as TEntityPM;
//                if (EntityPM == null)
//                {
//                    EntityPOCO = Repository.GetSingle(entityKeys);
//                    if (EntityPOCO != null)
//                    {
//                        EntityPM = new TEntityPM();
//                        mapping.CustomPOCOToPM(EntityPM, EntityPOCO);
//                        mapping.POCOToPM(EntityPM, EntityPOCO);
//                        if (getComposition)
//                        {
//                            GetComposition(entityKeys, EntityPM);
//                        }
//                    }
//                    CacheManager.CacheWrapper.Insert(entityKeyString, EntityPM);
//                }            
//#else
                string entityKeyString = entityKeys.GetEntityPMName() + "_" + entityKeys.GetFullKey();
                var cacheObj = CacheManager.CacheWrapper.Get(entityKeyString);
                if (cacheObj != null)
                {
                    EntityPM = cacheObj as TEntityPM;
                }
                else
                {
                    EntityPOCO = Repository.GetSingle(entityKeys);
                    if (EntityPOCO != null)
                    {
                        //EntityPM = new TEntityPM();
                        //mapping.CustomPOCOToPM(EntityPM, EntityPOCO);
                        //mapping.POCOToPM(EntityPM, EntityPOCO);
                        if (false)
                        {
                            EntityPM = GetEntityPM(EntityPOCO);
                            if (getComposition)
                            {
                                GetComposition(entityKeys, EntityPM);
                            }
                        }
                        EntityPM = GetEntityPM(EntityPOCO, getComposition, entityKeys);
                    }
                    if (EntityPM != null)
                    {
                        CacheManager.CacheWrapper.Insert(entityKeyString, EntityPM);
                    }
                    else
                    {
                        CacheManager.CacheWrapper.Insert(entityKeyString, new NullCache());
                    }
                }
//#endif
            }
            else
            {
                EntityPM = null;
                EntityPOCO = Repository.GetSingle(entityKeys);
                if (EntityPOCO != null)
                {
                    //EntityPM = new TEntityPM();
                    //mapping.CustomPOCOToPM(EntityPM, EntityPOCO);
                    //mapping.POCOToPM(EntityPM, EntityPOCO);
                    if (false)
                    {
                        EntityPM = GetEntityPM(EntityPOCO);
                        if (getComposition)
                        {
                            GetComposition(entityKeys, EntityPM);
                        }
                        mapping.CustomPOCOToPM(EntityPM, EntityPOCO);    
                    }
                    EntityPM = GetEntityPM(EntityPOCO, getComposition, entityKeys);    
                }       

                //TEntityPM securedPm = new TEntityPM();
                //SecuredMapping.GetMappedPM(EntityPM, securedPm, "Branch", tenant);
                return EntityPM;
            }
            return EntityPM;

        }

        public   TEntityPM GetEntityPM(TEntityPOCO entityPOCO, bool getComposition = false, TEntityKeys entityKeys = null)
        {
            var entityPM = new TEntityPM();
            //itzik please return null if (entityPOCO == null) return entityPM;
            if (entityPOCO == null)
            {
                return default(TEntityPM);
            } 
           // mapping.CustomPOCOToPM(entityPM, entityPOCO); composition need to be included in custom mapping mohammad.
            mapping.POCOToPM(entityPM, entityPOCO);
            if (getComposition)
            {
                GetComposition(entityKeys, entityPM);
            }
            mapping.CustomPOCOToPM(entityPM, entityPOCO);
            return entityPM;
        }
     
        public virtual void GetComposition(EntityKeyFields entityKeys,TEntityPM entityPM)
        {
        }

        protected abstract EntityKeyFields GetKeys(TEntityPOCO entityPOCO);

        //public List<TEntityPM> GetAll()
        //{
        //    var allPM = new List<TEntityPM>();
        //    allPM = Repository.GetAll().ToList().Select(poco => GetEntityPM(poco)).ToList();
        //    return allPM;
        //}

        public virtual void InitializeSettings()
        {

        }

    }




}
