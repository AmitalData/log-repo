 
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
   public partial class CB_RuleRepository:IRepository<CB_Rule>
   {
   
        private ICustomContext currentContext;
        public CB_RuleRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CB_RuleRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CB_Rule GetSingle(int id)
        {
            return (from a in context.CB_Rules
                    where a.ID == id 
                    select a).FirstOrDefault();
        }

        public IQueryable<CB_Rule> GetAll()
        {
            return from a in context.CB_Rules  
                   select a;
        }
				 
        public CB_Rule GetSingle(EntityKeyFields entityKeys)
        {
            CB_RuleKeys keys = entityKeys as CB_RuleKeys;
            return (from a in context.CB_Rules
                    where a.ID == keys.ID
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CB_Rule entity)
        {
            onAdd();
            context.CB_Rules.Add(entity);
        }

        public void Remove(CB_Rule entity)
        {
            context.CB_Rules.Attach(entity);
            context.CB_Rules.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CB_Rule entity)
        {
            onUpdate();
            context.CB_Rules.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CB_Rule> All()
        {
            return context.CB_Rules.ToList();
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
	 