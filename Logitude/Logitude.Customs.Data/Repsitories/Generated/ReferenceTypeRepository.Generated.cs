 
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
   public partial class ReferenceTypeRepository:IRepository<ReferenceType>
   {
   
        private ICustomContext currentContext;
        public ReferenceTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ReferenceTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ReferenceType GetSingle(string code)
        {
            return (from a in context.ReferenceTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ReferenceType> GetAll()
        {
            return from a in context.ReferenceTypes  
                   select a;
        }
				 
        public ReferenceType GetSingle(EntityKeyFields entityKeys)
        {
            ReferenceTypeKeys keys = entityKeys as ReferenceTypeKeys;
            return (from a in context.ReferenceTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ReferenceType entity)
        {
            onAdd();
            context.ReferenceTypes.Add(entity);
        }

        public void Remove(ReferenceType entity)
        {
            context.ReferenceTypes.Attach(entity);
            context.ReferenceTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ReferenceType entity)
        {
            onUpdate();
            context.ReferenceTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ReferenceType> All()
        {
            return context.ReferenceTypes.ToList();
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
	 