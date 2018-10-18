 
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
   public partial class PropertiesDetailsHistoryRepository:IRepository<PropertiesDetailsHistory>
   {
   
        private ICustomContext currentContext;
        public PropertiesDetailsHistoryRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public PropertiesDetailsHistoryRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  PropertiesDetailsHistory GetSingle(string id)
        {
            return (from a in context.PropertiesDetailsHistorys
                    where a.ID == id 
                    select a).FirstOrDefault();
        }

        public IQueryable<PropertiesDetailsHistory> GetAll()
        {
            return from a in context.PropertiesDetailsHistorys  
                   select a;
        }
				 
        public PropertiesDetailsHistory GetSingle(EntityKeyFields entityKeys)
        {
            PropertiesDetailsHistoryKeys keys = entityKeys as PropertiesDetailsHistoryKeys;
            return (from a in context.PropertiesDetailsHistorys
                    where a.ID == keys.ID
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PropertiesDetailsHistory entity)
        {
            onAdd();
            context.PropertiesDetailsHistorys.Add(entity);
        }

        public void Remove(PropertiesDetailsHistory entity)
        {
            context.PropertiesDetailsHistorys.Attach(entity);
            context.PropertiesDetailsHistorys.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PropertiesDetailsHistory entity)
        {
            onUpdate();
            context.PropertiesDetailsHistorys.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PropertiesDetailsHistory> All()
        {
            return context.PropertiesDetailsHistorys.ToList();
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
	 