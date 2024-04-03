using Simplog.Data.CommonDataModel.EntityPOCOs;
using Simplog.Server.Infrastructure;
using Simplog.Server.Infrastructure.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class HorseRepository : IRepository<Horse>
    {
        ICommonDataContext commonDataContext;

        public HorseRepository()
        {
            commonDataContext = new CommonDataContext();
        }

        public HorseRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public HorseRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<Horse> GetHorses(int tenant)
        {
            return (from record in context.Horses where record.Tenant == tenant select record);
        }

        public Horse GetSingleHorse(string id, int tenant)
        {
            return (from record in context.Horses.Include("CountryOfBirth").Include("CreatedByUser").Include("CreatedByUser.Contact").Include("UpdatedByUser").Include("UpdatedByUser.Contact").Include("HorseGender")
                    where record.Id == id && record.Tenant == tenant select record).FirstOrDefault();
        }
        
        public void Add(Horse entity)
        {
            this.context.Horses.Add(entity);
        }

        public void Remove(Horse entity)
        {
            try
            {
                this.context.Horses.Attach(entity);
            }
            catch { }
            this.context.Horses.Remove(entity);

        }

        public void Update(Horse entity)
        {
            try
            {
                this.context.Horses.Attach(entity);
            }
            catch { }
            this.context.SetAsModified(entity);

        }

        public List<Horse> All()
        {
            return this.context.Horses.ToList<Horse>();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            this.context.SaveChanges();
        }
        
        public List<Horse> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }

        public Horse GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new NotImplementedException();
        }
    }
}