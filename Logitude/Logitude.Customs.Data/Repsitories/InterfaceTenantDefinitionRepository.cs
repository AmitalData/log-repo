 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Customs.Data.EntityPOCOs;
using Logitude.Customs.Data.EntityKeys;
using Simplog.Server.Infrastructure;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;

namespace Logitude.Customs.Data.Repsitories
{
   public partial class InterfaceTenantDefinitionRepository:IRepository<InterfaceTenantDefinition>
   {
        
		public List<InterfaceTenantDefinition> GetMulti(EntityKeyFields entityKeys)
        {
            
			throw new NotImplementedException();
        }

        public InterfaceTenantDefinition GetSingleDefinitionByCode(string code, int tenant, bool getFromCache = true)
        {
            if (!string.IsNullOrEmpty(code))
            {
                InterfaceTenantDefinition entity;
                if (getFromCache)
                {
                    string entityKeyString = $"GetSingleDefinitionByCode({code},{tenant})";
                    if (CacheManager.CacheWrapper.Get(entityKeyString) == null)
                    {

                        entity = (from record in context.InterfaceTenantDefinitions where record.Code == code && record.Tenant == tenant select record).FirstOrDefault();

                        if (CacheManager.CacheWrapper.Get(entityKeyString) == null && entity != null)
                        {
                            CacheManager.CacheWrapper.Insert(entityKeyString, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    }
                    else
                    {
                        entity = (InterfaceTenantDefinition)CacheManager.CacheWrapper.Get(entityKeyString);
                    }

                }
                else
                {
                    entity = (from a in context.InterfaceTenantDefinitions
                             where a.Code == code && a.Tenant == tenant
                             select a).FirstOrDefault();
                }
                return entity;
            }
            return null;
        }
        public InterfaceTenantDefinition GetSingle(EntityKeyFields entityKeys, bool getFromCache = true)
        {
            InterfaceTenantDefinitionKeys keys = entityKeys as InterfaceTenantDefinitionKeys;
            InterfaceTenantDefinition entity;
                if (getFromCache)
                {
                    string entityKeyString = $"InterfaceTenantDefinitionGetSingle({keys.Id})";
                    if (CacheManager.CacheWrapper.Get(entityKeyString) == null)
                    {

                        entity = (from record in context.InterfaceTenantDefinitions where record.Id == keys.Id select record).FirstOrDefault();

                        if (CacheManager.CacheWrapper.Get(entityKeyString) == null && entity != null)
                        {
                            CacheManager.CacheWrapper.Insert(entityKeyString, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    }
                    else
                    {
                        entity = (InterfaceTenantDefinition)CacheManager.CacheWrapper.Get(entityKeyString);
                    }

                }
                else
                {
                    entity = (from a in context.InterfaceTenantDefinitions
                              where a.Id == keys.Id
                              select a).FirstOrDefault();
                }
                return entity;
            
        }


    }

}
   