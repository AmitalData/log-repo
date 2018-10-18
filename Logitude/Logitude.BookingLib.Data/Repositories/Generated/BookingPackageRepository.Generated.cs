 
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
   public partial class BookingPackageRepository:IRepository<BookingPackage>
   {
   
        private IBookingContext currentContext;
        public BookingPackageRepository(int tenant)
        {
            currentContext = BookingContext.GetContext(tenant);
        }

        public BookingPackageRepository(IBookingContext context)
        {
            currentContext = context;
        }

		 
		
		public  BookingPackage GetSingle(string id, int tenant)
        {
            return (from a in context.BookingPackages
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<BookingPackage> GetAll(int tenant)
        {
            return from a in context.BookingPackages  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public BookingPackage GetSingle(EntityKeyFields entityKeys)
        {
            BookingPackageKeys keys = entityKeys as BookingPackageKeys;
            return (from a in context.BookingPackages
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(BookingPackage entity)
        {
            onAdd();
            context.BookingPackages.Add(entity);
        }

        public void Remove(BookingPackage entity)
        {
            context.BookingPackages.Attach(entity);
            context.BookingPackages.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(BookingPackage entity)
        {
            onUpdate();
            context.BookingPackages.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<BookingPackage> All()
        {
            return context.BookingPackages.ToList();
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
	 