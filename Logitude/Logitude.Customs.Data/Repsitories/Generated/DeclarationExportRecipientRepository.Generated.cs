 
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
   public partial class DeclarationExportRecipientRepository:IRepository<DeclarationExportRecipient>
   {
   
        private ICustomContext currentContext;
        public DeclarationExportRecipientRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DeclarationExportRecipientRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DeclarationExportRecipient GetSingle(string declarationid, int? linenumber, int tenant)
        {
            return (from a in context.DeclarationExportRecipients
                    where a.DeclarationId == declarationid && a.LineNumber == linenumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<DeclarationExportRecipient> GetAll(int tenant)
        {
            return from a in context.DeclarationExportRecipients  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public DeclarationExportRecipient GetSingle(EntityKeyFields entityKeys)
        {
            DeclarationExportRecipientKeys keys = entityKeys as DeclarationExportRecipientKeys;
            return (from a in context.DeclarationExportRecipients
                    where a.DeclarationId == keys.DeclarationId && a.LineNumber == keys.LineNumber
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DeclarationExportRecipient entity)
        {
            onAdd();
            context.DeclarationExportRecipients.Add(entity);
        }

        public void Remove(DeclarationExportRecipient entity)
        {
            context.DeclarationExportRecipients.Attach(entity);
            context.DeclarationExportRecipients.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DeclarationExportRecipient entity)
        {
            onUpdate();
            context.DeclarationExportRecipients.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DeclarationExportRecipient> All()
        {
            return context.DeclarationExportRecipients.ToList();
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
	 