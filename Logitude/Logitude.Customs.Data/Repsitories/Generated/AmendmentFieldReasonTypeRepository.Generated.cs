 
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
   public partial class AmendmentFieldReasonTypeRepository:IRepository<AmendmentFieldReasonType>
   {
   
        private ICustomContext currentContext;
        public AmendmentFieldReasonTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public AmendmentFieldReasonTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  AmendmentFieldReasonType GetSingle(string code)
        {
            return (from a in context.AmendmentFieldReasonTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<AmendmentFieldReasonType> GetAll()
        {
            return from a in context.AmendmentFieldReasonTypes  
                   select a;
        }
				 
        public AmendmentFieldReasonType GetSingle(EntityKeyFields entityKeys)
        {
            AmendmentFieldReasonTypeKeys keys = entityKeys as AmendmentFieldReasonTypeKeys;
            return (from a in context.AmendmentFieldReasonTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(AmendmentFieldReasonType entity)
        {
            onAdd();
            context.AmendmentFieldReasonTypes.Add(entity);
        }

        public void Remove(AmendmentFieldReasonType entity)
        {
            context.AmendmentFieldReasonTypes.Attach(entity);
            context.AmendmentFieldReasonTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(AmendmentFieldReasonType entity)
        {
            onUpdate();
            context.AmendmentFieldReasonTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AmendmentFieldReasonType> All()
        {
            return context.AmendmentFieldReasonTypes.ToList();
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
	 