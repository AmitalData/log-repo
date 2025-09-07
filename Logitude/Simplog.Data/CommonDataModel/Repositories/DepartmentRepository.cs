using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class DepartmentRepository : IRepository<Department>
    {
        ICommonDataContext commonDataContext;



        public DepartmentRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public DepartmentRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public IQueryable<Department> GetDepartments(int tenant)
        {
            return (from record in context.Departments where record.Tenant == tenant select record);
        }
        public Department GetSingleDepartmentCache(string id, int tenant)
        {
            string entityKeyString = $"GetSingleDepartment({id},{tenant})";
            var res = CacheManager.GetOrInsertNewObject<Department>(entityKeyString, () =>
            {
                return this.GetSingleDepartment(id, tenant);
            });
            return res;

        }
        public Department GetSingleDepartment(string id, int tenant)
        {
            return (from record in context.Departments where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public Department GetSingleDepartmentByCode(string code, int tenant, bool getFromCache = false)
        {
            if (!string.IsNullOrEmpty(code))
            {
                Department entity;
                if (getFromCache)
                {
                    string entityKeyString = $"GetSingleDepartment({code},{tenant})";
                    if (CacheManager.CacheWrapper.Get(entityKeyString) == null)
                    {

                        entity = (from record in context.Departments where record.Code == code && record.Tenant == tenant select record).FirstOrDefault();

                        if (CacheManager.CacheWrapper.Get(entityKeyString) == null && entity != null)
                        {
                            CacheManager.CacheWrapper.Insert(entityKeyString, entity, null, System.DateTime.UtcNow.AddMinutes(30), TimeSpan.Zero);
                        }
                    }
                    else
                    {
                        entity = (Department)CacheManager.CacheWrapper.Get(entityKeyString);
                    }

                }
                else
                {
                    entity = (from record in context.Departments where record.Code == code && record.Tenant == tenant select record).FirstOrDefault();
                }
                return entity;
            }
            return null;
        }

        public Department GetDepartmentByName(string name, int tenant)
        {
            string nameNew = "";
            nameNew = name;

            var query = (from a in context.Departments
                         where a.Tenant == tenant && a.EnglishName.ToLower() == name.ToLower()
                         select a).FirstOrDefault();

            return query;
        }

        public void Add(Department entity)
        {
            this.context.Departments.Add(entity);
        }

        public void Remove(Department entity)
        {
            try
            {
                this.context.Departments.Attach(entity);
            }
            catch { }
            this.context.Departments.Remove(entity);
        }

        public void Update(Department entity)
        {
            try
            {
                this.context.Departments.Attach(entity);
            }
            catch { }
            this.context.SetAsModified(entity);
        }

        public List<Department> All()
        {
            return this.context.Departments.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }

        public List<Department> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public Department GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}