 
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
   public partial class DecCargoSplitConRepository:IRepository<DecCargoSplitCon>
   {
   
        private ICustomContext currentContext;
        public DecCargoSplitConRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DecCargoSplitConRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DecCargoSplitCon GetSingle(string declarationcargosplitid, int linenumber, int tenant)
        {
            return (from a in context.DecCargoSplitCons
                    where a.DeclarationCargoSplitId == declarationcargosplitid && a.LineNumber == linenumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<DecCargoSplitCon> GetAll(int tenant)
        {
            return from a in context.DecCargoSplitCons  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public DecCargoSplitCon GetSingle(EntityKeyFields entityKeys)
        {
            DecCargoSplitConKeys keys = entityKeys as DecCargoSplitConKeys;
            return (from a in context.DecCargoSplitCons
                    where a.DeclarationCargoSplitId == keys.DeclarationCargoSplitId && a.LineNumber == keys.LineNumber
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DecCargoSplitCon entity)
        {
            onAdd();
            context.DecCargoSplitCons.Add(entity);
        }

        public void Remove(DecCargoSplitCon entity)
        {
            context.DecCargoSplitCons.Attach(entity);
            context.DecCargoSplitCons.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DecCargoSplitCon entity)
        {
            onUpdate();
            context.DecCargoSplitCons.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DecCargoSplitCon> All()
        {
            return context.DecCargoSplitCons.ToList();
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
	 