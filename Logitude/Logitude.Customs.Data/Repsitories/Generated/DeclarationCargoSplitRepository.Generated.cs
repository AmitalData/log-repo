 
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
   public partial class DeclarationCargoSplitRepository:IRepository<DeclarationCargoSplit>
   {
   
        private ICustomContext currentContext;
        public DeclarationCargoSplitRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DeclarationCargoSplitRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DeclarationCargoSplit GetSingle(string id, int tenant)
        {
            return (from a in context.DeclarationCargoSplits
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<DeclarationCargoSplit> GetAll(int tenant)
        {
            return from a in context.DeclarationCargoSplits  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public DeclarationCargoSplit GetSingle(EntityKeyFields entityKeys)
        {
            DeclarationCargoSplitKeys keys = entityKeys as DeclarationCargoSplitKeys;
            return (from a in context.DeclarationCargoSplits
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DeclarationCargoSplit entity)
        {
            onAdd();
            context.DeclarationCargoSplits.Add(entity);
        }

        public void Remove(DeclarationCargoSplit entity)
        {
            context.DeclarationCargoSplits.Attach(entity);
            context.DeclarationCargoSplits.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DeclarationCargoSplit entity)
        {
            onUpdate();
            context.DeclarationCargoSplits.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DeclarationCargoSplit> All()
        {
            return context.DeclarationCargoSplits.ToList();
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
	 