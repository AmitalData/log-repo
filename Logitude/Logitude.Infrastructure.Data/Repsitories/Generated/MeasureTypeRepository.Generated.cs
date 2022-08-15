 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Infrastructure.Data.Repsitories
{
   public partial class MeasureTypeRepository:IRepository<MeasureType>
   {
   
        private IInfrastructureContext currentContext;
        public MeasureTypeRepository(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public MeasureTypeRepository(IInfrastructureContext context)
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

        private IInfrastructureContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 