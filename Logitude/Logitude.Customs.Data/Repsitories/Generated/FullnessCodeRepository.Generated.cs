 
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
   public partial class FullnessCodeRepository:IRepository<FullnessCode>
   {
   
        private ICustomContext currentContext;
        public FullnessCodeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public FullnessCodeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  FullnessCode GetSingle(string code)
        {
            return (from a in context.FullnessCodes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<FullnessCode> GetAll()
        {
            return from a in context.FullnessCodes  
                   select a;
        }
				 
        public FullnessCode GetSingle(EntityKeyFields entityKeys)
        {
            FullnessCodeKeys keys = entityKeys as FullnessCodeKeys;
            return (from a in context.FullnessCodes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(FullnessCode entity)
        {
            onAdd();
            context.FullnessCodes.Add(entity);
        }

        public void Remove(FullnessCode entity)
        {
            context.FullnessCodes.Attach(entity);
            context.FullnessCodes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(FullnessCode entity)
        {
            onUpdate();
            context.FullnessCodes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<FullnessCode> All()
        {
            return context.FullnessCodes.ToList();
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
	 