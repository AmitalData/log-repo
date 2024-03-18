 
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
   public partial class CB_QuotaDetailsHistoryRepository:IRepository<CB_QuotaDetailsHistory>
   {
   
        private ICustomContext currentContext;
        public CB_QuotaDetailsHistoryRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CB_QuotaDetailsHistoryRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CB_QuotaDetailsHistory GetSingle(int id)
        {
            return (from a in context.CB_QuotaDetailsHistorys
                    where a.ID == id 
                    select a).FirstOrDefault();
        }

        public IQueryable<CB_QuotaDetailsHistory> GetAll()
        {
            return from a in context.CB_QuotaDetailsHistorys  
                   select a;
        }
				 
        public CB_QuotaDetailsHistory GetSingle(EntityKeyFields entityKeys)
        {
            CB_QuotaDetailsHistoryKeys keys = entityKeys as CB_QuotaDetailsHistoryKeys;
            return (from a in context.CB_QuotaDetailsHistorys
                    where a.ID == keys.ID
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CB_QuotaDetailsHistory entity)
        {
            onAdd();
            context.CB_QuotaDetailsHistorys.Add(entity);
        }

        public void Remove(CB_QuotaDetailsHistory entity)
        {
            context.CB_QuotaDetailsHistorys.Attach(entity);
            context.CB_QuotaDetailsHistorys.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CB_QuotaDetailsHistory entity)
        {
            onUpdate();
            context.CB_QuotaDetailsHistorys.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CB_QuotaDetailsHistory> All()
        {
            return context.CB_QuotaDetailsHistorys.ToList();
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
	 