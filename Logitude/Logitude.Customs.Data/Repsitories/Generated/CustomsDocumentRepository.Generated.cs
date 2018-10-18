 
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
   public partial class CustomsDocumentRepository:IRepository<CustomsDocument>
   {
   
        private ICustomContext currentContext;
        public CustomsDocumentRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsDocumentRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsDocument GetSingle(string documentsfilingid, int tenant)
        {
            return (from a in context.CustomsDocuments
                    where a.DocumentsFilingId == documentsfilingid && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsDocument> GetAll(int tenant)
        {
            return from a in context.CustomsDocuments  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CustomsDocument GetSingle(EntityKeyFields entityKeys)
        {
            CustomsDocumentKeys keys = entityKeys as CustomsDocumentKeys;
            return (from a in context.CustomsDocuments
                    where a.DocumentsFilingId == keys.DocumentsFilingId
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsDocument entity)
        {
            onAdd();
            context.CustomsDocuments.Add(entity);
        }

        public void Remove(CustomsDocument entity)
        {
            context.CustomsDocuments.Attach(entity);
            context.CustomsDocuments.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsDocument entity)
        {
            onUpdate();
            context.CustomsDocuments.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsDocument> All()
        {
            return context.CustomsDocuments.ToList();
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
	 