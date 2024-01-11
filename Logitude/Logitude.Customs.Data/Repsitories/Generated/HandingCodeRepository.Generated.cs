 
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
   public partial class HandingCodeRepository:IRepository<HandingCode>
   {
   
        private ICustomContext currentContext;
        public HandingCodeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public HandingCodeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  HandingCode GetSingle(string code)
        {
            return (from a in context.HandingCodes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<HandingCode> GetAll()
        {
            return from a in context.HandingCodes  
                   select a;
        }
				 
        public HandingCode GetSingle(EntityKeyFields entityKeys)
        {
            HandingCodeKeys keys = entityKeys as HandingCodeKeys;
            return (from a in context.HandingCodes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(HandingCode entity)
        {
            onAdd();
            context.HandingCodes.Add(entity);
        }

        public void Remove(HandingCode entity)
        {
            context.HandingCodes.Attach(entity);
            context.HandingCodes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(HandingCode entity)
        {
            onUpdate();
            context.HandingCodes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<HandingCode> All()
        {
            return context.HandingCodes.ToList();
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
	 