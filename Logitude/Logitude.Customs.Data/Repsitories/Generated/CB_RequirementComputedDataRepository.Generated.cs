 
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
   public partial class CB_RequirementComputedDataRepository:IRepository<CB_RequirementComputedData>
   {
   
        private ICustomContext currentContext;
        public CB_RequirementComputedDataRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CB_RequirementComputedDataRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CB_RequirementComputedData GetSingle(string cb_id)
        {
            return (from a in context.CB_RequirementComputedDatas
                    where a.CB_ID == cb_id 
                    select a).FirstOrDefault();
        }

        public IQueryable<CB_RequirementComputedData> GetAll()
        {
            return from a in context.CB_RequirementComputedDatas  
                   select a;
        }
				 
        public CB_RequirementComputedData GetSingle(EntityKeyFields entityKeys)
        {
            CB_RequirementComputedDataKeys keys = entityKeys as CB_RequirementComputedDataKeys;
            return (from a in context.CB_RequirementComputedDatas
                    where a.CB_ID == keys.CB_ID
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CB_RequirementComputedData entity)
        {
            onAdd();
            context.CB_RequirementComputedDatas.Add(entity);
        }

        public void Remove(CB_RequirementComputedData entity)
        {
            context.CB_RequirementComputedDatas.Attach(entity);
            context.CB_RequirementComputedDatas.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CB_RequirementComputedData entity)
        {
            onUpdate();
            context.CB_RequirementComputedDatas.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CB_RequirementComputedData> All()
        {
            return context.CB_RequirementComputedDatas.ToList();
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
	 