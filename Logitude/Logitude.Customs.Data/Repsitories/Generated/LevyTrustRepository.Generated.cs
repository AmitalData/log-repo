 
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
   public partial class LevyTrustRepository:IRepository<LevyTrust>
   {
   
        private ICustomContext currentContext;
        public LevyTrustRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public LevyTrustRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  LevyTrust GetSingle(string code)
        {
            return (from a in context.LevyTrusts
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<LevyTrust> GetAll()
        {
            return from a in context.LevyTrusts  
                   select a;
        }
				 
        public LevyTrust GetSingle(EntityKeyFields entityKeys)
        {
            LevyTrustKeys keys = entityKeys as LevyTrustKeys;
            return (from a in context.LevyTrusts
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(LevyTrust entity)
        {
            onAdd();
            context.LevyTrusts.Add(entity);
        }

        public void Remove(LevyTrust entity)
        {
            context.LevyTrusts.Attach(entity);
            context.LevyTrusts.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(LevyTrust entity)
        {
            onUpdate();
            context.LevyTrusts.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<LevyTrust> All()
        {
            return context.LevyTrusts.ToList();
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
	 