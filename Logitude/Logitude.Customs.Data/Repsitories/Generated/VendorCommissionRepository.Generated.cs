 
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
   public partial class VendorCommissionRepository:IRepository<VendorCommission>
   {
   
        private ICustomContext currentContext;
        public VendorCommissionRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public VendorCommissionRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  VendorCommission GetSingle(string vendorid, string customerid, string modificationstypecode, int tenant)
        {
            return (from a in context.VendorCommissions
                    where a.VendorId == vendorid && a.CustomerId == customerid && a.ModificationsTypeCode == modificationstypecode && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<VendorCommission> GetAll(int tenant)
        {
            return from a in context.VendorCommissions  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public VendorCommission GetSingle(EntityKeyFields entityKeys)
        {
            VendorCommissionKeys keys = entityKeys as VendorCommissionKeys;
            return (from a in context.VendorCommissions
                    where a.VendorId == keys.VendorId && a.CustomerId == keys.CustomerId && a.ModificationsTypeCode == keys.ModificationsTypeCode
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(VendorCommission entity)
        {
            onAdd();
            context.VendorCommissions.Add(entity);
        }

        public void Remove(VendorCommission entity)
        {
            context.VendorCommissions.Attach(entity);
            context.VendorCommissions.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(VendorCommission entity)
        {
            onUpdate();
            context.VendorCommissions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<VendorCommission> All()
        {
            return context.VendorCommissions.ToList();
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
	 