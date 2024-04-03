 
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
   public partial class ClaimImporterDeclarsPage3BRepository:IRepository<ClaimImporterDeclarsPage3B>
   {
   
        private ICustomContext currentContext;
        public ClaimImporterDeclarsPage3BRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ClaimImporterDeclarsPage3BRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ClaimImporterDeclarsPage3B GetSingle(string claimid, int lineno, int tenant)
        {
            return (from a in context.ClaimImporterDeclarsPage3Bs
                    where a.ClaimId == claimid && a.LineNo == lineno && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ClaimImporterDeclarsPage3B> GetAll(int tenant)
        {
            return from a in context.ClaimImporterDeclarsPage3Bs  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ClaimImporterDeclarsPage3B GetSingle(EntityKeyFields entityKeys)
        {
            ClaimImporterDeclarsPage3BKeys keys = entityKeys as ClaimImporterDeclarsPage3BKeys;
            return (from a in context.ClaimImporterDeclarsPage3Bs
                    where a.ClaimId == keys.ClaimId && a.LineNo == keys.LineNo
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ClaimImporterDeclarsPage3B entity)
        {
            onAdd();
            context.ClaimImporterDeclarsPage3Bs.Add(entity);
        }

        public void Remove(ClaimImporterDeclarsPage3B entity)
        {
            context.ClaimImporterDeclarsPage3Bs.Attach(entity);
            context.ClaimImporterDeclarsPage3Bs.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ClaimImporterDeclarsPage3B entity)
        {
            onUpdate();
            context.ClaimImporterDeclarsPage3Bs.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ClaimImporterDeclarsPage3B> All()
        {
            return context.ClaimImporterDeclarsPage3Bs.ToList();
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
	 