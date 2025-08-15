using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CustomsInterfaceRepository : IRepository<CustomsInterface>
    {
        ICommonDataContext commonDataContext;



        public CustomsInterfaceRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public CustomsInterfaceRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<CustomsInterface> GetCustomsInterfaces()
        {
            return context.CustomsInterfaces;
        }

        public IQueryable<CustomsInterface> GetAll()
        {
            return context.CustomsInterfaces;
        }

        public CustomsInterface GetSingleCustomsInterface(string code)
        {
            return (from record in context.CustomsInterfaces where record.Code == code select record).FirstOrDefault();
        }

        public void Add(CustomsInterface entity)
        {
            context.CustomsInterfaces.Add(entity);
        }

        public void Remove(CustomsInterface entity)
        {
            context.CustomsInterfaces.Attach(entity);
            context.CustomsInterfaces.Remove(entity);
        }

        public void Update(CustomsInterface entity)
        {
            context.CustomsInterfaces.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CustomsInterface> All()
        {
            return context.CustomsInterfaces.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }
        
        public List<CustomsInterface> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public CustomsInterface GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}