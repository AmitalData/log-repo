 
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
   public partial class ClassificationTypeRepository:IRepository<ClassificationType>
   {
   
        private ICustomContext currentContext;
        public ClassificationTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ClassificationTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ClassificationType GetSingle(string code)
        {
            return (from a in context.ClassificationTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ClassificationType> GetAll()
        {
            return from a in context.ClassificationTypes  
                   select a;
        }
				 
        public ClassificationType GetSingle(EntityKeyFields entityKeys)
        {
            ClassificationTypeKeys keys = entityKeys as ClassificationTypeKeys;
            return (from a in context.ClassificationTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ClassificationType entity)
        {
            onAdd();
            context.ClassificationTypes.Add(entity);
        }

        public void Remove(ClassificationType entity)
        {
            context.ClassificationTypes.Attach(entity);
            context.ClassificationTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ClassificationType entity)
        {
            onUpdate();
            context.ClassificationTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ClassificationType> All()
        {
            return context.ClassificationTypes.ToList();
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
	 