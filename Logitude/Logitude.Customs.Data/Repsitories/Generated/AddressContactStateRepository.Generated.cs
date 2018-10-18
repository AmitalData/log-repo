 
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
   public partial class AddressContactStateRepository:IRepository<AddressContactState>
   {
   
        private ICustomContext currentContext;
        public AddressContactStateRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public AddressContactStateRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  AddressContactState GetSingle(string code)
        {
            return (from a in context.AddressContactStates
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<AddressContactState> GetAll()
        {
            return from a in context.AddressContactStates  
                   select a;
        }
				 
        public AddressContactState GetSingle(EntityKeyFields entityKeys)
        {
            AddressContactStateKeys keys = entityKeys as AddressContactStateKeys;
            return (from a in context.AddressContactStates
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(AddressContactState entity)
        {
            onAdd();
            context.AddressContactStates.Add(entity);
        }

        public void Remove(AddressContactState entity)
        {
            context.AddressContactStates.Attach(entity);
            context.AddressContactStates.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(AddressContactState entity)
        {
            onUpdate();
            context.AddressContactStates.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AddressContactState> All()
        {
            return context.AddressContactStates.ToList();
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
	 