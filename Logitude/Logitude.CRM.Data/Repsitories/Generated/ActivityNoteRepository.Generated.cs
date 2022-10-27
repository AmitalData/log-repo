 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.CRM.Data.Repsitories
{
   public partial class ActivityNoteRepository:IRepository<ActivityNote>
   {
   
        private ICRMContext currentContext;
        public ActivityNoteRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public ActivityNoteRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  ActivityNote GetSingle(string id, int tenant)
        {
            return (from a in context.ActivityNotes
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ActivityNote> GetAll(int tenant)
        {
            return from a in context.ActivityNotes  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ActivityNote GetSingle(EntityKeyFields entityKeys)
        {
            ActivityNoteKeys keys = entityKeys as ActivityNoteKeys;
            return (from a in context.ActivityNotes
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ActivityNote entity)
        {
            onAdd();
            context.ActivityNotes.Add(entity);
        }

        public void Remove(ActivityNote entity)
        {
            context.ActivityNotes.Attach(entity);
            context.ActivityNotes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ActivityNote entity)
        {
            onUpdate();
            context.ActivityNotes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ActivityNote> All()
        {
            return context.ActivityNotes.ToList();
        }

        private ICRMContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 