 
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
   public partial class CB_CustomsItemComputedDataRepository:IRepository<CB_CustomsItemComputedData>
   {
   
        private ICustomContext currentContext;
        public CB_CustomsItemComputedDataRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CB_CustomsItemComputedDataRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CB_CustomsItemComputedData GetSingle(string cb_id)
        {
            return (from a in context.CB_CustomsItemComputedDatas
                    where a.CB_ID == cb_id 
                    select a).FirstOrDefault();
        }

        public IQueryable<CB_CustomsItemComputedData> GetAll()
        {
            return from a in context.CB_CustomsItemComputedDatas  
                   select a;
        }
				 
        public CB_CustomsItemComputedData GetSingle(EntityKeyFields entityKeys)
        {
            CB_CustomsItemComputedDataKeys keys = entityKeys as CB_CustomsItemComputedDataKeys;
            return (from a in context.CB_CustomsItemComputedDatas
                    where a.CB_ID == keys.CB_ID
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CB_CustomsItemComputedData entity)
        {
            onAdd();
            context.CB_CustomsItemComputedDatas.Add(entity);
        }

        public void Remove(CB_CustomsItemComputedData entity)
        {
            context.CB_CustomsItemComputedDatas.Attach(entity);
            context.CB_CustomsItemComputedDatas.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CB_CustomsItemComputedData entity)
        {
            onUpdate();
            context.CB_CustomsItemComputedDatas.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CB_CustomsItemComputedData> All()
        {
            return context.CB_CustomsItemComputedDatas.ToList();
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
	 