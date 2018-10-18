 
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

namespace Logitude.Customs.Data.Repsitories
{
   public partial class ProceduralFaultsConnEntityRepository:IRepository<ProceduralFaultsConnEntity>
   {
   
        private ICustomContext currentContext;
        public ProceduralFaultsConnEntityRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ProceduralFaultsConnEntityRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ProceduralFaultsConnEntity GetSingle(string id, int tenant)
        {
            return (from a in context.ProceduralFaultsConnEntities
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ProceduralFaultsConnEntity> GetAll(int tenant)
        {
            return from a in context.ProceduralFaultsConnEntities  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ProceduralFaultsConnEntity GetSingle(EntityKeyFields entityKeys)
        {
            ProceduralFaultsConnEntityKeys keys = entityKeys as ProceduralFaultsConnEntityKeys;
            return (from a in context.ProceduralFaultsConnEntities
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ProceduralFaultsConnEntity entity)
        {
            onAdd();
            context.ProceduralFaultsConnEntities.Add(entity);
        }

        public void Remove(ProceduralFaultsConnEntity entity)
        {
            context.ProceduralFaultsConnEntities.Attach(entity);
            context.ProceduralFaultsConnEntities.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ProceduralFaultsConnEntity entity)
        {
            onUpdate();
            context.ProceduralFaultsConnEntities.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ProceduralFaultsConnEntity> All()
        {
            return context.ProceduralFaultsConnEntities.ToList();
        }

        private ICustomContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 