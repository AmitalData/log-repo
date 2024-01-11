 
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
   public partial class CustomsEntityStatusRepository:IRepository<CustomsEntityStatus>
   {
   
        private ICustomContext currentContext;
        public CustomsEntityStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsEntityStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsEntityStatus GetSingle(string code)
        {
            return (from a in context.CustomsEntityStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsEntityStatus> GetAll()
        {
            return from a in context.CustomsEntityStatuses  
                   select a;
        }
				 
        public CustomsEntityStatus GetSingle(EntityKeyFields entityKeys)
        {
            CustomsEntityStatusKeys keys = entityKeys as CustomsEntityStatusKeys;
            return (from a in context.CustomsEntityStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsEntityStatus entity)
        {
            onAdd();
            context.CustomsEntityStatuses.Add(entity);
        }

        public void Remove(CustomsEntityStatus entity)
        {
            context.CustomsEntityStatuses.Attach(entity);
            context.CustomsEntityStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsEntityStatus entity)
        {
            onUpdate();
            context.CustomsEntityStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsEntityStatus> All()
        {
            return context.CustomsEntityStatuses.ToList();
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
	 