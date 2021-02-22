 
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
   public partial class FclLclCodeRepository:IRepository<FclLclCode>
   {
   
        private ICustomContext currentContext;
        public FclLclCodeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public FclLclCodeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  FclLclCode GetSingle(string code)
        {
            return (from a in context.FclLclCodes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<FclLclCode> GetAll()
        {
            return from a in context.FclLclCodes  
                   select a;
        }
				 
        public FclLclCode GetSingle(EntityKeyFields entityKeys)
        {
            FclLclCodeKeys keys = entityKeys as FclLclCodeKeys;
            return (from a in context.FclLclCodes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(FclLclCode entity)
        {
            onAdd();
            context.FclLclCodes.Add(entity);
        }

        public void Remove(FclLclCode entity)
        {
            context.FclLclCodes.Attach(entity);
            context.FclLclCodes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(FclLclCode entity)
        {
            onUpdate();
            context.FclLclCodes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<FclLclCode> All()
        {
            return context.FclLclCodes.ToList();
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
	 