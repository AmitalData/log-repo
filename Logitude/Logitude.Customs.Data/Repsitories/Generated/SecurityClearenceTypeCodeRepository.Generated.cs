 
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
   public partial class SecurityClearenceTypeCodeRepository:IRepository<SecurityClearenceTypeCode>
   {
   
        private ICustomContext currentContext;
        public SecurityClearenceTypeCodeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SecurityClearenceTypeCodeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SecurityClearenceTypeCode GetSingle(string code)
        {
            return (from a in context.SecurityClearenceTypeCodes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<SecurityClearenceTypeCode> GetAll()
        {
            return from a in context.SecurityClearenceTypeCodes  
                   select a;
        }
				 
        public SecurityClearenceTypeCode GetSingle(EntityKeyFields entityKeys)
        {
            SecurityClearenceTypeCodeKeys keys = entityKeys as SecurityClearenceTypeCodeKeys;
            return (from a in context.SecurityClearenceTypeCodes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SecurityClearenceTypeCode entity)
        {
            onAdd();
            context.SecurityClearenceTypeCodes.Add(entity);
        }

        public void Remove(SecurityClearenceTypeCode entity)
        {
            context.SecurityClearenceTypeCodes.Attach(entity);
            context.SecurityClearenceTypeCodes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SecurityClearenceTypeCode entity)
        {
            onUpdate();
            context.SecurityClearenceTypeCodes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SecurityClearenceTypeCode> All()
        {
            return context.SecurityClearenceTypeCodes.ToList();
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
	 