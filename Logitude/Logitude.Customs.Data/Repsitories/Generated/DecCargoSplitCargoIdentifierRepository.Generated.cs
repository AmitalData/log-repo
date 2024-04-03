 
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
   public partial class DecCargoSplitCargoIdentifierRepository:IRepository<DecCargoSplitCargoIdentifier>
   {
   
        private ICustomContext currentContext;
        public DecCargoSplitCargoIdentifierRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DecCargoSplitCargoIdentifierRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DecCargoSplitCargoIdentifier GetSingle(string declarationcargosplitid, int linenumber, int tenant)
        {
            return (from a in context.DecCargoSplitCargoIdentifiers
                    where a.DeclarationCargoSplitId == declarationcargosplitid && a.LineNumber == linenumber && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<DecCargoSplitCargoIdentifier> GetAll(int tenant)
        {
            return from a in context.DecCargoSplitCargoIdentifiers  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public DecCargoSplitCargoIdentifier GetSingle(EntityKeyFields entityKeys)
        {
            DecCargoSplitCargoIdentifierKeys keys = entityKeys as DecCargoSplitCargoIdentifierKeys;
            return (from a in context.DecCargoSplitCargoIdentifiers
                    where a.DeclarationCargoSplitId == keys.DeclarationCargoSplitId && a.LineNumber == keys.LineNumber
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DecCargoSplitCargoIdentifier entity)
        {
            onAdd();
            context.DecCargoSplitCargoIdentifiers.Add(entity);
        }

        public void Remove(DecCargoSplitCargoIdentifier entity)
        {
            context.DecCargoSplitCargoIdentifiers.Attach(entity);
            context.DecCargoSplitCargoIdentifiers.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DecCargoSplitCargoIdentifier entity)
        {
            onUpdate();
            context.DecCargoSplitCargoIdentifiers.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DecCargoSplitCargoIdentifier> All()
        {
            return context.DecCargoSplitCargoIdentifiers.ToList();
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
	 