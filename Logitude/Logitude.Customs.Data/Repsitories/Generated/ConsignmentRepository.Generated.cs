 
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
   public partial class ConsignmentRepository:IRepository<Consignment>
   {
   
        private ICustomContext currentContext;
        public ConsignmentRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ConsignmentRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  Consignment GetSingle(string declarationid, int? consignmentnumber, int tenant)
        {
            return (from a in context.Consignments
                    where a.DeclarationId == declarationid && a.ConsignmentNumber == consignmentnumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Consignment> GetAll(int tenant)
        {
            return from a in context.Consignments  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Consignment GetSingle(EntityKeyFields entityKeys)
        {
            ConsignmentKeys keys = entityKeys as ConsignmentKeys;
            return (from a in context.Consignments
                    where a.DeclarationId == keys.DeclarationId && a.ConsignmentNumber == keys.ConsignmentNumber
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Consignment entity)
        {
            onAdd();
            context.Consignments.Add(entity);
        }

        public void Remove(Consignment entity)
        {
            context.Consignments.Attach(entity);
            context.Consignments.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Consignment entity)
        {
            onUpdate();
            context.Consignments.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Consignment> All()
        {
            return context.Consignments.ToList();
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
	 