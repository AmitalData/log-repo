 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.CRM.Data.Repsitories
{
   public partial class CorrespondencesAttachmentRepository:IRepository<CorrespondencesAttachment>
   {
   
        private ICRMContext currentContext;
        public CorrespondencesAttachmentRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public CorrespondencesAttachmentRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  CorrespondencesAttachment GetSingle(string id, int tenant)
        {
            return (from a in context.CorrespondencesAttachments
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<CorrespondencesAttachment> GetAll(int tenant)
        {
            return from a in context.CorrespondencesAttachments  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public CorrespondencesAttachment GetSingle(EntityKeyFields entityKeys)
        {
            CorrespondencesAttachmentKeys keys = entityKeys as CorrespondencesAttachmentKeys;
            return (from a in context.CorrespondencesAttachments
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CorrespondencesAttachment entity)
        {
            onAdd();
            context.CorrespondencesAttachments.Add(entity);
        }

        public void Remove(CorrespondencesAttachment entity)
        {
            context.CorrespondencesAttachments.Attach(entity);
            context.CorrespondencesAttachments.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CorrespondencesAttachment entity)
        {
            onUpdate();
            context.CorrespondencesAttachments.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CorrespondencesAttachment> All()
        {
            return context.CorrespondencesAttachments.ToList();
        }

        private ICRMContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 