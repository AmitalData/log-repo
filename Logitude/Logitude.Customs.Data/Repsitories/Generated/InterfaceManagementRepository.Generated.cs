 
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
   public partial class InterfaceManagementRepository:IRepository<InterfaceManagement>
   {
   
        private ICustomContext currentContext;
        public InterfaceManagementRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public InterfaceManagementRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  InterfaceManagement GetSingle(string code)
        {
            return (from a in context.InterfaceManagements
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<InterfaceManagement> GetAll()
        {
            return from a in context.InterfaceManagements  
                   select a;
        }
				 
        public InterfaceManagement GetSingle(EntityKeyFields entityKeys)
        {
            InterfaceManagementKeys keys = entityKeys as InterfaceManagementKeys;
            return (from a in context.InterfaceManagements
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(InterfaceManagement entity)
        {
            onAdd();
            context.InterfaceManagements.Add(entity);
        }

        public void Remove(InterfaceManagement entity)
        {
            context.InterfaceManagements.Attach(entity);
            context.InterfaceManagements.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(InterfaceManagement entity)
        {
            onUpdate();
            context.InterfaceManagements.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<InterfaceManagement> All()
        {
            return context.InterfaceManagements.ToList();
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
	 