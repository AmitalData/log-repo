 
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
   public partial class CollateralTypeRepository:IRepository<CollateralType>
   {
   
        private ICustomContext currentContext;
        public CollateralTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CollateralTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CollateralType GetSingle(string code)
        {
            return (from a in context.CollateralTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CollateralType> GetAll()
        {
            return from a in context.CollateralTypes  
                   select a;
        }
				 
        public CollateralType GetSingle(EntityKeyFields entityKeys)
        {
            CollateralTypeKeys keys = entityKeys as CollateralTypeKeys;
            return (from a in context.CollateralTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CollateralType entity)
        {
            onAdd();
            context.CollateralTypes.Add(entity);
        }

        public void Remove(CollateralType entity)
        {
            context.CollateralTypes.Attach(entity);
            context.CollateralTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CollateralType entity)
        {
            onUpdate();
            context.CollateralTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CollateralType> All()
        {
            return context.CollateralTypes.ToList();
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
	 