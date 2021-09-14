 
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
   public partial class ContainerizationHatataStatusRepository:IRepository<ContainerizationHatataStatus>
   {
   
        private ICustomContext currentContext;
        public ContainerizationHatataStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ContainerizationHatataStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ContainerizationHatataStatus GetSingle(string code)
        {
            return (from a in context.ContainerizationHatataStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ContainerizationHatataStatus> GetAll()
        {
            return from a in context.ContainerizationHatataStatuses  
                   select a;
        }
				 
        public ContainerizationHatataStatus GetSingle(EntityKeyFields entityKeys)
        {
            ContainerizationHatataStatusKeys keys = entityKeys as ContainerizationHatataStatusKeys;
            return (from a in context.ContainerizationHatataStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ContainerizationHatataStatus entity)
        {
            onAdd();
            context.ContainerizationHatataStatuses.Add(entity);
        }

        public void Remove(ContainerizationHatataStatus entity)
        {
            context.ContainerizationHatataStatuses.Attach(entity);
            context.ContainerizationHatataStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ContainerizationHatataStatus entity)
        {
            onUpdate();
            context.ContainerizationHatataStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ContainerizationHatataStatus> All()
        {
            return context.ContainerizationHatataStatuses.ToList();
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
	 