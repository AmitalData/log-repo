 
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
   public partial class ExportDeliveryDocumentMessageSenderCodeRepository:IRepository<ExportDeliveryDocumentMessageSenderCode>
   {
   
        private ICustomContext currentContext;
        public ExportDeliveryDocumentMessageSenderCodeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ExportDeliveryDocumentMessageSenderCodeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ExportDeliveryDocumentMessageSenderCode GetSingle(string code)
        {
            return (from a in context.ExportDeliveryDocumentMessageSenderCodes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ExportDeliveryDocumentMessageSenderCode> GetAll()
        {
            return from a in context.ExportDeliveryDocumentMessageSenderCodes  
                   select a;
        }
				 
        public ExportDeliveryDocumentMessageSenderCode GetSingle(EntityKeyFields entityKeys)
        {
            ExportDeliveryDocumentMessageSenderCodeKeys keys = entityKeys as ExportDeliveryDocumentMessageSenderCodeKeys;
            return (from a in context.ExportDeliveryDocumentMessageSenderCodes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ExportDeliveryDocumentMessageSenderCode entity)
        {
            onAdd();
            context.ExportDeliveryDocumentMessageSenderCodes.Add(entity);
        }

        public void Remove(ExportDeliveryDocumentMessageSenderCode entity)
        {
            context.ExportDeliveryDocumentMessageSenderCodes.Attach(entity);
            context.ExportDeliveryDocumentMessageSenderCodes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ExportDeliveryDocumentMessageSenderCode entity)
        {
            onUpdate();
            context.ExportDeliveryDocumentMessageSenderCodes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ExportDeliveryDocumentMessageSenderCode> All()
        {
            return context.ExportDeliveryDocumentMessageSenderCodes.ToList();
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
	 