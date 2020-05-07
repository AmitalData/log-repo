 
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
   public partial class CustomsShipRepository:IRepository<CustomsShip>
   {
   
        private ICustomContext currentContext;
        public CustomsShipRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public CustomsShipRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  CustomsShip GetSingle(string code)
        {
            return (from a in context.CustomsShips
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<CustomsShip> GetAll()
        {
            return from a in context.CustomsShips  
                   select a;
        }
				 
        public CustomsShip GetSingle(EntityKeyFields entityKeys)
        {
            CustomsShipKeys keys = entityKeys as CustomsShipKeys;
            return (from a in context.CustomsShips
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(CustomsShip entity)
        {
            onAdd();
            context.CustomsShips.Add(entity);
        }

        public void Remove(CustomsShip entity)
        {
            context.CustomsShips.Attach(entity);
            context.CustomsShips.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(CustomsShip entity)
        {
            onUpdate();
            context.CustomsShips.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsShip> All()
        {
            return context.CustomsShips.ToList();
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
	 