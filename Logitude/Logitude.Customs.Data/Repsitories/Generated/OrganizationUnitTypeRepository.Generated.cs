 
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
   public partial class OrganizationUnitTypeRepository:IRepository<OrganizationUnitType>
   {
   
        private ICustomContext currentContext;
        public OrganizationUnitTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public OrganizationUnitTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  OrganizationUnitType GetSingle(string code)
        {
            return (from a in context.OrganizationUnitTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<OrganizationUnitType> GetAll()
        {
            return from a in context.OrganizationUnitTypes  
                   select a;
        }
				 
        public OrganizationUnitType GetSingle(EntityKeyFields entityKeys)
        {
            OrganizationUnitTypeKeys keys = entityKeys as OrganizationUnitTypeKeys;
            return (from a in context.OrganizationUnitTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(OrganizationUnitType entity)
        {
            onAdd();
            context.OrganizationUnitTypes.Add(entity);
        }

        public void Remove(OrganizationUnitType entity)
        {
            context.OrganizationUnitTypes.Attach(entity);
            context.OrganizationUnitTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(OrganizationUnitType entity)
        {
            onUpdate();
            context.OrganizationUnitTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<OrganizationUnitType> All()
        {
            return context.OrganizationUnitTypes.ToList();
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
	 