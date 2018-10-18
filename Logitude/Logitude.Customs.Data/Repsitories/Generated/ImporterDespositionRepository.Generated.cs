 
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
   public partial class ImporterDespositionRepository:IRepository<ImporterDesposition>
   {
   
        private ICustomContext currentContext;
        public ImporterDespositionRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ImporterDespositionRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ImporterDesposition GetSingle(string id, int tenant)
        {
            return (from a in context.ImporterDespositions
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ImporterDesposition> GetAll(int tenant)
        {
            return from a in context.ImporterDespositions  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ImporterDesposition GetSingle(EntityKeyFields entityKeys)
        {
            ImporterDespositionKeys keys = entityKeys as ImporterDespositionKeys;
            return (from a in context.ImporterDespositions
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ImporterDesposition entity)
        {
            onAdd();
            context.ImporterDespositions.Add(entity);
        }

        public void Remove(ImporterDesposition entity)
        {
            context.ImporterDespositions.Attach(entity);
            context.ImporterDespositions.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ImporterDesposition entity)
        {
            onUpdate();
            context.ImporterDespositions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ImporterDesposition> All()
        {
            return context.ImporterDespositions.ToList();
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
	 