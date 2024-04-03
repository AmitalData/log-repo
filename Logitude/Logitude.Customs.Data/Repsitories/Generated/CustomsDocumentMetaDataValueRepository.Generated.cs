 
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
   public partial class CustomsDocumentMetaDataValueRepository:IRepository<CustomsDocumentMetaDataValue>
   {
   
        private ICustomContext currentContext;
        public CustomsDocumentMetaDataValueRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsDocumentMetaDataValueRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsDocumentMetaDataValue GetSingle(string customsdocumentid, string metadatatypecode, int tenant)
        {
            return (from a in context.CustomsDocumentMetaDataValues
                    where a.CustomsDocumentId == customsdocumentid && a.MetaDataTypeCode == metadatatypecode && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsDocumentMetaDataValue> GetAll(int tenant)
        {
            return from a in context.CustomsDocumentMetaDataValues  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CustomsDocumentMetaDataValue GetSingle(EntityKeyFields entityKeys)
        {
            CustomsDocumentMetaDataValueKeys keys = entityKeys as CustomsDocumentMetaDataValueKeys;
            return (from a in context.CustomsDocumentMetaDataValues
                    where a.CustomsDocumentId == keys.CustomsDocumentId && a.MetaDataTypeCode == keys.MetaDataTypeCode
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsDocumentMetaDataValue entity)
        {
            onAdd();
            context.CustomsDocumentMetaDataValues.Add(entity);
        }

        public void Remove(CustomsDocumentMetaDataValue entity)
        {
            context.CustomsDocumentMetaDataValues.Attach(entity);
            context.CustomsDocumentMetaDataValues.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsDocumentMetaDataValue entity)
        {
            onUpdate();
            context.CustomsDocumentMetaDataValues.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsDocumentMetaDataValue> All()
        {
            return context.CustomsDocumentMetaDataValues.ToList();
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
	 