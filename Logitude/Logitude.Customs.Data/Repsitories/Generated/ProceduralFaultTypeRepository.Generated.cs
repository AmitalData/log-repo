 
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
   public partial class ProceduralFaultTypeRepository:IRepository<ProceduralFaultType>
   {
   
        private ICustomContext currentContext;
        public ProceduralFaultTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ProceduralFaultTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ProceduralFaultType GetSingle(string code)
        {
            return (from a in context.ProceduralFaultTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ProceduralFaultType> GetAll()
        {
            return from a in context.ProceduralFaultTypes  
                   select a;
        }
				 
        public ProceduralFaultType GetSingle(EntityKeyFields entityKeys)
        {
            ProceduralFaultTypeKeys keys = entityKeys as ProceduralFaultTypeKeys;
            return (from a in context.ProceduralFaultTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ProceduralFaultType entity)
        {
            onAdd();
            context.ProceduralFaultTypes.Add(entity);
        }

        public void Remove(ProceduralFaultType entity)
        {
            context.ProceduralFaultTypes.Attach(entity);
            context.ProceduralFaultTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ProceduralFaultType entity)
        {
            onUpdate();
            context.ProceduralFaultTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ProceduralFaultType> All()
        {
            return context.ProceduralFaultTypes.ToList();
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
	 