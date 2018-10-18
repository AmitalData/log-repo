 
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
   public partial class RegisteredWarehouseSiteTypeRepository:IRepository<RegisteredWarehouseSiteType>
   {
   
        private ICustomContext currentContext;
        public RegisteredWarehouseSiteTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public RegisteredWarehouseSiteTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  RegisteredWarehouseSiteType GetSingle(string code)
        {
            return (from a in context.RegisteredWarehouseSiteTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<RegisteredWarehouseSiteType> GetAll()
        {
            return from a in context.RegisteredWarehouseSiteTypes  
                   select a;
        }
				 
        public RegisteredWarehouseSiteType GetSingle(EntityKeyFields entityKeys)
        {
            RegisteredWarehouseSiteTypeKeys keys = entityKeys as RegisteredWarehouseSiteTypeKeys;
            return (from a in context.RegisteredWarehouseSiteTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(RegisteredWarehouseSiteType entity)
        {
            onAdd();
            context.RegisteredWarehouseSiteTypes.Add(entity);
        }

        public void Remove(RegisteredWarehouseSiteType entity)
        {
            context.RegisteredWarehouseSiteTypes.Attach(entity);
            context.RegisteredWarehouseSiteTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(RegisteredWarehouseSiteType entity)
        {
            onUpdate();
            context.RegisteredWarehouseSiteTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<RegisteredWarehouseSiteType> All()
        {
            return context.RegisteredWarehouseSiteTypes.ToList();
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
	 