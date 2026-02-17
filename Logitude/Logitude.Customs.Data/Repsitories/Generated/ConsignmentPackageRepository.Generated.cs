 
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
   public partial class ConsignmentPackageRepository:IRepository<ConsignmentPackage>
   {
   
        private ICustomContext currentContext;
        public ConsignmentPackageRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ConsignmentPackageRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ConsignmentPackage GetSingle(string declarationid, int? consignmentnumber, int linenumber, int tenant)
        {
            return (from a in context.ConsignmentPackages
                    where a.DeclarationId == declarationid && a.ConsignmentNumber == consignmentnumber && a.LineNumber == linenumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ConsignmentPackage> GetAll(int tenant)
        {
            return from a in context.ConsignmentPackages  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ConsignmentPackage GetSingle(EntityKeyFields entityKeys)
        {
            ConsignmentPackageKeys keys = entityKeys as ConsignmentPackageKeys;
            return (from a in context.ConsignmentPackages
                    where a.DeclarationId == keys.DeclarationId && a.ConsignmentNumber == keys.ConsignmentNumber && a.LineNumber == keys.LineNumber
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ConsignmentPackage entity)
        {
            onAdd();
            context.ConsignmentPackages.Add(entity);
        }

        public void Remove(ConsignmentPackage entity)
        {
            context.ConsignmentPackages.Attach(entity);
            context.ConsignmentPackages.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ConsignmentPackage entity)
        {
            onUpdate();
            context.ConsignmentPackages.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ConsignmentPackage> All()
        {
            return context.ConsignmentPackages.ToList();
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
	 