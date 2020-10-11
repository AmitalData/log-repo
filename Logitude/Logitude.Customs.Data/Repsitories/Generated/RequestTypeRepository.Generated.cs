 
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
   public partial class RequestTypeRepository:IRepository<RequestType>
   {
   
        private ICustomContext currentContext;
        public RequestTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public RequestTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  RequestType GetSingle(string code)
        {
            return (from a in context.RequestTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<RequestType> GetAll()
        {
            return from a in context.RequestTypes  
                   select a;
        }
				 
        public RequestType GetSingle(EntityKeyFields entityKeys)
        {
            RequestTypeKeys keys = entityKeys as RequestTypeKeys;
            return (from a in context.RequestTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(RequestType entity)
        {
            onAdd();
            context.RequestTypes.Add(entity);
        }

        public void Remove(RequestType entity)
        {
            context.RequestTypes.Attach(entity);
            context.RequestTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(RequestType entity)
        {
            onUpdate();
            context.RequestTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<RequestType> All()
        {
            return context.RequestTypes.ToList();
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
	 