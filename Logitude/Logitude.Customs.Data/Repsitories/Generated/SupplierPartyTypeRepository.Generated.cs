 
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
   public partial class SupplierPartyTypeRepository:IRepository<SupplierPartyType>
   {
   
        private ICustomContext currentContext;
        public SupplierPartyTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SupplierPartyTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SupplierPartyType GetSingle(string code)
        {
            return (from a in context.SupplierPartyTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<SupplierPartyType> GetAll()
        {
            return from a in context.SupplierPartyTypes  
                   select a;
        }
				 
        public SupplierPartyType GetSingle(EntityKeyFields entityKeys)
        {
            SupplierPartyTypeKeys keys = entityKeys as SupplierPartyTypeKeys;
            return (from a in context.SupplierPartyTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SupplierPartyType entity)
        {
            onAdd();
            context.SupplierPartyTypes.Add(entity);
        }

        public void Remove(SupplierPartyType entity)
        {
            context.SupplierPartyTypes.Attach(entity);
            context.SupplierPartyTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SupplierPartyType entity)
        {
            onUpdate();
            context.SupplierPartyTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SupplierPartyType> All()
        {
            return context.SupplierPartyTypes.ToList();
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
	 