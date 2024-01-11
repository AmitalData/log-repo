 
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
   public partial class ImporterPeriodicDeclarStatusRepository:IRepository<ImporterPeriodicDeclarStatus>
   {
   
        private ICustomContext currentContext;
        public ImporterPeriodicDeclarStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ImporterPeriodicDeclarStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ImporterPeriodicDeclarStatus GetSingle(string code)
        {
            return (from a in context.ImporterPeriodicDeclarStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ImporterPeriodicDeclarStatus> GetAll()
        {
            return from a in context.ImporterPeriodicDeclarStatuses  
                   select a;
        }
				 
        public ImporterPeriodicDeclarStatus GetSingle(EntityKeyFields entityKeys)
        {
            ImporterPeriodicDeclarStatusKeys keys = entityKeys as ImporterPeriodicDeclarStatusKeys;
            return (from a in context.ImporterPeriodicDeclarStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ImporterPeriodicDeclarStatus entity)
        {
            onAdd();
            context.ImporterPeriodicDeclarStatuses.Add(entity);
        }

        public void Remove(ImporterPeriodicDeclarStatus entity)
        {
            context.ImporterPeriodicDeclarStatuses.Attach(entity);
            context.ImporterPeriodicDeclarStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ImporterPeriodicDeclarStatus entity)
        {
            onUpdate();
            context.ImporterPeriodicDeclarStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ImporterPeriodicDeclarStatus> All()
        {
            return context.ImporterPeriodicDeclarStatuses.ToList();
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
	 