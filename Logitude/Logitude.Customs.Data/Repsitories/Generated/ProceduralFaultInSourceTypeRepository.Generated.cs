 
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
   public partial class ProceduralFaultInSourceTypeRepository:IRepository<ProceduralFaultInSourceType>
   {
   
        private ICustomContext currentContext;
        public ProceduralFaultInSourceTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ProceduralFaultInSourceTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ProceduralFaultInSourceType GetSingle(string code)
        {
            return (from a in context.ProceduralFaultInSourceTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ProceduralFaultInSourceType> GetAll()
        {
            return from a in context.ProceduralFaultInSourceTypes  
                   select a;
        }
				 
        public ProceduralFaultInSourceType GetSingle(EntityKeyFields entityKeys)
        {
            ProceduralFaultInSourceTypeKeys keys = entityKeys as ProceduralFaultInSourceTypeKeys;
            return (from a in context.ProceduralFaultInSourceTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ProceduralFaultInSourceType entity)
        {
            onAdd();
            context.ProceduralFaultInSourceTypes.Add(entity);
        }

        public void Remove(ProceduralFaultInSourceType entity)
        {
            context.ProceduralFaultInSourceTypes.Attach(entity);
            context.ProceduralFaultInSourceTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ProceduralFaultInSourceType entity)
        {
            onUpdate();
            context.ProceduralFaultInSourceTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ProceduralFaultInSourceType> All()
        {
            return context.ProceduralFaultInSourceTypes.ToList();
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
	 