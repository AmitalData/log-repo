 
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
   public partial class LastReleaseFromWarehouseRepository:IRepository<LastReleaseFromWarehouse>
   {
   
        private ICustomContext currentContext;
        public LastReleaseFromWarehouseRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public LastReleaseFromWarehouseRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  LastReleaseFromWarehouse GetSingle(string code)
        {
            return (from a in context.LastReleaseFromWarehouses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<LastReleaseFromWarehouse> GetAll()
        {
            return from a in context.LastReleaseFromWarehouses  
                   select a;
        }
				 
        public LastReleaseFromWarehouse GetSingle(EntityKeyFields entityKeys)
        {
            LastReleaseFromWarehouseKeys keys = entityKeys as LastReleaseFromWarehouseKeys;
            return (from a in context.LastReleaseFromWarehouses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(LastReleaseFromWarehouse entity)
        {
            onAdd();
            context.LastReleaseFromWarehouses.Add(entity);
        }

        public void Remove(LastReleaseFromWarehouse entity)
        {
            context.LastReleaseFromWarehouses.Attach(entity);
            context.LastReleaseFromWarehouses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(LastReleaseFromWarehouse entity)
        {
            onUpdate();
            context.LastReleaseFromWarehouses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<LastReleaseFromWarehouse> All()
        {
            return context.LastReleaseFromWarehouses.ToList();
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
	 