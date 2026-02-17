 
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
   public partial class CurrencyTypeRepository:IRepository<CurrencyType>
   {
   
        private ICustomContext currentContext;
        public CurrencyTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CurrencyTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CurrencyType GetSingle(string code)
        {
            return (from a in context.CurrencyTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CurrencyType> GetAll()
        {
            return from a in context.CurrencyTypes  
                   select a;
        }
				 
        public CurrencyType GetSingle(EntityKeyFields entityKeys)
        {
            CurrencyTypeKeys keys = entityKeys as CurrencyTypeKeys;
            return (from a in context.CurrencyTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CurrencyType entity)
        {
            onAdd();
            context.CurrencyTypes.Add(entity);
        }

        public void Remove(CurrencyType entity)
        {
            context.CurrencyTypes.Attach(entity);
            context.CurrencyTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CurrencyType entity)
        {
            onUpdate();
            context.CurrencyTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CurrencyType> All()
        {
            return context.CurrencyTypes.ToList();
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
	 