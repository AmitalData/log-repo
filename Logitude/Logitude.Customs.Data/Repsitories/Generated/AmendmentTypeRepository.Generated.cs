 
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
   public partial class AmendmentTypeRepository:IRepository<AmendmentType>
   {
   
        private ICustomContext currentContext;
        public AmendmentTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public AmendmentTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  AmendmentType GetSingle(string code)
        {
            return (from a in context.AmendmentTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<AmendmentType> GetAll()
        {
            return from a in context.AmendmentTypes  
                   select a;
        }
				 
        public AmendmentType GetSingle(EntityKeyFields entityKeys)
        {
            AmendmentTypeKeys keys = entityKeys as AmendmentTypeKeys;
            return (from a in context.AmendmentTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(AmendmentType entity)
        {
            onAdd();
            context.AmendmentTypes.Add(entity);
        }

        public void Remove(AmendmentType entity)
        {
            context.AmendmentTypes.Attach(entity);
            context.AmendmentTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(AmendmentType entity)
        {
            onUpdate();
            context.AmendmentTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AmendmentType> All()
        {
            return context.AmendmentTypes.ToList();
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
	 