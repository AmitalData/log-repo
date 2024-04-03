 
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
   public partial class DepositEssenceTypeRepository:IRepository<DepositEssenceType>
   {
   
        private ICustomContext currentContext;
        public DepositEssenceTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DepositEssenceTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DepositEssenceType GetSingle(string code)
        {
            return (from a in context.DepositEssenceTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<DepositEssenceType> GetAll()
        {
            return from a in context.DepositEssenceTypes  
                   select a;
        }
				 
        public DepositEssenceType GetSingle(EntityKeyFields entityKeys)
        {
            DepositEssenceTypeKeys keys = entityKeys as DepositEssenceTypeKeys;
            return (from a in context.DepositEssenceTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DepositEssenceType entity)
        {
            onAdd();
            context.DepositEssenceTypes.Add(entity);
        }

        public void Remove(DepositEssenceType entity)
        {
            context.DepositEssenceTypes.Attach(entity);
            context.DepositEssenceTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DepositEssenceType entity)
        {
            onUpdate();
            context.DepositEssenceTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DepositEssenceType> All()
        {
            return context.DepositEssenceTypes.ToList();
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
	 