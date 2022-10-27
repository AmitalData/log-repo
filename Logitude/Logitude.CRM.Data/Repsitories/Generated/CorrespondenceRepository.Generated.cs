 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Logitude.CRM.Data.EntityPOCOs;
using Logitude.CRM.Data.EntityKeys;
using Simplog.Server.Infrastructure;

namespace Logitude.CRM.Data.Repsitories
{
   public partial class CorrespondenceRepository:IRepository<Correspondence>
   {
   
        private ICRMContext currentContext;
        public CorrespondenceRepository(int tenant)
        {
            currentContext = CRMContext.GetContext(tenant);
        }

        public CorrespondenceRepository(ICRMContext context)
        {
            currentContext = context;
        }

		 
		
		public  Correspondence GetSingle(string id, int tenant)
        {
            return (from a in context.Correspondences
                    where a.Id == id && a.Tenant == tenant
                    select a).FirstOrDefault();
        }

        public IQueryable<Correspondence> GetAll(int tenant)
        {
            return from a in context.Correspondences  
                   where a.Tenant == tenant
                   select a;
        }
				 
        public Correspondence GetSingle(EntityKeyFields entityKeys)
        {
            CorrespondenceKeys keys = entityKeys as CorrespondenceKeys;
            return (from a in context.Correspondences
                    where a.Id == keys.Id
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(Correspondence entity)
        {
            onAdd();
            context.Correspondences.Add(entity);
        }

        public void Remove(Correspondence entity)
        {
            context.Correspondences.Attach(entity);
            context.Correspondences.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(Correspondence entity)
        {
            onUpdate();
            context.Correspondences.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Correspondence> All()
        {
            return context.Correspondences.ToList();
        }

        private ICRMContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
	 
   }
   }
	 