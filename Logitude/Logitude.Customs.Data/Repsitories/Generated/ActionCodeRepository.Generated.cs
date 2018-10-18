 
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
   public partial class ActionCodeRepository:IRepository<ActionCode>
   {
   
        private ICustomContext currentContext;
        public ActionCodeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ActionCodeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ActionCode GetSingle(string code)
        {
            return (from a in context.ActionCodes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ActionCode> GetAll()
        {
            return from a in context.ActionCodes  
                   select a;
        }
				 
        public ActionCode GetSingle(EntityKeyFields entityKeys)
        {
            ActionCodeKeys keys = entityKeys as ActionCodeKeys;
            return (from a in context.ActionCodes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ActionCode entity)
        {
            onAdd();
            context.ActionCodes.Add(entity);
        }

        public void Remove(ActionCode entity)
        {
            context.ActionCodes.Attach(entity);
            context.ActionCodes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ActionCode entity)
        {
            onUpdate();
            context.ActionCodes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ActionCode> All()
        {
            return context.ActionCodes.ToList();
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
	 