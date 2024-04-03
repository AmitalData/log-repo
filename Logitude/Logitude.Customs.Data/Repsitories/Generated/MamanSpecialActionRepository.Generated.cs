 
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
   public partial class MamanSpecialActionRepository:IRepository<MamanSpecialAction>
   {
   
        private ICustomContext currentContext;
        public MamanSpecialActionRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public MamanSpecialActionRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  MamanSpecialAction GetSingle(string code)
        {
            return (from a in context.MamanSpecialActions
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<MamanSpecialAction> GetAll()
        {
            return from a in context.MamanSpecialActions  
                   select a;
        }
				 
        public MamanSpecialAction GetSingle(EntityKeyFields entityKeys)
        {
            MamanSpecialActionKeys keys = entityKeys as MamanSpecialActionKeys;
            return (from a in context.MamanSpecialActions
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(MamanSpecialAction entity)
        {
            onAdd();
            context.MamanSpecialActions.Add(entity);
        }

        public void Remove(MamanSpecialAction entity)
        {
            context.MamanSpecialActions.Attach(entity);
            context.MamanSpecialActions.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(MamanSpecialAction entity)
        {
            onUpdate();
            context.MamanSpecialActions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<MamanSpecialAction> All()
        {
            return context.MamanSpecialActions.ToList();
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
	 