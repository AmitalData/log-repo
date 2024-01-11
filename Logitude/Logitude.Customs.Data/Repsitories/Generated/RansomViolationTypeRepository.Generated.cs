 
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
   public partial class RansomViolationTypeRepository:IRepository<RansomViolationType>
   {
   
        private ICustomContext currentContext;
        public RansomViolationTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public RansomViolationTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  RansomViolationType GetSingle(string code)
        {
            return (from a in context.RansomViolationTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<RansomViolationType> GetAll()
        {
            return from a in context.RansomViolationTypes  
                   select a;
        }
				 
        public RansomViolationType GetSingle(EntityKeyFields entityKeys)
        {
            RansomViolationTypeKeys keys = entityKeys as RansomViolationTypeKeys;
            return (from a in context.RansomViolationTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(RansomViolationType entity)
        {
            onAdd();
            context.RansomViolationTypes.Add(entity);
        }

        public void Remove(RansomViolationType entity)
        {
            context.RansomViolationTypes.Attach(entity);
            context.RansomViolationTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(RansomViolationType entity)
        {
            onUpdate();
            context.RansomViolationTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<RansomViolationType> All()
        {
            return context.RansomViolationTypes.ToList();
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
	 