 
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
   public partial class CustomDocumentTypeRepository:IRepository<CustomDocumentType>
   {
   
        private ICustomContext currentContext;
        public CustomDocumentTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomDocumentTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomDocumentType GetSingle(string code)
        {
            return (from a in context.CustomDocumentTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomDocumentType> GetAll()
        {
            return from a in context.CustomDocumentTypes  
                   select a;
        }
				 
        public CustomDocumentType GetSingle(EntityKeyFields entityKeys)
        {
            CustomDocumentTypeKeys keys = entityKeys as CustomDocumentTypeKeys;
            return (from a in context.CustomDocumentTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomDocumentType entity)
        {
            onAdd();
            context.CustomDocumentTypes.Add(entity);
        }

        public void Remove(CustomDocumentType entity)
        {
            context.CustomDocumentTypes.Attach(entity);
            context.CustomDocumentTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomDocumentType entity)
        {
            onUpdate();
            context.CustomDocumentTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomDocumentType> All()
        {
            return context.CustomDocumentTypes.ToList();
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
	 