 
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
   public partial class CustomMetaDataTypeRepository:IRepository<CustomMetaDataType>
   {
   
        private ICustomContext currentContext;
        public CustomMetaDataTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomMetaDataTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomMetaDataType GetSingle(string code)
        {
            return (from a in context.CustomMetaDataTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomMetaDataType> GetAll()
        {
            return from a in context.CustomMetaDataTypes  
                   select a;
        }
				 
        public CustomMetaDataType GetSingle(EntityKeyFields entityKeys)
        {
            CustomMetaDataTypeKeys keys = entityKeys as CustomMetaDataTypeKeys;
            return (from a in context.CustomMetaDataTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomMetaDataType entity)
        {
            onAdd();
            context.CustomMetaDataTypes.Add(entity);
        }

        public void Remove(CustomMetaDataType entity)
        {
            context.CustomMetaDataTypes.Attach(entity);
            context.CustomMetaDataTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomMetaDataType entity)
        {
            onUpdate();
            context.CustomMetaDataTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomMetaDataType> All()
        {
            return context.CustomMetaDataTypes.ToList();
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
	 