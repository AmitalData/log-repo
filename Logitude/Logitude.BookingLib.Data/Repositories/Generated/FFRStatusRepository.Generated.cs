 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.BookingLib.Data.EntityPOCOs;
using Logitude.BookingLib.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.BookingLib.Data.Repositories
{
   public partial class FFRStatusRepository:IRepository<FFRStatus>
   {
   
        private IBookingContext currentContext;
        public FFRStatusRepository(int tenant)
        {
            currentContext = BookingContext.GetContext(tenant);
        }

        public FFRStatusRepository(IBookingContext context)
        {
            currentContext = context;
        }

		 
		
		public  FFRStatus GetSingle(string code)
        {
            return (from a in context.FFRStatus
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<FFRStatus> GetAll()
        {
            return from a in context.FFRStatus  
                   select a;
        }
				 
        public FFRStatus GetSingle(EntityKeyFields entityKeys)
        {
            FFRStatusKeys keys = entityKeys as FFRStatusKeys;
            return (from a in context.FFRStatus
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(FFRStatus entity)
        {
            onAdd();
            context.FFRStatus.Add(entity);
        }

        public void Remove(FFRStatus entity)
        {
            context.FFRStatus.Attach(entity);
            context.FFRStatus.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(FFRStatus entity)
        {
            onUpdate();
            context.FFRStatus.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<FFRStatus> All()
        {
            return context.FFRStatus.ToList();
        }

        private IBookingContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 