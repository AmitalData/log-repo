using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class EntityStatusRepository:IRepository<EntityStatus>
    {
        IWebFreightContext webFreightContext;
        public EntityStatusRepository()
        {
            webFreightContext = new WebFreightContext();
        }

        public EntityStatusRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }
        public EntityStatusRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public EntityStatus GetSingleEntityStatus(string id, int tenant)
        {
           
                EntityStatus entity   = (from a in context.EntityStatus.Include("ObjectTable").Include("EntityStatusType")
                                         where a.Tenant == tenant && a.Id == id
                                         select a).FirstOrDefault(); ;
                            
                return entity;
           

        }





        public EntityStatus GetSingleEntityStatusByCode(string code, int tenant)
        {
            EntityStatus entity = (from a in context.EntityStatus
                                   where a.Tenant == tenant && a.Code == code
                                   select a).FirstOrDefault(); 

            return entity;
        }

        public EntityStatus GetSingleEntityStatusByCodeTableId(string code, string objectTableId, int tenant)
        {
            EntityStatus entity = (from a in context.EntityStatus
                                   where a.Tenant == tenant && a.Code == code && a.ObjectTableId == objectTableId
                                   select a).FirstOrDefault();

            return entity;
        }


        public static EntityStatus GetSingleEntityStatusByCode(string code, int tenant, bool getFromCache)
        {
            if (!string.IsNullOrEmpty(code))
            {
                string entityName = "EntityStatus" + code + tenant;
                EntityStatus entity = null;
                if (getFromCache)
                {
                  
                        if (CacheManager.CacheWrapper.Get(entityName) == null)
                        {
                            IWebFreightContext context = WebFreightContext.GetContext(tenant);
                            var entitystatuses = (from a in context.EntityStatus
                                                  where a.Tenant == tenant
                                                  select a);
                            foreach (var s in entitystatuses)
                            {
                                string name = "EntityStatus" + s.Code + tenant;
                                if (CacheManager.CacheWrapper.Get(name) == null)
                                {
                                    CacheManager.CacheWrapper.Insert(name, s, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                                }
                            }
                            entity = (EntityStatus)CacheManager.CacheWrapper.Get(entityName);

                            //EntityStatus status = (from a in context.EntityStatus
                            //                         where a.Tenant == tenant && a.Id == id
                            //                         select a).FirstOrDefault();

                            //Entity = status;
                            //if (HttpContext.Current.Cache.Get(EntityName) == null)
                            //{
                            //    HttpContext.Current.Cache.Insert(EntityName, Entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            //}
                        }
                        else
                        {
                            entity = (EntityStatus)CacheManager.CacheWrapper.Get(entityName);
                            // HttpContext.Current.Cache.Insert(EntityNameValue, Entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    
        
                }
                else
                {
                    IWebFreightContext context = WebFreightContext.GetContext(tenant);
                    EntityStatus status = (from a in context.EntityStatus
                                           where a.Tenant == tenant && a.Code == code
                                           select a).FirstOrDefault();

                    entity = status;

                }
                return entity;
            }
            return null;

        }

        public static EntityStatus GetSingleEntityStatus(string id,int tenant,bool getFromCache)
        {
            if (!string.IsNullOrEmpty(id))
            {
                string entityName = "EntityStatus" + id + tenant;
                EntityStatus entity = null;
                if (getFromCache)
                {
                   
                        if (CacheManager.CacheWrapper.Get(entityName) == null)
                        {
                            IWebFreightContext context = WebFreightContext.GetContext(tenant);
                            var entitystatuses = (from a in context.EntityStatus
                                                  where a.Tenant == tenant
                                                  select a);
                            foreach (var s in entitystatuses)
                            {
                                string name = "EntityStatus" + s.Id + tenant;
                                if (CacheManager.CacheWrapper.Get(name) == null)
                                {
                                    CacheManager.CacheWrapper.Insert(name, s, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                                }
                            }
                            entity = (EntityStatus)CacheManager.CacheWrapper.Get(entityName);

                            //EntityStatus status = (from a in context.EntityStatus
                            //                         where a.Tenant == tenant && a.Id == id
                            //                         select a).FirstOrDefault();

                            //Entity = status;
                            //if (HttpContext.Current.Cache.Get(EntityName) == null)
                            //{
                            //    HttpContext.Current.Cache.Insert(EntityName, Entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                            //}
                        }
                        else
                        {
                            entity = (EntityStatus)CacheManager.CacheWrapper.Get(entityName);
                            // HttpContext.Current.Cache.Insert(EntityNameValue, Entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    
              
                }
                else
                {
                    IWebFreightContext context = WebFreightContext.GetContext(tenant);
                    EntityStatus status = (from a in context.EntityStatus
                                           where a.Tenant == tenant && a.Id == id
                                           select a).FirstOrDefault();

                    entity = status;
 
                }
                return entity;
            }
            return null;
          
        }

        public EntityStatus GetSingleEntityStatusByName(string name, int tenant,string objectTableId)
        {
          
            return (from a in context.EntityStatus
                    where a.Tenant == tenant && a.Name == name && a.ObjectTableId == objectTableId
                    select a).FirstOrDefault();
        }




        public string  GetEntityStatusArrivals(int tenant)
        {
            return (from a in context.EntityStatus
                    where a.Tenant == tenant && a.Code == "SARR"
                    select a.Id).FirstOrDefault();
        }



        public IQueryable<EntityStatus> GetEntityStatusByTenant(int tenant)
        {
            return (from a in context.EntityStatus
                    where a.Tenant == tenant
                    select a);
        }


        public IQueryable<EntityStatus> GetEntityStatus(int tenant)
        {
            return (from a in context.EntityStatus.Include("ObjectTable")
                    where a.Tenant == tenant && !a.InActive
                    select a);
        }

        


        public IQueryable<EntityStatus> GetEntityStatusByTenantAndObjectTableId(int tenant, string objectTableId)
        {
            return (from a in context.EntityStatus
                    where a.Tenant == tenant && a.ObjectTableId == objectTableId
                    select a);
        }
     
        public void Add(EntityStatus entity)
        {
            context.EntityStatus.Add(entity);
        }

        public void Remove(EntityStatus entity)
        {
            context.EntityStatus.Attach(entity);
            context.EntityStatus.Remove(entity);
        }

        public void Update(EntityStatus entity)
        {
            try
            {
                context.EntityStatus.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<EntityStatus> All()
        {
            return context.EntityStatus.ToList();
        }

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<EntityStatus> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public EntityStatus GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}