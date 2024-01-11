 
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
   public partial class SignStationRepository:IRepository<SignStation>
   {
   
        private ICustomContext currentContext;
        public SignStationRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public SignStationRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  SignStation GetSingle(string customsagentid, string personid, int tenant)
        {
            return (from a in context.SignStations
                    where a.CustomsAgentId == customsagentid && a.PersonId == personid && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<SignStation> GetAll(int tenant)
        {
            return from a in context.SignStations  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public SignStation GetSingle(EntityKeyFields entityKeys)
        {
            SignStationKeys keys = entityKeys as SignStationKeys;
            return (from a in context.SignStations
                    where a.CustomsAgentId == keys.CustomsAgentId && a.PersonId == keys.PersonId
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(SignStation entity)
        {
            onAdd();
            context.SignStations.Add(entity);
        }

        public void Remove(SignStation entity)
        {
            context.SignStations.Attach(entity);
            context.SignStations.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(SignStation entity)
        {
            onUpdate();
            context.SignStations.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<SignStation> All()
        {
            return context.SignStations.ToList();
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
	 