 
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
   public partial class ClaimImporterDeclarsPage3ARepository:IRepository<ClaimImporterDeclarsPage3A>
   {
   
        private ICustomContext currentContext;
        public ClaimImporterDeclarsPage3ARepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ClaimImporterDeclarsPage3ARepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ClaimImporterDeclarsPage3A GetSingle(string claimid, int lineno, int tenant)
        {
            return (from a in context.ClaimImporterDeclarsPage3As
                    where a.ClaimId == claimid && a.LineNo == lineno && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ClaimImporterDeclarsPage3A> GetAll(int tenant)
        {
            return from a in context.ClaimImporterDeclarsPage3As  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ClaimImporterDeclarsPage3A GetSingle(EntityKeyFields entityKeys)
        {
            ClaimImporterDeclarsPage3AKeys keys = entityKeys as ClaimImporterDeclarsPage3AKeys;
            return (from a in context.ClaimImporterDeclarsPage3As
                    where a.ClaimId == keys.ClaimId && a.LineNo == keys.LineNo
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ClaimImporterDeclarsPage3A entity)
        {
            onAdd();
            context.ClaimImporterDeclarsPage3As.Add(entity);
        }

        public void Remove(ClaimImporterDeclarsPage3A entity)
        {
            context.ClaimImporterDeclarsPage3As.Attach(entity);
            context.ClaimImporterDeclarsPage3As.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ClaimImporterDeclarsPage3A entity)
        {
            onUpdate();
            context.ClaimImporterDeclarsPage3As.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ClaimImporterDeclarsPage3A> All()
        {
            return context.ClaimImporterDeclarsPage3As.ToList();
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
	 