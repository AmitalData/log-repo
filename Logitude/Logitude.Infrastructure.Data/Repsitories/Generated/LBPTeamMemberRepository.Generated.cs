 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.Infrastructure.Data.EntityPOCOs;
using Logitude.Infrastructure.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.Infrastructure.Data.Repsitories
{
   public partial class LBPTeamMemberRepository:IRepository<LBPTeamMember>
   {
   
        private IInfrastructureContext currentContext;
        public LBPTeamMemberRepository(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public LBPTeamMemberRepository(IInfrastructureContext context)
        {
            currentContext = context;
        }

		 
		
		public  LBPTeamMember GetSingle(string id, int tenant)
        {
            return (from a in context.LBPTeamMembers
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<LBPTeamMember> GetAll(int tenant)
        {
            return from a in context.LBPTeamMembers  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public LBPTeamMember GetSingle(EntityKeyFields entityKeys)
        {
            LBPTeamMemberKeys keys = entityKeys as LBPTeamMemberKeys;
            return (from a in context.LBPTeamMembers
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(LBPTeamMember entity)
        {
            onAdd();
            context.LBPTeamMembers.Add(entity);
        }

        public void Remove(LBPTeamMember entity)
        {
            context.LBPTeamMembers.Attach(entity);
            context.LBPTeamMembers.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(LBPTeamMember entity)
        {
            onUpdate();
            context.LBPTeamMembers.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<LBPTeamMember> All()
        {
            return context.LBPTeamMembers.ToList();
        }

        private IInfrastructureContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 