using System;
using System.Collections.Generic;
using System.Linq;

using Simplog.Data.ShipmentsModel.EntityPOCOs;
using Simplog.Server.Infrastructure;

namespace Simplog.Data.ShipmentsModel.Repositories
{
    public class AWBSpecialHandlingCodeRepository: IRepository<AWBSpecialHandlingCode>
    {
        IShipmentsContext shipmentsContext;

        public AWBSpecialHandlingCodeRepository(int tenant)
        {
            shipmentsContext = ShipmentsContext.GetContext(tenant);
        }

        public AWBSpecialHandlingCodeRepository(IShipmentsContext context)
        {
            shipmentsContext = context;
        }

        public AWBSpecialHandlingCode GetSingleAWBSpecialHandlingCode(string id)
        {
            return (from a in context.AWBHandlingCodes where a.Id == id select a).FirstOrDefault();
        }
        
        public AWBSpecialHandlingCode GetSingleAWBHandlingCode(string id)
        {
            return (from a in context.AWBHandlingCodes where a.Id == id select a).FirstOrDefault();
        }
        public AWBSpecialHandlingCode GetSingleAWBHandlingCodeByCode(string code)
        {
            return (from a in context.AWBHandlingCodes where a.Code == code select a).FirstOrDefault();
        }

        public IQueryable<AWBSpecialHandlingCode> GetAWBHandlingCodes()
        {
            return (from a in context.AWBHandlingCodes select a);
        }

        public IQueryable<AWBSpecialHandlingCode> GetAWBSpecialHandlingCodes()
        {
            return (from a in context.AWBHandlingCodes select a);
        }
      
        

        public void Add(AWBSpecialHandlingCode entity)
        {
            context.AWBHandlingCodes.Add(entity);
        }

        public void Remove(AWBSpecialHandlingCode entity)
        {
            context.AWBHandlingCodes.Attach(entity);
            context.AWBHandlingCodes.Remove(entity);
        }

        public void Update(AWBSpecialHandlingCode entity)
        {
            context.AWBHandlingCodes.Attach(entity);
            context.SetAsModified(entity);
        }

        public List<AWBSpecialHandlingCode> All()
        {
            return context.AWBHandlingCodes.ToList();
        }

        public IShipmentsContext context
        {
            get {return  shipmentsContext; }
        }

        public void SubmitChanges()
        {
            context.SaveChanges();
        }

        public List<AWBSpecialHandlingCode> GetMulti(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public AWBSpecialHandlingCode GetSingle(Simplog.Server.Infrastructure.EntityKeyFields entityKeys)
        {
            throw new System.NotImplementedException();
        }

        public IQueryable<AWBSpecialHandlingCode> GetHandlingCodesByAirlineId(string airlineId)
        {
            
                return context.AWBHandlingCodes.Where(a => a.AirlineId == airlineId);
            
        }

        public List<string> GetAWBHandlingCodesByIds(List<string> specialHandlingIds)
        {
            List<string> specialHandlingCodes = new List<string>();
            if (specialHandlingIds != null && specialHandlingIds.Count() > 0)
            {
                specialHandlingCodes = (from a in context.AWBHandlingCodes
                                        where specialHandlingIds.Contains(a.Id)
                                        select a.Code).ToList();
            }
            return specialHandlingCodes;
        }
    }
}