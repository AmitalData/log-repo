 
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
   public partial class MeasurmentUnitRepository:IRepository<MeasurmentUnit>
   {
   
        private ICustomContext currentContext;
        public MeasurmentUnitRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public MeasurmentUnitRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  MeasurmentUnit GetSingle(string code)
        {
            return (from a in context.MeasurmentUnits
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<MeasurmentUnit> GetAll()
        {
            return from a in context.MeasurmentUnits  
                   select a;
        }
				 
        public MeasurmentUnit GetSingle(EntityKeyFields entityKeys)
        {
            MeasurmentUnitKeys keys = entityKeys as MeasurmentUnitKeys;
            return (from a in context.MeasurmentUnits
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(MeasurmentUnit entity)
        {
            onAdd();
            context.MeasurmentUnits.Add(entity);
        }

        public void Remove(MeasurmentUnit entity)
        {
            context.MeasurmentUnits.Attach(entity);
            context.MeasurmentUnits.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(MeasurmentUnit entity)
        {
            onUpdate();
            context.MeasurmentUnits.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<MeasurmentUnit> All()
        {
            return context.MeasurmentUnits.ToList();
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
	 