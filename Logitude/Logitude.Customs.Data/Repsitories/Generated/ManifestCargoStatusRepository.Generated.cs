 
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
   public partial class ManifestCargoStatusRepository:IRepository<ManifestCargoStatus>
   {
   
        private ICustomContext currentContext;
        public ManifestCargoStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ManifestCargoStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ManifestCargoStatus GetSingle(string code)
        {
            return (from a in context.ManifestCargoStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ManifestCargoStatus> GetAll()
        {
            return from a in context.ManifestCargoStatuses  
                   select a;
        }
				 
        public ManifestCargoStatus GetSingle(EntityKeyFields entityKeys)
        {
            ManifestCargoStatusKeys keys = entityKeys as ManifestCargoStatusKeys;
            return (from a in context.ManifestCargoStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ManifestCargoStatus entity)
        {
            onAdd();
            context.ManifestCargoStatuses.Add(entity);
        }

        public void Remove(ManifestCargoStatus entity)
        {
            context.ManifestCargoStatuses.Attach(entity);
            context.ManifestCargoStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ManifestCargoStatus entity)
        {
            onUpdate();
            context.ManifestCargoStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ManifestCargoStatus> All()
        {
            return context.ManifestCargoStatuses.ToList();
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
	 