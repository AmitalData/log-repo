 
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
   public partial class AmendCancellRequestInitiatorRepository:IRepository<AmendCancellRequestInitiator>
   {
   
        private ICustomContext currentContext;
        public AmendCancellRequestInitiatorRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public AmendCancellRequestInitiatorRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  AmendCancellRequestInitiator GetSingle(string code)
        {
            return (from a in context.AmendCancellRequestInitiators
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<AmendCancellRequestInitiator> GetAll()
        {
            return from a in context.AmendCancellRequestInitiators  
                   select a;
        }
				 
        public AmendCancellRequestInitiator GetSingle(EntityKeyFields entityKeys)
        {
            AmendCancellRequestInitiatorKeys keys = entityKeys as AmendCancellRequestInitiatorKeys;
            return (from a in context.AmendCancellRequestInitiators
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(AmendCancellRequestInitiator entity)
        {
            onAdd();
            context.AmendCancellRequestInitiators.Add(entity);
        }

        public void Remove(AmendCancellRequestInitiator entity)
        {
            context.AmendCancellRequestInitiators.Attach(entity);
            context.AmendCancellRequestInitiators.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(AmendCancellRequestInitiator entity)
        {
            onUpdate();
            context.AmendCancellRequestInitiators.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AmendCancellRequestInitiator> All()
        {
            return context.AmendCancellRequestInitiators.ToList();
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
	 