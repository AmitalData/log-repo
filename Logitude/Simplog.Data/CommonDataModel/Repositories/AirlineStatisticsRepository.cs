using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Server.Infrastructure.Helpers;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class AirlineStatisticsRepository : IRepository<AirlineStatistics>
    {
        ICommonDataContext commonDataContext;

        public AirlineStatisticsRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }



        public AirlineStatisticsRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<AirlineStatistics> GetAirlineStatistics(int tenant)
        {
            return (from record in context.AirlineStatistics.Include("ShipmentLevel") where record.Tenant == tenant select record);
        }

        public AirlineStatistics GetSingleAirlineStatistics(int tenant, string id)
        {
            return (from record in context.AirlineStatistics.Include("ShipmentLevel") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public AirlineStatistics GetSingleAirlineStatistics(string id, int tenant )
        {
            return (from record in context.AirlineStatistics.Include("ShipmentLevel") where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }

        public AirlineStatistics GetSingleAirlineStatisticsByBookingId(string id)
        {
            return (from record in context.AirlineStatistics.Include("ShipmentLevel") where record.BookingId == id select record).FirstOrDefault();
        }

        public AirlineStatistics GetSingleAirlineStatisticsByShipmentId(string id)
        {
            return (from record in context.AirlineStatistics.Include("ShipmentLevel") where record.ShipmentId == id select record).FirstOrDefault();
        }

        public void Add(AirlineStatistics entity)
        {
            context.AirlineStatistics.Add(entity);
        }

        public void Remove(AirlineStatistics entity)
        {
            try
            {
                context.AirlineStatistics.Attach(entity);
            }
            catch { }
            context.AirlineStatistics.Remove(entity);
        }

        public void Update(AirlineStatistics entity)
        {
            try
            {
                context.AirlineStatistics.Attach(entity);
            }
            catch { }
            context.SetAsModified(entity);
        }

        public List<AirlineStatistics> All()
        {
            return context.AirlineStatistics.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<AirlineStatistics> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public AirlineStatistics GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
