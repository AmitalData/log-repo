 
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
   public partial class ContainerTypeRepository:IRepository<ContainerType>
   {
   
        private ICustomContext currentContext;
        public ContainerTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ContainerTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ContainerType GetSingle(string code)
        {
            return (from a in context.ContainerTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ContainerType> GetAll()
        {
            return from a in context.ContainerTypes  
                   select a;
        }
				 
        public ContainerType GetSingle(EntityKeyFields entityKeys)
        {
            ContainerTypeKeys keys = entityKeys as ContainerTypeKeys;
            return (from a in context.ContainerTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ContainerType entity)
        {
            onAdd();
            context.ContainerTypes.Add(entity);
        }

        public void Remove(ContainerType entity)
        {
            context.ContainerTypes.Attach(entity);
            context.ContainerTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ContainerType entity)
        {
            onUpdate();
            context.ContainerTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ContainerType> All()
        {
            return context.ContainerTypes.ToList();
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
	 