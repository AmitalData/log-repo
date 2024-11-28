 
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Simplog.Server.Infrastructure;
using Simplog.Data.ShipmentsModel;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.Repositories
{
   public class ResponsibilityRepository : IRepository<Responsibility>
   {
   
        IShipmentsContext currentContext;

        public ResponsibilityRepository(int tenant)
        {
            currentContext = new ShipmentsContext();
        }

        public ResponsibilityRepository(IShipmentsContext context)
        {
            currentContext = context;
        }

		 
		
		public Responsibility GetSingleResponsibility(string code)
        {
            return (from a in context.Responsibilities
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<Responsibility> GetAll()
        {
            return from a in context.Responsibilities
                   select a;
        }

        public IQueryable<Responsibility> GetResponsibilities()
        {
            return from a in context.Responsibilities
                   select a;
        }

        public Responsibility GetSingle(EntityKeyFields entityKeys)
        {
            //ResponsibilityKeys keys = entityKeys as ResponsibilityKeys;
            //return (from a in context.Responsibilities
            //        where a.Code == keys.Code
            //        select a).FirstOrDefault();
            throw new NotImplementedException();
        }

        public void Add(Responsibility entity)
        {
            context.Responsibilities.Add(entity);
        }

        public void Remove(Responsibility entity)
        {
            context.Responsibilities.Attach(entity);
            context.Responsibilities.Remove(entity);
        }
        public void Update(Responsibility entity)
        {
            context.Responsibilities.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<Responsibility> All()
        {
            return context.Responsibilities.ToList();
        }

        public IShipmentsContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<Responsibility> GetMulti(EntityKeyFields entityKeys)
        {

            throw new NotImplementedException();
        }


    }
}
	 