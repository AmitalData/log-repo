 
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
   public partial class TaxReportLineTypeRepository:IRepository<TaxReportLineType>
   {
   
        private IAccountingContext currentContext;
        public TaxReportLineTypeRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public TaxReportLineTypeRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  TaxReportLineType GetSingle(string code)
        {
            return (from a in context.TaxReportLineTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<TaxReportLineType> GetAll()
        {
            return from a in context.TaxReportLineTypes  
                   select a;
        }
				 
        public TaxReportLineType GetSingle(EntityKeyFields entityKeys)
        {
            TaxReportLineTypeKeys keys = entityKeys as TaxReportLineTypeKeys;
            return (from a in context.TaxReportLineTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TaxReportLineType entity)
        {
            onAdd();
            context.TaxReportLineTypes.Add(entity);
        }

        public void Remove(TaxReportLineType entity)
        {
            context.TaxReportLineTypes.Attach(entity);
            context.TaxReportLineTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TaxReportLineType entity)
        {
            onUpdate();
            context.TaxReportLineTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TaxReportLineType> All()
        {
            return context.TaxReportLineTypes.ToList();
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
	 