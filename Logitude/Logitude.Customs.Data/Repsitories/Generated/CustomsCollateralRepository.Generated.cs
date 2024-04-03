 
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
   public partial class CustomsCollateralRepository:IRepository<CustomsCollateral>
   {
   
        private ICustomContext currentContext;
        public CustomsCollateralRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsCollateralRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsCollateral GetSingle(string id, int tenant)
        {
            return (from a in context.CustomsCollaterals
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsCollateral> GetAll(int tenant)
        {
            return from a in context.CustomsCollaterals  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CustomsCollateral GetSingle(EntityKeyFields entityKeys)
        {
            CustomsCollateralKeys keys = entityKeys as CustomsCollateralKeys;
            return (from a in context.CustomsCollaterals
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsCollateral entity)
        {
            onAdd();
            context.CustomsCollaterals.Add(entity);
        }

        public void Remove(CustomsCollateral entity)
        {
            context.CustomsCollaterals.Attach(entity);
            context.CustomsCollaterals.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsCollateral entity)
        {
            onUpdate();
            context.CustomsCollaterals.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsCollateral> All()
        {
            return context.CustomsCollaterals.ToList();
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
	 