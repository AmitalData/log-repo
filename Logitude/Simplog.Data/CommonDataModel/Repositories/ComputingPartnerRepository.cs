using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class ComputingPartnerRepository : IRepository<ComputingPartner>
    {
        ICommonDataContext context;
        public ICommonDataContext Context
        {
            get { return context; }
        }

        public ComputingPartnerRepository()
        {
            this.context = new CommonDataContext();
        }

        public ComputingPartnerRepository(int tenant)
        {
            this.context = CommonDataContext.GetContext(tenant);
        }

        public ComputingPartnerRepository(ICommonDataContext context)
        {
            this.context = context;
        }

        public IQueryable<ComputingPartner> GetComputingPartners(int tenant)
        {
            return (from d in Context.ComputingPartners where d.Tenant == tenant || d.Tenant == 0 && (d.Tenant == 0 && tenant != 0 ? d.InActive == false ? true : false : true) select d);
        }

        public ComputingPartner GetSingleComputingPartner(string id,int tenant)
        {
            return (from d in Context.ComputingPartners.Include("CreatedByUser").Include("UpdatedByUser").Include("CreatedByUser.Contact").Include("UpdatedByUser.Contact")
                    where d.Id == id && (d.Tenant == tenant || d.Tenant == 0) && (d.Tenant == 0 && tenant != 0 ? d.InActive == false ? true : false : true)
                    select d).FirstOrDefault();
        }

        public List<ComputingPartner> All()
        {
            return Context.ComputingPartners.ToList();
        }

        public List<ComputingPartner> GetMulti(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ComputingPartner GetSingle(EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public ComputingPartner GetSingleComputingPartnerByCode(string code)
        {
            return (from d in Context.ComputingPartners
                    where d.Code == code
                    select d).FirstOrDefault();
        }

        public ComputingPartner GetSingleComputingPartnerByCode(string code, int tenant)
        {
            return (from d in Context.ComputingPartners
                    where d.Code == code && d.Tenant == tenant
                    select d).FirstOrDefault();
        }

        public string GetSingleComputingPartnerCodeById(string id)
        {
            return (from d in Context.ComputingPartners
                    where d.Id == id
                    select d.Code).FirstOrDefault();
        }


        public void Add(ComputingPartner entity)
        {
            Context.ComputingPartners.Add(entity);
        }

        public void Remove(ComputingPartner entity)
        {
            Context.ComputingPartners.Attach(entity);
            Context.ComputingPartners.Remove(entity);
        }

        public void Update(ComputingPartner entity)
        {
            Context.ComputingPartners.Attach(entity);
            Context.SetAsModified(entity);
        }

        public void SubmitChanges()
        {
            Context.SaveChanges();
        }
    }
}
