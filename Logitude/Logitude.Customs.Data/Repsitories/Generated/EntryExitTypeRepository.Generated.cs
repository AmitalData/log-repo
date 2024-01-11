 
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
   public partial class EntryExitTypeRepository:IRepository<EntryExitType>
   {
   
        private ICustomContext currentContext;
        public EntryExitTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public EntryExitTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  EntryExitType GetSingle(string code)
        {
            return (from a in context.EntryExitTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<EntryExitType> GetAll()
        {
            return from a in context.EntryExitTypes  
                   select a;
        }
				 
        public EntryExitType GetSingle(EntityKeyFields entityKeys)
        {
            EntryExitTypeKeys keys = entityKeys as EntryExitTypeKeys;
            return (from a in context.EntryExitTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(EntryExitType entity)
        {
            onAdd();
            context.EntryExitTypes.Add(entity);
        }

        public void Remove(EntryExitType entity)
        {
            context.EntryExitTypes.Attach(entity);
            context.EntryExitTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(EntryExitType entity)
        {
            onUpdate();
            context.EntryExitTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<EntryExitType> All()
        {
            return context.EntryExitTypes.ToList();
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
	 