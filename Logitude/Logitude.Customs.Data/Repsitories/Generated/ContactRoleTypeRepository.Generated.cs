 
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
   public partial class ContactRoleTypeRepository:IRepository<ContactRoleType>
   {
   
        private ICustomContext currentContext;
        public ContactRoleTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ContactRoleTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ContactRoleType GetSingle(string code)
        {
            return (from a in context.ContactRoleTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ContactRoleType> GetAll()
        {
            return from a in context.ContactRoleTypes  
                   select a;
        }
				 
        public ContactRoleType GetSingle(EntityKeyFields entityKeys)
        {
            ContactRoleTypeKeys keys = entityKeys as ContactRoleTypeKeys;
            return (from a in context.ContactRoleTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ContactRoleType entity)
        {
            onAdd();
            context.ContactRoleTypes.Add(entity);
        }

        public void Remove(ContactRoleType entity)
        {
            context.ContactRoleTypes.Attach(entity);
            context.ContactRoleTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ContactRoleType entity)
        {
            onUpdate();
            context.ContactRoleTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ContactRoleType> All()
        {
            return context.ContactRoleTypes.ToList();
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
	 