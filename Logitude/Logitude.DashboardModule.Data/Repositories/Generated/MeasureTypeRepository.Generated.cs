 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.DashboardModule.Data.EntityPOCOs;
using Logitude.DashboardModule.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.DashboardModule.Data.Repositories
{
   public partial class MeasureTypeRepository:IRepository<MeasureType>
   {
   
        private IDashboardContext currentContext;
        public MeasureTypeRepository(int tenant)
        {
            currentContext = DashboardContext.GetContext(tenant);
        }

        public MeasureTypeRepository(IDashboardContext context)
        {
            currentContext = context;
        }

		 
		
		public  MeasureType GetSingle(string code)
        {
            return (from a in context.MeasureTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<MeasureType> GetAll()
        {
            return from a in context.MeasureTypes  
                   select a;
        }
				 
        public MeasureType GetSingle(EntityKeyFields entityKeys)
        {
            MeasureTypeKeys keys = entityKeys as MeasureTypeKeys;
            return (from a in context.MeasureTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(MeasureType entity)
        {
            onAdd();
            context.MeasureTypes.Add(entity);
        }

        public void Remove(MeasureType entity)
        {
            context.MeasureTypes.Attach(entity);
            context.MeasureTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(MeasureType entity)
        {
            onUpdate();
            context.MeasureTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<MeasureType> All()
        {
            return context.MeasureTypes.ToList();
        }

        private IDashboardContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 