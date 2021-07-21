 
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
   public partial class BuyerRoleTypeRepository:IRepository<BuyerRoleType>
   {
   
        private ICustomContext currentContext;
        public BuyerRoleTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public BuyerRoleTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  BuyerRoleType GetSingle(string code)
        {
            return (from a in context.BuyerRoleTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<BuyerRoleType> GetAll()
        {
            return from a in context.BuyerRoleTypes  
                   select a;
        }
				 
        public BuyerRoleType GetSingle(EntityKeyFields entityKeys)
        {
            BuyerRoleTypeKeys keys = entityKeys as BuyerRoleTypeKeys;
            return (from a in context.BuyerRoleTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(BuyerRoleType entity)
        {
            onAdd();
            context.BuyerRoleTypes.Add(entity);
        }

        public void Remove(BuyerRoleType entity)
        {
            context.BuyerRoleTypes.Attach(entity);
            context.BuyerRoleTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(BuyerRoleType entity)
        {
            onUpdate();
            context.BuyerRoleTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<BuyerRoleType> All()
        {
            return context.BuyerRoleTypes.ToList();
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
	 