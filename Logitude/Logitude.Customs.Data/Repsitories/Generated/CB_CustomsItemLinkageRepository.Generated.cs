 
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
   public partial class CB_CustomsItemLinkageRepository:IRepository<CB_CustomsItemLinkage>
   {
   
        private ICustomContext currentContext;
        public CB_CustomsItemLinkageRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CB_CustomsItemLinkageRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CB_CustomsItemLinkage GetSingle(string cb_id)
        {
            return (from a in context.CB_CustomsItemLinkages
                    where a.CB_ID == cb_id 
                    select a).FirstOrDefault();
        }

        public IQueryable<CB_CustomsItemLinkage> GetAll()
        {
            return from a in context.CB_CustomsItemLinkages  
                   select a;
        }
				 
        public CB_CustomsItemLinkage GetSingle(EntityKeyFields entityKeys)
        {
            CB_CustomsItemLinkageKeys keys = entityKeys as CB_CustomsItemLinkageKeys;
            return (from a in context.CB_CustomsItemLinkages
                    where a.CB_ID == keys.CB_ID
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CB_CustomsItemLinkage entity)
        {
            onAdd();
            context.CB_CustomsItemLinkages.Add(entity);
        }

        public void Remove(CB_CustomsItemLinkage entity)
        {
            context.CB_CustomsItemLinkages.Attach(entity);
            context.CB_CustomsItemLinkages.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CB_CustomsItemLinkage entity)
        {
            onUpdate();
            context.CB_CustomsItemLinkages.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CB_CustomsItemLinkage> All()
        {
            return context.CB_CustomsItemLinkages.ToList();
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
	 