 
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
   public partial class CB_CustomsBookAdditionRepository:IRepository<CB_CustomsBookAddition>
   {
   
        private ICustomContext currentContext;
        public CB_CustomsBookAdditionRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CB_CustomsBookAdditionRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CB_CustomsBookAddition GetSingle(string cb_id)
        {
            return (from a in context.CB_CustomsBookAdditions
                    where a.CB_ID == cb_id 
                    select a).FirstOrDefault();
        }

        public IQueryable<CB_CustomsBookAddition> GetAll()
        {
            return from a in context.CB_CustomsBookAdditions  
                   select a;
        }
				 
        public CB_CustomsBookAddition GetSingle(EntityKeyFields entityKeys)
        {
            CB_CustomsBookAdditionKeys keys = entityKeys as CB_CustomsBookAdditionKeys;
            return (from a in context.CB_CustomsBookAdditions
                    where a.CB_ID == keys.CB_ID
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CB_CustomsBookAddition entity)
        {
            onAdd();
            context.CB_CustomsBookAdditions.Add(entity);
        }

        public void Remove(CB_CustomsBookAddition entity)
        {
            context.CB_CustomsBookAdditions.Attach(entity);
            context.CB_CustomsBookAdditions.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CB_CustomsBookAddition entity)
        {
            onUpdate();
            context.CB_CustomsBookAdditions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CB_CustomsBookAddition> All()
        {
            return context.CB_CustomsBookAdditions.ToList();
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
	 