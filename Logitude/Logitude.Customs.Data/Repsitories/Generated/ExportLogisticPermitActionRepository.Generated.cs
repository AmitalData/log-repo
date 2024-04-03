 
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
   public partial class ExportLogisticPermitActionRepository:IRepository<ExportLogisticPermitAction>
   {
   
        private ICustomContext currentContext;
        public ExportLogisticPermitActionRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ExportLogisticPermitActionRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ExportLogisticPermitAction GetSingle(string code)
        {
            return (from a in context.ExportLogisticPermitActions
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ExportLogisticPermitAction> GetAll()
        {
            return from a in context.ExportLogisticPermitActions  
                   select a;
        }
				 
        public ExportLogisticPermitAction GetSingle(EntityKeyFields entityKeys)
        {
            ExportLogisticPermitActionKeys keys = entityKeys as ExportLogisticPermitActionKeys;
            return (from a in context.ExportLogisticPermitActions
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ExportLogisticPermitAction entity)
        {
            onAdd();
            context.ExportLogisticPermitActions.Add(entity);
        }

        public void Remove(ExportLogisticPermitAction entity)
        {
            context.ExportLogisticPermitActions.Attach(entity);
            context.ExportLogisticPermitActions.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ExportLogisticPermitAction entity)
        {
            onUpdate();
            context.ExportLogisticPermitActions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ExportLogisticPermitAction> All()
        {
            return context.ExportLogisticPermitActions.ToList();
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
	 