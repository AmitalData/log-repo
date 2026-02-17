 
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
   public partial class ProceduralFaultRepository:IRepository<ProceduralFault>
   {
   
        private ICustomContext currentContext;
        public ProceduralFaultRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ProceduralFaultRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ProceduralFault GetSingle(string id, int tenant)
        {
            return (from a in context.ProceduralFaults
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ProceduralFault> GetAll(int tenant)
        {
            return from a in context.ProceduralFaults  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ProceduralFault GetSingle(EntityKeyFields entityKeys)
        {
            ProceduralFaultKeys keys = entityKeys as ProceduralFaultKeys;
            return (from a in context.ProceduralFaults
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ProceduralFault entity)
        {
            onAdd();
            context.ProceduralFaults.Add(entity);
        }

        public void Remove(ProceduralFault entity)
        {
            context.ProceduralFaults.Attach(entity);
            context.ProceduralFaults.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ProceduralFault entity)
        {
            onUpdate();
            context.ProceduralFaults.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ProceduralFault> All()
        {
            return context.ProceduralFaults.ToList();
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
	 