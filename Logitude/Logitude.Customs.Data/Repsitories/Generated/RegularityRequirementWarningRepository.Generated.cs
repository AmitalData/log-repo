 
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
   public partial class RegularityRequirementWarningRepository:IRepository<RegularityRequirementWarning>
   {
   
        private ICustomContext currentContext;
        public RegularityRequirementWarningRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public RegularityRequirementWarningRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  RegularityRequirementWarning GetSingle(string code)
        {
            return (from a in context.RegularityRequirementWarnings
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<RegularityRequirementWarning> GetAll()
        {
            return from a in context.RegularityRequirementWarnings  
                   select a;
        }
				 
        public RegularityRequirementWarning GetSingle(EntityKeyFields entityKeys)
        {
            RegularityRequirementWarningKeys keys = entityKeys as RegularityRequirementWarningKeys;
            return (from a in context.RegularityRequirementWarnings
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(RegularityRequirementWarning entity)
        {
            onAdd();
            context.RegularityRequirementWarnings.Add(entity);
        }

        public void Remove(RegularityRequirementWarning entity)
        {
            context.RegularityRequirementWarnings.Attach(entity);
            context.RegularityRequirementWarnings.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(RegularityRequirementWarning entity)
        {
            onUpdate();
            context.RegularityRequirementWarnings.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<RegularityRequirementWarning> All()
        {
            return context.RegularityRequirementWarnings.ToList();
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
	 