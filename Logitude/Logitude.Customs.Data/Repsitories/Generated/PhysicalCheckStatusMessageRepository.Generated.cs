 
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
   public partial class PhysicalCheckStatusMessageRepository:IRepository<PhysicalCheckStatusMessage>
   {
   
        private ICustomContext currentContext;
        public PhysicalCheckStatusMessageRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public PhysicalCheckStatusMessageRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  PhysicalCheckStatusMessage GetSingle(string code)
        {
            return (from a in context.PhysicalCheckStatusMessages
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<PhysicalCheckStatusMessage> GetAll()
        {
            return from a in context.PhysicalCheckStatusMessages  
                   select a;
        }
				 
        public PhysicalCheckStatusMessage GetSingle(EntityKeyFields entityKeys)
        {
            PhysicalCheckStatusMessageKeys keys = entityKeys as PhysicalCheckStatusMessageKeys;
            return (from a in context.PhysicalCheckStatusMessages
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PhysicalCheckStatusMessage entity)
        {
            onAdd();
            context.PhysicalCheckStatusMessages.Add(entity);
        }

        public void Remove(PhysicalCheckStatusMessage entity)
        {
            context.PhysicalCheckStatusMessages.Attach(entity);
            context.PhysicalCheckStatusMessages.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PhysicalCheckStatusMessage entity)
        {
            onUpdate();
            context.PhysicalCheckStatusMessages.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PhysicalCheckStatusMessage> All()
        {
            return context.PhysicalCheckStatusMessages.ToList();
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
	 