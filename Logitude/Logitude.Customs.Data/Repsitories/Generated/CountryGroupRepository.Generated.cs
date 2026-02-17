 
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
   public partial class CountryGroupRepository:IRepository<CountryGroup>
   {
   
        private ICustomContext currentContext;
        public CountryGroupRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CountryGroupRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CountryGroup GetSingle(string code)
        {
            return (from a in context.CountryGroups
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CountryGroup> GetAll()
        {
            return from a in context.CountryGroups  
                   select a;
        }
				 
        public CountryGroup GetSingle(EntityKeyFields entityKeys)
        {
            CountryGroupKeys keys = entityKeys as CountryGroupKeys;
            return (from a in context.CountryGroups
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CountryGroup entity)
        {
            onAdd();
            context.CountryGroups.Add(entity);
        }

        public void Remove(CountryGroup entity)
        {
            context.CountryGroups.Attach(entity);
            context.CountryGroups.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CountryGroup entity)
        {
            onUpdate();
            context.CountryGroups.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CountryGroup> All()
        {
            return context.CountryGroups.ToList();
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
	 