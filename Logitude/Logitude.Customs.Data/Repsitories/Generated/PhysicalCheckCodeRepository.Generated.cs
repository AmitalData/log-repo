 
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
   public partial class PhysicalCheckCodeRepository:IRepository<PhysicalCheckCode>
   {
   
        private ICustomContext currentContext;
        public PhysicalCheckCodeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public PhysicalCheckCodeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  PhysicalCheckCode GetSingle(string code)
        {
            return (from a in context.PhysicalCheckCodes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<PhysicalCheckCode> GetAll()
        {
            return from a in context.PhysicalCheckCodes  
                   select a;
        }
				 
        public PhysicalCheckCode GetSingle(EntityKeyFields entityKeys)
        {
            PhysicalCheckCodeKeys keys = entityKeys as PhysicalCheckCodeKeys;
            return (from a in context.PhysicalCheckCodes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PhysicalCheckCode entity)
        {
            onAdd();
            context.PhysicalCheckCodes.Add(entity);
        }

        public void Remove(PhysicalCheckCode entity)
        {
            context.PhysicalCheckCodes.Attach(entity);
            context.PhysicalCheckCodes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PhysicalCheckCode entity)
        {
            onUpdate();
            context.PhysicalCheckCodes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PhysicalCheckCode> All()
        {
            return context.PhysicalCheckCodes.ToList();
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
	 