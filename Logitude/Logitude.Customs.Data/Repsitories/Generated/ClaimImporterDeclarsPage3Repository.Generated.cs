 
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
   public partial class ClaimImporterDeclarsPage3Repository:IRepository<ClaimImporterDeclarsPage3>
   {
   
        private ICustomContext currentContext;
        public ClaimImporterDeclarsPage3Repository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ClaimImporterDeclarsPage3Repository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ClaimImporterDeclarsPage3 GetSingle(string claimid, int lineno, int tenant)
        {
            return (from a in context.ClaimImporterDeclarsPage3s
                    where a.ClaimId == claimid && a.LineNo == lineno && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ClaimImporterDeclarsPage3> GetAll(int tenant)
        {
            return from a in context.ClaimImporterDeclarsPage3s  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ClaimImporterDeclarsPage3 GetSingle(EntityKeyFields entityKeys)
        {
            ClaimImporterDeclarsPage3Keys keys = entityKeys as ClaimImporterDeclarsPage3Keys;
            return (from a in context.ClaimImporterDeclarsPage3s
                    where a.ClaimId == keys.ClaimId && a.LineNo == keys.LineNo
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ClaimImporterDeclarsPage3 entity)
        {
            onAdd();
            context.ClaimImporterDeclarsPage3s.Add(entity);
        }

        public void Remove(ClaimImporterDeclarsPage3 entity)
        {
            context.ClaimImporterDeclarsPage3s.Attach(entity);
            context.ClaimImporterDeclarsPage3s.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ClaimImporterDeclarsPage3 entity)
        {
            onUpdate();
            context.ClaimImporterDeclarsPage3s.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ClaimImporterDeclarsPage3> All()
        {
            return context.ClaimImporterDeclarsPage3s.ToList();
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
	 