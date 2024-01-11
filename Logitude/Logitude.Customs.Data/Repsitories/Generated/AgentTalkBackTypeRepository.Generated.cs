 
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
   public partial class AgentTalkBackTypeRepository:IRepository<AgentTalkBackType>
   {
   
        private ICustomContext currentContext;
        public AgentTalkBackTypeRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public AgentTalkBackTypeRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  AgentTalkBackType GetSingle(string code)
        {
            return (from a in context.AgentTalkBackTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<AgentTalkBackType> GetAll()
        {
            return from a in context.AgentTalkBackTypes  
                   select a;
        }
				 
        public AgentTalkBackType GetSingle(EntityKeyFields entityKeys)
        {
            AgentTalkBackTypeKeys keys = entityKeys as AgentTalkBackTypeKeys;
            return (from a in context.AgentTalkBackTypes
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(AgentTalkBackType entity)
        {
            onAdd();
            context.AgentTalkBackTypes.Add(entity);
        }

        public void Remove(AgentTalkBackType entity)
        {
            context.AgentTalkBackTypes.Attach(entity);
            context.AgentTalkBackTypes.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(AgentTalkBackType entity)
        {
            onUpdate();
            context.AgentTalkBackTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AgentTalkBackType> All()
        {
            return context.AgentTalkBackTypes.ToList();
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
	 