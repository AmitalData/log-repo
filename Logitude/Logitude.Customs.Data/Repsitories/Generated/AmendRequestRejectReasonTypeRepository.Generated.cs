 
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
   public partial class AmendRequestRejectReasonTypeRepository:IRepository<AmendRequestRejectReasonType>
   {
   
        private ICustomContext currentContext;
        public AmendRequestRejectReasonTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public AmendRequestRejectReasonTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  AmendRequestRejectReasonType GetSingle(string code)
        {
            return (from a in context.AmendRequestRejectReasonTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<AmendRequestRejectReasonType> GetAll()
        {
            return from a in context.AmendRequestRejectReasonTypes  
                   select a;
        }
				 
        public AmendRequestRejectReasonType GetSingle(EntityKeyFields entityKeys)
        {
            AmendRequestRejectReasonTypeKeys keys = entityKeys as AmendRequestRejectReasonTypeKeys;
            return (from a in context.AmendRequestRejectReasonTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(AmendRequestRejectReasonType entity)
        {
            onAdd();
            context.AmendRequestRejectReasonTypes.Add(entity);
        }

        public void Remove(AmendRequestRejectReasonType entity)
        {
            context.AmendRequestRejectReasonTypes.Attach(entity);
            context.AmendRequestRejectReasonTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(AmendRequestRejectReasonType entity)
        {
            onUpdate();
            context.AmendRequestRejectReasonTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AmendRequestRejectReasonType> All()
        {
            return context.AmendRequestRejectReasonTypes.ToList();
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
	 