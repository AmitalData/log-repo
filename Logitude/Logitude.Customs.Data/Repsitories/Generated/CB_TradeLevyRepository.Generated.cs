 
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
   public partial class CB_TradeLevyRepository:IRepository<CB_TradeLevy>
   {
   
        private ICustomContext currentContext;
        public CB_TradeLevyRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CB_TradeLevyRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CB_TradeLevy GetSingle(int id)
        {
            return (from a in context.CB_TradeLevys
                    where a.ID == id 
                    select a).FirstOrDefault();
        }

        public IQueryable<CB_TradeLevy> GetAll()
        {
            return from a in context.CB_TradeLevys  
                   select a;
        }
				 
        public CB_TradeLevy GetSingle(EntityKeyFields entityKeys)
        {
            CB_TradeLevyKeys keys = entityKeys as CB_TradeLevyKeys;
            return (from a in context.CB_TradeLevys
                    where a.ID == keys.ID
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CB_TradeLevy entity)
        {
            onAdd();
            context.CB_TradeLevys.Add(entity);
        }

        public void Remove(CB_TradeLevy entity)
        {
            context.CB_TradeLevys.Attach(entity);
            context.CB_TradeLevys.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CB_TradeLevy entity)
        {
            onUpdate();
            context.CB_TradeLevys.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CB_TradeLevy> All()
        {
            return context.CB_TradeLevys.ToList();
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
	 