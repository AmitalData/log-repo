 
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
   public partial class AddressPurposeRepository:IRepository<AddressPurpose>
   {
   
        private ICustomContext currentContext;
        public AddressPurposeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public AddressPurposeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  AddressPurpose GetSingle(string code)
        {
            return (from a in context.AddressPurposes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<AddressPurpose> GetAll()
        {
            return from a in context.AddressPurposes  
                   select a;
        }
				 
        public AddressPurpose GetSingle(EntityKeyFields entityKeys)
        {
            AddressPurposeKeys keys = entityKeys as AddressPurposeKeys;
            return (from a in context.AddressPurposes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(AddressPurpose entity)
        {
            onAdd();
            context.AddressPurposes.Add(entity);
        }

        public void Remove(AddressPurpose entity)
        {
            context.AddressPurposes.Attach(entity);
            context.AddressPurposes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(AddressPurpose entity)
        {
            onUpdate();
            context.AddressPurposes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AddressPurpose> All()
        {
            return context.AddressPurposes.ToList();
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
	 