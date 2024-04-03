 
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
   public partial class SeizureMethodTypeRepository:IRepository<SeizureMethodType>
   {
   
        private ICustomContext currentContext;
        public SeizureMethodTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SeizureMethodTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SeizureMethodType GetSingle(string code)
        {
            return (from a in context.SeizureMethodTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<SeizureMethodType> GetAll()
        {
            return from a in context.SeizureMethodTypes  
                   select a;
        }
				 
        public SeizureMethodType GetSingle(EntityKeyFields entityKeys)
        {
            SeizureMethodTypeKeys keys = entityKeys as SeizureMethodTypeKeys;
            return (from a in context.SeizureMethodTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SeizureMethodType entity)
        {
            onAdd();
            context.SeizureMethodTypes.Add(entity);
        }

        public void Remove(SeizureMethodType entity)
        {
            context.SeizureMethodTypes.Attach(entity);
            context.SeizureMethodTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SeizureMethodType entity)
        {
            onUpdate();
            context.SeizureMethodTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SeizureMethodType> All()
        {
            return context.SeizureMethodTypes.ToList();
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
	 