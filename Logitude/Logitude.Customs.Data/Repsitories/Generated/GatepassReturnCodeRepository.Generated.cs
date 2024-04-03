 
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
   public partial class GatepassReturnCodeRepository:IRepository<GatepassReturnCode>
   {
   
        private ICustomContext currentContext;
        public GatepassReturnCodeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public GatepassReturnCodeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  GatepassReturnCode GetSingle(string code)
        {
            return (from a in context.GatepassReturnCodes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<GatepassReturnCode> GetAll()
        {
            return from a in context.GatepassReturnCodes  
                   select a;
        }
				 
        public GatepassReturnCode GetSingle(EntityKeyFields entityKeys)
        {
            GatepassReturnCodeKeys keys = entityKeys as GatepassReturnCodeKeys;
            return (from a in context.GatepassReturnCodes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(GatepassReturnCode entity)
        {
            onAdd();
            context.GatepassReturnCodes.Add(entity);
        }

        public void Remove(GatepassReturnCode entity)
        {
            context.GatepassReturnCodes.Attach(entity);
            context.GatepassReturnCodes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(GatepassReturnCode entity)
        {
            onUpdate();
            context.GatepassReturnCodes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<GatepassReturnCode> All()
        {
            return context.GatepassReturnCodes.ToList();
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
	 