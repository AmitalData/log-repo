 
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
   public partial class ContainerizationHataraStatusRepository:IRepository<ContainerizationHataraStatus>
   {
   
        private ICustomContext currentContext;
        public ContainerizationHataraStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ContainerizationHataraStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ContainerizationHataraStatus GetSingle(string code)
        {
            return (from a in context.ContainerizationHataraStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ContainerizationHataraStatus> GetAll()
        {
            return from a in context.ContainerizationHataraStatuses  
                   select a;
        }
				 
        public ContainerizationHataraStatus GetSingle(EntityKeyFields entityKeys)
        {
            ContainerizationHataraStatusKeys keys = entityKeys as ContainerizationHataraStatusKeys;
            return (from a in context.ContainerizationHataraStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ContainerizationHataraStatus entity)
        {
            onAdd();
            context.ContainerizationHataraStatuses.Add(entity);
        }

        public void Remove(ContainerizationHataraStatus entity)
        {
            context.ContainerizationHataraStatuses.Attach(entity);
            context.ContainerizationHataraStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ContainerizationHataraStatus entity)
        {
            onUpdate();
            context.ContainerizationHataraStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ContainerizationHataraStatus> All()
        {
            return context.ContainerizationHataraStatuses.ToList();
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
	 