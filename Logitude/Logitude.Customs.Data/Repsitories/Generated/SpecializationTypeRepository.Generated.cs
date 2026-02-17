 
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
   public partial class SpecializationTypeRepository:IRepository<SpecializationType>
   {
   
        private ICustomContext currentContext;
        public SpecializationTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SpecializationTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SpecializationType GetSingle(string code)
        {
            return (from a in context.SpecializationTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<SpecializationType> GetAll()
        {
            return from a in context.SpecializationTypes  
                   select a;
        }
				 
        public SpecializationType GetSingle(EntityKeyFields entityKeys)
        {
            SpecializationTypeKeys keys = entityKeys as SpecializationTypeKeys;
            return (from a in context.SpecializationTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SpecializationType entity)
        {
            onAdd();
            context.SpecializationTypes.Add(entity);
        }

        public void Remove(SpecializationType entity)
        {
            context.SpecializationTypes.Attach(entity);
            context.SpecializationTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SpecializationType entity)
        {
            onUpdate();
            context.SpecializationTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SpecializationType> All()
        {
            return context.SpecializationTypes.ToList();
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
	 