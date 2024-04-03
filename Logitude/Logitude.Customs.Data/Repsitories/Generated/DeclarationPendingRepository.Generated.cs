 
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
   public partial class DeclarationPendingRepository:IRepository<DeclarationPending>
   {
   
        private ICustomContext currentContext;
        public DeclarationPendingRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DeclarationPendingRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DeclarationPending GetSingle(string declarationid, string courierpendingreasoncode, int tenant)
        {
            return (from a in context.DeclarationPendings
                    where a.DeclarationID == declarationid && a.CourierPendingReasonCode == courierpendingreasoncode && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<DeclarationPending> GetAll(int tenant)
        {
            return from a in context.DeclarationPendings  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public DeclarationPending GetSingle(EntityKeyFields entityKeys)
        {
            DeclarationPendingKeys keys = entityKeys as DeclarationPendingKeys;
            return (from a in context.DeclarationPendings
                    where a.DeclarationID == keys.DeclarationID && a.CourierPendingReasonCode == keys.CourierPendingReasonCode
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DeclarationPending entity)
        {
            onAdd();
            context.DeclarationPendings.Add(entity);
        }

        public void Remove(DeclarationPending entity)
        {
            context.DeclarationPendings.Attach(entity);
            context.DeclarationPendings.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DeclarationPending entity)
        {
            onUpdate();
            context.DeclarationPendings.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DeclarationPending> All()
        {
            return context.DeclarationPendings.ToList();
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
	 