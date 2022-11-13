using Simplog.Data.InfrastructureModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.InfrastructureModel.Repositories
{
    public class CustomChildObjectRepository : IRepository<CustomChildObject>
    {
        IWebFreightContext webFreightContext;
        public static int MaxNumberOfCustomFields = 50;

        public IWebFreightContext context
        {
            get { return webFreightContext; }
        }

        public CustomChildObjectRepository(IWebFreightContext context)
        {
            webFreightContext = context;
        }

        public CustomChildObjectRepository()
        {
        }


        public CustomChildObject GetById(string id, int tenant)
        {
            return context.CustomChildObjects.Where(d => d.Id == id && d.Tenant == tenant).FirstOrDefault();
        }


        public List<CustomChildObject> GetByParentEntityIdAndParentObjectId(string parentEntityId , string parentObjectTableId , int tenant)
        {
            return (from a in context.CustomChildObjects
                    where a.Tenant == tenant && a.ParentEntityId == parentEntityId && a.ParentObjectTableId == parentObjectTableId
                    select a).ToList();
        }





        public CustomChildObjectRepository(int tenant)
        {
            webFreightContext = WebFreightContext.GetContext(tenant);
        }

        public void Add(CustomChildObject entity)
        {
            context.CustomChildObjects.Add(entity);

        }

        public void Remove(CustomChildObject entity)
        {
            context.CustomChildObjects.Attach(entity);
            context.CustomChildObjects.Remove(entity);
        }

        public void Update(CustomChildObject entity)
        {
            context.CustomChildObjects.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomChildObject> All()
        {
            return context.CustomChildObjects.ToList();
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CustomChildObject> GetMulti(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }




        public CustomChildObject GetSingle(EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public IQueryable<CustomChildObject> GetCustomChildObjects(int tenant)
        {
            return context.CustomChildObjects.Where(d => d.Tenant == tenant);
        }
    }
}
