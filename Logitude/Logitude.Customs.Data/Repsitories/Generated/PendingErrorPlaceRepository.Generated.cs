 
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
   public partial class PendingErrorPlaceRepository:IRepository<PendingErrorPlace>
   {
   
        private ICustomContext currentContext;
        public PendingErrorPlaceRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public PendingErrorPlaceRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  PendingErrorPlace GetSingle(string code)
        {
            return (from a in context.PendingErrorPlaces
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<PendingErrorPlace> GetAll()
        {
            return from a in context.PendingErrorPlaces  
                   select a;
        }
				 
        public PendingErrorPlace GetSingle(EntityKeyFields entityKeys)
        {
            PendingErrorPlaceKeys keys = entityKeys as PendingErrorPlaceKeys;
            return (from a in context.PendingErrorPlaces
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(PendingErrorPlace entity)
        {
            onAdd();
            context.PendingErrorPlaces.Add(entity);
        }

        public void Remove(PendingErrorPlace entity)
        {
            context.PendingErrorPlaces.Attach(entity);
            context.PendingErrorPlaces.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(PendingErrorPlace entity)
        {
            onUpdate();
            context.PendingErrorPlaces.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<PendingErrorPlace> All()
        {
            return context.PendingErrorPlaces.ToList();
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
	 