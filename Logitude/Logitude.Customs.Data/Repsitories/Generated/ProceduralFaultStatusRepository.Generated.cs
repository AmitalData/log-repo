 
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
   public partial class ProceduralFaultStatusRepository:IRepository<ProceduralFaultStatus>
   {
   
        private ICustomContext currentContext;
        public ProceduralFaultStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ProceduralFaultStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ProceduralFaultStatus GetSingle(string code)
        {
            return (from a in context.ProceduralFaultStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ProceduralFaultStatus> GetAll()
        {
            return from a in context.ProceduralFaultStatuses  
                   select a;
        }
				 
        public ProceduralFaultStatus GetSingle(EntityKeyFields entityKeys)
        {
            ProceduralFaultStatusKeys keys = entityKeys as ProceduralFaultStatusKeys;
            return (from a in context.ProceduralFaultStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ProceduralFaultStatus entity)
        {
            onAdd();
            context.ProceduralFaultStatuses.Add(entity);
        }

        public void Remove(ProceduralFaultStatus entity)
        {
            context.ProceduralFaultStatuses.Attach(entity);
            context.ProceduralFaultStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ProceduralFaultStatus entity)
        {
            onUpdate();
            context.ProceduralFaultStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ProceduralFaultStatus> All()
        {
            return context.ProceduralFaultStatuses.ToList();
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
	 