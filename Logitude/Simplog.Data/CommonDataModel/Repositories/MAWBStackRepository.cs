using System;
using System.Collections.Generic;
using System.Linq;

using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class MAWBStackRepository : IRepository<MAWBStack>
    {
        ICommonDataContext commonDataContext;

        public MAWBStackRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public MAWBStackRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public MAWBStackRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<MAWBStack> GetMAWBStacks(int tenant)
        {
            return (from record in context.MAWBStacks where record.Tenant == tenant select record);
        }

        public MAWBStack GetSingleMAWBStack(string id, int tenant)
        {
            return (from record in context.MAWBStacks where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public MAWBStack GetSingleMAWBStackByNumberAirline(long number, int tenant,string airlineId)
        {
            MAWBStack mAWBStack = (from a in context.MAWBStacks
                             where a.Tenant == tenant && a.Number == number && a.AirlineId == airlineId
                            select a).FirstOrDefault();
            return mAWBStack;
        }

        public List<MAWBStack> GetMAWBStacksByInsertionDate(int tenant, string airlineId,DateTime insertionDate)
        {
            List<MAWBStack> mAWBStacks = (from a in context.MAWBStacks
                                          where a.Tenant == tenant && a.InsertionDate == insertionDate && a.AirlineId == airlineId
                                          select a).ToList();
            return mAWBStacks;
        }

        public void Add(MAWBStack entity)
        {
            context.MAWBStacks.Add(entity);
        }

        public void Remove(MAWBStack entity)
        {
            try
            {
                context.MAWBStacks.Attach(entity);
            }
            catch { }
            context.MAWBStacks.Remove(entity);
        }

        public void Update(MAWBStack entity)
        {
            try
            {
                context.MAWBStacks.Attach(entity);
            }
            catch { }
            this.context.SetAsModified(entity);
        }

        public List<MAWBStack> All()
        {
            return context.MAWBStacks.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<MAWBStack> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public MAWBStack GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}