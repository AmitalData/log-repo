 
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
   public partial class ProceduralFaultInProcessTypeRepository:IRepository<ProceduralFaultInProcessType>
   {
   
        private ICustomContext currentContext;
        public ProceduralFaultInProcessTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ProceduralFaultInProcessTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ProceduralFaultInProcessType GetSingle(string code)
        {
            return (from a in context.ProceduralFaultInProcessTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ProceduralFaultInProcessType> GetAll()
        {
            return from a in context.ProceduralFaultInProcessTypes  
                   select a;
        }
				 
        public ProceduralFaultInProcessType GetSingle(EntityKeyFields entityKeys)
        {
            ProceduralFaultInProcessTypeKeys keys = entityKeys as ProceduralFaultInProcessTypeKeys;
            return (from a in context.ProceduralFaultInProcessTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ProceduralFaultInProcessType entity)
        {
            onAdd();
            context.ProceduralFaultInProcessTypes.Add(entity);
        }

        public void Remove(ProceduralFaultInProcessType entity)
        {
            context.ProceduralFaultInProcessTypes.Attach(entity);
            context.ProceduralFaultInProcessTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ProceduralFaultInProcessType entity)
        {
            onUpdate();
            context.ProceduralFaultInProcessTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ProceduralFaultInProcessType> All()
        {
            return context.ProceduralFaultInProcessTypes.ToList();
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
	 