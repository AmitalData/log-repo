 
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
   public partial class DBMigrationRepository:IRepository<DBMigration>
   {
   
        private ICustomContext currentContext;
        public DBMigrationRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DBMigrationRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DBMigration GetSingle(string id)
        {
            return (from a in context.DBMigrations
                    where a.Id == id 
                    select a).FirstOrDefault();
        }

        public IQueryable<DBMigration> GetAll()
        {
            return from a in context.DBMigrations  
                   select a;
        }
				 
        public DBMigration GetSingle(EntityKeyFields entityKeys)
        {
            DBMigrationKeys keys = entityKeys as DBMigrationKeys;
            return (from a in context.DBMigrations
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DBMigration entity)
        {
            onAdd();
            context.DBMigrations.Add(entity);
        }

        public void Remove(DBMigration entity)
        {
            context.DBMigrations.Attach(entity);
            context.DBMigrations.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DBMigration entity)
        {
            onUpdate();
            context.DBMigrations.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DBMigration> All()
        {
            return context.DBMigrations.ToList();
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
	 