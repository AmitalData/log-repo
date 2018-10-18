 
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
   public partial class ProcessingReasonRepository:IRepository<ProcessingReason>
   {
   
        private ICustomContext currentContext;
        public ProcessingReasonRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ProcessingReasonRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ProcessingReason GetSingle(string code)
        {
            return (from a in context.ProcessingReasons
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ProcessingReason> GetAll()
        {
            return from a in context.ProcessingReasons  
                   select a;
        }
				 
        public ProcessingReason GetSingle(EntityKeyFields entityKeys)
        {
            ProcessingReasonKeys keys = entityKeys as ProcessingReasonKeys;
            return (from a in context.ProcessingReasons
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ProcessingReason entity)
        {
            onAdd();
            context.ProcessingReasons.Add(entity);
        }

        public void Remove(ProcessingReason entity)
        {
            context.ProcessingReasons.Attach(entity);
            context.ProcessingReasons.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ProcessingReason entity)
        {
            onUpdate();
            context.ProcessingReasons.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ProcessingReason> All()
        {
            return context.ProcessingReasons.ToList();
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
	 