 
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
   public partial class CB_PreferenceRepository:IRepository<CB_Preference>
   {
   
        private ICustomContext currentContext;
        public CB_PreferenceRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CB_PreferenceRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CB_Preference GetSingle(string id, int tenant)
        {
            return (from a in context.CB_Preferences
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CB_Preference> GetAll(int tenant)
        {
            return from a in context.CB_Preferences  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CB_Preference GetSingle(EntityKeyFields entityKeys)
        {
            CB_PreferenceKeys keys = entityKeys as CB_PreferenceKeys;
            return (from a in context.CB_Preferences
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CB_Preference entity)
        {
            onAdd();
            context.CB_Preferences.Add(entity);
        }

        public void Remove(CB_Preference entity)
        {
            context.CB_Preferences.Attach(entity);
            context.CB_Preferences.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CB_Preference entity)
        {
            onUpdate();
            context.CB_Preferences.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CB_Preference> All()
        {
            return context.CB_Preferences.ToList();
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
	 