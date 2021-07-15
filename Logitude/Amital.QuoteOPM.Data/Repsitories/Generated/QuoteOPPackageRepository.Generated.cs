 
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
   public partial class QuoteopPackageRepository:IRepository<QuoteopPackage>
   {
   
        private IQuoteOPMContext currentContext;
        public QuoteopPackageRepository(int tenant)
        {
            currentContext = QuoteOPMContext.GetContext(tenant);
        }

        public QuoteopPackageRepository(IQuoteOPMContext context)
        {
            currentContext = context;
        }

		 
		
		public  QuoteopPackage GetSingle(string id, int tenant)
        {
            return (from a in context.QuoteopPackages
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<QuoteopPackage> GetAll(int tenant)
        {
            return from a in context.QuoteopPackages  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public QuoteopPackage GetSingle(EntityKeyFields entityKeys)
        {
            QuoteopPackageKeys keys = entityKeys as QuoteopPackageKeys;
            return (from a in context.QuoteopPackages
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(QuoteopPackage entity)
        {
            onAdd();
            context.QuoteopPackages.Add(entity);
        }

        public void Remove(QuoteopPackage entity)
        {
            context.QuoteopPackages.Attach(entity);
            context.QuoteopPackages.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(QuoteopPackage entity)
        {
            onUpdate();
            context.QuoteopPackages.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuoteopPackage> All()
        {
            return context.QuoteopPackages.ToList();
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
	 