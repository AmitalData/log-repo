 
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
   public partial class SeizureFactorTypeRepository:IRepository<SeizureFactorType>
   {
   
        private ICustomContext currentContext;
        public SeizureFactorTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SeizureFactorTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SeizureFactorType GetSingle(string code)
        {
            return (from a in context.SeizureFactorTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<SeizureFactorType> GetAll()
        {
            return from a in context.SeizureFactorTypes  
                   select a;
        }
				 
        public SeizureFactorType GetSingle(EntityKeyFields entityKeys)
        {
            SeizureFactorTypeKeys keys = entityKeys as SeizureFactorTypeKeys;
            return (from a in context.SeizureFactorTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SeizureFactorType entity)
        {
            onAdd();
            context.SeizureFactorTypes.Add(entity);
        }

        public void Remove(SeizureFactorType entity)
        {
            context.SeizureFactorTypes.Attach(entity);
            context.SeizureFactorTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SeizureFactorType entity)
        {
            onUpdate();
            context.SeizureFactorTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SeizureFactorType> All()
        {
            return context.SeizureFactorTypes.ToList();
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
	 