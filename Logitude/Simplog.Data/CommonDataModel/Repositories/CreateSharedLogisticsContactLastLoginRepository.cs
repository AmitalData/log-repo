using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class SharedLogisticsContactLastLoginRepository : IRepository<SharedLogisticsContactLastLogin>
    {
        ICommonDataContext commonDataContext;

  

        public SharedLogisticsContactLastLoginRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public SharedLogisticsContactLastLoginRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public IQueryable<SharedLogisticsContactLastLogin> GetSharedLogisticsContactLastLogins(int tenant)
        {
            return (from record in context.SharedLogisticsContactLastLogins where record.Tenant == tenant select record);
        }

        public SharedLogisticsContactLastLogin GetSingleSharedLogisticsContactLastLogin(string contactid,string  cardid,string partnertypeid,string via, int tenant)
        {
            return (from record in context.SharedLogisticsContactLastLogins where record.ContactId == contactid && record.CardId == cardid
                     && record.PartnerTypeId == partnertypeid && record.Via == via && record.Tenant == tenant select record).FirstOrDefault();
        }

       

        public void Add(SharedLogisticsContactLastLogin entity)
        {
            this.context.SharedLogisticsContactLastLogins.Add(entity);
        }

        public void Remove(SharedLogisticsContactLastLogin entity)
        {
            try
            {
                this.context.SharedLogisticsContactLastLogins.Attach(entity);
            }
            catch { }
            this.context.SharedLogisticsContactLastLogins.Remove(entity);
        }

        public void Update(SharedLogisticsContactLastLogin entity)
        {
            try
            {
                this.context.SharedLogisticsContactLastLogins.Attach(entity);
            }
            catch { }
            this.context.SetAsModified(entity);
        }

        public List<SharedLogisticsContactLastLogin> All()
        {
            return this.context.SharedLogisticsContactLastLogins.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }

        public List<SharedLogisticsContactLastLogin> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public SharedLogisticsContactLastLogin GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}