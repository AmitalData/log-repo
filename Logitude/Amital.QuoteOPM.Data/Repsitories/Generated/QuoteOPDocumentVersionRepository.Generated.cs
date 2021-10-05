 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Amital.QuoteOPM.Data.EntityPOCOs;
using Amital.QuoteOPM.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Amital.QuoteOPM.Data.Repsitories
{
   public partial class QuoteOPDocumentVersionRepository:IRepository<QuoteOPDocumentVersion>
   {
   
        private IQuoteOPMContext currentContext;
        public QuoteOPDocumentVersionRepository(int tenant)
        {
            currentContext = QuoteOPMContext.GetContext(tenant);
        }

        public QuoteOPDocumentVersionRepository(IQuoteOPMContext context)
        {
            currentContext = context;
        }

		 
		
		public  QuoteOPDocumentVersion GetSingle(string quoteopid, int versionnumber, int tenant)
        {
            return (from a in context.QuoteOPDocumentVersions
                    where a.QuoteOPId == quoteopid && a.VersionNumber == versionnumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<QuoteOPDocumentVersion> GetAll(int tenant)
        {
            return from a in context.QuoteOPDocumentVersions  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public QuoteOPDocumentVersion GetSingle(EntityKeyFields entityKeys)
        {
            QuoteOPDocumentVersionKeys keys = entityKeys as QuoteOPDocumentVersionKeys;
            return (from a in context.QuoteOPDocumentVersions
                    where a.QuoteOPId == keys.QuoteOPId && a.VersionNumber == keys.VersionNumber
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(QuoteOPDocumentVersion entity)
        {
            onAdd();
            context.QuoteOPDocumentVersions.Add(entity);
        }

        public void Remove(QuoteOPDocumentVersion entity)
        {
            context.QuoteOPDocumentVersions.Attach(entity);
            context.QuoteOPDocumentVersions.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(QuoteOPDocumentVersion entity)
        {
            onUpdate();
            context.QuoteOPDocumentVersions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuoteOPDocumentVersion> All()
        {
            return context.QuoteOPDocumentVersions.ToList();
        }

        private IQuoteOPMContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 