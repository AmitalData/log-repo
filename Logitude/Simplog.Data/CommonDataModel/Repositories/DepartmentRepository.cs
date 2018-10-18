using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class DepartmentRepository:IRepository<Department>
    {
        ICommonDataContext commonDataContext;

        public DepartmentRepository()
        {
            commonDataContext = new CommonDataContext();
        }

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

        public Department GetSingleDepartment(string id, int tenant)
        {
            return (from record in context.Departments where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public Department GetSingleDepartmentByCode(string code, int tenant)
        {
            return (from record in context.Departments where record.Code == code && record.Tenant == tenant select record).FirstOrDefault();
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