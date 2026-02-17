 
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
   public partial class TaxWithholdingAssessOfficeRepository:IRepository<TaxWithholdingAssessOffice>
   {
   
        private IAccountingContext currentContext;
        public TaxWithholdingAssessOfficeRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public TaxWithholdingAssessOfficeRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  TaxWithholdingAssessOffice GetSingle(string id, int tenant)
        {
            return (from a in context.TaxWithholdingAssessOffices
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<TaxWithholdingAssessOffice> GetAll(int tenant)
        {
            return from a in context.TaxWithholdingAssessOffices  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public TaxWithholdingAssessOffice GetSingle(EntityKeyFields entityKeys)
        {
            TaxWithholdingAssessOfficeKeys keys = entityKeys as TaxWithholdingAssessOfficeKeys;
            return (from a in context.TaxWithholdingAssessOffices
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TaxWithholdingAssessOffice entity)
        {
            onAdd();
            context.TaxWithholdingAssessOffices.Add(entity);
        }

        public void Remove(TaxWithholdingAssessOffice entity)
        {
            context.TaxWithholdingAssessOffices.Attach(entity);
            context.TaxWithholdingAssessOffices.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TaxWithholdingAssessOffice entity)
        {
            onUpdate();
            context.TaxWithholdingAssessOffices.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TaxWithholdingAssessOffice> All()
        {
            return context.TaxWithholdingAssessOffices.ToList();
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
	 