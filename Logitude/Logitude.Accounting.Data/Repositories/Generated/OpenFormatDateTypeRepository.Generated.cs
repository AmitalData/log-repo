 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Accounting.Data.EntityPOCOs;
using Logitude.Accounting.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Accounting.Data.Repositories
{
   public partial class OpenFormatDateTypeRepository:IRepository<OpenFormatDateType>
   {
   
        private IAccountingContext currentContext;
        public OpenFormatDateTypeRepository(int tenant)
        {
            currentContext = AccountingContext.GetContext(tenant);
        }

        public OpenFormatDateTypeRepository(IAccountingContext context)
        {
            currentContext = context;
        }

		 
		
		public  OpenFormatDateType GetSingle(string code)
        {
            return (from a in context.OpenFormatDateTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<OpenFormatDateType> GetAll()
        {
            return from a in context.OpenFormatDateTypes  
                   select a;
        }
				 
        public OpenFormatDateType GetSingle(EntityKeyFields entityKeys)
        {
            OpenFormatDateTypeKeys keys = entityKeys as OpenFormatDateTypeKeys;
            return (from a in context.OpenFormatDateTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(OpenFormatDateType entity)
        {
            onAdd();
            context.OpenFormatDateTypes.Add(entity);
        }

        public void Remove(OpenFormatDateType entity)
        {
            context.OpenFormatDateTypes.Attach(entity);
            context.OpenFormatDateTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(OpenFormatDateType entity)
        {
            onUpdate();
            context.OpenFormatDateTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<OpenFormatDateType> All()
        {
            return context.OpenFormatDateTypes.ToList();
        }

        private IAccountingContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 