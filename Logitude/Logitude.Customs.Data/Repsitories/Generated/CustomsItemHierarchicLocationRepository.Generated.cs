 
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
   public partial class CustomsItemHierarchicLocationRepository:IRepository<CustomsItemHierarchicLocation>
   {
   
        private ICustomContext currentContext;
        public CustomsItemHierarchicLocationRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsItemHierarchicLocationRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsItemHierarchicLocation GetSingle(string code)
        {
            return (from a in context.CustomsItemHierarchicLocations
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsItemHierarchicLocation> GetAll()
        {
            return from a in context.CustomsItemHierarchicLocations  
                   select a;
        }
				 
        public CustomsItemHierarchicLocation GetSingle(EntityKeyFields entityKeys)
        {
            CustomsItemHierarchicLocationKeys keys = entityKeys as CustomsItemHierarchicLocationKeys;
            return (from a in context.CustomsItemHierarchicLocations
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsItemHierarchicLocation entity)
        {
            onAdd();
            context.CustomsItemHierarchicLocations.Add(entity);
        }

        public void Remove(CustomsItemHierarchicLocation entity)
        {
            context.CustomsItemHierarchicLocations.Attach(entity);
            context.CustomsItemHierarchicLocations.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsItemHierarchicLocation entity)
        {
            onUpdate();
            context.CustomsItemHierarchicLocations.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsItemHierarchicLocation> All()
        {
            return context.CustomsItemHierarchicLocations.ToList();
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
	 