 
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
   public partial class ClaimEntityRepository:IRepository<ClaimEntity>
   {
   
        private ICustomContext currentContext;
        public ClaimEntityRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ClaimEntityRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ClaimEntity GetSingle(string code)
        {
            return (from a in context.ClaimEntities
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ClaimEntity> GetAll()
        {
            return from a in context.ClaimEntities  
                   select a;
        }
				 
        public ClaimEntity GetSingle(EntityKeyFields entityKeys)
        {
            ClaimEntityKeys keys = entityKeys as ClaimEntityKeys;
            return (from a in context.ClaimEntities
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ClaimEntity entity)
        {
            onAdd();
            context.ClaimEntities.Add(entity);
        }

        public void Remove(ClaimEntity entity)
        {
            context.ClaimEntities.Attach(entity);
            context.ClaimEntities.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ClaimEntity entity)
        {
            onUpdate();
            context.ClaimEntities.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ClaimEntity> All()
        {
            return context.ClaimEntities.ToList();
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
	 