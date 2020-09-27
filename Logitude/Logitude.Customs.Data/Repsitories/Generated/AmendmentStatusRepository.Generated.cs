 
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
   public partial class AmendmentStatusRepository:IRepository<AmendmentStatus>
   {
   
        private ICustomContext currentContext;
        public AmendmentStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public AmendmentStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  AmendmentStatus GetSingle(string code)
        {
            return (from a in context.AmendmentStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<AmendmentStatus> GetAll()
        {
            return from a in context.AmendmentStatuses  
                   select a;
        }
				 
        public AmendmentStatus GetSingle(EntityKeyFields entityKeys)
        {
            AmendmentStatusKeys keys = entityKeys as AmendmentStatusKeys;
            return (from a in context.AmendmentStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(AmendmentStatus entity)
        {
            onAdd();
            context.AmendmentStatuses.Add(entity);
        }

        public void Remove(AmendmentStatus entity)
        {
            context.AmendmentStatuses.Attach(entity);
            context.AmendmentStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(AmendmentStatus entity)
        {
            onUpdate();
            context.AmendmentStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AmendmentStatus> All()
        {
            return context.AmendmentStatuses.ToList();
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
	 