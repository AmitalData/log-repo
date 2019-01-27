 
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
   public partial class ToggleRepository:IRepository<Toggle>
   {
   
        private IInfrastructureContext currentContext;
        public ToggleRepository(int tenant)
        {
            currentContext = InfrastructureContext.GetContext(tenant);
        }

        public ToggleRepository(IInfrastructureContext context)
        {
            currentContext = context;
        }

		 
		
		public  Toggle GetSingle(string code)
        {
            return (from a in context.Toggles
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<Toggle> GetAll()
        {
            return from a in context.Toggles  
                   select a;
        }
				 
        public Toggle GetSingle(EntityKeyFields entityKeys)
        {
            ToggleKeys keys = entityKeys as ToggleKeys;
            return (from a in context.Toggles
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Toggle entity)
        {
            onAdd();
            context.Toggles.Add(entity);
        }

        public void Remove(Toggle entity)
        {
            context.Toggles.Attach(entity);
            context.Toggles.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Toggle entity)
        {
            onUpdate();
            context.Toggles.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Toggle> All()
        {
            return context.Toggles.ToList();
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
	 