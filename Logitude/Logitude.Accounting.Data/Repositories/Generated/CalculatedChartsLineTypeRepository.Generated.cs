 
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
   public partial class CalculatedChartsLineTypeRepository:IRepository<CalculatedChartsLineType>
   {
   
        private IAccountingContext currentContext;
        public CalculatedChartsLineTypeRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public CalculatedChartsLineTypeRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  CalculatedChartsLineType GetSingle(string code)
        {
            return (from a in context.CalculatedChartsLineTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CalculatedChartsLineType> GetAll()
        {
            return from a in context.CalculatedChartsLineTypes  
                   select a;
        }
				 
        public CalculatedChartsLineType GetSingle(EntityKeyFields entityKeys)
        {
            CalculatedChartsLineTypeKeys keys = entityKeys as CalculatedChartsLineTypeKeys;
            return (from a in context.CalculatedChartsLineTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CalculatedChartsLineType entity)
        {
            onAdd();
            context.CalculatedChartsLineTypes.Add(entity);
        }

        public void Remove(CalculatedChartsLineType entity)
        {
            context.CalculatedChartsLineTypes.Attach(entity);
            context.CalculatedChartsLineTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CalculatedChartsLineType entity)
        {
            onUpdate();
            context.CalculatedChartsLineTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CalculatedChartsLineType> All()
        {
            return context.CalculatedChartsLineTypes.ToList();
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
	 