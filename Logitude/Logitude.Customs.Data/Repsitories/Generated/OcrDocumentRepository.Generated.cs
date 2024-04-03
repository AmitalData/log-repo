 
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
   public partial class OcrDocumentRepository:IRepository<OcrDocument>
   {
   
        private ICustomContext currentContext;
        public OcrDocumentRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public OcrDocumentRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  OcrDocument GetSingle(string id, int tenant)
        {
            return (from a in context.OcrDocuments
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<OcrDocument> GetAll(int tenant)
        {
            return from a in context.OcrDocuments  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public OcrDocument GetSingle(EntityKeyFields entityKeys)
        {
            OcrDocumentKeys keys = entityKeys as OcrDocumentKeys;
            return (from a in context.OcrDocuments
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(OcrDocument entity)
        {
            onAdd();
            context.OcrDocuments.Add(entity);
        }

        public void Remove(OcrDocument entity)
        {
            context.OcrDocuments.Attach(entity);
            context.OcrDocuments.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(OcrDocument entity)
        {
            onUpdate();
            context.OcrDocuments.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<OcrDocument> All()
        {
            return context.OcrDocuments.ToList();
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
	 