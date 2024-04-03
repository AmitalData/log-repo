 
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
   public partial class CustomsAddressTypeRepository:IRepository<CustomsAddressType>
   {
   
        private ICustomContext currentContext;
        public CustomsAddressTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsAddressTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsAddressType GetSingle(string code)
        {
            return (from a in context.CustomsAddressTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsAddressType> GetAll()
        {
            return from a in context.CustomsAddressTypes  
                   select a;
        }
				 
        public CustomsAddressType GetSingle(EntityKeyFields entityKeys)
        {
            CustomsAddressTypeKeys keys = entityKeys as CustomsAddressTypeKeys;
            return (from a in context.CustomsAddressTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsAddressType entity)
        {
            onAdd();
            context.CustomsAddressTypes.Add(entity);
        }

        public void Remove(CustomsAddressType entity)
        {
            context.CustomsAddressTypes.Attach(entity);
            context.CustomsAddressTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsAddressType entity)
        {
            onUpdate();
            context.CustomsAddressTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsAddressType> All()
        {
            return context.CustomsAddressTypes.ToList();
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
	 