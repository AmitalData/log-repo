 
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
   public partial class CB_CustomsBookAdditionsDetailsHistoryRepository:IRepository<CB_CustomsBookAdditionsDetailsHistory>
   {
   
        private ICustomContext currentContext;
        public CB_CustomsBookAdditionsDetailsHistoryRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CB_CustomsBookAdditionsDetailsHistoryRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CB_CustomsBookAdditionsDetailsHistory GetSingle(string cb_id)
        {
            return (from a in context.CB_CustomsBookAdditionsDetailsHistorys
                    where a.CB_ID == cb_id 
                    select a).FirstOrDefault();
        }

        public IQueryable<CB_CustomsBookAdditionsDetailsHistory> GetAll()
        {
            return from a in context.CB_CustomsBookAdditionsDetailsHistorys  
                   select a;
        }
				 
        public CB_CustomsBookAdditionsDetailsHistory GetSingle(EntityKeyFields entityKeys)
        {
            CB_CustomsBookAdditionsDetailsHistoryKeys keys = entityKeys as CB_CustomsBookAdditionsDetailsHistoryKeys;
            return (from a in context.CB_CustomsBookAdditionsDetailsHistorys
                    where a.CB_ID == keys.CB_ID
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CB_CustomsBookAdditionsDetailsHistory entity)
        {
            onAdd();
            context.CB_CustomsBookAdditionsDetailsHistorys.Add(entity);
        }

        public void Remove(CB_CustomsBookAdditionsDetailsHistory entity)
        {
            context.CB_CustomsBookAdditionsDetailsHistorys.Attach(entity);
            context.CB_CustomsBookAdditionsDetailsHistorys.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CB_CustomsBookAdditionsDetailsHistory entity)
        {
            onUpdate();
            context.CB_CustomsBookAdditionsDetailsHistorys.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CB_CustomsBookAdditionsDetailsHistory> All()
        {
            return context.CB_CustomsBookAdditionsDetailsHistorys.ToList();
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
	 