 
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
   public partial class FaultInspectionTypeRepository:IRepository<FaultInspectionType>
   {
   
        private ICustomContext currentContext;
        public FaultInspectionTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public FaultInspectionTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  FaultInspectionType GetSingle(string code)
        {
            return (from a in context.FaultInspectionTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<FaultInspectionType> GetAll()
        {
            return from a in context.FaultInspectionTypes  
                   select a;
        }
				 
        public FaultInspectionType GetSingle(EntityKeyFields entityKeys)
        {
            FaultInspectionTypeKeys keys = entityKeys as FaultInspectionTypeKeys;
            return (from a in context.FaultInspectionTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(FaultInspectionType entity)
        {
            onAdd();
            context.FaultInspectionTypes.Add(entity);
        }

        public void Remove(FaultInspectionType entity)
        {
            context.FaultInspectionTypes.Attach(entity);
            context.FaultInspectionTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(FaultInspectionType entity)
        {
            onUpdate();
            context.FaultInspectionTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<FaultInspectionType> All()
        {
            return context.FaultInspectionTypes.ToList();
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
	 