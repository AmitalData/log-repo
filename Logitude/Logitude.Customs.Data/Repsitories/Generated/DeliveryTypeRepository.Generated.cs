 
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
   public partial class DeliveryTypeRepository:IRepository<DeliveryType>
   {
   
        private ICustomContext currentContext;
        public DeliveryTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DeliveryTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DeliveryType GetSingle(string code)
        {
            return (from a in context.DeliveryTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<DeliveryType> GetAll()
        {
            return from a in context.DeliveryTypes  
                   select a;
        }
				 
        public DeliveryType GetSingle(EntityKeyFields entityKeys)
        {
            DeliveryTypeKeys keys = entityKeys as DeliveryTypeKeys;
            return (from a in context.DeliveryTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DeliveryType entity)
        {
            onAdd();
            context.DeliveryTypes.Add(entity);
        }

        public void Remove(DeliveryType entity)
        {
            context.DeliveryTypes.Attach(entity);
            context.DeliveryTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DeliveryType entity)
        {
            onUpdate();
            context.DeliveryTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DeliveryType> All()
        {
            return context.DeliveryTypes.ToList();
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
	 