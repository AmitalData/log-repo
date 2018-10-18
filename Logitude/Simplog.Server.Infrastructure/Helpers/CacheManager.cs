using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Simplog.Server.Infrastructure.Helpers
{
    public static class CacheManager
    {
        private static ICacheWrapper cacheWrapper;
        public static ICacheWrapper CacheWrapper 
        {
            get { return cacheWrapper; }
            set
            {
                if (cacheWrapper != null) return; //only set once 
                cacheWrapper = value;
            }
        }
        public static int ClearCacheItems(Func<string,bool> pattrenFunc = null)
        {
            int clearItem = 0;
            var enumerator = CacheManager.CacheWrapper.GetEnumerator();

            while (enumerator.MoveNext())
            {
                try
                {
                    string key = enumerator.Key.ToString();
                    bool toClear = true;
                    if (pattrenFunc != null)
                    {
                        toClear = pattrenFunc(key);
                    }
                    if (toClear)
                    {
                        CacheManager.CacheWrapper.Remove(key);
                        clearItem++;
                    }
                }
                catch (Exception)
                {

                    ///throw;
                }
                
            }
            return clearItem;
        }
        public static TEntity GetOrInsertNewObject<TEntity>(string entityKeyString, Func<TEntity> GetNewObject, bool fromCache = true, bool donotCacheNull = false, bool supressForceInsert = true) //Itzik Test
            where TEntity : class ///,new()
            
        {
            if (CacheManager.CacheWrapper== null)//unitest  CacheManager.CacheWrapper is null!!!
            {
                throw new Exception("please int  CacheManager.CacheWrapper if unitest try like  JournalValidator.OverrideITextCodeTranslator ");

                GetNewObject(); ;
            }
            ///string entityKeyString = entityKeys.GetEntityPMName() + "_" + entityKeys.GetFullKey();
            TEntity EntityPM = null;
            object cacheObj=null ;//= CacheManager.CacheWrapper.Get(entityKeyString);
            if (supressForceInsert)
            {
                cacheObj = CacheManager.CacheWrapper.Get(entityKeyString);
            }

            if (cacheObj != null)
            {
                EntityPM = cacheObj as TEntity;
            }
            else
            {
                
                EntityPM = GetNewObject();
                if (EntityPM != null)
                {
                    CacheManager.CacheWrapper.Insert(entityKeyString, EntityPM);
                }
                else
                {
                    if (!donotCacheNull)
                    {
                    CacheManager.CacheWrapper.Insert(entityKeyString, new NullCache());
                }
            }
            }
            return EntityPM;
        }

    }
}
