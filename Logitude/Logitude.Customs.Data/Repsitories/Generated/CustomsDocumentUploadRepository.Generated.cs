 
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
   public partial class CustomsDocumentUploadRepository:IRepository<CustomsDocumentUpload>
   {
   
        private ICustomContext currentContext;
        public CustomsDocumentUploadRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsDocumentUploadRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsDocumentUpload GetSingle(string code)
        {
            return (from a in context.CustomsDocumentUploads
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsDocumentUpload> GetAll()
        {
            return from a in context.CustomsDocumentUploads  
                   select a;
        }
				 
        public CustomsDocumentUpload GetSingle(EntityKeyFields entityKeys)
        {
            CustomsDocumentUploadKeys keys = entityKeys as CustomsDocumentUploadKeys;
            return (from a in context.CustomsDocumentUploads
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsDocumentUpload entity)
        {
            onAdd();
            context.CustomsDocumentUploads.Add(entity);
        }

        public void Remove(CustomsDocumentUpload entity)
        {
            context.CustomsDocumentUploads.Attach(entity);
            context.CustomsDocumentUploads.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsDocumentUpload entity)
        {
            onUpdate();
            context.CustomsDocumentUploads.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsDocumentUpload> All()
        {
            return context.CustomsDocumentUploads.ToList();
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
	 