 
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
   public partial class ClosedTableStatusRepository:IRepository<ClosedTableStatus>
   {
   
        private ICustomContext currentContext;
        public ClosedTableStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ClosedTableStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ClosedTableStatus GetSingle(string code)
        {
            return (from a in context.ClosedTableStatus
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ClosedTableStatus> GetAll()
        {
            return from a in context.ClosedTableStatus  
                   select a;
        }
				 
        public ClosedTableStatus GetSingle(EntityKeyFields entityKeys)
        {
            ClosedTableStatusKeys keys = entityKeys as ClosedTableStatusKeys;
            return (from a in context.ClosedTableStatus
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ClosedTableStatus entity)
        {
            onAdd();
            context.ClosedTableStatus.Add(entity);
        }

        public void Remove(ClosedTableStatus entity)
        {
            context.ClosedTableStatus.Attach(entity);
            context.ClosedTableStatus.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ClosedTableStatus entity)
        {
            onUpdate();
            context.ClosedTableStatus.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ClosedTableStatus> All()
        {
            return context.ClosedTableStatus.ToList();
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
	 