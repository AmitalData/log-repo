 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Accounting.Data.Repositories
{
   public partial class ExternalPageAdditionalDataRepository:IRepository<ExternalPageAdditionalData>
   {
   
        private IAccountingContext currentContext;
        public ExternalPageAdditionalDataRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public ExternalPageAdditionalDataRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  ExternalPageAdditionalData GetSingle(string objecttableid, string entityid, int tenant)
        {
            return (from a in context.ExternalPageAdditionalDatas
                    where a.ObjectTableId == objecttableid && a.EntityId == entityid && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ExternalPageAdditionalData> GetAll(int tenant)
        {
            return from a in context.ExternalPageAdditionalDatas  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ExternalPageAdditionalData GetSingle(EntityKeyFields entityKeys)
        {
            ExternalPageAdditionalDataKeys keys = entityKeys as ExternalPageAdditionalDataKeys;
            return (from a in context.ExternalPageAdditionalDatas
                    where a.ObjectTableId == keys.ObjectTableId && a.EntityId == keys.EntityId
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ExternalPageAdditionalData entity)
        {
            onAdd();
            context.ExternalPageAdditionalDatas.Add(entity);
        }

        public void Remove(ExternalPageAdditionalData entity)
        {
            context.ExternalPageAdditionalDatas.Attach(entity);
            context.ExternalPageAdditionalDatas.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ExternalPageAdditionalData entity)
        {
            onUpdate();
            context.ExternalPageAdditionalDatas.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ExternalPageAdditionalData> All()
        {
            return context.ExternalPageAdditionalDatas.ToList();
        }

        private IAccountingContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 