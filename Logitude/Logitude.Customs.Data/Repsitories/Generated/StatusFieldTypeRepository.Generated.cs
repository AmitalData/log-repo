 
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
   public partial class StatusFieldTypeRepository:IRepository<StatusFieldType>
   {
   
        private ICustomContext currentContext;
        public StatusFieldTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public StatusFieldTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  StatusFieldType GetSingle(string code)
        {
            return (from a in context.StatusFieldTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<StatusFieldType> GetAll()
        {
            return from a in context.StatusFieldTypes  
                   select a;
        }
				 
        public StatusFieldType GetSingle(EntityKeyFields entityKeys)
        {
            StatusFieldTypeKeys keys = entityKeys as StatusFieldTypeKeys;
            return (from a in context.StatusFieldTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(StatusFieldType entity)
        {
            onAdd();
            context.StatusFieldTypes.Add(entity);
        }

        public void Remove(StatusFieldType entity)
        {
            context.StatusFieldTypes.Attach(entity);
            context.StatusFieldTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(StatusFieldType entity)
        {
            onUpdate();
            context.StatusFieldTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<StatusFieldType> All()
        {
            return context.StatusFieldTypes.ToList();
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
	 