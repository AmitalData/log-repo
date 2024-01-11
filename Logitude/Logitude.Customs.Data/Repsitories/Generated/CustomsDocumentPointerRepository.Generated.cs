 
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
   public partial class CustomsDocumentPointerRepository:IRepository<CustomsDocumentPointer>
   {
   
        private ICustomContext currentContext;
        public CustomsDocumentPointerRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsDocumentPointerRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsDocumentPointer GetSingle(string id, int tenant)
        {
            return (from a in context.CustomsDocumentPointers
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsDocumentPointer> GetAll(int tenant)
        {
            return from a in context.CustomsDocumentPointers  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CustomsDocumentPointer GetSingle(EntityKeyFields entityKeys)
        {
            CustomsDocumentPointerKeys keys = entityKeys as CustomsDocumentPointerKeys;
            return (from a in context.CustomsDocumentPointers
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsDocumentPointer entity)
        {
            onAdd();
            context.CustomsDocumentPointers.Add(entity);
        }

        public void Remove(CustomsDocumentPointer entity)
        {
            context.CustomsDocumentPointers.Attach(entity);
            context.CustomsDocumentPointers.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsDocumentPointer entity)
        {
            onUpdate();
            context.CustomsDocumentPointers.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsDocumentPointer> All()
        {
            return context.CustomsDocumentPointers.ToList();
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
	 