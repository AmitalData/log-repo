 
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
   public partial class ClientDrivingLicenseTypeRepository:IRepository<ClientDrivingLicenseType>
   {
   
        private ICustomContext currentContext;
        public ClientDrivingLicenseTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ClientDrivingLicenseTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ClientDrivingLicenseType GetSingle(string clientid, int clientdrivinglicenseline, string driverslicensetypecode, int tenant)
        {
            return (from a in context.ClientDrivingLicenseTypes
                    where a.ClientId == clientid && a.ClientDrivingLicenseLine == clientdrivinglicenseline && a.DriversLicenseTypeCode == driverslicensetypecode && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ClientDrivingLicenseType> GetAll(int tenant)
        {
            return from a in context.ClientDrivingLicenseTypes  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ClientDrivingLicenseType GetSingle(EntityKeyFields entityKeys)
        {
            ClientDrivingLicenseTypeKeys keys = entityKeys as ClientDrivingLicenseTypeKeys;
            return (from a in context.ClientDrivingLicenseTypes
                    where a.ClientId == keys.ClientId && a.ClientDrivingLicenseLine == keys.ClientDrivingLicenseLine && a.DriversLicenseTypeCode == keys.DriversLicenseTypeCode
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ClientDrivingLicenseType entity)
        {
            onAdd();
            context.ClientDrivingLicenseTypes.Add(entity);
        }

        public void Remove(ClientDrivingLicenseType entity)
        {
            context.ClientDrivingLicenseTypes.Attach(entity);
            context.ClientDrivingLicenseTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ClientDrivingLicenseType entity)
        {
            onUpdate();
            context.ClientDrivingLicenseTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ClientDrivingLicenseType> All()
        {
            return context.ClientDrivingLicenseTypes.ToList();
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
	 