 
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
   public partial class SpecialActionDescriptionTypeRepository:IRepository<SpecialActionDescriptionType>
   {
   
        private ICustomContext currentContext;
        public SpecialActionDescriptionTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SpecialActionDescriptionTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SpecialActionDescriptionType GetSingle(string code)
        {
            return (from a in context.SpecialActionDescriptionTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<SpecialActionDescriptionType> GetAll()
        {
            return from a in context.SpecialActionDescriptionTypes  
                   select a;
        }
				 
        public SpecialActionDescriptionType GetSingle(EntityKeyFields entityKeys)
        {
            SpecialActionDescriptionTypeKeys keys = entityKeys as SpecialActionDescriptionTypeKeys;
            return (from a in context.SpecialActionDescriptionTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SpecialActionDescriptionType entity)
        {
            onAdd();
            context.SpecialActionDescriptionTypes.Add(entity);
        }

        public void Remove(SpecialActionDescriptionType entity)
        {
            context.SpecialActionDescriptionTypes.Attach(entity);
            context.SpecialActionDescriptionTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SpecialActionDescriptionType entity)
        {
            onUpdate();
            context.SpecialActionDescriptionTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SpecialActionDescriptionType> All()
        {
            return context.SpecialActionDescriptionTypes.ToList();
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
	 