 
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
   public partial class CarWeightRepository:IRepository<CarWeight>
   {
   
        private ICustomContext currentContext;
        public CarWeightRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CarWeightRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CarWeight GetSingle(string code)
        {
            return (from a in context.CarWeights
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CarWeight> GetAll()
        {
            return from a in context.CarWeights  
                   select a;
        }
				 
        public CarWeight GetSingle(EntityKeyFields entityKeys)
        {
            CarWeightKeys keys = entityKeys as CarWeightKeys;
            return (from a in context.CarWeights
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CarWeight entity)
        {
            onAdd();
            context.CarWeights.Add(entity);
        }

        public void Remove(CarWeight entity)
        {
            context.CarWeights.Attach(entity);
            context.CarWeights.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CarWeight entity)
        {
            onUpdate();
            context.CarWeights.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CarWeight> All()
        {
            return context.CarWeights.ToList();
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
	 