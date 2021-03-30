 
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
   public partial class NDMessageActionCodeRepository:IRepository<NDMessageActionCode>
   {
   
        private ICustomContext currentContext;
        public NDMessageActionCodeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public NDMessageActionCodeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  NDMessageActionCode GetSingle(string code)
        {
            return (from a in context.NDMessageActionCodes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<NDMessageActionCode> GetAll()
        {
            return from a in context.NDMessageActionCodes  
                   select a;
        }
				 
        public NDMessageActionCode GetSingle(EntityKeyFields entityKeys)
        {
            NDMessageActionCodeKeys keys = entityKeys as NDMessageActionCodeKeys;
            return (from a in context.NDMessageActionCodes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(NDMessageActionCode entity)
        {
            onAdd();
            context.NDMessageActionCodes.Add(entity);
        }

        public void Remove(NDMessageActionCode entity)
        {
            context.NDMessageActionCodes.Attach(entity);
            context.NDMessageActionCodes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(NDMessageActionCode entity)
        {
            onUpdate();
            context.NDMessageActionCodes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<NDMessageActionCode> All()
        {
            return context.NDMessageActionCodes.ToList();
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
	 