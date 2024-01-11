 
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
   public partial class TradeLevyStatusRepository:IRepository<TradeLevyStatus>
   {
   
        private ICustomContext currentContext;
        public TradeLevyStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public TradeLevyStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  TradeLevyStatus GetSingle(string code)
        {
            return (from a in context.TradeLevystatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<TradeLevyStatus> GetAll()
        {
            return from a in context.TradeLevystatuses  
                   select a;
        }
				 
        public TradeLevyStatus GetSingle(EntityKeyFields entityKeys)
        {
            TradeLevyStatusKeys keys = entityKeys as TradeLevyStatusKeys;
            return (from a in context.TradeLevystatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TradeLevyStatus entity)
        {
            onAdd();
            context.TradeLevystatuses.Add(entity);
        }

        public void Remove(TradeLevyStatus entity)
        {
            context.TradeLevystatuses.Attach(entity);
            context.TradeLevystatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TradeLevyStatus entity)
        {
            onUpdate();
            context.TradeLevystatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TradeLevyStatus> All()
        {
            return context.TradeLevystatuses.ToList();
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
	 