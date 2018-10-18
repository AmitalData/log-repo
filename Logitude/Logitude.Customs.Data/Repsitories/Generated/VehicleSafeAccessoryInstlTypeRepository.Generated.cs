 
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
   public partial class VehicleSafeAccessoryInstlTypeRepository:IRepository<VehicleSafeAccessoryInstlType>
   {
   
        private ICustomContext currentContext;
        public VehicleSafeAccessoryInstlTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public VehicleSafeAccessoryInstlTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  VehicleSafeAccessoryInstlType GetSingle(string code)
        {
            return (from a in context.VehicleSafeAccessoryInstlTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<VehicleSafeAccessoryInstlType> GetAll()
        {
            return from a in context.VehicleSafeAccessoryInstlTypes  
                   select a;
        }
				 
        public VehicleSafeAccessoryInstlType GetSingle(EntityKeyFields entityKeys)
        {
            VehicleSafeAccessoryInstlTypeKeys keys = entityKeys as VehicleSafeAccessoryInstlTypeKeys;
            return (from a in context.VehicleSafeAccessoryInstlTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(VehicleSafeAccessoryInstlType entity)
        {
            onAdd();
            context.VehicleSafeAccessoryInstlTypes.Add(entity);
        }

        public void Remove(VehicleSafeAccessoryInstlType entity)
        {
            context.VehicleSafeAccessoryInstlTypes.Attach(entity);
            context.VehicleSafeAccessoryInstlTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(VehicleSafeAccessoryInstlType entity)
        {
            onUpdate();
            context.VehicleSafeAccessoryInstlTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<VehicleSafeAccessoryInstlType> All()
        {
            return context.VehicleSafeAccessoryInstlTypes.ToList();
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
	 