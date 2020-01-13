 
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
   public partial class ContinuousRequestTypeRepository:IRepository<ContinuousRequestType>
   {
   
        private ICustomContext currentContext;
        public ContinuousRequestTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ContinuousRequestTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ContinuousRequestType GetSingle(string code)
        {
            return (from a in context.ContinuousRequestTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ContinuousRequestType> GetAll()
        {
            return from a in context.ContinuousRequestTypes  
                   select a;
        }
				 
        public ContinuousRequestType GetSingle(EntityKeyFields entityKeys)
        {
            ContinuousRequestTypeKeys keys = entityKeys as ContinuousRequestTypeKeys;
            return (from a in context.ContinuousRequestTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ContinuousRequestType entity)
        {
            onAdd();
            context.ContinuousRequestTypes.Add(entity);
        }

        public void Remove(ContinuousRequestType entity)
        {
            context.ContinuousRequestTypes.Attach(entity);
            context.ContinuousRequestTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ContinuousRequestType entity)
        {
            onUpdate();
            context.ContinuousRequestTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ContinuousRequestType> All()
        {
            return context.ContinuousRequestTypes.ToList();
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
	 