 
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
   public partial class AcceptanceStatusRepository:IRepository<AcceptanceStatus>
   {
   
        private ICustomContext currentContext;
        public AcceptanceStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public AcceptanceStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  AcceptanceStatus GetSingle(string code)
        {
            return (from a in context.AcceptanceStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<AcceptanceStatus> GetAll()
        {
            return from a in context.AcceptanceStatuses  
                   select a;
        }
				 
        public AcceptanceStatus GetSingle(EntityKeyFields entityKeys)
        {
            AcceptanceStatusKeys keys = entityKeys as AcceptanceStatusKeys;
            return (from a in context.AcceptanceStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(AcceptanceStatus entity)
        {
            onAdd();
            context.AcceptanceStatuses.Add(entity);
        }

        public void Remove(AcceptanceStatus entity)
        {
            context.AcceptanceStatuses.Attach(entity);
            context.AcceptanceStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(AcceptanceStatus entity)
        {
            onUpdate();
            context.AcceptanceStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AcceptanceStatus> All()
        {
            return context.AcceptanceStatuses.ToList();
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
	 