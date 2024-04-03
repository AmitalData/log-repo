 
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
   public partial class CustomsVerificationStatusTypeRepository:IRepository<CustomsVerificationStatusType>
   {
   
        private ICustomContext currentContext;
        public CustomsVerificationStatusTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsVerificationStatusTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsVerificationStatusType GetSingle(string code)
        {
            return (from a in context.CustomsVerificationStatusTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsVerificationStatusType> GetAll()
        {
            return from a in context.CustomsVerificationStatusTypes  
                   select a;
        }
				 
        public CustomsVerificationStatusType GetSingle(EntityKeyFields entityKeys)
        {
            CustomsVerificationStatusTypeKeys keys = entityKeys as CustomsVerificationStatusTypeKeys;
            return (from a in context.CustomsVerificationStatusTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsVerificationStatusType entity)
        {
            onAdd();
            context.CustomsVerificationStatusTypes.Add(entity);
        }

        public void Remove(CustomsVerificationStatusType entity)
        {
            context.CustomsVerificationStatusTypes.Attach(entity);
            context.CustomsVerificationStatusTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsVerificationStatusType entity)
        {
            onUpdate();
            context.CustomsVerificationStatusTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsVerificationStatusType> All()
        {
            return context.CustomsVerificationStatusTypes.ToList();
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
	 