 
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
   public partial class AmountTypeRepository:IRepository<AmountType>
   {
   
        private ICustomContext currentContext;
        public AmountTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public AmountTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  AmountType GetSingle(string code)
        {
            return (from a in context.AmountTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<AmountType> GetAll()
        {
            return from a in context.AmountTypes  
                   select a;
        }
				 
        public AmountType GetSingle(EntityKeyFields entityKeys)
        {
            AmountTypeKeys keys = entityKeys as AmountTypeKeys;
            return (from a in context.AmountTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(AmountType entity)
        {
            onAdd();
            context.AmountTypes.Add(entity);
        }

        public void Remove(AmountType entity)
        {
            context.AmountTypes.Attach(entity);
            context.AmountTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(AmountType entity)
        {
            onUpdate();
            context.AmountTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AmountType> All()
        {
            return context.AmountTypes.ToList();
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
	 