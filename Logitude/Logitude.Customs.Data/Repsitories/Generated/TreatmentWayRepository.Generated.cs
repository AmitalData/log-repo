 
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
   public partial class TreatmentWayRepository:IRepository<TreatmentWay>
   {
   
        private ICustomContext currentContext;
        public TreatmentWayRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public TreatmentWayRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  TreatmentWay GetSingle(string code)
        {
            return (from a in context.TreatmentWays
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<TreatmentWay> GetAll()
        {
            return from a in context.TreatmentWays  
                   select a;
        }
				 
        public TreatmentWay GetSingle(EntityKeyFields entityKeys)
        {
            TreatmentWayKeys keys = entityKeys as TreatmentWayKeys;
            return (from a in context.TreatmentWays
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(TreatmentWay entity)
        {
            onAdd();
            context.TreatmentWays.Add(entity);
        }

        public void Remove(TreatmentWay entity)
        {
            context.TreatmentWays.Attach(entity);
            context.TreatmentWays.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(TreatmentWay entity)
        {
            onUpdate();
            context.TreatmentWays.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<TreatmentWay> All()
        {
            return context.TreatmentWays.ToList();
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
	 