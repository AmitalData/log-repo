 
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
   public partial class PoaStatusTypeLookUpRepository:IRepository<PoaStatusTypeLookUp>
   {
   
        private ICustomContext currentContext;
        public PoaStatusTypeLookUpRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public PoaStatusTypeLookUpRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  PoaStatusTypeLookUp GetSingle(string code)
        {
            return (from a in context.PoaStatusTypeLookUps
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<PoaStatusTypeLookUp> GetAll()
        {
            return from a in context.PoaStatusTypeLookUps  
                   select a;
        }
				 
        public PoaStatusTypeLookUp GetSingle(EntityKeyFields entityKeys)
        {
            PoaStatusTypeLookUpKeys keys = entityKeys as PoaStatusTypeLookUpKeys;
            return (from a in context.PoaStatusTypeLookUps
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PoaStatusTypeLookUp entity)
        {
            onAdd();
            context.PoaStatusTypeLookUps.Add(entity);
        }

        public void Remove(PoaStatusTypeLookUp entity)
        {
            context.PoaStatusTypeLookUps.Attach(entity);
            context.PoaStatusTypeLookUps.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PoaStatusTypeLookUp entity)
        {
            onUpdate();
            context.PoaStatusTypeLookUps.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PoaStatusTypeLookUp> All()
        {
            return context.PoaStatusTypeLookUps.ToList();
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
	 