using System.Collections.Generic;
using System.Linq;
using Simplog.Server.Infrastructure;
using Simplog.Data.ShipmentsModel.EntityPOCOs;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class AWBChargesCodeRepository: IRepository<AWBChargesCode>
    {
        IShipmentsContext shipmentsContext;

        public AWBChargesCodeRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }

        public AWBChargesCodeRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public AWBChargesCode GetSingleAWBChargeCode(string code)
        {
            return (from a in context.AWBChargeCodes where a.Code == code select a).FirstOrDefault();
        }

        public AWBChargesCode GetSingleAWBChargesCode(string code)
        {
            return (from a in context.AWBChargeCodes where a.Code == code select a).FirstOrDefault();
        }



        public IQueryable<AWBChargesCode> GetAll()
        {
            return (from a in context.AWBChargeCodes select a);
        }

        public IQueryable<AWBChargesCode> GetAWBChargesCodes()
        {
            return (from a in context.AWBChargeCodes select a);
        }    
        public IQueryable<AWBChargesCode> GetAWBChargeCodes()
        {
            return (from a in context.AWBChargeCodes select a);
        }       

        public void Add(AWBChargesCode entity)
        {
            context.AWBChargeCodes.Add(entity);
        }

        public void Remove(AWBChargesCode entity)
        {
            context.AWBChargeCodes.Attach(entity);
            context.AWBChargeCodes.Remove(entity);
        }

        public void Update(AWBChargesCode entity)
        {
            context.AWBChargeCodes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AWBChargesCode> All()
        {
            return context.AWBChargeCodes.ToList();
        }

        public IShipmentsContext context
        {
            get {return  shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<AWBChargesCode> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public AWBChargesCode GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }
    }
}