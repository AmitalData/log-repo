 
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
   public partial class RequestReasonCodeEnumRepository:IRepository<RequestReasonCodeEnum>
   {
   
        private ICustomContext currentContext;
        public RequestReasonCodeEnumRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public RequestReasonCodeEnumRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  RequestReasonCodeEnum GetSingle(string code)
        {
            return (from a in context.RequestReasonCodeEnums
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<RequestReasonCodeEnum> GetAll()
        {
            return from a in context.RequestReasonCodeEnums  
                   select a;
        }
				 
        public RequestReasonCodeEnum GetSingle(EntityKeyFields entityKeys)
        {
            RequestReasonCodeEnumKeys keys = entityKeys as RequestReasonCodeEnumKeys;
            return (from a in context.RequestReasonCodeEnums
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(RequestReasonCodeEnum entity)
        {
            onAdd();
            context.RequestReasonCodeEnums.Add(entity);
        }

        public void Remove(RequestReasonCodeEnum entity)
        {
            context.RequestReasonCodeEnums.Attach(entity);
            context.RequestReasonCodeEnums.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(RequestReasonCodeEnum entity)
        {
            onUpdate();
            context.RequestReasonCodeEnums.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<RequestReasonCodeEnum> All()
        {
            return context.RequestReasonCodeEnums.ToList();
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
	 