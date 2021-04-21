 
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
   public partial class CancelRequestRejectReasonTypeRepository:IRepository<CancelRequestRejectReasonType>
   {
   
        private ICustomContext currentContext;
        public CancelRequestRejectReasonTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CancelRequestRejectReasonTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CancelRequestRejectReasonType GetSingle(string code)
        {
            return (from a in context.CancelRequestRejectReasonTypess
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CancelRequestRejectReasonType> GetAll()
        {
            return from a in context.CancelRequestRejectReasonTypess  
                   select a;
        }
				 
        public CancelRequestRejectReasonType GetSingle(EntityKeyFields entityKeys)
        {
            CancelRequestRejectReasonTypeKeys keys = entityKeys as CancelRequestRejectReasonTypeKeys;
            return (from a in context.CancelRequestRejectReasonTypess
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CancelRequestRejectReasonType entity)
        {
            onAdd();
            context.CancelRequestRejectReasonTypess.Add(entity);
        }

        public void Remove(CancelRequestRejectReasonType entity)
        {
            context.CancelRequestRejectReasonTypess.Attach(entity);
            context.CancelRequestRejectReasonTypess.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CancelRequestRejectReasonType entity)
        {
            onUpdate();
            context.CancelRequestRejectReasonTypess.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CancelRequestRejectReasonType> All()
        {
            return context.CancelRequestRejectReasonTypess.ToList();
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
	 