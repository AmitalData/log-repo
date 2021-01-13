 
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
   public partial class DocumentRejectTypeRepository:IRepository<DocumentRejectType>
   {
   
        private ICustomContext currentContext;
        public DocumentRejectTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DocumentRejectTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DocumentRejectType GetSingle(string code)
        {
            return (from a in context.DocumentRejectTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<DocumentRejectType> GetAll()
        {
            return from a in context.DocumentRejectTypes  
                   select a;
        }
				 
        public DocumentRejectType GetSingle(EntityKeyFields entityKeys)
        {
            DocumentRejectTypeKeys keys = entityKeys as DocumentRejectTypeKeys;
            return (from a in context.DocumentRejectTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DocumentRejectType entity)
        {
            onAdd();
            context.DocumentRejectTypes.Add(entity);
        }

        public void Remove(DocumentRejectType entity)
        {
            context.DocumentRejectTypes.Attach(entity);
            context.DocumentRejectTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DocumentRejectType entity)
        {
            onUpdate();
            context.DocumentRejectTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DocumentRejectType> All()
        {
            return context.DocumentRejectTypes.ToList();
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
	 