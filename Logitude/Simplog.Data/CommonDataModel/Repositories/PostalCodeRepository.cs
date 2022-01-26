using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class PostalCodeRepository : IRepository<PostalCode>
    {
        ICommonDataContext commonDataContext;

        public PostalCodeRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public PostalCodeRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public PostalCodeRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public IQueryable<PostalCode> GetPostalCodes()
        {
            return context.PostalCodes;
        }

        public IQueryable<PostalCode> GetAll()
        {
            return context.PostalCodes;
        }

        public PostalCode GetSinglePostalCode(string code)
        {
            return (from record in context.PostalCodes where record.Code == code select record).FirstOrDefault();
        }

        public PostalCode GetSinglePostalCodeUpdate(string code)
        {
            return (from record in context.PostalCodes where record.Code == code select record).FirstOrDefault();
        }

        public void Add(PostalCode entity)
        {
            context.PostalCodes.Add(entity);
        }

        public void Remove(PostalCode entity)
        {
            try
            {
                context.PostalCodes.Attach(entity);
            }
            catch { }
            context.PostalCodes.Remove(entity);
        }

        public void Update(PostalCode entity)
        {
            try
            {
                context.PostalCodes.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<PostalCode> All()
        {
            return context.PostalCodes.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<PostalCode> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public PostalCode GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}
