 
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
   public partial class ApprovedProfessionRepository:IRepository<ApprovedProfession>
   {
   
        private ICustomContext currentContext;
        public ApprovedProfessionRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public ApprovedProfessionRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  ApprovedProfession GetSingle(string code)
        {
            return (from a in context.ApprovedProfessions
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ApprovedProfession> GetAll()
        {
            return from a in context.ApprovedProfessions  
                   select a;
        }
				 
        public ApprovedProfession GetSingle(EntityKeyFields entityKeys)
        {
            ApprovedProfessionKeys keys = entityKeys as ApprovedProfessionKeys;
            return (from a in context.ApprovedProfessions
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		         
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(ApprovedProfession entity)
        {
            onAdd();
            context.ApprovedProfessions.Add(entity);
        }

        public void Remove(ApprovedProfession entity)
        {
            context.ApprovedProfessions.Attach(entity);
            context.ApprovedProfessions.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(ApprovedProfession entity)
        {
            onUpdate();
            context.ApprovedProfessions.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ApprovedProfession> All()
        {
            return context.ApprovedProfessions.ToList();
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
	 