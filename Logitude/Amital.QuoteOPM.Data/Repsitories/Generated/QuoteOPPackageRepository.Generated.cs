 
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
   public partial class QuoteOPPackageRepository:IRepository<QuoteOPPackage>
   {
   
        private IQuoteOPMContext currentContext;
        public QuoteOPPackageRepository(int tenant)
        {
            currentContext = QuoteOPMContext.GetContext(tenant);
        }

        public QuoteOPPackageRepository(IQuoteOPMContext context)
        {
            currentContext = context;
        }

		 
		
		public  QuoteOPPackage GetSingle(string id, int tenant)
        {
            return (from a in context.QuoteOPPackages
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<QuoteOPPackage> GetAll(int tenant)
        {
            return from a in context.QuoteOPPackages  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public QuoteOPPackage GetSingle(EntityKeyFields entityKeys)
        {
            QuoteOPPackageKeys keys = entityKeys as QuoteOPPackageKeys;
            return (from a in context.QuoteOPPackages
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(QuoteOPPackage entity)
        {
            onAdd();
            context.QuoteOPPackages.Add(entity);
        }

        public void Remove(QuoteOPPackage entity)
        {
            context.QuoteOPPackages.Attach(entity);
            context.QuoteOPPackages.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(QuoteOPPackage entity)
        {
            onUpdate();
            context.QuoteOPPackages.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuoteOPPackage> All()
        {
            return context.QuoteOPPackages.ToList();
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
	 