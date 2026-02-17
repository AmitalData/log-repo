 
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
   public partial class ContinuousMessagesTypeCodeRepository:IRepository<ContinuousMessagesTypeCode>
   {
   
        private ICustomContext currentContext;
        public ContinuousMessagesTypeCodeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ContinuousMessagesTypeCodeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ContinuousMessagesTypeCode GetSingle(string code)
        {
            return (from a in context.ContinuousMessagesTypeCodes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ContinuousMessagesTypeCode> GetAll()
        {
            return from a in context.ContinuousMessagesTypeCodes  
                   select a;
        }
				 
        public ContinuousMessagesTypeCode GetSingle(EntityKeyFields entityKeys)
        {
            ContinuousMessagesTypeCodeKeys keys = entityKeys as ContinuousMessagesTypeCodeKeys;
            return (from a in context.ContinuousMessagesTypeCodes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ContinuousMessagesTypeCode entity)
        {
            onAdd();
            context.ContinuousMessagesTypeCodes.Add(entity);
        }

        public void Remove(ContinuousMessagesTypeCode entity)
        {
            context.ContinuousMessagesTypeCodes.Attach(entity);
            context.ContinuousMessagesTypeCodes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ContinuousMessagesTypeCode entity)
        {
            onUpdate();
            context.ContinuousMessagesTypeCodes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ContinuousMessagesTypeCode> All()
        {
            return context.ContinuousMessagesTypeCodes.ToList();
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
	 