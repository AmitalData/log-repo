 
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
   public partial class CB_TariffComputedDataRepository:IRepository<CB_TariffComputedData>
   {
   
        private ICustomContext currentContext;
        public CB_TariffComputedDataRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CB_TariffComputedDataRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CB_TariffComputedData GetSingle(string cb_id)
        {
            return (from a in context.CB_TariffComputedDatas
                    where a.CB_ID == cb_id 
                    select a).FirstOrDefault();
        }

        public IQueryable<CB_TariffComputedData> GetAll()
        {
            return from a in context.CB_TariffComputedDatas  
                   select a;
        }
				 
        public CB_TariffComputedData GetSingle(EntityKeyFields entityKeys)
        {
            CB_TariffComputedDataKeys keys = entityKeys as CB_TariffComputedDataKeys;
            return (from a in context.CB_TariffComputedDatas
                    where a.CB_ID == keys.CB_ID
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CB_TariffComputedData entity)
        {
            onAdd();
            context.CB_TariffComputedDatas.Add(entity);
        }

        public void Remove(CB_TariffComputedData entity)
        {
            context.CB_TariffComputedDatas.Attach(entity);
            context.CB_TariffComputedDatas.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CB_TariffComputedData entity)
        {
            onUpdate();
            context.CB_TariffComputedDatas.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CB_TariffComputedData> All()
        {
            return context.CB_TariffComputedDatas.ToList();
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
	 