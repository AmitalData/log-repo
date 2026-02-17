 
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
   public partial class ClaimImporterDeclarsP3LoiRepository:IRepository<ClaimImporterDeclarsP3Loi>
   {
   
        private ICustomContext currentContext;
        public ClaimImporterDeclarsP3LoiRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ClaimImporterDeclarsP3LoiRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ClaimImporterDeclarsP3Loi GetSingle(string claimid, int counterkey, int lineno, int tenant)
        {
            return (from a in context.ClaimImporterDeclarsP3Lois
                    where a.ClaimId == claimid && a.CounterKey == counterkey && a.LineNo == lineno && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ClaimImporterDeclarsP3Loi> GetAll(int tenant)
        {
            return from a in context.ClaimImporterDeclarsP3Lois  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ClaimImporterDeclarsP3Loi GetSingle(EntityKeyFields entityKeys)
        {
            ClaimImporterDeclarsP3LoiKeys keys = entityKeys as ClaimImporterDeclarsP3LoiKeys;
            return (from a in context.ClaimImporterDeclarsP3Lois
                    where a.ClaimId == keys.ClaimId && a.CounterKey == keys.CounterKey && a.LineNo == keys.LineNo
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ClaimImporterDeclarsP3Loi entity)
        {
            onAdd();
            context.ClaimImporterDeclarsP3Lois.Add(entity);
        }

        public void Remove(ClaimImporterDeclarsP3Loi entity)
        {
            context.ClaimImporterDeclarsP3Lois.Attach(entity);
            context.ClaimImporterDeclarsP3Lois.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ClaimImporterDeclarsP3Loi entity)
        {
            onUpdate();
            context.ClaimImporterDeclarsP3Lois.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ClaimImporterDeclarsP3Loi> All()
        {
            return context.ClaimImporterDeclarsP3Lois.ToList();
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
	 