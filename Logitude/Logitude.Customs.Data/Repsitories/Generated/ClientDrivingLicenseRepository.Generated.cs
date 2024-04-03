 
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
   public partial class ClientDrivingLicenseRepository:IRepository<ClientDrivingLicense>
   {
   
        private ICustomContext currentContext;
        public ClientDrivingLicenseRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ClientDrivingLicenseRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ClientDrivingLicense GetSingle(string clientid, int line, int tenant)
        {
            return (from a in context.ClientDrivingLicenses
                    where a.ClientId == clientid && a.Line == line && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ClientDrivingLicense> GetAll(int tenant)
        {
            return from a in context.ClientDrivingLicenses  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ClientDrivingLicense GetSingle(EntityKeyFields entityKeys)
        {
            ClientDrivingLicenseKeys keys = entityKeys as ClientDrivingLicenseKeys;
            return (from a in context.ClientDrivingLicenses
                    where a.ClientId == keys.ClientId && a.Line == keys.Line
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ClientDrivingLicense entity)
        {
            onAdd();
            context.ClientDrivingLicenses.Add(entity);
        }

        public void Remove(ClientDrivingLicense entity)
        {
            context.ClientDrivingLicenses.Attach(entity);
            context.ClientDrivingLicenses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ClientDrivingLicense entity)
        {
            onUpdate();
            context.ClientDrivingLicenses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ClientDrivingLicense> All()
        {
            return context.ClientDrivingLicenses.ToList();
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
	 