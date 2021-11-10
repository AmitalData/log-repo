 
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
   public partial class PhysicalCheckSearchResultTypeRepository:IRepository<PhysicalCheckSearchResultType>
   {
   
        private ICustomContext currentContext;
        public PhysicalCheckSearchResultTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public PhysicalCheckSearchResultTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  PhysicalCheckSearchResultType GetSingle(string code)
        {
            return (from a in context.PhysicalCheckSearchResultTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<PhysicalCheckSearchResultType> GetAll()
        {
            return from a in context.PhysicalCheckSearchResultTypes  
                   select a;
        }
				 
        public PhysicalCheckSearchResultType GetSingle(EntityKeyFields entityKeys)
        {
            PhysicalCheckSearchResultTypeKeys keys = entityKeys as PhysicalCheckSearchResultTypeKeys;
            return (from a in context.PhysicalCheckSearchResultTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PhysicalCheckSearchResultType entity)
        {
            onAdd();
            context.PhysicalCheckSearchResultTypes.Add(entity);
        }

        public void Remove(PhysicalCheckSearchResultType entity)
        {
            context.PhysicalCheckSearchResultTypes.Attach(entity);
            context.PhysicalCheckSearchResultTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PhysicalCheckSearchResultType entity)
        {
            onUpdate();
            context.PhysicalCheckSearchResultTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PhysicalCheckSearchResultType> All()
        {
            return context.PhysicalCheckSearchResultTypes.ToList();
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
	 