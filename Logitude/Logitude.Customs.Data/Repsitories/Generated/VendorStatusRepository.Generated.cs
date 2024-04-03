 
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
   public partial class VendorStatusRepository:IRepository<VendorStatus>
   {
   
        private ICustomContext currentContext;
        public VendorStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public VendorStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  VendorStatus GetSingle(string code)
        {
            return (from a in context.VendorStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<VendorStatus> GetAll()
        {
            return from a in context.VendorStatuses  
                   select a;
        }
				 
        public VendorStatus GetSingle(EntityKeyFields entityKeys)
        {
            VendorStatusKeys keys = entityKeys as VendorStatusKeys;
            return (from a in context.VendorStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(VendorStatus entity)
        {
            onAdd();
            context.VendorStatuses.Add(entity);
        }

        public void Remove(VendorStatus entity)
        {
            context.VendorStatuses.Attach(entity);
            context.VendorStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(VendorStatus entity)
        {
            onUpdate();
            context.VendorStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<VendorStatus> All()
        {
            return context.VendorStatuses.ToList();
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
	 