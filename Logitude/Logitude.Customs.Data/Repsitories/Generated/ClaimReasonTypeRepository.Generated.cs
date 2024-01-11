 
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
   public partial class ClaimReasonTypeRepository:IRepository<ClaimReasonType>
   {
   
        private ICustomContext currentContext;
        public ClaimReasonTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ClaimReasonTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ClaimReasonType GetSingle(string code)
        {
            return (from a in context.ClaimReasonTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ClaimReasonType> GetAll()
        {
            return from a in context.ClaimReasonTypes  
                   select a;
        }
				 
        public ClaimReasonType GetSingle(EntityKeyFields entityKeys)
        {
            ClaimReasonTypeKeys keys = entityKeys as ClaimReasonTypeKeys;
            return (from a in context.ClaimReasonTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ClaimReasonType entity)
        {
            onAdd();
            context.ClaimReasonTypes.Add(entity);
        }

        public void Remove(ClaimReasonType entity)
        {
            context.ClaimReasonTypes.Attach(entity);
            context.ClaimReasonTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ClaimReasonType entity)
        {
            onUpdate();
            context.ClaimReasonTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ClaimReasonType> All()
        {
            return context.ClaimReasonTypes.ToList();
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
	 