 
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
   public partial class MamanSpecialActionStatusRepository:IRepository<MamanSpecialActionStatus>
   {
   
        private ICustomContext currentContext;
        public MamanSpecialActionStatusRepository(int tenant)
        {
            currentContext = CustomContext.GetContext(tenant);
        }

        public MamanSpecialActionStatusRepository(ICustomContext context)
        {
            currentContext = context;
        }

		 
		
		public  MamanSpecialActionStatus GetSingle(string code)
        {
            return (from a in context.MamanSpecialActionStatuses
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<MamanSpecialActionStatus> GetAll()
        {
            return from a in context.MamanSpecialActionStatuses  
                   select a;
        }
				 
        public MamanSpecialActionStatus GetSingle(EntityKeyFields entityKeys)
        {
            MamanSpecialActionStatusKeys keys = entityKeys as MamanSpecialActionStatusKeys;
            return (from a in context.MamanSpecialActionStatuses
                    where a.Code == keys.Code
                    select a).FirstOrDefault();
        }
		 		                 
        partial void onAdd();//Partial Methods Definition in Generated
        public void Add(MamanSpecialActionStatus entity)
        {
            onAdd();
            context.MamanSpecialActionStatuses.Add(entity);
        }

        public void Remove(MamanSpecialActionStatus entity)
        {
            context.MamanSpecialActionStatuses.Attach(entity);
            context.MamanSpecialActionStatuses.Remove(entity);
        }

        partial void onUpdate();//Partial Methods Definition in Generated
        public void Update(MamanSpecialActionStatus entity)
        {
            onUpdate();
            context.MamanSpecialActionStatuses.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<MamanSpecialActionStatus> All()
        {
            return context.MamanSpecialActionStatuses.ToList();
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
	 