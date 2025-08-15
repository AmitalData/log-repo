using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Simplog.Data.CommonDataModel.EntityPOCOs; using Simplog.Global.Data.GlobalModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.CommonDataModel.Repositories
{
    public class HorseGenderRepository : IRepository<HorseGender>
    {
        ICommonDataContext commonDataContext;



        public HorseGenderRepository(ICommonDataContext context)
        {
            commonDataContext = context;
        }

        public HorseGenderRepository(int tenant)
        {
            commonDataContext = CommonDataContext.GetContext(tenant);
        }

        public IQueryable<HorseGender> GetHorseGenders()
        {
            return context.HorseGenders;
        }

        public IQueryable<HorseGender> GetAll()
        {
            return context.HorseGenders;
        }

        public HorseGender GetSingleHorseGender(string code)
        {
            return (from record in context.HorseGenders where record.Code == code select record).FirstOrDefault();
        }

        public void Add(HorseGender entity)
        {
            context.HorseGenders.Add(entity);
        }

        public void Remove(HorseGender entity)
        {
            context.HorseGenders.Attach(entity);
            context.HorseGenders.Remove(entity);
        }

        public void Update(HorseGender entity)
        {
            context.HorseGenders.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<HorseGender> All()
        {
            return context.HorseGenders.ToList();
        }

        public ICommonDataContext context
        {
            get { return commonDataContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }


        public List<HorseGender> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public HorseGender GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}
