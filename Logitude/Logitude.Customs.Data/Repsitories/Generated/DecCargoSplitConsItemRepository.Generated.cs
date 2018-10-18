 
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
   public partial class DecCargoSplitConsItemRepository:IRepository<DecCargoSplitConsItem>
   {
   
        private ICustomContext currentContext;
        public DecCargoSplitConsItemRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DecCargoSplitConsItemRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DecCargoSplitConsItem GetSingle(string declarationcargosplitid, int? deccargosplitconslineno, int itemline, int tenant)
        {
            return (from a in context.DecCargoSplitConsItems
                    where a.DeclarationCargoSplitId == declarationcargosplitid && a.DecCargoSplitConsLineNo == deccargosplitconslineno && a.ItemLine == itemline && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<DecCargoSplitConsItem> GetAll(int tenant)
        {
            return from a in context.DecCargoSplitConsItems  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public DecCargoSplitConsItem GetSingle(EntityKeyFields entityKeys)
        {
            DecCargoSplitConsItemKeys keys = entityKeys as DecCargoSplitConsItemKeys;
            return (from a in context.DecCargoSplitConsItems
                    where a.DeclarationCargoSplitId == keys.DeclarationCargoSplitId && a.DecCargoSplitConsLineNo == keys.DecCargoSplitConsLineNo && a.ItemLine == keys.ItemLine
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DecCargoSplitConsItem entity)
        {
            onAdd();
            context.DecCargoSplitConsItems.Add(entity);
        }

        public void Remove(DecCargoSplitConsItem entity)
        {
            context.DecCargoSplitConsItems.Attach(entity);
            context.DecCargoSplitConsItems.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DecCargoSplitConsItem entity)
        {
            onUpdate();
            context.DecCargoSplitConsItems.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DecCargoSplitConsItem> All()
        {
            return context.DecCargoSplitConsItems.ToList();
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
	 