 
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
   public partial class AutonomyTypeRepository:IRepository<AutonomyType>
   {
   
        private ICustomContext currentContext;
        public AutonomyTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public AutonomyTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  AutonomyType GetSingle(string code)
        {
            return (from a in context.AutonomyTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<AutonomyType> GetAll()
        {
            return from a in context.AutonomyTypes  
                   select a;
        }
				 
        public AutonomyType GetSingle(EntityKeyFields entityKeys)
        {
            AutonomyTypeKeys keys = entityKeys as AutonomyTypeKeys;
            return (from a in context.AutonomyTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(AutonomyType entity)
        {
            onAdd();
            context.AutonomyTypes.Add(entity);
        }

        public void Remove(AutonomyType entity)
        {
            context.AutonomyTypes.Attach(entity);
            context.AutonomyTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(AutonomyType entity)
        {
            onUpdate();
            context.AutonomyTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AutonomyType> All()
        {
            return context.AutonomyTypes.ToList();
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
	 