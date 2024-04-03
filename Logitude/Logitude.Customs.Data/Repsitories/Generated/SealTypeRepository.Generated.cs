 
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
   public partial class SealTypeRepository:IRepository<SealType>
   {
   
        private ICustomContext currentContext;
        public SealTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SealTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SealType GetSingle(string code)
        {
            return (from a in context.SealTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<SealType> GetAll()
        {
            return from a in context.SealTypes  
                   select a;
        }
				 
        public SealType GetSingle(EntityKeyFields entityKeys)
        {
            SealTypeKeys keys = entityKeys as SealTypeKeys;
            return (from a in context.SealTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SealType entity)
        {
            onAdd();
            context.SealTypes.Add(entity);
        }

        public void Remove(SealType entity)
        {
            context.SealTypes.Attach(entity);
            context.SealTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SealType entity)
        {
            onUpdate();
            context.SealTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SealType> All()
        {
            return context.SealTypes.ToList();
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
	 