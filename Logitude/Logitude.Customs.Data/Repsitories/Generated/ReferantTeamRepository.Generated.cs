 
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
   public partial class ReferantTeamRepository:IRepository<ReferantTeam>
   {
   
        private ICustomContext currentContext;
        public ReferantTeamRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ReferantTeamRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ReferantTeam GetSingle(string code, int tenant)
        {
            return (from a in context.ReferantTeams
                    where a.Code == code && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<ReferantTeam> GetAll(int tenant)
        {
            return from a in context.ReferantTeams  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public ReferantTeam GetSingle(EntityKeyFields entityKeys)
        {
            ReferantTeamKeys keys = entityKeys as ReferantTeamKeys;
            return (from a in context.ReferantTeams
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ReferantTeam entity)
        {
            onAdd();
            context.ReferantTeams.Add(entity);
        }

        public void Remove(ReferantTeam entity)
        {
            context.ReferantTeams.Attach(entity);
            context.ReferantTeams.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ReferantTeam entity)
        {
            onUpdate();
            context.ReferantTeams.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ReferantTeam> All()
        {
            return context.ReferantTeams.ToList();
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
	 