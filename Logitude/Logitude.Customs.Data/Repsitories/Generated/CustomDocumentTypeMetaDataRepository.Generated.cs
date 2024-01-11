 
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
   public partial class CustomDocumentTypeMetaDataRepository:IRepository<CustomDocumentTypeMetaData>
   {
   
        private ICustomContext currentContext;
        public CustomDocumentTypeMetaDataRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomDocumentTypeMetaDataRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomDocumentTypeMetaData GetSingle(string metadatatypecode, string documenttypecode)
        {
            return (from a in context.CustomDocumentTypeMetaData
                    where a.MetaDataTypeCode == metadatatypecode && a.DocumentTypeCode == documenttypecode 
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomDocumentTypeMetaData> GetAll()
        {
            return from a in context.CustomDocumentTypeMetaData  
                   select a;
        }
				 
        public CustomDocumentTypeMetaData GetSingle(EntityKeyFields entityKeys)
        {
            CustomDocumentTypeMetaDataKeys keys = entityKeys as CustomDocumentTypeMetaDataKeys;
            return (from a in context.CustomDocumentTypeMetaData
                    where a.MetaDataTypeCode == keys.MetaDataTypeCode && a.DocumentTypeCode == keys.DocumentTypeCode
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomDocumentTypeMetaData entity)
        {
            onAdd();
            context.CustomDocumentTypeMetaData.Add(entity);
        }

        public void Remove(CustomDocumentTypeMetaData entity)
        {
            context.CustomDocumentTypeMetaData.Attach(entity);
            context.CustomDocumentTypeMetaData.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomDocumentTypeMetaData entity)
        {
            onUpdate();
            context.CustomDocumentTypeMetaData.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomDocumentTypeMetaData> All()
        {
            return context.CustomDocumentTypeMetaData.ToList();
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
	 