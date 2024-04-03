 
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
   public partial class ClaimExplanationCodeRepository:IRepository<ClaimExplanationCode>
   {
   
        private ICustomContext currentContext;
        public ClaimExplanationCodeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ClaimExplanationCodeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ClaimExplanationCode GetSingle(string code)
        {
            return (from a in context.ClaimExplanationCodes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ClaimExplanationCode> GetAll()
        {
            return from a in context.ClaimExplanationCodes  
                   select a;
        }
				 
        public ClaimExplanationCode GetSingle(EntityKeyFields entityKeys)
        {
            ClaimExplanationCodeKeys keys = entityKeys as ClaimExplanationCodeKeys;
            return (from a in context.ClaimExplanationCodes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ClaimExplanationCode entity)
        {
            onAdd();
            context.ClaimExplanationCodes.Add(entity);
        }

        public void Remove(ClaimExplanationCode entity)
        {
            context.ClaimExplanationCodes.Attach(entity);
            context.ClaimExplanationCodes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ClaimExplanationCode entity)
        {
            onUpdate();
            context.ClaimExplanationCodes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ClaimExplanationCode> All()
        {
            return context.ClaimExplanationCodes.ToList();
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
	 