 
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
   public partial class CustomsDocumentStatusTypeRepository:IRepository<CustomsDocumentStatusType>
   {
   
        private ICustomContext currentContext;
        public CustomsDocumentStatusTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsDocumentStatusTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsDocumentStatusType GetSingle(string code)
        {
            return (from a in context.CustomsDocumentStatusTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsDocumentStatusType> GetAll()
        {
            return from a in context.CustomsDocumentStatusTypes  
                   select a;
        }
				 
        public CustomsDocumentStatusType GetSingle(EntityKeyFields entityKeys)
        {
            CustomsDocumentStatusTypeKeys keys = entityKeys as CustomsDocumentStatusTypeKeys;
            return (from a in context.CustomsDocumentStatusTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsDocumentStatusType entity)
        {
            onAdd();
            context.CustomsDocumentStatusTypes.Add(entity);
        }

        public void Remove(CustomsDocumentStatusType entity)
        {
            context.CustomsDocumentStatusTypes.Attach(entity);
            context.CustomsDocumentStatusTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsDocumentStatusType entity)
        {
            onUpdate();
            context.CustomsDocumentStatusTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsDocumentStatusType> All()
        {
            return context.CustomsDocumentStatusTypes.ToList();
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
	 