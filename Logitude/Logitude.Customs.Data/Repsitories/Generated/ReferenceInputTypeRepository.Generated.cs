 
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
   public partial class ReferenceInputTypeRepository:IRepository<ReferenceInputType>
   {
   
        private ICustomContext currentContext;
        public ReferenceInputTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ReferenceInputTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ReferenceInputType GetSingle(string code)
        {
            return (from a in context.ReferenceInputTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ReferenceInputType> GetAll()
        {
            return from a in context.ReferenceInputTypes  
                   select a;
        }
				 
        public ReferenceInputType GetSingle(EntityKeyFields entityKeys)
        {
            ReferenceInputTypeKeys keys = entityKeys as ReferenceInputTypeKeys;
            return (from a in context.ReferenceInputTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ReferenceInputType entity)
        {
            onAdd();
            context.ReferenceInputTypes.Add(entity);
        }

        public void Remove(ReferenceInputType entity)
        {
            context.ReferenceInputTypes.Attach(entity);
            context.ReferenceInputTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ReferenceInputType entity)
        {
            onUpdate();
            context.ReferenceInputTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ReferenceInputType> All()
        {
            return context.ReferenceInputTypes.ToList();
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
	 