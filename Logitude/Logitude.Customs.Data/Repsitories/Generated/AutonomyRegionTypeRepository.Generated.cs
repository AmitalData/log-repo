 
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
   public partial class AutonomyRegionTypeRepository:IRepository<AutonomyRegionType>
   {
   
        private ICustomContext currentContext;
        public AutonomyRegionTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public AutonomyRegionTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  AutonomyRegionType GetSingle(string code)
        {
            return (from a in context.AutonomyRegionTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<AutonomyRegionType> GetAll()
        {
            return from a in context.AutonomyRegionTypes  
                   select a;
        }
				 
        public AutonomyRegionType GetSingle(EntityKeyFields entityKeys)
        {
            AutonomyRegionTypeKeys keys = entityKeys as AutonomyRegionTypeKeys;
            return (from a in context.AutonomyRegionTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(AutonomyRegionType entity)
        {
            onAdd();
            context.AutonomyRegionTypes.Add(entity);
        }

        public void Remove(AutonomyRegionType entity)
        {
            context.AutonomyRegionTypes.Attach(entity);
            context.AutonomyRegionTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(AutonomyRegionType entity)
        {
            onUpdate();
            context.AutonomyRegionTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AutonomyRegionType> All()
        {
            return context.AutonomyRegionTypes.ToList();
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
	 