 
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
   public partial class ContainerizationStatusCodeRepository:IRepository<ContainerizationStatusCode>
   {
   
        private ICustomContext currentContext;
        public ContainerizationStatusCodeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ContainerizationStatusCodeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ContainerizationStatusCode GetSingle(string code)
        {
            return (from a in context.ContainerizationStatusCodes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ContainerizationStatusCode> GetAll()
        {
            return from a in context.ContainerizationStatusCodes  
                   select a;
        }
				 
        public ContainerizationStatusCode GetSingle(EntityKeyFields entityKeys)
        {
            ContainerizationStatusCodeKeys keys = entityKeys as ContainerizationStatusCodeKeys;
            return (from a in context.ContainerizationStatusCodes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ContainerizationStatusCode entity)
        {
            onAdd();
            context.ContainerizationStatusCodes.Add(entity);
        }

        public void Remove(ContainerizationStatusCode entity)
        {
            context.ContainerizationStatusCodes.Attach(entity);
            context.ContainerizationStatusCodes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ContainerizationStatusCode entity)
        {
            onUpdate();
            context.ContainerizationStatusCodes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ContainerizationStatusCode> All()
        {
            return context.ContainerizationStatusCodes.ToList();
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
	 