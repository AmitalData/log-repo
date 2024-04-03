 
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
   public partial class InterfaceSendOptionRepository:IRepository<InterfaceSendOption>
   {
   
        private ICustomContext currentContext;
        public InterfaceSendOptionRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public InterfaceSendOptionRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  InterfaceSendOption GetSingle(string code)
        {
            return (from a in context.InterfaceSendOptions
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<InterfaceSendOption> GetAll()
        {
            return from a in context.InterfaceSendOptions  
                   select a;
        }
				 
        public InterfaceSendOption GetSingle(EntityKeyFields entityKeys)
        {
            InterfaceSendOptionKeys keys = entityKeys as InterfaceSendOptionKeys;
            return (from a in context.InterfaceSendOptions
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(InterfaceSendOption entity)
        {
            onAdd();
            context.InterfaceSendOptions.Add(entity);
        }

        public void Remove(InterfaceSendOption entity)
        {
            context.InterfaceSendOptions.Attach(entity);
            context.InterfaceSendOptions.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(InterfaceSendOption entity)
        {
            onUpdate();
            context.InterfaceSendOptions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<InterfaceSendOption> All()
        {
            return context.InterfaceSendOptions.ToList();
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
	 