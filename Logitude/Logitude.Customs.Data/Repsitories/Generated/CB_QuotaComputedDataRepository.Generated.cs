 
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
   public partial class CB_QuotaComputedDataRepository:IRepository<CB_QuotaComputedData>
   {
   
        private ICustomContext currentContext;
        public CB_QuotaComputedDataRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CB_QuotaComputedDataRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CB_QuotaComputedData GetSingle(string cb_id)
        {
            return (from a in context.CB_QuotaComputedDatas
                    where a.CB_ID == cb_id 
                    select a).FirstOrDefault();
        }

        public IQueryable<CB_QuotaComputedData> GetAll()
        {
            return from a in context.CB_QuotaComputedDatas  
                   select a;
        }
				 
        public CB_QuotaComputedData GetSingle(EntityKeyFields entityKeys)
        {
            CB_QuotaComputedDataKeys keys = entityKeys as CB_QuotaComputedDataKeys;
            return (from a in context.CB_QuotaComputedDatas
                    where a.CB_ID == keys.CB_ID
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CB_QuotaComputedData entity)
        {
            onAdd();
            context.CB_QuotaComputedDatas.Add(entity);
        }

        public void Remove(CB_QuotaComputedData entity)
        {
            context.CB_QuotaComputedDatas.Attach(entity);
            context.CB_QuotaComputedDatas.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CB_QuotaComputedData entity)
        {
            onUpdate();
            context.CB_QuotaComputedDatas.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CB_QuotaComputedData> All()
        {
            return context.CB_QuotaComputedDatas.ToList();
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
	 