 
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
   public partial class CancellationReasonRequestTypeRepository:IRepository<CancellationReasonRequestType>
   {
   
        private ICustomContext currentContext;
        public CancellationReasonRequestTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CancellationReasonRequestTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CancellationReasonRequestType GetSingle(string code)
        {
            return (from a in context.CancellationReasonRequestTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CancellationReasonRequestType> GetAll()
        {
            return from a in context.CancellationReasonRequestTypes  
                   select a;
        }
				 
        public CancellationReasonRequestType GetSingle(EntityKeyFields entityKeys)
        {
            CancellationReasonRequestTypeKeys keys = entityKeys as CancellationReasonRequestTypeKeys;
            return (from a in context.CancellationReasonRequestTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CancellationReasonRequestType entity)
        {
            onAdd();
            context.CancellationReasonRequestTypes.Add(entity);
        }

        public void Remove(CancellationReasonRequestType entity)
        {
            context.CancellationReasonRequestTypes.Attach(entity);
            context.CancellationReasonRequestTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CancellationReasonRequestType entity)
        {
            onUpdate();
            context.CancellationReasonRequestTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CancellationReasonRequestType> All()
        {
            return context.CancellationReasonRequestTypes.ToList();
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
	 