 
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
   public partial class DBMigrationLineRepository:IRepository<DBMigrationLine>
   {
   
        private ICustomContext currentContext;
        public DBMigrationLineRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public DBMigrationLineRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  DBMigrationLine GetSingle(string dbmigrationid, int counterkey)
        {
            return (from a in context.DBMigrationLines
                    where a.DBMigrationId == dbmigrationid && a.CounterKey == counterkey 
                    select a).FirstOrDefault();
        }

        public IQueryable<DBMigrationLine> GetAll()
        {
            return from a in context.DBMigrationLines  
                   select a;
        }
				 
        public DBMigrationLine GetSingle(EntityKeyFields entityKeys)
        {
            DBMigrationLineKeys keys = entityKeys as DBMigrationLineKeys;
            return (from a in context.DBMigrationLines
                    where a.DBMigrationId == keys.DBMigrationId && a.CounterKey == keys.CounterKey
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(DBMigrationLine entity)
        {
            onAdd();
            context.DBMigrationLines.Add(entity);
        }

        public void Remove(DBMigrationLine entity)
        {
            context.DBMigrationLines.Attach(entity);
            context.DBMigrationLines.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(DBMigrationLine entity)
        {
            onUpdate();
            context.DBMigrationLines.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<DBMigrationLine> All()
        {
            return context.DBMigrationLines.ToList();
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
	 