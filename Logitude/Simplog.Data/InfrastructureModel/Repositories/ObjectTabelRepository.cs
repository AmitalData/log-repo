using System;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;
using System.Web;
using Simplog.Data.InfrastructureModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System.Transactions;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class ObjectTableRepository:IRepository<ObjectTable>, Simplog.Data.InfrastructureModel.Repositories.IObjectTableRepository
    {
         IWebFreightContext webFreightContext;
        public static  int tenantId = 0;


        public ObjectTableRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public ObjectTableRepository()
        {
               //Context=new WebFreightContext(); 
        }

        public ObjectTableRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
            tenantId = tenant;
        }

		public bool IsObjectTableType(string objectTableId, string type, bool getFromCache = true)
		{
			string cacheKey = $"ObjectTable{type}{objectTableId}";

			if (getFromCache)
			{
				var cachedValue = CacheManager.CacheWrapper.Get(cacheKey);
				if (cachedValue != null)
				{
					return (bool)cachedValue;
				}
			}
			bool result = context.ObjectTables.Any(d => d.Id == objectTableId && d.Name == type);
			if (getFromCache)
			{
				CacheManager.CacheWrapper.Insert(cacheKey, result, null, DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
			}
			return result;
		}
		public bool IsObjectTableMaster(string objectTableId, bool getFromCache = true)
		{
			return IsObjectTableType(objectTableId, "Master", getFromCache);
		}
		public bool IsObjectTableShipment(string objectTableId, bool getFromCache = true)
		{
			return IsObjectTableType(objectTableId, "Shipment", getFromCache);
		}

		public IQueryable<ObjectTable> GetObjects()
        {
            return context.ObjectTables;
        }

        public ObjectTable GetObjectTableByName(string name,int tenant,bool getFromCache)
        {
            ObjectTable entity;
            if (getFromCache)
            {
                string entityName = $"ObjectTable{name}_{tenant}_{SettingUtil.GetCurrentTenant()}";

                if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        entity = context.ObjectTables.Where(d => d.Name == name && (d.Tenant == tenant || d.Tenant == 0)).FirstOrDefault();

                        if (CacheManager.CacheWrapper.Get(entityName) == null)
                        {
                            if (entity != null)
                            {
                                CacheManager.CacheWrapper.Insert(entityName, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }
                    }

                    else
                    {
                        entity = (ObjectTable)CacheManager.CacheWrapper.Get(entityName);

                    }
                

              
            }

            else
            {
                entity = context.ObjectTables.Where(d => d.Name == name && (d.Tenant == tenant || d.Tenant == 0)).FirstOrDefault();
            }

            return entity;
        }

        public ObjectTable GetSingleObjectTable(string id, int tenant, bool getFromCache)
        {
            string entityName = $"ObjectTable{id}_{tenant}_{SettingUtil.GetCurrentTenant()}";
            ObjectTable entity;
            getFromCache = true;

			if (getFromCache)
            {
               
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        entity = context.ObjectTables.Where(d => d.Id == id && (d.Tenant == tenant || d.Tenant == 0)).FirstOrDefault();
                        if (CacheManager.CacheWrapper.Get(entityName) == null)
                        {
                            if (entity != null)
                            {
                                CacheManager.CacheWrapper.Insert(entityName, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }
                    }
                    else
                    {
                        entity = (ObjectTable)CacheManager.CacheWrapper.Get(entityName);
                    }
                
            
            }
            else
            {
                entity = context.ObjectTables.Include("FullNameTextCode").Where(d => d.Id == id && (d.Tenant == tenant || d.Tenant == 0)).FirstOrDefault();

            }
            return entity; 
        }

        public IQueryable<ObjectTable> GetObjectsByTenant(int tenant)
        {
            IQueryable<ObjectTable> objectTables = from a in context.ObjectTables
                                                   where a.Tenant == tenant || a.Tenant == 0
                                                   select a;
            return objectTables;
        }

        public IQueryable<ObjectTable> GetObjectsByTenantOrTenantZero(int tenant)
        {
            IQueryable<ObjectTable> objectTables = from a in context.ObjectTables
                                                   where a.Tenant == tenant || a.Tenant == 0
                                                   select a;
            return objectTables;
        }
   
        public ObjectTable GetObjectTableById(string id, int tenant)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string entityName = $"ObjectTable{id}_{tenant}_{SettingUtil.GetCurrentTenant()}";
                ObjectTable entity;

                if (CacheManager.CacheWrapper != null)
                {
                    if (CacheManager.CacheWrapper.Get(entityName) == null)
                    {
                        entity = context.ObjectTables.Where(d => d.Id == id && (d.Tenant == tenant || d.Tenant == 0)).FirstOrDefault();
                        if (CacheManager.CacheWrapper.Get(entityName) == null)
                        {
                            if (entity != null)
                            {
                                CacheManager.CacheWrapper.Insert(entityName, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            }
                        }
                    }
                    else
                    {
                        entity = (ObjectTable)CacheManager.CacheWrapper.Get(entityName);

                    }
                }
                else
                {
                    entity = context.ObjectTables.Where(d => d.Id == id && (d.Tenant == tenant || d.Tenant == 0)).FirstOrDefault();
                }

                return entity;
            }
            return null;
        }
      
        public ObjectTable GetFirstObjectTable(int tenant)
        {
            return (from a in context.ObjectTables
                    where a.Tenant == tenant || a.Tenant == 0
                    select a).FirstOrDefault();
        }

        public void Add(ObjectTable entity)
        {
            entity.LastUpdateDate = DateTime.Now;
            context.ObjectTables.Add(entity);
        }

        public void Remove(ObjectTable entity)
        {
            context.ObjectTables.Attach(entity);
            context.ObjectTables.Remove(entity);
        }

        public void Update(ObjectTable entity)
        {

            try
            {
                context.ObjectTables.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<ObjectTable> All()
        {
            return context.ObjectTables.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ObjectTable> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public ObjectTable GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public static string GetObjectTableByName(string objectTableName)
        {
            if (String.IsNullOrWhiteSpace(objectTableName)) return "";//not must 
            int tenant = SettingUtil.GetCurrentTenant();
            var objectTableRepository = new ObjectTableRepository(tenant); // ObjectTabelRepository tenant must be zero !!
            var objectTable = objectTableRepository.GetObjectTableByName(objectTableName,// "Customs.PhysicalCheck", 
                0, true);
            if(objectTable!=null)
            return objectTable.Id;
            return null;
        }

        public static ObjectTable GetSingleObjectTableById(string id, int tenant)
        {
            ObjectTable table = null;
            if (!string.IsNullOrEmpty(id))
            {
                table = GetObjectTablesWithTenantZero(tenant).Where(t => t.Id == id).FirstOrDefault();
            }

            return table;
        }

        public static List<ObjectTable> GetObjectTablesWithTenantZero(int tenant)
        {
            string listName = "tenantzerotextobjecttables";
            string tenantListName = "tenantobjecttables" + tenant;

            List<ObjectTable> result = new List<ObjectTable>();
            List<ObjectTable> currentTenantTables = new List<ObjectTable>();
            List<ObjectTable> zeroTenantTables = new List<ObjectTable>();

          



            if (tenant != 0)
            {
               
                    if (CacheManager.CacheWrapper.Get(tenantListName) == null)
                    {
                        using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                        {
                            IWebFreightContext context = WebFreightContext.GetContext(tenant);
                            currentTenantTables = (from a in context.ObjectTables//.Include("HeaderScreen").Include("DescriptionTextCode").Include("NewButtonTextCode")
                                                   where (a.Tenant == 0 && a.InActive == false)
                                                 select a).ToList();
                            scope.Complete();
                        }

                        CacheManager.CacheWrapper.Insert(tenantListName, currentTenantTables, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                    }
                    else
                    {
                        currentTenantTables = (List<ObjectTable>)CacheManager.CacheWrapper.Get(tenantListName);
                    }
                
       
            }

           
                if (CacheManager.CacheWrapper.Get(listName) == null)
                {
                    using (TransactionScope scope = TransactionFactory.GetNewTransaction())
                    {
                        IWebFreightContext context = WebFreightContext.GetContext(tenant);
                        zeroTenantTables = (from a in context.ObjectTables//.Include("HeaderScreen").Include("DescriptionTextCode").Include("NewButtonTextCode")
                                            where (a.Tenant == 0 && a.InActive == false)
                                            select a).ToList();

                        scope.Complete();
                    }


                    CacheManager.CacheWrapper.Insert(listName, zeroTenantTables, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                }
                else
                {
                    zeroTenantTables = (List<ObjectTable>)CacheManager.CacheWrapper.Get(listName);
                }
            
   
            zeroTenantTables = zeroTenantTables == null ? new List<ObjectTable>() : zeroTenantTables;
            currentTenantTables = currentTenantTables == null ? new List<ObjectTable>() : currentTenantTables;

            result = zeroTenantTables.Concat(currentTenantTables).ToList();

            return result;
        }

        public string GetObjectTableIdByName(string tablename)
        {
            return (from a in context.ObjectTables
                    where  a.Name == tablename
                    select a.Id).FirstOrDefault();
        }

        public string GetObjectTableIdByName(string tablename , int tenant)
        {
            return (from a in context.ObjectTables
                    where a.Name == tablename && (a.Tenant ==tenant || a.Tenant == 0)
                    select a.Id).FirstOrDefault();
        }


        public List<ObjectTable> GetAllCacheOnClient(int tenant)
        {
            

            IWebFreightContext context = WebFreightContext.GetContext(tenant);
            var q = (from a in context.ObjectTables//.Include("HeaderScreen").Include("DescriptionTextCode").Include("NewButtonTextCode")
                     where (a.Tenant == tenant && a.InActive == false && a.CacheOnClient == true)
                     select a);
            if (LogitudeSettings.IsCostomsDeploy)
            {
                ///select * From objecttables where name like 'Customs.%'
                q = q
                    .Where(r => r.Name != null)
                    .Where(r => r.Name.StartsWith("Customs."));
            }
            var currentTenantTables = q.ToList();
            return currentTenantTables;
        }

        public static string GetNameById(string id , int tenant)
        {
            IWebFreightContext webFreightContext = WebFreightContext.GetContext(tenant);
            return (from a in webFreightContext.ObjectTables
                    where a.Id == id
                    select a.Name).FirstOrDefault();
        }

        public static bool  IsApplyGenericCustomFields(string name , int tenant)
        {
            IWebFreightContext webFreightContext = WebFreightContext.GetContext(tenant);

            return (from a in webFreightContext.ObjectTables
                        where a.Name == name  && !a.IsCustom 
                        select a.ApplyGenericCustomFields).FirstOrDefault();
         
        }

    }














}
