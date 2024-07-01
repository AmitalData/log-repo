 
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
   public class ReferenceTypeRepository:IRepository<ReferenceType>
   {
   
        IShipmentsContext currentContext;

        public ReferenceTypeRepository(int tenant)
        {
            currentContext = new ShipmentsContext();
        }

        public ReferenceTypeRepository(IShipmentsContext context)
        {
            currentContext = context;
        }

		 
		
		public  ReferenceType GetSingle(string code)
        {
            return (from a in context.ReferenceTypes
                    where a.Code == code 
                    select a).FirstOrDefault();
        }

        public IQueryable<ReferenceType> GetAll()
        {
            return from a in context.ReferenceTypes  
                   select a;
        }
		        
        public ReferenceType GetSingle(EntityKeyFields entityKeys)
        {
            //ReferenceTypeKeys keys = entityKeys as ReferenceTypeKeys;
            //return (from a in context.ReferenceTypes
            //        where a.Code == keys.Code
            //        select a).FirstOrDefault();
            throw new NotImplementedException();
        }

        public void Add(ReferenceType entity)
        {
            context.ReferenceTypes.Add(entity);
        }

        public void Remove(ReferenceType entity)
        {
            context.ReferenceTypes.Attach(entity);
            context.ReferenceTypes.Remove(entity);
        }
        public void Update(ReferenceType entity)
        {
            context.ReferenceTypes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<ReferenceType> All()
        {
            return context.ReferenceTypes.ToList();
        }

        public IShipmentsContext context
        {
            get { return currentContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<ReferenceType> GetMulti(EntityKeyFields entityKeys)
        {

            throw new NotImplementedException();
        }


    }
}
	 