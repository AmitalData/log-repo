 
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
   public partial class VendorCommunicationRepository:IRepository<VendorCommunication>
   {
   
        private ICustomContext currentContext;
        public VendorCommunicationRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public VendorCommunicationRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  VendorCommunication GetSingle(string vendorid, int linenumber, int tenant)
        {
            return (from a in context.VendorCommunications
                    where a.VendorId == vendorid && a.LineNumber == linenumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<VendorCommunication> GetAll(int tenant)
        {
            return from a in context.VendorCommunications  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public VendorCommunication GetSingle(EntityKeyFields entityKeys)
        {
            VendorCommunicationKeys keys = entityKeys as VendorCommunicationKeys;
            return (from a in context.VendorCommunications
                    where a.VendorId == keys.VendorId && a.LineNumber == keys.LineNumber
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(VendorCommunication entity)
        {
            onAdd();
            context.VendorCommunications.Add(entity);
        }

        public void Remove(VendorCommunication entity)
        {
            context.VendorCommunications.Attach(entity);
            context.VendorCommunications.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(VendorCommunication entity)
        {
            onUpdate();
            context.VendorCommunications.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<VendorCommunication> All()
        {
            return context.VendorCommunications.ToList();
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
	 