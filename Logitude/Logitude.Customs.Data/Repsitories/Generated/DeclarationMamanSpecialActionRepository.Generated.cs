 
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
   public partial class DeclarationMamanSpecialActionRepository:IRepository<DeclarationMamanSpecialAction>
   {
   
        private ICustomContext currentContext;
        public DeclarationMamanSpecialActionRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DeclarationMamanSpecialActionRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DeclarationMamanSpecialAction GetSingle(string declarationid, string mamanspecialactioncode, int tenant)
        {
            return (from a in context.DeclarationMamanSpecialActions
                    where a.DeclarationId == declarationid && a.MamanSpecialActionCode == mamanspecialactioncode && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<DeclarationMamanSpecialAction> GetAll(int tenant)
        {
            return from a in context.DeclarationMamanSpecialActions  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public DeclarationMamanSpecialAction GetSingle(EntityKeyFields entityKeys)
        {
            DeclarationMamanSpecialActionKeys keys = entityKeys as DeclarationMamanSpecialActionKeys;
            return (from a in context.DeclarationMamanSpecialActions
                    where a.DeclarationId == keys.DeclarationId && a.MamanSpecialActionCode == keys.MamanSpecialActionCode
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DeclarationMamanSpecialAction entity)
        {
            onAdd();
            context.DeclarationMamanSpecialActions.Add(entity);
        }

        public void Remove(DeclarationMamanSpecialAction entity)
        {
            context.DeclarationMamanSpecialActions.Attach(entity);
            context.DeclarationMamanSpecialActions.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DeclarationMamanSpecialAction entity)
        {
            onUpdate();
            context.DeclarationMamanSpecialActions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DeclarationMamanSpecialAction> All()
        {
            return context.DeclarationMamanSpecialActions.ToList();
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
	 