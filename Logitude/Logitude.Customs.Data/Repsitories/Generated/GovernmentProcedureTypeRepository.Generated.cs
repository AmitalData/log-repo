 
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
   public partial class GovernmentProcedureTypeRepository:IRepository<GovernmentProcedureType>
   {
   
        private ICustomContext currentContext;
        public GovernmentProcedureTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public GovernmentProcedureTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  GovernmentProcedureType GetSingle(string code)
        {
            return (from a in context.GovernmentProcedureTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<GovernmentProcedureType> GetAll()
        {
            return from a in context.GovernmentProcedureTypes  
                   select a;
        }
				 
        public GovernmentProcedureType GetSingle(EntityKeyFields entityKeys)
        {
            GovernmentProcedureTypeKeys keys = entityKeys as GovernmentProcedureTypeKeys;
            return (from a in context.GovernmentProcedureTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(GovernmentProcedureType entity)
        {
            onAdd();
            context.GovernmentProcedureTypes.Add(entity);
        }

        public void Remove(GovernmentProcedureType entity)
        {
            context.GovernmentProcedureTypes.Attach(entity);
            context.GovernmentProcedureTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(GovernmentProcedureType entity)
        {
            onUpdate();
            context.GovernmentProcedureTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<GovernmentProcedureType> All()
        {
            return context.GovernmentProcedureTypes.ToList();
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
	 