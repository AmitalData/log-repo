 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.CRM.Data.Repsitories
{
   public partial class RatingRepository:IRepository<Rating>
   {
   
        private ICRMContext currentContext;
        public RatingRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public RatingRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  Rating GetSingle(string code)
        {
            return (from a in context.Ratings
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<Rating> GetAll()
        {
            return from a in context.Ratings  
                   select a;
        }
				 
        public Rating GetSingle(EntityKeyFields entityKeys)
        {
            RatingKeys keys = entityKeys as RatingKeys;
            return (from a in context.Ratings
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Rating entity)
        {
            onAdd();
            context.Ratings.Add(entity);
        }

        public void Remove(Rating entity)
        {
            context.Ratings.Attach(entity);
            context.Ratings.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Rating entity)
        {
            onUpdate();
            context.Ratings.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Rating> All()
        {
            return context.Ratings.ToList();
        }

        private ICRMContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 