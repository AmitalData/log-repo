 
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
   public partial class HazardousSubstanceRepository:IRepository<HazardousSubstance>
   {
   
        private ICustomContext currentContext;
        public HazardousSubstanceRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public HazardousSubstanceRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  HazardousSubstance GetSingle(string code)
        {
            return (from a in context.HazardousSubstances
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<HazardousSubstance> GetAll()
        {
            return from a in context.HazardousSubstances  
                   select a;
        }
				 
        public HazardousSubstance GetSingle(EntityKeyFields entityKeys)
        {
            HazardousSubstanceKeys keys = entityKeys as HazardousSubstanceKeys;
            return (from a in context.HazardousSubstances
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(HazardousSubstance entity)
        {
            onAdd();
            context.HazardousSubstances.Add(entity);
        }

        public void Remove(HazardousSubstance entity)
        {
            context.HazardousSubstances.Attach(entity);
            context.HazardousSubstances.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(HazardousSubstance entity)
        {
            onUpdate();
            context.HazardousSubstances.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<HazardousSubstance> All()
        {
            return context.HazardousSubstances.ToList();
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
	 