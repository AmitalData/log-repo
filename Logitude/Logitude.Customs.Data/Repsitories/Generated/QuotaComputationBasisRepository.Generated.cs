 
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
   public partial class QuotaComputationBasisRepository:IRepository<QuotaComputationBasis>
   {
   
        private ICustomContext currentContext;
        public QuotaComputationBasisRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public QuotaComputationBasisRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  QuotaComputationBasis GetSingle(string code)
        {
            return (from a in context.QuotaComputationBasises
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<QuotaComputationBasis> GetAll()
        {
            return from a in context.QuotaComputationBasises  
                   select a;
        }
				 
        public QuotaComputationBasis GetSingle(EntityKeyFields entityKeys)
        {
            QuotaComputationBasisKeys keys = entityKeys as QuotaComputationBasisKeys;
            return (from a in context.QuotaComputationBasises
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(QuotaComputationBasis entity)
        {
            onAdd();
            context.QuotaComputationBasises.Add(entity);
        }

        public void Remove(QuotaComputationBasis entity)
        {
            context.QuotaComputationBasises.Attach(entity);
            context.QuotaComputationBasises.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(QuotaComputationBasis entity)
        {
            onUpdate();
            context.QuotaComputationBasises.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<QuotaComputationBasis> All()
        {
            return context.QuotaComputationBasises.ToList();
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
	 