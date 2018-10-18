 
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
   public partial class LeadDocumentExceptionTypeRepository:IRepository<LeadDocumentExceptionType>
   {
   
        private ICustomContext currentContext;
        public LeadDocumentExceptionTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public LeadDocumentExceptionTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  LeadDocumentExceptionType GetSingle(string code)
        {
            return (from a in context.LeadDocumentExceptionTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<LeadDocumentExceptionType> GetAll()
        {
            return from a in context.LeadDocumentExceptionTypes  
                   select a;
        }
				 
        public LeadDocumentExceptionType GetSingle(EntityKeyFields entityKeys)
        {
            LeadDocumentExceptionTypeKeys keys = entityKeys as LeadDocumentExceptionTypeKeys;
            return (from a in context.LeadDocumentExceptionTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(LeadDocumentExceptionType entity)
        {
            onAdd();
            context.LeadDocumentExceptionTypes.Add(entity);
        }

        public void Remove(LeadDocumentExceptionType entity)
        {
            context.LeadDocumentExceptionTypes.Attach(entity);
            context.LeadDocumentExceptionTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(LeadDocumentExceptionType entity)
        {
            onUpdate();
            context.LeadDocumentExceptionTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<LeadDocumentExceptionType> All()
        {
            return context.LeadDocumentExceptionTypes.ToList();
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
	 