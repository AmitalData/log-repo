 
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
   public partial class AvailableStatusFieldRepository:IRepository<AvailableStatusField>
   {
   
        private ICustomContext currentContext;
        public AvailableStatusFieldRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public AvailableStatusFieldRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  AvailableStatusField GetSingle(string fieldcode, int tenant)
        {
            return (from a in context.AvailableStatusFields
                    where a.FieldCode == fieldcode && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<AvailableStatusField> GetAll(int tenant)
        {
            return from a in context.AvailableStatusFields  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public AvailableStatusField GetSingle(EntityKeyFields entityKeys)
        {
            AvailableStatusFieldKeys keys = entityKeys as AvailableStatusFieldKeys;
            return (from a in context.AvailableStatusFields
                    where a.FieldCode == keys.FieldCode
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(AvailableStatusField entity)
        {
            onAdd();
            context.AvailableStatusFields.Add(entity);
        }

        public void Remove(AvailableStatusField entity)
        {
            context.AvailableStatusFields.Attach(entity);
            context.AvailableStatusFields.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(AvailableStatusField entity)
        {
            onUpdate();
            context.AvailableStatusFields.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AvailableStatusField> All()
        {
            return context.AvailableStatusFields.ToList();
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
	 