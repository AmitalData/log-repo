using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class CheckDigitControlAlgorithmRepository: IRepository<CheckDigitControlAlgorithm>
    {
        ICommonDataContext Context;
        public CheckDigitControlAlgorithmRepository()
        {
            Context = new CommonDataContext();

        }
        public CheckDigitControlAlgorithmRepository(ICommonDataContext context)
        {
            Context = context;
        }
        public CheckDigitControlAlgorithmRepository(int tenant)
        {
            Context = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<CheckDigitControlAlgorithm> GetCheckDigitControlAlgorithms()
        {
            return context.CheckDigitControlAlgorithms;
        }

        public CheckDigitControlAlgorithm GetSingleCheckDigitControlAlgorithm(string code)
        {
            return (from a in context.CheckDigitControlAlgorithms
                    where a.Code == code
                    select a).FirstOrDefault();
        }

        public IQueryable<CheckDigitControlAlgorithm> GetAll()
        {
            return from a in context.CheckDigitControlAlgorithms
                   select a;
        }

        public void Add(CheckDigitControlAlgorithm entity)
        {
            context.CheckDigitControlAlgorithms.Add(entity);
        }

        public void Remove(CheckDigitControlAlgorithm entity)
        {
            context.CheckDigitControlAlgorithms.Remove(entity);
        }

        public void Update(CheckDigitControlAlgorithm entity)
        {
            context.CheckDigitControlAlgorithms.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<CheckDigitControlAlgorithm> All()
        {
            return context.CheckDigitControlAlgorithms.ToList();
        }

        public ICommonDataContext context
        {
            get { return Context; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<CheckDigitControlAlgorithm> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public CheckDigitControlAlgorithm GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
