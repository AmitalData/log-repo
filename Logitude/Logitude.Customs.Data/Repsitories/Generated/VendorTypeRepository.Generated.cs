 
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
   public partial class VendorTypeRepository:IRepository<VendorType>
   {
   
        private ICustomContext currentContext;
        public VendorTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public VendorTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  VendorType GetSingle(string code)
        {
            return (from a in context.VendorTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<VendorType> GetAll()
        {
            return from a in context.VendorTypes  
                   select a;
        }
				 
        public VendorType GetSingle(EntityKeyFields entityKeys)
        {
            VendorTypeKeys keys = entityKeys as VendorTypeKeys;
            return (from a in context.VendorTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(VendorType entity)
        {
            onAdd();
            context.VendorTypes.Add(entity);
        }

        public void Remove(VendorType entity)
        {
            context.VendorTypes.Attach(entity);
            context.VendorTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(VendorType entity)
        {
            onUpdate();
            context.VendorTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<VendorType> All()
        {
            return context.VendorTypes.ToList();
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
	 