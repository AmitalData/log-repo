 
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
   public partial class CB_PropertiesDetailsHistoryRepository:IRepository<CB_PropertiesDetailsHistory>
   {
   
        private ICustomContext currentContext;
        public CB_PropertiesDetailsHistoryRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CB_PropertiesDetailsHistoryRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CB_PropertiesDetailsHistory GetSingle(int id)
        {
            return (from a in context.CB_PropertiesDetailsHistorys
                    where a.ID == id 
                    select a).FirstOrDefault();
        }

        public IQueryable<CB_PropertiesDetailsHistory> GetAll()
        {
            return from a in context.CB_PropertiesDetailsHistorys  
                   select a;
        }
				 
        public CB_PropertiesDetailsHistory GetSingle(EntityKeyFields entityKeys)
        {
            CB_PropertiesDetailsHistoryKeys keys = entityKeys as CB_PropertiesDetailsHistoryKeys;
            return (from a in context.CB_PropertiesDetailsHistorys
                    where a.ID == keys.ID
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CB_PropertiesDetailsHistory entity)
        {
            onAdd();
            context.CB_PropertiesDetailsHistorys.Add(entity);
        }

        public void Remove(CB_PropertiesDetailsHistory entity)
        {
            context.CB_PropertiesDetailsHistorys.Attach(entity);
            context.CB_PropertiesDetailsHistorys.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CB_PropertiesDetailsHistory entity)
        {
            onUpdate();
            context.CB_PropertiesDetailsHistorys.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CB_PropertiesDetailsHistory> All()
        {
            return context.CB_PropertiesDetailsHistorys.ToList();
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
	 