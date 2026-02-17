 
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
   public partial class CollateralsRequestFileCondRepository:IRepository<CollateralsRequestFileCond>
   {
   
        private ICustomContext currentContext;
        public CollateralsRequestFileCondRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CollateralsRequestFileCondRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CollateralsRequestFileCond GetSingle(string customscollateralid, string conditioncode, int linenumber, int tenant)
        {
            return (from a in context.CollateralsRequestFileConds
                    where a.CustomsCollateralId == customscollateralid && a.ConditionCode == conditioncode && a.LineNumber == linenumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CollateralsRequestFileCond> GetAll(int tenant)
        {
            return from a in context.CollateralsRequestFileConds  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CollateralsRequestFileCond GetSingle(EntityKeyFields entityKeys)
        {
            CollateralsRequestFileCondKeys keys = entityKeys as CollateralsRequestFileCondKeys;
            return (from a in context.CollateralsRequestFileConds
                    where a.CustomsCollateralId == keys.CustomsCollateralId && a.ConditionCode == keys.ConditionCode && a.LineNumber == keys.LineNumber
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CollateralsRequestFileCond entity)
        {
            onAdd();
            context.CollateralsRequestFileConds.Add(entity);
        }

        public void Remove(CollateralsRequestFileCond entity)
        {
            context.CollateralsRequestFileConds.Attach(entity);
            context.CollateralsRequestFileConds.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CollateralsRequestFileCond entity)
        {
            onUpdate();
            context.CollateralsRequestFileConds.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CollateralsRequestFileCond> All()
        {
            return context.CollateralsRequestFileConds.ToList();
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
	 