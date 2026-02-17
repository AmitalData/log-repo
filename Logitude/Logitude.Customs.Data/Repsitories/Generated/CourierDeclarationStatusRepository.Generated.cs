 
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
   public partial class CourierDeclarationStatusRepository:IRepository<CourierDeclarationStatus>
   {
   
        private ICustomContext currentContext;
        public CourierDeclarationStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CourierDeclarationStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CourierDeclarationStatus GetSingle(string code)
        {
            return (from a in context.CourierDeclarationStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CourierDeclarationStatus> GetAll()
        {
            return from a in context.CourierDeclarationStatuses  
                   select a;
        }
				 
        public CourierDeclarationStatus GetSingle(EntityKeyFields entityKeys)
        {
            CourierDeclarationStatusKeys keys = entityKeys as CourierDeclarationStatusKeys;
            return (from a in context.CourierDeclarationStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CourierDeclarationStatus entity)
        {
            onAdd();
            context.CourierDeclarationStatuses.Add(entity);
        }

        public void Remove(CourierDeclarationStatus entity)
        {
            context.CourierDeclarationStatuses.Attach(entity);
            context.CourierDeclarationStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CourierDeclarationStatus entity)
        {
            onUpdate();
            context.CourierDeclarationStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CourierDeclarationStatus> All()
        {
            return context.CourierDeclarationStatuses.ToList();
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
	 