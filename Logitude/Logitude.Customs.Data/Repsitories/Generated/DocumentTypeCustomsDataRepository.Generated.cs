 
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
   public partial class DocumentTypeCustomsDataRepository:IRepository<DocumentTypeCustomsData>
   {
   
        private ICustomContext currentContext;
        public DocumentTypeCustomsDataRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DocumentTypeCustomsDataRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DocumentTypeCustomsData GetSingle(string documenttypeid, int tenant)
        {
            return (from a in context.DocumentTypeCustomsData
                    where a.DocumentTypeId == documenttypeid && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<DocumentTypeCustomsData> GetAll(int tenant)
        {
            return from a in context.DocumentTypeCustomsData  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public DocumentTypeCustomsData GetSingle(EntityKeyFields entityKeys)
        {
            DocumentTypeCustomsDataKeys keys = entityKeys as DocumentTypeCustomsDataKeys;
            return (from a in context.DocumentTypeCustomsData
                    where a.DocumentTypeId == keys.DocumentTypeId
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DocumentTypeCustomsData entity)
        {
            onAdd();
            context.DocumentTypeCustomsData.Add(entity);
        }

        public void Remove(DocumentTypeCustomsData entity)
        {
            context.DocumentTypeCustomsData.Attach(entity);
            context.DocumentTypeCustomsData.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DocumentTypeCustomsData entity)
        {
            onUpdate();
            context.DocumentTypeCustomsData.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DocumentTypeCustomsData> All()
        {
            return context.DocumentTypeCustomsData.ToList();
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
	 