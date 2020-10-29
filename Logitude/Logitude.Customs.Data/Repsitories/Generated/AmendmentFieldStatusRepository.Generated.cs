 
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
   public partial class AmendmentFieldStatusRepository:IRepository<AmendmentFieldStatus>
   {
   
        private ICustomContext currentContext;
        public AmendmentFieldStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public AmendmentFieldStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  AmendmentFieldStatus GetSingle(string code)
        {
            return (from a in context.AmendmentFieldStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<AmendmentFieldStatus> GetAll()
        {
            return from a in context.AmendmentFieldStatuses  
                   select a;
        }
				 
        public AmendmentFieldStatus GetSingle(EntityKeyFields entityKeys)
        {
            AmendmentFieldStatusKeys keys = entityKeys as AmendmentFieldStatusKeys;
            return (from a in context.AmendmentFieldStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(AmendmentFieldStatus entity)
        {
            onAdd();
            context.AmendmentFieldStatuses.Add(entity);
        }

        public void Remove(AmendmentFieldStatus entity)
        {
            context.AmendmentFieldStatuses.Attach(entity);
            context.AmendmentFieldStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(AmendmentFieldStatus entity)
        {
            onUpdate();
            context.AmendmentFieldStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AmendmentFieldStatus> All()
        {
            return context.AmendmentFieldStatuses.ToList();
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
	 