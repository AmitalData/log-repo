 
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
   public partial class AttachmentTypeRepository:IRepository<AttachmentType>
   {
   
        private ICustomContext currentContext;
        public AttachmentTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public AttachmentTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  AttachmentType GetSingle(string code)
        {
            return (from a in context.AttachmentTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<AttachmentType> GetAll()
        {
            return from a in context.AttachmentTypes  
                   select a;
        }
				 
        public AttachmentType GetSingle(EntityKeyFields entityKeys)
        {
            AttachmentTypeKeys keys = entityKeys as AttachmentTypeKeys;
            return (from a in context.AttachmentTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(AttachmentType entity)
        {
            onAdd();
            context.AttachmentTypes.Add(entity);
        }

        public void Remove(AttachmentType entity)
        {
            context.AttachmentTypes.Attach(entity);
            context.AttachmentTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(AttachmentType entity)
        {
            onUpdate();
            context.AttachmentTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AttachmentType> All()
        {
            return context.AttachmentTypes.ToList();
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
	 