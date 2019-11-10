 
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
   public partial class ConsignmentPackDangerRepository:IRepository<ConsignmentPackDanger>
   {
   
        private ICustomContext currentContext;
        public ConsignmentPackDangerRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ConsignmentPackDangerRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ConsignmentPackDanger GetSingle(string declarationid, int? consignmentnumber, int? linenumber, int? dangerouslineno, int tenant)
        {
            return (from a in context.ConsignmentPackDangers
                    where a.DeclarationId == declarationid && a.ConsignmentNumber == consignmentnumber && a.LineNumber == linenumber && a.DangerousLineNo == dangerouslineno && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ConsignmentPackDanger> GetAll(int tenant)
        {
            return from a in context.ConsignmentPackDangers  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ConsignmentPackDanger GetSingle(EntityKeyFields entityKeys)
        {
            ConsignmentPackDangerKeys keys = entityKeys as ConsignmentPackDangerKeys;
            return (from a in context.ConsignmentPackDangers
                    where a.DeclarationId == keys.DeclarationId && a.ConsignmentNumber == keys.ConsignmentNumber && a.LineNumber == keys.LineNumber && a.DangerousLineNo == keys.DangerousLineNo
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ConsignmentPackDanger entity)
        {
            onAdd();
            context.ConsignmentPackDangers.Add(entity);
        }

        public void Remove(ConsignmentPackDanger entity)
        {
            context.ConsignmentPackDangers.Attach(entity);
            context.ConsignmentPackDangers.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ConsignmentPackDanger entity)
        {
            onUpdate();
            context.ConsignmentPackDangers.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ConsignmentPackDanger> All()
        {
            return context.ConsignmentPackDangers.ToList();
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
	 