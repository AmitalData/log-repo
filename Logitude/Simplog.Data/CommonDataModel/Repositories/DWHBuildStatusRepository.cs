using System.Collections.Generic;
using System.Linq;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class DWHBuildStatusRepository : IRepository<DWHBuildStatus>
    {
        ICommonDataContext commonDataContext;



        public DWHBuildStatusRepository(ICommonDataContext context)
        {
            commonDataContext = context;

        }

        public DWHBuildStatusRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public DWHBuildStatus GetSingleDWHBuildStatus()
        {
            return (from a in this.context.DWHBuildStatus
                    select a).FirstOrDefault();
        }

        public void Add(DWHBuildStatus entity)
        {
            this.context.DWHBuildStatus.Add(entity);
        }

        public void Remove(DWHBuildStatus entity)
        {

            this.context.DWHBuildStatus.Attach(entity);

            this.context.DWHBuildStatus.Remove(entity);
        }

        public void Update(DWHBuildStatus entity)
        {
            this.context.DWHBuildStatus.Attach(entity);

            this.context.SetAsModified(entity);
        }

        public List<DWHBuildStatus> All()
        {
            return this.context.DWHBuildStatus.ToList();
        }

        public ICommonDataContext context
        {
            get { return this.commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }

        public List<DWHBuildStatus> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public DWHBuildStatus GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}