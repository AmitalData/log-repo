 
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
   public partial class DecCargoSplitConsPackDetRepository:IRepository<DecCargoSplitConsPackDet>
   {
   
        private ICustomContext currentContext;
        public DecCargoSplitConsPackDetRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DecCargoSplitConsPackDetRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DecCargoSplitConsPackDet GetSingle(string declarationcargosplitid, int? deccargosplitconslineno, int deccargosplitconsitemline, int packageline, int tenant)
        {
            return (from a in context.DecCargoSplitConsPackDets
                    where a.DeclarationCargoSplitId == declarationcargosplitid && a.DecCargoSplitConsLineNo == deccargosplitconslineno && a.DecCargoSplitConsItemLine == deccargosplitconsitemline && a.PackageLine == packageline && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<DecCargoSplitConsPackDet> GetAll(int tenant)
        {
            return from a in context.DecCargoSplitConsPackDets  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public DecCargoSplitConsPackDet GetSingle(EntityKeyFields entityKeys)
        {
            DecCargoSplitConsPackDetKeys keys = entityKeys as DecCargoSplitConsPackDetKeys;
            return (from a in context.DecCargoSplitConsPackDets
                    where a.DeclarationCargoSplitId == keys.DeclarationCargoSplitId && a.DecCargoSplitConsLineNo == keys.DecCargoSplitConsLineNo && a.DecCargoSplitConsItemLine == keys.DecCargoSplitConsItemLine && a.PackageLine == keys.PackageLine
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DecCargoSplitConsPackDet entity)
        {
            onAdd();
            context.DecCargoSplitConsPackDets.Add(entity);
        }

        public void Remove(DecCargoSplitConsPackDet entity)
        {
            context.DecCargoSplitConsPackDets.Attach(entity);
            context.DecCargoSplitConsPackDets.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DecCargoSplitConsPackDet entity)
        {
            onUpdate();
            context.DecCargoSplitConsPackDets.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DecCargoSplitConsPackDet> All()
        {
            return context.DecCargoSplitConsPackDets.ToList();
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
	 