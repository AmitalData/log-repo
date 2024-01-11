 
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
   public partial class PassportTypeRepository:IRepository<PassportType>
   {
   
        private ICustomContext currentContext;
        public PassportTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public PassportTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  PassportType GetSingle(string code)
        {
            return (from a in context.PassportTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<PassportType> GetAll()
        {
            return from a in context.PassportTypes  
                   select a;
        }
				 
        public PassportType GetSingle(EntityKeyFields entityKeys)
        {
            PassportTypeKeys keys = entityKeys as PassportTypeKeys;
            return (from a in context.PassportTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PassportType entity)
        {
            onAdd();
            context.PassportTypes.Add(entity);
        }

        public void Remove(PassportType entity)
        {
            context.PassportTypes.Attach(entity);
            context.PassportTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PassportType entity)
        {
            onUpdate();
            context.PassportTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PassportType> All()
        {
            return context.PassportTypes.ToList();
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
	 