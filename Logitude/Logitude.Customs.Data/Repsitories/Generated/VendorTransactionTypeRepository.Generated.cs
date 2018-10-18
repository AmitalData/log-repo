 
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
   public partial class VendorTransactionTypeRepository:IRepository<VendorTransactionType>
   {
   
        private ICustomContext currentContext;
        public VendorTransactionTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public VendorTransactionTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  VendorTransactionType GetSingle(string code)
        {
            return (from a in context.VendorTransactionTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<VendorTransactionType> GetAll()
        {
            return from a in context.VendorTransactionTypes  
                   select a;
        }
				 
        public VendorTransactionType GetSingle(EntityKeyFields entityKeys)
        {
            VendorTransactionTypeKeys keys = entityKeys as VendorTransactionTypeKeys;
            return (from a in context.VendorTransactionTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(VendorTransactionType entity)
        {
            onAdd();
            context.VendorTransactionTypes.Add(entity);
        }

        public void Remove(VendorTransactionType entity)
        {
            context.VendorTransactionTypes.Attach(entity);
            context.VendorTransactionTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(VendorTransactionType entity)
        {
            onUpdate();
            context.VendorTransactionTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<VendorTransactionType> All()
        {
            return context.VendorTransactionTypes.ToList();
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
	 