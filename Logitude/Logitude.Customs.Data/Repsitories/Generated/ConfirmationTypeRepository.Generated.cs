 
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
   public partial class ConfirmationTypeRepository:IRepository<ConfirmationType>
   {
   
        private ICustomContext currentContext;
        public ConfirmationTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ConfirmationTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ConfirmationType GetSingle(string code)
        {
            return (from a in context.ConfirmationTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ConfirmationType> GetAll()
        {
            return from a in context.ConfirmationTypes  
                   select a;
        }
				 
        public ConfirmationType GetSingle(EntityKeyFields entityKeys)
        {
            ConfirmationTypeKeys keys = entityKeys as ConfirmationTypeKeys;
            return (from a in context.ConfirmationTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ConfirmationType entity)
        {
            onAdd();
            context.ConfirmationTypes.Add(entity);
        }

        public void Remove(ConfirmationType entity)
        {
            context.ConfirmationTypes.Attach(entity);
            context.ConfirmationTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ConfirmationType entity)
        {
            onUpdate();
            context.ConfirmationTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ConfirmationType> All()
        {
            return context.ConfirmationTypes.ToList();
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
	 