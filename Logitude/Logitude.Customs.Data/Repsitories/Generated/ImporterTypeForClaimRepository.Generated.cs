 
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
   public partial class ImporterTypeForClaimRepository:IRepository<ImporterTypeForClaim>
   {
   
        private ICustomContext currentContext;
        public ImporterTypeForClaimRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ImporterTypeForClaimRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ImporterTypeForClaim GetSingle(string code)
        {
            return (from a in context.ImporterTypeForClaims
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ImporterTypeForClaim> GetAll()
        {
            return from a in context.ImporterTypeForClaims  
                   select a;
        }
				 
        public ImporterTypeForClaim GetSingle(EntityKeyFields entityKeys)
        {
            ImporterTypeForClaimKeys keys = entityKeys as ImporterTypeForClaimKeys;
            return (from a in context.ImporterTypeForClaims
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ImporterTypeForClaim entity)
        {
            onAdd();
            context.ImporterTypeForClaims.Add(entity);
        }

        public void Remove(ImporterTypeForClaim entity)
        {
            context.ImporterTypeForClaims.Attach(entity);
            context.ImporterTypeForClaims.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ImporterTypeForClaim entity)
        {
            onUpdate();
            context.ImporterTypeForClaims.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ImporterTypeForClaim> All()
        {
            return context.ImporterTypeForClaims.ToList();
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
	 