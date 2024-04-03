 
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
   public partial class MamanStatusRepository:IRepository<MamanStatus>
   {
   
        private ICustomContext currentContext;
        public MamanStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public MamanStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  MamanStatus GetSingle(string code)
        {
            return (from a in context.MamanStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<MamanStatus> GetAll()
        {
            return from a in context.MamanStatuses  
                   select a;
        }
				 
        public MamanStatus GetSingle(EntityKeyFields entityKeys)
        {
            MamanStatusKeys keys = entityKeys as MamanStatusKeys;
            return (from a in context.MamanStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(MamanStatus entity)
        {
            onAdd();
            context.MamanStatuses.Add(entity);
        }

        public void Remove(MamanStatus entity)
        {
            context.MamanStatuses.Attach(entity);
            context.MamanStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(MamanStatus entity)
        {
            onUpdate();
            context.MamanStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<MamanStatus> All()
        {
            return context.MamanStatuses.ToList();
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
	 