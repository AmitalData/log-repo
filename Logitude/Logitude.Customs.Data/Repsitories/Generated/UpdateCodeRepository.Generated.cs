 
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
   public partial class UpdateCodeRepository:IRepository<UpdateCode>
   {
   
        private ICustomContext currentContext;
        public UpdateCodeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public UpdateCodeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  UpdateCode GetSingle(string code)
        {
            return (from a in context.UpdateCodes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<UpdateCode> GetAll()
        {
            return from a in context.UpdateCodes  
                   select a;
        }
				 
        public UpdateCode GetSingle(EntityKeyFields entityKeys)
        {
            UpdateCodeKeys keys = entityKeys as UpdateCodeKeys;
            return (from a in context.UpdateCodes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(UpdateCode entity)
        {
            onAdd();
            context.UpdateCodes.Add(entity);
        }

        public void Remove(UpdateCode entity)
        {
            context.UpdateCodes.Attach(entity);
            context.UpdateCodes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(UpdateCode entity)
        {
            onUpdate();
            context.UpdateCodes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<UpdateCode> All()
        {
            return context.UpdateCodes.ToList();
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
	 