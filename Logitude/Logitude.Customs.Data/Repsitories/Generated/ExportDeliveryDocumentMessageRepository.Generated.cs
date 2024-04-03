 
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
   public partial class ExportDeliveryDocumentMessageRepository:IRepository<ExportDeliveryDocumentMessage>
   {
   
        private ICustomContext currentContext;
        public ExportDeliveryDocumentMessageRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ExportDeliveryDocumentMessageRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ExportDeliveryDocumentMessage GetSingle(string code)
        {
            return (from a in context.ExportDeliveryDocumentMessages
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ExportDeliveryDocumentMessage> GetAll()
        {
            return from a in context.ExportDeliveryDocumentMessages  
                   select a;
        }
				 
        public ExportDeliveryDocumentMessage GetSingle(EntityKeyFields entityKeys)
        {
            ExportDeliveryDocumentMessageKeys keys = entityKeys as ExportDeliveryDocumentMessageKeys;
            return (from a in context.ExportDeliveryDocumentMessages
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ExportDeliveryDocumentMessage entity)
        {
            onAdd();
            context.ExportDeliveryDocumentMessages.Add(entity);
        }

        public void Remove(ExportDeliveryDocumentMessage entity)
        {
            context.ExportDeliveryDocumentMessages.Attach(entity);
            context.ExportDeliveryDocumentMessages.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ExportDeliveryDocumentMessage entity)
        {
            onUpdate();
            context.ExportDeliveryDocumentMessages.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ExportDeliveryDocumentMessage> All()
        {
            return context.ExportDeliveryDocumentMessages.ToList();
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
	 