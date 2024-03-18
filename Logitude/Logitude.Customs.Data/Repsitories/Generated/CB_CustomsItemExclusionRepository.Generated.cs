 
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
   public partial class CB_CustomsItemExclusionRepository:IRepository<CB_CustomsItemExclusion>
   {
   
        private ICustomContext currentContext;
        public CB_CustomsItemExclusionRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CB_CustomsItemExclusionRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CB_CustomsItemExclusion GetSingle(int id)
        {
            return (from a in context.CB_CustomsItemExclusion
                    where a.ID == id 
                    select a).FirstOrDefault();
        }

        public IQueryable<CB_CustomsItemExclusion> GetAll()
        {
            return from a in context.CB_CustomsItemExclusion  
                   select a;
        }
				 
        public CB_CustomsItemExclusion GetSingle(EntityKeyFields entityKeys)
        {
            CB_CustomsItemExclusionKeys keys = entityKeys as CB_CustomsItemExclusionKeys;
            return (from a in context.CB_CustomsItemExclusion
                    where a.ID == keys.ID
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CB_CustomsItemExclusion entity)
        {
            onAdd();
            context.CB_CustomsItemExclusion.Add(entity);
        }

        public void Remove(CB_CustomsItemExclusion entity)
        {
            context.CB_CustomsItemExclusion.Attach(entity);
            context.CB_CustomsItemExclusion.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CB_CustomsItemExclusion entity)
        {
            onUpdate();
            context.CB_CustomsItemExclusion.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CB_CustomsItemExclusion> All()
        {
            return context.CB_CustomsItemExclusion.ToList();
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
	 