 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.CRM.Data.Repsitories
{
   public partial class CallTypeRepository:IRepository<CallType>
   {
   
        private ICRMContext currentContext;
        public CallTypeRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public CallTypeRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  CallType GetSingle(string code)
        {
            return (from a in context.CallTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CallType> GetAll()
        {
            return from a in context.CallTypes  
                   select a;
        }
				 
        public CallType GetSingle(EntityKeyFields entityKeys)
        {
            CallTypeKeys keys = entityKeys as CallTypeKeys;
            return (from a in context.CallTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CallType entity)
        {
            onAdd();
            context.CallTypes.Add(entity);
        }

        public void Remove(CallType entity)
        {
            context.CallTypes.Attach(entity);
            context.CallTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CallType entity)
        {
            onUpdate();
            context.CallTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CallType> All()
        {
            return context.CallTypes.ToList();
        }

        private ICRMContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 