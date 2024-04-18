 
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
   public partial class ChangeTypeRepository:IRepository<ChangeType>
   {
   
        private ICustomContext currentContext;
        public ChangeTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ChangeTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ChangeType GetSingle(string code)
        {
            return (from a in context.ChangeTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ChangeType> GetAll()
        {
            return from a in context.ChangeTypes  
                   select a;
        }
				 
        public ChangeType GetSingle(EntityKeyFields entityKeys)
        {
            ChangeTypeKeys keys = entityKeys as ChangeTypeKeys;
            return (from a in context.ChangeTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ChangeType entity)
        {
            onAdd();
            context.ChangeTypes.Add(entity);
        }

        public void Remove(ChangeType entity)
        {
            context.ChangeTypes.Attach(entity);
            context.ChangeTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ChangeType entity)
        {
            onUpdate();
            context.ChangeTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ChangeType> All()
        {
            return context.ChangeTypes.ToList();
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
	 