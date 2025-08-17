using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.DomainServices.Server;
using System.Web;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure.Helpers;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class TenantAdditionalDataRepository : IRepository<TenantAdditionalData>
    {
        ICommonDataContext commonDataContext;

        public TenantAdditionalDataRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }



        public TenantAdditionalDataRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<TenantAdditionalData> GetTenantAdditionalDatas()
        {
            return this.context.TenantAdditionalDatas;
        }
         
        public TenantAdditionalData GetSingleTenantAdditionalData(int id)
        {
            TenantAdditionalData entity = (from a in context.TenantAdditionalDatas where a.Tenant == id select a).FirstOrDefault();                      
            return entity;
        }

        public TenantAdditionalData GetSingleTenantAdditionalDataByTenant(int tenant)
        {
            TenantAdditionalData entity = (from a in context.TenantAdditionalDatas where a.Tenant == tenant select a).FirstOrDefault();
            return entity;
        }

        public IQueryable<TenantAdditionalData> GetTenantAdditionalDatas(int tenant)
        {
            TenantAdditionalData entity = (from a in context.TenantAdditionalDatas where a.Tenant == tenant select a).FirstOrDefault();
            return this.context.TenantAdditionalDatas;
        }

        public TenantAdditionalData GetSingleTenantAdditionalData(int id , int tenant)
        {
            TenantAdditionalData entity = (from a in context.TenantAdditionalDatas where a.Tenant == tenant && a.Id== id select a).FirstOrDefault();
            return entity;
        }


        public TenantAdditionalData GetSingleTenantAdditionalDataByState(string state)
        {
            TenantAdditionalData entity = (from a in context.TenantAdditionalDatas where a.DropBoxState == state select a).FirstOrDefault();
            return entity;
        }

        public TenantAdditionalData GetSingleTenantAdditionalDataByGateWayPartnerCode(string Code)
        {
            TenantAdditionalData entity = (from a in context.TenantAdditionalDatas where a.PaymentGatewayPartnerCode == Code select a).FirstOrDefault();
            return entity;
        }

        public TenantAdditionalData GetSingleTenantAdditionalDataByUID(string UID)
        {
            TenantAdditionalData entity = (from a in context.TenantAdditionalDatas where a.DropBoxUID == UID select a).FirstOrDefault();
            return entity;
        }

        public void Add(TenantAdditionalData entity)
        {
            this.context.TenantAdditionalDatas.Add(entity);
        }

        public void Remove(TenantAdditionalData entity)
        {
            try
            {
                this.context.TenantAdditionalDatas.Attach(entity);
            }
            catch { }
            this.context.TenantAdditionalDatas.Remove(entity);
        }

        public void Update(TenantAdditionalData entity)
        {
            try
            {
                this.context.TenantAdditionalDatas.Attach(entity);
            }
            catch { }
            this.context.SetAsModified(entity);
        }

        public List<TenantAdditionalData> All()
        {
            return this.context.TenantAdditionalDatas.ToList<TenantAdditionalData>();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }


        public List<TenantAdditionalData> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public TenantAdditionalData GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public List<TenantAdditionalData>  GetTenantListByListIds(List<string> ids)
        {

            return (from d in this.context.TenantAdditionalDatas
                    where ids.Contains(d.Id.ToString())
                    select d).ToList();
      
        }
    }
}
