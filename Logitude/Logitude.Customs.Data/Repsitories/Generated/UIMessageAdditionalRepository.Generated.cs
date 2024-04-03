 
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
   public partial class UIMessageAdditionalRepository:IRepository<UIMessageAdditional>
   {
   
        private ICustomContext currentContext;
        public UIMessageAdditionalRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public UIMessageAdditionalRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  UIMessageAdditional GetSingle(string id, string code, int tenant)
        {
            return (from a in context.UIMessageAdditionals
                    where a.Id == id && a.Code == code && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<UIMessageAdditional> GetAll(int tenant)
        {
            return from a in context.UIMessageAdditionals  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public UIMessageAdditional GetSingle(EntityKeyFields entityKeys)
        {
            UIMessageAdditionalKeys keys = entityKeys as UIMessageAdditionalKeys;
            return (from a in context.UIMessageAdditionals
                    where a.Id == keys.Id && a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(UIMessageAdditional entity)
        {
            onAdd();
            context.UIMessageAdditionals.Add(entity);
        }

        public void Remove(UIMessageAdditional entity)
        {
            context.UIMessageAdditionals.Attach(entity);
            context.UIMessageAdditionals.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(UIMessageAdditional entity)
        {
            onUpdate();
            context.UIMessageAdditionals.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<UIMessageAdditional> All()
        {
            return context.UIMessageAdditionals.ToList();
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
	 